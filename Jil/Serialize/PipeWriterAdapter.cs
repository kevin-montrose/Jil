#if BUFFER_AND_SEQUENCE

using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;

namespace Jil.Serialize
{
    internal sealed class PipeWriterAdapter: IBufferWriter<char>
    {
        private int _BytesWritten;
        public int BytesWritten => _BytesWritten; 

        private readonly PipeWriter Writer;
        private readonly Encoder Encoder;
        private readonly int MinimumByteBufferSize;

        private IMemoryOwner<char> Current;

        public PipeWriterAdapter(PipeWriter writer, Encoding enc)
        {
            Writer = writer;
            Encoder = enc.GetEncoder();
            MinimumByteBufferSize = enc.GetMaxByteCount(1);
            Current = null;
        }

        public void Advance(int count)
        {
            var toWrite = Current.Memory.Span.Slice(0, count);

            ConvertAndWrite(toWrite, false);
        }

        public Memory<char> GetMemory(int sizeHint = 0)
        {
            var neededSize = sizeHint;
            if (neededSize == 0) neededSize = -1;

            if(Current == null)
            {
                Current = MemoryPool<char>.Shared.Rent(neededSize);
            }

            if(Current.Memory.Length < neededSize)
            {
                Current.Dispose();
                Current = MemoryPool<char>.Shared.Rent(neededSize);
            }

            return Current.Memory;
        }

        public Span<char> GetSpan(int sizeHint = 0)
        => GetMemory(sizeHint).Span;

        // write anything pending out to the writer
        public void Complete()
        {
            ConvertAndWrite(ReadOnlySpan<char>.Empty, true);
        }

        private void ConvertAndWrite(ReadOnlySpan<char> chars, bool flush)
        {
            var completed = false;

            while (chars.Length > 0 || flush && !completed)
            {
                var toWriteTo = Writer.GetSpan(MinimumByteBufferSize);

                Encoder.Convert(chars, toWriteTo, flush, out var charsUsed, out var bytesUsed, out completed);

                chars = chars.Slice(charsUsed);
                Writer.Advance(bytesUsed);

                _BytesWritten += bytesUsed;
            }
        }
    }
}

#endif