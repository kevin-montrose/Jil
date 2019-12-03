#if BUFFER_AND_SEQUENCE

using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Text;

namespace Jil.Deserialize
{
    internal sealed class PipeReaderAdapter: TextReader
    {
        private readonly PipeReader Reader;
        private readonly Decoder Decoder;
        private readonly int MinimumCharBufferSize;

        private bool IsCompleted;
        private SequencePosition? CurrentSequenceEnd;
        private ReadOnlySequence<byte>.Enumerator CurrentSequence;
        private ReadOnlyMemory<byte> CurrentSequenceChunk;

        private IMemoryOwner<char> ConvertedOwner;
        private Memory<char> Converted;

        public PipeReaderAdapter(PipeReader reader, Encoding encoding)
        {
            Reader = reader;
            Decoder = encoding.GetDecoder();
            MinimumCharBufferSize = encoding.GetMaxCharCount(1);

            IsCompleted = false;
            CurrentSequenceEnd = null;
            CurrentSequence = default;
            CurrentSequenceChunk = ReadOnlyMemory<byte>.Empty;

            ConvertedOwner = MemoryPool<char>.Shared.Rent(MinimumCharBufferSize);
            Converted = Memory<char>.Empty;
        }

        public override int Peek()
        {
            if(Converted.IsEmpty)
            {
                TryAdvance();

                if (Converted.IsEmpty)
                {
                    return -1;
                }
            }

            return Converted.Span[0];
        }

        public override int Read()
        {
            if (Converted.IsEmpty)
            {
                TryAdvance();

                if (Converted.IsEmpty)
                {
                    return -1;
                }
            }

            var ret = Converted.Span[0];
            Converted = Converted.Slice(1);

            return ret;
        }

        private void TryAdvance()
        {
            if (IsCompleted) return;

tryAgain:

            // do we have anything pending to convert?
            if (CurrentSequenceChunk.IsEmpty)
            {
                // try and move forward in the sequence
                if (CurrentSequence.MoveNext())
                {
                    CurrentSequenceChunk = CurrentSequence.Current;
                    goto tryAgain;
                }
                else
                {
                    if (CurrentSequenceEnd != null)
                    {
                        // we handled a previous chunk, release it
                        Reader.AdvanceTo(CurrentSequenceEnd.Value);
                        CurrentSequenceEnd = null;
                    }

                    // need to advance
                    while (true)
                    {
                        if (Reader.TryRead(out var res))
                        {
                            var buff = res.Buffer;
                            if(res.IsCanceled || (buff.IsEmpty && res.IsCompleted))
                            {
                                // we're done, bail and don't try again
                                IsCompleted = true;

                                // flush state
                                var completed = false;
                                while (!completed)
                                {
                                    checkSpace:
                                    var into = ConvertedOwner.Memory.Span.Slice(Converted.Length);

                                    // need to grow the buffer
                                    if (into.IsEmpty)
                                    {
                                        var newMem = MemoryPool<char>.Shared.Rent(ConvertedOwner.Memory.Length * 2);
                                        ConvertedOwner.Memory.CopyTo(newMem.Memory);
                                        Converted = newMem.Memory.Slice(0, Converted.Length);
                                        ConvertedOwner.Dispose();
                                        ConvertedOwner = newMem;

                                        goto checkSpace;
                                    }

                                    // flush the decoder
                                    Decoder.Convert(ReadOnlySpan<byte>.Empty, into, true, out _, out var chars, out completed);
                                    
                                    // update Converted to be the correct length, given what we decoded
                                    var newLen = Converted.Length + chars;
                                    Converted = ConvertedOwner.Memory.Slice(0, newLen);
                                }

                                return;
                            }

                            CurrentSequence = buff.GetEnumerator();
                            CurrentSequenceEnd = buff.End;
                            goto tryAgain;
                        }
                    }
                }
            }

            // need to use _the whole_ span, Converted gets cut down as we advance through it
            var intoSpan = ConvertedOwner.Memory.Span;

            // map some bytes from the CurrentSequenceChunk into chars
            Decoder.Convert(CurrentSequenceChunk.Span, intoSpan, false, out var bytesUsed, out var charsCreated, out _);

            // update pending chunk and converted to represent just unhandled data
            CurrentSequenceChunk = CurrentSequenceChunk.Slice(bytesUsed);
            Converted = ConvertedOwner.Memory.Slice(0, charsCreated);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ConvertedOwner?.Dispose();
                ConvertedOwner = null;
            }

            base.Dispose(disposing);
        }
    }
}

#endif