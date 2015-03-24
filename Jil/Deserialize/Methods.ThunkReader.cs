using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static partial class Methods
    {
        static readonly MethodInfo ConsumeWhiteSpaceThunkReader = typeof(Methods).GetMethod("_ConsumeWhiteSpaceThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _ConsumeWhiteSpaceThunkReader(ref ThunkReader reader)
        {
            int c;
            while ((c = reader.Peek()) != -1)
            {
                if (!IsWhiteSpace(c)) return;

                reader.Read();
            }
        }

        static readonly MethodInfo ReadUInt8ThunkReader = typeof(Methods).GetMethod("_ReadUInt8ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte _ReadUInt8ThunkReader(ref ThunkReader reader)
        {
            // max:  512
            // min:    0
            // digits: 3

            uint ret = 0;

            // first digit *must* exist, we can't overread
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Unexpected end of reader", ref reader, true);
            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (byte)ret;
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (byte)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            AssertNotFollowedByDigit(ref reader);

            checked
            {
                return (byte)ret;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AssertNotFollowedByDigit(ref ThunkReader reader)
        {
            var next = reader.Peek();

            if (next >= '0' && next <= '9')
            {
                throw new DeserializationException(new OverflowException("Number did not end when expected, may overflow"), ref reader, false);
            }
        }
    }
}
