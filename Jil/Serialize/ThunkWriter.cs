using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    delegate void StringThunkDelegate<T>(ref ThunkWriter writer, T data, int depth);

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    sealed class ConstantThunkStringValueAttribute : Attribute
    {
        public string Value { get; private set; }
        public ConstantThunkStringValueAttribute(string value)
        {
            Value = value;
        }
    }

    // Each of these enum has an int value composed of two shorts; the first of which is the length of the string; the second is it's index.
    // All the Values of their annotations are concated so that we can quickly look them up and do so w/o allocations
    enum ConstantString : int
    {
        [ConstantThunkStringValue("-2147483648")]
        Int_MinValue = (11 << 16) | 0,
        [ConstantThunkStringValue("-9223372036854775808")]
        LongMinValue = (20 << 16) | 11,
        [ConstantThunkStringValue("\"")]
        Quote = (1 << 16) | (20 + 11),
        [ConstantThunkStringValue(@"\\")]
        DoubleBackSlash = (2 << 16) | (20 + 11 + 1),
        [ConstantThunkStringValue("\\\"")]
        BackSlashQuote = (2 << 16) | (20 + 11 + 1 + 2),
        [ConstantThunkStringValue("null")]
        Null = (4 << 16) | (20 + 11 + 1 + 2 + 2),
        [ConstantThunkStringValue(@"\u0000")]
        EscapeSequence_0000 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4),
        [ConstantThunkStringValue(@"\u0001")]
        EscapeSequence_0001 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6),
        [ConstantThunkStringValue(@"\u0002")]
        EscapeSequence_0002 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6),
        [ConstantThunkStringValue(@"\u0003")]
        EscapeSequence_0003 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0004")]
        EscapeSequence_0004 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0005")]
        EscapeSequence_0005 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0006")]
        EscapeSequence_0006 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0007")]
        EscapeSequence_0007 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u000B")]
        EscapeSequence_000B = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u000E")]
        EscapeSequence_000E = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u000F")]
        EscapeSequence_000F = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0010")]
        EscapeSequence_0010 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0011")]
        EscapeSequence_0011 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0012")]
        EscapeSequence_0012 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0013")]
        EscapeSequence_0013 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0014")]
        EscapeSequence_0014 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0015")]
        EscapeSequence_0015 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0016")]
        EscapeSequence_0016 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0017")]
        EscapeSequence_0017 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0018")]
        EscapeSequence_0018 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u0019")]
        EscapeSequence_0019 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u001A")]
        EscapeSequence_001A = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u001B")]
        EscapeSequence_001B = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u001C")]
        EscapeSequence_001C = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u001D")]
        EscapeSequence_001D = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u001E")]
        EscapeSequence_001E = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u001F")]
        EscapeSequence_001F = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u2028")]
        EscapeSequence_2028 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\u2029")]
        EscapeSequence_2029 = (6 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\b")]
        EscapeSequence_b = (2 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6),
        [ConstantThunkStringValue(@"\t")]
        EscapeSequence_t = (2 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 2),
        [ConstantThunkStringValue(@"\n")]
        EscapeSequence_n = (2 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 2 + 2),
        [ConstantThunkStringValue(@"\f")]
        EscapeSequence_f = (2 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 2 + 2 + 2),
        [ConstantThunkStringValue(@"\r")]
        EscapeSequence_r = (2 << 16) | (20 + 11 + 1 + 2 + 2 + 4 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 6 + 2 + 2 + 2 + 2)
    }

    struct ThunkWriter
    {
        const int InitialSize = 128;

        static readonly char[] ConstantStrings;

        static ThunkWriter()
        {
            var vals = Enum.GetValues(typeof(ConstantString));
            var maxIx = 0;
            var sizeAtMaxIx = 0;
            foreach(var val in vals)
            {
                var asInt = (int)(ConstantString)val;
                var ix = asInt & 0xFFFF;
                var size = (asInt >> 16) & 0xFFFF;

                if (ix > maxIx)
                {
                    maxIx = ix;
                    sizeAtMaxIx = size;
                }
            }

            ConstantStrings = new char[maxIx + sizeAtMaxIx];
            foreach (var val in vals)
            {
                var asInt = (int)(ConstantString)val;
                var ix = asInt & 0xFFFF;
                var size = (asInt >> 16) & 0xFFFF;

                var field = typeof(ConstantString).GetField(((ConstantString)val).ToString(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var str = ((ConstantThunkStringValueAttribute)field.GetCustomAttributes(typeof(ConstantThunkStringValueAttribute), false)[0]).Value;
                var asChar = str.ToCharArray();

                if (size != asChar.Length)
                {
                    throw new Exception("Expected " + str + " to be of length " + size);
                }

                Array.Copy(asChar, 0, ConstantStrings, ix, size);
            }
        }

        int Index;
        char[] Builder;

        public void Init()
        {
            Index = 0;
            Builder = new char[InitialSize];
        }

        public void Write(float f)
        {
            Write(f.ToString(CultureInfo.InvariantCulture));
        }

        public void Write(double d)
        {
            Write(d.ToString(CultureInfo.InvariantCulture));
        }

        public void Write(decimal m)
        {
            Write(m.ToString(CultureInfo.InvariantCulture));
        }

        void Expand(int adding)
        {
            var mustBeAtLeast = Index + adding;
            if (mustBeAtLeast >= Builder.Length)
            {
                var newBuilder = new char[Builder.Length * 2];
                Builder.CopyTo(newBuilder, 0);
                Builder = newBuilder;
            }
        }

        public void Write(char[] ch, int startIx, int len)
        {
            Expand(len);

            Array.Copy(ch, startIx, Builder, Index, len);

            Index += len;
        }

        public void Write(char ch)
        {
            Expand(1);
            Builder[Index] = ch;
            Index++;
        }

        public void WriteConstant(ConstantString str)
        {
            var asInt = (int)(ConstantString)str;
            var ix = asInt & 0xFFFF;
            var size = (asInt >> 16) & 0xFFFF;

            Expand(size);

            Array.Copy(ConstantStrings, ix, Builder, Index, size);
            Index += size;
        }

        public string StaticToString()
        {
            return new string(Builder, 0, Index);
        }

        #region Slow Builds Only
        // these methods are only called to compare faster methods to serializing
        //   as such they need not be optimized

        void Write(string strRef)
        {
            var asChar = strRef.ToCharArray();
            Write(asChar, 0, asChar.Length);
        }

        public void Write(byte b)
        {
            Write(b.ToString());
        }
        public void Write(sbyte b)
        {
            Write(b.ToString());
        }
        public void Write(short b)
        {
            Write(b.ToString());
        }
        public void Write(ushort b)
        {
            Write(b.ToString());
        }
        public void Write(int b)
        {
            Write(b.ToString());
        }
        public void Write(uint b)
        {
            Write(b.ToString());
        }
        public void Write(long b)
        {
            Write(b.ToString());
        }
        public void Write(ulong b)
        {
            Write(b.ToString());
        }
        #endregion
    }
}
