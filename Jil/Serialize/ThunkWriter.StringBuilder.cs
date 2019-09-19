#if !BUFFER_AND_SEQUENCE
using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Jil.Serialize
{
    internal ref partial struct ThunkWriter
    {
        public bool HasValue => Builder != null;

        StringBuilder Builder;

        private sealed class StringBuilderTextWriter : TextWriter
        {
            public override Encoding Encoding => throw new NotImplementedException();

            private readonly StringBuilder Inner;

            public StringBuilderTextWriter(StringBuilder inner)
            {
                Inner = inner;
            }

            public override void Write(char value)
            {
                Inner.Append(value);
            }

            public override void Write(string value)
            {
                Inner.Append(value);
            }

            public override void Write(bool a)
            => throw new NotImplementedException();

            public override void Write(char[] a, int b, int c)
            => throw new NotImplementedException();

            public override void Write(decimal a)
            => throw new NotImplementedException();

            public override void Write(int a)
            => throw new NotImplementedException();

            public override void Write(long a)
            => throw new NotImplementedException();

            public override void Write(object a)
            => throw new NotImplementedException();

            public override void Write(string a, object b)
            => throw new NotImplementedException();

            public override void Write(string a, object b, object c)
            => throw new NotImplementedException();

            public override void Write(string a, object[] b)
            => throw new NotImplementedException();

            public override void Write(uint a)
            => throw new NotImplementedException();

            public override void Write(ulong a)
            => throw new NotImplementedException();

            // todo: rest of overrides
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init()
        {
            Builder = (Builder ?? new StringBuilder()).Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(float f)
        {
            Write(f.ToString(CultureInfo.InvariantCulture));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(double d)
        {
            Write(d.ToString(CultureInfo.InvariantCulture));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(decimal m)
        {
            Write(m.ToString(CultureInfo.InvariantCulture));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char[] ch, int startIx, int len)
        {
            Builder.Append(ch, startIx, len);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char ch)
        {
            Builder.Append(ch);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteCommonConstant(ConstantString_Common str)
        {
            var asUShort = (ushort)str;
            var ix = asUShort >> 8;
            var size = asUShort & 0xFF;

            Builder.Append(ThunkWriterCharArrays.ConstantString_Common_Chars, ix, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFormattingConstant(ConstantString_Formatting str)
        {
            var asUShort = (ushort)str;
            var ix = (asUShort >> 8);
            var len = asUShort & 0xFF;

            Builder.Append(ThunkWriterCharArrays.ConstantString_Formatting_Chars, ix, len);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteMinConstant(ConstantString_Min str)
        {
            var asUShort = (ushort)str;
            var ix = (asUShort >> 8);
            var len = asUShort & 0xFF;

            Builder.Append(ThunkWriterCharArrays.ConstantString_Min_Chars, ix, len);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteValueConstant(ConstantString_Value str)
        {
            var asUShort = (ushort)str;
            var ix = (asUShort >> 8);
            var len = asUShort & 0xFF;

            Builder.Append(ThunkWriterCharArrays.ConstantString_Value_Chars, ix, len);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write000EscapeConstant(ConstantString_000Escape str)
        {
            var ix = (byte)str;

            Builder.Append(ThunkWriterCharArrays.Escape000Prefix);
            Builder.Append(ThunkWriterCharArrays.ConstantString_000Escape_Chars[ix]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write001EscapeConstant(ConstantString_001Escape str)
        {
            var ix = (byte)str;

            Builder.Append(ThunkWriterCharArrays.Escape001Prefix);
            Builder.Append(ThunkWriterCharArrays.ConstantString_001Escape_Chars[ix]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDayOfWeek(ConstantString_DaysOfWeek str)
        {
            var ix = (byte)str;
            Builder.Append(ThunkWriterCharArrays.ConstantString_DaysOfWeek, ix, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Write(string strRef)
        {
            Builder.Append(strRef);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string StaticToString()
        {
            return Builder.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void End(ref SerializeDynamic.WriterProxy proxy)
        {
            proxy.Write(StaticToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SerializeDynamic.WriterProxy AsWriterProxy()
        {
            var ret = new SerializeDynamic.WriterProxy();
            ret.InitWithWriter(new StringBuilderTextWriter(Builder));

            return ret;
        }
    }
}
#endif