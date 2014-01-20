﻿using System;
using System.Collections.Generic;
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
        public static readonly MethodInfo ReadUInt8 = typeof(Methods).GetMethod("_ReadUInt8", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte _ReadUInt8(TextReader reader)
        {
            // max:  512
            // min:    0
            // digits: 3

            uint ret = 0;

            // digit #1
            var c = reader.Read();
            if (c == -1) throw new DeserializationException("Expected digit");

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += (uint)c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (byte)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (byte)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            CheckNumberOverflow(reader);

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
            if (c == -1) throw new DeserializationException("Expected digit or '-'");

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit");
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (sbyte)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (sbyte)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (sbyte)c;

            CheckNumberOverflow(reader);

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
            if (c == -1) throw new DeserializationException("Expected digit or '-'");

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit");
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (short)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (short)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #4
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (short)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #5
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (short)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            CheckNumberOverflow(reader);

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
            if (c == -1) throw new DeserializationException("Expected digit");

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += (uint)c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (ushort)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (ushort)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (ushort)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (ushort)c;

            // digit #5
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (ushort)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (ushort)c;

            CheckNumberOverflow(reader);

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
            if (c == -1) throw new DeserializationException("Expected digit or '-'");

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit");
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #4
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #5
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #6
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #7
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #8
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #9
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            // digit #10
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (int)(ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += c;

            CheckNumberOverflow(reader);

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
            if (c == -1) throw new DeserializationException("Expected digit");

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += (uint)c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #5
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #6
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #7
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #8
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #9
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #10
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return (uint)ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            CheckNumberOverflow(reader);

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
            if (c == -1) throw new DeserializationException("Expected digit or '-'");

            if (c == '-')
            {
                negative = true;
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Expected digit");
            }

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += (uint)c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #5
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #6
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #7
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #8
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #9
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #10
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #11
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #12
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #13
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #14
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #15
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #16
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #17
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #18
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #19
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ((long)ret * (negative ? -1 : 1));

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            CheckNumberOverflow(reader);

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
            if (c == -1) throw new DeserializationException("Expected digit");

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret += (uint)c;

            // digit #2
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #3
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #4
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #5
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #6
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #7
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #8
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #9
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #10
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #11
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #12
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #13
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #14
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #15
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #16
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #17
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #18
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #19
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            ret *= 10;
            ret += (uint)c;

            // digit #20
            c = reader.Read();
            if (c == -1 || IsWhiteSpace(c)) return ret;

            c = c - '0';
            if (c < 0 || c > 9) throw new DeserializationException("Expected digit");
            checked
            {
                ret *= 10;
                ret += (uint)c;
            }

            CheckNumberOverflow(reader);

            return ret;
        }

        public static readonly MethodInfo ReadDouble = typeof(Methods).GetMethod("_ReadDouble", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double _ReadDouble(TextReader reader, StringBuilder commonSb)
        {
            int c;
            bool negative = false;
            bool seenDecimal = false;

            var first = reader.Read();
            if (first == -1) throw new DeserializationException("Expected digit or '-'");

            if (first == '-')
            {
                negative = true;
                first = reader.Read();
                if (first == -1 || IsWhiteSpace(first) || first < '0' || first > '9') throw new DeserializationException("Expected digit");
                commonSb.Append((char)first);
            }
            else
            {
                commonSb.Append((char)first);
            }

            while ((c = reader.Read()) != -1 && !IsWhiteSpace(c))
            {
                if (c >= '0' && c <= '9')
                {
                    commonSb.Append((char)c);
                    continue;
                }

                if (c == '.')
                {
                    if (seenDecimal) throw new DeserializationException("Two decimals in one floating point number");

                    commonSb.Append('.');
                    seenDecimal = true;

                    // there must be a following digit to be valid JSON
                    c = reader.Read();
                    if (c == -1 || IsWhiteSpace(c) || c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    commonSb.Append((char)c);

                    continue;
                }

                // exponent?
                if (c == 'e' || c == 'E')
                {
                    var leading = double.Parse(commonSb.ToString()) * (negative ? -1.0 : 1.0);
                    commonSb.Clear();   // cleanup for the next use

                    var sign = reader.Peek();
                    if (sign == -1 || (sign != '-' && sign != '+' && !(sign >= '0' && sign <= '9'))) throw new DeserializationException("Expected sign or digit");

                    var exponentNegative = (sign == '-');
                    if (sign == '-' || sign == '+') reader.Read();

                    double exp = _ReadUInt64(reader);

                    exp = Math.Pow(10.0, (exp * (exponentNegative ? -1.0 : 1.0)));

                    return leading * exp;
                }
            }

            var ret = double.Parse(commonSb.ToString());
            commonSb.Clear();   // cleanup for the next use

            return ret * (negative ? -1.0 : 1.0);
        }

        public static readonly MethodInfo ReadSingle = typeof(Methods).GetMethod("_ReadSingle", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float _ReadSingle(TextReader reader, StringBuilder commonSb)
        {
            int c;
            bool negative = false;
            bool seenDecimal = false;

            var first = reader.Read();
            if (first == -1) throw new DeserializationException("Expected digit or '-'");

            if (first == '-')
            {
                negative = true;
                first = reader.Read();
                if (first == -1 || IsWhiteSpace(first) || first < '0' || first > '9') throw new DeserializationException("Expected digit");
                commonSb.Append((char)first);
            }
            else
            {
                commonSb.Append((char)first);
            }

            while ((c = reader.Read()) != -1 && !IsWhiteSpace(c))
            {
                if (c >= '0' && c <= '9')
                {
                    commonSb.Append((char)c);
                    continue;
                }

                if (c == '.')
                {
                    if (seenDecimal) throw new DeserializationException("Two decimals in one floating point number");

                    commonSb.Append('.');
                    seenDecimal = true;

                    // there must be a following digit to be valid JSON
                    c = reader.Read();
                    if (c == -1 || IsWhiteSpace(c) || c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    commonSb.Append((char)c);

                    continue;
                }

                // exponent?
                if (c == 'e' || c == 'E')
                {
                    // we're doing the math in doubles intentionally here, for precisions sake
                    var leading = double.Parse(commonSb.ToString()) * (negative ? -1.0 : 1.0);
                    commonSb.Clear();   // cleanup for the next use

                    var sign = reader.Peek();
                    if (sign == -1 || (sign != '-' && sign != '+' && !(sign >= '0' && sign <= '9'))) throw new DeserializationException("Expected sign or digit");

                    var exponentNegative = (sign == '-');
                    if (sign == '-' || sign == '+') reader.Read();

                    double exp = _ReadUInt64(reader);

                    exp = Math.Pow(10.0, (exp * (exponentNegative ? -1.0 : 1.0)));

                    return (float)(leading * exp);
                }
            }

            var ret = float.Parse(commonSb.ToString());
            commonSb.Clear();   // cleanup for the next use

            return ret * (negative ? -1f : 1f);
        }

        public static readonly MethodInfo ReadDecimal = typeof(Methods).GetMethod("_ReadDecimal", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static decimal _ReadDecimal(TextReader reader, StringBuilder commonSb)
        {
            int c;
            bool negative = false;
            bool seenDecimal = false;

            var first = reader.Read();
            if (first == -1) throw new DeserializationException("Expected digit or '-'");

            if (first == '-')
            {
                negative = true;
                first = reader.Read();
                if (first == -1 || IsWhiteSpace(first) || first < '0' || first > '9') throw new DeserializationException("Expected digit");
                commonSb.Append((char)first);
            }
            else
            {
                commonSb.Append((char)first);
            }

            while ((c = reader.Read()) != -1 && !IsWhiteSpace(c))
            {
                if (c >= '0' && c <= '9')
                {
                    commonSb.Append((char)c);
                    continue;
                }

                if (c == '.')
                {
                    if (seenDecimal) throw new DeserializationException("Two decimals in one floating point number");

                    commonSb.Append('.');
                    seenDecimal = true;

                    // there must be a following digit to be valid JSON
                    c = reader.Read();
                    if (c == -1 || IsWhiteSpace(c) || c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    commonSb.Append((char)c);

                    continue;
                }

                // exponent?
                if (c == 'e' || c == 'E')
                {
                    var leading = decimal.Parse(commonSb.ToString()) * (negative ? -1m : 1m);
                    commonSb.Clear();   // cleanup for the next use

                    var sign = reader.Peek();
                    if (sign == -1 || (sign != '-' && sign != '+' && !(sign >= '0' && sign <= '9'))) throw new DeserializationException("Expected sign or digit");

                    var exponentNegative = (sign == '-');
                    if (sign == '-' || sign == '+') reader.Read();

                    decimal exp = _ReadUInt64(reader);

                    exp = (decimal)Math.Pow(10.0, (double)(exp * (exponentNegative ? -1m : 1m)));

                    return leading * exp;
                }
            }

            var ret = decimal.Parse(commonSb.ToString());
            commonSb.Clear();   // cleanup for the next use

            return ret * (negative ? -1m : 1m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CheckNumberOverflow(TextReader reader)
        {
            var next = reader.Peek();

            if (next == -1 || IsWhiteSpace(next)) return;

            throw new OverflowException("Number did not end when expected, may overflow");
        }
    }
}
