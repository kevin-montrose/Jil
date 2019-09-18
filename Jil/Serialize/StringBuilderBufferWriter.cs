#if BUFFER_AND_SEQUENCE
using System;
using System.Buffers;
using System.Text;

namespace Jil.Serialize
{
    internal sealed class StringBuilderBufferWriter : IBufferWriter<char>, IDisposable
    {
        private StringBuilder Builder;
        private IMemoryOwner<char> Scratch;
        private Memory<char> ScratchMemory => Scratch.Memory;

        public StringBuilderBufferWriter()
        {
            Builder = new StringBuilder();
            Scratch = MemoryPool<char>.Shared.Rent();
        }

        public void Advance(int count)
        {
            Builder.Append(ScratchMemory.Slice(0, count));
        }

        public Memory<char> GetMemory(int sizeHint = 0)
        {
            if(sizeHint == 0 || sizeHint < ScratchMemory.Length)
            {
                return ScratchMemory;
            }

            int rentSize;
            if(sizeHint == 0)
            {
                rentSize = -1;
            }
            else
            {
                rentSize = sizeHint;
            }

            Scratch.Dispose();
            Scratch = MemoryPool<char>.Shared.Rent(rentSize);

            return ScratchMemory;
        }

        public Span<char> GetSpan(int sizeHint = 0)
        => GetMemory(sizeHint).Span;

        public string GetString()
        {
            return Builder.ToString();
        }

        public void Dispose()
        {
            Scratch.Dispose();
        }
    }
}
#endif