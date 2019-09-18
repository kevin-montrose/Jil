#if BUFFER_AND_SEQUENCE
using System;
using System.Buffers;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jil.SerializeDynamic
{
    internal ref partial struct WriterProxy
    {
        #region proxy for crossing the line between Dynamic and Static serialization, static their wants a TextWriter so proxy accordingly
        private class StubTextWriter: TextWriter
        {
            public override Encoding Encoding => throw new NotImplementedException();

            private IBufferWriter<char> Inner;
            private Memory<char> CurrentMemory;
            private Span<char> Current => CurrentMemory.Span;
            private int Start;

            public StubTextWriter(IBufferWriter<char> inner)
            {
                Inner = inner;
                CurrentMemory = inner.GetMemory();
                Start = 0;
            }

            private void AdvanceAndAcquire()
            {
                var toFlush = Start;
                if (toFlush == 0)
                {
                    // implies what we were given was too small
                    var requestLength = Current.Length * 2;
                    Inner.Advance(0);
                    CurrentMemory = Inner.GetMemory(requestLength);
                }
                else
                {
                    Inner.Advance(toFlush);
                    CurrentMemory = Inner.GetMemory();
                }

                Start = 0;
            }

            public override void Write(char c)
            {
                if (Start == Current.Length)
                {
                    AdvanceAndAcquire();
                }

                Current[Start] = c;
                Start++;
            }

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

            public override void Write(char[] buffer)
            {
                var asSpan = buffer.AsSpan();
                WriteSpan(asSpan);
            }

            public override void Write(char[] buffer, int index, int count)
            {
                var asSpan = buffer.AsSpan().Slice(index, count);
                WriteSpan(asSpan);
            }

            public override void Write(decimal value)
            {
                tryAgain:
                if(!value.TryFormat(Current.Slice(Start), out var written, provider: CultureInfo.InvariantCulture))
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                Start += written;
            }

            public override void Write(double value)
            {
                tryAgain:
                if (!value.TryFormat(Current.Slice(Start), out var written, provider: CultureInfo.InvariantCulture))
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                Start += written;
            }

            public override void Write(float value)
            {
                tryAgain:
                if (!value.TryFormat(Current.Slice(Start), out var written, provider: CultureInfo.InvariantCulture))
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                Start += written;
            }

            public override void Write(int value)
            {
                tryAgain:
                if (!value.TryFormat(Current.Slice(Start), out var written, provider: CultureInfo.InvariantCulture))
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                Start += written;
            }

            public override void Write(long value)
            {
                tryAgain:
                if (!value.TryFormat(Current.Slice(Start), out var written, provider: CultureInfo.InvariantCulture))
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                Start += written;
            }

            public override void Write(string value)
            {
                WriteSpan(value.AsSpan());
            }

            public override void Write(uint value)
            {
            tryAgain:
                if (!value.TryFormat(Current.Slice(Start), out var written, provider: CultureInfo.InvariantCulture))
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                Start += written;
            }

            public override void Write(ulong value)
            {
            tryAgain:
                if (!value.TryFormat(Current.Slice(Start), out var written, provider: CultureInfo.InvariantCulture))
                {
                    AdvanceAndAcquire();
                    goto tryAgain;
                }

                Start += written;
            }

            public void End()
            {
                Inner.Advance(Start);
                Inner = null;

                CurrentMemory = Memory<char>.Empty;
                Start = 0;
            }

            public override void Close() => throw new NotImplementedException();
            public override void Flush() => throw new NotImplementedException();
            public override Task FlushAsync() => throw new NotImplementedException();
            public override void Write(bool value) => throw new NotImplementedException();
            public override void Write(object value) => throw new NotImplementedException();
            public override void Write(ReadOnlySpan<char> buffer) => throw new NotImplementedException();
            public override void Write(string format, object arg0) => throw new NotImplementedException();
            public override void Write(string format, object arg0, object arg1) => throw new NotImplementedException();
            public override void Write(string format, object arg0, object arg1, object arg2) => throw new NotImplementedException();
            public override void Write(string format, params object[] arg) => throw new NotImplementedException();
            public override Task WriteAsync(char value) => throw new NotImplementedException();
            public override Task WriteAsync(char[] buffer, int index, int count) => throw new NotImplementedException();
            public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public override Task WriteAsync(string value) => throw new NotImplementedException();
            public override void WriteLine() => throw new NotImplementedException();
            public override void WriteLine(bool value) => throw new NotImplementedException();
            public override void WriteLine(char value) => throw new NotImplementedException();
            public override void WriteLine(char[] buffer) => throw new NotImplementedException();
            public override void WriteLine(char[] buffer, int index, int count) => throw new NotImplementedException();
            public override void WriteLine(decimal value) => throw new NotImplementedException();
            public override void WriteLine(double value) => throw new NotImplementedException();
            public override void WriteLine(float value) => throw new NotImplementedException();
            public override void WriteLine(int value) => throw new NotImplementedException();
            public override void WriteLine(long value) => throw new NotImplementedException();
            public override void WriteLine(object value) => throw new NotImplementedException();
            public override void WriteLine(ReadOnlySpan<char> buffer) => throw new NotImplementedException();
            public override void WriteLine(string format, object arg0) => throw new NotImplementedException();
            public override void WriteLine(string format, object arg0, object arg1) => throw new NotImplementedException();
            public override void WriteLine(string format, object arg0, object arg1, object arg2) => throw new NotImplementedException();
            public override void WriteLine(string format, params object[] arg) => throw new NotImplementedException();
            public override void WriteLine(string value) => throw new NotImplementedException();
            public override void WriteLine(uint value) => throw new NotImplementedException();
            public override void WriteLine(ulong value) => throw new NotImplementedException();
            public override Task WriteLineAsync(char value) => throw new NotImplementedException();
            public override Task WriteLineAsync() => throw new NotImplementedException();
            public override Task WriteLineAsync(char[] buffer, int index, int count) => throw new NotImplementedException();
            public override Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public override Task WriteLineAsync(string value) => throw new NotImplementedException();
        }
        #endregion

        private TextWriter Writer;

        private IBufferWriter<char> Inner;
        private Span<char> Current;
        private int Start;
        private StubTextWriter Stub;

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
        public void InitWithWriter(TextWriter inner)
        {
            Writer = inner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char c)
        {
            if(Writer != null)
            {
                Writer.Write(c);
                return;
            }

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
            if(Writer != null)
            {
                Writer.Write(str);
                return;
            }

            var asSpan = str.AsSpan();
            WriteSpan(asSpan);
        }

        public TextWriter AsWriter()
        {
            if(Writer != null)
            {
                return Writer;
            }

            // force the text writer to finish
            if (Start > 0)
            {
                Inner.Advance(Start);
            }
            Start = 0;
            Current = Span<char>.Empty;

            Stub = new StubTextWriter(Inner);
            return Stub;
        }

        public void DoneWithWriter()
        {
            if(Writer != null)
            {
                return;
            }

            Stub.End();
            Stub = null;
        }

        public void End()
        {
            if(Writer != null)
            {
                Writer = null;
                return;
            }

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