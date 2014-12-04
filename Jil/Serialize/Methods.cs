using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static partial class Methods
    {
        struct TwoDigits
        {
            public readonly char First;
            public readonly char Second;

            public TwoDigits(char first, char second)
            {
                First = first;
                Second = second;
            }
        }

        private static readonly TwoDigits[] DigitPairs;
        private static readonly char[] DigitTriplets;
        static Methods()
        {
            DigitPairs = new TwoDigits[100];
            for (var i = 0; i < 100; ++i)
            {
                DigitPairs[i] = new TwoDigits((char)('0' + (i / 10)), (char)+('0' + (i % 10)));
            }

            DigitTriplets = new char[1000 * 3];
            for (var i = 0; i < 1000; ++i)
            {
                DigitTriplets[i * 3 + 0] = (char)('0' + i / 100 % 10);
                DigitTriplets[i * 3 + 1] = (char)('0' + i / 10 % 10);
                DigitTriplets[i * 3 + 2] = (char)('0' + i % 10);
            }
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct GuidStruct
        {
            [FieldOffset(0)]
            private Guid Value;

            [FieldOffset(0)]
            public readonly byte B00;
            [FieldOffset(1)]
            public readonly byte B01;
            [FieldOffset(2)]
            public readonly byte B02;
            [FieldOffset(3)]
            public readonly byte B03;
            [FieldOffset(4)]
            public readonly byte B04;
            [FieldOffset(5)]
            public readonly byte B05;

            [FieldOffset(6)]
            public readonly byte B06;
            [FieldOffset(7)]
            public readonly byte B07;
            [FieldOffset(8)]
            public readonly byte B08;
            [FieldOffset(9)]
            public readonly byte B09;

            [FieldOffset(10)]
            public readonly byte B10;
            [FieldOffset(11)]
            public readonly byte B11;

            [FieldOffset(12)]
            public readonly byte B12;
            [FieldOffset(13)]
            public readonly byte B13;
            [FieldOffset(14)]
            public readonly byte B14;
            [FieldOffset(15)]
            public readonly byte B15;

            public GuidStruct(Guid invisibleMembers)
                : this()
            {
                Value = invisibleMembers;
            }
        }

        static readonly char[] WriteGuidLookup = new char[] { '0', '0', '0', '1', '0', '2', '0', '3', '0', '4', '0', '5', '0', '6', '0', '7', '0', '8', '0', '9', '0', 'a', '0', 'b', '0', 'c', '0', 'd', '0', 'e', '0', 'f', '1', '0', '1', '1', '1', '2', '1', '3', '1', '4', '1', '5', '1', '6', '1', '7', '1', '8', '1', '9', '1', 'a', '1', 'b', '1', 'c', '1', 'd', '1', 'e', '1', 'f', '2', '0', '2', '1', '2', '2', '2', '3', '2', '4', '2', '5', '2', '6', '2', '7', '2', '8', '2', '9', '2', 'a', '2', 'b', '2', 'c', '2', 'd', '2', 'e', '2', 'f', '3', '0', '3', '1', '3', '2', '3', '3', '3', '4', '3', '5', '3', '6', '3', '7', '3', '8', '3', '9', '3', 'a', '3', 'b', '3', 'c', '3', 'd', '3', 'e', '3', 'f', '4', '0', '4', '1', '4', '2', '4', '3', '4', '4', '4', '5', '4', '6', '4', '7', '4', '8', '4', '9', '4', 'a', '4', 'b', '4', 'c', '4', 'd', '4', 'e', '4', 'f', '5', '0', '5', '1', '5', '2', '5', '3', '5', '4', '5', '5', '5', '6', '5', '7', '5', '8', '5', '9', '5', 'a', '5', 'b', '5', 'c', '5', 'd', '5', 'e', '5', 'f', '6', '0', '6', '1', '6', '2', '6', '3', '6', '4', '6', '5', '6', '6', '6', '7', '6', '8', '6', '9', '6', 'a', '6', 'b', '6', 'c', '6', 'd', '6', 'e', '6', 'f', '7', '0', '7', '1', '7', '2', '7', '3', '7', '4', '7', '5', '7', '6', '7', '7', '7', '8', '7', '9', '7', 'a', '7', 'b', '7', 'c', '7', 'd', '7', 'e', '7', 'f', '8', '0', '8', '1', '8', '2', '8', '3', '8', '4', '8', '5', '8', '6', '8', '7', '8', '8', '8', '9', '8', 'a', '8', 'b', '8', 'c', '8', 'd', '8', 'e', '8', 'f', '9', '0', '9', '1', '9', '2', '9', '3', '9', '4', '9', '5', '9', '6', '9', '7', '9', '8', '9', '9', '9', 'a', '9', 'b', '9', 'c', '9', 'd', '9', 'e', '9', 'f', 'a', '0', 'a', '1', 'a', '2', 'a', '3', 'a', '4', 'a', '5', 'a', '6', 'a', '7', 'a', '8', 'a', '9', 'a', 'a', 'a', 'b', 'a', 'c', 'a', 'd', 'a', 'e', 'a', 'f', 'b', '0', 'b', '1', 'b', '2', 'b', '3', 'b', '4', 'b', '5', 'b', '6', 'b', '7', 'b', '8', 'b', '9', 'b', 'a', 'b', 'b', 'b', 'c', 'b', 'd', 'b', 'e', 'b', 'f', 'c', '0', 'c', '1', 'c', '2', 'c', '3', 'c', '4', 'c', '5', 'c', '6', 'c', '7', 'c', '8', 'c', '9', 'c', 'a', 'c', 'b', 'c', 'c', 'c', 'd', 'c', 'e', 'c', 'f', 'd', '0', 'd', '1', 'd', '2', 'd', '3', 'd', '4', 'd', '5', 'd', '6', 'd', '7', 'd', '8', 'd', '9', 'd', 'a', 'd', 'b', 'd', 'c', 'd', 'd', 'd', 'e', 'd', 'f', 'e', '0', 'e', '1', 'e', '2', 'e', '3', 'e', '4', 'e', '5', 'e', '6', 'e', '7', 'e', '8', 'e', '9', 'e', 'a', 'e', 'b', 'e', 'c', 'e', 'd', 'e', 'e', 'e', 'f', 'f', '0', 'f', '1', 'f', '2', 'f', '3', 'f', '4', 'f', '5', 'f', '6', 'f', '7', 'f', '8', 'f', '9', 'f', 'a', 'f', 'b', 'f', 'c', 'f', 'd', 'f', 'e', 'f', 'f' };

        static readonly MethodInfo WriteGuid = typeof(Methods).GetMethod("_WriteGuid", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteGuid(TextWriter writer, Guid guid, char[] buffer)
        {
            // 1314FAD4-7505-439D-ABD2-DBD89242928C
            // 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
            //
            // Guid is guaranteed to be a 36 character string

            // get all the dashes in place
            buffer[8] = '-';
            buffer[13] = '-';
            buffer[18] = '-';
            buffer[23] = '-';

            // Bytes are in a different order than you might expect
            // For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0 
            // Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0 
            // Ix:   0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
            //
            // And we have to account for dashes
            //
            // So the map is like so:
            // bytes[0]  -> chars[3]  -> buffer[ 6, 7]
            // bytes[1]  -> chars[2]  -> buffer[ 4, 5]
            // bytes[2]  -> chars[1]  -> buffer[ 2, 3]
            // bytes[3]  -> chars[0]  -> buffer[ 0, 1]
            // bytes[4]  -> chars[5]  -> buffer[11,12]
            // bytes[5]  -> chars[4]  -> buffer[ 9,10]
            // bytes[6]  -> chars[7]  -> buffer[16,17]
            // bytes[7]  -> chars[6]  -> buffer[14,15]
            // bytes[8]  -> chars[8]  -> buffer[19,20]
            // bytes[9]  -> chars[9]  -> buffer[21,22]
            // bytes[10] -> chars[10] -> buffer[24,25]
            // bytes[11] -> chars[11] -> buffer[26,27]
            // bytes[12] -> chars[12] -> buffer[28,29]
            // bytes[13] -> chars[13] -> buffer[30,31]
            // bytes[14] -> chars[14] -> buffer[32,33]
            // bytes[15] -> chars[15] -> buffer[34,35]
            var visibleMembers = new GuidStruct(guid);

            // bytes[0]
            var b = visibleMembers.B00 * 2;
            buffer[6] = WriteGuidLookup[b];
            buffer[7] = WriteGuidLookup[b + 1];

            // bytes[1]
            b = visibleMembers.B01 * 2;
            buffer[4] = WriteGuidLookup[b];
            buffer[5] = WriteGuidLookup[b + 1];

            // bytes[2]
            b = visibleMembers.B02 * 2;
            buffer[2] = WriteGuidLookup[b];
            buffer[3] = WriteGuidLookup[b + 1];

            // bytes[3]
            b = visibleMembers.B03 * 2;
            buffer[0] = WriteGuidLookup[b];
            buffer[1] = WriteGuidLookup[b + 1];

            // bytes[4]
            b = visibleMembers.B04 * 2;
            buffer[11] = WriteGuidLookup[b];
            buffer[12] = WriteGuidLookup[b + 1];

            // bytes[5]
            b = visibleMembers.B05 * 2;
            buffer[9] = WriteGuidLookup[b];
            buffer[10] = WriteGuidLookup[b + 1];

            // bytes[6]
            b = visibleMembers.B06 * 2;
            buffer[16] = WriteGuidLookup[b];
            buffer[17] = WriteGuidLookup[b + 1];

            // bytes[7]
            b = visibleMembers.B07 * 2;
            buffer[14] = WriteGuidLookup[b];
            buffer[15] = WriteGuidLookup[b + 1];

            // bytes[8]
            b = visibleMembers.B08 * 2;
            buffer[19] = WriteGuidLookup[b];
            buffer[20] = WriteGuidLookup[b + 1];

            // bytes[9]
            b = visibleMembers.B09 * 2;
            buffer[21] = WriteGuidLookup[b];
            buffer[22] = WriteGuidLookup[b + 1];

            // bytes[10]
            b = visibleMembers.B10 * 2;
            buffer[24] = WriteGuidLookup[b];
            buffer[25] = WriteGuidLookup[b + 1];

            // bytes[11]
            b = visibleMembers.B11 * 2;
            buffer[26] = WriteGuidLookup[b];
            buffer[27] = WriteGuidLookup[b + 1];

            // bytes[12]
            b = visibleMembers.B12 * 2;
            buffer[28] = WriteGuidLookup[b];
            buffer[29] = WriteGuidLookup[b + 1];

            // bytes[13]
            b = visibleMembers.B13 * 2;
            buffer[30] = WriteGuidLookup[b];
            buffer[31] = WriteGuidLookup[b + 1];

            // bytes[14]
            b = visibleMembers.B14 * 2;
            buffer[32] = WriteGuidLookup[b];
            buffer[33] = WriteGuidLookup[b + 1];

            // bytes[15]
            b = visibleMembers.B15 * 2;
            buffer[34] = WriteGuidLookup[b];
            buffer[35] = WriteGuidLookup[b + 1];

            writer.Write(buffer, 0, 36);
        }

        static readonly MethodInfo CustomISO8601ToString = typeof(Methods).GetMethod("_CustomISO8601ToString", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomISO8601ToString(TextWriter writer, DateTime dt, char[] buffer)
        {
            // "yyyy-mm-ddThh:mm:ss.fffffffZ"
            // 0123456789ABCDEFGHIJKL
            //
            // Yes, DateTime.Max is in fact guaranteed to have a 4 digit year (and no more)
            // f of 7 digits allows for 1 Tick level resolution

            buffer[0] = '"';

            dt = dt.ToUniversalTime();

            uint val;

            // Year
            val = (uint)dt.Year;
            var digits = DigitPairs[(byte)(val % 100)];
            buffer[4] = digits.Second;
            buffer[3] = digits.First;
            digits = DigitPairs[(byte)(val / 100)];
            buffer[2] = digits.Second;
            buffer[1] = digits.First;

            // delimiter
            buffer[5] = '-';

            // Month
            digits = DigitPairs[dt.Month];
            buffer[7] = digits.Second;
            buffer[6] = digits.First;

            // Delimiter
            buffer[8] = '-';

            // Day
            digits = DigitPairs[dt.Day];
            buffer[10] = digits.Second;
            buffer[9] = digits.First;

            // Delimiter
            buffer[11] = 'T';

            digits = DigitPairs[dt.Hour];
            buffer[13] = digits.Second;
            buffer[12] = digits.First;

            // Delimiter
            buffer[14] = ':';

            digits = DigitPairs[dt.Minute];
            buffer[16] = digits.Second;
            buffer[15] = digits.First;

            // Delimiter
            buffer[17] = ':';

            digits = DigitPairs[dt.Second];
            buffer[19] = digits.Second;
            buffer[18] = digits.First;

            int fracEnd;
            var remainingTicks = (dt - new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second)).Ticks;
            if (remainingTicks > 0)
            {
                buffer[20] = '.';

                var fracPart = remainingTicks % 100;
                remainingTicks /= 100;
                if (fracPart > 0)
                {
                    digits = DigitPairs[fracPart];
                    buffer[27] = digits.Second;
                    buffer[26] = digits.First;
                    fracEnd = 28;
                }
                else
                {
                    fracEnd = 26;
                }

                fracPart = remainingTicks % 100;
                remainingTicks /= 100;
                if (fracPart > 0)
                {
                    digits = DigitPairs[fracPart];
                    buffer[25] = digits.Second;
                    buffer[24] = digits.First;
                }
                else
                {
                    if (fracEnd == 26)
                    {
                        fracEnd = 24;
                    }
                    else
                    {
                        buffer[25] = '0';
                        buffer[24] = '0';
                    }
                }

                fracPart = remainingTicks % 100;
                remainingTicks /= 100;
                if (fracPart > 0)
                {
                    digits = DigitPairs[fracPart];
                    buffer[23] = digits.Second;
                    buffer[22] = digits.First;
                }
                else
                {
                    if (fracEnd == 24)
                    {
                        fracEnd = 22;
                    }
                    else
                    {
                        buffer[23] = '0';
                        buffer[22] = '0';
                    }
                }

                fracPart = remainingTicks;
                buffer[21] = (char)('0' + fracPart);
            }
            else
            {
                fracEnd = 20;
            }

            buffer[fracEnd] = 'Z';
            buffer[fracEnd + 1] = '"';

            writer.Write(buffer, 0, fracEnd + 2);
        }

        static readonly MethodInfo WriteEncodedStringWithQuotesWithNullsInlineUnsafe = typeof(Methods).GetMethod("_WriteEncodedStringWithQuotesWithNullsInlineUnsafe", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void _WriteEncodedStringWithQuotesWithNullsInlineUnsafe(TextWriter writer, string strRef)
        {
            if (strRef == null)
            {
                writer.Write("null");
                return;
            }

            writer.Write("\"");

            fixed (char* strFixed = strRef)
            {
                char* str = strFixed;
                char c;
                var len = strRef.Length;

                while (len > 0)
                {
                    c = *str;
                    str++;
                    len--;

                    if (c == '\\')
                    {
                        writer.Write(@"\\");
                        continue;
                    }

                    if (c == '"')
                    {
                        writer.Write("\\\"");
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

            writer.Write("\"");
        }

        static readonly MethodInfo WriteEncodedStringWithQuotesWithNullsInlineJSONPUnsafe = typeof(Methods).GetMethod("_WriteEncodedStringWithQuotesWithNullsInlineJSONPUnsafe", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void _WriteEncodedStringWithQuotesWithNullsInlineJSONPUnsafe(TextWriter writer, string strRef)
        {
            if (strRef == null)
            {
                writer.Write("null");
                return;
            }

            writer.Write("\"");

            fixed (char* strFixed = strRef)
            {
                char* str = strFixed;
                char c;
                var len = strRef.Length;

                while (len > 0)
                {
                    c = *str;
                    str++;
                    len--;

                    if (c == '\\')
                    {
                        writer.Write(@"\\");
                        continue;
                    }

                    if (c == '"')
                    {
                        writer.Write("\\\"");
                        continue;
                    }

                    if (c == '\u2028')
                    {
                        writer.Write(@"\u2028");
                        continue;
                    }

                    if (c == '\u2029')
                    {
                        writer.Write(@"\u2029");
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

            writer.Write("\"");
        }
        
        static readonly MethodInfo WriteEncodedStringWithNullsInlineJSONPUnsafe = typeof(Methods).GetMethod("_WriteEncodedStringWithNullsInlineJSONPUnsafe", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void _WriteEncodedStringWithNullsInlineJSONPUnsafe(TextWriter writer, string strRef)
        {
            if (strRef == null)
            {
                writer.Write("null");
                return;
            }

            fixed (char* strFixed = strRef)
            {
                char* str = strFixed;
                char c;
                var len = strRef.Length;

                while (len > 0)
                {
                    c = *str;
                    str++;
                    len--;

                    if (c == '\\')
                    {
                        writer.Write(@"\\");
                        continue;
                    }

                    if (c == '"')
                    {
                        writer.Write("\\\"");
                        continue;
                    }

                    if (c == '\u2028')
                    {
                        writer.Write(@"\u2028");
                        continue;
                    }

                    if (c == '\u2029')
                    {
                        writer.Write(@"\u2029");
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
        }

        static readonly MethodInfo WriteEncodedStringWithNullsInlineUnsafe = typeof(Methods).GetMethod("_WriteEncodedStringWithNullsInlineUnsafe", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void _WriteEncodedStringWithNullsInlineUnsafe(TextWriter writer, string strRef)
        {
            if (strRef == null)
            {
                writer.Write("null");
                return;
            }

            fixed (char* strFixed = strRef)
            {
                char* str = strFixed;
                char c;
                var len = strRef.Length;

                while (len > 0)
                {
                    c = *str;
                    str++;
                    len--;

                    if (c == '\\')
                    {
                        writer.Write(@"\\");
                        continue;
                    }

                    if (c == '"')
                    {
                        writer.Write("\\\"");
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
        }

        static readonly MethodInfo CustomWriteInt = typeof(Methods).GetMethod("_CustomWriteInt", BindingFlags.Static | BindingFlags.NonPublic);
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

            uint copy;
            if (number < 0)
            {
                writer.Write('-');
                copy = (uint)(-number);
            }
            else
            {
                copy = (uint)number;
            }

            do
            {
                byte ix = (byte)(copy % 100);
                copy /= 100;

                var chars = DigitPairs[ix];
                buffer[ptr--] = chars.Second;
                buffer[ptr--] = chars.First;
            } while (copy != 0);

            if (buffer[ptr + 1] == '0')
            {
                ptr++;
            }

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }

        static readonly MethodInfo CustomWriteIntUnrolledSigned = typeof(Methods).GetMethod("_CustomWriteIntUnrolledSigned", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteIntUnrolledSigned(TextWriter writer, int num, char[] buffer)
        {
            // Why signed integers?
            // Earlier versions of this code used unsigned integers, 
            //   but it turns out that's not ideal and here's why.
            // 
            // The signed version of the relevant code gets JIT'd down to:
            // instr       operands                    latency/throughput (approx. worst case; Haswell)
            // ========================================================================================
            // mov         ecx,###                      2 /  0.5 
            // cdq                                      1 /  -
            // idiv        eax,ecx                     29 / 11
            // mov         ecx,###                      2 /  0.5
            // cdq                                      1 /  -
            // idiv        eax,ecx                     29 / 11
            // movsx       edx,dl                       - /  0.5
            //
            // The unsigned version gets JIT'd down to:
            // instr       operands                    latency/throughput (approx. worst case; Haswell)
            // ========================================================================================
            // mov         ecx,###                       2 /  0.5
            // xor         edx,edx                       1 /  0.25
            // div         eax,ecx                      29 / 11
            // mov         ecx,###                       2 /  0.5
            // xor         edx,edx                       1 /  0.25
            // div         eax,ecx                      29 / 11
            // and         edx,###                       1 /  0.25
            //
            // In theory div (usigned division) is faster than idiv, and it probably is *but* cdq + cdq + movsx is
            //   faster than xor + xor + and; in practice it's fast *enough* to make up the difference.
            // have to special case this, we can't negate it
            if (num == int.MinValue)
            {
                writer.Write("-2147483648");
                return;
            }

            int numLen;
            int number;

            if (num < 0) 
            {
                writer.Write('-');
                number = -num;
            }
            else 
            {
                number = num;
            }

            if (number < 1000)
            {
                if (number >= 100)
                {
                    writer.Write(DigitTriplets, number * 3, 3);
                }
                else
                {
                    if (number >= 10)
                    {
                        writer.Write(DigitTriplets, number * 3 + 1, 2);
                    }
                    else
                    {
                        writer.Write(DigitTriplets, number * 3 + 2, 1);
                    }
                }
                return;
            }
            var d012 = number % 1000 * 3;

            int d543;
            if (number < 1000000)
            {
                d543 = (number / 1000) * 3;
                if (number >= 100000)
                {
                    numLen = 6;
                    goto digit5;
                }
                else
                {
                    if (number >= 10000)
                    {
                        numLen = 5;
                        goto digit4;
                    }
                    else
                    {
                        numLen = 4;
                        goto digit3;
                    }
                }
            }
            d543 = (number / 1000) % 1000 * 3;

            int d876;
            if (number < 1000000000)
            {
                d876 = (number / 1000000) * 3;
                if (number >= 100000000)
                {
                    numLen = 9;
                    goto digit8;
                }
                else
                {
                    if (number >= 10000000)
                    {
                        numLen = 8;
                        goto digit7;
                    }
                    else
                    {
                        numLen = 7;
                        goto digit6;
                    }
                }
            }
            d876 = (number / 1000000) % 1000 * 3;

            numLen = 10;

            // uint is between 0 & 4,294,967,295 (in practice we only get to int.MaxValue, but that's the same # of digits)
            // so 1 to 10 digits

            // [01,]000,000-[99,]000,000
            var d9 = number / 1000000000;
            buffer[0] = (char)('0' + d9);
            
            digit8:
            buffer[1] = DigitTriplets[d876];
            digit7:
            buffer[2] = DigitTriplets[d876 + 1];
            digit6:
            buffer[3] = DigitTriplets[d876 + 2];

            digit5:
            buffer[4] = DigitTriplets[d543];
            digit4:
            buffer[5] = DigitTriplets[d543 + 1];
            digit3:
            buffer[6] = DigitTriplets[d543 + 2];

            buffer[7] = DigitTriplets[d012];
            buffer[8] = DigitTriplets[d012 + 1];
            buffer[9] = DigitTriplets[d012 + 2];

            writer.Write(buffer, 10 - numLen, numLen);
        }

        static readonly MethodInfo CustomWriteUInt = typeof(Methods).GetMethod("_CustomWriteUInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteUInt(TextWriter writer, uint number, char[] buffer)
        {
            var ptr = InlineSerializer<object>.CharBufferSize - 1;

            var copy = number;

            do
            {
                byte ix = (byte)(copy % 100);
                copy /= 100;

                var chars = DigitPairs[ix];
                buffer[ptr--] = chars.Second;
                buffer[ptr--] = chars.First;
            } while (copy != 0);

            if (buffer[ptr + 1] == '0')
            {
                ptr++;
            }

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }

        static readonly MethodInfo CustomWriteUIntUnrolled = typeof(Methods).GetMethod("_CustomWriteUIntUnrolled", BindingFlags.Static | BindingFlags.NonPublic);
        static void _CustomWriteUIntUnrolled(TextWriter writer, uint number, char[] buffer)
        {
            int numLen;

            if (number < 1000)
            {
                if (number >= 100)
                {
                    writer.Write(DigitTriplets, (int)(number * 3), 3);
                }
                else
                {
                    if (number >= 10)
                    {
                        writer.Write(DigitTriplets, (int)(number * 3 + 1), 2);
                    }
                    else
                    {
                        writer.Write(DigitTriplets, (int)(number * 3 + 2), 1);
                    }
                }
                return;
            }
            var d012 = number % 1000 * 3;

            uint d543;
            if (number < 1000000)
            {
                d543 = (number / 1000) * 3;
                if (number >= 100000)
                {
                    numLen = 6;
                    goto digit5;
                }
                else
                {
                    if (number >= 10000)
                    {
                        numLen = 5;
                        goto digit4;
                    }
                    else
                    {
                        numLen = 4;
                        goto digit3;
                    }
                }
            }
            d543 = (number / 1000) % 1000 * 3;

            uint d876;
            if (number < 1000000000)
            {
                d876 = (number / 1000000) * 3;
                if (number >= 100000000)
                {
                    numLen = 9;
                    goto digit8;
                }
                else
                {
                    if (number >= 10000000)
                    {
                        numLen = 8;
                        goto digit7;
                    }
                    else
                    {
                        numLen = 7;
                        goto digit6;
                    }
                }
            }
            d876 = (number / 1000000) % 1000 * 3;

            numLen = 10;

            // uint is between 0 & 4,294,967,295 (in practice we only get to int.MaxValue, but that's the same # of digits)
            // so 1 to 10 digits

            // [01,]000,000-[99,]000,000
            var d9 = number / 1000000000;
            buffer[0] = (char)('0' + d9);
            
            digit8:
            buffer[1] = DigitTriplets[d876];
            digit7:
            buffer[2] = DigitTriplets[d876 + 1];
            digit6:
            buffer[3] = DigitTriplets[d876 + 2];

            digit5:
            buffer[4] = DigitTriplets[d543];
            digit4:
            buffer[5] = DigitTriplets[d543 + 1];
            digit3:
            buffer[6] = DigitTriplets[d543 + 2];

            buffer[7] = DigitTriplets[d012];
            buffer[8] = DigitTriplets[d012 + 1];
            buffer[9] = DigitTriplets[d012 + 2];

            writer.Write(buffer, 10 - numLen, numLen);
        }

        static readonly MethodInfo CustomWriteLong = typeof(Methods).GetMethod("_CustomWriteLong", BindingFlags.Static | BindingFlags.NonPublic);
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

            ulong copy;
            if (number < 0)
            {
                writer.Write('-');
                copy = (ulong)(-number);
            }
            else
            {
                copy = (ulong)number;
            }

            do
            {
                byte ix = (byte)(copy % 100);
                copy /= 100;

                var chars = DigitPairs[ix];
                buffer[ptr--] = chars.Second;
                buffer[ptr--] = chars.First;
            } while (copy != 0);

            if (buffer[ptr + 1] == '0')
            {
                ptr++;
            }

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }
        
        static readonly MethodInfo CustomWriteULong = typeof(Methods).GetMethod("_CustomWriteULong", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomWriteULong(TextWriter writer, ulong number, char[] buffer)
        {
            var ptr = InlineSerializer<object>.CharBufferSize - 1;

            var copy = number;

            do
            {
                byte ix = (byte)(copy % 100);
                copy /= 100;

                var chars = DigitPairs[ix];
                buffer[ptr--] = chars.Second;
                buffer[ptr--] = chars.First;
            } while (copy != 0);

            if (buffer[ptr + 1] == '0')
            {
                ptr++;
            }

            writer.Write(buffer, ptr + 1, InlineSerializer<object>.CharBufferSize - 1 - ptr);
        }

        static readonly MethodInfo ProxyFloat = typeof(Methods).GetMethod("_ProxyFloat", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _ProxyFloat(TextWriter writer, float f)
        {
            var invariant = CultureInfo.InvariantCulture;

            var canUseBuiltIn = writer.FormatProvider == invariant;
            if (canUseBuiltIn)
            {
                writer.Write(f);
                return;
            }

            writer.Write(f.ToString(invariant));
        }

        static readonly MethodInfo ProxyDouble = typeof(Methods).GetMethod("_ProxyDouble", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _ProxyDouble(TextWriter writer, double d)
        {
            var invariant = CultureInfo.InvariantCulture;

            var canUseBuiltIn = writer.FormatProvider == invariant;
            if (canUseBuiltIn)
            {
                writer.Write(d);
                return;
            }

            writer.Write(d.ToString(invariant));
        }

        static readonly MethodInfo ProxyDecimal = typeof(Methods).GetMethod("_ProxyDecimal", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _ProxyDecimal(TextWriter writer, decimal d)
        {
            var invariant = CultureInfo.InvariantCulture;

            var canUseBuiltIn = writer.FormatProvider == invariant;
            if (canUseBuiltIn)
            {
                writer.Write(d);
                return;
            }

            writer.Write(d.ToString(invariant));
        }
    }
}
