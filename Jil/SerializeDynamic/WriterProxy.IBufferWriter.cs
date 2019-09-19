#if BUFFER_AND_SEQUENCE
using Jil.Serialize;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Jil.SerializeDynamic
{
    // todo: try and just replace WriterProxy with ThunkWriter?

    internal ref partial struct WriterProxy
    {
        private IBufferWriter<char> Inner;
        private Span<char> Current;
        private int Start;

        private ThunkWriter Stub;

        private void AdvanceAndAcquire()
        {
            var toFlush = Start;
            if (toFlush == 0)
            {
                // implies what we were given was too small
                var requestLength = Current.Length * 2;
                Inner.Advance(0);
                Current = Inner.GetSpan(requestLength);
            }
            else
            {
                Inner.Advance(toFlush);
                Current = Inner.GetSpan();
            }

            Start = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpan(ReadOnlySpan<char> toCopy)
        {
            while (!toCopy.IsEmpty)
            {
                tryAgain:
                var available = Current.Length - Start;
                if (available == 0)
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                var copyLen = Math.Min(available, toCopy.Length);
                var subset = toCopy.Slice(0, copyLen);

                subset.CopyTo(Current.Slice(Start));
                toCopy = toCopy.Slice(copyLen);

                Start += copyLen;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(IBufferWriter<char> inner)
        {
            Inner = inner;
            Current = Span<char>.Empty;
            Start = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char c)
        {
            if (Start == Current.Length)
            {
                AdvanceAndAcquire();
            }

            Current[Start] = c;
            Start++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(string str)
        {
            var asSpan = str.AsSpan();
            WriteSpan(asSpan);
        }

        public ThunkWriter AsThunkWriter()
        {
            if(Start > 0)
            {
                Inner.Advance(Start);
                Start = 0;
                Current = Span<char>.Empty;
            }

            Stub = new ThunkWriter();
            Stub.Init(Inner);

            return Stub;
        }

        public void DoneWithThunkWriter()
        {
            SerializeDynamic.WriterProxy _ = default;

            if(Start > 0)
            {
                Inner.Advance(Start);
                Start = 0;
                Current = Span<char>.Empty;
            }

            Stub.End(ref _);
        }

        public void End()
        {
            if (Start > 0)
            {
                Inner.Advance(Start);
            }

            Current = Span<char>.Empty;
            Start = 0;
            Inner = null;
        }
    }
}
#endif