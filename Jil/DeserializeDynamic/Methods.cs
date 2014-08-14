using Jil.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    static partial class Methods
    {
        internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // .NET will expand a char[] allocation up to 32, don't bother trimming this unless you can get this down to 16
        //    you'll just be making string parsing slower for no benefit
        public const int CharBufferSize = 32;

        public static void ConsumeWhiteSpace(TextReader reader)
        {
            int c;
            while ((c = reader.Peek()) != -1)
            {
                if (!IsWhiteSpace(c)) return;

                reader.Read();
            }
        }

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

        public static string ReadEncodedStringWithBuffer(TextReader reader, char[] buffer, ref StringBuilder commonSb)
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

        public static double ReadDouble(char firstChar, TextReader reader, ref StringBuilder commonSb)
        {
            commonSb = commonSb ?? new StringBuilder();

            int c;

            int prev = firstChar;
            var afterFirstDigit = (firstChar >= '0' && firstChar <= '9');
            var afterE = false;
            var afterDot = false;

            commonSb.Append(firstChar);

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
                        if (!(prev == 'e' || prev == 'E'))
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

        public static ulong ReadULong(char firstChar, TextReader reader, out byte overflowByPowersOfTen)
        {
            // ulong.MaxValue
            // ==============
            // 18446744073709551615
            // 123456789ABCDEFGHIJK
            //
            // Length: 20 

            overflowByPowersOfTen = 0;
            ulong ret;
            int c;

            // char1
            ret = (ulong)(firstChar - '0');

            for (var i = 2; i < 20; i++)
            {
                c = reader.Peek();
                if (c < '0' || c > '9') return ret;
                reader.Read();
                ret *= 10;
                ret += (uint)(c - '0');
            }

            // still more number to go?
            c = reader.Peek();
            if (c < '0' || c > '9') return ret;

            // now we need to see if we can pack this very last digit into the ulong
            //  this check is only necessary *now* because, due to base 10, you can't 
            //  overflow a ulong until the 20th digit
            var retCanBeMultiplied = (ulong.MaxValue / ret) >= 10;
            if(retCanBeMultiplied)
            {
                // remaining space will only be < 9 when we're really close
                //   to ulong.MaxValue (ie. we've read 1844674407370955161 
                //   and are now on the last digit, which could be [0, 5])
                var remainingSpace = (ulong.MaxValue - (ret * 10UL));
                var asAdd = (uint)(c - '0');
                if (asAdd <= remainingSpace)
                {
                    // we fit the criteria, advance!
                    reader.Read();
                    ret *= 10;
                    ret += asAdd;
                }
            }

            // now every character is just an extra order of magnitude for the exponent
            for(var i = 0; i < byte.MaxValue; i++)
            {
                c = reader.Peek();
                if (c < '0' || c > '9') return ret;
                reader.Read();
                overflowByPowersOfTen++;
            }

            throw new DeserializationException("Number too large to be parsed encountered", reader);
        }

        public static long ReadLong(char firstChar, TextReader reader)
        {
            var negate = false;
            long ret = 0;
            if (firstChar == '-')
            {
                negate = true;
            }
            else
            {
                ret = (firstChar - '0');
            }

            int c;
            while ((c = reader.Peek()) != -1)
            {
                c -= '0';
                if (c < 0 || c > 9) break;

                reader.Read();  // skip digit

                ret *= 10;
                ret += c;
            }

            if (negate) ret = -ret;

            return ret;
        }

        public static uint ReadUInt(char firstChar, TextReader reader, out byte length)
        {
            length = 1;
            uint ret = (uint)(firstChar - '0');

            int c;
            while ((c = reader.Peek()) != -1)
            {
                c -= '0';
                if (c < 0 || c > 9) break;

                reader.Read();  // skip digit
                length++;

                ret *= 10;
                ret += (uint)c;
            }

            return ret;
        }

        public static bool ReadNewtonsoftStyleDateTime(string str, out DateTime dt)
        {
            // Format: /Date(#####+####)/

            dt = DateTime.MinValue;

            if (str.Length < 9) return false;

            if (str[0] != '/' || str[1] != 'D' || str[2] != 'a' || str[3] != 't' || str[4] != 'e' || str[5] != '(')
            {
                return false;
            }

            bool negative = false;

            var ix = 6;
            var c = str[ix];
            if (c == '-')
            {
                negative = true;
                ix++;
                c = str[ix];
            }

            if (c < '0' || c > '9') return false;

            long l = 0;
            for (var i = 0; ix < str.Length - 1 && i < 20; i++)
            {
                l *= 10;
                l += (c - '0');
                ix++;
                c = str[ix];
                if (c < '0' || c > '9') break;
            }

            if (negative) l = -l;

            if (ix == str.Length) return false;
            var hasTimeZone = str[ix] == '+' || str[ix] == '-';
            if (hasTimeZone)
            {
                ix++;
                for (var i = 0; i < 4; i++)
                {
                    if (ix == str.Length) return false;
                    c = str[ix];
                    if (c < '0' || c > '9') return false;
                    ix++;
                }
            }

            var remainingLen = str.Length - ix;
            if (remainingLen != 2) return false;
            if (str[ix] != ')') return false;
            ix++;
            if (str[ix] != '/') return false;

            dt = UnixEpoch + TimeSpan.FromMilliseconds(l);
            return true;
        }
    }
}
