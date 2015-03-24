using System;
using System.Collections.Generic;
using System.Globalization;
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

        static readonly MethodInfo ReadDoubleFastThunkReader = typeof(Methods).GetMethod("_ReadDoubleFastThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double _ReadDoubleFastThunkReader(ref ThunkReader reader, ref char[] buffer)
        {
            var idx = 0;
            InitDynamicBuffer(ref buffer);

            int c;

            var prev = -1;

            var firstDigitIdx = -1;
            var firstValidCharIdx = -1;
            var decimalPointIdx = -1;
            var eIdx = -1;

            while ((c = reader.Peek()) != -1)
            {
                if (c >= '0' && c <= '9')
                {
                    if (firstDigitIdx < 0)
                    {
                        firstDigitIdx = idx;
                        if (firstValidCharIdx < 0)
                        {
                            firstValidCharIdx = idx;
                        }
                    }
                }
                else
                {
                    if (c == '+')
                    {
                        if (!(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected +", ref reader, false);
                        }
                        firstValidCharIdx = idx;
                    }
                    else
                    {
                        if (c == '-')
                        {
                            if (prev != -1 && !(prev == 'e' || prev == 'E'))
                            {
                                throw new DeserializationException("Unexpected -", ref reader, false);
                            }
                            firstValidCharIdx = idx;
                        }
                        else
                        {
                            if (c == 'e' || c == 'E')
                            {
                                if (eIdx >= 0 || firstDigitIdx < 0)
                                {
                                    throw new DeserializationException("Unexpected " + c, ref reader, false);
                                }
                                eIdx = idx;
                            }
                            else
                            {
                                if (c == '.')
                                {
                                    if (eIdx >= 0 || decimalPointIdx >= 0)
                                    {
                                        throw new DeserializationException("Unexpected .", ref reader, false);
                                    }
                                    decimalPointIdx = idx;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                buffer[idx] = (char)c;
                idx++;

                if (idx >= buffer.Length)
                {
                    GrowDynamicBuffer(ref buffer);
                }

                reader.Read();
                prev = c;
            }

            if (buffer[idx - 1] == '.') throw new DeserializationException("Number cannot end with .", ref reader, false);
            if (idx >= 2 && buffer[0] == '0')
            {
                var secondChar = buffer[1];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }
            if (idx >= 3 && buffer[0] == '-' && buffer[1] == '0')
            {
                var secondChar = buffer[2];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }

            if (eIdx < 0)
            {
                var endIdx = idx;
                while (decimalPointIdx >= 0 && endIdx > 1 && buffer[endIdx - 1] == '0')
                {
                    endIdx--;
                }

                var startIdx =
                    decimalPointIdx < 0 ?
                        firstDigitIdx :
                        Math.Min(decimalPointIdx, firstDigitIdx);

                while (startIdx < endIdx && buffer[startIdx] == '0')
                {
                    startIdx++;
                }

                var hasIntegerComponent = buffer[startIdx] != '.';
                var includesDecimalPoint = decimalPointIdx >= 0;
                var lastCharIs5 = endIdx > 1 && buffer[endIdx - 1] == '5';
                var maxChars = 5 +
                    (hasIntegerComponent ? 1 : 0) +
                    (includesDecimalPoint ? 1 : 0) +
                    (lastCharIs5 ? 1 : 0);

                if (endIdx - startIdx <= maxChars)
                {
                    if (decimalPointIdx == endIdx - 1)
                    {
                        decimalPointIdx = -1;
                        endIdx--;
                    }

                    var n = 0;
                    for (idx = startIdx; idx < endIdx; ++idx)
                    {
                        if (idx != decimalPointIdx)
                            n = n * 10 + buffer[idx] - '0';
                    }

                    if (buffer[firstValidCharIdx] == '-')
                    {
                        n = -n;
                    }

                    var result = (double)n;
                    if (decimalPointIdx >= 0)
                    {
                        result /= DoubleDividers[endIdx - decimalPointIdx - 1];
                    }

                    return result;
                }
            }

            return double.Parse(new string(buffer, 0, idx), CultureInfo.InvariantCulture);
        }

        static readonly MethodInfo ReadDoubleCharArrayThunkReader = typeof(Methods).GetMethod("_ReadDoubleCharArrayThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double _ReadDoubleCharArrayThunkReader(ref ThunkReader reader, ref char[] buffer)
        {
            var idx = 0;
            InitDynamicBuffer(ref buffer);

            int c;

            var prev = -1;
            var afterFirstDigit = false;
            var afterE = false;
            var afterDot = false;

            while ((c = reader.Peek()) != -1)
            {
                var isDigit = c >= '0' && c <= '9';
                if (!isDigit)
                {
                    var isPlus = c == '+';
                    if (isPlus)
                    {
                        if (!(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected +", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, ref reader, false);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", ref reader, false);
                        }

                        afterDot = true;
                        goto storeChar;
                    }

                    break;
                }
                else
                {
                    afterFirstDigit = true;
                }

            storeChar:
                buffer[idx++] = (char)c;
                if (idx >= buffer.Length)
                {
                    GrowDynamicBuffer(ref buffer);
                }
                reader.Read();
                prev = c;
            }

            if (buffer[idx - 1] == '.') throw new DeserializationException("Number cannot end with .", ref reader, false);
            if (idx >= 2 && buffer[0] == '0')
            {
                var secondChar = buffer[1];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }
            if (idx >= 3 && buffer[0] == '-' && buffer[1] == '0')
            {
                var secondChar = buffer[2];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }


            var result = double.Parse(new string(buffer, 0, idx), CultureInfo.InvariantCulture);
            return result;
        }

        static readonly MethodInfo ReadDoubleThunkReader = typeof(Methods).GetMethod("_ReadDoubleThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double _ReadDoubleThunkReader(ref ThunkReader reader, ref StringBuilder commonSb)
        {
            commonSb = commonSb ?? new StringBuilder();

            int c;

            var prev = -1;
            var afterFirstDigit = false;
            var afterE = false;
            var afterDot = false;

            while ((c = reader.Peek()) != -1)
            {
                var isDigit = c >= '0' && c <= '9';
                if (!isDigit)
                {
                    var isPlus = c == '+';
                    if (isPlus)
                    {
                        if (!(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected +", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, ref reader, false);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", ref reader, false);
                        }

                        afterDot = true;
                        goto storeChar;
                    }

                    break;
                }
                else
                {
                    afterFirstDigit = true;
                }

            storeChar:
                commonSb.Append((char)c);
                reader.Read();
                prev = c;
            }

            var asStr = commonSb.ToString();
            var strLen = asStr.Length;
            if (asStr[strLen - 1] == '.') throw new DeserializationException("Number cannot end with .", ref reader, false);
            if (strLen >= 2 && asStr[0] == '0')
            {
                var secondChar = asStr[1];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }
            if (strLen >= 3 && asStr[0] == '-' && asStr[1] == '0')
            {
                var secondChar = asStr[2];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }

            var result = double.Parse(asStr, CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        static readonly MethodInfo ReadSingleFastThunkReader = typeof(Methods).GetMethod("_ReadSingleFastThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float _ReadSingleFastThunkReader(ref ThunkReader reader, ref char[] buffer)
        {
            var idx = 0;
            InitDynamicBuffer(ref buffer);

            int c;

            var prev = -1;

            var firstDigitIdx = -1;
            var firstValidCharIdx = -1;
            var decimalPointIdx = -1;
            var eIdx = -1;

            while ((c = reader.Peek()) != -1)
            {
                if (c >= '0' && c <= '9')
                {
                    if (firstDigitIdx < 0)
                    {
                        firstDigitIdx = idx;
                        if (firstValidCharIdx < 0)
                        {
                            firstValidCharIdx = idx;
                        }
                    }
                }
                else
                {
                    if (c == '+')
                    {
                        if (!(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected +", ref reader, false);
                        }
                        firstValidCharIdx = idx;
                    }
                    else
                    {
                        if (c == '-')
                        {
                            if (prev != -1 && !(prev == 'e' || prev == 'E'))
                            {
                                throw new DeserializationException("Unexpected -", ref reader, false);
                            }
                            firstValidCharIdx = idx;
                        }
                        else
                        {
                            if (c == 'e' || c == 'E')
                            {
                                if (eIdx >= 0 || firstDigitIdx < 0)
                                {
                                    throw new DeserializationException("Unexpected " + c, ref reader, false);
                                }
                                eIdx = idx;
                            }
                            else
                            {
                                if (c == '.')
                                {
                                    if (eIdx >= 0 || decimalPointIdx >= 0)
                                    {
                                        throw new DeserializationException("Unexpected .", ref reader, false);
                                    }
                                    decimalPointIdx = idx;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                buffer[idx] = (char)c;
                idx++;

                if (idx >= buffer.Length)
                {
                    GrowDynamicBuffer(ref buffer);
                }

                reader.Read();
                prev = c;
            }

            if (buffer[idx - 1] == '.') throw new DeserializationException("Number cannot end with .", ref reader, false);
            if (idx >= 2 && buffer[0] == '0')
            {
                var secondChar = buffer[1];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }
            if (idx >= 3 && buffer[0] == '-' && buffer[1] == '0')
            {
                var secondChar = buffer[2];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }

            if (eIdx < 0)
            {
                var endIdx = idx;
                while (decimalPointIdx >= 0 && endIdx > 1 && buffer[endIdx - 1] == '0')
                {
                    endIdx--;
                }

                var startIdx =
                    decimalPointIdx < 0 ?
                        firstDigitIdx :
                        Math.Min(decimalPointIdx, firstDigitIdx);

                while (startIdx < endIdx && buffer[startIdx] == '0')
                {
                    startIdx++;
                }

                var hasIntegerComponent = buffer[startIdx] != '.';
                var includesDecimalPoint = decimalPointIdx >= 0;
                var maxChars =
                    6 +
                    (hasIntegerComponent ? 1 : 0) +
                    (includesDecimalPoint ? 1 : 0);

                if (endIdx - startIdx <= maxChars)
                {
                    if (decimalPointIdx == endIdx - 1)
                    {
                        decimalPointIdx = -1;
                        endIdx--;
                    }

                    var n = 0;
                    for (idx = startIdx; idx < endIdx; idx++)
                    {
                        if (idx != decimalPointIdx)
                        {
                            n = n * 10 + buffer[idx] - '0';
                        }
                    }

                    if (buffer[firstValidCharIdx] == '-')
                    {
                        n = -n;
                    }

                    var result = (float)n;
                    if (decimalPointIdx >= 0)
                    {
                        result /= SingleDividers[endIdx - decimalPointIdx - 1];
                    }

                    return result;
                }
            }

            return float.Parse(new string(buffer, 0, idx), CultureInfo.InvariantCulture);
        }

        static readonly MethodInfo ReadSingleCharArrayThunkReader = typeof(Methods).GetMethod("_ReadSingleCharArrayThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float _ReadSingleCharArrayThunkReader(ref ThunkReader reader, ref char[] buffer)
        {
            var idx = 0;
            InitDynamicBuffer(ref buffer);

            int c;

            var prev = -1;
            var afterFirstDigit = false;
            var afterE = false;
            var afterDot = false;

            while ((c = reader.Peek()) != -1)
            {
                var isDigit = c >= '0' && c <= '9';
                if (!isDigit)
                {
                    var isPlus = c == '+';
                    if (isPlus)
                    {
                        if (!(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected +", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, ref reader, false);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", ref reader, false);
                        }

                        afterDot = true;
                        goto storeChar;
                    }

                    break;
                }
                else
                {
                    afterFirstDigit = true;
                }

            storeChar:
                buffer[idx++] = (char)c;
                if (idx >= buffer.Length)
                {
                    GrowDynamicBuffer(ref buffer);
                }
                reader.Read();
                prev = c;
            }

            if (buffer[idx - 1] == '.') throw new DeserializationException("Number cannot end with .", ref reader, false);
            if (idx >= 2 && buffer[0] == '0')
            {
                var secondChar = buffer[1];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }
            if (idx >= 3 && buffer[0] == '-' && buffer[1] == '0')
            {
                var secondChar = buffer[2];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }

            var result = float.Parse(new string(buffer, 0, idx), CultureInfo.InvariantCulture);
            return result;
        }

        static readonly MethodInfo ReadSingleThunkReader = typeof(Methods).GetMethod("_ReadSingleThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float _ReadSingleThunkReader(ref ThunkReader reader, ref StringBuilder commonSb)
        {
            commonSb = commonSb ?? new StringBuilder();

            int c;

            var prev = -1;
            var afterFirstDigit = false;
            var afterE = false;
            var afterDot = false;

            while ((c = reader.Peek()) != -1)
            {
                var isDigit = c >= '0' && c <= '9';
                if (!isDigit)
                {
                    var isPlus = c == '+';
                    if (isPlus)
                    {
                        if (!(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected +", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", ref reader, false);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, ref reader, false);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", ref reader, false);
                        }

                        afterDot = true;
                        goto storeChar;
                    }

                    break;
                }
                else
                {
                    afterFirstDigit = true;
                }

            storeChar:
                commonSb.Append((char)c);
                reader.Read();
                prev = c;
            }

            var asStr = commonSb.ToString();
            var strLen = asStr.Length;
            if (asStr[strLen - 1] == '.') throw new DeserializationException("Number cannot end with .", ref reader, false);
            if (strLen >= 2 && asStr[0] == '0')
            {
                var secondChar = asStr[1];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }
            if (strLen >= 3 && asStr[0] == '-' && asStr[1] == '0')
            {
                var secondChar = asStr[2];
                if (secondChar != '.' && secondChar != 'e' && secondChar != 'E')
                {
                    throw new DeserializationException("Number cannot have leading zeros", ref reader, false);
                }
            }

            var result = float.Parse(commonSb.ToString(), CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }
    }
}
