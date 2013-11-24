using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static class Methods
    {
        public const int CharBufferSize = 4;

        public static readonly MethodInfo ReadUInt8TillEnd = typeof(Methods).GetMethod("_ReadUInt8TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte _ReadUInt8TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadInt8TillEnd = typeof(Methods).GetMethod("_ReadInt8TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static sbyte _ReadInt8TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadInt16TillEnd = typeof(Methods).GetMethod("_ReadInt16TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static short _ReadInt16TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadUInt16TillEnd = typeof(Methods).GetMethod("_ReadUInt16TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ushort _ReadUInt16TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadInt32TillEnd = typeof(Methods).GetMethod("_ReadInt32TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _ReadInt32TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadUInt32TillEnd = typeof(Methods).GetMethod("_ReadUInt32TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint _ReadUInt32TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadInt64TillEnd = typeof(Methods).GetMethod("_ReadInt64TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long _ReadInt64TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadUInt64TillEnd = typeof(Methods).GetMethod("_ReadUInt64TillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong _ReadUInt64TillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadFloatingPointTillEnd = typeof(Methods).GetMethod("_ReadFloatPointTillEnd", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double _ReadFloatPointTillEnd(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static readonly MethodInfo ReadEncodedString = typeof(Methods).GetMethod("_ReadEncodedString", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedString(TextReader reader, char[] buffer, StringBuilder commonSb)
        {
            {
                var ix = 0;

                while (ix <= CharBufferSize)
                {
                    if (ix == CharBufferSize)
                    {
                        commonSb.Append(new string(buffer, 0, ix));
                        break;
                    }

                    var first = reader.Read();
                    if (first == -1) throw new DeserializationException("Expected any character");

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
                    if (second == -1) throw new DeserializationException("Expected any character");

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

                    if (second != 'u') throw new DeserializationException("Unrecognized escape sequence");

                    commonSb.Append(buffer, 0, ix);

                    // now we're in an escape sequence, we expect 4 hex #s; always
                    ix = 0;
                    var read = 0;
                    var toRead = 4;
                    do
                    {
                        read = reader.Read(buffer, ix, toRead);
                        if (read == 0) throw new DeserializationException("Expected characters");

                        toRead -= read;
                        ix += read;
                    } while (toRead > 0);

                    var c = FastHexToInt(buffer);
                    commonSb.Append((char)c);
                    break;
                }
            }

            // fall through to using a StringBuilder

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character");

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
                if (second == -1) throw new DeserializationException("Expected any character");

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

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence");

                // now we're in an escape sequence, we expect 4 hex #s; always
                var ix = 0;
                var read = 0;
                var toRead = 4;
                do
                {
                    read = reader.Read(buffer, ix, toRead);
                    if (read == 0) throw new DeserializationException("Expected characters");

                    toRead -= read;
                    ix += read;
                } while (toRead > 0);

                var asInt = FastHexToInt(buffer);
                commonSb.Append((char)asInt);
            }

            var ret = commonSb.ToString();

            // leave this clean for the next use
            commonSb.Clear();

            return ret;
        }

        public static readonly MethodInfo ReadEncodedChar = typeof(Methods).GetMethod("_ReadEncodedChar", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char _ReadEncodedChar(TextReader reader, char[] buffer)
        {
            var first = reader.Read();
            if (first == -1) throw new DeserializationException("Expected any character");

            // TODO: high/low surrogates, do we need to worry about those?
            if (first != '\\') return (char)first;

            var second = reader.Read();
            if (second == -1) throw new DeserializationException("Expected any character");

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

            if (second != 'u') throw new DeserializationException("Unrecognized escape sequence");

            // now we're in an escape sequence, we expect 4 hex #s; always
            var ix = 0;
            var read = 0;
            var toRead = 4;
            do
            {
                read = reader.Read(buffer, ix, toRead);
                if (read == 0) throw new DeserializationException("Expected characters");

                toRead -= read;
                ix += read;
            } while (toRead > 0);


            return (char)FastHexToInt(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int FastHexToInt(char[] buffer)
        {
            var ret = 0;

            char1:
            {
                const int ix = 0;

                var c = (int)buffer[ix];

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

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char2;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }

            char2:
            ret *= 16;
            {
                const int ix = 1;

                var c = (int)buffer[ix];

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

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char3;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }

            char3:
            ret *= 16;
            {
                const int ix = 2;

                var c = (int)buffer[ix];

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

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char4;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }

            char4:
            ret *= 16;
            {
                const int ix = 3;

                var c = (int)buffer[ix];

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    return ret;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    return ret;
                }

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    return ret;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }
        }
    }
}
