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
        internal static readonly MethodInfo InlineSerializer_WriteEncodedStringWithQuotesInline = typeof(Methods).GetMethod("WriteEncodedStringWithQuotesInline", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void WriteEncodedStringWithQuotesInline(TextWriter writer, string str)
        {
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

        internal static readonly MethodInfo InlineSerializer_WriteEncodedStringInline = typeof(Methods).GetMethod("WriteEncodedStringInline", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void WriteEncodedStringInline(TextWriter writer, string str)
        {
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

        internal static readonly MethodInfo InlineSerializer_CustomWriteInt = typeof(Methods).GetMethod("CustomWriteInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteInt(TextWriter writer, int number, char[] buffer)
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

        internal static readonly MethodInfo InlineSerializer_CustomWriteUInt = typeof(Methods).GetMethod("CustomWriteUInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteUInt(TextWriter writer, uint number, char[] buffer)
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

        internal static readonly MethodInfo InlineSerializer_CustomWriteLong = typeof(Methods).GetMethod("CustomWriteLong", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteLong(TextWriter writer, long number, char[] buffer)
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

        internal static readonly MethodInfo InlineSerializer_CustomWriteULong = typeof(Methods).GetMethod("CustomWriteULong", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteULong(TextWriter writer, ulong number, char[] buffer)
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
