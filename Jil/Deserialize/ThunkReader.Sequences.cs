#if BUFFER_AND_SEQUENCE
using System;
using System.Buffers;

namespace Jil.Deserialize
{
    internal ref partial struct ThunkReader
    {
        public int Position;

        private ReadOnlySequence<char>.Enumerator Inner;
        private ReadOnlySpan<char> Current;
        private int Index;

        public ThunkReader(ReadOnlySequence<char> sequence)
        {
            Inner = sequence.GetEnumerator();
            Current = ReadOnlySpan<char>.Empty;
            Index = 0;
            Position = 0;
        }

        private bool TryAdvance()
        {
            if (!Inner.MoveNext())
            {
                return false;
            }

            Current = Inner.Current.Span;
            Index = 0;
            return true;
        }

        public int Peek()
        {
            if(Index == Current.Length)
            {
                if (!TryAdvance())
                {
                    return -1;
                }
            }

            return Current[Index];
        }

        public int Read()
        {
            if(Index == Current.Length)
            {
                if(!TryAdvance())
                {
                    return -1;
                }
            }

            Position++;

            var ret = Current[Index];
            Index++;
            return ret;
        }
    }
}
#endif