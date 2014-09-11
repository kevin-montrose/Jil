using Jil.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static partial class Methods
    {
        public const int CharBufferSize = 33;

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct GuidStruct
        {
            [FieldOffset(0)]
            public readonly Guid Value;

            [FieldOffset(0)]
            public byte B00;
            [FieldOffset(1)]
            public byte B01;
            [FieldOffset(2)]
            public byte B02;
            [FieldOffset(3)]
            public byte B03;
            [FieldOffset(4)]
            public byte B04;
            [FieldOffset(5)]
            public byte B05;

            [FieldOffset(6)]
            public byte B06;
            [FieldOffset(7)]
            public byte B07;
            [FieldOffset(8)]
            public byte B08;
            [FieldOffset(9)]
            public byte B09;

            [FieldOffset(10)]
            public byte B10;
            [FieldOffset(11)]
            public byte B11;

            [FieldOffset(12)]
            public byte B12;
            [FieldOffset(13)]
            public byte B13;
            [FieldOffset(14)]
            public byte B14;
            [FieldOffset(15)]
            public byte B15;
        }

        public static readonly MethodInfo ReadGuid = typeof(Methods).GetMethod("_ReadGuid", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Guid _ReadGuid(TextReader reader)
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
            asStruct.B03 = ReadGuidByte(reader);
            asStruct.B02 = ReadGuidByte(reader);
            asStruct.B01 = ReadGuidByte(reader);
            asStruct.B00 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -", reader);

            asStruct.B05 = ReadGuidByte(reader);
            asStruct.B04 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -", reader);

            asStruct.B07 = ReadGuidByte(reader);
            asStruct.B06 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -", reader);

            asStruct.B08 = ReadGuidByte(reader);
            asStruct.B09 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -", reader);

            asStruct.B10 = ReadGuidByte(reader);
            asStruct.B11 = ReadGuidByte(reader);
            asStruct.B12 = ReadGuidByte(reader);
            asStruct.B13 = ReadGuidByte(reader);
            asStruct.B14 = ReadGuidByte(reader);
            asStruct.B15 = ReadGuidByte(reader);

            return asStruct.Value;
        }

        static byte ReadGuidByte(TextReader reader)
        {
            var a = reader.Read();
            if (a == -1) throw new DeserializationException("Expected any character", reader);
            if (!((a >= '0' && a <= '9') || (a >= 'A' && a <= 'F') || (a >= 'a' && a <= 'f'))) throw new DeserializationException("Expected a hex number", reader);
            var b = reader.Read();
            if (b == -1) throw new DeserializationException("Expected any character", reader);
            if (!((b >= '0' && b <= '9') || (b >= 'A' && b <= 'F') || (b >= 'a' && b <= 'f'))) throw new DeserializationException("Expected a hex number", reader);

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

        public static readonly MethodInfo Skip = typeof(Methods).GetMethod("_Skip", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _Skip(TextReader reader)
        {
            SkipWithLeadChar(reader, reader.Read());
        }

        static void SkipWithLeadChar(TextReader reader, int leadChar)
        {
            // skip null
            if (leadChar == 'n')
            {
                if (reader.Read() != 'u') throw new DeserializationException("Expected u", reader);
                if (reader.Read() != 'l') throw new DeserializationException("Expected l", reader);
                if (reader.Read() != 'l') throw new DeserializationException("Expected l", reader);
                return;
            }

            // skip a string
            if (leadChar == '"')
            {
                SkipEncodedStringWithLeadChar(reader, leadChar);
                return;
            }

            // skip an object
            if (leadChar == '{')
            {
                SkipObject(reader, leadChar);
                return;
            }

            // skip a list
            if (leadChar == '[')
            {
                SkipList(reader, leadChar);
                return;
            }

            // skip a number
            if ((leadChar >= '0' && leadChar <= '9') || leadChar == '-')
            {
                SkipNumber(reader, leadChar);
                return;
            }

            // skip false
            if (leadChar == 'f')
            {
                if (reader.Read() != 'a') throw new DeserializationException("Expected a", reader);
                if (reader.Read() != 'l') throw new DeserializationException("Expected l", reader);
                if (reader.Read() != 's') throw new DeserializationException("Expected s", reader);
                if (reader.Read() != 'e') throw new DeserializationException("Expected e", reader);
                return;
            }

            // skip true
            if (leadChar == 't')
            {
                if (reader.Read() != 'r') throw new DeserializationException("Expected r", reader);
                if (reader.Read() != 'u') throw new DeserializationException("Expected u", reader);
                if (reader.Read() != 'e') throw new DeserializationException("Expected e", reader);
                return;
            }

            throw new DeserializationException("Expected digit, -, \", {, n, t, f, or [", reader);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipObject(TextReader reader, int leadChar)
        {
            if (leadChar != '{') throw new DeserializationException("Expected {", reader);

            int c;

            c = _ReadSkipWhitespace(reader);
            if (c == '}')
            {
                return;
            }
            SkipEncodedStringWithLeadChar(reader, c);
            c = _ReadSkipWhitespace(reader);
            if (c != ':') throw new DeserializationException("Expected :", reader);
            c = _ReadSkipWhitespace(reader);
            SkipWithLeadChar(reader, c);

            while (true)
            {
                c = _ReadSkipWhitespace(reader);
                if (c == '}') return;
                if (c != ',') throw new DeserializationException("Expected ,", reader);

                c = _ReadSkipWhitespace(reader);
                SkipEncodedStringWithLeadChar(reader, c);
                c = _ReadSkipWhitespace(reader);
                if (c != ':') throw new DeserializationException("Expected :", reader);
                c = _ReadSkipWhitespace(reader);
                SkipWithLeadChar(reader, c);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipList(TextReader reader, int leadChar)
        {
            if (leadChar != '[') throw new DeserializationException("Expected [", reader);

            int c;

            c = _ReadSkipWhitespace(reader);
            if (c == ']')
            {
                return;
            }
            SkipWithLeadChar(reader, c);

            while (true)
            {
                c = _ReadSkipWhitespace(reader);
                if (c == ']') return;
                if (c != ',') throw new DeserializationException("Expected ], or ,", reader);
                c = _ReadSkipWhitespace(reader);
                SkipWithLeadChar(reader, c);
            }
        }

        public static MethodInfo SkipEncodedString = typeof(Methods).GetMethod("_SkipEncodedString", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _SkipEncodedString(TextReader reader)
        {
            SkipEncodedStringWithLeadChar(reader, reader.Read());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipEncodedStringWithLeadChar(TextReader reader, int leadChar)
        {
            if (leadChar != '"') throw new DeserializationException("Expected \"", reader);

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character", reader);

                // we didn't have to use anything but the buffer, make a string and return it!
                if (first == '"')
                {
                    return;
                }

                if (first != '\\')
                {
                    continue;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);

                switch (second)
                {
                    case '"': continue;
                    case '\\': continue;
                    case '/': continue;
                    case 'b': continue;
                    case 'f': continue;
                    case 'n': continue;
                    case 'r': continue;
                    case 't': continue;
                }

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                var u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') || (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit", reader);
                u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') || (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit", reader);
                u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') || (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit", reader);
                u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') || (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit", reader);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipNumber(TextReader reader, int leadChar)
        {
            // leadChar should be a start of the number

            var seenDecimal = false;
            var seenExponent = false;

            while (true)
            {
                var c = reader.Peek();

                if (c >= '0' && c <= '9')
                {
                    reader.Read();  // skip the digit
                    continue;
                }

                if (c == '.' && !seenDecimal)
                {
                    reader.Read();      // skip the decimal
                    seenDecimal = true;
                    continue;
                }

                if ((c == 'e' || c == 'E') && !seenExponent)
                {
                    reader.Read();      // skip the decimal
                    seenExponent = true;
                    seenDecimal = true;

                    var next = reader.Peek();
                    if (next == '-' || next == '+' || (next >= '0' && next <= '9'))
                    {
                        reader.Read();
                        continue;
                    }

                    throw new DeserializationException("Expected -, or a digit", reader);
                }

                return;
            }
        }

        public static readonly MethodInfo ConsumeWhiteSpace = typeof(Methods).GetMethod("_ConsumeWhiteSpace", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _ConsumeWhiteSpace(TextReader reader)
        {
            int c;
            while ((c = reader.Peek()) != -1)
            {
                if (!IsWhiteSpace(c)) return;

                reader.Read();
            }
        }

        public static readonly MethodInfo ReadSkipWhitespace = typeof(Methods).GetMethod("_ReadSkipWhitespace", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _ReadSkipWhitespace(TextReader reader)
        {
            int c;
            do { c = reader.Read(); }
            while (IsWhiteSpace(c));
            return c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsWhiteSpace(int c)
        {
            // per http://www.ietf.org/rfc/rfc4627.txt
            // insignificant whitespace in JSON is defined as 
            //  \u0020  - space
            //  \u0009  - tab
            //  \u000A  - new line
            //  \u000D  - carriage return

            return
                c == 0x20 ||
                c == 0x09 ||
                c == 0x0A ||
                c == 0x0D;
        }

        public static readonly MethodInfo ReadEncodedString = typeof(Methods).GetMethod("_ReadEncodedString", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedString(TextReader reader, ref StringBuilder commonSb)
        {
            commonSb = commonSb ?? new StringBuilder();

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character", reader);

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
                if (second == -1) throw new DeserializationException("Expected any character", reader);

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

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                ReadHexQuadToBuilder(reader, commonSb);
            }

            var ret = commonSb.ToString();

            // leave this clean for the next use
            commonSb.Clear();

            return ret;
        }

        public static readonly MethodInfo ReadEncodedStringCustom = typeof(Methods).GetMethod("_ReadEncodedStringCustom", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedStringCustom(TextReader reader, ref CustomStringBuilder commonSb)
        {
            commonSb.Clear();

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character", reader);

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
                if (second == -1) throw new DeserializationException("Expected any character", reader);

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

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                ReadHexQuadToCustomBuilder(reader, ref commonSb);
            }

            var ret = commonSb.StaticToString();

            return ret;
        }

        public static readonly MethodInfo ReadEncodedStringWithBuffer = typeof(Methods).GetMethod("_ReadEncodedStringWithBuffer", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedStringWithBuffer(TextReader reader, char[] buffer, ref StringBuilder commonSb)
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
                    if (first == -1) throw new DeserializationException("Expected any character", reader);

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
                    if (second == -1) throw new DeserializationException("Expected any character", reader);

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

                    if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

                    commonSb.Append(buffer, 0, ix);

                    // now we're in an escape sequence, we expect 4 hex #s; always
                    ReadHexQuadToBuilder(reader, commonSb);
                    break;
                }
            }

            // fall through to using a StringBuilder

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character", reader);

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
                if (second == -1) throw new DeserializationException("Expected any character", reader);

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

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                ReadHexQuadToBuilder(reader, commonSb);
            }

            var ret = commonSb.ToString();

            // leave this clean for the next use
            commonSb.Clear();

            return ret;
        }

        public static readonly MethodInfo ReadEncodedStringWithBufferCustom = typeof(Methods).GetMethod("_ReadEncodedStringWithBufferCustom", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedStringWithBufferCustom(TextReader reader, char[] buffer, ref CustomStringBuilder commonSb)
        {
            commonSb.Clear();

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
                    if (first == -1) throw new DeserializationException("Expected any character", reader);

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
                    if (second == -1) throw new DeserializationException("Expected any character", reader);

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

                    if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

                    commonSb.Append(buffer, 0, ix);

                    // now we're in an escape sequence, we expect 4 hex #s; always
                    ReadHexQuadToCustomBuilder(reader, ref commonSb);
                    break;
                }
            }

            // fall through to using a StringBuilder

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character", reader);

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
                if (second == -1) throw new DeserializationException("Expected any character", reader);

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

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                ReadHexQuadToCustomBuilder(reader, ref commonSb);
            }

            var ret = commonSb.StaticToString();

            return ret;
        }

        public static readonly MethodInfo ReadEncodedChar = typeof(Methods).GetMethod("_ReadEncodedChar", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char _ReadEncodedChar(TextReader reader)
        {
            var first = reader.Read();
            if (first == -1) throw new DeserializationException("Expected any character", reader);

            if (first != '\\') return (char)first;

            var second = reader.Read();
            if (second == -1) throw new DeserializationException("Expected any character", reader);

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

            if (second != 'u') throw new DeserializationException("Unrecognized escape sequence", reader);

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            finished:
            if (ret < char.MinValue || ret > char.MaxValue) throw new DeserializationException("Encoded character out of System.Char range, found: " + ret, reader);

            return (char)ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadHexQuad(TextReader reader)
        {
            int unescaped = 0;

            //char1:
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char2:
            unescaped *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char3:
            unescaped *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char4:
            unescaped *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            finished:
            return unescaped;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ReadHexQuadToBuilder(TextReader reader, StringBuilder commonSb)
        {
            var encodedChar = 0;

            //char1:
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char2:
            encodedChar *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char3:
            encodedChar *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char4:
            encodedChar *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            finished:
            commonSb.Append(Utils.SafeConvertFromUtf32(encodedChar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ReadHexQuadToCustomBuilder(TextReader reader, ref CustomStringBuilder commonSb)
        {
            var encodedChar = 0;

            //char1:
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char2:
            encodedChar *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char3:
            encodedChar *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            char4:
            encodedChar *= 16;
            {
                var c = reader.Read();

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

                throw new DeserializationException("Expected hex digit, found: " + c, reader);
            }

            finished:
            commonSb.Append(Utils.SafeConvertFromUtf32(encodedChar));
        }

        public static readonly MethodInfo ParseEnum = typeof(Methods).GetMethod("_ParseEnum", BindingFlags.NonPublic | BindingFlags.Static);
        static TEnum _ParseEnum<TEnum>(string asStr, TextReader reader)
            where TEnum : struct
        {
            TEnum ret;
            if (!TryParseEnum<TEnum>(asStr, out ret))
            {
                throw new DeserializationException("Unexpected value for " + typeof(TEnum).Name + ": " + asStr, reader);
            }

            return ret;
        }

        static bool TryParseEnum<TEnum>(string asStr, out TEnum parsed)
            where TEnum : struct
        {
            return EnumValues<TEnum>.TryParse(asStr, out parsed);
        }

        public static readonly MethodInfo ReadFlagsEnum = typeof(Methods).GetMethod("_ReadFlagsEnum", BindingFlags.NonPublic | BindingFlags.Static);
        static TEnum _ReadFlagsEnum<TEnum>(TextReader reader, ref StringBuilder commonSb)
            where TEnum : struct
        {
            commonSb = commonSb ?? new StringBuilder();

            var ret = default(TEnum);

            while (true)
            {
                var c = _ReadEncodedChar(reader);

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
                            throw new DeserializationException("Expected " + typeof(TEnum).Name + ", found: " + asStr, reader);
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

        public static readonly MethodInfo ReadFlagsEnumCustom = typeof(Methods).GetMethod("_ReadFlagsEnumCustom", BindingFlags.NonPublic | BindingFlags.Static);
        static TEnum _ReadFlagsEnumCustom<TEnum>(TextReader reader, ref CustomStringBuilder commonSb)
            where TEnum : struct
        {
            commonSb.Clear();

            var ret = default(TEnum);

            while (true)
            {
                var c = _ReadEncodedChar(reader);

                // ignore this *particular* whitespace
                if (c != ' ')
                {
                    // comma delimited
                    if (c == ',' || c == '"')
                    {
                        var asStr = commonSb.StaticToString();
                        TEnum parsed;
                        if (!TryParseEnum<TEnum>(asStr, out parsed))
                        {
                            throw new DeserializationException("Expected " + typeof(TEnum).Name + ", found: " + asStr, reader);
                        }

                        ret = FlagsEnumCombiner<TEnum>.Combine(ret, parsed);

                        commonSb.Clear();

                        if (c == '"') break;

                        continue;
                    }
                    commonSb.Append(c);
                }
            }

            return ret;
        }
    }
}
