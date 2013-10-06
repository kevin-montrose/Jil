using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static class Methods
    {
        internal static readonly MethodInfo CustomWriteIntYear = typeof(Methods).GetMethod("_CustomWriteIntYear", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteIntYear(TextWriter writer, int number, char[] buffer)
        {
            if (number < 1000)
            {
                if (number < 100)
                {
                    if (number < 10)
                    {
                        if (number == 0)
                        {
                            writer.Write("0000");
                            return;
                        }

                        writer.Write("000");
                    }
                    else
                    {
                        writer.Write("00");
                    }
                }
                else
                {
                    writer.Write("0");
                }
            }

            var ptr = InlineSerializer<object>.CharBufferSize - 1;

            var copy = number;
            if (copy < 0)
            {
                copy = -copy;
            }

            do
            {
                var ix = copy % 10;
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }

        internal static readonly MethodInfo CustomWriteIntMonthDay = typeof(Methods).GetMethod("_CustomWriteIntMonthDay", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteIntMonthDay(TextWriter writer, int number, char[] buffer)
        {
            switch (number)
            {
                case 0: writer.Write("00"); return;
                case 1: writer.Write("01"); return;
                case 2: writer.Write("02"); return;
                case 3: writer.Write("03"); return;
                case 4: writer.Write("04"); return;
                case 5: writer.Write("05"); return;
                case 6: writer.Write("06"); return;
                case 7: writer.Write("07"); return;
                case 8: writer.Write("08"); return;
                case 9: writer.Write("09"); return;
                case 10: writer.Write("10"); return;
                case 11: writer.Write("11"); return;
                case 12: writer.Write("12"); return;
                case 13: writer.Write("13"); return;
                case 14: writer.Write("14"); return;
                case 15: writer.Write("15"); return;
                case 16: writer.Write("16"); return;
                case 17: writer.Write("17"); return;
                case 18: writer.Write("18"); return;
                case 19: writer.Write("19"); return;
                case 20: writer.Write("20"); return;
                case 21: writer.Write("21"); return;
                case 22: writer.Write("22"); return;
                case 23: writer.Write("23"); return;
                case 24: writer.Write("24"); return;
                case 25: writer.Write("25"); return;
                case 26: writer.Write("26"); return;
                case 27: writer.Write("27"); return;
                case 28: writer.Write("28"); return;
                case 29: writer.Write("29"); return;
                case 30: writer.Write("30"); return;
                case 31: writer.Write("31"); return;
                case 32: writer.Write("32"); return;
                case 33: writer.Write("33"); return;
                case 34: writer.Write("34"); return;
                case 35: writer.Write("35"); return;
                case 36: writer.Write("36"); return;
                case 37: writer.Write("37"); return;
                case 38: writer.Write("38"); return;
                case 39: writer.Write("39"); return;
                case 40: writer.Write("40"); return;
                case 41: writer.Write("41"); return;
                case 42: writer.Write("42"); return;
                case 43: writer.Write("43"); return;
                case 44: writer.Write("44"); return;
                case 45: writer.Write("45"); return;
                case 46: writer.Write("46"); return;
                case 47: writer.Write("47"); return;
                case 48: writer.Write("48"); return;
                case 49: writer.Write("49"); return;
                case 50: writer.Write("50"); return;
                case 51: writer.Write("51"); return;
                case 52: writer.Write("52"); return;
                case 53: writer.Write("53"); return;
                case 54: writer.Write("54"); return;
                case 55: writer.Write("55"); return;
                case 56: writer.Write("56"); return;
                case 57: writer.Write("57"); return;
                case 58: writer.Write("58"); return;
                case 59: writer.Write("59"); return;
                case 60: writer.Write("60"); return;
                case 61: writer.Write("61"); return;    // Leap seconds!
            }
        }

        internal static readonly MethodInfo WriteEncodedStringWithQuotesWithoutNullsInline = typeof(Methods).GetMethod("_WriteEncodedStringWithQuotesWithoutNullsInline", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithQuotesWithoutNullsInline(TextWriter writer, string str)
        {
            if (str == null) return;

            writer.Write("\"");

            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '\\')
                {
                    writer.Write(@"\\");
                    continue;
                }

                if (c == '"')
                {
                    writer.Write("\"");
                    continue;
                }

                // This is converted into an IL switch, so don't fret about lookup times
                switch (c)
                {
                    case '\u0000': writer.Write(@"\u0000"); continue;
                    case '\u0001': writer.Write(@"\u0001"); continue;
                    case '\u0002': writer.Write(@"\u0002"); continue;
                    case '\u0003': writer.Write(@"\u0003"); continue;
                    case '\u0004': writer.Write(@"\u0004"); continue;
                    case '\u0005': writer.Write(@"\u0005"); continue;
                    case '\u0006': writer.Write(@"\u0006"); continue;
                    case '\u0007': writer.Write(@"\u0007"); continue;
                    case '\u0008': writer.Write(@"\b"); continue;
                    case '\u0009': writer.Write(@"\t"); continue;
                    case '\u000A': writer.Write(@"\n"); continue;
                    case '\u000B': writer.Write(@"\u000B"); continue;
                    case '\u000C': writer.Write(@"\f"); continue;
                    case '\u000D': writer.Write(@"\r"); continue;
                    case '\u000E': writer.Write(@"\u000E"); continue;
                    case '\u000F': writer.Write(@"\u000F"); continue;
                    case '\u0010': writer.Write(@"\u0010"); continue;
                    case '\u0011': writer.Write(@"\u0011"); continue;
                    case '\u0012': writer.Write(@"\u0012"); continue;
                    case '\u0013': writer.Write(@"\u0013"); continue;
                    case '\u0014': writer.Write(@"\u0014"); continue;
                    case '\u0015': writer.Write(@"\u0015"); continue;
                    case '\u0016': writer.Write(@"\u0016"); continue;
                    case '\u0017': writer.Write(@"\u0017"); continue;
                    case '\u0018': writer.Write(@"\u0018"); continue;
                    case '\u0019': writer.Write(@"\u0019"); continue;
                    case '\u001A': writer.Write(@"\u001A"); continue;
                    case '\u001B': writer.Write(@"\u001B"); continue;
                    case '\u001C': writer.Write(@"\u001C"); continue;
                    case '\u001D': writer.Write(@"\u001D"); continue;
                    case '\u001E': writer.Write(@"\u001E"); continue;
                    case '\u001F': writer.Write(@"\u001F"); continue;
                    default: writer.Write(c); continue;
                }
            }

            writer.Write("\"");
        }

        internal static readonly MethodInfo WriteEncodedStringWithQuotesWithNullsInline = typeof(Methods).GetMethod("_WriteEncodedStringWithQuotesWithNullsInline", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithQuotesWithNullsInline(TextWriter writer, string str)
        {
            if (str == null)
            {
                writer.Write("null");
                return;
            }

            writer.Write("\"");

            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '\\')
                {
                    writer.Write(@"\\");
                    continue;
                }

                if (c == '"')
                {
                    writer.Write("\"");
                    continue;
                }

                // This is converted into an IL switch, so don't fret about lookup times
                switch (c)
                {
                    case '\u0000': writer.Write(@"\u0000"); continue;
                    case '\u0001': writer.Write(@"\u0001"); continue;
                    case '\u0002': writer.Write(@"\u0002"); continue;
                    case '\u0003': writer.Write(@"\u0003"); continue;
                    case '\u0004': writer.Write(@"\u0004"); continue;
                    case '\u0005': writer.Write(@"\u0005"); continue;
                    case '\u0006': writer.Write(@"\u0006"); continue;
                    case '\u0007': writer.Write(@"\u0007"); continue;
                    case '\u0008': writer.Write(@"\b"); continue;
                    case '\u0009': writer.Write(@"\t"); continue;
                    case '\u000A': writer.Write(@"\n"); continue;
                    case '\u000B': writer.Write(@"\u000B"); continue;
                    case '\u000C': writer.Write(@"\f"); continue;
                    case '\u000D': writer.Write(@"\r"); continue;
                    case '\u000E': writer.Write(@"\u000E"); continue;
                    case '\u000F': writer.Write(@"\u000F"); continue;
                    case '\u0010': writer.Write(@"\u0010"); continue;
                    case '\u0011': writer.Write(@"\u0011"); continue;
                    case '\u0012': writer.Write(@"\u0012"); continue;
                    case '\u0013': writer.Write(@"\u0013"); continue;
                    case '\u0014': writer.Write(@"\u0014"); continue;
                    case '\u0015': writer.Write(@"\u0015"); continue;
                    case '\u0016': writer.Write(@"\u0016"); continue;
                    case '\u0017': writer.Write(@"\u0017"); continue;
                    case '\u0018': writer.Write(@"\u0018"); continue;
                    case '\u0019': writer.Write(@"\u0019"); continue;
                    case '\u001A': writer.Write(@"\u001A"); continue;
                    case '\u001B': writer.Write(@"\u001B"); continue;
                    case '\u001C': writer.Write(@"\u001C"); continue;
                    case '\u001D': writer.Write(@"\u001D"); continue;
                    case '\u001E': writer.Write(@"\u001E"); continue;
                    case '\u001F': writer.Write(@"\u001F"); continue;
                    default: writer.Write(c); continue;
                }
            }

            writer.Write("\"");
        }

        internal static readonly MethodInfo WriteEncodedStringWithoutNullsInline = typeof(Methods).GetMethod("_WriteEncodedStringWithoutNullsInline", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithoutNullsInline(TextWriter writer, string str)
        {
            if (str == null) return;

            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '\\')
                {
                    writer.Write(@"\\");
                    continue;
                }

                if (c == '"')
                {
                    writer.Write("\"");
                    continue;
                }

                // This is converted into an IL switch, so don't fret about lookup times
                switch (c)
                {
                    case '\u0000': writer.Write(@"\u0000"); continue;
                    case '\u0001': writer.Write(@"\u0001"); continue;
                    case '\u0002': writer.Write(@"\u0002"); continue;
                    case '\u0003': writer.Write(@"\u0003"); continue;
                    case '\u0004': writer.Write(@"\u0004"); continue;
                    case '\u0005': writer.Write(@"\u0005"); continue;
                    case '\u0006': writer.Write(@"\u0006"); continue;
                    case '\u0007': writer.Write(@"\u0007"); continue;
                    case '\u0008': writer.Write(@"\b"); continue;
                    case '\u0009': writer.Write(@"\t"); continue;
                    case '\u000A': writer.Write(@"\n"); continue;
                    case '\u000B': writer.Write(@"\u000B"); continue;
                    case '\u000C': writer.Write(@"\f"); continue;
                    case '\u000D': writer.Write(@"\r"); continue;
                    case '\u000E': writer.Write(@"\u000E"); continue;
                    case '\u000F': writer.Write(@"\u000F"); continue;
                    case '\u0010': writer.Write(@"\u0010"); continue;
                    case '\u0011': writer.Write(@"\u0011"); continue;
                    case '\u0012': writer.Write(@"\u0012"); continue;
                    case '\u0013': writer.Write(@"\u0013"); continue;
                    case '\u0014': writer.Write(@"\u0014"); continue;
                    case '\u0015': writer.Write(@"\u0015"); continue;
                    case '\u0016': writer.Write(@"\u0016"); continue;
                    case '\u0017': writer.Write(@"\u0017"); continue;
                    case '\u0018': writer.Write(@"\u0018"); continue;
                    case '\u0019': writer.Write(@"\u0019"); continue;
                    case '\u001A': writer.Write(@"\u001A"); continue;
                    case '\u001B': writer.Write(@"\u001B"); continue;
                    case '\u001C': writer.Write(@"\u001C"); continue;
                    case '\u001D': writer.Write(@"\u001D"); continue;
                    case '\u001E': writer.Write(@"\u001E"); continue;
                    case '\u001F': writer.Write(@"\u001F"); continue;
                    default: writer.Write(c); continue;
                }
            }
        }

        internal static readonly MethodInfo WriteEncodedStringWithNullsInline = typeof(Methods).GetMethod("_WriteEncodedStringWithNullsInline", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithNullsInline(TextWriter writer, string str)
        {
            if (str == null)
            {
                writer.Write("null");
                return;
            }

            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '\\')
                {
                    writer.Write(@"\\");
                    continue;
                }

                if (c == '"')
                {
                    writer.Write("\"");
                    continue;
                }

                // This is converted into an IL switch, so don't fret about lookup times
                switch (c)
                {
                    case '\u0000': writer.Write(@"\u0000"); continue;
                    case '\u0001': writer.Write(@"\u0001"); continue;
                    case '\u0002': writer.Write(@"\u0002"); continue;
                    case '\u0003': writer.Write(@"\u0003"); continue;
                    case '\u0004': writer.Write(@"\u0004"); continue;
                    case '\u0005': writer.Write(@"\u0005"); continue;
                    case '\u0006': writer.Write(@"\u0006"); continue;
                    case '\u0007': writer.Write(@"\u0007"); continue;
                    case '\u0008': writer.Write(@"\b"); continue;
                    case '\u0009': writer.Write(@"\t"); continue;
                    case '\u000A': writer.Write(@"\n"); continue;
                    case '\u000B': writer.Write(@"\u000B"); continue;
                    case '\u000C': writer.Write(@"\f"); continue;
                    case '\u000D': writer.Write(@"\r"); continue;
                    case '\u000E': writer.Write(@"\u000E"); continue;
                    case '\u000F': writer.Write(@"\u000F"); continue;
                    case '\u0010': writer.Write(@"\u0010"); continue;
                    case '\u0011': writer.Write(@"\u0011"); continue;
                    case '\u0012': writer.Write(@"\u0012"); continue;
                    case '\u0013': writer.Write(@"\u0013"); continue;
                    case '\u0014': writer.Write(@"\u0014"); continue;
                    case '\u0015': writer.Write(@"\u0015"); continue;
                    case '\u0016': writer.Write(@"\u0016"); continue;
                    case '\u0017': writer.Write(@"\u0017"); continue;
                    case '\u0018': writer.Write(@"\u0018"); continue;
                    case '\u0019': writer.Write(@"\u0019"); continue;
                    case '\u001A': writer.Write(@"\u001A"); continue;
                    case '\u001B': writer.Write(@"\u001B"); continue;
                    case '\u001C': writer.Write(@"\u001C"); continue;
                    case '\u001D': writer.Write(@"\u001D"); continue;
                    case '\u001E': writer.Write(@"\u001E"); continue;
                    case '\u001F': writer.Write(@"\u001F"); continue;
                    default: writer.Write(c); continue;
                }
            }
        }

        internal static readonly MethodInfo CustomWriteInt = typeof(Methods).GetMethod("_CustomWriteInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteInt(TextWriter writer, int number, char[] buffer)
        {
            // Gotta special case this, we can't negate it
            if (number == int.MinValue)
            {
                writer.Write("-2147483648");
                return;
            }

            var ptr = InlineSerializer<object>.CharBufferSize - 1;

            var copy = number;
            if (copy < 0)
            {
                copy = -copy;
            }

            do
            {
                var ix = copy % 10;
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            if (number < 0)
            {
                buffer[ptr] = '-';
                ptr--;
            }

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }

        internal static readonly MethodInfo CustomWriteUInt = typeof(Methods).GetMethod("_CustomWriteUInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteUInt(TextWriter writer, uint number, char[] buffer)
        {
            var ptr = InlineSerializer<object>.CharBufferSize - 1;

            var copy = number;

            do
            {
                var ix = copy % 10;
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }

        internal static readonly MethodInfo CustomWriteLong = typeof(Methods).GetMethod("_CustomWriteLong", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteLong(TextWriter writer, long number, char[] buffer)
        {
            // Gotta special case this, we can't negate it
            if (number == long.MinValue)
            {
                writer.Write("-9223372036854775808");
                return;
            }

            var ptr = InlineSerializer<object>.CharBufferSize - 1;

            var copy = number;
            if (copy < 0)
            {
                copy = -copy;
            }

            do
            {
                var ix = (int)(copy % 10);
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            if (number < 0)
            {
                buffer[ptr] = '-';
                ptr--;
            }

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }

        internal static readonly MethodInfo CustomWriteULong = typeof(Methods).GetMethod("_CustomWriteULong", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteULong(TextWriter writer, ulong number, char[] buffer)
        {
            var ptr = InlineSerializer<object>.CharBufferSize - 1;

            var copy = number;

            do
            {
                var ix = (int)(copy % 10);
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }
    }
}
