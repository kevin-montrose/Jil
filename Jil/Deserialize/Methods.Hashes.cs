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
    partial class Methods
    {
        public static readonly MethodInfo MemberHash1 = typeof(Methods).GetMethod("_MemberHash1", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _MemberHash1(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new Exception("Expected any character");

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = 0;

            return length;
        }

        public static readonly MethodInfo EnumHash1 = typeof(Methods).GetMethod("_EnumHash1", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _EnumHash1(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new Exception("Expected any character");

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);
                
                finished:
                unescaped = ToLowerChar(unescaped);
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = 0;

            return length;
        }

        public static readonly MethodInfo MemberHash2 = typeof(Methods).GetMethod("_MemberHash2", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _MemberHash2(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new Exception("Expected any character");

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 2);

            return length;
        }

        public static readonly MethodInfo EnumHash2 = typeof(Methods).GetMethod("_EnumHash2", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _EnumHash2(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new Exception("Expected any character");

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                unescaped = ToLowerChar(unescaped);
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 2);

            return length;
        }

        public static readonly MethodInfo MemberHash4 = typeof(Methods).GetMethod("_MemberHash4", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _MemberHash4(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 4);

            return length;
        }

        public static readonly MethodInfo EnumHash4 = typeof(Methods).GetMethod("_EnumHash4", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _EnumHash4(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);
                
                finished:
                unescaped = ToLowerChar(unescaped);
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 4);

            return length;
        }

        public static readonly MethodInfo MemberHash8 = typeof(Methods).GetMethod("_MemberHash8", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _MemberHash8(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 8);

            return length;
        }

        public static readonly MethodInfo EnumHash8 = typeof(Methods).GetMethod("_EnumHash8", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _EnumHash8(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                unescaped = ToLowerChar(unescaped);
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 8);

            return length;
        }

        public static readonly MethodInfo MemberHash16 = typeof(Methods).GetMethod("_MemberHash16", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _MemberHash16(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 16);

            return length;
        }

        public static readonly MethodInfo EnumHash16 = typeof(Methods).GetMethod("_EnumHash16", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _EnumHash16(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                unescaped = ToLowerChar(unescaped);
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 16);

            return length;
        }

        public static readonly MethodInfo MemberHash32 = typeof(Methods).GetMethod("_MemberHash32", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _MemberHash32(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 32);

            return length;
        }

        public static readonly MethodInfo EnumHash32 = typeof(Methods).GetMethod("_EnumHash32", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _EnumHash32(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                unescaped = ToLowerChar(unescaped);
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 32);

            return length;
        }

        public static readonly MethodInfo MemberHash64 = typeof(Methods).GetMethod("_MemberHash64", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _MemberHash64(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 64);

            return length;
        }

        public static readonly MethodInfo EnumHash64 = typeof(Methods).GetMethod("_EnumHash64", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _EnumHash64(TextReader reader, out int bucket, out uint fullHash)
        {
            // This is basically: http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            const uint prime = 16777619;
            const uint offset = 2166136261;

            var result = offset;
            int unescaped;

            var length = 0;

            while (true)
            {
                var first = reader.Peek();

                if (first == -1) throw new DeserializationException("Expected any character", reader);

                if (first == '"') break;

                reader.Read();

                if (first != '\\')
                {
                    unescaped = first;
                    goto finished;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character", reader);
                if (second != 'u') throw new DeserializationException("Unexpected escape sequence in object member", reader);

                // now we're in an escape sequence, we expect 4 hex #s; always
                unescaped = ReadHexQuad(reader);

                finished:
                unescaped = ToLowerChar(unescaped);
                result ^= (uint)(unescaped & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 8) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 16) & 0xFF);
                result *= prime;
                result ^= (uint)((unescaped >> 24) & 0xFF);
                result *= prime;

                length++;
            }

            fullHash = result;
            bucket = (int)(result % 64);

            return length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int ReadHexQuad(TextReader reader)
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

        public static bool UseQuickLowerLookup = true;

        static readonly int[] QuickLowerLookup = new int[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        static int ToLowerChar(int c)
        {
            if (UseQuickLowerLookup)
            {
                // Typically we're gonna be working with ASCII, in those cases there's only a tiny continuous range we have to map,
                //    so avoid the whole cast and call business if we can
                if (c <= 126)
                {
                    var ix = c - 65;
                    if (ix < 0 || ix > 25) return c;

                    return QuickLowerLookup[ix];
                }
            }

            return (int)char.ToLowerInvariant((char)c);
        }
    }
}