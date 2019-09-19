#if BUFFER_AND_SEQUENCE
using System;
using System.Buffers;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Jil.Serialize
{
    internal ref partial struct ThunkWriter
    {
        public bool HasValue => Builder != null;

        IBufferWriter<char> Builder;
        Span<char> Current;
        int Start;

        ReadOnlySpan<char> ConstantString_Common_Chars_Span;
        ReadOnlySpan<char> ConstantString_Formatting_Chars_Span;
        ReadOnlySpan<char> ConstantString_Min_Chars_Span;
        ReadOnlySpan<char> ConstantString_Value_Chars_Span;
        ReadOnlySpan<char> Escape000Prefix_Span;
        ReadOnlySpan<char> Escape001Prefix_Span;
        ReadOnlySpan<char> ConstantString_DaysOfWeek_Span;

        private void AdvanceAndAcquire()
        {
            var toFlush = Start;
            if(toFlush == 0)
            {
                // implies what we were given was too small
                var requestLength = Current.Length * 2;
                Current = Builder.GetSpan(requestLength);
            }
            else
            {
                Builder.Advance(toFlush);
                Current = Builder.GetSpan();
            }

            Start = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(IBufferWriter<char> buffer)
        {
            Builder = buffer;
            Start = 0;
            Current = Span<char>.Empty;

            ConstantString_Common_Chars_Span = ThunkWriterCharArrays.ConstantString_Common_Chars.AsSpan();
            ConstantString_Formatting_Chars_Span = ThunkWriterCharArrays.ConstantString_Formatting_Chars.AsSpan();
            ConstantString_Min_Chars_Span = ThunkWriterCharArrays.ConstantString_Min_Chars.AsSpan();
            ConstantString_Value_Chars_Span = ThunkWriterCharArrays.ConstantString_Value_Chars.AsSpan();
            Escape000Prefix_Span = ThunkWriterCharArrays.Escape000Prefix.AsSpan();
            Escape001Prefix_Span = ThunkWriterCharArrays.Escape001Prefix.AsSpan();
            ConstantString_DaysOfWeek_Span = ThunkWriterCharArrays.ConstantString_DaysOfWeek.AsSpan();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(float f)
        {
            tryAgain:
            if(!f.TryFormat(Current.Slice(Start), out var chars, provider: CultureInfo.InvariantCulture))
            {
                AdvanceAndAcquire();
                goto tryAgain;
            }

            Start += chars;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(double d)
        {
            tryAgain:
            if (!d.TryFormat(Current.Slice(Start), out var chars, provider: CultureInfo.InvariantCulture))
            {
                AdvanceAndAcquire();
                goto tryAgain;
            }

            Start += chars;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(decimal m)
        {
            tryAgain:
            if (!m.TryFormat(Current.Slice(Start), out var chars, provider: CultureInfo.InvariantCulture))
            {
                AdvanceAndAcquire();
                goto tryAgain;
            }

            Start += chars;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char[] ch, int startIx, int len)
        {
            var toCopy = ch.AsSpan().Slice(startIx, len);

            WriteSpan(toCopy);
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
        public void Write(char ch)
        {
            if(Start == Current.Length)
            {
                AdvanceAndAcquire();
            }

            Current[Start] = ch;
            Start++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteCommonConstant(ConstantString_Common str)
        {
            var asUShort = (ushort)str;
            var ix = asUShort >> 8;
            var len = asUShort & 0xFF;

            var subset = ConstantString_Common_Chars_Span.Slice(ix, len);

            WriteSpan(subset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFormattingConstant(ConstantString_Formatting str)
        {
            var asUShort = (ushort)str;
            var ix = (asUShort >> 8);
            var len = asUShort & 0xFF;

            var subset = ConstantString_Formatting_Chars_Span.Slice(ix, len);

            WriteSpan(subset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteMinConstant(ConstantString_Min str)
        {
            var asUShort = (ushort)str;
            var ix = (asUShort >> 8);
            var len = asUShort & 0xFF;

            var subset = ConstantString_Min_Chars_Span.Slice(ix, len);

            WriteSpan(subset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteValueConstant(ConstantString_Value str)
        {
            var asUShort = (ushort)str;
            var ix = (asUShort >> 8);
            var len = asUShort & 0xFF;

            var subset = ConstantString_Value_Chars_Span.Slice(ix, len);

            WriteSpan(subset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write000EscapeConstant(ConstantString_000Escape str)
        {
            var ix = (byte)str;

            WriteSpan(Escape000Prefix_Span);
            Write(ThunkWriterCharArrays.ConstantString_000Escape_Chars[ix]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write001EscapeConstant(ConstantString_001Escape str)
        {
            var ix = (byte)str;

            WriteSpan(Escape001Prefix_Span);
            Write(ThunkWriterCharArrays.ConstantString_001Escape_Chars[ix]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDayOfWeek(ConstantString_DaysOfWeek str)
        {
            var ix = (byte)str;

            var subset = ConstantString_DaysOfWeek_Span.Slice(ix, 3);

            WriteSpan(subset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(string strRef)
        {
            WriteSpan(strRef.AsSpan());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void End()
        {
            if(Start > 0)
            {
                Builder.Advance(Start);
            }

            Current = Span<char>.Empty;
            Start = 0;
            Builder = null;
        }
    }
}
#endif