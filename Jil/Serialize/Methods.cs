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
        static char[] WriteGuidLookup =
            new char[]
            {
                '0', '0', // 0
                '0', '1', // 1
                '0', '2', // 2
                '0', '3', // 3
                '0', '4', // 4
                '0', '5', // 5
                '0', '6', // 6
                '0', '7', // 7
                '0', '8', // 8
                '0', '9', // 9
                '0', 'a', // 10
                '0', 'b', // 11
                '0', 'c', // 12
                '0', 'd', // 13
                '0', 'e', // 14
                '0', 'f', // 15
                '1', '0', // 16
                '1', '1', // 17
                '1', '2', // 18
                '1', '3', // 19
                '1', '4', // 20
                '1', '5', // 21
                '1', '6', // 22
                '1', '7', // 23
                '1', '8', // 24
                '1', '9', // 25
                '1', 'a', // 26
                '1', 'b', // 27
                '1', 'c', // 28
                '1', 'd', // 29
                '1', 'e', // 30
                '1', 'f', // 31
                '2', '0', // 32
                '2', '1', // 33
                '2', '2', // 34
                '2', '3', // 35
                '2', '4', // 36
                '2', '5', // 37
                '2', '6', // 38
                '2', '7', // 39
                '2', '8', // 40
                '2', '9', // 41
                '2', 'a', // 42
                '2', 'b', // 43
                '2', 'c', // 44
                '2', 'd', // 45
                '2', 'e', // 46
                '2', 'f', // 47
                '3', '0', // 48
                '3', '1', // 49
                '3', '2', // 50
                '3', '3', // 51
                '3', '4', // 52
                '3', '5', // 53
                '3', '6', // 54
                '3', '7', // 55
                '3', '8', // 56
                '3', '9', // 57
                '3', 'a', // 58
                '3', 'b', // 59
                '3', 'c', // 60
                '3', 'd', // 61
                '3', 'e', // 62
                '3', 'f', // 63
                '4', '0', // 64
                '4', '1', // 65
                '4', '2', // 66
                '4', '3', // 67
                '4', '4', // 68
                '4', '5', // 69
                '4', '6', // 70
                '4', '7', // 71
                '4', '8', // 72
                '4', '9', // 73
                '4', 'a', // 74
                '4', 'b', // 75
                '4', 'c', // 76
                '4', 'd', // 77
                '4', 'e', // 78
                '4', 'f', // 79
                '5', '0', // 80
                '5', '1', // 81
                '5', '2', // 82
                '5', '3', // 83
                '5', '4', // 84
                '5', '5', // 85
                '5', '6', // 86
                '5', '7', // 87
                '5', '8', // 88
                '5', '9', // 89
                '5', 'a', // 90
                '5', 'b', // 91
                '5', 'c', // 92
                '5', 'd', // 93
                '5', 'e', // 94
                '5', 'f', // 95
                '6', '0', // 96
                '6', '1', // 97
                '6', '2', // 98
                '6', '3', // 99
                '6', '4', // 100
                '6', '5', // 101
                '6', '6', // 102
                '6', '7', // 103
                '6', '8', // 104
                '6', '9', // 105
                '6', 'a', // 106
                '6', 'b', // 107
                '6', 'c', // 108
                '6', 'd', // 109
                '6', 'e', // 110
                '6', 'f', // 111
                '7', '0', // 112
                '7', '1', // 113
                '7', '2', // 114
                '7', '3', // 115
                '7', '4', // 116
                '7', '5', // 117
                '7', '6', // 118
                '7', '7', // 119
                '7', '8', // 120
                '7', '9', // 121
                '7', 'a', // 122
                '7', 'b', // 123
                '7', 'c', // 124
                '7', 'd', // 125
                '7', 'e', // 126
                '7', 'f', // 127
                '8', '0', // 128
                '8', '1', // 129
                '8', '2', // 130
                '8', '3', // 131
                '8', '4', // 132
                '8', '5', // 133
                '8', '6', // 134
                '8', '7', // 135
                '8', '8', // 136
                '8', '9', // 137
                '8', 'a', // 138
                '8', 'b', // 139
                '8', 'c', // 140
                '8', 'd', // 141
                '8', 'e', // 142
                '8', 'f', // 143
                '9', '0', // 144
                '9', '1', // 145
                '9', '2', // 146
                '9', '3', // 147
                '9', '4', // 148
                '9', '5', // 149
                '9', '6', // 150
                '9', '7', // 151
                '9', '8', // 152
                '9', '9', // 153
                '9', 'a', // 154
                '9', 'b', // 155
                '9', 'c', // 156
                '9', 'd', // 157
                '9', 'e', // 158
                '9', 'f', // 159
                'a', '0', // 160
                'a', '1', // 161
                'a', '2', // 162
                'a', '3', // 163
                'a', '4', // 164
                'a', '5', // 165
                'a', '6', // 166
                'a', '7', // 167
                'a', '8', // 168
                'a', '9', // 169
                'a', 'a', // 170
                'a', 'b', // 171
                'a', 'c', // 172
                'a', 'd', // 173
                'a', 'e', // 174
                'a', 'f', // 175
                'b', '0', // 176
                'b', '1', // 177
                'b', '2', // 178
                'b', '3', // 179
                'b', '4', // 180
                'b', '5', // 181
                'b', '6', // 182
                'b', '7', // 183
                'b', '8', // 184
                'b', '9', // 185
                'b', 'a', // 186
                'b', 'b', // 187
                'b', 'c', // 188
                'b', 'd', // 189
                'b', 'e', // 190
                'b', 'f', // 191
                'c', '0', // 192
                'c', '1', // 193
                'c', '2', // 194
                'c', '3', // 195
                'c', '4', // 196
                'c', '5', // 197
                'c', '6', // 198
                'c', '7', // 199
                'c', '8', // 200
                'c', '9', // 201
                'c', 'a', // 202
                'c', 'b', // 203
                'c', 'c', // 204
                'c', 'd', // 205
                'c', 'e', // 206
                'c', 'f', // 207
                'd', '0', // 208
                'd', '1', // 209
                'd', '2', // 210
                'd', '3', // 211
                'd', '4', // 212
                'd', '5', // 213
                'd', '6', // 214
                'd', '7', // 215
                'd', '8', // 216
                'd', '9', // 217
                'd', 'a', // 218
                'd', 'b', // 219
                'd', 'c', // 220
                'd', 'd', // 221
                'd', 'e', // 222
                'd', 'f', // 223
                'e', '0', // 224
                'e', '1', // 225
                'e', '2', // 226
                'e', '3', // 227
                'e', '4', // 228
                'e', '5', // 229
                'e', '6', // 230
                'e', '7', // 231
                'e', '8', // 232
                'e', '9', // 233
                'e', 'a', // 234
                'e', 'b', // 235
                'e', 'c', // 236
                'e', 'd', // 237
                'e', 'e', // 238
                'e', 'f', // 239
                'f', '0', // 240
                'f', '1', // 241
                'f', '2', // 242
                'f', '3', // 243
                'f', '4', // 244
                'f', '5', // 245
                'f', '6', // 246
                'f', '7', // 247
                'f', '8', // 248
                'f', '9', // 249
                'f', 'a', // 250
                'f', 'b', // 251
                'f', 'c', // 252
                'f', 'd', // 253
                'f', 'e', // 254
                'f', 'f', // 255
            };

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

            // ToByteArray() returns the bytes in a different order than you might expect
            // For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0 
            // Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0 
            // Ix:   0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
            //
            // And then we have to account for dashes
            //
            // So the map is like so:
            // bytes[0]  -> chars[3]  -> buffer[ 6, 7]
            // bytes[1]  -> chars[2]  -> buffer[ 4, 5]
            // bytes[2]  -> chars[1]  -> buffer[ 2, 3]
            // bytes[3]  -> chars[0]  -> buffer[ 0, 1]
            // bytes[4]  -> chars[5]  -> buffer[10,11] -> [11,12]
            // bytes[5]  -> chars[4]  -> buffer[ 8, 9] -> [ 9,10]
            // bytes[6]  -> chars[7]  -> buffer[14,15] -> [15,16] -> [16,17]
            // bytes[7]  -> chars[6]  -> buffer[12,13] -> [13,14] -> [14,15]
            // bytes[8]  -> chars[8]  -> buffer[16,17] -> [17,18] -> [18,19] -> [19,20]
            // bytes[9]  -> chars[9]  -> buffer[18,19] -> [19,20] -> [20,21] -> [21,22]
            // bytes[10] -> chars[10] -> buffer[20,21] -> [21,22] -> [22,23] -> [23,24] -> [24,25]
            // bytes[11] -> chars[11] -> buffer[22,23] -> [23,24] -> [24,25] -> [25,26] -> [26,27]
            // bytes[12] -> chars[12] -> buffer[24,25] -> [25,26] -> [26,27] -> [27,28] -> [28,29]
            // bytes[13] -> chars[13] -> buffer[26,27] -> [27,28] -> [28,29] -> [29,30] -> [30,31]
            // bytes[14] -> chars[14] -> buffer[28,29] -> [29,30] -> [30,31] -> [31,32] -> [32,33]
            // bytes[15] -> chars[15] -> buffer[30,31] -> [31,32] -> [32,33] -> [33,34] -> [34,35]
            var bytes = guid.ToByteArray();

            // bytes[0] -> chars[3]
            var b = bytes[0] * 2;
            buffer[6] = WriteGuidLookup[b];
            buffer[7] = WriteGuidLookup[b + 1];

            // bytes[1] -> chars[2]
            b = bytes[1] * 2;
            buffer[4] = WriteGuidLookup[b];
            buffer[5] = WriteGuidLookup[b + 1];

            // bytes[2] -> chars[1]
            b = bytes[2] * 2;
            buffer[2] = WriteGuidLookup[b];
            buffer[3] = WriteGuidLookup[b + 1];

            // bytes[3] -> chars[0]
            b = bytes[3] * 2;
            buffer[0] = WriteGuidLookup[b];
            buffer[1] = WriteGuidLookup[b + 1];

            // bytes[4] -> chars[5]
            b = bytes[4] * 2;
            buffer[11] = WriteGuidLookup[b];
            buffer[12] = WriteGuidLookup[b + 1];

            // bytes[5] -> chars[4]
            b = bytes[5] * 2;
            buffer[9] = WriteGuidLookup[b];
            buffer[10] = WriteGuidLookup[b + 1];

            // bytes[6] -> chars[7]
            b = bytes[6] * 2;
            buffer[16] = WriteGuidLookup[b];
            buffer[17] = WriteGuidLookup[b + 1];

            // bytes[7] -> chars[6]
            b = bytes[7] * 2;
            buffer[14] = WriteGuidLookup[b];
            buffer[15] = WriteGuidLookup[b + 1];

            // bytes[8] -> chars[8]
            b = bytes[8] * 2;
            buffer[19] = WriteGuidLookup[b];
            buffer[20] = WriteGuidLookup[b + 1];

            // bytes[9] -> chars[9]
            b = bytes[9] * 2;
            buffer[21] = WriteGuidLookup[b];
            buffer[22] = WriteGuidLookup[b + 1];

            // bytes[10] -> chars[10]
            b = bytes[10] * 2;
            buffer[24] = WriteGuidLookup[b];
            buffer[25] = WriteGuidLookup[b + 1];

            // bytes[11] -> chars[11]
            b = bytes[11] * 2;
            buffer[26] = WriteGuidLookup[b];
            buffer[27] = WriteGuidLookup[b + 1];

            // bytes[12] -> chars[12]
            b = bytes[12] * 2;
            buffer[28] = WriteGuidLookup[b];
            buffer[29] = WriteGuidLookup[b + 1];

            // bytes[13] -> chars[13]
            b = bytes[13] * 2;
            buffer[30] = WriteGuidLookup[b];
            buffer[31] = WriteGuidLookup[b + 1];

            // bytes[14] -> chars[14]
            b = bytes[14] * 2;
            buffer[32] = WriteGuidLookup[b];
            buffer[33] = WriteGuidLookup[b + 1];

            // bytes[15] -> chars[15]
            b = bytes[15] * 2;
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
