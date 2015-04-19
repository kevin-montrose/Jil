using Jil.Common;
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

        static readonly MethodInfo ReadDecimalFastThunkReader = typeof(Methods).GetMethod("_ReadDecimalFastThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static decimal _ReadDecimalFastThunkReader(ref ThunkReader reader, ref char[] buffer)
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

                var maxChars = decimalPointIdx < 0 ? 18 : 19;
                if (endIdx - firstDigitIdx <= maxChars)
                {
                    if (decimalPointIdx == endIdx - 1)
                    {
                        decimalPointIdx = -1;
                        endIdx--;
                    }

                    var negative = buffer[firstValidCharIdx] == '-';

                    decimal result;
                    var n1 = 0; // we use int rather than long so as to work well on 32-bit runtime

                    for (idx = firstDigitIdx; idx < endIdx && n1 < 100000000; ++idx)
                    {
                        if (idx != decimalPointIdx)
                        {
                            n1 = n1 * 10 + buffer[idx] - '0';
                        }
                    }

                    if (negative)
                    {
                        n1 = -n1;
                    }

                    if (idx == endIdx)
                    {
                        result = n1;
                    }
                    else
                    {
                        var n2 = 0;
                        var multiplier = 1;
                        while (idx < endIdx)
                        {
                            if (idx != decimalPointIdx)
                            {
                                multiplier *= 10;
                                n2 = n2 * 10 + buffer[idx] - '0';
                            }

                            idx++;
                        }

                        result = (long)n1 * multiplier + (long)n2;
                    }

                    if (decimalPointIdx > 0)
                    {
                        result *= DecimalMultipliers[endIdx - decimalPointIdx - 1];
                    }

                    return result;
                }
            }

            return decimal.Parse(new string(buffer, 0, idx), NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        static readonly MethodInfo ReadDecimalCharArrayThunkReader = typeof(Methods).GetMethod("_ReadDecimalCharArrayThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static decimal _ReadDecimalCharArrayThunkReader(ref ThunkReader reader, ref char[] buffer)
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

            var asStr = new string(buffer, 0, idx);
            var result = decimal.Parse(asStr, NumberStyles.Float, CultureInfo.InvariantCulture);
            return result;
        }

        static readonly MethodInfo ReadDecimalThunkReader = typeof(Methods).GetMethod("_ReadDecimalThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static decimal _ReadDecimalThunkReader(ref ThunkReader reader, ref StringBuilder commonSb)
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

            var result = decimal.Parse(asStr, NumberStyles.Float, CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        static readonly MethodInfo ReadEncodedCharThunkReader = typeof(Methods).GetMethod("_ReadEncodedCharThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char _ReadEncodedCharThunkReader(ref ThunkReader reader)
        {
            var first = reader.Read();
            if (first == -1) throw new DeserializationException("Expected any character", ref reader, true);

            if (first != '\\') return (char)first;

            var second = reader.Read();
            if (second == -1) throw new DeserializationException("Expected any character", ref reader, true);

            switch (second)
            {
                case '"': return '"';
                case '\\': return '\\';
                case '/': return '/';
                case 'b': return '\b';
                case 'f': return '\f';
                case 'n': return '\n';
                case 'r': return '\r';
                case 't': return '\t';
            }

            if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", ref reader, false);

            // now we're in an escape sequence, we expect 4 hex #s; always
            var ret = 0;

            //char1:
            {
                var c = reader.Read();

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    goto char2;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char2;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char2;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

        char2:
            ret *= 16;
            {
                var c = reader.Read();

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    goto char3;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char3;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char3;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

        char3:
            ret *= 16;
            {
                var c = reader.Read();

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    goto char4;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char4;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char4;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

        char4:
            ret *= 16;
            {
                var c = reader.Read();

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    goto finished;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto finished;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto finished;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

        finished:
            if (ret < char.MinValue || ret > char.MaxValue) throw new DeserializationException("Encoded character out of System.Char range, found: " + ret, ref reader, false);

            return (char)ret;
        }

        static readonly MethodInfo ReadEncodedStringWithCharArrayThunkReader = typeof(Methods).GetMethod("_ReadEncodedStringWithCharArrayThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        static string _ReadEncodedStringWithCharArrayThunkReader(ref ThunkReader reader, ref char[] buffer)
        {
            var idx = 0;
            InitDynamicBuffer(ref buffer);

            while (true)
            {
                while (idx < buffer.Length - 1)
                {
                    var first = reader.Read();
                    if (first == -1) throw new DeserializationException("Expected any character", ref reader, true);

                    if (first == '"')
                    {
                        goto complete;
                    }

                    if (first != '\\')
                    {
                        buffer[idx++] = (char)first;
                        continue;
                    }

                    var second = reader.Read();
                    if (second == -1) throw new DeserializationException("Expected any character", ref reader, true);

                    switch (second)
                    {
                        case '"': buffer[idx++] = '"'; continue;
                        case '\\': buffer[idx++] = '\\'; continue;
                        case '/': buffer[idx++] = '/'; continue;
                        case 'b': buffer[idx++] = '\b'; continue;
                        case 'f': buffer[idx++] = '\f'; continue;
                        case 'n': buffer[idx++] = '\n'; continue;
                        case 'r': buffer[idx++] = '\r'; continue;
                        case 't': buffer[idx++] = '\t'; continue;
                    }

                    if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", ref reader, false);

                    // now we're in an escape sequence, we expect 4 hex #s; always
                    buffer[idx++] = ReadHexQuad2(ref reader);
                }

                GrowDynamicBuffer(ref buffer);
            }

            complete: 
            return new string(buffer, 0, idx);
        }

        static char ReadHexQuad2(ref ThunkReader reader)
        {
            int unescaped;
            int c;

            c = reader.Read();
            if (c >= '0' && c <= '9')
            {
                unescaped = (c - '0') << 12;
            }
            else if (c >= 'A' && c <= 'F')
            {
                unescaped = (10 + c - 'A') << 12;
            }
            else if (c >= 'a' && c <= 'f')
            {
                unescaped = (10 + c - 'a') << 12;
            }
            else
            {
                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

            c = reader.Read();
            if (c >= '0' && c <= '9')
            {
                unescaped |= (c - '0') << 8;
            }
            else if (c >= 'A' && c <= 'F')
            {
                unescaped |= (10 + c - 'A') << 8;
            }
            else if (c >= 'a' && c <= 'f')
            {
                unescaped |= (10 + c - 'a') << 8;
            }
            else
            {
                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

            c = reader.Read();
            if (c >= '0' && c <= '9')
            {
                unescaped |= (c - '0') << 4;
            }
            else if (c >= 'A' && c <= 'F')
            {
                unescaped |= (10 + c - 'A') << 4;
            }
            else if (c >= 'a' && c <= 'f')
            {
                unescaped |= (10 + c - 'a') << 4;
            }
            else
            {
                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

            c = reader.Read();
            if (c >= '0' && c <= '9')
            {
                unescaped |= c - '0';
            }
            else if (c >= 'A' && c <= 'F')
            {
                unescaped |= 10 + c - 'A';
            }
            else if (c >= 'a' && c <= 'f')
            {
                unescaped |= 10 + c - 'a';
            }
            else
            {
                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, c == -1);
            }

            return (char)unescaped;
        }

        static readonly MethodInfo ReadEncodedStringWithBufferThunkReader = typeof(Methods).GetMethod("_ReadEncodedStringWithBufferThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedStringWithBufferThunkReader(ref ThunkReader reader, char[] buffer, ref StringBuilder commonSb)
        {
            commonSb = commonSb ?? new StringBuilder();

            {
                var ix = 0;

                while (ix <= CharBufferSize)
                {
                    if (ix == CharBufferSize)
                    {
                        commonSb.Append(buffer, 0, ix);
                        break;
                    }

                    var first = reader.Read();
                    if (first == -1) throw new DeserializationException("Expected any character", ref reader, true);

                    // we didn't have to use anything but the buffer, make a string and return it!
                    if (first == '"')
                    {
                        // avoid an allocation here
                        if (ix == 0) return "";

                        return new string(buffer, 0, ix);
                    }

                    if (first != '\\')
                    {
                        buffer[ix] = (char)first;
                        ix++;
                        continue;
                    }

                    var second = reader.Read();
                    if (second == -1) throw new DeserializationException("Expected any character", ref reader, true);

                    switch (second)
                    {
                        case '"': buffer[ix] = '"'; ix++; continue;
                        case '\\': buffer[ix] = '\\'; ix++; continue;
                        case '/': buffer[ix] = '/'; ix++; continue;
                        case 'b': buffer[ix] = '\b'; ix++; continue;
                        case 'f': buffer[ix] = '\f'; ix++; continue;
                        case 'n': buffer[ix] = '\n'; ix++; continue;
                        case 'r': buffer[ix] = '\r'; ix++; continue;
                        case 't': buffer[ix] = '\t'; ix++; continue;
                    }

                    if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", ref reader, false);

                    commonSb.Append(buffer, 0, ix);

                    // now we're in an escape sequence, we expect 4 hex #s; always
                    ReadHexQuadToBuilder(ref reader, commonSb);
                    break;
                }
            }

            // fall through to using a StringBuilder

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character", ref reader, true);

                if (first == '"')
                {
                    break;
                }

                if (first != '\\')
                {
                    commonSb.Append((char)first);
                    continue;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", ref reader, true);

                switch (second)
                {
                    case '"': commonSb.Append('"'); continue;
                    case '\\': commonSb.Append('\\'); continue;
                    case '/': commonSb.Append('/'); continue;
                    case 'b': commonSb.Append('\b'); continue;
                    case 'f': commonSb.Append('\f'); continue;
                    case 'n': commonSb.Append('\n'); continue;
                    case 'r': commonSb.Append('\r'); continue;
                    case 't': commonSb.Append('\t'); continue;
                }

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", ref reader, false);

                // now we're in an escape sequence, we expect 4 hex #s; always
                ReadHexQuadToBuilder(ref reader, commonSb);
            }

            var ret = commonSb.ToString();

            // leave this clean for the next use
            commonSb.Clear();

            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ReadHexQuadToBuilder(ref ThunkReader reader, StringBuilder commonSb)
        {
            var encodedChar = 0;

            //char1:
            {
                var c = reader.Read();

                if (c == -1) throw new DeserializationException("Expected hex digit, stream ended instead", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    encodedChar += c;
                    goto char2;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto char2;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto char2;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        char2:
            encodedChar *= 16;
            {
                var c = reader.Read();

                if (c == -1) throw new DeserializationException("Expected hex digit, stream ended instead", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    encodedChar += c;
                    goto char3;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto char3;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto char3;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        char3:
            encodedChar *= 16;
            {
                var c = reader.Read();

                if (c == -1) throw new DeserializationException("Expected hex digit, stream ended instead", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    encodedChar += c;
                    goto char4;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto char4;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto char4;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        char4:
            encodedChar *= 16;
            {
                var c = reader.Read();

                if (c == -1) throw new DeserializationException("Expected hex digit, stream ended instead", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    encodedChar += c;
                    goto finished;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto finished;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    encodedChar += 10 + c;
                    goto finished;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        finished:
            commonSb.Append(Utils.SafeConvertFromUtf32(encodedChar));
        }

        static readonly MethodInfo ReadEncodedStringThunkReader = typeof(Methods).GetMethod("_ReadEncodedStringThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedStringThunkReader(ref ThunkReader reader, ref StringBuilder commonSb)
        {
            commonSb = commonSb ?? new StringBuilder();

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character", ref reader, true);

                if (first == '"')
                {
                    break;
                }

                if (first != '\\')
                {
                    commonSb.Append((char)first);
                    continue;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", ref reader, true);

                switch (second)
                {
                    case '"': commonSb.Append('"'); continue;
                    case '\\': commonSb.Append('\\'); continue;
                    case '/': commonSb.Append('/'); continue;
                    case 'b': commonSb.Append('\b'); continue;
                    case 'f': commonSb.Append('\f'); continue;
                    case 'n': commonSb.Append('\n'); continue;
                    case 'r': commonSb.Append('\r'); continue;
                    case 't': commonSb.Append('\t'); continue;
                }

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", ref reader, false);

                // now we're in an escape sequence, we expect 4 hex #s; always
                ReadHexQuadToBuilder(ref reader, commonSb);
            }

            var ret = commonSb.ToString();

            // leave this clean for the next use
            commonSb.Clear();

            return ret;
        }

        static readonly MethodInfo ReadGuidThunkReader = typeof(Methods).GetMethod("_ReadGuidThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Guid _ReadGuidThunkReader(ref ThunkReader reader)
        {
            // 1314FAD4-7505-439D-ABD2-DBD89242928C
            // 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
            //
            // Guid is guaranteed to be a 36 character string

            // Bytes are in a different order than you might expect
            // For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0 
            // Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0 
            // Ix:   0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
            //
            // And we have to account for dashes
            //
            // So the map is like so:
            // chars[0]  -> bytes[3]  -> buffer[ 0, 1]
            // chars[1]  -> bytes[2]  -> buffer[ 2, 3]
            // chars[2]  -> bytes[1]  -> buffer[ 4, 5]
            // chars[3]  -> bytes[0]  -> buffer[ 6, 7]
            // chars[4]  -> bytes[5]  -> buffer[ 9,10]
            // chars[5]  -> bytes[4]  -> buffer[11,12]
            // chars[6]  -> bytes[7]  -> buffer[14,15]
            // chars[7]  -> bytes[6]  -> buffer[16,17]
            // chars[8]  -> bytes[8]  -> buffer[19,20]
            // chars[9]  -> bytes[9]  -> buffer[21,22]
            // chars[10] -> bytes[10] -> buffer[24,25]
            // chars[11] -> bytes[11] -> buffer[26,27]
            // chars[12] -> bytes[12] -> buffer[28,29]
            // chars[13] -> bytes[13] -> buffer[30,31]
            // chars[14] -> bytes[14] -> buffer[32,33]
            // chars[15] -> bytes[15] -> buffer[34,35]

            var asStruct = new GuidStruct();
            asStruct.B03 = ReadGuidByte(ref reader);
            asStruct.B02 = ReadGuidByte(ref reader);
            asStruct.B01 = ReadGuidByte(ref reader);
            asStruct.B00 = ReadGuidByte(ref reader);

            var c = reader.Read();
            if (c != '-') throw new DeserializationException("Expected -", ref reader, c == -1);

            asStruct.B05 = ReadGuidByte(ref reader);
            asStruct.B04 = ReadGuidByte(ref reader);

            c = reader.Read();
            if (c != '-') throw new DeserializationException("Expected -", ref reader, c == -1);

            asStruct.B07 = ReadGuidByte(ref reader);
            asStruct.B06 = ReadGuidByte(ref reader);

            c = reader.Read();
            if (c != '-') throw new DeserializationException("Expected -", ref reader, c == -1);

            asStruct.B08 = ReadGuidByte(ref reader);
            asStruct.B09 = ReadGuidByte(ref reader);

            c = reader.Read();
            if (c != '-') throw new DeserializationException("Expected -", ref reader, c == -1);

            asStruct.B10 = ReadGuidByte(ref reader);
            asStruct.B11 = ReadGuidByte(ref reader);
            asStruct.B12 = ReadGuidByte(ref reader);
            asStruct.B13 = ReadGuidByte(ref reader);
            asStruct.B14 = ReadGuidByte(ref reader);
            asStruct.B15 = ReadGuidByte(ref reader);

            return asStruct.Value;
        }

        static byte ReadGuidByte(ref ThunkReader reader)
        {
            var a = reader.Read();
            if (a == -1) throw new DeserializationException("Expected any character", ref reader, true);
            if (!((a >= '0' && a <= '9') || (a >= 'A' && a <= 'F') || (a >= 'a' && a <= 'f'))) throw new DeserializationException("Expected a hex number", ref reader, false);
            var b = reader.Read();
            if (b == -1) throw new DeserializationException("Expected any character", ref reader, true);
            if (!((b >= '0' && b <= '9') || (b >= 'A' && b <= 'F') || (b >= 'a' && b <= 'f'))) throw new DeserializationException("Expected a hex number", ref reader, false);

            if (a <= '9')
            {
                a -= '0';
            }
            else
            {
                if (a <= 'F')
                {
                    a -= ('A' - 10);
                }
                else
                {
                    a -= ('a' - 10);
                }
            }

            if (b <= '9')
            {
                b -= '0';
            }
            else
            {
                if (b <= 'F')
                {
                    b -= ('A' - 10);
                }
                else
                {
                    b -= ('a' - 10);
                }
            }

            return (byte)(a * 16 + b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadHexQuadThunkReader(ref ThunkReader reader)
        {
            int unescaped = 0;

            //char1:
            {
                var c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected hex digit, instead reader ended", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    unescaped += c;
                    goto char2;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto char2;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto char2;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        char2:
            unescaped *= 16;
            {
                var c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected hex digit, instead reader ended", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    unescaped += c;
                    goto char3;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto char3;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto char3;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        char3:
            unescaped *= 16;
            {
                var c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected hex digit, instead reader ended", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    unescaped += c;
                    goto char4;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto char4;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto char4;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        char4:
            unescaped *= 16;
            {
                var c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected hex digit, instead reader ended", ref reader, true);

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    unescaped += c;
                    goto finished;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto finished;
                }

                c -= ('f' - 'F');
                if (c >= 0 && c <= 5)
                {
                    unescaped += 10 + c;
                    goto finished;
                }

                throw new DeserializationException("Expected hex digit, found: " + c, ref reader, false);
            }

        finished:
            return unescaped;
        }

        static readonly MethodInfo ParseEnumThunkReader = typeof(Methods).GetMethod("_ParseEnumThunkReader", BindingFlags.NonPublic | BindingFlags.Static);
        static TEnum _ParseEnumThunkReader<TEnum>(string asStr, ref ThunkReader reader)
            where TEnum : struct
        {
            TEnum ret;
            if (!TryParseEnum<TEnum>(asStr, out ret))
            {
                throw new DeserializationException("Unexpected value for " + typeof(TEnum).Name + ": " + asStr, ref reader, false);
            }

            return ret;
        }

        static readonly MethodInfo ReadFlagsEnumThunkReader = typeof(Methods).GetMethod("_ReadFlagsEnumThunkReader", BindingFlags.NonPublic | BindingFlags.Static);
        static TEnum _ReadFlagsEnumThunkReader<TEnum>(ref ThunkReader reader, ref StringBuilder commonSb)
            where TEnum : struct
        {
            commonSb = commonSb ?? new StringBuilder();

            var ret = default(TEnum);

            while (true)
            {
                var c = _ReadEncodedCharThunkReader(ref reader);

                // ignore this *particular* whitespace
                if (c != ' ')
                {
                    // comma delimited
                    if (c == ',' || c == '"')
                    {
                        var asStr = commonSb.ToString();
                        TEnum parsed;
                        if (!TryParseEnum<TEnum>(asStr, out parsed))
                        {
                            throw new DeserializationException("Expected " + typeof(TEnum).Name + ", found: " + asStr, ref reader, false);
                        }

                        ret = FlagsEnumCombiner<TEnum>.Combine(ret, parsed);

                        commonSb.Clear();

                        if (c == '"') break;

                        continue;
                    }
                    commonSb.Append(c);
                }
            }

            // reset before returning
            commonSb.Clear();

            return ret;
        }
    }
}
