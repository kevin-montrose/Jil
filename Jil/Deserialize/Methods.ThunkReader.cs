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

        static readonly MethodInfo ReadInt8ThunkReader = typeof(Methods).GetMethod("_ReadInt8ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static sbyte _ReadInt8ThunkReader(ref ThunkReader reader)
        {
            // max:  127
            // min: -127
            // digits: 3

            int ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", ref reader, true);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", ref reader, true);
            }

            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (sbyte)(ret * (negative ? -1 : 1));
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (sbyte)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            AssertNotFollowedByDigit(ref reader);

            checked
            {
                return (sbyte)(ret * (negative ? -1 : 1));
            }
        }

        static readonly MethodInfo ReadInt16ThunkReader = typeof(Methods).GetMethod("_ReadInt16ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static short _ReadInt16ThunkReader(ref ThunkReader reader)
        {
            // max:  32767
            // min: -32768
            // digits:   5

            int ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", ref reader, true);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", ref reader, true);
            }

            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (short)(ret * (negative ? -1 : 1));
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (sbyte)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #4
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (short)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #5
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (short)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            AssertNotFollowedByDigit(ref reader);

            checked
            {
                return (short)(ret * (negative ? -1 : 1));
            }
        }

        static readonly MethodInfo ReadUInt16ThunkReader = typeof(Methods).GetMethod("_ReadUInt16ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ushort _ReadUInt16ThunkReader(ref ThunkReader reader)
        {
            // max: 65535
            // min:     0
            // digits:  5

            uint ret = 0;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit", ref reader, true);

            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (ushort)ret;
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (ushort)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (ushort)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #5
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (ushort)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            AssertNotFollowedByDigit(ref reader);

            checked
            {
                return (ushort)ret;
            }
        }

        static readonly MethodInfo ReadInt32ThunkReader = typeof(Methods).GetMethod("_ReadInt32ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _ReadInt32ThunkReader(ref ThunkReader reader)
        {
            // max:  2147483647
            // min: -2147483648
            // digits:       10

            long ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", ref reader, true);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", ref reader, true);
            }

            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #4
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #5
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #6
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #7
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #8
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #9
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            // digit #10
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += c;

            AssertNotFollowedByDigit(ref reader);

            checked
            {
                return (int)(ret * (negative ? -1 : 1));
            }
        }

        static readonly MethodInfo ReadUInt32ThunkReader = typeof(Methods).GetMethod("_ReadUInt32ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint _ReadUInt32ThunkReader(ref ThunkReader reader)
        {
            // max: 4294967295
            // min:          0
            // digits:      10

            ulong ret = 0;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit", ref reader, true);

            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #5
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #6
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #7
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #8
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #9
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #10
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            AssertNotFollowedByDigit(ref reader);

            checked
            {
                return (uint)ret;
            }
        }

        static readonly MethodInfo ReadInt64ThunkReader = typeof(Methods).GetMethod("_ReadInt64ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long _ReadInt64ThunkReader(ref ThunkReader reader)
        {
            // max:  9223372036854775807
            // min: -9223372036854775808
            // digits:                19

            ulong ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", ref reader, true);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", ref reader, true);
            }

            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #5
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #6
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #7
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #8
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #9
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #10
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #11
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #12
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #13
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #14
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #15
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #16
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #17
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #18
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #19
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            AssertNotFollowedByDigit(ref reader);

            // one edge case, the minimum long value has to be checked for
            //   because otherwise we will overflow long and throw
            if (negative && ret == 9223372036854775808UL)
            {
                return long.MinValue;
            }

            checked
            {
                return ((long)ret * (negative ? -1 : 1));
            }
        }

        static readonly MethodInfo ReadUInt64ThunkReader = typeof(Methods).GetMethod("_ReadUInt64ThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong _ReadUInt64ThunkReader(ref ThunkReader reader)
        {
            // max: 18446744073709551615
            // min:                    0
            // digits:                20

            ulong ret = 0;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit", ref reader, true);

            var firstDigitZero = c == '0';
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", ref reader, false);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            if (firstDigitZero) throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #5
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #6
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #7
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #8
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #9
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #10
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #11
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #12
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #13
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #14
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #15
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #16
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #17
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #18
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #19
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();
            ret *= 10;
            ret += (uint)c;

            // digit #20
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
            reader.Read();

            AssertNotFollowedByDigit(ref reader);

            // ulong special case, we can overflow in the last **addition* instead of the last cast
            checked
            {
                ret *= 10;
                ret += (uint)c;

                return ret;
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
