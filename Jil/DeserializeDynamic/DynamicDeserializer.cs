using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    class DynamicDeserializer
    {
        internal static bool UseFastNumberParsing = true;
        internal static bool UseFastIntegerConversion = true;
        internal static bool UseFastMemberStartCheck = true;

        public static ObjectBuilder Deserialize(TextReader reader)
        {
            var ret = new ObjectBuilder();

            DeserializeMember(reader, ret);

            Methods.ConsumeWhiteSpace(reader);
            var c = reader.Peek();
            if (c != -1) throw new DeserializationException("Expected end of stream", reader);

            return ret;
        }

        static void DeserializeMember(TextReader reader, ObjectBuilder builder)
        {
            Methods.ConsumeWhiteSpace(reader);

            var c = reader.Read();
            if (UseFastMemberStartCheck)
            {
                // What's going on here is a little tricky.
                // Basically, the switch in the else of this outer if
                //   is one of the hottest parts of dynamic deserialization.
                // Speeding up a switch is *hard*.
                // The theory is that, since the switch is turned into a series of ifs,
                //   the branch predictor is gonna be really bad.  Exactly which character
                //   will be in `c` is very nearly random.
                // Instead of a naive switch, I found a dinky little formula that maps
                //   the 7 characters we care about to a contiguous range.  This lets us turn
                //   the series of ifs into a jump table.  This lets us cut out the branch predictor
                //   somewhat, and turns the remaining ifs into "almost always false"-branches that
                //   the predictor should do a good job on.
                // In the outer else a character would go through, on average, 4 comparisons.
                // This code will go through, *always*, 3 comparisons.  More math though.
                //
                // It remains to be seen if this code is actually faster though.  There are other,
                //   potentially lighter weight, functions that could be tried as well.

                if (c == -1) throw new DeserializationException("Unexpected end of stream", reader);

                var val = (uint)c;

                var a = (byte)((val >> 4) & 0x0F);
                var b = (byte)(val & 0x0F);
                if (b == 0) goto checkNumber;
                uint ix = ((a - (uint)13) % (b * (uint)159)) % (uint)16;
                ix -= 9;
                switch (ix)
                {
                    // "
                    case 0:
                        if (c != '"') break;
                        DeserializeString(reader, builder);
                        return;
                    // [
                    case 1:
                        if (c != '[') break;
                        DeserializeArray(reader, builder);
                        return;
                    // n
                    case 2:
                        if (c != 'n') break;
                        DeserializeNull(reader, builder);
                        return;
                    // {
                    case 3:
                        if (c != '{') break;
                        DeserializeObject(reader, builder);
                        return;
                    // f
                    case 4:
                        if (c != 'f') break;
                        DeserializeFalse(reader, builder);
                        return;
                    // t
                    case 5:
                        if (c != 't') break;
                        DeserializeTrue(reader, builder);
                        return;
                    // -
                    case 6:
                        if (c != '-') break;
                        DeserializeNumber('-', reader, builder);
                        return;
                }
            }
            else
            {
                switch (c)
                {
                    case -1: throw new DeserializationException("Unexpected end of stream", reader);
                    case '"': DeserializeString(reader, builder); return;
                    case '[': DeserializeArray(reader, builder); return;
                    case '{': DeserializeObject(reader, builder); return;
                    case 'n': DeserializeNull(reader, builder); return;
                    case 't': DeserializeTrue(reader, builder); return;
                    case 'f': DeserializeFalse(reader, builder); return;
                    case '-': DeserializeNumber((char)c, reader, builder); return;
                }
            }

            checkNumber:

            if (c >= '0' && c <= '9')
            {
                DeserializeNumber((char)c, reader, builder);
                return;
            }

            throw new DeserializationException("Expected \", [, {, n, t, f, -, 0, 1, 2, 3, 4, 5, 6, 7, 8, or 9; found " + (char)c, reader);
        }

        static void DeserializeTrue(TextReader reader, ObjectBuilder builder)
        {
            var c = reader.Read();
            if (c != 'r') throw new DeserializationException("Expected r", reader);
            c = reader.Read();
            if (c != 'u') throw new DeserializationException("Expected u", reader);
            c = reader.Read();
            if (c != 'e') throw new DeserializationException("Expected e", reader);

            builder.PutTrue();
        }

        static void DeserializeFalse(TextReader reader, ObjectBuilder builder)
        {
            var c = reader.Read();
            if (c != 'a') throw new DeserializationException("Expected a", reader);
            c = reader.Read();
            if (c != 'l') throw new DeserializationException("Expected l", reader);
            c = reader.Read();
            if (c != 's') throw new DeserializationException("Expected s", reader);
            c = reader.Read();
            if (c != 'e') throw new DeserializationException("Expected e", reader);

            builder.PutFalse();
        }

        static void DeserializeNull(TextReader reader, ObjectBuilder builder)
        {
            var c = reader.Read();
            if (c != 'u') throw new DeserializationException("Expected u", reader);
            c = reader.Read();
            if (c != 'l') throw new DeserializationException("Expected l", reader);
            c = reader.Read();
            if (c != 'l') throw new DeserializationException("Expected l", reader);

            builder.PutNull();
        }

        static void DeserializeString(TextReader reader, ObjectBuilder builder)
        {
            var str = Methods.ReadEncodedStringWithBuffer(reader, builder.CommonCharBuffer, ref builder.CommonStringBuffer);

            builder.PutString(str);
        }

        static void DeserializeArray(TextReader reader, ObjectBuilder builder)
        {
            int c;
            builder.StartArray();

            while(true)
            {
                c = reader.Peek();
                if (c == -1) throw new DeserializationException("Unexpected end of stream", reader);
                if (c == ']')
                {
                    reader.Read();  // skip the ]
                    break;
                }

                DeserializeMember(reader, builder);
                Methods.ConsumeWhiteSpace(reader);
                c = reader.Read();

                if(c == ',') continue;
                if(c == ']') break;

                if(c == -1) throw new DeserializationException("Unexpected end of stream", reader);

                throw new DeserializationException("Expected , or ], found "+(char)c, reader);
            }

            builder.EndArray();
        }

        static void DeserializeObject(TextReader reader, ObjectBuilder builder)
        {
            int c;
            builder.StartObject();

            while (true)
            {
                Methods.ConsumeWhiteSpace(reader);

                c = reader.Peek();
                if (c == -1) throw new DeserializationException("Unexpected end of stream", reader);
                if (c == '}')
                {
                    reader.Read();  // skip }
                    break;
                }

                c = reader.Read();
                if (c == -1) throw new DeserializationException("Unexpected end of stream", reader);
                if (c != '"') throw new DeserializationException("Expected \", found " + (char)c, reader);

                builder.StartObjectMember();
                DeserializeString(reader, builder);

                Methods.ConsumeWhiteSpace(reader);
                c = reader.Read();
                if (c == -1) throw new DeserializationException("Unexpected end of stream", reader);
                if (c != ':') throw new DeserializationException("Expected :, found " + (char)c, reader);

                DeserializeMember(reader, builder);

                builder.EndObjectMember();

                Methods.ConsumeWhiteSpace(reader);
                c = reader.Read();

                if (c == ',') continue;
                if (c == '}') break;

                if (c == -1) throw new DeserializationException("Unexpected end of stream", reader);

                throw new DeserializationException("Expected , or }, found " + (char)c, reader);
            }

            builder.EndObject();
        }

        static void DeserializeNumber(char leadingChar, TextReader reader, ObjectBuilder builder)
        {
            if (!UseFastNumberParsing)
            {
                var number = Methods.ReadDouble(leadingChar, reader, ref builder.CommonStringBuffer);

                builder.PutNumber(number);

                return;
            }

            bool negative;
            ulong beforeDot;
            long afterE;
            uint afterDot;
            byte afterDotLen;
            byte extraOrdersOfMagnitude;

            if (leadingChar == '-')
            {
                var next = reader.Read();
                if (next != '-' && !(next >= '0' && next <= '9')) throw new DeserializationException("Expected -, or digit", reader);

                leadingChar = (char)next;
                negative = true;
            }
            else
            {
                negative = false;
            }

            beforeDot = Methods.ReadULong(leadingChar, reader, out extraOrdersOfMagnitude);
            var c = reader.Peek();
            if (c == '.')
            {
                reader.Read();
                c = reader.Read();
                if (c < '0' && c > '9') throw new DeserializationException("Expected digit", reader);

                afterDot = Methods.ReadUInt((char)c, reader, out afterDotLen);

                c = reader.Peek();
            }
            else
            {
                afterDot = afterDotLen = 0;
            }

            if (c == 'e' || c == 'E')
            {
                reader.Read();
                c = reader.Read();
                if (c == '+')
                {
                    c = reader.Read();
                }
                if (c != '-' && !(c >= '0' || c <= '9')) throw new DeserializationException("Expected -, +, or digit", reader);
                afterE = Methods.ReadLong((char)c, reader);
            }
            else
            {
                afterE = 0;
            }

            if (extraOrdersOfMagnitude != 0)
            {
                try
                {
                    checked
                    {
                        afterE += extraOrdersOfMagnitude;
                    }
                }
                catch (OverflowException)
                {
                    throw new DeserializationException("Number too large to be parsed encountered", reader);
                }
            }

            builder.PutFastNumber(negative, beforeDot, afterDot, afterDotLen, afterE);
        }
    }
}
