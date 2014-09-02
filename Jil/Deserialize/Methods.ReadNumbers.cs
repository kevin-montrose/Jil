using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    // Let's talk about parsing numbers.
    //
    // Most of this file handles parsing integer numbers, it cheats for speed in a few ways.
    //   - unroll the loops because the type of a number gives us it's maximum size
    //   - omit sign checks when dealing with unsigned types
    //   - build up w/o allocation using * and +, since we know everything's base-10 w/o separators
    //   - only do overflow checking after reading the *last* digit
    //      * base 10 again, you can't overflow 256 with 2 or fewer characters
    //      * we specify the math we easily can in Int32, which is probably the fastest silicon on the chip anyway
    //   - oversize accumulator (int for byte, long for int, etc.) so we don't need to special case SignedType.MinValue
    //      * *except* for long, where we accumulate into a ulong and do the special checked; because there's no larger type
    static partial class Methods
    {
        public static readonly MethodInfo DiscardNewtonsoftTimeZoneOffset = typeof(Methods).GetMethod("_DiscardNewtonsoftTimeZoneOffset", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _DiscardNewtonsoftTimeZoneOffset(TextReader reader)
        {
            // this is a special case when reading timezone information for NewtsonsoftStyle DateTimes
            //   so far as I can tell this is pointless data, the millisecond offset is still UTC relative
            //   so just use that... should validate that this correct though
            // max +9999
            // min -9999
            // digits: 4

            var c = reader.Peek();
            if (c != '-' && c != '+') return;

            reader.Read();

            var temp = 0;

            // first digit hour
            c = reader.Read();
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            temp += c;

            // second digit hour
            temp *= 10;
            c = reader.Read();
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            temp += c;

            if (temp > 23) throw new DeserializationException("Expected hour portion of timezone offset between 0 and 24", reader);

            temp = 0;
            // first digit minute
            temp *= 10;
            c = reader.Read();
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            temp += c;

            // second digit minute
            temp *= 10;
            c = reader.Read();
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            temp += c;

            if (temp > 59) throw new DeserializationException("Expected minute portion of timezone offset between 0 and 59", reader);
        }

        public static readonly MethodInfo ReadUInt8 = typeof(Methods).GetMethod("_ReadUInt8", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte _ReadUInt8(TextReader reader)
        {
            // max:  512
            // min:    0
            // digits: 3

            uint ret = 0;

            // first digit *must* exist, we can't overread
            var c = reader.Read();
            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (byte)ret;
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

            AssertNotFollowedByDigit(reader);

            checked
            {
                return (byte)ret;
            }
        }

        public static readonly MethodInfo ReadInt8 = typeof(Methods).GetMethod("_ReadInt8", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static sbyte _ReadInt8(TextReader reader)
        {
            // max:  127
            // min: -127
            // digits: 3

            int ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", reader);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", reader);
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (sbyte)(ret * (negative ? -1 : 1));
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

            AssertNotFollowedByDigit(reader);

            checked
            {
                return (sbyte)(ret * (negative ? -1 : 1));
            }
        }

        public static readonly MethodInfo ReadInt16 = typeof(Methods).GetMethod("_ReadInt16", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static short _ReadInt16(TextReader reader)
        {
            // max:  32767
            // min: -32768
            // digits:   5

            int ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", reader);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", reader);
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (short)(ret * (negative ? -1 : 1));
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

            AssertNotFollowedByDigit(reader);

            checked
            {
                return (short)(ret * (negative ? -1 : 1));
            }
        }

        public static readonly MethodInfo ReadUInt16 = typeof(Methods).GetMethod("_ReadUInt16", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ushort _ReadUInt16(TextReader reader)
        {
            // max: 65535
            // min:     0
            // digits:  5

            uint ret = 0;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit", reader);

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (ushort)ret;
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

            AssertNotFollowedByDigit(reader);

            checked
            {
                return (ushort)ret;
            }
        }

        public static readonly MethodInfo ReadInt32 = typeof(Methods).GetMethod("_ReadInt32", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _ReadInt32(TextReader reader)
        {
            // max:  2147483647
            // min: -2147483648
            // digits:       10

            long ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", reader);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", reader);
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (int)(ret * (negative ? -1 : 1));
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

            AssertNotFollowedByDigit(reader);

            checked
            {
                return (int)(ret * (negative ? -1 : 1));
            }
        }

        public static readonly MethodInfo ReadUInt32 = typeof(Methods).GetMethod("_ReadUInt32", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint _ReadUInt32(TextReader reader)
        {
            // max: 4294967295
            // min:          0
            // digits:      10

            ulong ret = 0;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit", reader);

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return (uint)ret;
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

            AssertNotFollowedByDigit(reader);

            checked
            {
                return (uint)ret;
            }
        }

        public static readonly MethodInfo ReadInt64 = typeof(Methods).GetMethod("_ReadInt64", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long _ReadInt64(TextReader reader)
        {
            // max:  9223372036854775807
            // min: -9223372036854775808
            // digits:                19

            ulong ret = 0;
            var negative = false;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit or '-'", reader);

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit", reader);
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ((long)ret * (negative ? -1 : 1));
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

            AssertNotFollowedByDigit(reader);

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

        public static readonly MethodInfo ReadUInt64 = typeof(Methods).GetMethod("_ReadUInt64", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong _ReadUInt64(TextReader reader)
        {
            // max: 18446744073709551615
            // min:                    0
            // digits:                20

            ulong ret = 0;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit", reader);

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit", reader);
            ret += (uint)c;

            // digit #2
            c = reader.Peek();
            c = c - '0';
            if (c < 0 || c > 9) return ret;
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

            AssertNotFollowedByDigit(reader);

            // ulong special case, we can overflow in the last **addition* instead of the last cast
            checked
            {
                ret *= 10;
                ret += (uint)c;

                return ret;
            }
        }

        public static readonly MethodInfo ReadDouble = typeof(Methods).GetMethod("_ReadDouble", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double _ReadDouble(TextReader reader, ref StringBuilder commonSb)
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
                            throw new DeserializationException("Unexpected +", reader);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", reader);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, reader);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", reader);
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

            var result = double.Parse(commonSb.ToString(), CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        public static readonly MethodInfo ReadDoubleCustom = typeof(Methods).GetMethod("_ReadDoubleCustom", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double _ReadDoubleCustom(TextReader reader, ref CustomStringBuilder commonSb)
        {
            commonSb = commonSb ?? new CustomStringBuilder();

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
                            throw new DeserializationException("Unexpected +", reader);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", reader);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, reader);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", reader);
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

            var result = double.Parse(commonSb.ToString(), CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        public static readonly MethodInfo ReadSingle = typeof(Methods).GetMethod("_ReadSingle", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float _ReadSingle(TextReader reader, ref StringBuilder commonSb)
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
                            throw new DeserializationException("Unexpected +", reader);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", reader);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, reader);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", reader);
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

            var result = float.Parse(commonSb.ToString(), CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        public static readonly MethodInfo ReadSingleCustom = typeof(Methods).GetMethod("_ReadSingleCustom", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float _ReadSingleCustom(TextReader reader, ref CustomStringBuilder commonSb)
        {
            commonSb = commonSb ?? new CustomStringBuilder();

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
                            throw new DeserializationException("Unexpected +", reader);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", reader);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, reader);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", reader);
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

            var result = float.Parse(commonSb.ToString(), CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        public static readonly MethodInfo ReadDecimal = typeof(Methods).GetMethod("_ReadDecimal", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static decimal _ReadDecimal(TextReader reader, ref StringBuilder commonSb)
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
                            throw new DeserializationException("Unexpected +", reader);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", reader);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, reader);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", reader);
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

            var result = decimal.Parse(commonSb.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        public static readonly MethodInfo ReadDecimalCustom = typeof(Methods).GetMethod("_ReadDecimalCustom", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static decimal _ReadDecimalCustom(TextReader reader, ref CustomStringBuilder commonSb)
        {
            commonSb = commonSb ?? new CustomStringBuilder();

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
                            throw new DeserializationException("Unexpected +", reader);
                        }

                        goto storeChar;
                    }

                    var isMinus = c == '-';
                    if (isMinus)
                    {
                        if (prev != -1 && !(prev == 'e' || prev == 'E'))
                        {
                            throw new DeserializationException("Unexpected -", reader);
                        }

                        goto storeChar;
                    }

                    var isE = c == 'e' || c == 'E';
                    if (isE)
                    {
                        if (afterE || !afterFirstDigit)
                        {
                            throw new DeserializationException("Unexpected " + c, reader);
                        }

                        afterE = true;
                        goto storeChar;
                    }

                    var isDot = c == '.';
                    if (isDot)
                    {
                        if (!afterFirstDigit || afterE || afterDot)
                        {
                            throw new DeserializationException("Unexpected .", reader);
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

            var result = decimal.Parse(commonSb.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
            commonSb.Clear();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AssertNotFollowedByDigit(TextReader reader)
        {
            var next = reader.Peek();

            if (next >= '0' && next <= '9')
            {
                throw new DeserializationException(new OverflowException("Number did not end when expected, may overflow"), reader);
            }
        }
    }
}
