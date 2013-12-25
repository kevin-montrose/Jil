using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static class Methods
    {
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

        internal static readonly MethodInfo WriteGuid = typeof(Methods).GetMethod("_WriteGuid", BindingFlags.NonPublic | BindingFlags.Static);
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

        internal static readonly MethodInfo CustomISO8601ToString = typeof(Methods).GetMethod("_CustomISO8601ToString", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _CustomISO8601ToString(TextWriter writer, DateTime dt, char[] buffer)
        {
            // "yyyy-mm-ddThh:mm:ssZ"
            // 0123456789ABCDEFGHIJKL
            //
            // Yes, DateTime.Max is in fact guaranteed to have a 4 digit year (and no more)

            buffer[0] = '"';

            dt = dt.ToUniversalTime();

            int ix, val;
            
            // Year
            val = dt.Year;
            ix = val % 10;
            val /= 10;
            buffer[4] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[3] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[2] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[1] = (char)('0' + ix);

            // delimiter
            buffer[5] = '-';

            // Month
            val = dt.Month;
            ix = val % 10;
            val /= 10;
            buffer[7] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[6] = (char)('0' + ix);

            // Delimiter
            buffer[8] = '-';

            // Day
            val = dt.Day;
            ix = val % 10;
            val /= 10;
            buffer[10] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[9] = (char)('0' + ix);

            // Delimiter
            buffer[11] = 'T';

            val = dt.Hour;
            ix = val % 10;
            val /= 10;
            buffer[13] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[12] = (char)('0' + ix);

            // Delimiter
            buffer[14] = ':';

            val = dt.Minute;
            ix = val % 10;
            val /= 10;
            buffer[16] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[15] = (char)('0' + ix);

            // Delimiter
            buffer[17] = ':';

            val = dt.Second;
            ix = val % 10;
            val /= 10;
            buffer[19] = (char)('0' + ix);
            ix = val % 10;
            val /= 10;
            buffer[18] = (char)('0' + ix);

            buffer[20] = 'Z';
            buffer[21] = '"';

            writer.Write(buffer, 0, 22);
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

            writer.Write("\"");
        }

        internal static readonly MethodInfo WriteEncodedStringWithQuotesWithoutNullsInlineJSONP = typeof(Methods).GetMethod("_WriteEncodedStringWithQuotesWithoutNullsInlineJSONP", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithQuotesWithoutNullsInlineJSONP(TextWriter writer, string str)
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

            writer.Write("\"");
        }

        internal static readonly MethodInfo WriteEncodedStringWithQuotesWithNullsInlineJSONP = typeof(Methods).GetMethod("_WriteEncodedStringWithQuotesWithNullsInlineJSONP", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithQuotesWithNullsInlineJSONP(TextWriter writer, string str)
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

        internal static readonly MethodInfo WriteEncodedStringWithoutNullsInlineJSONP = typeof(Methods).GetMethod("_WriteEncodedStringWithoutNullsInlineJSONP", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithoutNullsInlineJSONP(TextWriter writer, string str)
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

        internal static readonly MethodInfo WriteEncodedStringWithNullsInlineJSONP = typeof(Methods).GetMethod("_WriteEncodedStringWithNullsInlineJSONP", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _WriteEncodedStringWithNullsInlineJSONP(TextWriter writer, string str)
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

        internal static readonly MethodInfo SwitchWriteInt = typeof(Methods).GetMethod("_SwitchWriteInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool _SwitchWriteInt(TextWriter writer, int number)
        {
            switch (number)
            {
                case 0: writer.Write("0"); return true;
                case 1: writer.Write("1"); return true;
                case 2: writer.Write("2"); return true;
                case 3: writer.Write("3"); return true;
                case 4: writer.Write("4"); return true;
                case 5: writer.Write("5"); return true;
                case 6: writer.Write("6"); return true;
                case 7: writer.Write("7"); return true;
                case 8: writer.Write("8"); return true;
                case 9: writer.Write("9"); return true;
                case 10: writer.Write("10"); return true;
                case 11: writer.Write("11"); return true;
                case 12: writer.Write("12"); return true;
                case 13: writer.Write("13"); return true;
                case 14: writer.Write("14"); return true;
                case 15: writer.Write("15"); return true;
                case 16: writer.Write("16"); return true;
                case 17: writer.Write("17"); return true;
                case 18: writer.Write("18"); return true;
                case 19: writer.Write("19"); return true;
                case 20: writer.Write("20"); return true;
                case 21: writer.Write("21"); return true;
                case 22: writer.Write("22"); return true;
                case 23: writer.Write("23"); return true;
                case 24: writer.Write("24"); return true;
                case 25: writer.Write("25"); return true;
                case 26: writer.Write("26"); return true;
                case 27: writer.Write("27"); return true;
                case 28: writer.Write("28"); return true;
                case 29: writer.Write("29"); return true;
                case 30: writer.Write("30"); return true;
                case 31: writer.Write("31"); return true;
                case 32: writer.Write("32"); return true;
                case 33: writer.Write("33"); return true;
                case 34: writer.Write("34"); return true;
                case 35: writer.Write("35"); return true;
                case 36: writer.Write("36"); return true;
                case 37: writer.Write("37"); return true;
                case 38: writer.Write("38"); return true;
                case 39: writer.Write("39"); return true;
                case 40: writer.Write("40"); return true;
                case 41: writer.Write("41"); return true;
                case 42: writer.Write("42"); return true;
                case 43: writer.Write("43"); return true;
                case 44: writer.Write("44"); return true;
                case 45: writer.Write("45"); return true;
                case 46: writer.Write("46"); return true;
                case 47: writer.Write("47"); return true;
                case 48: writer.Write("48"); return true;
                case 49: writer.Write("49"); return true;
                case 50: writer.Write("50"); return true;
                case 51: writer.Write("51"); return true;
                case 52: writer.Write("52"); return true;
                case 53: writer.Write("53"); return true;
                case 54: writer.Write("54"); return true;
                case 55: writer.Write("55"); return true;
                case 56: writer.Write("56"); return true;
                case 57: writer.Write("57"); return true;
                case 58: writer.Write("58"); return true;
                case 59: writer.Write("59"); return true;
                case 60: writer.Write("60"); return true;
                case 61: writer.Write("61"); return true;
                case 62: writer.Write("62"); return true;
                case 63: writer.Write("63"); return true;
                case 64: writer.Write("64"); return true;
                case 65: writer.Write("65"); return true;
                case 66: writer.Write("66"); return true;
                case 67: writer.Write("67"); return true;
                case 68: writer.Write("68"); return true;
                case 69: writer.Write("69"); return true;
                case 70: writer.Write("70"); return true;
                case 71: writer.Write("71"); return true;
                case 72: writer.Write("72"); return true;
                case 73: writer.Write("73"); return true;
                case 74: writer.Write("74"); return true;
                case 75: writer.Write("75"); return true;
                case 76: writer.Write("76"); return true;
                case 77: writer.Write("77"); return true;
                case 78: writer.Write("78"); return true;
                case 79: writer.Write("79"); return true;
                case 80: writer.Write("80"); return true;
                case 81: writer.Write("81"); return true;
                case 82: writer.Write("82"); return true;
                case 83: writer.Write("83"); return true;
                case 84: writer.Write("84"); return true;
                case 85: writer.Write("85"); return true;
                case 86: writer.Write("86"); return true;
                case 87: writer.Write("87"); return true;
                case 88: writer.Write("88"); return true;
                case 89: writer.Write("89"); return true;
                case 90: writer.Write("90"); return true;
                case 91: writer.Write("91"); return true;
                case 92: writer.Write("92"); return true;
                case 93: writer.Write("93"); return true;
                case 94: writer.Write("94"); return true;
                case 95: writer.Write("95"); return true;
                case 96: writer.Write("96"); return true;
                case 97: writer.Write("97"); return true;
                case 98: writer.Write("98"); return true;
                case 99: writer.Write("99"); return true;
                case 100: writer.Write("100"); return true;
                default: return false;
            }
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
