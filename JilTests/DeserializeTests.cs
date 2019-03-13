using Jil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JilTests
{
    public interface _Interface1
    {
        int A { get; set; }
        string B { get; set; }
    }

    public interface _Interface2 : _Interface1
    {
        double C { get; set; }
    }

    public interface _Interface3
    {
        int A { get; }
        int B { set; }
        int C { get; set; }
    }

    public class DeserializeTests
    {
#pragma warning disable 0649
        struct _ValueTypes
        {
            public string A;
            public int B { get; set; }
        }
#pragma warning restore 0649

        [Fact]
        public void ValueTypes()
        {
            using (var str = new StringReader("{\"A\":\"hello\\u0000world\", \"B\":12345}"))
            {
                var res = JSON.Deserialize<_ValueTypes>(str);
                Assert.Equal("hello\0world", res.A);
                Assert.Equal(12345, res.B);
            }
        }

#pragma warning disable 0649
        class _LargeCharBuffer
        {
            public DateTime Date;
            public string String;
        }
#pragma warning restore 0649

        [Fact]
        public void LargeCharBuffer()
        {
            using (var str = new StringReader("{\"Date\": \"2013-12-30T04:17:21Z\", \"String\": \"hello world\"}"))
            {
                var res = JSON.Deserialize<_LargeCharBuffer>(str, Options.ISO8601);
                Assert.Equal(new DateTime(2013, 12, 30, 4, 17, 21, DateTimeKind.Utc), res.Date);
                Assert.Equal("hello world", res.String);
            }
        }

#pragma warning disable 0649
        class _SmallCharBuffer
        {
            public DateTime Date;
            public string String;
        }
#pragma warning restore 0649

        [Fact]
        public void SmallCharBuffer()
        {
            using (var str = new StringReader("{\"Date\": 1388377041, \"String\": \"hello world\"}"))
            {
                var res = JSON.Deserialize<_SmallCharBuffer>(str, Options.SecondsSinceUnixEpoch);
                Assert.Equal(new DateTime(2013, 12, 30, 4, 17, 21, DateTimeKind.Utc), res.Date);
                Assert.Equal("hello world", res.String);
            }
        }

        [Fact]
        public void IDictionaryIntToInt()
        {
            using (var str = new StringReader("{\"1\":2, \"3\":4, \"5\": 6}"))
            {
                var res = JSON.Deserialize<IDictionary<int, int>>(str);
                Assert.Equal(3, res.Count);
                Assert.Equal(2, res[1]);
                Assert.Equal(4, res[3]);
                Assert.Equal(6, res[5]);
            }
        }

        enum _DictionaryEnumKeys1 : byte
        {
            A,
            B
        }

        enum _DictionaryEnumKeys2 : sbyte
        {
            A,
            B
        }

        enum _DictionaryEnumKeys3 : short
        {
            A,
            B
        }

        enum _DictionaryEnumKeys4 : ushort
        {
            A,
            B
        }

        enum _DictionaryEnumKeys5 : int
        {
            A,
            B
        }

        enum _DictionaryEnumKeys6 : uint
        {
            A,
            B
        }

        enum _DictionaryEnumKeys7 : long
        {
            A,
            B
        }

        enum _DictionaryEnumKeys8 : ulong
        {
            A,
            B
        }

        [Fact]
        public void DictionaryEnumKeys()
        {
            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys1, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys1.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys1, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys1.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys1.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys2, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys2.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys2, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys2.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys2.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys3, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys3.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys3, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys3.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys3.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys4, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys4.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys4, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys4.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys4.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys5, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys5.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys5, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys5.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys5.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys6, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys6.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys6, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys6.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys6.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys7, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys7.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys7, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys7.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys7.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys8, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[_DictionaryEnumKeys8.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys8, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys8.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys8.B]);
            }
        }

        [Fact]
        public void IReadOnlyDictionaryEnumKeys()
        {
            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys1, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys1.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys1, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys1.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys1.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys2, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys2.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys2, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys2.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys2.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys3, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys3.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys3, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys3.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys3.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys4, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys4.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys4, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys4.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys4.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys5, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys5.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys5, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys5.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys5.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys6, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys6.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys6, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys6.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys6.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys7, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys7.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys7, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys7.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys7.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys8, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys8.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<_DictionaryEnumKeys8, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[_DictionaryEnumKeys8.A]);
                Assert.Equal("fizz buzz", res[_DictionaryEnumKeys8.B]);
            }
        }

        [Fact]
        public void DictionaryNumberKeys()
        {
            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<byte, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<byte, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"-1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<sbyte, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[-1]);
            }

            using (var str = new StringReader("{\"-1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<sbyte, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[-1]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<short, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"-22\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<short, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[-22]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ushort, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"234\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ushort, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[234]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<int, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<int, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<uint, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2456789\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<uint, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[2456789]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<long, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"-1234567890\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<long, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[-1234567890]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ulong, string>>(str);
                Assert.Single(res);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"" + ulong.MaxValue + "\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ulong, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[ulong.MaxValue]);
            }
        }


        [Fact]
        public void IReadOnlyDictionaryNumberKeys()
        {
            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<byte, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<byte, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"-1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<sbyte, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[-1]);
            }

            using (var str = new StringReader("{\"-1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<sbyte, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[-1]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<short, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"-22\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<short, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[-22]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<ushort, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"234\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<ushort, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[234]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<int, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<int, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<uint, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2456789\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<uint, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[2456789]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<long, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"-1234567890\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<long, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[-1234567890]);
                Assert.Equal("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<ulong, string>>(str);
                Assert.Equal(1, res.Count);
                Assert.Equal("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"" + ulong.MaxValue + "\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<IReadOnlyDictionary<ulong, string>>(str);
                Assert.Equal(2, res.Count);
                Assert.Equal("hello world", res[1]);
                Assert.Equal("fizz buzz", res[ulong.MaxValue]);
            }
        }

#pragma warning disable 0649
        class _IDictionaries
        {
            public IDictionary<string, sbyte> StringToBytes;
            public IDictionary<string, IDictionary<string, int>> StringToStringToBytes;
        }
#pragma warning restore 0649

        [Fact]
        public void IDictionaries()
        {
            using (var str = new StringReader("{\"StringToBytes\":{\"a\":-1,\"b\":127,\"c\":8},\"StringToStringToBytes\":{\"foo\":{\"bar\":123}, \"fizz\":{\"buzz\":456, \"bar\":789}}}"))
            {
                var res = JSON.Deserialize<_IDictionaries>(str);
                Assert.NotNull(res);

                Assert.NotNull(res.StringToBytes);
                Assert.Equal(3, res.StringToBytes.Count);
                Assert.True(res.StringToBytes.ContainsKey("a"));
                Assert.Equal((sbyte)-1, res.StringToBytes["a"]);
                Assert.True(res.StringToBytes.ContainsKey("b"));
                Assert.Equal((sbyte)127, res.StringToBytes["b"]);
                Assert.True(res.StringToBytes.ContainsKey("c"));
                Assert.Equal((sbyte)8, res.StringToBytes["c"]);

                Assert.NotNull(res.StringToStringToBytes);
                Assert.Equal(2, res.StringToStringToBytes.Count);
                Assert.True(res.StringToStringToBytes.ContainsKey("foo"));
                Assert.Equal(1, res.StringToStringToBytes["foo"].Count);
                Assert.True(res.StringToStringToBytes["foo"].ContainsKey("bar"));
                Assert.Equal(123, res.StringToStringToBytes["foo"]["bar"]);
                Assert.True(res.StringToStringToBytes.ContainsKey("fizz"));
                Assert.Equal(2, res.StringToStringToBytes["fizz"].Count);
                Assert.True(res.StringToStringToBytes["fizz"].ContainsKey("buzz"));
                Assert.Equal(456, res.StringToStringToBytes["fizz"]["buzz"]);
                Assert.True(res.StringToStringToBytes["fizz"].ContainsKey("bar"));
                Assert.Equal(789, res.StringToStringToBytes["fizz"]["bar"]);
            }
        }

#pragma warning disable 0649
        class _IReadOnlyDictionaries
        {
            public IReadOnlyDictionary<string, sbyte> StringToBytes;
            public IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> StringToStringToBytes;
        }
#pragma warning restore 0649

        [Fact]
        public void IReadOnlyDictionaries()
        {
            using (var str = new StringReader("{\"StringToBytes\":{\"a\":-1,\"b\":127,\"c\":8},\"StringToStringToBytes\":{\"foo\":{\"bar\":123}, \"fizz\":{\"buzz\":456, \"bar\":789}}}"))
            {
                var res = JSON.Deserialize<_IReadOnlyDictionaries>(str);
                Assert.NotNull(res);

                Assert.NotNull(res.StringToBytes);
                Assert.Equal(3, res.StringToBytes.Count);
                Assert.True(res.StringToBytes.ContainsKey("a"));
                Assert.Equal((sbyte)-1, res.StringToBytes["a"]);
                Assert.True(res.StringToBytes.ContainsKey("b"));
                Assert.Equal((sbyte)127, res.StringToBytes["b"]);
                Assert.True(res.StringToBytes.ContainsKey("c"));
                Assert.Equal((sbyte)8, res.StringToBytes["c"]);

                Assert.NotNull(res.StringToStringToBytes);
                Assert.Equal(2, res.StringToStringToBytes.Count);
                Assert.True(res.StringToStringToBytes.ContainsKey("foo"));
                Assert.Equal(1, res.StringToStringToBytes["foo"].Count);
                Assert.True(res.StringToStringToBytes["foo"].ContainsKey("bar"));
                Assert.Equal(123, res.StringToStringToBytes["foo"]["bar"]);
                Assert.True(res.StringToStringToBytes.ContainsKey("fizz"));
                Assert.Equal(2, res.StringToStringToBytes["fizz"].Count);
                Assert.True(res.StringToStringToBytes["fizz"].ContainsKey("buzz"));
                Assert.Equal(456, res.StringToStringToBytes["fizz"]["buzz"]);
                Assert.True(res.StringToStringToBytes["fizz"].ContainsKey("bar"));
                Assert.Equal(789, res.StringToStringToBytes["fizz"]["bar"]);
            }
        }

#pragma warning disable 0649
        class _ILists
        {
            public IList<byte> Bytes;
            public IList<IList<int>> IntsOfInts;
        }
#pragma warning restore 0649

        [Fact]
        public void ILists()
        {
            using (var str = new StringReader("{\"Bytes\":[255,0,128],\"IntsOfInts\":[[1,2,3],[4,5,6],[7,8,9]]}"))
            {
                var res = JSON.Deserialize<_ILists>(str);
                Assert.NotNull(res);

                Assert.NotNull(res.Bytes);
                Assert.Equal(3, res.Bytes.Count);
                Assert.Equal(255, res.Bytes[0]);
                Assert.Equal(0, res.Bytes[1]);
                Assert.Equal(128, res.Bytes[2]);

                Assert.NotNull(res.IntsOfInts);
                Assert.Equal(3, res.IntsOfInts.Count);
                Assert.NotNull(res.IntsOfInts[0]);
                Assert.Equal(3, res.IntsOfInts[0].Count);
                Assert.Equal(1, res.IntsOfInts[0][0]);
                Assert.Equal(2, res.IntsOfInts[0][1]);
                Assert.Equal(3, res.IntsOfInts[0][2]);
                Assert.Equal(3, res.IntsOfInts[1].Count);
                Assert.Equal(4, res.IntsOfInts[1][0]);
                Assert.Equal(5, res.IntsOfInts[1][1]);
                Assert.Equal(6, res.IntsOfInts[1][2]);
                Assert.Equal(3, res.IntsOfInts[2].Count);
                Assert.Equal(7, res.IntsOfInts[2][0]);
                Assert.Equal(8, res.IntsOfInts[2][1]);
                Assert.Equal(9, res.IntsOfInts[2][2]);
            }
        }

#pragma warning disable 0649
        class _IReadOnlyLists
        {
            public IReadOnlyList<byte> Bytes;
            public IReadOnlyList<IReadOnlyList<int>> IntsOfInts;
        }
#pragma warning restore 0649

        [Fact]
        public void IReadOnlyLists()
        {
            using (var str = new StringReader("{\"Bytes\":[255,0,128],\"IntsOfInts\":[[1,2,3],[4,5,6],[7,8,9]]}"))
            {
                var res = JSON.Deserialize<_IReadOnlyLists>(str);
                Assert.NotNull(res);

                Assert.NotNull(res.Bytes);
                Assert.Equal(3, res.Bytes.Count);
                Assert.Equal(255, res.Bytes[0]);
                Assert.Equal(0, res.Bytes[1]);
                Assert.Equal(128, res.Bytes[2]);

                Assert.NotNull(res.IntsOfInts);
                Assert.Equal(3, res.IntsOfInts.Count);
                Assert.NotNull(res.IntsOfInts[0]);
                Assert.Equal(3, res.IntsOfInts[0].Count);
                Assert.Equal(1, res.IntsOfInts[0][0]);
                Assert.Equal(2, res.IntsOfInts[0][1]);
                Assert.Equal(3, res.IntsOfInts[0][2]);
                Assert.Equal(3, res.IntsOfInts[1].Count);
                Assert.Equal(4, res.IntsOfInts[1][0]);
                Assert.Equal(5, res.IntsOfInts[1][1]);
                Assert.Equal(6, res.IntsOfInts[1][2]);
                Assert.Equal(3, res.IntsOfInts[2].Count);
                Assert.Equal(7, res.IntsOfInts[2][0]);
                Assert.Equal(8, res.IntsOfInts[2][1]);
                Assert.Equal(9, res.IntsOfInts[2][2]);
            }
        }

#pragma warning disable 0649
        class _ISets
        {
            public ISet<int> IntSet;
            public HashSet<string> StringHashSet;
            public SortedSet<int> IntSortedSet;
        }
#pragma warning restore 0649

        [Fact]
        public void ISets()
        {
            using (var str = new StringReader("{\"IntSet\":[17,23],\"StringHashSet\":[\"Hello\",\"Hash\",\"Set\"],\"IntSortedSet\":[1,2,3]}"))
            {
                var res = JSON.Deserialize<_ISets>(str);
                Assert.NotNull(res);

                Assert.NotNull(res.IntSet);
                Assert.IsType<HashSet<int>>(res.IntSet);
                Assert.Equal(2, res.IntSet.Count);
                Assert.True(res.IntSet.Contains(17));
                Assert.True(res.IntSet.Contains(23));

                Assert.NotNull(res.StringHashSet);
                Assert.Equal(3, res.StringHashSet.Count);
                Assert.Contains("Hello", res.StringHashSet);
                Assert.Contains("Hash", res.StringHashSet);
                Assert.Contains("Set", res.StringHashSet);

                Assert.NotNull(res.IntSortedSet);
                Assert.Equal(3, res.IntSortedSet.Count);
                Assert.Contains(1, res.IntSortedSet);
                Assert.Contains(2, res.IntSortedSet);
                Assert.Contains(3, res.IntSortedSet);
            }
        }

#pragma warning disable 0649
        class _InfoFailure
        {
            public decimal? questions_per_minute;
            public decimal? answers_per_minute;
        }
#pragma warning restore 0649

        [Fact]
        public void InfoFailure()
        {
            {
                const string data = "{\"questions_per_minute\":0,\"answers_per_minute\":null}";
                using (var str = new StringReader(data))
                {
                    var res = JSON.Deserialize<_InfoFailure>(str, Options.ISO8601);
                    Assert.NotNull(res);
                    Assert.Equal(0, res.questions_per_minute);
                    Assert.Null(res.answers_per_minute);
                }
            }

            {
                const string data = "{\"questions_per_minute\":0}";
                using (var str = new StringReader(data))
                {
                    var res = JSON.Deserialize<_InfoFailure>(str, Options.ISO8601);
                    Assert.NotNull(res);
                    Assert.Equal(0, res.questions_per_minute);
                }
            }
        }

        [Fact]
        public void Zeros()
        {
            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<int>(str);
                Assert.Equal(0, ret);
            }

            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<float>(str);
                Assert.Equal(0f, ret);
            }

            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<double>(str);
                Assert.Equal(0.0, ret);
            }

            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<decimal>(str);
                Assert.Equal(0m, ret);
            }
        }

        [Fact]
        public void Arrays()
        {
            using (var str = new StringReader("[0,1,2,3,4,5]"))
            {
                var ret = JSON.Deserialize<int[]>(str);
                Assert.Equal(6, ret.Length);
                Assert.Equal(0, ret[0]);
                Assert.Equal(1, ret[1]);
                Assert.Equal(2, ret[2]);
                Assert.Equal(3, ret[3]);
                Assert.Equal(4, ret[4]);
                Assert.Equal(5, ret[5]);
            }
        }

        [Fact]
        public void ParseISO8601()
        {
            using (var str = new StringReader("\"1900\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1991-02\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1991, 02, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 00, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12,5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12.5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234,5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234.5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456,5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456.5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12,5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12.5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234,5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234.5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456,5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456.5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01t12:30+00\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2004-366\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"2004366\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }
        }

        [Theory]
        [InlineData("\"99\"", "ISO8601 date must begin with a 4 character year")]
        [InlineData("\"0000\"", "ISO8601 year 0000 cannot be converted to a DateTime")]
        [InlineData("\"1999-13\"", "Expected month to be between 01 and 12")]
        [InlineData("\"1999-12-00\"", "Expected day to be between 01 and 31")]
        [InlineData("\"19991200\"", "Expected day to be between 01 and 31")]
        [InlineData("\"1900-01-01T12:34:56.123456789+00:00\"", "ISO8601 date is too long, expected " + "33" + " characters or less")] // Jil.Deserialize.Methods.CharBufferSize
        [InlineData("\"19000101T1234:56\"", "Unexpected separator in ISO8601 time")]
        [InlineData("\"1900-01-01+00:30\"", "Unexpected date string length")]
        [InlineData("\"1900-\"", "Unexpected date string length")]
        [InlineData("\"1900-01-\"", "Expected digit")]
        [InlineData("\"1900-01-01T\"", "ISO8601 time must begin with a 2 character hour")]
        [InlineData("\"1900-01-01T19:\"", "Expected minute part of ISO8601 time")]
        [InlineData("\"1900-01-01T19:19+\"", "Expected hour part of ISO8601 timezone offset")]
        [InlineData("\"1900-01-01T19:19+00:\"", "Not enough character for ISO8601 timezone offset")]
        [InlineData("\"1900-366\"", "Ordinal day can only be 366 in a leap year")]
        [InlineData("\"1900366\"", "Ordinal day can only be 366 in a leap year")]
        [InlineData("\"1900-999\"", "Expected ordinal day to be between 001 and 366")]
        [InlineData("\"1900999\"", "Expected ordinal day to be between 001 and 366")]
        [InlineData("\"1900-000\"", "Expected ordinal day to be between 001 and 366")]
        [InlineData("\"1900000\"", "Expected ordinal day to be between 001 and 366")]
        [InlineData("\"1999-02-29\"", "ISO8601 date could not be mapped to DateTime")]
        [InlineData("\"1999-W01-8\"", "Expected day to be a digit between 1 and 7")]
        [InlineData("\"1999-W01-0\"", "Expected day to be a digit between 1 and 7")]
        [InlineData("\"1999W018\"", "Expected day to be a digit between 1 and 7")]
        [InlineData("\"1999W010\"", "Expected day to be a digit between 1 and 7")]
        [InlineData("\"1999-W1\"", "Unexpected date string length")]
        [InlineData("\"1999W1\"", "Unexpected date string length")]
        [InlineData("\"1999-W00\"", "Expected week to be between 01 and 53")]
        [InlineData("\"1999W00\"", "Expected week to be between 01 and 53")]
        [InlineData("\"1999-W54\"", "Expected week to be between 01 and 53")]
        [InlineData("\"1999W54\"", "Expected week to be between 01 and 53")]
        public void MalformedISO8601(string dateString, string expectedError)
        {
            using (var str = new StringReader(dateString))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<DateTime>(str, Options.ISO8601));
                Assert.Equal(expectedError, ex.Message);
            }
        }

        [Fact]
        public void Issue143DateTime()
        {
            var date = new DateTime(21, DateTimeKind.Utc);
            var str = JSON.Serialize(date, Options.ISO8601);

            // ThunkReader
            var result = JSON.Deserialize<DateTime>(str, Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);

            // TextReader
            result = JSON.Deserialize<DateTime>(new StringReader(str), Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);
        }

        [Fact]
        public void Issue143DateTimeFractionOverflow()
        {
            var date = new DateTime(21, DateTimeKind.Utc);
            const string str = "\"0001-01-01T00:00:00.0000021001Z\"";

            // ThunkReader
            var result = JSON.Deserialize<DateTime>(str, Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);

            // TextReader
            result = JSON.Deserialize<DateTime>(new StringReader(str), Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);
        }

        [Fact]
        public void Issue143TimeSpan()
        {
            var span = new TimeSpan(21);
            var str = JSON.Serialize(span, Options.ISO8601);

            // ThunkReader
            var result = JSON.Deserialize<TimeSpan>(str, Options.ISO8601);
            Assert.Equal(span.Ticks, result.Ticks);

            // TextReader
            result = JSON.Deserialize<TimeSpan>(new StringReader(str), Options.ISO8601);
            Assert.Equal(span.Ticks, result.Ticks);
        }


        [Fact]
        public void Issue143TimeSpanFractionOverflow()
        {
            var date = new TimeSpan(21);
            const string str = "\"PT0.00000210000001S\"";

            // ThunkReader
            var result = JSON.Deserialize<TimeSpan>(str, Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);

            // TextReader
            result = JSON.Deserialize<TimeSpan>(new StringReader(str), Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);
        }

        [Fact]
        public void Issue143DateTimeOffset()
        {
            var offset = new DateTimeOffset(new DateTime(21, DateTimeKind.Utc), TimeSpan.Zero);
            var str = JSON.Serialize(offset, Options.ISO8601);

            // ThunkReader
            var result = JSON.Deserialize<DateTimeOffset>(str, Options.ISO8601);
            Assert.Equal(offset.Ticks, result.Ticks);

            // TextReader
            result = JSON.Deserialize<DateTimeOffset>(new StringReader(str), Options.ISO8601);
            Assert.Equal(offset.Ticks, result.Ticks);
        }


        [Fact]
        public void Issue143DateTimeOffsetFractionOverflow()
        {
            var offset = new DateTimeOffset(new DateTime(21, DateTimeKind.Utc), TimeSpan.Zero);
            const string str = "\"0001-01-01T00:00:00.000002100001Z\"";

            // ThunkReader
            var result = JSON.Deserialize<DateTimeOffset>(str, Options.ISO8601);
            Assert.Equal(offset.Ticks, result.Ticks);

            // TextReader
            result = JSON.Deserialize<DateTimeOffset>(new StringReader(str), Options.ISO8601);
            Assert.Equal(offset.Ticks, result.Ticks);
        }

        [Fact]
        public void SecondDateTimes()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.UtcNow;

            var dtStr = JSON.Serialize(dt, Options.SecondsSinceUnixEpoch);
            var nowStr = JSON.Serialize(now, Options.SecondsSinceUnixEpoch);

            using (var str = new StringReader(dtStr))
            {
                var dtRet = JSON.Deserialize<DateTime>(str, Options.SecondsSinceUnixEpoch);
                var delta = (dtRet - dt).Duration().TotalSeconds;
                Assert.True(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.Deserialize<DateTime>(str, Options.SecondsSinceUnixEpoch);
                var delta = (nowRet - now).Duration().TotalSeconds;
                Assert.True(delta < 1);
            }
        }

        [Fact]
        public void MillisecondDateTimes()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.UtcNow;

            var dtStr = JSON.Serialize(dt, Options.MillisecondsSinceUnixEpoch);
            var nowStr = JSON.Serialize(now, Options.MillisecondsSinceUnixEpoch);

            using (var str = new StringReader(dtStr))
            {
                var dtRet = JSON.Deserialize<DateTime>(str, Options.MillisecondsSinceUnixEpoch);
                var delta = (dtRet - dt).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.Deserialize<DateTime>(str, Options.MillisecondsSinceUnixEpoch);
                var delta = (nowRet - now).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }
        }

        [Fact]
        public void NewtsoftDateTimes()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.UtcNow;

            var dtStr = JSON.Serialize(dt);
            var nowStr = JSON.Serialize(now);

            using (var str = new StringReader(dtStr))
            {
                var dtRet = JSON.Deserialize<DateTime>(str);
                var delta = (dtRet - dt).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.Deserialize<DateTime>(str);
                var delta = (nowRet - now).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }
        }

        [Fact]
        public void MicrosoftDateTimesWithTimeZones()
        {
            var newtonsoft = Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings
            {
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat
            });

            var now = DateTime.UtcNow;

            for (var i = 0; i < 100000; i++)
            {
                var dtUtc = now + TimeSpan.FromMilliseconds(i);
                var dtLocal = dtUtc.ToLocalTime();

                string asStr;
                using (var str = new StringWriter())
                {
                    newtonsoft.Serialize(str, dtLocal);
                    asStr = str.ToString();
                    Assert.True(asStr.Contains('-') || asStr.Contains('+'));
                }

                DateTime shouldMatch, shouldMatchUtc;
                using (var str = new StringReader(asStr))
                {
                    shouldMatch = (DateTime)newtonsoft.Deserialize(str, typeof(DateTime));
                    shouldMatchUtc = shouldMatch.ToUniversalTime();
                }

                DateTime jilDt, jilDtUtc;
                using (var str = new StringReader(asStr))
                {
                    jilDt = JSON.Deserialize<DateTime>(str);
                    jilDtUtc = jilDt.ToUniversalTime();
                }

                Assert.True((dtUtc - shouldMatchUtc).Duration().TotalMilliseconds < 1);
                Assert.True((dtUtc - jilDtUtc).Duration().TotalMilliseconds < 1);
                Assert.True((shouldMatchUtc - jilDtUtc).Duration().TotalMilliseconds == 0);
            }
        }

        [Fact]
        public void Guids()
        {
            var guid = Guid.NewGuid();

            using (var str = new StringReader("\"" + guid.ToString("d").ToUpper() + "\""))
            {
                var g = JSON.Deserialize<Guid>(str);
                Assert.Equal(guid, g);
            }

            using (var str = new StringReader("\"" + guid.ToString("d").ToLower() + "\""))
            {
                var g = JSON.Deserialize<Guid>(str);
                Assert.Equal(guid, g);
            }
        }

        [Fact]
        public void Overflow()
        {
            // byte
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("1234")) JSON.Deserialize<byte>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("257")) JSON.Deserialize<byte>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);
            }

            // sbyte
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("1234")) JSON.Deserialize<sbyte>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("128")) JSON.Deserialize<sbyte>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);

                var ex3 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("-129")) JSON.Deserialize<sbyte>(str); });
                Assert.IsType<OverflowException>(ex3.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex3.Message);
            }

            // short
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("320000")) JSON.Deserialize<short>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("32768")) JSON.Deserialize<short>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);

                var ex3 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("-32769")) JSON.Deserialize<short>(str); });
                Assert.IsType<OverflowException>(ex3.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex3.Message);
            }

            // ushort
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("320000")) JSON.Deserialize<ushort>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("65536")) JSON.Deserialize<ushort>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);
            }

            // int
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("21474830000")) JSON.Deserialize<int>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("2147483648")) JSON.Deserialize<int>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);

                var ex3 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("-2147483649")) JSON.Deserialize<int>(str); });
                Assert.IsType<OverflowException>(ex3.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex3.Message);
            }

            // uint
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("42949670000")) JSON.Deserialize<uint>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("4294967296")) JSON.Deserialize<uint>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);
            }

            // long
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("92233720368547750000")) JSON.Deserialize<long>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("9223372036854775808")) JSON.Deserialize<long>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);

                var ex3 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("-9223372036854775809")) JSON.Deserialize<long>(str); });
                Assert.IsType<OverflowException>(ex3.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex3.Message);
            }

            // ulong
            {
                var ex = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("184467440737095510000")) JSON.Deserialize<ulong>(str); });
                Assert.IsType<OverflowException>(ex.InnerException);
                Assert.Equal("Number did not end when expected, may overflow", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => { using (var str = new StringReader("18446744073709551616")) JSON.Deserialize<ulong>(str); });
                Assert.IsType<OverflowException>(ex2.InnerException);
                Assert.Equal("Arithmetic operation resulted in an overflow.", ex2.Message);
            }
        }

        [Fact]
        public void Decimals()
        {
            for (var i = -11.1m; i <= 22.2m; i += 0.03m)
            {
                var asStr = i.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<decimal>(str);

                    Assert.Equal(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.Equal(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<decimal>(str);

                Assert.Equal(0m, res);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<decimal>(str);

                Assert.Equal(0m, res);
                Assert.Equal(-1, str.Peek());
            }

            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                decimal d = rand.Next() * (rand.Next() % 2 == 0 ? -1 : 1);
                d *= (decimal)rand.NextDouble();

                var asStr = d.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<decimal>(str);

                    Assert.Equal(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.Equal(-1, str.Peek());
                }
            }

            //This string_values are from Mono project
            // mono / mcs / class / corlib / Test / System / DoubleTest.cs

            // DoubleTest.cs - NUnit Test Cases for the System.Double class
            //
            // Bob Doan <bdoan@sicompos.com>
            //
            // (C) Ximian, Inc.  http://www.ximian.com
            // Copyright (C) 2005 Novell, Inc (http://www.novell.com)
            //
            const string sep = ".";
            string[] string_values = new string[7];
            string_values[0] = "1";
            string_values[1] = "1" + sep + "1";
            string_values[2] = "-12";
            string_values[3] = "44" + sep + "444432";
            string_values[4] = "         -221" + sep + "3233";
            string_values[5] = "6" + sep + "28318530717958647692528676655900577";
            string_values[6] = "1e-05";
            foreach (var i in string_values)
            {
                using (var str = new StringReader(i))
                {
                    var res = JSON.Deserialize<decimal>(str);

                    Assert.Equal(decimal.Parse(i, NumberStyles.Float, CultureInfo.InvariantCulture), res);
                    Assert.Equal(-1, str.Peek());
                }
            }
        }

        [Fact]
        public void Doubles()
        {
            for (var i = -11.1; i <= 22.2; i += 0.03)
            {
                var asStr = i.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<double>(str);

                    Assert.Equal(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.Equal(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<double>(str);

                Assert.Equal(0.0, res);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<double>(str);

                Assert.Equal(0.0, res);
                Assert.Equal(-1, str.Peek());
            }

            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                double d = rand.Next() * (rand.Next() % 2 == 0 ? -1 : 1);
                d *= rand.NextDouble();

                var asStr = d.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<double>(str);

                    Assert.Equal(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.Equal(-1, str.Peek());
                }
            }

            //This string_values are from Mono project
            // mono / mcs / class / corlib / Test / System / DoubleTest.cs

            // DoubleTest.cs - NUnit Test Cases for the System.Double class
            //
            // Bob Doan <bdoan@sicompos.com>
            //
            // (C) Ximian, Inc.  http://www.ximian.com
            // Copyright (C) 2005 Novell, Inc (http://www.novell.com)
            //
            const string sep = ".";
            string[] string_values = new string[10];
            string_values[0] = "1";
            string_values[1] = "1" + sep + "1";
            string_values[2] = "-12";
            string_values[3] = "44" + sep + "444432";
            string_values[4] = "         -221" + sep + "3233";
            string_values[5] = " 1" + sep + "7976931348623157e308 ";
            string_values[6] = "-1" + sep + "7976931348623157e308";
            string_values[7] = "4" + sep + "9406564584124650e-324";
            string_values[8] = "6" + sep + "28318530717958647692528676655900577";
            string_values[9] = "1e-05";
            foreach (var i in string_values)
            {
                using (var str = new StringReader(i))
                {
                    var res = JSON.Deserialize<double>(str);

                    Assert.Equal(double.Parse(i, CultureInfo.InvariantCulture), res);
                    Assert.Equal(-1, str.Peek());
                }
            }
        }

        [Fact]
        public void Floats()
        {
            for (var i = -11.1f; i <= 22.2f; i += 0.03f)
            {
                var asStr = i.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<float>(str);

                    Assert.Equal(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.Equal(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<float>(str);

                Assert.Equal(0f, res);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<float>(str);

                Assert.Equal(0f, res);
                Assert.Equal(-1, str.Peek());
            }

            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                float f = rand.Next() * (rand.Next() % 2 == 0 ? -1 : 1);
                f *= (float)rand.NextDouble();

                var asStr = f.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<float>(str);

                    Assert.Equal(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.Equal(-1, str.Peek());
                }
            }

            //This string_values are from Mono project
            // mono / mcs / class / corlib / Test / System / DoubleTest.cs

            // DoubleTest.cs - NUnit Test Cases for the System.Double class
            //
            // Bob Doan <bdoan@sicompos.com>
            //
            // (C) Ximian, Inc.  http://www.ximian.com
            // Copyright (C) 2005 Novell, Inc (http://www.novell.com)
            //
            const string sep = ".";
            string[] string_values = new string[7];
            string_values[0] = "1";
            string_values[1] = "1" + sep + "1";
            string_values[2] = "-12";
            string_values[3] = "44" + sep + "444432";
            string_values[4] = "         -221" + sep + "3233";
            string_values[5] = "6" + sep + "28318530717958647692528676655900577";
            string_values[6] = "1e-05";
            foreach (var i in string_values)
            {
                using (var str = new StringReader(i))
                {
                    var res = JSON.Deserialize<float>(str);

                    Assert.Equal(float.Parse(i, CultureInfo.InvariantCulture), res);
                    Assert.Equal(-1, str.Peek());
                }
            }
        }

        [Fact]
        public void Longs()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.Equal(0L, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(long.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.Equal(long.MaxValue, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(long.MinValue.ToString()))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.Equal(long.MinValue, i);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void ULongs()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.Equal((ulong)0, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(ulong.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.Equal(ulong.MaxValue, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(ulong.MinValue.ToString()))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.Equal(ulong.MinValue, i);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void Ints()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.Equal(0, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(int.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.Equal(int.MaxValue, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(int.MinValue.ToString()))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.Equal(int.MinValue, i);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void UInts()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.Equal((uint)0, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(uint.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.Equal(uint.MaxValue, i);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(uint.MinValue.ToString()))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.Equal(uint.MinValue, i);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void Shorts()
        {
            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<short>(str);

                    Assert.Equal((short)i, b);
                    Assert.Equal(-1, str.Peek());
                }
            }

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<short>(str);

                        Assert.Equal((short)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<short>(str);

                        Assert.Equal((short)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<short>(str);

                        Assert.Equal((short)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }
        }

        [Fact]
        public void UShort()
        {
            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<ushort>(str);

                    Assert.Equal((ushort)i, b);
                    Assert.Equal(-1, str.Peek());
                }
            }

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<ushort>(str);

                        Assert.Equal((ushort)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<ushort>(str);

                        Assert.Equal((ushort)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<ushort>(str);

                        Assert.Equal((ushort)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }
        }

        [Fact]
        public void Bytes()
        {
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<byte>(str);

                    Assert.Equal((byte)i, b);
                    Assert.Equal(-1, str.Peek());
                }
            }

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<byte>(str);

                        Assert.Equal((byte)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<byte>(str);

                        Assert.Equal((byte)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<byte>(str);

                        Assert.Equal((byte)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }
        }

        [Fact]
        public void SBytes()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<sbyte>(str);

                    Assert.Equal((sbyte)i, b);
                    Assert.Equal(-1, str.Peek());
                }
            }

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<sbyte>(str);

                        Assert.Equal((sbyte)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<sbyte>(str);

                        Assert.Equal((sbyte)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<sbyte>(str);

                        Assert.Equal((sbyte)i, b);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }
        }

        [Fact]
        public void Strings()
        {
            using (var str = new StringReader("null"))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Null(c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"a\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("a", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\\\\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("\\", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\/\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("/", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("\b", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\f\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("\f", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\r\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("\r", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\n\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("\n", c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\t\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.Equal("\t", c);
                Assert.Equal(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asStr = "\"\\u" + i.ToString("X4") + "\"";

                using (var str = new StringReader(asStr))
                {
                    var c = JSON.Deserialize<string>(str);

                    var shouldBe = "" + (char)i;

                    Assert.Equal(shouldBe, c);
                    Assert.Equal(-1, str.Peek());
                }
            }

            using (var str = new StringReader("\"abc\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.Equal("abc", s);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"abcd\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.Equal("abcd", s);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\\f\\t\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.Equal("\b\f\t", s);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\\f\\t\\r\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.Equal("\b\f\t\r", s);
                Assert.Equal(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asEncodedChar = "\\u" + i.ToString("X4");

                for (var j = 0; j < 10; j++)
                {
                    var asStr = "\"" + string.Join("", Enumerable.Range(0, j).Select(_ => asEncodedChar)) + "\"";

                    using (var str = new StringReader(asStr))
                    {
                        var c = JSON.Deserialize<string>(str);

                        var shouldBe = new string((char)i, j);

                        Assert.Equal(shouldBe, c);
                        Assert.Equal(-1, str.Peek());
                    }
                }
            }
        }

        [Fact]
        public void Chars()
        {
            using (var str = new StringReader("\"a\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('a', c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\\\\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('\\', c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\/\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('/', c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('\b', c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\f\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('\f', c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\r\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('\r', c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\n\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('\n', c);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\t\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.Equal('\t', c);
                Assert.Equal(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asStr = "\"\\u" + i.ToString("X4") + "\"";

                using (var str = new StringReader(asStr))
                {
                    var c = JSON.Deserialize<char>(str);

                    Assert.Equal(i, (int)c);
                    Assert.Equal(-1, str.Peek());
                }
            }
        }

        enum _Enums : int
        {
            Hello,
            World,
            Foo
        }

        [Fact]
        public void Enums()
        {
            using (var str = new StringReader("\"Hello\""))
            {
                var val = JSON.Deserialize<_Enums>(str);

                Assert.Equal(_Enums.Hello, val);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void Nullables()
        {
            using (var str = new StringReader("1"))
            {
                var val = JSON.Deserialize<int?>(str);

                Assert.Equal((int?)1, val);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<int?>(str);

                Assert.Equal((int?)null, val);
                Assert.Equal(-1, str.Peek());
            }
        }

        [JilPrimitiveWrapper]
        public class PrimitiveWrapperWithDefaultCtor
        {
            public int Value { get; private set; }
        }

        [JilPrimitiveWrapper]
        public class PrimitiveWrapperWithNonDefaultCtor
        {
            public PrimitiveWrapperWithNonDefaultCtor(int value)
            {
                Value = value;
            }

            public int Value { get; private set; }
        }

        [JilPrimitiveWrapper]
        public struct PrimitiveWrapperAsStructWithNonDefaultCtor
        {
            public PrimitiveWrapperAsStructWithNonDefaultCtor(int value)
            {
                Value = value;
            }

            public readonly int Value;
        }

        [JilPrimitiveWrapper]
        public class PrimitiveWrapperWithNonDefaultCtorWithField
        {
            public PrimitiveWrapperWithNonDefaultCtorWithField(int value)
            {
                Value = value;
            }

            public int Value;
        }

        [Fact]
        public void PrimitiveWrappers()
        {
            using (var str = new StringReader("1"))
            {
                var val = JSON.Deserialize<PrimitiveWrapperWithDefaultCtor>(str);

                Assert.Equal(1, val.Value);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("1"))
            {
                var val = JSON.Deserialize<PrimitiveWrapperWithNonDefaultCtor>(str);

                Assert.Equal(1, val.Value);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("1"))
            {
                var val = JSON.Deserialize<PrimitiveWrapperAsStructWithNonDefaultCtor>(str);

                Assert.Equal(1, val.Value);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("1"))
            {
                var val = JSON.Deserialize<PrimitiveWrapperWithNonDefaultCtorWithField>(str);

                Assert.Equal(1, val.Value);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void Bools()
        {
            using (var str = new StringReader("true"))
            {
                var val = JSON.Deserialize<bool>(str);

                Assert.True(val);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("false"))
            {
                var val = JSON.Deserialize<bool>(str);

                Assert.False(val);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void Lists()
        {
            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<List<int>>(str);

                Assert.Null(val);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("[]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.Empty(val);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(" [     ] "))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.Empty(val);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("[1]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.Single(val);
                Assert.Equal(1, val[0]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("[1,2]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.Equal(2, val.Count);
                Assert.Equal(1, val[0]);
                Assert.Equal(2, val[1]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("[1,2,3]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.Equal(3, val.Count);
                Assert.Equal(1, val[0]);
                Assert.Equal(2, val[1]);
                Assert.Equal(3, val[2]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader(" [ 1,2 ,3   ]    "))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.Equal(3, val.Count);
                Assert.Equal(1, val[0]);
                Assert.Equal(2, val[1]);
                Assert.Equal(3, val[2]);
                Assert.Equal(-1, str.Peek());
            }
        }

#pragma warning disable 0649
        class _Objects
        {
            public int A;
            public string B { get; set; }
        }
#pragma warning restore 0649

        [Fact]
        public void Objects()
        {
            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.Null(val);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(123, val.A);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\": \"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal("hello", val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 456, \"B\": \"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(456, val.A);
                Assert.Equal("hello", val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\": \"hello\", \"A\": 456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(456, val.A);
                Assert.Equal("hello", val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\":\"hello\",\"A\":456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(456, val.A);
                Assert.Equal("hello", val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("   {  \"B\"    :   \"hello\"    ,    \"A\"   :   456   }  "))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(456, val.A);
                Assert.Equal("hello", val.B);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void ObjectsSkipMembers()
        {
            using (var str = new StringReader("{\"C\":123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":123.456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }



            using (var str = new StringReader("{\"C\":-123.456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1E12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1E12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1E-12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1E-12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1.1E2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1.1E2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1.1E+2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1.1E+2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":\"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": []}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1,2]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1,2,3]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123, \"B\": 456}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123, \"B\": 456, \"C\": \"hello world\"}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": null, \"CC\": null}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [null], \"CC\": [null]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\":null}, \"CC\": {\"B\":null}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.NotNull(val);
                Assert.Equal(default(int), val.A);
                Assert.Null(val.B);
                Assert.Equal(-1, str.Peek());
            }
        }

#pragma warning disable 0649
        class _RecursiveObjects
        {
            public string A;
            public _RecursiveObjects B;
        }
#pragma warning restore 0649

        [Fact]
        public void RecursiveObjects()
        {
            using (var str = new StringReader("{\"A\": \"hello world\", \"B\": { \"A\": \"foo bar\", \"B\": {\"A\": \"fizz buzz\"}}}"))
            {
                var val = JSON.Deserialize<_RecursiveObjects>(str);
                Assert.NotNull(val);
                Assert.Equal("hello world", val.A);
                Assert.Equal("foo bar", val.B.A);
                Assert.Equal("fizz buzz", val.B.B.A);
                Assert.Null(val.B.B.B);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void Dictionaries()
        {
            using (var str = new StringReader("{\"A\": 123}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.NotNull(val);
                Assert.Single(val);
                Assert.Equal(123, val["A"]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123, \"B\": 456}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.NotNull(val);
                Assert.Equal(2, val.Count);
                Assert.Equal(123, val["A"]);
                Assert.Equal(456, val["B"]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123, \"B\": 456, \"C\": 789}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.NotNull(val);
                Assert.Equal(3, val.Count);
                Assert.Equal(123, val["A"]);
                Assert.Equal(456, val["B"]);
                Assert.Equal(789, val["C"]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": null}"))
            {
                var val = JSON.Deserialize<Dictionary<string, string>>(str);
                Assert.NotNull(val);
                Assert.Single(val);
                Assert.Null(val["A"]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": \"hello\", \"B\": \"world\"}"))
            {
                var val = JSON.Deserialize<Dictionary<string, string>>(str);
                Assert.NotNull(val);
                Assert.Equal(2, val.Count);
                Assert.Equal("hello", val["A"]);
                Assert.Equal("world", val["B"]);
                Assert.Equal(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": {\"A\":123}, \"B\": {\"B\": \"abc\"}, \"C\": {\"A\":456, \"B\":\"fizz\"} }"))
            {
                var val = JSON.Deserialize<Dictionary<string, _Objects>>(str);
                Assert.NotNull(val);
                Assert.Equal(3, val.Count);
                Assert.Equal(123, val["A"].A);
                Assert.Equal("abc", val["B"].B);
                Assert.Equal(456, val["C"].A);
                Assert.Equal("fizz", val["C"].B);
                Assert.Equal(-1, str.Peek());
            }
        }

        [Fact]
        public void ISO8601WeekDates()
        {
            using (var str = new StringReader("\"2009-W01-1\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(2008, 12, 29, 0, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2009W011\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(2008, 12, 29, 0, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2009-W53-7\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(2010, 1, 3, 0, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2009W537\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.Equal(new DateTime(2010, 1, 3, 0, 0, 0, DateTimeKind.Utc), dt);
            }
        }

        [Fact]
        public void Surrogates()
        {
            var data = "abc" + Char.ConvertFromUtf32(Int32.Parse("2A601", System.Globalization.NumberStyles.HexNumber)) + "def";

            Assert.Contains(data, c => char.IsHighSurrogate(c));
            Assert.Contains(data, c => char.IsLowSurrogate(c));

            using (var str = new StringReader("\"" + data + "\""))
            {
                var res = JSON.Deserialize<string>(str);
                Assert.Equal(data, res);
            }
        }

        List<T> AnonObjectByExample<T>(T example, string str)
        {
            var opts = new Options(dateFormat: Jil.DateTimeFormat.ISO8601);
            return JSON.Deserialize<List<T>>(str, opts);
        }

        [Fact]
        public void AnonNulls()
        {
            {
                var example = new { A = 1 };

                var a = AnonObjectByExample(example, "[null, {\"A\":1234}, null]");
                Assert.Equal(3, a.Count);
                Assert.Null(a[0]);
                Assert.NotNull(a[1]);
                Assert.Equal(1234, a[1].A);
                Assert.Null(a[2]);
            }

            {
                var example = new { A = 1 };

                var a = AnonObjectByExample(example, "[null, {\"A\":1234}, null]");
                Assert.Equal(3, a.Count);
                Assert.Null(a[0]);
                Assert.NotNull(a[1]);
                Assert.Equal(1234, a[1].A);
                Assert.Null(a[2]);
            }
        }

        [Fact]
        public void AnonObjects()
        {
            {
                var example =
                    new
                    {
                        A = 1,
                        B = 1.0,
                        C = 1.0f,
                        D = 1.0m,
                        E = "",
                        F = 'a',
                        G = Guid.NewGuid(),
                        H = DateTime.UtcNow,
                        I = new[] { 1, 2, 3 }
                    };

                const string str = "[{\"A\":1234, \"B\": 123.45, \"C\": 678.90, \"E\": \"hello world\", \"F\": \"c\", \"G\": \"EB29803F-A68D-4647-8512-5F0EE906CC90\", \"H\": \"1999-12-31\", \"I\": [1,2,3,4,5,6,7,8,9,10]}, {\"A\":1234, \"B\": 123.45, \"C\": 678.90, \"E\": \"hello world\", \"F\": \"c\", \"G\": \"EB29803F-A68D-4647-8512-5F0EE906CC90\", \"H\": \"1999-12-31\", \"I\": [1,2,3,4,5,6,7,8,9,10]}, {\"A\":1234, \"B\": 123.45, \"C\": 678.90, \"E\": \"hello world\", \"F\": \"c\", \"G\": \"EB29803F-A68D-4647-8512-5F0EE906CC90\", \"H\": \"1999-12-31\", \"I\": [1,2,3,4,5,6,7,8,9,10]}]";

                var res = AnonObjectByExample(example, str);
                Assert.NotNull(res);
                Assert.Equal(3, res.Count);
                var first = res[0];
                Assert.Equal(1234, first.A);
                Assert.Equal(123.45, first.B);
                Assert.Equal(678.90f, first.C);
                Assert.Equal(0m, first.D);
                Assert.Equal("hello world", first.E);
                Assert.Equal('c', first.F);
                Assert.Equal(Guid.Parse("EB29803F-A68D-4647-8512-5F0EE906CC90"), first.G);
                Assert.Equal(new DateTime(1999, 12, 31, 0, 0, 0, DateTimeKind.Utc), first.H);
                Assert.NotNull(first.I);
                Assert.Equal(10, first.I.Length);

                for (var i = 0; i < 10; i++)
                {
                    Assert.Equal(i + 1, first.I[i]);
                }
            }

            {
                var example =
                    new
                    {
                        A = 1,
                        B = 1,
                        C = 1,
                        D = 1,
                        E = 1,
                        F = 1,
                        G = 1,
                        H = 1,
                        I = 1
                    };

                const string str = "[{\"A\":1, \"B\": 2, \"C\": 3, \"E\": 4, \"F\": 5, \"G\": 6, \"H\": 7, \"I\": 8}]";

                var res = AnonObjectByExample(example, str);
                Assert.NotNull(res);
                Assert.Single(res);
                var first = res[0];
                Assert.Equal(1, first.A);
                Assert.Equal(2, first.B);
                Assert.Equal(3, first.C);
                Assert.Equal(0, first.D);
                Assert.Equal(4, first.E);
                Assert.Equal(5, first.F);
                Assert.Equal(6, first.G);
                Assert.Equal(7, first.H);
                Assert.Equal(8, first.I);
            }

            {
                var example =
                    new
                    {
                        A = 1,
                        B = 1.0,
                        C = 1.0f,
                        D = 1.0m,
                        E = "",
                        F = 'a',
                        G = Guid.NewGuid(),
                        H = DateTime.UtcNow,
                        I = new[] { 1, 2, 3 }
                    };

                const string str = "[{\"A\":1234, \"B\": 123.45, \"C\": 678.90, \"E\": \"hello world\", \"F\": \"c\", \"G\": \"EB29803F-A68D-4647-8512-5F0EE906CC90\", \"H\": \"1999-12-31\", \"I\": [1,2,3,4,5,6,7,8,9,10]}, {\"A\":1234, \"B\": 123.45, \"C\": 678.90, \"E\": \"hello world\", \"F\": \"c\", \"G\": \"EB29803F-A68D-4647-8512-5F0EE906CC90\", \"H\": \"1999-12-31\", \"I\": [1,2,3,4,5,6,7,8,9,10]}, {\"A\":1234, \"B\": 123.45, \"C\": 678.90, \"E\": \"hello world\", \"F\": \"c\", \"G\": \"EB29803F-A68D-4647-8512-5F0EE906CC90\", \"H\": \"1999-12-31\", \"I\": [1,2,3,4,5,6,7,8,9,10]}]";

                var res = AnonObjectByExample(example, str);
                Assert.NotNull(res);
                Assert.Equal(3, res.Count);
                var first = res[0];
                Assert.Equal(1234, first.A);
                Assert.Equal(123.45, first.B);
                Assert.Equal(678.90f, first.C);
                Assert.Equal(0m, first.D);
                Assert.Equal("hello world", first.E);
                Assert.Equal('c', first.F);
                Assert.Equal(Guid.Parse("EB29803F-A68D-4647-8512-5F0EE906CC90"), first.G);
                Assert.Equal(new DateTime(1999, 12, 31, 0, 0, 0, DateTimeKind.Utc), first.H);
                Assert.NotNull(first.I);
                Assert.Equal(10, first.I.Length);

                for (var i = 0; i < 10; i++)
                {
                    Assert.Equal(i + 1, first.I[i]);
                }
            }

            {
                var example =
                    new
                    {
                        A = 1,
                        B = 1,
                        C = 1,
                        D = 1,
                        E = 1,
                        F = 1,
                        G = 1,
                        H = 1,
                        I = 1
                    };

                const string str = "[{\"A\":1, \"B\": 2, \"C\": 3, \"E\": 4, \"F\": 5, \"G\": 6, \"H\": 7, \"I\": 8}]";

                var res = AnonObjectByExample(example, str);
                Assert.NotNull(res);
                Assert.Single(res);
                var first = res[0];
                Assert.Equal(1, first.A);
                Assert.Equal(2, first.B);
                Assert.Equal(3, first.C);
                Assert.Equal(0, first.D);
                Assert.Equal(4, first.E);
                Assert.Equal(5, first.F);
                Assert.Equal(6, first.G);
                Assert.Equal(7, first.H);
                Assert.Equal(8, first.I);
            }
        }

        class _DataMemberName
        {
            public string Plain { get; set; }

            [DataMember(Name = "FakeName")]
            public string RealName { get; set; }

#pragma warning disable 0649
            [DataMember(Name = "NotSoSecretName")]
            public int SecretName;
#pragma warning restore 0649
        }

        [Fact]
        public void DataMemberName()
        {
            using (var str = new StringReader("{\"NotSoSecretName\":314159,\"FakeName\":\"Really RealName\",\"Plain\":\"hello world\"}"))
            {
                var obj = JSON.Deserialize<_DataMemberName>(str);

                Assert.NotNull(obj);
                Assert.Equal("hello world", obj.Plain);
                Assert.Equal("Really RealName", obj.RealName);
                Assert.Equal(314159, obj.SecretName);
            }
        }

        static T _EmptyAnonymousObject<T>(T example, string str, Options opts)
        {
            return JSON.Deserialize<T>(str, opts);
        }

        [Fact]
        public void EmptyAnonymousObject()
        {
            var ex = new { };

            {
                {
                    var obj = _EmptyAnonymousObject(ex, "null", Options.Default);
                    Assert.Null(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{}", Options.Default);
                    Assert.NotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234}", Options.Default);
                    Assert.NotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234,\"B\":5678}", Options.Default);
                    Assert.NotNull(obj);
                }
            }

            {
                {
                    var obj = _EmptyAnonymousObject(ex, "null", new Options());
                    Assert.Null(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{}", new Options());
                    Assert.NotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234}", new Options());
                    Assert.NotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234,\"B\":5678}", new Options());
                    Assert.NotNull(obj);
                }
            }
        }

        [Fact]
        public void SystemObject()
        {
            {
                using (var str = new StringReader("null"))
                {
                    var obj = JSON.Deserialize<object>(str);

                    Assert.Null(obj);
                }

                using (var str = new StringReader("{}"))
                {
                    var obj = JSON.Deserialize<object>(str);

                    Assert.NotNull(obj);
                }

                using (var str = new StringReader("{\"A\":1234}"))
                {
                    var obj = JSON.Deserialize<object>(str);

                    Assert.NotNull(obj);
                }
            }

            {
                using (var str = new StringReader("null"))
                {
                    var obj = JSON.Deserialize<object>(str, new Options());

                    Assert.Null(obj);
                }

                using (var str = new StringReader("{}"))
                {
                    var obj = JSON.Deserialize<object>(str, new Options());

                    Assert.NotNull(obj);
                }

                using (var str = new StringReader("{\"A\":1234}"))
                {
                    var obj = JSON.Deserialize<object>(str, new Options());

                    Assert.NotNull(obj);
                }
            }
        }

        class _MissingConstructor
        {
            public int A;
            public double B;

            public _MissingConstructor(int a, double b)
            {
                A = a;
                B = b;
            }
        }

        [Fact]
        public void MissingConstructor()
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_MissingConstructor>("null"));
            Assert.IsType<Jil.Common.ConstructionException>(ex.InnerException);
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_MissingConstructor: Expected a parameterless constructor for JilTests.DeserializeTests+_MissingConstructor", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_MissingConstructor>("null", new Options()));
            Assert.IsType<Jil.Common.ConstructionException>(ex2.InnerException);
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_MissingConstructor: Expected a parameterless constructor for JilTests.DeserializeTests+_MissingConstructor", ex2.Message);
        }

        class _NoNameDataMember
        {
            [DataMember(Order = 1)]
            public int Id { get; set; }
        }

        [Fact]
        public void NoNameDataMember()
        {
            using (var str = new StringReader("{\"Id\":1234}"))
            {
                var res = JSON.Deserialize<_NoNameDataMember>(str);
                Assert.NotNull(res);
                Assert.Equal(1234, res.Id);
            }
        }

        [Theory]
        [InlineData("1.2E10E10", "E10")]
        [InlineData("1.2.3.4.5.6", ".3.4.5.6")]
        [InlineData("1.2E++10", "+10")]
        public void BadDouble(string input, string expectedSnippetAfter)
        {
            using (var str = new StringReader(input))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<double>(str));
                Assert.Equal(expectedSnippetAfter, ex.SnippetAfterError);
            }
        }

        [Theory]
        [InlineData("1.2E10E10", "E10")]
        [InlineData("1.2.3.4.5.6", ".3.4.5.6")]
        [InlineData("1.2E++10", "+10")]
        public void BadFloat(string input, string expectedSnippetAfter)
        {
            using (var str = new StringReader(input))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<float>(str));
                Assert.Equal(expectedSnippetAfter, ex.SnippetAfterError);
            }
        }

        [Theory]
        [InlineData("1.2E10E10", "E10")]
        [InlineData("1.2.3.4.5.6", ".3.4.5.6")]
        [InlineData("1.2E++10", "+10")]
        public void BadDecimal(string input, string expectedSnippetAfter)
        {
            using (var str = new StringReader(input))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<float>(str));
                Assert.Equal(expectedSnippetAfter, ex.SnippetAfterError);
            }
        }

        class _IEnumerableMember
        {
            public IEnumerable<string> A { get; set; }
        }

        [Fact]
        public void IEnumerableMember()
        {
            using (var str = new StringReader("{\"A\":[\"abcd\", \"efgh\"]}"))
            {
                var res = JSON.Deserialize<_IEnumerableMember>(str);
                Assert.NotNull(res);
                Assert.Equal(2, res.A.Count());
                Assert.Equal("abcd", res.A.ElementAt(0));
                Assert.Equal("efgh", res.A.ElementAt(1));
            }
        }

        class _IReadOnlyListMember
        {
            public IReadOnlyList<string> A { get; set; }
        }

        [Fact]
        public void IReadOnlyListMember()
        {
            using (var str = new StringReader("{\"A\":[\"abcd\", \"efgh\"]}"))
            {
                var res = JSON.Deserialize<_IReadOnlyListMember>(str);
                Assert.NotNull(res);
                Assert.Equal(2, res.A.Count());
                Assert.Equal("abcd", res.A.ElementAt(0));
                Assert.Equal("efgh", res.A.ElementAt(1));
            }
        }

        [Fact]
        public void Interface()
        {
            using (var str = new StringReader("{\"A\":1234, \"B\": \"hello world\"}"))
            {
                var res = JSON.Deserialize<_Interface1>(str);
                Assert.NotNull(res);
                Assert.Equal(1234, res.A);
                Assert.Equal("hello world", res.B);
            }

            using (var str = new StringReader("{\"A\":1234, \"B\": \"hello world\", \"C\": 3.14159}"))
            {
                var res = JSON.Deserialize<_Interface2>(str);
                Assert.NotNull(res);
                Assert.Equal(1234, res.A);
                Assert.Equal("hello world", res.B);
                Assert.Equal(3.14159, res.C);
            }

            using (var str = new StringReader("{\"A\":1234, \"B\":4567, \"C\":890}"))
            {
                var res = JSON.Deserialize<_Interface3>(str);
                Assert.NotNull(res);
                Assert.Equal(1234, res.A);
                Assert.Equal(890, res.C);
            }
        }

        class _Issue19
        {
            public bool B { get; set; }
        }

        [Fact]
        public void Issue19()
        {
            using (var str = new StringReader("{\"A\":true}"))
            {
                var res = JSON.Deserialize<_Issue19>(str);
                Assert.NotNull(res);
                Assert.False(res.B);
            }

            using (var str = new StringReader("{\"A\":true, \"B\":false}"))
            {
                var res = JSON.Deserialize<_Issue19>(str);
                Assert.NotNull(res);
                Assert.False(res.B);
            }

            using (var str = new StringReader("{\"A\":false, \"B\":true}"))
            {
                var res = JSON.Deserialize<_Issue19>(str);
                Assert.NotNull(res);
                Assert.True(res.B);
            }
        }

        [Flags]
        enum _FlagsEnum
        {
            A = 1,
            B = 2,
            C = 4
        }

        [Fact]
        public void FlagsEnum()
        {
            Assert.Equal(_FlagsEnum.A, JSON.Deserialize<_FlagsEnum>("\"A\""));
            Assert.Equal(_FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"B\""));
            Assert.Equal(_FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"A, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"A,B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"B, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"B,A\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,A\""));

            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, C\""));
            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,C\""));
            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, B\""));
            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,B\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, B, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, B,C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,B, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,B,C\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, C, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, C,B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,C, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,C,B\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, A, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, A,C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,A, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,A,C\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, C, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, C,A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,C, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,C,A\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, A, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, A,B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,A, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,A,B\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, B, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, B,A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,B, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,B,A\""));
        }

        enum _EnumMemberAttributeDefault
        {
            [EnumMember]
            A = 1,
            [EnumMember]
            B = 2,
            [EnumMember]
            C = 4
        }

        [Fact]
        public void EnumMemberAttributeDefault()
        {
            Assert.Equal(_EnumMemberAttributeDefault.A, JSON.Deserialize<_EnumMemberAttributeDefault>("\"A\""));
            Assert.Equal(_EnumMemberAttributeDefault.B, JSON.Deserialize<_EnumMemberAttributeDefault>("\"B\""));
            Assert.Equal(_EnumMemberAttributeDefault.C, JSON.Deserialize<_EnumMemberAttributeDefault>("\"C\""));
        }

        enum _EnumMemberAttributeOverride
        {
            [EnumMember(Value = "1")]
            A = 1,
            [EnumMember(Value = "2")]
            B = 2,
            [EnumMember(Value = "4")]
            C = 4
        }

        [Fact]
        public void EnumMemberAttributeOverride()
        {
            Assert.Equal(_EnumMemberAttributeOverride.A, JSON.Deserialize<_EnumMemberAttributeOverride>("\"1\""));
            Assert.Equal(_EnumMemberAttributeOverride.B, JSON.Deserialize<_EnumMemberAttributeOverride>("\"2\""));
            Assert.Equal(_EnumMemberAttributeOverride.C, JSON.Deserialize<_EnumMemberAttributeOverride>("\"4\""));
        }

        [Flags]
        enum _EnumMemberAttributeOverrideFlags
        {
            [EnumMember(Value = "1")]
            A = 1,
            [EnumMember(Value = "2")]
            B = 2,
            [EnumMember(Value = "3")]
            C = 4
        }

        [Fact]
        public void EnumMemberAttributeOverrideFlags()
        {
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,1\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,1\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,2\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 2, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 2,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,2, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,2,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 3, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,3, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,3,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 1, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 1,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,1, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,1,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 3, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 3,1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,3, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,3,1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 1, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 1,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,1, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,1,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 2, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 2,1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,2, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,2,1\""));
        }

        enum _CaseInsensitiveEnums
        {
            Foo,
            Bar,
            Fizz
        }

        [Fact]
        public void CaseInsensitiveEnums()
        {
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"foo\""));
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"Foo\""));
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"fOo\""));
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"foO\""));
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"FOo\""));
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"FoO\""));
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"fOO\""));
            Assert.Equal(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"FOO\""));

            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"bar\""));
            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"Bar\""));
            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"bAr\""));
            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"baR\""));
            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"BAr\""));
            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"BaR\""));
            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"bAR\""));
            Assert.Equal(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"BAR\""));

            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fizz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"Fizz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIzz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fiZz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fizZ\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIzz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FiZz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FizZ\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIZz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIzZ\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fiZZ\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIZz\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIzZ\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIZZ\""));
            Assert.Equal(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIZZ\""));
        }

        [Fact]
        public void DynamicMembers()
        {
            const string json = @"{
                  ""index.analysis.analyzer.stem.tokenizer"" : ""standard"",
                  ""index.analysis.analyzer.exact.filter.0"" : ""lowercase"",
                  ""index.refresh_interval"" : ""1s"",
                  ""index.analysis.analyzer.exact.type"" : ""custom"",
                  ""test-dummy-obj"": { ""hello"": 123 }
	        }";

            var dyn = JSON.Deserialize<Dictionary<string, dynamic>>(json);
            Assert.NotNull(dyn);
            Assert.Equal(5, dyn.Count);
            Assert.Equal("standard", (string)dyn["index.analysis.analyzer.stem.tokenizer"]);
            Assert.Equal("lowercase", (string)dyn["index.analysis.analyzer.exact.filter.0"]);
            Assert.Equal("1s", (string)dyn["index.refresh_interval"]);
            Assert.Equal("custom", (string)dyn["index.analysis.analyzer.exact.type"]);
            Assert.NotNull(dyn["test-dummy-obj"]);
            var testDummyObj = dyn["test-dummy-obj"];

            var count = 0;
            foreach (var kv in testDummyObj)
            {
                var key = kv.Key;
                var val = kv.Value;
                count++;

                Assert.Equal("hello", (string)key);
                Assert.Equal(123, (int)val);
            }

            Assert.Equal(1, count);
        }

        class _Issue25
        {
            public int Id { get; set; }
            public __Issue25 Foo { get; set; }
        }

        class __Issue25 {  /* nothing here .. yet */ }

        static T ___Issue25DeserializeByExample<T>(T example, string json, Options opts)
        {
            return JSON.Deserialize<T>(json, opts);
        }

        [Fact]
        public void Issue25()
        {
            const string json = "{ \"Id\" : 17, \"Foo\" : { \"Bar\" : 17} }";

            {
                var res = JSON.Deserialize<_Issue25>(json);

                Assert.Equal(17, res.Id);
                Assert.NotNull(res.Foo);
            }

            {
                var res = JSON.Deserialize<_Issue25>(json, new Options());

                Assert.Equal(17, res.Id);
                Assert.NotNull(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = ___Issue25DeserializeByExample(example, json, Options.Default);

                Assert.Equal(17, res.Id);
                Assert.NotNull(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = ___Issue25DeserializeByExample(example, json, new Options());

                Assert.Equal(17, res.Id);
                Assert.NotNull(res.Foo);
            }
        }

        class _EmptyMembers { }

        [Fact]
        public void EmptyMembers()
        {
            {
                const string str1 = "{}";
                const string str2 = "{\"foo\":0}";
                const string str3 = "{\"foo\":0, \"bar\":0}";
                const string str4 = "{\"foo\":0, \"bar\":0, \"fizz\":0}";

                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str1));
                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str2));
                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str3));
                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str4));
            }

            {
                const string str1 = "{}";
                const string str2 = "{\"foo\":0}";
                const string str3 = "{\"foo\":0, \"bar\":0}";
                const string str4 = "{\"foo\":0, \"bar\":0, \"fizz\":0}";

                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str1, new Options()));
                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str2, new Options()));
                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str3, new Options()));
                Assert.NotNull(JSON.Deserialize<_EmptyMembers>(str4, new Options()));
            }
        }

        class _NullEmptyMembers
        {
            public int Id { get; set; }
            public __NullEmptyMembers Foo { get; set; }
        }

        class __NullEmptyMembers {  /* nothing here .. yet */ }

        static T NullEmptyMembersDeserializeByExample<T>(T example, string json, Options opts)
        {
            return JSON.Deserialize<T>(json, opts);
        }

        [Fact]
        public void NullEmptyMembers()
        {
            const string json = "{ \"Id\" : 17, \"Foo\" : null }";

            {
                var res = JSON.Deserialize<_NullEmptyMembers>(json);

                Assert.Equal(17, res.Id);
                Assert.Null(res.Foo);
            }

            {
                var res = JSON.Deserialize<_NullEmptyMembers>(json, new Options());

                Assert.Equal(17, res.Id);
                Assert.Null(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = NullEmptyMembersDeserializeByExample(example, json, Options.Default);

                Assert.Equal(17, res.Id);
                Assert.Null(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = NullEmptyMembersDeserializeByExample(example, json, new Options());

                Assert.Equal(17, res.Id);
                Assert.Null(res.Foo);
            }
        }

        [Fact]
        public void DateTimeOffsets()
        {
            // ISO8601
            using (var str = new StringReader("\"1900-01-01 12:30Z\""))
            {
                var dto = new DateTimeOffset(1900, 1, 1, 12, 30, 0, TimeSpan.Zero);
                var res = JSON.Deserialize<DateTimeOffset>(str, Options.ISO8601);
                Assert.Equal(dto, res);
            }

            // Newtonsoft
            {
                var newtonsoft =
                    Newtonsoft.Json.JsonSerializer.Create(
                        new Newtonsoft.Json.JsonSerializerSettings
                        {
                            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat
                        }
                    );

                var now = DateTime.UtcNow;
                string newtonsoftStyle;
                using(var str = new StringWriter())
                {
                   newtonsoft.Serialize(str, now);
                   newtonsoftStyle = str.ToString();
                }

                using (var str = new StringReader(newtonsoftStyle))
                {
                    var res = JSON.Deserialize<DateTimeOffset>(str, Options.Default).UtcDateTime;
                    var delta = (now - res).Duration();
                    Assert.True(delta < TimeSpan.FromMilliseconds(1));
                }
            }

            // Milliseconds
            {
                var now = DateTime.UtcNow;
                var asStr = JSON.Serialize(now, Options.MillisecondsSinceUnixEpoch);

                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<DateTimeOffset>(str, Options.MillisecondsSinceUnixEpoch);
                    var delta = (now - res).Duration();
                    Assert.True(delta < TimeSpan.FromMilliseconds(1));
                }
            }

            // Seconds
            {
                var now = DateTime.UtcNow;
                var asStr = JSON.Serialize(now, Options.SecondsSinceUnixEpoch);

                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<DateTimeOffset>(str, Options.SecondsSinceUnixEpoch);
                    var delta = (now - res).Duration();
                    Assert.True(delta < TimeSpan.FromSeconds(1));
                }
            }
        }

        [Fact]
        public void Issue43()
        {
            var shouldMatch = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
            shouldMatch = new DateTime(shouldMatch.Ticks + 5339, DateTimeKind.Utc);
            var dt = JSON.Deserialize<DateTime>("\"2014-08-08T14:04:01.4265339+00:00\"", Options.ISO8601);
            Assert.Equal(shouldMatch, dt);
        }

        public class _Issue48
        {
            public string S { get; set; }
        }

        [Fact]
        public void Issue48()
        {
            var res2 = JSON.Deserialize<string>("\"\\uabcd\"");
            Assert.Equal("\uabcd", res2);

            const string text = "{\"T\":\"\\u003c\"}";
            var res = JSON.Deserialize<_Issue48>(text);
            Assert.NotNull(res);
            Assert.Null(res.S);
            var dyn = JSON.DeserializeDynamic(text);
            Assert.Equal("\u003c", (string)dyn.T);
        }

        [Fact]
        public void IllegalUTF16Char()
        {
            // Ok, this is a pain
            //   There are certain codepoints that are now valid unicode that char.ConvertFromUtf32 can't deal with
            //   What tripped this was \uD83D which is now an emoji, but is considered an illegal surrogate
            //   We have to deal with these somewhat gracefully, even if we can't really turn them into what they
            //   should be...

            var raw = JSON.Deserialize<string>("\"\\uD83D\"");
            Assert.Equal(0xD83D, (int)raw[0]);
        }

        public class _Issue53
        {
            [JilDirective(Ignore = true)]
            public DateTime NotSerializedProperty { get; set; }

            [JilDirective(Name = "NotSerializedProperty")]
            public string SerializedProperty { get; set; }
        }


        [Fact]
        public void Issue53()
        {
            var empty = JSON.Deserialize<_Issue53>("{}");
            Assert.NotNull(empty);
            Assert.Null(empty.SerializedProperty);
            Assert.Equal(default(DateTime), empty.NotSerializedProperty);

            var data = JSON.Deserialize<_Issue53>("{\"NotSerializedProperty\":\"a value!\"}");
            Assert.NotNull(data);
            Assert.Equal("a value!", data.SerializedProperty);
            Assert.Equal(default(DateTime), data.NotSerializedProperty);
        }

        enum _BadEnum1 { A, B };
        enum _BadEnum2 { A, B };

        [Fact]
        public void BadEnum()
        {
            try
            {
                Jil.Deserialize.InlineDeserializer<_BadEnum1>.UseNameAutomataForEnums = false;
                var e = JSON.Deserialize<_BadEnum1>("\"C\"");
                Assert.True(false, "Should have failed, instead got: " + e);
            }
            catch (DeserializationException e)
            {
                Assert.Equal("Unexpected value for _BadEnum1: C", e.Message);
            }
            finally
            {
                Jil.Deserialize.InlineDeserializer<_BadEnum1>.UseNameAutomataForEnums = true;
            }

            try
            {
                Jil.Deserialize.InlineDeserializer<_BadEnum2>.UseNameAutomataForEnums = true;
                var e = JSON.Deserialize<_BadEnum2>("\"C\"");
                Assert.True(false, "Should have failed, instead got: " + e);
            }
            catch (DeserializationException e)
            {
                Assert.Equal("Unexpected value for _BadEnum2", e.Message);
            }
            finally
            {
                Jil.Deserialize.InlineDeserializer<_BadEnum2>.UseNameAutomataForEnums = true;
            }
        }

        enum _EnumEscapes
        {
            Foo,
            Bar,
            Résumé
        }

        [Fact]
        public void EnumEscapes()
        {
            Assert.Equal(_EnumEscapes.Foo, JSON.Deserialize<_EnumEscapes>(@"""F\u006f\u006F"""));
            Assert.Equal(_EnumEscapes.Bar, JSON.Deserialize<_EnumEscapes>(@"""\u0042\u0061\u0072"""));
            Assert.Equal(_EnumEscapes.Résumé, JSON.Deserialize<_EnumEscapes>(@"""R\u00e9sum\u00E9"""));
        }

        class _DeserializeNonGenericClass
        {
            public string A { get; set; }
            public int B { get; set; }
        }

        struct _DeserializeNonGenericStruct
        {
            public string A { get; set; }
            public int B { get; set; }
        }

        [Fact]
        public void DeserializeNonGeneric()
        {
            var a = (_DeserializeNonGenericClass)JSON.Deserialize("{\"A\":\"hello world\", \"B\":123}", typeof(_DeserializeNonGenericClass));
            Assert.NotNull(a);
            Assert.Equal("hello world", a.A);
            Assert.Equal(123, a.B);

            var b = (_DeserializeNonGenericStruct)JSON.Deserialize("{\"A\":\"hello world\", \"B\":123}", typeof(_DeserializeNonGenericStruct));
            Assert.Equal("hello world", b.A);
            Assert.Equal(123, b.B);
        }

        class _Issue73
        {
            public string[] foo { get; set; }
        }

        [Fact]
        public void Issue73()
        {
            {
                var obj = JSON.Deserialize<_Issue73>(@"{""foo"":null}");
                Assert.NotNull(obj);
                Assert.Null(obj.foo);
            }
        }

        class _ExpectedEndOfStream
        {
            public _ExpectedEndOfStream Other { get; set; }
            public string Foo { get; set; }
        }

        [Fact]
        public void ExpectedEndOfStream()
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<string>("\"hello world\"       {"));
            Assert.Equal("Expected end of stream", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_ExpectedEndOfStream>("{\"Other\":{\"Foo\":\"do a thing!\"}, \"Foo\":\"another thing!\"}   dfsfsd"));
            Assert.Equal("Expected end of stream", ex2.Message);
        }

        class _Issue86List : List<string>
        {
            public _Issue86List(string magicString)
            {
                if (magicString != "magic!") throw new Exception();
            }
        }

        class _Issue86Dict : Dictionary<string, string>
        {
            public _Issue86Dict(string magicString)
            {
                if (magicString != "magic!") throw new Exception();
            }
        }

        [Fact]
        public void Issue86()
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_Issue86List>("[\"hello\", \"world\"]"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_Issue86List: Expected a parameterless constructor for JilTests.DeserializeTests+_Issue86List", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_Issue86Dict>("{\"hello\": \"world\"}"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_Issue86Dict: Expected a parameterless constructor for JilTests.DeserializeTests+_Issue86Dict", ex2.Message);
        }

        class _ConvertEnumsToPrimitives
        {
            public enum A : byte { X1, Y1, Z1 }
            public enum B : sbyte { X2, Y2, Z2 }
            public enum C : short { X3, Y3, Z3 }
            public enum D : ushort { X4, Y4, Z4 }
            public enum E : int { X5, Y5, Z5 }
            public enum F : uint { X6, Y6, Z6 }
            public enum G : long { X7, Y7, Z7 }
            public enum H : ulong { X8, Y8, Z8 }

            public A A1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(byte))]
            public A A2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(short))]
            public A A3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ushort))]
            public A A4 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public A A5 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(uint))]
            public A A6 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public A A7 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public A A8 { get; set; }

            public B B1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(sbyte))]
            public B B2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(short))]
            public B B3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public B B4 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public B B5 { get; set; }

            public C C1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(short))]
            public C C2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public C C3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public C C4 { get; set; }

            public D D1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ushort))]
            public D D2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public D D3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(uint))]
            public D D4 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public D D5 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public D D6 { get; set; }

            public E E1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public E E2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public E E3 { get; set; }

            public F F1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(uint))]
            public F F2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public F F3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public F F4 { get; set; }

            public G G1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public G G2 { get; set; }

            public H H1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public H H2 { get; set; }
        }

        [Fact]
        public void ConvertEnumsToPrimitives()
        {
            var res = JSON.Deserialize<_ConvertEnumsToPrimitives>("{\n \"G1\": \"X7\",\n \"G2\": 1,\n \"H1\": \"X8\",\n \"H2\": 1,\n \"E1\": \"X5\",\n \"E2\": 1,\n \"E3\": 2,\n \"F1\": \"X6\",\n \"F2\": 1,\n \"F3\": 2,\n \"F4\": 0,\n \"C1\": \"X3\",\n \"C2\": 1,\n \"C3\": 2,\n \"C4\": 0,\n \"D1\": \"X4\",\n \"D2\": 1,\n \"D3\": 2,\n \"D4\": 0,\n \"D5\": 1,\n \"D6\": 2,\n \"A1\": \"X1\",\n \"A2\": 1,\n \"A3\": 2,\n \"A4\": 0,\n \"A5\": 1,\n \"A6\": 2,\n \"A7\": 0,\n \"A8\": 1,\n \"B1\": \"X2\",\n \"B2\": 1,\n \"B3\": 2,\n \"B4\": 0,\n \"B5\": 1\n}");

            Assert.NotNull(res);
            Assert.True(res.A1 == _ConvertEnumsToPrimitives.A.X1);
            Assert.True(res.A2 == _ConvertEnumsToPrimitives.A.Y1);
            Assert.True(res.A3 == _ConvertEnumsToPrimitives.A.Z1);
            Assert.True(res.A4 == _ConvertEnumsToPrimitives.A.X1);
            Assert.True(res.A5 == _ConvertEnumsToPrimitives.A.Y1);
            Assert.True(res.A6 == _ConvertEnumsToPrimitives.A.Z1);
            Assert.True(res.A7 == _ConvertEnumsToPrimitives.A.X1);
            Assert.True(res.A8 == _ConvertEnumsToPrimitives.A.Y1);
            Assert.True(res.B1 == _ConvertEnumsToPrimitives.B.X2);
            Assert.True(res.B2 == _ConvertEnumsToPrimitives.B.Y2);
            Assert.True(res.B3 == _ConvertEnumsToPrimitives.B.Z2);
            Assert.True(res.B4 == _ConvertEnumsToPrimitives.B.X2);
            Assert.True(res.B5 == _ConvertEnumsToPrimitives.B.Y2);
            Assert.True(res.C1 == _ConvertEnumsToPrimitives.C.X3);
            Assert.True(res.C2 == _ConvertEnumsToPrimitives.C.Y3);
            Assert.True(res.C3 == _ConvertEnumsToPrimitives.C.Z3);
            Assert.True(res.C4 == _ConvertEnumsToPrimitives.C.X3);
            Assert.True(res.D1 == _ConvertEnumsToPrimitives.D.X4);
            Assert.True(res.D2 == _ConvertEnumsToPrimitives.D.Y4);
            Assert.True(res.D3 == _ConvertEnumsToPrimitives.D.Z4);
            Assert.True(res.D4 == _ConvertEnumsToPrimitives.D.X4);
            Assert.True(res.D5 == _ConvertEnumsToPrimitives.D.Y4);
            Assert.True(res.D6 == _ConvertEnumsToPrimitives.D.Z4);
            Assert.True(res.E1 == _ConvertEnumsToPrimitives.E.X5);
            Assert.True(res.E2 == _ConvertEnumsToPrimitives.E.Y5);
            Assert.True(res.E3 == _ConvertEnumsToPrimitives.E.Z5);
            Assert.True(res.F1 == _ConvertEnumsToPrimitives.F.X6);
            Assert.True(res.F2 == _ConvertEnumsToPrimitives.F.Y6);
            Assert.True(res.F3 == _ConvertEnumsToPrimitives.F.Z6);
            Assert.True(res.F4 == _ConvertEnumsToPrimitives.F.X6);
            Assert.True(res.G1 == _ConvertEnumsToPrimitives.G.X7);
            Assert.True(res.G2 == _ConvertEnumsToPrimitives.G.Y7);
            Assert.True(res.H1 == _ConvertEnumsToPrimitives.H.X8);
            Assert.True(res.H2 == _ConvertEnumsToPrimitives.H.Y8);
        }

        [Fact]
        public void SecondsTimeSpan()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            timeSpans.Add(default(TimeSpan));

            foreach (var ts1 in timeSpans)
            {
                var json = JSON.Serialize(ts1, Options.SecondsSinceUnixEpoch);
                var ts2 = JSON.Deserialize<TimeSpan>(json, Options.SecondsSinceUnixEpoch);

                Assert.Equal(Math.Round(ts1.TotalSeconds), Math.Round(ts2.TotalSeconds));
            }
        }

        [Fact]
        public void MillisecondsTimeSpan()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            timeSpans.Add(default(TimeSpan));

            foreach (var ts1 in timeSpans)
            {
                var json = JSON.Serialize(ts1, Options.MillisecondsSinceUnixEpoch);
                var ts2 = JSON.Deserialize<TimeSpan>(json, Options.MillisecondsSinceUnixEpoch);

                Assert.Equal(Math.Round(ts1.TotalMilliseconds), Math.Round(ts2.TotalMilliseconds));
            }
        }

        [Fact]
        public void MicrosoftTimeSpan()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            timeSpans.Add(default(TimeSpan));

            foreach (var ts1 in timeSpans)
            {
                var json = JSON.Serialize(ts1, Options.Default);
                var ts2 = JSON.Deserialize<TimeSpan>(json, Options.Default);

                Assert.Equal(Math.Round(ts1.TotalMilliseconds), Math.Round(ts2.TotalMilliseconds));
            }
        }

        [Fact]
        public void ISO8601TimeSpan()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            timeSpans.Add(default(TimeSpan));

            foreach (var ts1 in timeSpans)
            {
                var json = JSON.Serialize(ts1, Options.ISO8601);
                var ts2 = JSON.Deserialize<TimeSpan>(json, Options.ISO8601);
                var ts4 = JSON.Deserialize<TimeSpan>(json.Replace('.', ','), Options.ISO8601);

                var txtJson = json.Replace("\"", "");
                var ts3 = System.Xml.XmlConvert.ToTimeSpan(txtJson);

                Assert.Equal(ts2, ts4);
                Assert.Equal(Math.Round(ts1.TotalMilliseconds), Math.Round(ts2.TotalMilliseconds));
                Assert.Equal(Math.Round(ts3.TotalMilliseconds), Math.Round(ts2.TotalMilliseconds));
            }
        }

        [Fact]
        public void ISO8601TimeSpan_YearsMonth()
        {
            var rand = new Random();
            var timeSpans = new List<string>();

            for (var i = 0; i < 1000; i++)
            {
                var y = rand.Next(10000);
                var m = rand.Next(100);

                var str = "P" + y + "Y" + m + "M";
                if (rand.Next(2) == 0)
                {
                    str = "-" + str;
                }

                timeSpans.Add(str);
            }

            foreach (var str in timeSpans)
            {
                var shouldMatch = System.Xml.XmlConvert.ToTimeSpan(str);
                var ts = JSON.Deserialize<TimeSpan>("\"" + str + "\"", Options.ISO8601);

                Assert.Equal(shouldMatch.Ticks, ts.Ticks);
            }
        }

        [Fact]
        public void ISO8601TimeSpan_Weeks()
        {
            var rand = new Random();
            var timeSpans = new List<Tuple<int, string>>();

            for (var i = 0; i < 1000; i++)
            {
                var w = rand.Next(10000);

                var str = "P" + w + "W";
                if (rand.Next(2) == 0)
                {
                    w = -w;
                    str = "-" + str;
                }

                timeSpans.Add(Tuple.Create(w, str));
            }

            foreach (var t in timeSpans)
            {
                var w = t.Item1;
                var str = t.Item2;
                var ts = JSON.Deserialize<TimeSpan>("\"" + str + "\"", Options.ISO8601);

                Assert.Equal(w, ts.TotalDays / 7);
            }
        }

        class _PrivateConstructor_Object
        {
            public string A { get; set; }
            public int B { get; set; }

            private _PrivateConstructor_Object() { }
        }

        [Fact]
        public void PrivateConstructor_Object()
        {
            var res = JSON.Deserialize<_PrivateConstructor_Object>("{\"A\":\"hello world\", \"B\": 12345}");
            Assert.Equal("hello world", res.A);
            Assert.Equal(12345, res.B);
        }

        class _PrivateConstructor_List : List<string>
        {
            private _PrivateConstructor_List() : base() { }
        }

        [Fact]
        public void PrivateConstructor_List()
        {
            var res = JSON.Deserialize<_PrivateConstructor_List>("[\"hello\", \"world\"]");
            Assert.Equal(2, res.Count);
            Assert.Equal("hello", res[0]);
            Assert.Equal("world", res[1]);
        }

        class _PrivateConstructor_Dictionary : Dictionary<string, int>
        {
            private _PrivateConstructor_Dictionary() : base() { }
        }

        [Fact]
        public void PrivateConstructor_Dictionary()
        {
            var res = JSON.Deserialize<_PrivateConstructor_Dictionary>("{\"hello\": 123, \"world\":456, \"foo\":789}");
            Assert.Equal(3, res.Count);
            Assert.Equal(123, res["hello"]);
            Assert.Equal(456, res["world"]);
            Assert.Equal(789, res["foo"]);
        }

        class _SeekNotSupported : Stream
        {
            readonly byte[] Data;
            int Index = 0;

            public _SeekNotSupported(string str)
            {
                Data = Encoding.UTF8.GetBytes(str);
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void Flush()
            {

            }

            public override long Length
            {
                get { throw new NotImplementedException(); }
            }

            public override long Position
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (count < 0) throw new Exception();

                if (Index == Data.Length) return 0;

                buffer[offset] = Data[Index];
                Index++;
                return 1;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void SeekNotSupported()
        {
            using(var str = new _SeekNotSupported("{\"hello\": 123, \"world\":456, \"foo\":789}"))
            using(var reader = new StreamReader(str))
            {
                var res = JSON.Deserialize<Dictionary<string, int>>(reader);
                Assert.Equal(3, res.Count);
                Assert.Equal(123, res["hello"]);
                Assert.Equal(456, res["world"]);
                Assert.Equal(789, res["foo"]);
            }

            using (var str = new _SeekNotSupported("{\"hello\": 123, \"world\":456, \"foo\":789}"))
            using (var reader = new StreamReader(str))
            {
                var res = (Dictionary<string, int>)JSON.Deserialize(reader, typeof(Dictionary<string, int>));
                Assert.Equal(3, res.Count);
                Assert.Equal(123, res["hello"]);
                Assert.Equal(456, res["world"]);
                Assert.Equal(789, res["foo"]);
            }

            using (var str = new _SeekNotSupported("{\"hello\": 123, \"world\":456, \"foo\":789}"))
            using (var reader = new StreamReader(str))
            {
                var res = JSON.DeserializeDynamic(reader);
                Assert.Equal(123, (int)res.hello);
                Assert.Equal(456, (int)res.world);
                Assert.Equal(789, (int)res.foo);
            }
        }

        [Fact]
        public void EmptyArrayWithSpace()
        {
            var res = JSON.Deserialize<object[]>("[ ]");
            Assert.NotNull(res);
            Assert.Empty(res);
        }

        [Fact]
        public void DateTimeOffsetPreservesOffset()
        {
            var toTest = new List<DateTimeOffset>();
            toTest.Add(DateTimeOffset.Now);

            for (var h = 0; h <= 14; h++)
            {
                for (var m = 0; m < 60; m++)
                {
                    if (h == 14 && m > 0) continue;

                    var offsetPos = new TimeSpan(h, m, 0);
                    var offsetNeg = offsetPos.Negate();

                    var now = DateTime.Now;
                    now = DateTime.SpecifyKind(now, DateTimeKind.Unspecified);

                    toTest.Add(new DateTimeOffset(now, offsetPos));
                    toTest.Add(new DateTimeOffset(now, offsetNeg));
                }
            }

            foreach (var testDto in toTest)
            {
                var strStr = JSON.Serialize(testDto, Options.ISO8601);
                var dto = JSON.Deserialize<DateTimeOffset>(strStr, Options.ISO8601);

                Assert.Equal(testDto.Year, dto.Year);
                Assert.Equal(testDto.Month, dto.Month);
                Assert.Equal(testDto.Day, dto.Day);
                Assert.Equal(testDto.Hour, dto.Hour);
                Assert.Equal(testDto.Minute, dto.Minute);
                Assert.Equal(testDto.Second, dto.Second);
                Assert.Equal(testDto.Millisecond, dto.Millisecond);
                Assert.Equal(testDto.Offset.Hours, dto.Offset.Hours);
                Assert.Equal(testDto.Offset.Minutes, dto.Offset.Minutes);
            }
        }

        class _EarlyStreamEnds_Int
        {
            public int A { get; set; }
        }

        [Fact]
        public void EarlyStreamEnds()
        {
            using (var str = new StringReader("{\"A\":"))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_EarlyStreamEnds_Int>(str));
                Assert.True(ex.EndedUnexpectedly);
            }

            using (var str = new StringReader("{\"A\""))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_EarlyStreamEnds_Int>(str));
                Assert.True(ex.EndedUnexpectedly);
            }

            using (var str = new StringReader("{"))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_EarlyStreamEnds_Int>(str));
                Assert.True(ex.EndedUnexpectedly);
            }

            using(var str = new StringReader("\"123"))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<Guid>(str));
                Assert.True(ex.EndedUnexpectedly);
            }

            using (var str = new StringReader("[0, "))
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<int[]>(str));
                Assert.True(ex.EndedUnexpectedly);
            }
        }

        static void _Issue117_TrailingDot<T>() where T : struct
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("1."));
            Assert.Equal("Number cannot end with .", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("12."));
            Assert.Equal("Number cannot end with .", ex2.Message);

            var ex3 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("123."));
            Assert.Equal("Number cannot end with .", ex3.Message);
        }

        static void _Issue117_NegativeTrailingDot<T>() where T : struct
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("-1."));
            Assert.Equal("Number cannot end with .", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("-12."));
            Assert.Equal("Number cannot end with .", ex2.Message);

            var ex3 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("-123."));
            Assert.Equal("Number cannot end with .", ex3.Message);
        }

        static void _Issue117_LeadingZero<T>() where T : struct
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("01"));
            Assert.Equal("Number cannot have leading zeros", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("001"));
            Assert.Equal("Number cannot have leading zeros", ex2.Message);
        }

        static void _Issue117_NegativeLeadingZero<T>() where T : struct
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("-01"));
            Assert.Equal("Number cannot have leading zeros", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<T>("-001"));
            Assert.Equal("Number cannot have leading zeros", ex2.Message);
        }

        [Fact]
        public void Issue117_Decimal()
        {
            _Issue117_TrailingDot<decimal>();
            _Issue117_NegativeTrailingDot<decimal>();
            _Issue117_LeadingZero<decimal>();
            _Issue117_NegativeLeadingZero<decimal>();
        }

        [Fact]
        public void Issue117_Float()
        {
            _Issue117_TrailingDot<float>();
            _Issue117_NegativeTrailingDot<float>();
            _Issue117_LeadingZero<float>();
            _Issue117_NegativeLeadingZero<float>();
        }

        [Fact]
        public void Issue117_Double()
        {
            _Issue117_TrailingDot<double>();
            _Issue117_NegativeTrailingDot<double>();
            _Issue117_LeadingZero<double>();
            _Issue117_NegativeLeadingZero<double>();
        }

        [Fact]
        public void Issue117_SByte()
        {
            _Issue117_LeadingZero<sbyte>();
            _Issue117_NegativeLeadingZero<sbyte>();
        }

        [Fact]
        public void Issue117_Byte()
        {
            _Issue117_LeadingZero<byte>();
        }

        [Fact]
        public void Issue117_UShort()
        {
            _Issue117_LeadingZero<ushort>();
        }

        [Fact]
        public void Issue117_Short()
        {
            _Issue117_LeadingZero<short>();
            _Issue117_NegativeLeadingZero<short>();
        }

        [Fact]
        public void Issue117_UInt()
        {
            _Issue117_LeadingZero<uint>();
        }

        [Fact]
        public void Issue117_Int()
        {
            _Issue117_LeadingZero<int>();
            _Issue117_NegativeLeadingZero<int>();
        }

        [Fact]
        public void Issue117_ULong()
        {
            _Issue117_LeadingZero<ulong>();
        }

        [Fact]
        public void Issue117_Long()
        {
            _Issue117_LeadingZero<long>();
            _Issue117_NegativeLeadingZero<long>();
        }

#if !DEBUG
        [Fact]
        public void RFC1123DateTimes()
        {
            var toTest = new List<DateTime>();

            var now = DateTime.UtcNow;
            toTest.Add(now);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            for (var i = 0; i < 357; i++)
            {
                toTest.Add(epoch + TimeSpan.FromDays(i));
                for (var j = 0; j < 24; j++)
                {
                    toTest.Add(epoch + TimeSpan.FromDays(i) + TimeSpan.FromHours(j));
                    for (var k = 0; k < 60; k++)
                    {
                        toTest.Add(epoch + TimeSpan.FromDays(i) + TimeSpan.FromHours(j) + TimeSpan.FromMinutes(k));
                        for (var l = 0; l < 60; l++)
                        {
                            toTest.Add(epoch + TimeSpan.FromDays(i) + TimeSpan.FromHours(j) + TimeSpan.FromMinutes(k) + TimeSpan.FromSeconds(l));
                        }
                    }
                }
            }

            foreach (var dt in toTest)
            {
                var json = "\"" + dt.ToString("R") + "\"";
                var res = JSON.Deserialize<DateTime>(json, Options.RFC1123);

                var diff = dt - res;
                Assert.True(diff.TotalSeconds < 1);
            }
        }
#endif

        [Fact]
        public void NaNFails()
        {
            Assert.Throws<DeserializationException>(() => JSON.Deserialize<float>("NaN"));
            Assert.Throws<DeserializationException>(() => JSON.Deserialize<double>("NaN"));
            Assert.Throws<DeserializationException>(() => JSON.Deserialize<decimal>("NaN"));
        }

        [Fact]
        public void Issue126()
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<decimal>("\"20.00\""));
            Assert.Equal("Expected a decimal value", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<float>("\"20.00\""));
            Assert.Equal("Expected a float value", ex2.Message);

            var ex3 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<double>("\"20.00\""));
            Assert.Equal("Expected a double value", ex3.Message);
        }

        [Fact]
        public void Issue128()
        {
            const string str = "{\"Fields\":[{\"Field\":{\"Name\":{\"en\":\"Type\"}},\"Container\":{\"Name\":{\"en\":\"tt\"}}}]}";
            using (var s = new StringReader(str))
            {
                var ret = JSON.Deserialize<DocumentType>(s);
                Assert.NotNull(ret);
            }
        }

        public class DocumentType
        {
            public IList<DocumentTypeField> Fields { get; set; }
        }

        public class DocumentTypeField
        {
            public FieldDescriptor Field { get; set; }
            public FieldDescriptor Container { get; set; }
        }

        public class FieldDescriptor
        {
            public Dictionary<string, string> Name { get; set; }
            [JilDirective(true)]
            public System.Collections.Hashtable a { get; set; }
        }

        [Fact]
        public void MicrosoftDateTimeOffsets()
        {
            // While *DateTimes* in Microsoft format don't do anything with the offset (they just write it, then ignore it),
            //   DateTimeOffsets do.

            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            var dtos =
                new[]
                {
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(1)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(-1)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(2)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(-2)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(3)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(-3)),
                    new DateTimeOffset(1234, 5, 6, 7, 8, 9, new TimeSpan(4, 30, 00)),
                    new DateTimeOffset(1234, 5, 6, 7, 8, 9, new TimeSpan(-4, -30, 00)),
                };

            foreach (var dto in dtos)
            {
                var val = Newtonsoft.Json.JsonConvert.SerializeObject(dto, settings);

                // with streams
                using (var str = new StringReader(val))
                {
                    var res = JSON.Deserialize<DateTimeOffset>(str);
                    Assert.Equal(dto.UtcTicks, res.UtcTicks);
                    Assert.Equal(dto.Offset, res.Offset);
                }

                // with strings
                {
                    var res = JSON.Deserialize<DateTimeOffset>(val);
                    Assert.Equal(dto.UtcTicks, res.UtcTicks);
                    Assert.Equal(dto.Offset, res.Offset);
                }
            }
        }

        class _SimpleDiscriminateUnion
        {
            [JilDirective(Name = "Data", IsUnion = true)]
            public string AsStr { get; set; }
            [JilDirective(Name = "Data", IsUnion = true)]
            public int AsInt { get; set; }
        }

        [Fact]
        public void SimpleDiscriminateUnion()
        {
            const string StrJSON = "{\"Data\": \"hello world\"}";
            const string IntJSON = "{\"Data\": 314159}";

            var asStr = JSON.Deserialize<_SimpleDiscriminateUnion>(StrJSON);
            Assert.NotNull(asStr);
            Assert.Equal("hello world", asStr.AsStr);
            Assert.Equal(0, asStr.AsInt);

            var asInt = JSON.Deserialize<_SimpleDiscriminateUnion>(IntJSON);
            Assert.NotNull(asInt);
            Assert.Equal(314159, asInt.AsInt);
            Assert.Null(asInt.AsStr);
        }

        class _ComplicatedDiscriminateUnion_1
        {
            [JilDirective(Name = "Data", IsUnion = true)]
            public DateTime ISODateTime { get; set; }
            [JilDirective(Name = "Data", IsUnion = true)]
            public int Number { get; set; }
            [JilDirective(Name = "Data", IsUnion = true)]
            public bool Boolean { get; set; }
        }

        class _ComplicatedDiscriminateUnion_2
        {
            [JilDirective(Name = "Data", IsUnion = true)]
            public Guid AsGuid { get; set; }

            [JilDirective(Name = "Data", IsUnion = true)]
            public DateTime UnixDateTime{ get; set; }
        }

        class _ComplicatedDiscriminateUnion_3
        {
            [JilDirective(Name = "Data", IsUnion = true)]
            public uint AsUInt { get; set; }

            [JilDirective(Name = "Data", IsUnion = true)]
            public TimeSpan AsTimeSpan { get; set; }
        }

        class _ComplicatedDiscriminateUnion_4
        {
            [JilDirective(Name = "A", IsUnion = true)]
            public string A_AsString { get; set; }
            [JilDirective(Name = "A", IsUnion = true)]
            public double A_AsDouble { get; set; }
            [JilDirective(Name = "A", IsUnion = true, IsUnionType = true)]
            public Type A_Type { get; set; }

            [JilDirective(Name = "B", IsUnion = true)]
            public DateTime B_AsDateTime { get; set; }
            [JilDirective(Name = "B", IsUnion = true)]
            public int B_AsInt { get; set; }
            [JilDirective(Name = "B", IsUnion = true)]
            public List<string> B_AsList { get; set; }
            [JilDirective(Name = "B", IsUnion = true, IsUnionType = true)]
            public Type B_Type { get; set; }

            public int NonUnion { get; set; }
        }

        [Fact]
        public void ComplicatedDiscriminateUnion()
        {
            {
                const string DateJSON = "{\"Data\": \"2015-06-20T11:11\"}";
                const string IntJSON = "{\"Data\": 314159}";
                const string BoolJSON = "{\"Data\":true}";

                var asDate = JSON.Deserialize<_ComplicatedDiscriminateUnion_1>(DateJSON, Options.ISO8601);
                Assert.NotNull(asDate);
                Assert.Equal(new DateTime(2015, 06, 20, 11, 11, 00, 00, DateTimeKind.Utc), asDate.ISODateTime);
                Assert.Equal(0, asDate.Number);
                Assert.False(asDate.Boolean);

                var asInt = JSON.Deserialize<_ComplicatedDiscriminateUnion_1>(IntJSON, Options.ISO8601);
                Assert.NotNull(asInt);
                Assert.Equal(default(DateTime), asInt.ISODateTime);
                Assert.Equal(314159, asInt.Number);
                Assert.False(asInt.Boolean);

                var asBool = JSON.Deserialize<_ComplicatedDiscriminateUnion_1>(BoolJSON, Options.ISO8601);
                Assert.NotNull(asBool);
                Assert.Equal(default(DateTime), asBool.ISODateTime);
                Assert.Equal(0, asBool.Number);
                Assert.True(asBool.Boolean);
            }

            {
                var now = DateTime.UtcNow;
                const string GuidJSON = "{\"Data\":\"E4A278B7-DA54-459A-9D1F-2FE1C82EE4CB\"}";
                var IntDateJSON = "{\"Data\":" + JSON.Serialize(now, Options.SecondsSinceUnixEpoch) + "}";

                var asGuid = JSON.Deserialize<_ComplicatedDiscriminateUnion_2>(GuidJSON, Options.SecondsSinceUnixEpoch);
                Assert.NotNull(asGuid);
                Assert.Equal(Guid.Parse("E4A278B7-DA54-459A-9D1F-2FE1C82EE4CB"), asGuid.AsGuid);
                Assert.Equal(default(DateTime), asGuid.UnixDateTime);

                var asDate = JSON.Deserialize<_ComplicatedDiscriminateUnion_2>(IntDateJSON, Options.SecondsSinceUnixEpoch);
                Assert.NotNull(asDate);
                Assert.Equal(default(Guid), asDate.AsGuid);
                var dateDiff = (now - asDate.UnixDateTime).Duration();
                Assert.True(dateDiff.TotalSeconds <= 1);
            }

            {
                var ts = new TimeSpan(10, 9, 8, 7, 6);
                const string UIntJSON = "{\"Data\": 1234567}";
                var TimeSpanJSON = "{\"Data\": " + JSON.Serialize(ts) + "}";

                var asUInt = JSON.Deserialize<_ComplicatedDiscriminateUnion_3>(UIntJSON);
                Assert.NotNull(asUInt);
                Assert.Equal(default(TimeSpan), asUInt.AsTimeSpan);
                Assert.Equal(1234567U, asUInt.AsUInt);

                var asTimeSpan = JSON.Deserialize<_ComplicatedDiscriminateUnion_3>(TimeSpanJSON);
                Assert.NotNull(asTimeSpan);
                Assert.Equal(default(uint), asTimeSpan.AsUInt);
                Assert.Equal(ts, asTimeSpan.AsTimeSpan);
            }

            {
                var now = DateTime.UtcNow;
                const string EmptyJson = "{}";
                const string A_StrJSON = "{\"A\": \"hello world\"}";
                const string A_DoubleJSON = "{\"A\": 3.1415}";

                var B_DateTimeJSON = "{\"B\": " + JSON.Serialize(now, Options.Default) + "}";
                const string B_IntJSON = "{\"B\": 8675}";
                const string B_ListJSON = "{\"B\": [\"foo\", \"bar\"]}";

                var A_Str_B_DateTimeJSON = "{\"A\": \"hello world\", \"B\": " + JSON.Serialize(now, Options.Default) + "}";
                const string A_Str_B_IntJSON = "{\"A\": \"hello world\", \"B\": 8675}";
                const string A_Str_B_ListJSON = "{\"A\": \"hello world\", \"B\": [\"foo\", \"bar\"]}";

                var A_Double_B_DateTimeJSON = "{\"A\": 3.1415, \"B\": " + JSON.Serialize(now, Options.Default) + "}";
                const string A_Double_B_IntJSON = "{\"A\": 3.1415, \"B\": 8675}";
                const string A_Double_B_ListJSON = "{\"A\": 3.1415, \"B\": [\"foo\", \"bar\"]}";

                var empty = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(EmptyJson);
                Assert.NotNull(empty);
                Assert.Equal(default(double), empty.A_AsDouble);
                Assert.Null(empty.A_AsString);
                Assert.Null(empty.A_Type);
                Assert.Equal(default(DateTime), empty.B_AsDateTime);
                Assert.Equal(default(int), empty.B_AsInt);
                Assert.Null(empty.B_AsList);
                Assert.Null(empty.B_Type);

                var aStr = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_StrJSON);
                Assert.NotNull(aStr);
                Assert.Equal(default(double), aStr.A_AsDouble);
                Assert.Equal("hello world", aStr.A_AsString);
                Assert.Equal(typeof(string), aStr.A_Type);
                Assert.Equal(default(DateTime), aStr.B_AsDateTime);
                Assert.Equal(default(int), aStr.B_AsInt);
                Assert.Null(aStr.B_AsList);
                Assert.Null(aStr.B_Type);

                var aDouble = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_DoubleJSON);
                Assert.NotNull(aDouble);
                Assert.Equal(3.1415, aDouble.A_AsDouble);
                Assert.Null(aDouble.A_AsString);
                Assert.Equal(typeof(double), aDouble.A_Type);
                Assert.Equal(default(DateTime), aDouble.B_AsDateTime);
                Assert.Equal(default(int), aDouble.B_AsInt);
                Assert.Null(aDouble.B_AsList);
                Assert.Null(aDouble.B_Type);

                var bDateTime = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(B_DateTimeJSON);
                Assert.NotNull(bDateTime);
                Assert.Equal(default(double), bDateTime.A_AsDouble);
                Assert.Null(bDateTime.A_AsString);
                Assert.Null(bDateTime.A_Type);
                var bdtOffset = (now - bDateTime.B_AsDateTime).Duration();
                Assert.True(bdtOffset.TotalMilliseconds <= 1);
                Assert.Equal(default(int), bDateTime.B_AsInt);
                Assert.Null(bDateTime.B_AsList);
                Assert.Equal(typeof(DateTime), bDateTime.B_Type);

                var bInt = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(B_IntJSON);
                Assert.NotNull(bInt);
                Assert.Equal(default(double), bInt.A_AsDouble);
                Assert.Null(bInt.A_AsString);
                Assert.Null(bInt.A_Type);
                Assert.Equal(default(DateTime), bInt.B_AsDateTime);
                Assert.Equal(8675, bInt.B_AsInt);
                Assert.Null(bInt.B_AsList);
                Assert.Equal(typeof(int), bInt.B_Type);

                var bList = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(B_ListJSON);
                Assert.NotNull(bList);
                Assert.Equal(default(double), bList.A_AsDouble);
                Assert.Null(bList.A_AsString);
                Assert.Null(bList.A_Type);
                Assert.Equal(default(DateTime), bList.B_AsDateTime);
                Assert.Equal(default(int), bList.B_AsInt);
                Assert.NotNull(bList.B_AsList);
                Assert.Equal(2, bList.B_AsList.Count);
                Assert.Equal("foo", bList.B_AsList[0]);
                Assert.Equal("bar", bList.B_AsList[1]);
                Assert.Equal(typeof(List<string>), bList.B_Type);

                var aStrBDateTime = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_Str_B_DateTimeJSON);
                Assert.NotNull(aStrBDateTime);
                Assert.Equal(default(double), aStrBDateTime.A_AsDouble);
                Assert.Equal("hello world", aStrBDateTime.A_AsString);
                Assert.Equal(typeof(string), aStrBDateTime.A_Type);
                var asbdtOffset = (now - aStrBDateTime.B_AsDateTime).Duration();
                Assert.True(asbdtOffset.TotalMilliseconds <= 1);
                Assert.Equal(default(int), aStrBDateTime.B_AsInt);
                Assert.Null(aStrBDateTime.B_AsList);
                Assert.Equal(typeof(DateTime), aStrBDateTime.B_Type);

                var aStrBInt = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_Str_B_IntJSON);
                Assert.NotNull(aStrBInt);
                Assert.Equal(default(double), aStrBInt.A_AsDouble);
                Assert.Equal("hello world", aStrBInt.A_AsString);
                Assert.Equal(typeof(string), aStrBInt.A_Type);
                Assert.Equal(default(DateTime), aStrBInt.B_AsDateTime);
                Assert.Equal(8675, aStrBInt.B_AsInt);
                Assert.Null(aStrBInt.B_AsList);
                Assert.Equal(typeof(int), aStrBInt.B_Type);

                var aStrBList = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_Str_B_ListJSON);
                Assert.NotNull(aStrBList);
                Assert.Equal(default(double), aStrBList.A_AsDouble);
                Assert.Equal("hello world", aStrBList.A_AsString);
                Assert.Equal(typeof(string), aStrBList.A_Type);
                Assert.Equal(default(DateTime), aStrBList.B_AsDateTime);
                Assert.Equal(default(int), aStrBList.B_AsInt);
                Assert.NotNull(aStrBList.B_AsList);
                Assert.Equal(2, aStrBList.B_AsList.Count);
                Assert.Equal("foo", aStrBList.B_AsList[0]);
                Assert.Equal("bar", aStrBList.B_AsList[1]);
                Assert.Equal(typeof(List<string>), aStrBList.B_Type);

                var aDoubleBDateTime = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_Double_B_DateTimeJSON);
                Assert.NotNull(aDoubleBDateTime);
                Assert.Equal(3.1415, aDoubleBDateTime.A_AsDouble);
                Assert.Null(aDoubleBDateTime.A_AsString);
                Assert.Equal(typeof(double), aDoubleBDateTime.A_Type);
                var adbdtOffset = (now - aDoubleBDateTime.B_AsDateTime).Duration();
                Assert.True(adbdtOffset.TotalMilliseconds <= 1);
                Assert.Equal(default(int), aDoubleBDateTime.B_AsInt);
                Assert.Null(aDoubleBDateTime.B_AsList);
                Assert.Equal(typeof(DateTime), aDoubleBDateTime.B_Type);

                var aDoubleBInt = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_Double_B_IntJSON);
                Assert.NotNull(aDoubleBInt);
                Assert.Equal(3.1415, aDoubleBInt.A_AsDouble);
                Assert.Null(aDoubleBInt.A_AsString);
                Assert.Equal(typeof(double), aDoubleBInt.A_Type);
                Assert.Equal(default(DateTime), aDoubleBInt.B_AsDateTime);
                Assert.Equal(8675, aDoubleBInt.B_AsInt);
                Assert.Null(aDoubleBInt.B_AsList);
                Assert.Equal(typeof(int), aDoubleBInt.B_Type);

                var aDoubleBList = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(A_Double_B_ListJSON);
                Assert.NotNull(aDoubleBList);
                Assert.Equal(3.1415, aDoubleBList.A_AsDouble);
                Assert.Null(aDoubleBList.A_AsString);
                Assert.Equal(typeof(double), aDoubleBList.A_Type);
                Assert.Equal(default(DateTime), aDoubleBList.B_AsDateTime);
                Assert.Equal(default(int), aDoubleBList.B_AsInt);
                Assert.NotNull(aDoubleBList.B_AsList);
                Assert.Equal(2, aDoubleBList.B_AsList.Count);
                Assert.Equal("foo", aDoubleBList.B_AsList[0]);
                Assert.Equal("bar", aDoubleBList.B_AsList[1]);
                Assert.Equal(typeof(List<string>), aDoubleBList.B_Type);
            }

            {
                const string NonUnion = "{\"NonUnion\": 123}";
                const string NonUnion_A = "{\"NonUnion\": 123, \"A\": \"hello world\"}";
                const string NonUnion_B = "{\"NonUnion\": 123, \"B\": [\"hello\", \"world\"]}";
                const string NonUnion_A_B = "{\"NonUnion\": 123, \"A\": \"hello world\", \"B\": [\"hello\", \"world\"]}";

                var nu = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(NonUnion);
                Assert.NotNull(nu);
                Assert.Equal(123, nu.NonUnion);
                Assert.Equal(default(double), nu.A_AsDouble);
                Assert.Null(nu.A_AsString);
                Assert.Null(nu.A_Type);
                Assert.Equal(default(DateTime), nu.B_AsDateTime);
                Assert.Equal(default(int), nu.B_AsInt);
                Assert.Null(nu.B_AsList);
                Assert.Null(nu.B_Type);

                var nua = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(NonUnion_A);
                Assert.NotNull(nua);
                Assert.Equal(123, nua.NonUnion);
                Assert.Equal(default(double), nua.A_AsDouble);
                Assert.Equal("hello world", nua.A_AsString);
                Assert.Equal(typeof(string), nua.A_Type);
                Assert.Equal(default(DateTime), nua.B_AsDateTime);
                Assert.Equal(default(int), nua.B_AsInt);
                Assert.Null(nua.B_AsList);
                Assert.Null(nua.B_Type);

                var nub = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(NonUnion_B);
                Assert.NotNull(nub);
                Assert.Equal(123, nub.NonUnion);
                Assert.Equal(default(double), nub.A_AsDouble);
                Assert.Null(nub.A_AsString);
                Assert.Null(nub.A_Type);
                Assert.Equal(default(DateTime), nub.B_AsDateTime);
                Assert.Equal(default(int), nub.B_AsInt);
                Assert.NotNull(nub.B_AsList);
                Assert.Equal(2, nub.B_AsList.Count);
                Assert.Equal("hello", nub.B_AsList[0]);
                Assert.Equal("world", nub.B_AsList[1]);
                Assert.Equal(typeof(List<string>), nub.B_Type);

                var nuab = JSON.Deserialize<_ComplicatedDiscriminateUnion_4>(NonUnion_A_B);
                Assert.NotNull(nuab);
                Assert.Equal(123, nuab.NonUnion);
                Assert.Equal(default(double), nuab.A_AsDouble);
                Assert.Equal("hello world", nuab.A_AsString);
                Assert.Equal(typeof(string), nuab.A_Type);
                Assert.Equal(default(DateTime), nuab.B_AsDateTime);
                Assert.Equal(default(int), nuab.B_AsInt);
                Assert.NotNull(nuab.B_AsList);
                Assert.Equal(2, nuab.B_AsList.Count);
                Assert.Equal("hello", nuab.B_AsList[0]);
                Assert.Equal("world", nuab.B_AsList[1]);
                Assert.Equal(typeof(List<string>), nuab.B_Type);
            }
        }

        class _UnionMisconfigured_1
        {
            [JilDirective(Name = "A", IsUnion = true)]
            public DateTime A_DateTime { get; set; }
            [JilDirective(Name = "A", IsUnion = true)]
            public string A_String { get; set; }
        }

        class _UnionMisconfigured_2
        {
            [JilDirective(Name = "A", IsUnion = true)]
            public int A_Int { get; set; }
            [JilDirective(Name = "A", IsUnion = true)]
            public double A_Double { get; set; }
        }

        class _UnionMisconfigured_3
        {
            [JilDirective(Name = "A", IsUnion = true)]
            public _UnionMisconfigured_1 A_Object { get; set; }
            [JilDirective(Name = "A", IsUnion = true)]
            public Dictionary<string, string> A_Dictionary { get; set; }
        }

        class _UnionMisconfigured_4
        {
            [JilDirective(Name = "A", IsUnion = true)]
            public TimeSpan A_TimeSpan { get; set; }
            [JilDirective(Name = "A", IsUnion = true)]
            public string A_String { get; set; }
        }

        class _UnionMisconfigured_5
        {
            [JilDirective(Name = "A", IsUnion = true)]
            public Guid A_Guid { get; set; }
            [JilDirective(Name = "A", IsUnion = true)]
            public string A_String { get; set; }
        }

        class _UnionMisconfigured_6
        {
            [JilDirective(Name = "A", IsUnion = true)]
            public Guid A_Guid { get; set; }
            [JilDirective(Name = "A", IsUnion = true)]
            public DateTime A_DateTime { get; set; }
        }

        [Fact]
        public void UnionMisconfigured()
        {
            {
                JSON.Deserialize<_UnionMisconfigured_1>("{}", Options.SecondsSinceUnixEpoch);
                JSON.Deserialize<_UnionMisconfigured_1>("{}", Options.MillisecondsSinceUnixEpoch);

                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_1>("{}", Options.Default));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_1: The members  [A_DateTime, A_String] cannot be distiguished in a union because they can each start with these characters [\"]", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_1>("{}", Options.ISO8601));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_1: The members  [A_DateTime, A_String] cannot be distiguished in a union because they can each start with these characters [\"]", ex2.Message);

                var ex3 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_1>("{}", Options.RFC1123));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_1: The members  [A_DateTime, A_String] cannot be distiguished in a union because they can each start with these characters [\"]", ex3.Message);
            }

            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_2>("{}", Options.Default));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_2: The members  [A_Int, A_Double] cannot be distiguished in a union because they can each start with these characters [-, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9]", ex.Message);
            }

            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_3>("{}", Options.Default));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_3: The members  [A_Object, A_Dictionary] cannot be distiguished in a union because they can each start with these characters [{]", ex.Message);
            }

            {
                JSON.Deserialize<_UnionMisconfigured_4>("{}", Options.SecondsSinceUnixEpoch);
                JSON.Deserialize<_UnionMisconfigured_4>("{}", Options.MillisecondsSinceUnixEpoch);

                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_4>("{}", Options.Default));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_4: The members  [A_TimeSpan, A_String] cannot be distiguished in a union because they can each start with these characters [\"]", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_4>("{}", Options.ISO8601));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_4: The members  [A_TimeSpan, A_String] cannot be distiguished in a union because they can each start with these characters [\"]", ex2.Message);

                var ex3 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_4>("{}", Options.RFC1123));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_4: The members  [A_TimeSpan, A_String] cannot be distiguished in a union because they can each start with these characters [\"]", ex3.Message);
            }

            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_5>("{}", Options.Default));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_5: The members  [A_Guid, A_String] cannot be distiguished in a union because they can each start with these characters [\"]", ex.Message);
            }

            {
                JSON.Deserialize<_UnionMisconfigured_6>("{}", Options.SecondsSinceUnixEpoch);
                JSON.Deserialize<_UnionMisconfigured_6>("{}", Options.MillisecondsSinceUnixEpoch);

                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_6>("{}", Options.Default));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_6: The members  [A_Guid, A_DateTime] cannot be distiguished in a union because they can each start with these characters [\"]", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_6>("{}", Options.ISO8601));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_6: The members  [A_Guid, A_DateTime] cannot be distiguished in a union because they can each start with these characters [\"]", ex2.Message);

                var ex3 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionMisconfigured_6>("{}", Options.RFC1123));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionMisconfigured_6: The members  [A_Guid, A_DateTime] cannot be distiguished in a union because they can each start with these characters [\"]", ex3.Message);
            }
        }

        class _MultipleNullableDiscriminantUnion
        {
            [JilDirective(Name = "Data", IsUnion = true)]
            public string AsStr { get; set; }
            [JilDirective(Name = "Data", IsUnion = true)]
            public int AsInt { get; set; }
            [JilDirective(Name = "Data", IsUnion = true)]
            public bool? AsNullableBool { get; set; }
            [JilDirective(Name = "Data", IsUnion = true)]
            public List<int> AsList { get; set; }
            [JilDirective(Name = "Data", IsUnion = true, IsUnionType = true)]
            public Type UnionType { get; set; }
        }

        [Fact]
        public void MultipleNullableDiscriminantUnion()
        {
            {
                const string StrJson = "{\"Data\": \"hello world\"}";
                const string IntJson = "{\"Data\": 1234}";
                const string NullableBoolJson = "{\"Data\": true}";
                const string ListJson = "{\"Data\": [1,2,3]}";
                const string NullJson = "{\"Data\": null}";

                var asStr = JSON.Deserialize<_MultipleNullableDiscriminantUnion>(StrJson);
                Assert.NotNull(asStr);
                Assert.Equal("hello world", asStr.AsStr);
                Assert.Equal(typeof(string), asStr.UnionType);

                var asInt = JSON.Deserialize<_MultipleNullableDiscriminantUnion>(IntJson);
                Assert.NotNull(asInt);
                Assert.Equal(1234, asInt.AsInt);
                Assert.Equal(typeof(int), asInt.UnionType);

                var asNullableBool = JSON.Deserialize<_MultipleNullableDiscriminantUnion>(NullableBoolJson);
                Assert.NotNull(asNullableBool);
                Assert.True(asNullableBool.AsNullableBool);
                Assert.Equal(typeof(bool?), asNullableBool.UnionType);

                var asList = JSON.Deserialize<_MultipleNullableDiscriminantUnion>(ListJson);
                Assert.NotNull(asList);
                Assert.NotNull(asList.AsList);
                Assert.Equal(3, asList.AsList.Count);
                Assert.Equal(1, asList.AsList[0]);
                Assert.Equal(2, asList.AsList[1]);
                Assert.Equal(3, asList.AsList[2]);
                Assert.Equal(typeof(List<int>), asList.UnionType);

                var asNull = JSON.Deserialize<_MultipleNullableDiscriminantUnion>(NullJson);
                Assert.NotNull(asNull);
                Assert.Null(asNull.AsStr);
                Assert.Null(asNull.AsList);
                Assert.Equal(default(int), asNull.AsInt);
                Assert.Equal(default(bool?), asNull.AsNullableBool);
                Assert.Null(asNull.UnionType);
            }
        }

        class _UnionAttributeError
        {
            public string Data { get; set; }

            [JilDirective(Name = "Data")]
            public int AsInt { get; set; }
        }

        [Fact]
        public void UnionAttributeError()
        {
            const string json = "{\"Data\": 31415 }";

            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionAttributeError>(json));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionAttributeError: Member [Data] isn't marked as part of a union, but other members share the same Name [Data]", ex.Message);
        }

        class _NonDiscriminantUnion
        {
            [JilDirective(IsUnion = true)]
            public string Data { get; set; }

            [JilDirective(Name = "Data", IsUnion = true)]
            public DateTime AsDateTime { get; set; }
        }

        [Fact]
        public void NonDiscriminantUnion()
        {
            const string json = "{\"Data\": \"hello world\" }";

            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_NonDiscriminantUnion>(json));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_NonDiscriminantUnion: The members  [Data, AsDateTime] cannot be distiguished in a union because they can each start with these characters [\"]", ex.Message);
        }

        class _UnionTypeIndication
        {
            [JilDirective(Name = "Data", IsUnion = true)]
            public string AsStr { get; set; }
            [JilDirective(Name = "Data", IsUnion = true)]
            public int AsInt { get; set; }
            [JilDirective(Name = "Data", IsUnion = true, IsUnionType = true)]
            public Type WhatType { get; set; }
        }

        [Fact]
        public void UnionTypeIndication()
        {
            const string StrJSON = "{\"Data\": \"hello world\"}";
            const string IntJSON = "{\"Data\": 314159}";
            const string EmptyJSON = "{}";

            var asStr = JSON.Deserialize<_UnionTypeIndication>(StrJSON);
            Assert.NotNull(asStr);
            Assert.Equal("hello world", asStr.AsStr);
            Assert.Equal(0, asStr.AsInt);
            Assert.Equal(typeof(string), asStr.WhatType);

            var asInt = JSON.Deserialize<_UnionTypeIndication>(IntJSON);
            Assert.NotNull(asInt);
            Assert.Equal(314159, asInt.AsInt);
            Assert.Null(asInt.AsStr);
            Assert.Equal(typeof(int), asInt.WhatType);

            var empty = JSON.Deserialize<_UnionTypeIndication>(EmptyJSON);
            Assert.NotNull(empty);
            Assert.Null(empty.AsStr);
            Assert.Equal(0, empty.AsInt);
            Assert.Null(empty.WhatType);
        }

        class _UnionTypeMisconfiguration_1
        {
            [JilDirective(IsUnion = true, Name = "Data")]
            public string AStr { get; set; }
            [JilDirective(IsUnion = true, Name = "Data")]
            public int AInt { get; set; }

            [JilDirective(IsUnion = true, Name = "Data", IsUnionType = true)]
            public string Data_Type { get; set; }
        }

        class _UnionTypeMisconfiguration_2
        {
            [JilDirective(IsUnion = true, Name = "Data")]
            public string AStr { get; set; }
            [JilDirective(IsUnion = true, Name = "Data")]
            public int AInt { get; set; }

            [JilDirective(IsUnion = true, Name = "Data", IsUnionType = true)]
            public Type Data_Type1 { get; set; }
            [JilDirective(IsUnion = true, Name = "Data", IsUnionType = true)]
            public Type Data_Type2 { get; set; }
        }

        public class SerializationNameFormatsDeserialization_CamelCase
        {
            public string oneTwo { get; set; }
            public string ThreeFour { get; set; }
            public string FIVESIX { get; set; }
        }

        public class SerializationNameFormatsDeserialization_AttributeNames
        {
            [DataMember(Name = "ExplicitMember")]
            public string MemberProperty { get; set; }

            [JilDirective(Name = "Directive")]
            public string DirectiveProperty { get; set; }

            public string NekkidProperty { get; set; }
        }

        [Fact]
        public void SerializationNameFormatsDeserialization()
        {
            const string verbtaimStr = "{\"FIVESIX\":\"3\",\"ThreeFour\":\"2\",\"oneTwo\":\"1\"}";
            var verbatimDynamic = JSON.DeserializeDynamic(verbtaimStr, new Options(serializationNameFormat: SerializationNameFormat.Verbatim));

            Assert.Equal("1", (string)verbatimDynamic.oneTwo);
            Assert.Equal("2", (string)verbatimDynamic.ThreeFour);
            Assert.Equal("3", (string)verbatimDynamic.FIVESIX);

            var verbatimStatic = JSON.Deserialize<SerializationNameFormatsDeserialization_CamelCase>(verbtaimStr, new Options(serializationNameFormat: SerializationNameFormat.Verbatim));

            Assert.Equal("1", verbatimStatic.oneTwo);
            Assert.Equal("2", verbatimStatic.ThreeFour);
            Assert.Equal("3", verbatimStatic.FIVESIX);

            const string camelStr = "{\"fivesix\":\"3\",\"threeFour\":\"2\",\"oneTwo\":\"1\"}";
            var camelDynamic = JSON.DeserializeDynamic(camelStr, new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

            Assert.Equal("1", (string)camelDynamic.oneTwo);
            Assert.Equal("2", (string)camelDynamic.threeFour);
            Assert.Equal("3", (string)camelDynamic.fivesix);

            var camelStatic = JSON.Deserialize<SerializationNameFormatsDeserialization_CamelCase>(camelStr, new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

            Assert.Equal("1", camelStatic.oneTwo);
            Assert.Equal("2", camelStatic.ThreeFour);
            Assert.Equal("3", camelStatic.FIVESIX);

            const string verbatimStr2 = "{\"NekkidProperty\":\"NekkidValie\",\"Directive\":\"DirectiveValue\",\"ExplicitMember\":\"MemberValue\"}";
            var verbatimStatic2 = JSON.Deserialize<SerializationNameFormatsDeserialization_AttributeNames>(verbatimStr2, new Options(serializationNameFormat: SerializationNameFormat.Verbatim));

            Assert.Equal("NekkidValie", verbatimStatic2.NekkidProperty);
            Assert.Equal("DirectiveValue", verbatimStatic2.DirectiveProperty);
            Assert.Equal("MemberValue", verbatimStatic2.MemberProperty);

            const string camelStr2 = "{\"nekkidProperty\":\"NekkidValie\",\"Directive\":\"DirectiveValue\",\"ExplicitMember\":\"MemberValue\"}";
            var camelStatic2 = JSON.Deserialize<SerializationNameFormatsDeserialization_AttributeNames>(camelStr2, new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

            Assert.Equal("NekkidValie", camelStatic2.NekkidProperty);
            Assert.Equal("DirectiveValue", camelStatic2.DirectiveProperty);
            Assert.Equal("MemberValue", camelStatic2.MemberProperty);
        }

        // Also see DeserializeTests._Issue150
        class _Issue150
        {
            public enum A { A, B, C }

            public class _InArray<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public T[] ArrayOfEnum = null;
            }

            public class _InList<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public List<T> ListOfEnum = null;
            }

            public class _InListProp<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public List<T> ListOfEnum { get; set; }
            }

            public class _InEnumerable<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public IEnumerable<T> EnumerableOfEnum = null;
            }

            public class _AsDictionaryKey<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public Dictionary<T, int> DictionaryWithEnumKey = null;
            }

            public class _AsDictionaryValue<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public Dictionary<int, T> DictionaryWithEnumValue = null;
            }
        }

        [Fact]
        public void Issue150()
        {
            {
                var obj = JSON.Deserialize<_Issue150._InArray<_Issue150.A>>("{\"ArrayOfEnum\":[0,1]}");
                Assert.Equal(new[] { _Issue150.A.A, _Issue150.A.B }, obj.ArrayOfEnum);
            }

            {
                var obj = JSON.Deserialize<_Issue150._InArray<_Issue150.A?>>("{\"ArrayOfEnum\":[0,null]}");
                Assert.Equal(new _Issue150.A?[] { _Issue150.A.A, null }, obj.ArrayOfEnum);
            }

            {
                var obj = JSON.Deserialize<_Issue150._InList<_Issue150.A>>("{\"ListOfEnum\":[0,1]}");
                Assert.Equal(new[] { _Issue150.A.A, _Issue150.A.B }, obj.ListOfEnum);
            }

            {
                var obj = JSON.Deserialize<_Issue150._InList<_Issue150.A?>>("{\"ListOfEnum\":[0,null]}");
                Assert.Equal(new _Issue150.A?[] { _Issue150.A.A, null }, obj.ListOfEnum);
            }

            {
                var obj = JSON.Deserialize<_Issue150._InListProp<_Issue150.A>>("{\"ListOfEnum\":[0,1]}");
                Assert.Equal(new[] { _Issue150.A.A, _Issue150.A.B }, obj.ListOfEnum);
            }

            {
                var obj = JSON.Deserialize<_Issue150._InListProp<_Issue150.A?>>("{\"ListOfEnum\":[0,null]}");
                Assert.Equal(new _Issue150.A?[] { _Issue150.A.A, null }, obj.ListOfEnum);
            }

            {
                var obj = JSON.Deserialize<_Issue150._AsDictionaryKey<_Issue150.A>>("{\"DictionaryWithEnumKey\":{\"0\":10,\"1\":20}}");
                Assert.Equal(new Dictionary<_Issue150.A, int>
                {
                    { _Issue150.A.A, 10 },
                    { _Issue150.A.B, 20 }
                }, obj.DictionaryWithEnumKey);
            }

            {
                var obj = JSON.Deserialize<_Issue150._AsDictionaryValue<_Issue150.A>>("{\"DictionaryWithEnumValue\":{\"10\":0,\"20\":1}}");
                Assert.Equal(new Dictionary<int, _Issue150.A>
                {
                    { 10, _Issue150.A.A },
                    { 20, _Issue150.A.B }
                }, obj.DictionaryWithEnumValue);
            }

            {
                var obj = JSON.Deserialize<_Issue150._AsDictionaryValue<_Issue150.A?>>("{\"DictionaryWithEnumValue\":{\"10\":0,\"20\":null}}");
                Assert.Equal(new Dictionary<int, _Issue150.A?>
                {
                    { 10, _Issue150.A.A },
                    { 20, null }
                }, obj.DictionaryWithEnumValue);
            }
        }

        // Also see SeserializeTests._Issue151
        class _Issue151
        {
            public enum A { A, B, C }

            [JilDirective(TreatEnumerationAs = typeof(int))]
            public A? NullableEnum = null;
        }

        [Fact]
        public void Issue151()
        {
            {
                var obj = JSON.Deserialize<_Issue151>("{\"NullableEnum\":1}");
                Assert.True(obj.NullableEnum.HasValue);
                Assert.Equal(_Issue151.A.B, obj.NullableEnum.Value);
            }

            {
                var obj = JSON.Deserialize<_Issue151>("{\"NullableEnum\":null}");
                Assert.False(obj.NullableEnum.HasValue);
            }
        }

        [Fact]
        public void Issue162()
        {
            var res = JSON.Deserialize<ICollection<int>>("[1, 2, 3]");
            Assert.NotNull(res);
            Assert.Equal(3, res.Count);
            Assert.Equal(1, res.ElementAt(0));
            Assert.Equal(2, res.ElementAt(1));
            Assert.Equal(3, res.ElementAt(2));
        }

        [JilPrimitiveWrapper]
        class _BadPrimitiveWrapper1
        {
            public int Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        [JilPrimitiveWrapper]
        class _BadPrimitiveWrapper2
        {

        }

        [JilPrimitiveWrapper]
        class _BadPrimitiveWrapper3
        {
#pragma warning disable 0649
            public int Field1;
            public int Field2;
#pragma warning restore 0649
        }

        [JilPrimitiveWrapper]
        class _BadPrimitiveWrapper4
        {
            public int A1;

            public _BadPrimitiveWrapper4(int a1, int a2)
            {
                A1 = a1;
            }
        }

        [Fact]
        public void BadPrimitiveWrapper()
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_BadPrimitiveWrapper1>("1"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_BadPrimitiveWrapper1: Primitive wrappers can only have 1 declared primitive member, found 2 for _BadPrimitiveWrapper1", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_BadPrimitiveWrapper2>("1"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_BadPrimitiveWrapper2: Primitive wrappers can only have 1 declared primitive member, found 0 for _BadPrimitiveWrapper2", ex2.Message);

            var ex3 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_BadPrimitiveWrapper3>("1"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_BadPrimitiveWrapper3: Primitive wrappers can only have 1 declared primitive member, found 2 for _BadPrimitiveWrapper3", ex3.Message);

            var ex4 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_BadPrimitiveWrapper4>("1"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_BadPrimitiveWrapper4: Primitive wrapper JilTests.DeserializeTests+_BadPrimitiveWrapper4 needs a default constructor, or a constructor taking a single System.Int32 parameter", ex4.Message);
        }

        [Fact]
        public void UnionTypeMisconfiguration()
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionTypeMisconfiguration_1>("{}"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionTypeMisconfiguration_1: Member [Data_Type] has IsUnionType set, but is not of type System.Type", ex.Message);

            var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_UnionTypeMisconfiguration_2>("{}"));
            Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_UnionTypeMisconfiguration_2: Member [Data_Type2] has IsUnionType set, but IsUnionType is also set for [Data_Type1]", ex2.Message);
        }

        public enum _Issue193Enum
        {
            T001 = 0,
            T002 = 1,
            T003 = 2,
            T004 = 3,
            T005 = 4,
            T006 = 11,
            T007 = 12,
            T008 = 13,
            T009 = 21,
            T010 = 22,
            T011 = 23,
            T012 = 31,
            T013 = 32,
            T014 = 33,
            T015 = 34,
            T016 = 35,
            T017 = 36,
            T018 = 37,
            T019 = 41,
            T020 = 42,
            T021 = 43,
            T022 = 51,
            T023 = 52,
            T024 = 53,
            T025 = 61,
            T026 = 63,
            T027 = 62,
            T028 = 64,
            T029 = 71,
            T030 = 72,
            T031 = 81,
            T032 = 82,
            T033 = 83,
            T034 = 91,
            T035 = 101,
            T036 = 102,
            T037 = 103,
            T038 = 111,
            T039 = 112,
            T040 = 113,
            T041 = 121,
            T042 = 131,
            T043 = 141,
            T044 = 142,
            T045 = 143,
            T046 = 144,
            T047 = 145,
            T048 = 146,
            T049 = 147,
            T050 = 148,
            T051 = 149,

            /* Comment out T052 and it works fine */
            T052 = 148,
        }

        public class _Issue193
        {
            public _Issue193Enum[] AnEnumArray { get; set; }
        }

        [Fact]
        public void Issue193()
        {
            var data = JSON.Deserialize<_Issue193>("{\"AnEnumArray\":[\"T052\",\"T050\",\"T001\"]}");
            Assert.NotNull(data);
            Assert.NotNull(data.AnEnumArray);
            Assert.Equal(3, data.AnEnumArray.Length);
            Assert.Equal(_Issue193Enum.T052, data.AnEnumArray[0]);
            Assert.Equal(_Issue193Enum.T050, data.AnEnumArray[1]);
            Assert.Equal(_Issue193Enum.T001, data.AnEnumArray[2]);
        }

        [Flags]
        public enum _Issue210_1
        {
            A = 1,
            B = 2
        }
        [Flags]
        public enum _Issue210_2 : long
        {
            A = 1,
            B = 2
        }

        [Fact]
        public void Issue210()
        {
            var res1 = Jil.JSON.Deserialize<_Issue210_1>(Jil.JSON.Serialize<_Issue210_1>(_Issue210_1.A));
            Assert.Equal(_Issue210_1.A, res1);

            var res2 = Jil.JSON.Deserialize<_Issue210_2>(Jil.JSON.Serialize<_Issue210_2>(_Issue210_2.A));
            Assert.Equal(_Issue210_2.A, res2);
        }

        [Flags]
        public enum _Issue203
        {
            Type1 = 1 << 0,
            Type2 = 1 << 1,
            Type3 = 1 << 2,
            Type4 = 1 << 3
        }

        [Fact]
        public void Issue203()
        {
            var json = JSON.Serialize((_Issue203)0);

            var res = JSON.Deserialize<_Issue203>(json);
            Assert.Equal((_Issue203)0, res);
        }

        enum _EmptyEnum
        {
            // nothing here, by design
        }

#pragma warning disable 0649
        class _EmptyEnumWrapper1
        {
            [JilDirective(Ignore = true)]
            public _EmptyEnum A;
        }

        class _EmptyEnumWrapper2
        {
            public _EmptyEnum? A;
        }

        class _EmptyEnumWrapper3
        {
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public _EmptyEnum A;
        }

        class _EmptyEnumWrapper4
        {
            public _EmptyEnum A;
        }
#pragma warning restore 0649

        [Fact]
        public void EmptyEnum()
        {
            {
                var ex = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_EmptyEnum>("0"));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_EmptyEnum: JilTests.DeserializeTests+_EmptyEnum has no values, and cannot be deserialized; add a value, make nullable, or configure to treat as integer", ex.Message);

                var ex2 = Assert.Throws<DeserializationException>(() => JSON.Deserialize<_EmptyEnumWrapper4>("null"));
                Assert.Equal("Error occurred building a deserializer for JilTests.DeserializeTests+_EmptyEnumWrapper4: JilTests.DeserializeTests+_EmptyEnum has no values, and cannot be deserialized; add a value, make nullable, or configure to treat as integer", ex2.Message);
            }

            {
                var res = JSON.Deserialize<_EmptyEnumWrapper1>("{}");
                Assert.NotNull(res);
            }

            {
                var res = JSON.Deserialize<_EmptyEnumWrapper2>("{\"A\":null}");
                Assert.True(res.A == null);
            }

            {
                var res = JSON.Deserialize<_EmptyEnum?>("null");
                Assert.True(res == null);
            }

            {
                var res = JSON.Deserialize<_EmptyEnumWrapper3>("{\"A\":0}");
                Assert.NotNull(res);
                Assert.True(res.A == default(_EmptyEnum));
            }

            {
                var res = JSON.Deserialize<_EmptyEnumWrapper3>("{\"A\":1}");
                Assert.NotNull(res);
                Assert.True(res.A == (_EmptyEnum)1);
            }
        }

        [Fact]
        public void Issue235()
        {
            // strings
            {
                var res1 = JSON.Deserialize<decimal>("-100000000000001");
                Assert.Equal(-100000000000001m, res1);

                var res2 = JSON.Deserialize<decimal>("-100000000000002");
                Assert.Equal(-100000000000002m, res2);
            }

            // streams
            {
                using (var stream = new StringReader("-100000000000001"))
                {
                    var res = JSON.Deserialize<decimal>(stream);
                    Assert.Equal(-100000000000001m, res);
                }

                using (var stream = new StringReader("-100000000000002"))
                {
                    var res = JSON.Deserialize<decimal>(stream);
                    Assert.Equal(-100000000000002m, res);
                }
            }
        }

        public class _Issue227
        {
            public _Issue227_1 EnumProperty { get; set; }
        }

        public enum _Issue227_1
        {
            Addin,
            AddInSettingsDetail,
            AddInSettingsEncryptionHelper,
            UpdatePayments
        }

        [Fact]
        public void Issue227()
        {
            var ser = JSON.Serialize(_Issue227_1.AddInSettingsDetail);

            var val = JSON.Deserialize<_Issue227_1>(ser);
            Assert.Equal(_Issue227_1.AddInSettingsDetail, val);
        }

        [DataContract]
        public enum _Issue225_1
        {
            [EnumMember]
            Value0 = 0,
            [EnumMember]
            Value1 = 1
        }

        [DataContract]
        public class _Issue225
        {
            [DataMember(Name = "PT")]
            public _Issue225_1 PackagedType;
        }

        [Fact]
        public void Issue225()
        {
            var json = JSON.Serialize(new _Issue225
            {
                PackagedType = _Issue225_1.Value0
            });

            var bean = JSON.Deserialize<_Issue225>(json);
            Assert.NotNull(bean);
            Assert.Equal(_Issue225_1.Value0, bean.PackagedType);
        }

        class _Issue176_1
        {
            public List<int> Foo { get; set; }
        }

        class _Issue176_1_Derived : _Issue176_1
        {
            public new int Foo { get; set; }
        }

        class _Issue176_2
        {
            public List<int> Foo { get; set; }
        }

        class _Issue176_2_Derived : _Issue176_2
        {
            public new int[] Foo { get; set; }
        }

        class _Issue176_3
        {
            public int Foo { get; set; }
        }

        class _Issue176_3_Derived : _Issue176_3
        {
            public new string Foo { get; set; }
        }

        [Fact]
        public void Issue176()
        {
            {
                JSON.Deserialize<_Issue176_1_Derived>("{}");
                var res = JSON.Deserialize<_Issue176_1_Derived>("{\"Foo\":123}");
                Assert.NotNull(res);
                Assert.Equal(123, res.Foo);
            }

            {
                JSON.Deserialize<_Issue176_2_Derived>("{}");
                var res = JSON.Deserialize<_Issue176_2_Derived>("{\"Foo\":[1,2,3]}");
                Assert.NotNull(res);
                Assert.NotNull(res.Foo);
                Assert.Equal(3, res.Foo.Length);
                Assert.Equal(1, res.Foo[0]);
                Assert.Equal(2, res.Foo[1]);
                Assert.Equal(3, res.Foo[2]);
            }

            {
                JSON.Deserialize<_Issue176_3_Derived>("{}");
                var res = JSON.Deserialize< _Issue176_3_Derived>("{\"Foo\":\"Bar\"}");
                Assert.NotNull(res);
                Assert.Equal("Bar", res.Foo);
            }
        }

        [Fact]
        public void Issue229()
        {
            var expected = new DateTime(2016, 05, 06, 15, 57, 34, DateTimeKind.Utc);

            var result = JSON.Deserialize<_Issue229>("{\"createdate\":\"2016-05-06T15:57:34.000+0000\"}", new Options(dateFormat: Jil.DateTimeFormat.ISO8601));

            Assert.Equal(expected, result.createdate);
        }

        class _Issue229
        {
            public DateTime createdate { get; set; }
        }

        [JilPrimitiveWrapper]
        struct _Issue270
        {
            public _Issue270(int val)
            {
                Val = val;
            }
#pragma warning disable 649
            public int Val;
#pragma warning restore 649
        }

        [Fact]
        public void Issue270()
        {
            {
                var res = JSON.Deserialize<_Issue270?>("123");
                Assert.True(res != null);
                Assert.Equal(123, res.Value.Val);
            }

            {
                var res = JSON.Deserialize<_Issue270?>("null");
                Assert.True(res == null);
            }
        }

        [Fact]
        public void Issue248()
        {
            // reader
            {
                using (var str = new StringReader("\"0001\""))
                {
                    DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601Utc);
                    JSON.SerializeDynamic(dto, Options.ISO8601Utc);

                    // the expected output is actually UNDEFINED, because the spec says to treat
                    //  just a years as LOCAL time; so the expected result depends on the timezone
                    //  of the machine
                    //
                    // the important thing is that it doesn't explode in any timezone  (yes, this means
                    //   you have to change the timezone on your machine for this test to really matter)
                }
            }

            // string
            {
                DateTimeOffset dto = JSON.DeserializeDynamic("\"0001\"", Options.ISO8601Utc);
                JSON.SerializeDynamic(dto, Options.ISO8601Utc);

                // see above
            }
        }

        [DataContract]
        public class _Issue253
        {
            [DataMember]
            public _Issue253_Inner Item
            {
                get;
                set;
            }
        }

        [DataContract]
        public class _Issue253_Inner : ICollection<decimal>
        {
            [IgnoreDataMember]
            private List<decimal> items = new List<decimal>();

            int ICollection<decimal>.Count
            {
                get
                {
                    return ((ICollection<decimal>)this.items).Count;
                }
            }

            bool ICollection<decimal>.IsReadOnly
            {
                get
                {
                    return ((ICollection<decimal>)this.items).IsReadOnly;
                }
            }

            void ICollection<decimal>.Add(decimal item)
            {
                ((ICollection<decimal>)this.items).Add(item);
            }

            void ICollection<decimal>.Clear()
            {
                ((ICollection<decimal>)this.items).Clear();
            }

            bool ICollection<decimal>.Contains(decimal item)
            {
                return ((ICollection<decimal>)this.items).Contains(item);
            }

            void ICollection<decimal>.CopyTo(decimal[] array, int arrayIndex)
            {
                ((ICollection<decimal>)this.items).CopyTo(array, arrayIndex);
            }

            bool ICollection<decimal>.Remove(decimal item)
            {
                return ((ICollection<decimal>)this.items).Remove(item);
            }

            IEnumerator<decimal> IEnumerable<decimal>.GetEnumerator()
            {
                return ((ICollection<decimal>)this.items).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((System.Collections.IEnumerable)this.items).GetEnumerator();
            }
        }

        [Fact]
        public void Issue253()
        {
            var res = Jil.JSON.Deserialize<_Issue253>("{\"Item\": null}");
            Assert.NotNull(res);
            Assert.Null(res.Item);
        }

#if !DEBUG
#region SlowSpinUp Types

        public class ClusterNodeInfo
        {
            [DataMember(Name = "cluster_name")]
            public string ClusterName { get; internal set; }

            [DataMember(Name = "nodes")]
            public Dictionary<string, NodeInfo> Nodes { get; set; }
        }

        public class NodeInfo
        {
            [DataMember(Name = "name")]
            public string Name { get; internal set; }

            [DataMember(Name = "transport_address")]
            public string TransportAddress { get; internal set; }

            [DataMember(Name = "host")]
            public string Hostname { get; internal set; }

            [DataMember(Name = "version")]
            public string Version { get; internal set; }

            [DataMember(Name = "http_address")]
            public string HttpAddress { get; internal set; }

            [DataMember(Name = "settings")]
            public Dictionary<string, dynamic> Settings { get; internal set; }

            [DataMember(Name = "os")]
            public OSInfo OS { get; internal set; }

            [DataMember(Name = "process")]
            public ProcessInfo Process { get; internal set; }

            [DataMember(Name = "jvm")]
            public JVMInfo JVM { get; internal set; }

            [DataMember(Name = "thread_pool")]
            public Dictionary<string, ThreadPoolThreadInfo> ThreadPool { get; internal set; }

            [DataMember(Name = "network")]
            public NetworkInfo Network { get; internal set; }

            [DataMember(Name = "transport")]
            public TransportInfo Transport { get; internal set; }

            [DataMember(Name = "http")]
            public HTTPInfo HTTP { get; internal set; }
        }

        public class OSInfo
        {
            [DataMember(Name = "refresh_interval")]
            public int RefreshInterval { get; internal set; }

            [DataMember(Name = "available_processors")]
            public int AvailableProcessors { get; internal set; }

            [DataMember(Name = "cpu")]
            public OSCPUInfo Cpu { get; internal set; }
            [DataMember(Name = "mem")]
            public MemoryInfo Mem { get; internal set; }
            [DataMember(Name = "swap")]
            public MemoryInfo Swap { get; internal set; }

            public class OSCPUInfo
            {
                [DataMember(Name = "vendor")]
                public string Vendor { get; internal set; }
                [DataMember(Name = "model")]
                public string Model { get; internal set; }
                [DataMember(Name = "mhz")]
                public int Mhz { get; internal set; }
                [DataMember(Name = "total_cores")]
                public int TotalCores { get; internal set; }
                [DataMember(Name = "total_sockets")]
                public int TotalSockets { get; internal set; }
                [DataMember(Name = "cores_per_socket")]
                public int CoresPerSocket { get; internal set; }
                [DataMember(Name = "cache_size")]
                public string CacheSize { get; internal set; }
                [DataMember(Name = "cache_size_in_bytes")]
                public int CacheSizeInBytes { get; internal set; }
            }

            public class MemoryInfo
            {
                [DataMember(Name = "total")]
                public string Total { get; internal set; }
                [DataMember(Name = "total_in_bytes")]
                public long TotalInBytes { get; internal set; }
            }
        }

        public class ProcessInfo
        {
            [DataMember(Name = "refresh_interval")]
            public int RefreshInterval { get; internal set; }
            [DataMember(Name = "id")]
            public long Id { get; internal set; }
            [DataMember(Name = "max_file_descriptors")]
            public int MaxFileDescriptors { get; internal set; }
        }

        public class JVMInfo
        {
            [DataMember(Name = "pid")]
            public int PID { get; internal set; }
            [DataMember(Name = "version")]
            public string Version { get; internal set; }
            [DataMember(Name = "vm_name")]
            public string VMName { get; internal set; }
            [DataMember(Name = "vm_version")]
            public string VMVersion { get; internal set; }
            [DataMember(Name = "vm_vendor")]
            public string VMVendor { get; internal set; }
            [DataMember(Name = "start_time")]
            public long StartTime { get; internal set; }
            [DataMember(Name = "mem")]
            public JVMMemoryInfo Memory { get; internal set; }

            public class JVMMemoryInfo
            {
                [DataMember(Name = "heap_init")]
                public string HeapInit { get; internal set; }
                [DataMember(Name = "heap_init_in_bytes")]
                public long HeapInitInBytes { get; internal set; }
                [DataMember(Name = "heap_max")]
                public string HeapMax { get; internal set; }
                [DataMember(Name = "heap_max_in_bytes")]
                public long HeapMaxInBytes { get; internal set; }
                [DataMember(Name = "non_heap_init")]
                public string NonHeapInit { get; internal set; }
                [DataMember(Name = "non_heap_init_in_bytes")]
                public long NonHeapInitInBytes { get; internal set; }
                [DataMember(Name = "non_heap_max")]
                public string NonHeapMax { get; internal set; }
                [DataMember(Name = "non_heap_max_in_bytes")]
                public long NonHeapMaxInBytes { get; internal set; }
                [DataMember(Name = "direct_max")]
                public string DirectMax { get; internal set; }
                [DataMember(Name = "direct_max_in_bytes")]
                public long DirectMaxInBytes { get; internal set; }
            }
        }

        public class ThreadPoolThreadInfo
        {
            [DataMember(Name = "type")]
            public string Type { get; internal set; }
            [DataMember(Name = "min")]
            public int? Min { get; internal set; }
            [DataMember(Name = "max")]
            public int? Max { get; internal set; }
            [DataMember(Name = "keep_alive")]
            public string KeepAlive { get; internal set; }
        }

        public class NetworkInfo
        {
            [DataMember(Name = "refresh_interval")]
            public int RefreshInterval { get; internal set; }
            [DataMember(Name = "primary_interface")]
            public NetworkInterfaceInfo PrimaryInterface { get; internal set; }

            public class NetworkInterfaceInfo
            {
                [DataMember(Name = "address")]
                public string Address { get; internal set; }
                [DataMember(Name = "name")]
                public string Name { get; internal set; }
                [DataMember(Name = "mac_address")]
                public string MacAddress { get; internal set; }
            }
        }

        public class TransportInfo
        {
            [DataMember(Name = "bound_address")]
            public string BoundAddress { get; internal set; }
            [DataMember(Name = "publish_address")]
            public string PublishAddress { get; internal set; }
        }

        public class HTTPInfo
        {
            [DataMember(Name = "bound_address")]
            public string BoundAddress { get; internal set; }
            [DataMember(Name = "publish_address")]
            public string PublishAddress { get; internal set; }
            [DataMember(Name = "max_content_length")]
            public string MaxContentLength { get; internal set; }
            [DataMember(Name = "max_content_length_in_bytes")]
            public long MaxContentLengthInBytes { get; internal set; }
        }

#endregion

        [Fact]
        public void SlowSpinUp()
        {
            var json = @"{""cluster_name"":""ml-elastic-cluster"",""nodes"":{""CHtYMjlNRJWGzVGGhxxN-w"":{""name"":""Jimmy Woo"",""transport_address"":""inet[/10.7.3.182:9300]"",""host"":""ny-mlelastic03.ds.stackexchange.com"",""ip"":""10.7.3.182"",""version"":""1.2.1"",""build"":""6c95b75"",""http_address"":""inet[/10.7.3.182:9200]"",""settings"":{""path"":{""data"":""/cassandra/elasticsearch/data"",""work"":""/tmp/elasticsearch"",""home"":""/usr/share/elasticsearch"",""conf"":""/etc/elasticsearch"",""logs"":""/cassandra/elasticsearch/log""},""pidfile"":""/var/run/elasticsearch/elasticsearch.pid"",""cluster"":{""name"":""ml-elastic-cluster""},""script"":{""disable_dynamic"":""false""},""discovery"":{""zen"":{""ping"":{""unicast"":{""hosts"":[""ny-mlelastic01"",""ny-mlelastic02"",""ny-mlelastic03""]},""multicast"":{""enabled"":""false""}}}},""name"":""Jimmy Woo""},""os"":{""refresh_interval_in_millis"":1000,""available_processors"":32,""cpu"":{""vendor"":""Intel"",""model"":""Xeon"",""mhz"":2600,""total_cores"":32,""total_sockets"":1,""cores_per_socket"":32,""cache_size_in_bytes"":20480},""mem"":{""total_in_bytes"":101392883712},""swap"":{""total_in_bytes"":137438945280}},""process"":{""refresh_interval_in_millis"":1000,""id"":36752,""max_file_descriptors"":65535,""mlockall"":false},""jvm"":{""pid"":36752,""version"":""1.7.0_51"",""vm_name"":""Java HotSpot(TM) 64-Bit Server VM"",""vm_version"":""24.51-b03"",""vm_vendor"":""Oracle Corporation"",""start_time_in_millis"":1403026987431,""mem"":{""heap_init_in_bytes"":268435456,""heap_max_in_bytes"":1037959168,""non_heap_init_in_bytes"":24313856,""non_heap_max_in_bytes"":136314880,""direct_max_in_bytes"":1037959168},""gc_collectors"":[""ParNew"",""ConcurrentMarkSweep""],""memory_pools"":[""Code Cache"",""Par Eden Space"",""Par Survivor Space"",""CMS Old Gen"",""CMS Perm Gen""]},""thread_pool"":{""generic"":{""type"":""cached"",""keep_alive"":""30s""},""index"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""200""},""snapshot_data"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""bench"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""get"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""snapshot"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""merge"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""suggest"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""bulk"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""50""},""optimize"":{""type"":""fixed"",""min"":1,""max"":1},""warmer"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""flush"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""search"":{""type"":""fixed"",""min"":96,""max"":96,""queue_size"":""1k""},""percolate"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""management"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""refresh"":{""type"":""scaling"",""min"":1,""max"":10,""keep_alive"":""5m""}},""network"":{""refresh_interval_in_millis"":5000,""primary_interface"":{""address"":""10.7.3.182"",""name"":""em4"",""mac_address"":""B8:CA:3A:70:A8:3D""}},""transport"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9300]"",""publish_address"":""inet[/10.7.3.182:9300]""},""http"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9200]"",""publish_address"":""inet[/10.7.3.182:9200]"",""max_content_length_in_bytes"":104857600},""plugins"":[]},""y2D2HmdTTIGSD8zc74Oz7g"":{""name"":""Tony Stark"",""transport_address"":""inet[/10.7.3.181:9300]"",""host"":""ny-mlelastic02.ds.stackexchange.com"",""ip"":""10.7.3.181"",""version"":""1.2.1"",""build"":""6c95b75"",""http_address"":""inet[/10.7.3.181:9200]"",""settings"":{""path"":{""data"":""/cassandra/elasticsearch/data"",""work"":""/tmp/elasticsearch"",""home"":""/usr/share/elasticsearch"",""conf"":""/etc/elasticsearch"",""logs"":""/cassandra/elasticsearch/log""},""pidfile"":""/var/run/elasticsearch/elasticsearch.pid"",""cluster"":{""name"":""ml-elastic-cluster""},""script"":{""disable_dynamic"":""false""},""discovery"":{""zen"":{""ping"":{""unicast"":{""hosts"":[""ny-mlelastic01"",""ny-mlelastic02"",""ny-mlelastic03""]},""multicast"":{""enabled"":""false""}}}},""name"":""Tony Stark""},""os"":{""refresh_interval_in_millis"":1000,""available_processors"":32,""cpu"":{""vendor"":""Intel"",""model"":""Xeon"",""mhz"":2600,""total_cores"":32,""total_sockets"":1,""cores_per_socket"":32,""cache_size_in_bytes"":20480},""mem"":{""total_in_bytes"":101392883712},""swap"":{""total_in_bytes"":137438945280}},""process"":{""refresh_interval_in_millis"":1000,""id"":13069,""max_file_descriptors"":65535,""mlockall"":false},""jvm"":{""pid"":13069,""version"":""1.7.0_51"",""vm_name"":""Java HotSpot(TM) 64-Bit Server VM"",""vm_version"":""24.51-b03"",""vm_vendor"":""Oracle Corporation"",""start_time_in_millis"":1403026959588,""mem"":{""heap_init_in_bytes"":268435456,""heap_max_in_bytes"":1037959168,""non_heap_init_in_bytes"":24313856,""non_heap_max_in_bytes"":136314880,""direct_max_in_bytes"":1037959168},""gc_collectors"":[""ParNew"",""ConcurrentMarkSweep""],""memory_pools"":[""Code Cache"",""Par Eden Space"",""Par Survivor Space"",""CMS Old Gen"",""CMS Perm Gen""]},""thread_pool"":{""generic"":{""type"":""cached"",""keep_alive"":""30s""},""index"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""200""},""snapshot_data"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""bench"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""get"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""snapshot"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""merge"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""suggest"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""bulk"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""50""},""optimize"":{""type"":""fixed"",""min"":1,""max"":1},""warmer"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""flush"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""search"":{""type"":""fixed"",""min"":96,""max"":96,""queue_size"":""1k""},""percolate"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""management"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""refresh"":{""type"":""scaling"",""min"":1,""max"":10,""keep_alive"":""5m""}},""network"":{""refresh_interval_in_millis"":5000,""primary_interface"":{""address"":""10.7.3.181"",""name"":""em4"",""mac_address"":""B8:CA:3A:70:AB:6D""}},""transport"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9300]"",""publish_address"":""inet[/10.7.3.181:9300]""},""http"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9200]"",""publish_address"":""inet[/10.7.3.181:9200]"",""max_content_length_in_bytes"":104857600},""plugins"":[]},""b3UvPCMmS-67jl6OWxFShw"":{""name"":""Invisible Girl"",""transport_address"":""inet[/10.7.3.180:9300]"",""host"":""ny-mlelastic01"",""ip"":""10.7.3.180"",""version"":""1.2.1"",""build"":""6c95b75"",""http_address"":""inet[/10.7.3.180:9200]"",""settings"":{""path"":{""data"":""/cassandra/elasticsearch/data"",""work"":""/tmp/elasticsearch"",""home"":""/usr/share/elasticsearch"",""conf"":""/etc/elasticsearch"",""logs"":""/cassandra/elasticsearch/log""},""pidfile"":""/var/run/elasticsearch/elasticsearch.pid"",""cluster"":{""name"":""ml-elastic-cluster""},""script"":{""disable_dynamic"":""false""},""discovery"":{""zen"":{""ping"":{""unicast"":{""hosts"":[""ny-mlelastic01"",""ny-mlelastic02"",""ny-mlelastic03""]},""multicast"":{""enabled"":""false""}}}},""name"":""Invisible Girl""},""os"":{""refresh_interval_in_millis"":1000,""available_processors"":32,""cpu"":{""vendor"":""Intel"",""model"":""Xeon"",""mhz"":2599,""total_cores"":32,""total_sockets"":1,""cores_per_socket"":32,""cache_size_in_bytes"":20480},""mem"":{""total_in_bytes"":101392883712},""swap"":{""total_in_bytes"":137438945280}},""process"":{""refresh_interval_in_millis"":1000,""id"":4764,""max_file_descriptors"":65535,""mlockall"":false},""jvm"":{""pid"":4764,""version"":""1.7.0_51"",""vm_name"":""Java HotSpot(TM) 64-Bit Server VM"",""vm_version"":""24.51-b03"",""vm_vendor"":""Oracle Corporation"",""start_time_in_millis"":1403026924332,""mem"":{""heap_init_in_bytes"":268435456,""heap_max_in_bytes"":1037959168,""non_heap_init_in_bytes"":24313856,""non_heap_max_in_bytes"":136314880,""direct_max_in_bytes"":1037959168},""gc_collectors"":[""ParNew"",""ConcurrentMarkSweep""],""memory_pools"":[""Code Cache"",""Par Eden Space"",""Par Survivor Space"",""CMS Old Gen"",""CMS Perm Gen""]},""thread_pool"":{""generic"":{""type"":""cached"",""keep_alive"":""30s""},""index"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""200""},""snapshot_data"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""bench"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""get"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""snapshot"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""merge"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""suggest"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""bulk"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""50""},""optimize"":{""type"":""fixed"",""min"":1,""max"":1},""warmer"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""flush"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""search"":{""type"":""fixed"",""min"":96,""max"":96,""queue_size"":""1k""},""percolate"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""management"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""refresh"":{""type"":""scaling"",""min"":1,""max"":10,""keep_alive"":""5m""}},""network"":{""refresh_interval_in_millis"":5000,""primary_interface"":{""address"":""10.7.3.180"",""name"":""em4"",""mac_address"":""B8:CA:3A:70:D8:A5""}},""transport"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0:9300]"",""publish_address"":""inet[/10.7.3.180:9300]""},""http"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0:9200]"",""publish_address"":""inet[/10.7.3.180:9200]"",""max_content_length_in_bytes"":104857600},""plugins"":[]}}}";

            var timer = new Stopwatch();
            timer.Start();
            var result = JSON.Deserialize<ClusterNodeInfo>(json, Options.SecondsSinceUnixEpochExcludeNulls);
            timer.Stop();

            Assert.True(timer.ElapsedMilliseconds < 1500, "Took: " + timer.ElapsedMilliseconds + "ms");
        }
#endif

#if !DEBUG

        class _DegeneratelyLargeNested
        {
            public class _Inner1 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner2 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner3 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner4 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner5 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner6 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner7 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner8 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner9 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner10 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner11 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner12 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner13 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner14 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner15 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner16 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner17 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner18 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner19 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner20 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner21 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner22 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner23 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner24 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner25 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner26 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner27 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner28 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner29 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner30 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; }public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner31 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner32 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner33 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner34 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner35 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner36 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner37 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner38 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner39 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner40 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner41 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner42 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner43 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner44 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner45 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner46 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner47 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner48 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner49 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner50 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner51 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner52 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner53 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner54 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner55 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner56 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner57 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner58 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner59 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }
            public class _Inner60 { public bool? A { get; set; } public bool? B { get; set; } public bool? C { get; set; } public bool? D { get; set; } public bool? E { get; set; } public bool? F { get; set; } public bool? G { get; set; } public bool? H { get; set; } public bool? I { get; set; } public bool? J { get; set; } public bool? K { get; set; } public bool? L { get; set; } public bool? M { get; set; } public bool? N { get; set; } public bool? O { get; set; } public bool? P { get; set; } public bool? Q { get; set; } public bool? R { get; set; } public bool? S { get; set; } public bool? T { get; set; } public bool? U { get; set; } public bool? V { get; set; } public bool? W { get; set; } public bool? X { get; set; } public bool? Y { get; set; } public bool? Z { get; set; } public bool? a { get; set; } public bool? b { get; set; } public bool? c { get; set; } public bool? d { get; set; } public bool? e { get; set; } public bool? f { get; set; } public bool? g { get; set; } public bool? h { get; set; } public bool? i { get; set; } public bool? j { get; set; } public bool? k { get; set; } public bool? l { get; set; } public bool? m { get; set; } public bool? n { get; set; } public bool? o { get; set; } public bool? p { get; set; } public bool? q { get; set; } public bool? r { get; set; } public bool? s { get; set; } public bool? t { get; set; } public bool? u { get; set; } public bool? v { get; set; } public bool? w { get; set; } public bool? x { get; set; } public bool? y { get; set; } public bool? z { get; set; } }

            public bool Success { get; set; }

            public _Inner1 I1 { get; set; }
            public _Inner2 I2 { get; set; }
            public _Inner3 I3 { get; set; }
            public _Inner4 I4 { get; set; }
            public _Inner5 I5 { get; set; }
            public _Inner6 I6 { get; set; }
            public _Inner7 I7 { get; set; }
            public _Inner8 I8 { get; set; }
            public _Inner9 I9 { get; set; }
            public _Inner10 I10 { get; set; }
            public _Inner11 I11 { get; set; }
            public _Inner12 I12 { get; set; }
            public _Inner13 I13 { get; set; }
            public _Inner14 I14 { get; set; }
            public _Inner15 I15 { get; set; }
            public _Inner16 I16 { get; set; }
            public _Inner17 I17 { get; set; }
            public _Inner18 I18 { get; set; }
            public _Inner19 I19 { get; set; }
            public _Inner20 I20 { get; set; }
            public _Inner21 I21 { get; set; }
            public _Inner22 I22 { get; set; }
            public _Inner23 I23 { get; set; }
            public _Inner24 I24 { get; set; }
            public _Inner25 I25 { get; set; }
            public _Inner26 I26 { get; set; }
            public _Inner27 I27 { get; set; }
            public _Inner28 I28 { get; set; }
            public _Inner29 I29 { get; set; }
            public _Inner30 I30 { get; set; }
            public _Inner31 I31 { get; set; }
            public _Inner32 I32 { get; set; }
            public _Inner33 I33 { get; set; }
            public _Inner34 I34 { get; set; }
            public _Inner35 I35 { get; set; }
            public _Inner36 I36 { get; set; }
            public _Inner37 I37 { get; set; }
            public _Inner38 I38 { get; set; }
            public _Inner39 I39 { get; set; }
            public _Inner40 I40 { get; set; }
            public _Inner41 I41 { get; set; }
            public _Inner42 I42 { get; set; }
            public _Inner43 I43 { get; set; }
            public _Inner44 I44 { get; set; }
            public _Inner45 I45 { get; set; }
            public _Inner46 I46 { get; set; }
            public _Inner47 I47 { get; set; }
            public _Inner48 I48 { get; set; }
            public _Inner49 I49 { get; set; }
            public _Inner50 I50 { get; set; }
            public _Inner51 I51 { get; set; }
            public _Inner52 I52 { get; set; }
            public _Inner53 I53 { get; set; }
            public _Inner54 I54 { get; set; }
            public _Inner55 I55 { get; set; }
            public _Inner56 I56 { get; set; }
            public _Inner57 I57 { get; set; }
            public _Inner58 I58 { get; set; }
            public _Inner59 I59 { get; set; }
            public _Inner60 I60 { get; set; }
        }

        [Fact]
        public void DegeneratelyLargeNested()
        {
            // string
            {
                var res = JSON.Deserialize<_DegeneratelyLargeNested>("{ \"Success\": true }");
                Assert.NotNull(res);
                Assert.True(res.Success);
            }

            // stream
            {
                using (var str = new StringReader("{ \"Success\": true }"))
                {
                    var res = JSON.Deserialize<_DegeneratelyLargeNested>(str);
                    Assert.NotNull(res);
                    Assert.True(res.Success);
                }
            }
        }

        class _DegeneratelyLargeFlat
        {
            public bool Success { get; set; }
            public bool? A1 { get; set; } public bool? B1 { get; set; } public bool? C1 { get; set; } public bool? D1 { get; set; } public bool? E1 { get; set; } public bool? F1 { get; set; } public bool? G1 { get; set; } public bool? H1 { get; set; } public bool? I1 { get; set; } public bool? J1 { get; set; } public bool? K1 { get; set; } public bool? L1 { get; set; } public bool? M1 { get; set; } public bool? N1 { get; set; } public bool? O1 { get; set; } public bool? P1 { get; set; } public bool? Q1 { get; set; } public bool? R1 { get; set; } public bool? S1 { get; set; } public bool? T1 { get; set; } public bool? U1 { get; set; } public bool? V1 { get; set; } public bool? W1 { get; set; } public bool? X1 { get; set; } public bool? Y1 { get; set; } public bool? Z1 { get; set; } public bool? a1 { get; set; } public bool? b1 { get; set; } public bool? c1 { get; set; } public bool? d1 { get; set; } public bool? e1 { get; set; } public bool? f1 { get; set; } public bool? g1 { get; set; } public bool? h1 { get; set; } public bool? i1 { get; set; } public bool? j1 { get; set; } public bool? k1 { get; set; } public bool? l1 { get; set; } public bool? m1 { get; set; } public bool? n1 { get; set; } public bool? o1 { get; set; } public bool? p1 { get; set; } public bool? q1 { get; set; } public bool? r1 { get; set; } public bool? s1 { get; set; } public bool? t1 { get; set; } public bool? u1 { get; set; } public bool? v1 { get; set; } public bool? w1 { get; set; } public bool? x1 { get; set; } public bool? y1 { get; set; } public bool? z1 { get; set; } 
            public bool? A2 { get; set; } public bool? B2 { get; set; } public bool? C2 { get; set; } public bool? D2 { get; set; } public bool? E2 { get; set; } public bool? F2 { get; set; } public bool? G2 { get; set; } public bool? H2 { get; set; } public bool? I2 { get; set; } public bool? J2 { get; set; } public bool? K2 { get; set; } public bool? L2 { get; set; } public bool? M2 { get; set; } public bool? N2 { get; set; } public bool? O2 { get; set; } public bool? P2 { get; set; } public bool? Q2 { get; set; } public bool? R2 { get; set; } public bool? S2 { get; set; } public bool? T2 { get; set; } public bool? U2 { get; set; } public bool? V2 { get; set; } public bool? W2 { get; set; } public bool? X2 { get; set; } public bool? Y2 { get; set; } public bool? Z2 { get; set; } public bool? a2 { get; set; } public bool? b2 { get; set; } public bool? c2 { get; set; } public bool? d2 { get; set; } public bool? e2 { get; set; } public bool? f2 { get; set; } public bool? g2 { get; set; } public bool? h2 { get; set; } public bool? i2 { get; set; } public bool? j2 { get; set; } public bool? k2 { get; set; } public bool? l2 { get; set; } public bool? m2 { get; set; } public bool? n2 { get; set; } public bool? o2 { get; set; } public bool? p2 { get; set; } public bool? q2 { get; set; } public bool? r2 { get; set; } public bool? s2 { get; set; } public bool? t2 { get; set; } public bool? u2 { get; set; } public bool? v2 { get; set; } public bool? w2 { get; set; } public bool? x2 { get; set; } public bool? y2 { get; set; } public bool? z2 { get; set; } 
            public bool? A3 { get; set; } public bool? B3 { get; set; } public bool? C3 { get; set; } public bool? D3 { get; set; } public bool? E3 { get; set; } public bool? F3 { get; set; } public bool? G3 { get; set; } public bool? H3 { get; set; } public bool? I3 { get; set; } public bool? J3 { get; set; } public bool? K3 { get; set; } public bool? L3 { get; set; } public bool? M3 { get; set; } public bool? N3 { get; set; } public bool? O3 { get; set; } public bool? P3 { get; set; } public bool? Q3 { get; set; } public bool? R3 { get; set; } public bool? S3 { get; set; } public bool? T3 { get; set; } public bool? U3 { get; set; } public bool? V3 { get; set; } public bool? W3 { get; set; } public bool? X3 { get; set; } public bool? Y3 { get; set; } public bool? Z3 { get; set; } public bool? a3 { get; set; } public bool? b3 { get; set; } public bool? c3 { get; set; } public bool? d3 { get; set; } public bool? e3 { get; set; } public bool? f3 { get; set; } public bool? g3 { get; set; } public bool? h3 { get; set; } public bool? i3 { get; set; } public bool? j3 { get; set; } public bool? k3 { get; set; } public bool? l3 { get; set; } public bool? m3 { get; set; } public bool? n3 { get; set; } public bool? o3 { get; set; } public bool? p3 { get; set; } public bool? q3 { get; set; } public bool? r3 { get; set; } public bool? s3 { get; set; } public bool? t3 { get; set; } public bool? u3 { get; set; } public bool? v3 { get; set; } public bool? w3 { get; set; } public bool? x3 { get; set; } public bool? y3 { get; set; } public bool? z3 { get; set; } 
            public bool? A4 { get; set; } public bool? B4 { get; set; } public bool? C4 { get; set; } public bool? D4 { get; set; } public bool? E4 { get; set; } public bool? F4 { get; set; } public bool? G4 { get; set; } public bool? H4 { get; set; } public bool? I4 { get; set; } public bool? J4 { get; set; } public bool? K4 { get; set; } public bool? L4 { get; set; } public bool? M4 { get; set; } public bool? N4 { get; set; } public bool? O4 { get; set; } public bool? P4 { get; set; } public bool? Q4 { get; set; } public bool? R4 { get; set; } public bool? S4 { get; set; } public bool? T4 { get; set; } public bool? U4 { get; set; } public bool? V4 { get; set; } public bool? W4 { get; set; } public bool? X4 { get; set; } public bool? Y4 { get; set; } public bool? Z4 { get; set; } public bool? a4 { get; set; } public bool? b4 { get; set; } public bool? c4 { get; set; } public bool? d4 { get; set; } public bool? e4 { get; set; } public bool? f4 { get; set; } public bool? g4 { get; set; } public bool? h4 { get; set; } public bool? i4 { get; set; } public bool? j4 { get; set; } public bool? k4 { get; set; } public bool? l4 { get; set; } public bool? m4 { get; set; } public bool? n4 { get; set; } public bool? o4 { get; set; } public bool? p4 { get; set; } public bool? q4 { get; set; } public bool? r4 { get; set; } public bool? s4 { get; set; } public bool? t4 { get; set; } public bool? u4 { get; set; } public bool? v4 { get; set; } public bool? w4 { get; set; } public bool? x4 { get; set; } public bool? y4 { get; set; } public bool? z4 { get; set; } 
            public bool? A5 { get; set; } public bool? B5 { get; set; } public bool? C5 { get; set; } public bool? D5 { get; set; } public bool? E5 { get; set; } public bool? F5 { get; set; } public bool? G5 { get; set; } public bool? H5 { get; set; } public bool? I5 { get; set; } public bool? J5 { get; set; } public bool? K5 { get; set; } public bool? L5 { get; set; } public bool? M5 { get; set; } public bool? N5 { get; set; } public bool? O5 { get; set; } public bool? P5 { get; set; } public bool? Q5 { get; set; } public bool? R5 { get; set; } public bool? S5 { get; set; } public bool? T5 { get; set; } public bool? U5 { get; set; } public bool? V5 { get; set; } public bool? W5 { get; set; } public bool? X5 { get; set; } public bool? Y5 { get; set; } public bool? Z5 { get; set; } public bool? a5 { get; set; } public bool? b5 { get; set; } public bool? c5 { get; set; } public bool? d5 { get; set; } public bool? e5 { get; set; } public bool? f5 { get; set; } public bool? g5 { get; set; } public bool? h5 { get; set; } public bool? i5 { get; set; } public bool? j5 { get; set; } public bool? k5 { get; set; } public bool? l5 { get; set; } public bool? m5 { get; set; } public bool? n5 { get; set; } public bool? o5 { get; set; } public bool? p5 { get; set; } public bool? q5 { get; set; } public bool? r5 { get; set; } public bool? s5 { get; set; } public bool? t5 { get; set; } public bool? u5 { get; set; } public bool? v5 { get; set; } public bool? w5 { get; set; } public bool? x5 { get; set; } public bool? y5 { get; set; } public bool? z5 { get; set; } 
            public bool? A6 { get; set; } public bool? B6 { get; set; } public bool? C6 { get; set; } public bool? D6 { get; set; } public bool? E6 { get; set; } public bool? F6 { get; set; } public bool? G6 { get; set; } public bool? H6 { get; set; } public bool? I6 { get; set; } public bool? J6 { get; set; } public bool? K6 { get; set; } public bool? L6 { get; set; } public bool? M6 { get; set; } public bool? N6 { get; set; } public bool? O6 { get; set; } public bool? P6 { get; set; } public bool? Q6 { get; set; } public bool? R6 { get; set; } public bool? S6 { get; set; } public bool? T6 { get; set; } public bool? U6 { get; set; } public bool? V6 { get; set; } public bool? W6 { get; set; } public bool? X6 { get; set; } public bool? Y6 { get; set; } public bool? Z6 { get; set; } public bool? a6 { get; set; } public bool? b6 { get; set; } public bool? c6 { get; set; } public bool? d6 { get; set; } public bool? e6 { get; set; } public bool? f6 { get; set; } public bool? g6 { get; set; } public bool? h6 { get; set; } public bool? i6 { get; set; } public bool? j6 { get; set; } public bool? k6 { get; set; } public bool? l6 { get; set; } public bool? m6 { get; set; } public bool? n6 { get; set; } public bool? o6 { get; set; } public bool? p6 { get; set; } public bool? q6 { get; set; } public bool? r6 { get; set; } public bool? s6 { get; set; } public bool? t6 { get; set; } public bool? u6 { get; set; } public bool? v6 { get; set; } public bool? w6 { get; set; } public bool? x6 { get; set; } public bool? y6 { get; set; } public bool? z6 { get; set; } 
            public bool? A7 { get; set; } public bool? B7 { get; set; } public bool? C7 { get; set; } public bool? D7 { get; set; } public bool? E7 { get; set; } public bool? F7 { get; set; } public bool? G7 { get; set; } public bool? H7 { get; set; } public bool? I7 { get; set; } public bool? J7 { get; set; } public bool? K7 { get; set; } public bool? L7 { get; set; } public bool? M7 { get; set; } public bool? N7 { get; set; } public bool? O7 { get; set; } public bool? P7 { get; set; } public bool? Q7 { get; set; } public bool? R7 { get; set; } public bool? S7 { get; set; } public bool? T7 { get; set; } public bool? U7 { get; set; } public bool? V7 { get; set; } public bool? W7 { get; set; } public bool? X7 { get; set; } public bool? Y7 { get; set; } public bool? Z7 { get; set; } public bool? a7 { get; set; } public bool? b7 { get; set; } public bool? c7 { get; set; } public bool? d7 { get; set; } public bool? e7 { get; set; } public bool? f7 { get; set; } public bool? g7 { get; set; } public bool? h7 { get; set; } public bool? i7 { get; set; } public bool? j7 { get; set; } public bool? k7 { get; set; } public bool? l7 { get; set; } public bool? m7 { get; set; } public bool? n7 { get; set; } public bool? o7 { get; set; } public bool? p7 { get; set; } public bool? q7 { get; set; } public bool? r7 { get; set; } public bool? s7 { get; set; } public bool? t7 { get; set; } public bool? u7 { get; set; } public bool? v7 { get; set; } public bool? w7 { get; set; } public bool? x7 { get; set; } public bool? y7 { get; set; } public bool? z7 { get; set; } 
            public bool? A8 { get; set; } public bool? B8 { get; set; } public bool? C8 { get; set; } public bool? D8 { get; set; } public bool? E8 { get; set; } public bool? F8 { get; set; } public bool? G8 { get; set; } public bool? H8 { get; set; } public bool? I8 { get; set; } public bool? J8 { get; set; } public bool? K8 { get; set; } public bool? L8 { get; set; } public bool? M8 { get; set; } public bool? N8 { get; set; } public bool? O8 { get; set; } public bool? P8 { get; set; } public bool? Q8 { get; set; } public bool? R8 { get; set; } public bool? S8 { get; set; } public bool? T8 { get; set; } public bool? U8 { get; set; } public bool? V8 { get; set; } public bool? W8 { get; set; } public bool? X8 { get; set; } public bool? Y8 { get; set; } public bool? Z8 { get; set; } public bool? a8 { get; set; } public bool? b8 { get; set; } public bool? c8 { get; set; } public bool? d8 { get; set; } public bool? e8 { get; set; } public bool? f8 { get; set; } public bool? g8 { get; set; } public bool? h8 { get; set; } public bool? i8 { get; set; } public bool? j8 { get; set; } public bool? k8 { get; set; } public bool? l8 { get; set; } public bool? m8 { get; set; } public bool? n8 { get; set; } public bool? o8 { get; set; } public bool? p8 { get; set; } public bool? q8 { get; set; } public bool? r8 { get; set; } public bool? s8 { get; set; } public bool? t8 { get; set; } public bool? u8 { get; set; } public bool? v8 { get; set; } public bool? w8 { get; set; } public bool? x8 { get; set; } public bool? y8 { get; set; } public bool? z8 { get; set; } 
            public bool? A9 { get; set; } public bool? B9 { get; set; } public bool? C9 { get; set; } public bool? D9 { get; set; } public bool? E9 { get; set; } public bool? F9 { get; set; } public bool? G9 { get; set; } public bool? H9 { get; set; } public bool? I9 { get; set; } public bool? J9 { get; set; } public bool? K9 { get; set; } public bool? L9 { get; set; } public bool? M9 { get; set; } public bool? N9 { get; set; } public bool? O9 { get; set; } public bool? P9 { get; set; } public bool? Q9 { get; set; } public bool? R9 { get; set; } public bool? S9 { get; set; } public bool? T9 { get; set; } public bool? U9 { get; set; } public bool? V9 { get; set; } public bool? W9 { get; set; } public bool? X9 { get; set; } public bool? Y9 { get; set; } public bool? Z9 { get; set; } public bool? a9 { get; set; } public bool? b9 { get; set; } public bool? c9 { get; set; } public bool? d9 { get; set; } public bool? e9 { get; set; } public bool? f9 { get; set; } public bool? g9 { get; set; } public bool? h9 { get; set; } public bool? i9 { get; set; } public bool? j9 { get; set; } public bool? k9 { get; set; } public bool? l9 { get; set; } public bool? m9 { get; set; } public bool? n9 { get; set; } public bool? o9 { get; set; } public bool? p9 { get; set; } public bool? q9 { get; set; } public bool? r9 { get; set; } public bool? s9 { get; set; } public bool? t9 { get; set; } public bool? u9 { get; set; } public bool? v9 { get; set; } public bool? w9 { get; set; } public bool? x9 { get; set; } public bool? y9 { get; set; } public bool? z9 { get; set; } 
            public bool? A10 { get; set; } public bool? B10 { get; set; } public bool? C10 { get; set; } public bool? D10 { get; set; } public bool? E10 { get; set; } public bool? F10 { get; set; } public bool? G10 { get; set; } public bool? H10 { get; set; } public bool? I10 { get; set; } public bool? J10 { get; set; } public bool? K10 { get; set; } public bool? L10 { get; set; } public bool? M10 { get; set; } public bool? N10 { get; set; } public bool? O10 { get; set; } public bool? P10 { get; set; } public bool? Q10 { get; set; } public bool? R10 { get; set; } public bool? S10 { get; set; } public bool? T10 { get; set; } public bool? U10 { get; set; } public bool? V10 { get; set; } public bool? W10 { get; set; } public bool? X10 { get; set; } public bool? Y10 { get; set; } public bool? Z10 { get; set; } public bool? a10 { get; set; } public bool? b10 { get; set; } public bool? c10 { get; set; } public bool? d10 { get; set; } public bool? e10 { get; set; } public bool? f10 { get; set; } public bool? g10 { get; set; } public bool? h10 { get; set; } public bool? i10 { get; set; } public bool? j10 { get; set; } public bool? k10 { get; set; } public bool? l10 { get; set; } public bool? m10 { get; set; } public bool? n10 { get; set; } public bool? o10 { get; set; } public bool? p10 { get; set; } public bool? q10 { get; set; } public bool? r10 { get; set; } public bool? s10 { get; set; } public bool? t10 { get; set; } public bool? u10 { get; set; } public bool? v10 { get; set; } public bool? w10 { get; set; } public bool? x10 { get; set; } public bool? y10 { get; set; } public bool? z10 { get; set; } 
            public bool? A11 { get; set; } public bool? B11 { get; set; } public bool? C11 { get; set; } public bool? D11 { get; set; } public bool? E11 { get; set; } public bool? F11 { get; set; } public bool? G11 { get; set; } public bool? H11 { get; set; } public bool? I11 { get; set; } public bool? J11 { get; set; } public bool? K11 { get; set; } public bool? L11 { get; set; } public bool? M11 { get; set; } public bool? N11 { get; set; } public bool? O11 { get; set; } public bool? P11 { get; set; } public bool? Q11 { get; set; } public bool? R11 { get; set; } public bool? S11 { get; set; } public bool? T11 { get; set; } public bool? U11 { get; set; } public bool? V11 { get; set; } public bool? W11 { get; set; } public bool? X11 { get; set; } public bool? Y11 { get; set; } public bool? Z11 { get; set; } public bool? a11 { get; set; } public bool? b11 { get; set; } public bool? c11 { get; set; } public bool? d11 { get; set; } public bool? e11 { get; set; } public bool? f11 { get; set; } public bool? g11 { get; set; } public bool? h11 { get; set; } public bool? i11 { get; set; } public bool? j11 { get; set; } public bool? k11 { get; set; } public bool? l11 { get; set; } public bool? m11 { get; set; } public bool? n11 { get; set; } public bool? o11 { get; set; } public bool? p11 { get; set; } public bool? q11 { get; set; } public bool? r11 { get; set; } public bool? s11 { get; set; } public bool? t11 { get; set; } public bool? u11 { get; set; } public bool? v11 { get; set; } public bool? w11 { get; set; } public bool? x11 { get; set; } public bool? y11 { get; set; } public bool? z11 { get; set; } 
            public bool? A12 { get; set; } public bool? B12 { get; set; } public bool? C12 { get; set; } public bool? D12 { get; set; } public bool? E12 { get; set; } public bool? F12 { get; set; } public bool? G12 { get; set; } public bool? H12 { get; set; } public bool? I12 { get; set; } public bool? J12 { get; set; } public bool? K12 { get; set; } public bool? L12 { get; set; } public bool? M12 { get; set; } public bool? N12 { get; set; } public bool? O12 { get; set; } public bool? P12 { get; set; } public bool? Q12 { get; set; } public bool? R12 { get; set; } public bool? S12 { get; set; } public bool? T12 { get; set; } public bool? U12 { get; set; } public bool? V12 { get; set; } public bool? W12 { get; set; } public bool? X12 { get; set; } public bool? Y12 { get; set; } public bool? Z12 { get; set; } public bool? a12 { get; set; } public bool? b12 { get; set; } public bool? c12 { get; set; } public bool? d12 { get; set; } public bool? e12 { get; set; } public bool? f12 { get; set; } public bool? g12 { get; set; } public bool? h12 { get; set; } public bool? i12 { get; set; } public bool? j12 { get; set; } public bool? k12 { get; set; } public bool? l12 { get; set; } public bool? m12 { get; set; } public bool? n12 { get; set; } public bool? o12 { get; set; } public bool? p12 { get; set; } public bool? q12 { get; set; } public bool? r12 { get; set; } public bool? s12 { get; set; } public bool? t12 { get; set; } public bool? u12 { get; set; } public bool? v12 { get; set; } public bool? w12 { get; set; } public bool? x12 { get; set; } public bool? y12 { get; set; } public bool? z12 { get; set; } 
            public bool? A13 { get; set; } public bool? B13 { get; set; } public bool? C13 { get; set; } public bool? D13 { get; set; } public bool? E13 { get; set; } public bool? F13 { get; set; } public bool? G13 { get; set; } public bool? H13 { get; set; } public bool? I13 { get; set; } public bool? J13 { get; set; } public bool? K13 { get; set; } public bool? L13 { get; set; } public bool? M13 { get; set; } public bool? N13 { get; set; } public bool? O13 { get; set; } public bool? P13 { get; set; } public bool? Q13 { get; set; } public bool? R13 { get; set; } public bool? S13 { get; set; } public bool? T13 { get; set; } public bool? U13 { get; set; } public bool? V13 { get; set; } public bool? W13 { get; set; } public bool? X13 { get; set; } public bool? Y13 { get; set; } public bool? Z13 { get; set; } public bool? a13 { get; set; } public bool? b13 { get; set; } public bool? c13 { get; set; } public bool? d13 { get; set; } public bool? e13 { get; set; } public bool? f13 { get; set; } public bool? g13 { get; set; } public bool? h13 { get; set; } public bool? i13 { get; set; } public bool? j13 { get; set; } public bool? k13 { get; set; } public bool? l13 { get; set; } public bool? m13 { get; set; } public bool? n13 { get; set; } public bool? o13 { get; set; } public bool? p13 { get; set; } public bool? q13 { get; set; } public bool? r13 { get; set; } public bool? s13 { get; set; } public bool? t13 { get; set; } public bool? u13 { get; set; } public bool? v13 { get; set; } public bool? w13 { get; set; } public bool? x13 { get; set; } public bool? y13 { get; set; } public bool? z13 { get; set; } 
            public bool? A14 { get; set; } public bool? B14 { get; set; } public bool? C14 { get; set; } public bool? D14 { get; set; } public bool? E14 { get; set; } public bool? F14 { get; set; } public bool? G14 { get; set; } public bool? H14 { get; set; } public bool? I14 { get; set; } public bool? J14 { get; set; } public bool? K14 { get; set; } public bool? L14 { get; set; } public bool? M14 { get; set; } public bool? N14 { get; set; } public bool? O14 { get; set; } public bool? P14 { get; set; } public bool? Q14 { get; set; } public bool? R14 { get; set; } public bool? S14 { get; set; } public bool? T14 { get; set; } public bool? U14 { get; set; } public bool? V14 { get; set; } public bool? W14 { get; set; } public bool? X14 { get; set; } public bool? Y14 { get; set; } public bool? Z14 { get; set; } public bool? a14 { get; set; } public bool? b14 { get; set; } public bool? c14 { get; set; } public bool? d14 { get; set; } public bool? e14 { get; set; } public bool? f14 { get; set; } public bool? g14 { get; set; } public bool? h14 { get; set; } public bool? i14 { get; set; } public bool? j14 { get; set; } public bool? k14 { get; set; } public bool? l14 { get; set; } public bool? m14 { get; set; } public bool? n14 { get; set; } public bool? o14 { get; set; } public bool? p14 { get; set; } public bool? q14 { get; set; } public bool? r14 { get; set; } public bool? s14 { get; set; } public bool? t14 { get; set; } public bool? u14 { get; set; } public bool? v14 { get; set; } public bool? w14 { get; set; } public bool? x14 { get; set; } public bool? y14 { get; set; } public bool? z14 { get; set; } 
            public bool? A15 { get; set; } public bool? B15 { get; set; } public bool? C15 { get; set; } public bool? D15 { get; set; } public bool? E15 { get; set; } public bool? F15 { get; set; } public bool? G15 { get; set; } public bool? H15 { get; set; } public bool? I15 { get; set; } public bool? J15 { get; set; } public bool? K15 { get; set; } public bool? L15 { get; set; } public bool? M15 { get; set; } public bool? N15 { get; set; } public bool? O15 { get; set; } public bool? P15 { get; set; } public bool? Q15 { get; set; } public bool? R15 { get; set; } public bool? S15 { get; set; } public bool? T15 { get; set; } public bool? U15 { get; set; } public bool? V15 { get; set; } public bool? W15 { get; set; } public bool? X15 { get; set; } public bool? Y15 { get; set; } public bool? Z15 { get; set; } public bool? a15 { get; set; } public bool? b15 { get; set; } public bool? c15 { get; set; } public bool? d15 { get; set; } public bool? e15 { get; set; } public bool? f15 { get; set; } public bool? g15 { get; set; } public bool? h15 { get; set; } public bool? i15 { get; set; } public bool? j15 { get; set; } public bool? k15 { get; set; } public bool? l15 { get; set; } public bool? m15 { get; set; } public bool? n15 { get; set; } public bool? o15 { get; set; } public bool? p15 { get; set; } public bool? q15 { get; set; } public bool? r15 { get; set; } public bool? s15 { get; set; } public bool? t15 { get; set; } public bool? u15 { get; set; } public bool? v15 { get; set; } public bool? w15 { get; set; } public bool? x15 { get; set; } public bool? y15 { get; set; } public bool? z15 { get; set; } 
            public bool? A16 { get; set; } public bool? B16 { get; set; } public bool? C16 { get; set; } public bool? D16 { get; set; } public bool? E16 { get; set; } public bool? F16 { get; set; } public bool? G16 { get; set; } public bool? H16 { get; set; } public bool? I16 { get; set; } public bool? J16 { get; set; } public bool? K16 { get; set; } public bool? L16 { get; set; } public bool? M16 { get; set; } public bool? N16 { get; set; } public bool? O16 { get; set; } public bool? P16 { get; set; } public bool? Q16 { get; set; } public bool? R16 { get; set; } public bool? S16 { get; set; } public bool? T16 { get; set; } public bool? U16 { get; set; } public bool? V16 { get; set; } public bool? W16 { get; set; } public bool? X16 { get; set; } public bool? Y16 { get; set; } public bool? Z16 { get; set; } public bool? a16 { get; set; } public bool? b16 { get; set; } public bool? c16 { get; set; } public bool? d16 { get; set; } public bool? e16 { get; set; } public bool? f16 { get; set; } public bool? g16 { get; set; } public bool? h16 { get; set; } public bool? i16 { get; set; } public bool? j16 { get; set; } public bool? k16 { get; set; } public bool? l16 { get; set; } public bool? m16 { get; set; } public bool? n16 { get; set; } public bool? o16 { get; set; } public bool? p16 { get; set; } public bool? q16 { get; set; } public bool? r16 { get; set; } public bool? s16 { get; set; } public bool? t16 { get; set; } public bool? u16 { get; set; } public bool? v16 { get; set; } public bool? w16 { get; set; } public bool? x16 { get; set; } public bool? y16 { get; set; } public bool? z16 { get; set; } 
            public bool? A17 { get; set; } public bool? B17 { get; set; } public bool? C17 { get; set; } public bool? D17 { get; set; } public bool? E17 { get; set; } public bool? F17 { get; set; } public bool? G17 { get; set; } public bool? H17 { get; set; } public bool? I17 { get; set; } public bool? J17 { get; set; } public bool? K17 { get; set; } public bool? L17 { get; set; } public bool? M17 { get; set; } public bool? N17 { get; set; } public bool? O17 { get; set; } public bool? P17 { get; set; } public bool? Q17 { get; set; } public bool? R17 { get; set; } public bool? S17 { get; set; } public bool? T17 { get; set; } public bool? U17 { get; set; } public bool? V17 { get; set; } public bool? W17 { get; set; } public bool? X17 { get; set; } public bool? Y17 { get; set; } public bool? Z17 { get; set; } public bool? a17 { get; set; } public bool? b17 { get; set; } public bool? c17 { get; set; } public bool? d17 { get; set; } public bool? e17 { get; set; } public bool? f17 { get; set; } public bool? g17 { get; set; } public bool? h17 { get; set; } public bool? i17 { get; set; } public bool? j17 { get; set; } public bool? k17 { get; set; } public bool? l17 { get; set; } public bool? m17 { get; set; } public bool? n17 { get; set; } public bool? o17 { get; set; } public bool? p17 { get; set; } public bool? q17 { get; set; } public bool? r17 { get; set; } public bool? s17 { get; set; } public bool? t17 { get; set; } public bool? u17 { get; set; } public bool? v17 { get; set; } public bool? w17 { get; set; } public bool? x17 { get; set; } public bool? y17 { get; set; } public bool? z17 { get; set; } 
            public bool? A18 { get; set; } public bool? B18 { get; set; } public bool? C18 { get; set; } public bool? D18 { get; set; } public bool? E18 { get; set; } public bool? F18 { get; set; } public bool? G18 { get; set; } public bool? H18 { get; set; } public bool? I18 { get; set; } public bool? J18 { get; set; } public bool? K18 { get; set; } public bool? L18 { get; set; } public bool? M18 { get; set; } public bool? N18 { get; set; } public bool? O18 { get; set; } public bool? P18 { get; set; } public bool? Q18 { get; set; } public bool? R18 { get; set; } public bool? S18 { get; set; } public bool? T18 { get; set; } public bool? U18 { get; set; } public bool? V18 { get; set; } public bool? W18 { get; set; } public bool? X18 { get; set; } public bool? Y18 { get; set; } public bool? Z18 { get; set; } public bool? a18 { get; set; } public bool? b18 { get; set; } public bool? c18 { get; set; } public bool? d18 { get; set; } public bool? e18 { get; set; } public bool? f18 { get; set; } public bool? g18 { get; set; } public bool? h18 { get; set; } public bool? i18 { get; set; } public bool? j18 { get; set; } public bool? k18 { get; set; } public bool? l18 { get; set; } public bool? m18 { get; set; } public bool? n18 { get; set; } public bool? o18 { get; set; } public bool? p18 { get; set; } public bool? q18 { get; set; } public bool? r18 { get; set; } public bool? s18 { get; set; } public bool? t18 { get; set; } public bool? u18 { get; set; } public bool? v18 { get; set; } public bool? w18 { get; set; } public bool? x18 { get; set; } public bool? y18 { get; set; } public bool? z18 { get; set; } 
            public bool? A19 { get; set; } public bool? B19 { get; set; } public bool? C19 { get; set; } public bool? D19 { get; set; } public bool? E19 { get; set; } public bool? F19 { get; set; } public bool? G19 { get; set; } public bool? H19 { get; set; } public bool? I19 { get; set; } public bool? J19 { get; set; } public bool? K19 { get; set; } public bool? L19 { get; set; } public bool? M19 { get; set; } public bool? N19 { get; set; } public bool? O19 { get; set; } public bool? P19 { get; set; } public bool? Q19 { get; set; } public bool? R19 { get; set; } public bool? S19 { get; set; } public bool? T19 { get; set; } public bool? U19 { get; set; } public bool? V19 { get; set; } public bool? W19 { get; set; } public bool? X19 { get; set; } public bool? Y19 { get; set; } public bool? Z19 { get; set; } public bool? a19 { get; set; } public bool? b19 { get; set; } public bool? c19 { get; set; } public bool? d19 { get; set; } public bool? e19 { get; set; } public bool? f19 { get; set; } public bool? g19 { get; set; } public bool? h19 { get; set; } public bool? i19 { get; set; } public bool? j19 { get; set; } public bool? k19 { get; set; } public bool? l19 { get; set; } public bool? m19 { get; set; } public bool? n19 { get; set; } public bool? o19 { get; set; } public bool? p19 { get; set; } public bool? q19 { get; set; } public bool? r19 { get; set; } public bool? s19 { get; set; } public bool? t19 { get; set; } public bool? u19 { get; set; } public bool? v19 { get; set; } public bool? w19 { get; set; } public bool? x19 { get; set; } public bool? y19 { get; set; } public bool? z19 { get; set; } 
            public bool? A20 { get; set; } public bool? B20 { get; set; } public bool? C20 { get; set; } public bool? D20 { get; set; } public bool? E20 { get; set; } public bool? F20 { get; set; } public bool? G20 { get; set; } public bool? H20 { get; set; } public bool? I20 { get; set; } public bool? J20 { get; set; } public bool? K20 { get; set; } public bool? L20 { get; set; } public bool? M20 { get; set; } public bool? N20 { get; set; } public bool? O20 { get; set; } public bool? P20 { get; set; } public bool? Q20 { get; set; } public bool? R20 { get; set; } public bool? S20 { get; set; } public bool? T20 { get; set; } public bool? U20 { get; set; } public bool? V20 { get; set; } public bool? W20 { get; set; } public bool? X20 { get; set; } public bool? Y20 { get; set; } public bool? Z20 { get; set; } public bool? a20 { get; set; } public bool? b20 { get; set; } public bool? c20 { get; set; } public bool? d20 { get; set; } public bool? e20 { get; set; } public bool? f20 { get; set; } public bool? g20 { get; set; } public bool? h20 { get; set; } public bool? i20 { get; set; } public bool? j20 { get; set; } public bool? k20 { get; set; } public bool? l20 { get; set; } public bool? m20 { get; set; } public bool? n20 { get; set; } public bool? o20 { get; set; } public bool? p20 { get; set; } public bool? q20 { get; set; } public bool? r20 { get; set; } public bool? s20 { get; set; } public bool? t20 { get; set; } public bool? u20 { get; set; } public bool? v20 { get; set; } public bool? w20 { get; set; } public bool? x20 { get; set; } public bool? y20 { get; set; } public bool? z20 { get; set; } 
            public bool? A21 { get; set; } public bool? B21 { get; set; } public bool? C21 { get; set; } public bool? D21 { get; set; } public bool? E21 { get; set; } public bool? F21 { get; set; } public bool? G21 { get; set; } public bool? H21 { get; set; } public bool? I21 { get; set; } public bool? J21 { get; set; } public bool? K21 { get; set; } public bool? L21 { get; set; } public bool? M21 { get; set; } public bool? N21 { get; set; } public bool? O21 { get; set; } public bool? P21 { get; set; } public bool? Q21 { get; set; } public bool? R21 { get; set; } public bool? S21 { get; set; } public bool? T21 { get; set; } public bool? U21 { get; set; } public bool? V21 { get; set; } public bool? W21 { get; set; } public bool? X21 { get; set; } public bool? Y21 { get; set; } public bool? Z21 { get; set; } public bool? a21 { get; set; } public bool? b21 { get; set; } public bool? c21 { get; set; } public bool? d21 { get; set; } public bool? e21 { get; set; } public bool? f21 { get; set; } public bool? g21 { get; set; } public bool? h21 { get; set; } public bool? i21 { get; set; } public bool? j21 { get; set; } public bool? k21 { get; set; } public bool? l21 { get; set; } public bool? m21 { get; set; } public bool? n21 { get; set; } public bool? o21 { get; set; } public bool? p21 { get; set; } public bool? q21 { get; set; } public bool? r21 { get; set; } public bool? s21 { get; set; } public bool? t21 { get; set; } public bool? u21 { get; set; } public bool? v21 { get; set; } public bool? w21 { get; set; } public bool? x21 { get; set; } public bool? y21 { get; set; } public bool? z21 { get; set; } 
            public bool? A22 { get; set; } public bool? B22 { get; set; } public bool? C22 { get; set; } public bool? D22 { get; set; } public bool? E22 { get; set; } public bool? F22 { get; set; } public bool? G22 { get; set; } public bool? H22 { get; set; } public bool? I22 { get; set; } public bool? J22 { get; set; } public bool? K22 { get; set; } public bool? L22 { get; set; } public bool? M22 { get; set; } public bool? N22 { get; set; } public bool? O22 { get; set; } public bool? P22 { get; set; } public bool? Q22 { get; set; } public bool? R22 { get; set; } public bool? S22 { get; set; } public bool? T22 { get; set; } public bool? U22 { get; set; } public bool? V22 { get; set; } public bool? W22 { get; set; } public bool? X22 { get; set; } public bool? Y22 { get; set; } public bool? Z22 { get; set; } public bool? a22 { get; set; } public bool? b22 { get; set; } public bool? c22 { get; set; } public bool? d22 { get; set; } public bool? e22 { get; set; } public bool? f22 { get; set; } public bool? g22 { get; set; } public bool? h22 { get; set; } public bool? i22 { get; set; } public bool? j22 { get; set; } public bool? k22 { get; set; } public bool? l22 { get; set; } public bool? m22 { get; set; } public bool? n22 { get; set; } public bool? o22 { get; set; } public bool? p22 { get; set; } public bool? q22 { get; set; } public bool? r22 { get; set; } public bool? s22 { get; set; } public bool? t22 { get; set; } public bool? u22 { get; set; } public bool? v22 { get; set; } public bool? w22 { get; set; } public bool? x22 { get; set; } public bool? y22 { get; set; } public bool? z22 { get; set; } 
            public bool? A23 { get; set; } public bool? B23 { get; set; } public bool? C23 { get; set; } public bool? D23 { get; set; } public bool? E23 { get; set; } public bool? F23 { get; set; } public bool? G23 { get; set; } public bool? H23 { get; set; } public bool? I23 { get; set; } public bool? J23 { get; set; } public bool? K23 { get; set; } public bool? L23 { get; set; } public bool? M23 { get; set; } public bool? N23 { get; set; } public bool? O23 { get; set; } public bool? P23 { get; set; } public bool? Q23 { get; set; } public bool? R23 { get; set; } public bool? S23 { get; set; } public bool? T23 { get; set; } public bool? U23 { get; set; } public bool? V23 { get; set; } public bool? W23 { get; set; } public bool? X23 { get; set; } public bool? Y23 { get; set; } public bool? Z23 { get; set; } public bool? a23 { get; set; } public bool? b23 { get; set; } public bool? c23 { get; set; } public bool? d23 { get; set; } public bool? e23 { get; set; } public bool? f23 { get; set; } public bool? g23 { get; set; } public bool? h23 { get; set; } public bool? i23 { get; set; } public bool? j23 { get; set; } public bool? k23 { get; set; } public bool? l23 { get; set; } public bool? m23 { get; set; } public bool? n23 { get; set; } public bool? o23 { get; set; } public bool? p23 { get; set; } public bool? q23 { get; set; } public bool? r23 { get; set; } public bool? s23 { get; set; } public bool? t23 { get; set; } public bool? u23 { get; set; } public bool? v23 { get; set; } public bool? w23 { get; set; } public bool? x23 { get; set; } public bool? y23 { get; set; } public bool? z23 { get; set; } 
            public bool? A24 { get; set; } public bool? B24 { get; set; } public bool? C24 { get; set; } public bool? D24 { get; set; } public bool? E24 { get; set; } public bool? F24 { get; set; } public bool? G24 { get; set; } public bool? H24 { get; set; } public bool? I24 { get; set; } public bool? J24 { get; set; } public bool? K24 { get; set; } public bool? L24 { get; set; } public bool? M24 { get; set; } public bool? N24 { get; set; } public bool? O24 { get; set; } public bool? P24 { get; set; } public bool? Q24 { get; set; } public bool? R24 { get; set; } public bool? S24 { get; set; } public bool? T24 { get; set; } public bool? U24 { get; set; } public bool? V24 { get; set; } public bool? W24 { get; set; } public bool? X24 { get; set; } public bool? Y24 { get; set; } public bool? Z24 { get; set; } public bool? a24 { get; set; } public bool? b24 { get; set; } public bool? c24 { get; set; } public bool? d24 { get; set; } public bool? e24 { get; set; } public bool? f24 { get; set; } public bool? g24 { get; set; } public bool? h24 { get; set; } public bool? i24 { get; set; } public bool? j24 { get; set; } public bool? k24 { get; set; } public bool? l24 { get; set; } public bool? m24 { get; set; } public bool? n24 { get; set; } public bool? o24 { get; set; } public bool? p24 { get; set; } public bool? q24 { get; set; } public bool? r24 { get; set; } public bool? s24 { get; set; } public bool? t24 { get; set; } public bool? u24 { get; set; } public bool? v24 { get; set; } public bool? w24 { get; set; } public bool? x24 { get; set; } public bool? y24 { get; set; } public bool? z24 { get; set; } 
            public bool? A25 { get; set; } public bool? B25 { get; set; } public bool? C25 { get; set; } public bool? D25 { get; set; } public bool? E25 { get; set; } public bool? F25 { get; set; } public bool? G25 { get; set; } public bool? H25 { get; set; } public bool? I25 { get; set; } public bool? J25 { get; set; } public bool? K25 { get; set; } public bool? L25 { get; set; } public bool? M25 { get; set; } public bool? N25 { get; set; } public bool? O25 { get; set; } public bool? P25 { get; set; } public bool? Q25 { get; set; } public bool? R25 { get; set; } public bool? S25 { get; set; } public bool? T25 { get; set; } public bool? U25 { get; set; } public bool? V25 { get; set; } public bool? W25 { get; set; } public bool? X25 { get; set; } public bool? Y25 { get; set; } public bool? Z25 { get; set; } public bool? a25 { get; set; } public bool? b25 { get; set; } public bool? c25 { get; set; } public bool? d25 { get; set; } public bool? e25 { get; set; } public bool? f25 { get; set; } public bool? g25 { get; set; } public bool? h25 { get; set; } public bool? i25 { get; set; } public bool? j25 { get; set; } public bool? k25 { get; set; } public bool? l25 { get; set; } public bool? m25 { get; set; } public bool? n25 { get; set; } public bool? o25 { get; set; } public bool? p25 { get; set; } public bool? q25 { get; set; } public bool? r25 { get; set; } public bool? s25 { get; set; } public bool? t25 { get; set; } public bool? u25 { get; set; } public bool? v25 { get; set; } public bool? w25 { get; set; } public bool? x25 { get; set; } public bool? y25 { get; set; } public bool? z25 { get; set; } 
            public bool? A26 { get; set; } public bool? B26 { get; set; } public bool? C26 { get; set; } public bool? D26 { get; set; } public bool? E26 { get; set; } public bool? F26 { get; set; } public bool? G26 { get; set; } public bool? H26 { get; set; } public bool? I26 { get; set; } public bool? J26 { get; set; } public bool? K26 { get; set; } public bool? L26 { get; set; } public bool? M26 { get; set; } public bool? N26 { get; set; } public bool? O26 { get; set; } public bool? P26 { get; set; } public bool? Q26 { get; set; } public bool? R26 { get; set; } public bool? S26 { get; set; } public bool? T26 { get; set; } public bool? U26 { get; set; } public bool? V26 { get; set; } public bool? W26 { get; set; } public bool? X26 { get; set; } public bool? Y26 { get; set; } public bool? Z26 { get; set; } public bool? a26 { get; set; } public bool? b26 { get; set; } public bool? c26 { get; set; } public bool? d26 { get; set; } public bool? e26 { get; set; } public bool? f26 { get; set; } public bool? g26 { get; set; } public bool? h26 { get; set; } public bool? i26 { get; set; } public bool? j26 { get; set; } public bool? k26 { get; set; } public bool? l26 { get; set; } public bool? m26 { get; set; } public bool? n26 { get; set; } public bool? o26 { get; set; } public bool? p26 { get; set; } public bool? q26 { get; set; } public bool? r26 { get; set; } public bool? s26 { get; set; } public bool? t26 { get; set; } public bool? u26 { get; set; } public bool? v26 { get; set; } public bool? w26 { get; set; } public bool? x26 { get; set; } public bool? y26 { get; set; } public bool? z26 { get; set; } 
            public bool? A27 { get; set; } public bool? B27 { get; set; } public bool? C27 { get; set; } public bool? D27 { get; set; } public bool? E27 { get; set; } public bool? F27 { get; set; } public bool? G27 { get; set; } public bool? H27 { get; set; } public bool? I27 { get; set; } public bool? J27 { get; set; } public bool? K27 { get; set; } public bool? L27 { get; set; } public bool? M27 { get; set; } public bool? N27 { get; set; } public bool? O27 { get; set; } public bool? P27 { get; set; } public bool? Q27 { get; set; } public bool? R27 { get; set; } public bool? S27 { get; set; } public bool? T27 { get; set; } public bool? U27 { get; set; } public bool? V27 { get; set; } public bool? W27 { get; set; } public bool? X27 { get; set; } public bool? Y27 { get; set; } public bool? Z27 { get; set; } public bool? a27 { get; set; } public bool? b27 { get; set; } public bool? c27 { get; set; } public bool? d27 { get; set; } public bool? e27 { get; set; } public bool? f27 { get; set; } public bool? g27 { get; set; } public bool? h27 { get; set; } public bool? i27 { get; set; } public bool? j27 { get; set; } public bool? k27 { get; set; } public bool? l27 { get; set; } public bool? m27 { get; set; } public bool? n27 { get; set; } public bool? o27 { get; set; } public bool? p27 { get; set; } public bool? q27 { get; set; } public bool? r27 { get; set; } public bool? s27 { get; set; } public bool? t27 { get; set; } public bool? u27 { get; set; } public bool? v27 { get; set; } public bool? w27 { get; set; } public bool? x27 { get; set; } public bool? y27 { get; set; } public bool? z27 { get; set; } 
            public bool? A28 { get; set; } public bool? B28 { get; set; } public bool? C28 { get; set; } public bool? D28 { get; set; } public bool? E28 { get; set; } public bool? F28 { get; set; } public bool? G28 { get; set; } public bool? H28 { get; set; } public bool? I28 { get; set; } public bool? J28 { get; set; } public bool? K28 { get; set; } public bool? L28 { get; set; } public bool? M28 { get; set; } public bool? N28 { get; set; } public bool? O28 { get; set; } public bool? P28 { get; set; } public bool? Q28 { get; set; } public bool? R28 { get; set; } public bool? S28 { get; set; } public bool? T28 { get; set; } public bool? U28 { get; set; } public bool? V28 { get; set; } public bool? W28 { get; set; } public bool? X28 { get; set; } public bool? Y28 { get; set; } public bool? Z28 { get; set; } public bool? a28 { get; set; } public bool? b28 { get; set; } public bool? c28 { get; set; } public bool? d28 { get; set; } public bool? e28 { get; set; } public bool? f28 { get; set; } public bool? g28 { get; set; } public bool? h28 { get; set; } public bool? i28 { get; set; } public bool? j28 { get; set; } public bool? k28 { get; set; } public bool? l28 { get; set; } public bool? m28 { get; set; } public bool? n28 { get; set; } public bool? o28 { get; set; } public bool? p28 { get; set; } public bool? q28 { get; set; } public bool? r28 { get; set; } public bool? s28 { get; set; } public bool? t28 { get; set; } public bool? u28 { get; set; } public bool? v28 { get; set; } public bool? w28 { get; set; } public bool? x28 { get; set; } public bool? y28 { get; set; } public bool? z28 { get; set; } 
            public bool? A29 { get; set; } public bool? B29 { get; set; } public bool? C29 { get; set; } public bool? D29 { get; set; } public bool? E29 { get; set; } public bool? F29 { get; set; } public bool? G29 { get; set; } public bool? H29 { get; set; } public bool? I29 { get; set; } public bool? J29 { get; set; } public bool? K29 { get; set; } public bool? L29 { get; set; } public bool? M29 { get; set; } public bool? N29 { get; set; } public bool? O29 { get; set; } public bool? P29 { get; set; } public bool? Q29 { get; set; } public bool? R29 { get; set; } public bool? S29 { get; set; } public bool? T29 { get; set; } public bool? U29 { get; set; } public bool? V29 { get; set; } public bool? W29 { get; set; } public bool? X29 { get; set; } public bool? Y29 { get; set; } public bool? Z29 { get; set; } public bool? a29 { get; set; } public bool? b29 { get; set; } public bool? c29 { get; set; } public bool? d29 { get; set; } public bool? e29 { get; set; } public bool? f29 { get; set; } public bool? g29 { get; set; } public bool? h29 { get; set; } public bool? i29 { get; set; } public bool? j29 { get; set; } public bool? k29 { get; set; } public bool? l29 { get; set; } public bool? m29 { get; set; } public bool? n29 { get; set; } public bool? o29 { get; set; } public bool? p29 { get; set; } public bool? q29 { get; set; } public bool? r29 { get; set; } public bool? s29 { get; set; } public bool? t29 { get; set; } public bool? u29 { get; set; } public bool? v29 { get; set; } public bool? w29 { get; set; } public bool? x29 { get; set; } public bool? y29 { get; set; } public bool? z29 { get; set; } 
            public bool? A31 { get; set; } public bool? B31 { get; set; } public bool? C31 { get; set; } public bool? D31 { get; set; } public bool? E31 { get; set; } public bool? F31 { get; set; } public bool? G31 { get; set; } public bool? H31 { get; set; } public bool? I31 { get; set; } public bool? J31 { get; set; } public bool? K31 { get; set; } public bool? L31 { get; set; } public bool? M31 { get; set; } public bool? N31 { get; set; } public bool? O31 { get; set; } public bool? P31 { get; set; } public bool? Q31 { get; set; } public bool? R31 { get; set; } public bool? S31 { get; set; } public bool? T31 { get; set; } public bool? U31 { get; set; } public bool? V31 { get; set; } public bool? W31 { get; set; } public bool? X31 { get; set; } public bool? Y31 { get; set; } public bool? Z31 { get; set; } public bool? a31 { get; set; } public bool? b31 { get; set; } public bool? c31 { get; set; } public bool? d31 { get; set; } public bool? e31 { get; set; } public bool? f31 { get; set; } public bool? g31 { get; set; } public bool? h31 { get; set; } public bool? i31 { get; set; } public bool? j31 { get; set; } public bool? k31 { get; set; } public bool? l31 { get; set; } public bool? m31 { get; set; } public bool? n31 { get; set; } public bool? o31 { get; set; } public bool? p31 { get; set; } public bool? q31 { get; set; } public bool? r31 { get; set; } public bool? s31 { get; set; } public bool? t31 { get; set; } public bool? u31 { get; set; } public bool? v31 { get; set; } public bool? w31 { get; set; } public bool? x31 { get; set; } public bool? y31 { get; set; } public bool? z31 { get; set; } 
            public bool? A32 { get; set; } public bool? B32 { get; set; } public bool? C32 { get; set; } public bool? D32 { get; set; } public bool? E32 { get; set; } public bool? F32 { get; set; } public bool? G32 { get; set; } public bool? H32 { get; set; } public bool? I32 { get; set; } public bool? J32 { get; set; } public bool? K32 { get; set; } public bool? L32 { get; set; } public bool? M32 { get; set; } public bool? N32 { get; set; } public bool? O32 { get; set; } public bool? P32 { get; set; } public bool? Q32 { get; set; } public bool? R32 { get; set; } public bool? S32 { get; set; } public bool? T32 { get; set; } public bool? U32 { get; set; } public bool? V32 { get; set; } public bool? W32 { get; set; } public bool? X32 { get; set; } public bool? Y32 { get; set; } public bool? Z32 { get; set; } public bool? a32 { get; set; } public bool? b32 { get; set; } public bool? c32 { get; set; } public bool? d32 { get; set; } public bool? e32 { get; set; } public bool? f32 { get; set; } public bool? g32 { get; set; } public bool? h32 { get; set; } public bool? i32 { get; set; } public bool? j32 { get; set; } public bool? k32 { get; set; } public bool? l32 { get; set; } public bool? m32 { get; set; } public bool? n32 { get; set; } public bool? o32 { get; set; } public bool? p32 { get; set; } public bool? q32 { get; set; } public bool? r32 { get; set; } public bool? s32 { get; set; } public bool? t32 { get; set; } public bool? u32 { get; set; } public bool? v32 { get; set; } public bool? w32 { get; set; } public bool? x32 { get; set; } public bool? y32 { get; set; } public bool? z32 { get; set; } 
            public bool? A33 { get; set; } public bool? B33 { get; set; } public bool? C33 { get; set; } public bool? D33 { get; set; } public bool? E33 { get; set; } public bool? F33 { get; set; } public bool? G33 { get; set; } public bool? H33 { get; set; } public bool? I33 { get; set; } public bool? J33 { get; set; } public bool? K33 { get; set; } public bool? L33 { get; set; } public bool? M33 { get; set; } public bool? N33 { get; set; } public bool? O33 { get; set; } public bool? P33 { get; set; } public bool? Q33 { get; set; } public bool? R33 { get; set; } public bool? S33 { get; set; } public bool? T33 { get; set; } public bool? U33 { get; set; } public bool? V33 { get; set; } public bool? W33 { get; set; } public bool? X33 { get; set; } public bool? Y33 { get; set; } public bool? Z33 { get; set; } public bool? a33 { get; set; } public bool? b33 { get; set; } public bool? c33 { get; set; } public bool? d33 { get; set; } public bool? e33 { get; set; } public bool? f33 { get; set; } public bool? g33 { get; set; } public bool? h33 { get; set; } public bool? i33 { get; set; } public bool? j33 { get; set; } public bool? k33 { get; set; } public bool? l33 { get; set; } public bool? m33 { get; set; } public bool? n33 { get; set; } public bool? o33 { get; set; } public bool? p33 { get; set; } public bool? q33 { get; set; } public bool? r33 { get; set; } public bool? s33 { get; set; } public bool? t33 { get; set; } public bool? u33 { get; set; } public bool? v33 { get; set; } public bool? w33 { get; set; } public bool? x33 { get; set; } public bool? y33 { get; set; } public bool? z33 { get; set; } 
            public bool? A34 { get; set; } public bool? B34 { get; set; } public bool? C34 { get; set; } public bool? D34 { get; set; } public bool? E34 { get; set; } public bool? F34 { get; set; } public bool? G34 { get; set; } public bool? H34 { get; set; } public bool? I34 { get; set; } public bool? J34 { get; set; } public bool? K34 { get; set; } public bool? L34 { get; set; } public bool? M34 { get; set; } public bool? N34 { get; set; } public bool? O34 { get; set; } public bool? P34 { get; set; } public bool? Q34 { get; set; } public bool? R34 { get; set; } public bool? S34 { get; set; } public bool? T34 { get; set; } public bool? U34 { get; set; } public bool? V34 { get; set; } public bool? W34 { get; set; } public bool? X34 { get; set; } public bool? Y34 { get; set; } public bool? Z34 { get; set; } public bool? a34 { get; set; } public bool? b34 { get; set; } public bool? c34 { get; set; } public bool? d34 { get; set; } public bool? e34 { get; set; } public bool? f34 { get; set; } public bool? g34 { get; set; } public bool? h34 { get; set; } public bool? i34 { get; set; } public bool? j34 { get; set; } public bool? k34 { get; set; } public bool? l34 { get; set; } public bool? m34 { get; set; } public bool? n34 { get; set; } public bool? o34 { get; set; } public bool? p34 { get; set; } public bool? q34 { get; set; } public bool? r34 { get; set; } public bool? s34 { get; set; } public bool? t34 { get; set; } public bool? u34 { get; set; } public bool? v34 { get; set; } public bool? w34 { get; set; } public bool? x34 { get; set; } public bool? y34 { get; set; } public bool? z34 { get; set; } 
            public bool? A35 { get; set; } public bool? B35 { get; set; } public bool? C35 { get; set; } public bool? D35 { get; set; } public bool? E35 { get; set; } public bool? F35 { get; set; } public bool? G35 { get; set; } public bool? H35 { get; set; } public bool? I35 { get; set; } public bool? J35 { get; set; } public bool? K35 { get; set; } public bool? L35 { get; set; } public bool? M35 { get; set; } public bool? N35 { get; set; } public bool? O35 { get; set; } public bool? P35 { get; set; } public bool? Q35 { get; set; } public bool? R35 { get; set; } public bool? S35 { get; set; } public bool? T35 { get; set; } public bool? U35 { get; set; } public bool? V35 { get; set; } public bool? W35 { get; set; } public bool? X35 { get; set; } public bool? Y35 { get; set; } public bool? Z35 { get; set; } public bool? a35 { get; set; } public bool? b35 { get; set; } public bool? c35 { get; set; } public bool? d35 { get; set; } public bool? e35 { get; set; } public bool? f35 { get; set; } public bool? g35 { get; set; } public bool? h35 { get; set; } public bool? i35 { get; set; } public bool? j35 { get; set; } public bool? k35 { get; set; } public bool? l35 { get; set; } public bool? m35 { get; set; } public bool? n35 { get; set; } public bool? o35 { get; set; } public bool? p35 { get; set; } public bool? q35 { get; set; } public bool? r35 { get; set; } public bool? s35 { get; set; } public bool? t35 { get; set; } public bool? u35 { get; set; } public bool? v35 { get; set; } public bool? w35 { get; set; } public bool? x35 { get; set; } public bool? y35 { get; set; } public bool? z35 { get; set; } 
            public bool? A36 { get; set; } public bool? B36 { get; set; } public bool? C36 { get; set; } public bool? D36 { get; set; } public bool? E36 { get; set; } public bool? F36 { get; set; } public bool? G36 { get; set; } public bool? H36 { get; set; } public bool? I36 { get; set; } public bool? J36 { get; set; } public bool? K36 { get; set; } public bool? L36 { get; set; } public bool? M36 { get; set; } public bool? N36 { get; set; } public bool? O36 { get; set; } public bool? P36 { get; set; } public bool? Q36 { get; set; } public bool? R36 { get; set; } public bool? S36 { get; set; } public bool? T36 { get; set; } public bool? U36 { get; set; } public bool? V36 { get; set; } public bool? W36 { get; set; } public bool? X36 { get; set; } public bool? Y36 { get; set; } public bool? Z36 { get; set; } public bool? a36 { get; set; } public bool? b36 { get; set; } public bool? c36 { get; set; } public bool? d36 { get; set; } public bool? e36 { get; set; } public bool? f36 { get; set; } public bool? g36 { get; set; } public bool? h36 { get; set; } public bool? i36 { get; set; } public bool? j36 { get; set; } public bool? k36 { get; set; } public bool? l36 { get; set; } public bool? m36 { get; set; } public bool? n36 { get; set; } public bool? o36 { get; set; } public bool? p36 { get; set; } public bool? q36 { get; set; } public bool? r36 { get; set; } public bool? s36 { get; set; } public bool? t36 { get; set; } public bool? u36 { get; set; } public bool? v36 { get; set; } public bool? w36 { get; set; } public bool? x36 { get; set; } public bool? y36 { get; set; } public bool? z36 { get; set; } 
            public bool? A37 { get; set; } public bool? B37 { get; set; } public bool? C37 { get; set; } public bool? D37 { get; set; } public bool? E37 { get; set; } public bool? F37 { get; set; } public bool? G37 { get; set; } public bool? H37 { get; set; } public bool? I37 { get; set; } public bool? J37 { get; set; } public bool? K37 { get; set; } public bool? L37 { get; set; } public bool? M37 { get; set; } public bool? N37 { get; set; } public bool? O37 { get; set; } public bool? P37 { get; set; } public bool? Q37 { get; set; } public bool? R37 { get; set; } public bool? S37 { get; set; } public bool? T37 { get; set; } public bool? U37 { get; set; } public bool? V37 { get; set; } public bool? W37 { get; set; } public bool? X37 { get; set; } public bool? Y37 { get; set; } public bool? Z37 { get; set; } public bool? a37 { get; set; } public bool? b37 { get; set; } public bool? c37 { get; set; } public bool? d37 { get; set; } public bool? e37 { get; set; } public bool? f37 { get; set; } public bool? g37 { get; set; } public bool? h37 { get; set; } public bool? i37 { get; set; } public bool? j37 { get; set; } public bool? k37 { get; set; } public bool? l37 { get; set; } public bool? m37 { get; set; } public bool? n37 { get; set; } public bool? o37 { get; set; } public bool? p37 { get; set; } public bool? q37 { get; set; } public bool? r37 { get; set; } public bool? s37 { get; set; } public bool? t37 { get; set; } public bool? u37 { get; set; } public bool? v37 { get; set; } public bool? w37 { get; set; } public bool? x37 { get; set; } public bool? y37 { get; set; } public bool? z37 { get; set; } 
            public bool? A38 { get; set; } public bool? B38 { get; set; } public bool? C38 { get; set; } public bool? D38 { get; set; } public bool? E38 { get; set; } public bool? F38 { get; set; } public bool? G38 { get; set; } public bool? H38 { get; set; } public bool? I38 { get; set; } public bool? J38 { get; set; } public bool? K38 { get; set; } public bool? L38 { get; set; } public bool? M38 { get; set; } public bool? N38 { get; set; } public bool? O38 { get; set; } public bool? P38 { get; set; } public bool? Q38 { get; set; } public bool? R38 { get; set; } public bool? S38 { get; set; } public bool? T38 { get; set; } public bool? U38 { get; set; } public bool? V38 { get; set; } public bool? W38 { get; set; } public bool? X38 { get; set; } public bool? Y38 { get; set; } public bool? Z38 { get; set; } public bool? a38 { get; set; } public bool? b38 { get; set; } public bool? c38 { get; set; } public bool? d38 { get; set; } public bool? e38 { get; set; } public bool? f38 { get; set; } public bool? g38 { get; set; } public bool? h38 { get; set; } public bool? i38 { get; set; } public bool? j38 { get; set; } public bool? k38 { get; set; } public bool? l38 { get; set; } public bool? m38 { get; set; } public bool? n38 { get; set; } public bool? o38 { get; set; } public bool? p38 { get; set; } public bool? q38 { get; set; } public bool? r38 { get; set; } public bool? s38 { get; set; } public bool? t38 { get; set; } public bool? u38 { get; set; } public bool? v38 { get; set; } public bool? w38 { get; set; } public bool? x38 { get; set; } public bool? y38 { get; set; } public bool? z38 { get; set; } 
            public bool? A39 { get; set; } public bool? B39 { get; set; } public bool? C39 { get; set; } public bool? D39 { get; set; } public bool? E39 { get; set; } public bool? F39 { get; set; } public bool? G39 { get; set; } public bool? H39 { get; set; } public bool? I39 { get; set; } public bool? J39 { get; set; } public bool? K39 { get; set; } public bool? L39 { get; set; } public bool? M39 { get; set; } public bool? N39 { get; set; } public bool? O39 { get; set; } public bool? P39 { get; set; } public bool? Q39 { get; set; } public bool? R39 { get; set; } public bool? S39 { get; set; } public bool? T39 { get; set; } public bool? U39 { get; set; } public bool? V39 { get; set; } public bool? W39 { get; set; } public bool? X39 { get; set; } public bool? Y39 { get; set; } public bool? Z39 { get; set; } public bool? a39 { get; set; } public bool? b39 { get; set; } public bool? c39 { get; set; } public bool? d39 { get; set; } public bool? e39 { get; set; } public bool? f39 { get; set; } public bool? g39 { get; set; } public bool? h39 { get; set; } public bool? i39 { get; set; } public bool? j39 { get; set; } public bool? k39 { get; set; } public bool? l39 { get; set; } public bool? m39 { get; set; } public bool? n39 { get; set; } public bool? o39 { get; set; } public bool? p39 { get; set; } public bool? q39 { get; set; } public bool? r39 { get; set; } public bool? s39 { get; set; } public bool? t39 { get; set; } public bool? u39 { get; set; } public bool? v39 { get; set; } public bool? w39 { get; set; } public bool? x39 { get; set; } public bool? y39 { get; set; } public bool? z39 { get; set; } 
            public bool? A41 { get; set; } public bool? B41 { get; set; } public bool? C41 { get; set; } public bool? D41 { get; set; } public bool? E41 { get; set; } public bool? F41 { get; set; } public bool? G41 { get; set; } public bool? H41 { get; set; } public bool? I41 { get; set; } public bool? J41 { get; set; } public bool? K41 { get; set; } public bool? L41 { get; set; } public bool? M41 { get; set; } public bool? N41 { get; set; } public bool? O41 { get; set; } public bool? P41 { get; set; } public bool? Q41 { get; set; } public bool? R41 { get; set; } public bool? S41 { get; set; } public bool? T41 { get; set; } public bool? U41 { get; set; } public bool? V41 { get; set; } public bool? W41 { get; set; } public bool? X41 { get; set; } public bool? Y41 { get; set; } public bool? Z41 { get; set; } public bool? a41 { get; set; } public bool? b41 { get; set; } public bool? c41 { get; set; } public bool? d41 { get; set; } public bool? e41 { get; set; } public bool? f41 { get; set; } public bool? g41 { get; set; } public bool? h41 { get; set; } public bool? i41 { get; set; } public bool? j41 { get; set; } public bool? k41 { get; set; } public bool? l41 { get; set; } public bool? m41 { get; set; } public bool? n41 { get; set; } public bool? o41 { get; set; } public bool? p41 { get; set; } public bool? q41 { get; set; } public bool? r41 { get; set; } public bool? s41 { get; set; } public bool? t41 { get; set; } public bool? u41 { get; set; } public bool? v41 { get; set; } public bool? w41 { get; set; } public bool? x41 { get; set; } public bool? y41 { get; set; } public bool? z41 { get; set; } 
            public bool? A42 { get; set; } public bool? B42 { get; set; } public bool? C42 { get; set; } public bool? D42 { get; set; } public bool? E42 { get; set; } public bool? F42 { get; set; } public bool? G42 { get; set; } public bool? H42 { get; set; } public bool? I42 { get; set; } public bool? J42 { get; set; } public bool? K42 { get; set; } public bool? L42 { get; set; } public bool? M42 { get; set; } public bool? N42 { get; set; } public bool? O42 { get; set; } public bool? P42 { get; set; } public bool? Q42 { get; set; } public bool? R42 { get; set; } public bool? S42 { get; set; } public bool? T42 { get; set; } public bool? U42 { get; set; } public bool? V42 { get; set; } public bool? W42 { get; set; } public bool? X42 { get; set; } public bool? Y42 { get; set; } public bool? Z42 { get; set; } public bool? a42 { get; set; } public bool? b42 { get; set; } public bool? c42 { get; set; } public bool? d42 { get; set; } public bool? e42 { get; set; } public bool? f42 { get; set; } public bool? g42 { get; set; } public bool? h42 { get; set; } public bool? i42 { get; set; } public bool? j42 { get; set; } public bool? k42 { get; set; } public bool? l42 { get; set; } public bool? m42 { get; set; } public bool? n42 { get; set; } public bool? o42 { get; set; } public bool? p42 { get; set; } public bool? q42 { get; set; } public bool? r42 { get; set; } public bool? s42 { get; set; } public bool? t42 { get; set; } public bool? u42 { get; set; } public bool? v42 { get; set; } public bool? w42 { get; set; } public bool? x42 { get; set; } public bool? y42 { get; set; } public bool? z42 { get; set; } 
            public bool? A43 { get; set; } public bool? B43 { get; set; } public bool? C43 { get; set; } public bool? D43 { get; set; } public bool? E43 { get; set; } public bool? F43 { get; set; } public bool? G43 { get; set; } public bool? H43 { get; set; } public bool? I43 { get; set; } public bool? J43 { get; set; } public bool? K43 { get; set; } public bool? L43 { get; set; } public bool? M43 { get; set; } public bool? N43 { get; set; } public bool? O43 { get; set; } public bool? P43 { get; set; } public bool? Q43 { get; set; } public bool? R43 { get; set; } public bool? S43 { get; set; } public bool? T43 { get; set; } public bool? U43 { get; set; } public bool? V43 { get; set; } public bool? W43 { get; set; } public bool? X43 { get; set; } public bool? Y43 { get; set; } public bool? Z43 { get; set; } public bool? a43 { get; set; } public bool? b43 { get; set; } public bool? c43 { get; set; } public bool? d43 { get; set; } public bool? e43 { get; set; } public bool? f43 { get; set; } public bool? g43 { get; set; } public bool? h43 { get; set; } public bool? i43 { get; set; } public bool? j43 { get; set; } public bool? k43 { get; set; } public bool? l43 { get; set; } public bool? m43 { get; set; } public bool? n43 { get; set; } public bool? o43 { get; set; } public bool? p43 { get; set; } public bool? q43 { get; set; } public bool? r43 { get; set; } public bool? s43 { get; set; } public bool? t43 { get; set; } public bool? u43 { get; set; } public bool? v43 { get; set; } public bool? w43 { get; set; } public bool? x43 { get; set; } public bool? y43 { get; set; } public bool? z43 { get; set; } 
            public bool? A44 { get; set; } public bool? B44 { get; set; } public bool? C44 { get; set; } public bool? D44 { get; set; } public bool? E44 { get; set; } public bool? F44 { get; set; } public bool? G44 { get; set; } public bool? H44 { get; set; } public bool? I44 { get; set; } public bool? J44 { get; set; } public bool? K44 { get; set; } public bool? L44 { get; set; } public bool? M44 { get; set; } public bool? N44 { get; set; } public bool? O44 { get; set; } public bool? P44 { get; set; } public bool? Q44 { get; set; } public bool? R44 { get; set; } public bool? S44 { get; set; } public bool? T44 { get; set; } public bool? U44 { get; set; } public bool? V44 { get; set; } public bool? W44 { get; set; } public bool? X44 { get; set; } public bool? Y44 { get; set; } public bool? Z44 { get; set; } public bool? a44 { get; set; } public bool? b44 { get; set; } public bool? c44 { get; set; } public bool? d44 { get; set; } public bool? e44 { get; set; } public bool? f44 { get; set; } public bool? g44 { get; set; } public bool? h44 { get; set; } public bool? i44 { get; set; } public bool? j44 { get; set; } public bool? k44 { get; set; } public bool? l44 { get; set; } public bool? m44 { get; set; } public bool? n44 { get; set; } public bool? o44 { get; set; } public bool? p44 { get; set; } public bool? q44 { get; set; } public bool? r44 { get; set; } public bool? s44 { get; set; } public bool? t44 { get; set; } public bool? u44 { get; set; } public bool? v44 { get; set; } public bool? w44 { get; set; } public bool? x44 { get; set; } public bool? y44 { get; set; } public bool? z44 { get; set; } 
            public bool? A45 { get; set; } public bool? B45 { get; set; } public bool? C45 { get; set; } public bool? D45 { get; set; } public bool? E45 { get; set; } public bool? F45 { get; set; } public bool? G45 { get; set; } public bool? H45 { get; set; } public bool? I45 { get; set; } public bool? J45 { get; set; } public bool? K45 { get; set; } public bool? L45 { get; set; } public bool? M45 { get; set; } public bool? N45 { get; set; } public bool? O45 { get; set; } public bool? P45 { get; set; } public bool? Q45 { get; set; } public bool? R45 { get; set; } public bool? S45 { get; set; } public bool? T45 { get; set; } public bool? U45 { get; set; } public bool? V45 { get; set; } public bool? W45 { get; set; } public bool? X45 { get; set; } public bool? Y45 { get; set; } public bool? Z45 { get; set; } public bool? a45 { get; set; } public bool? b45 { get; set; } public bool? c45 { get; set; } public bool? d45 { get; set; } public bool? e45 { get; set; } public bool? f45 { get; set; } public bool? g45 { get; set; } public bool? h45 { get; set; } public bool? i45 { get; set; } public bool? j45 { get; set; } public bool? k45 { get; set; } public bool? l45 { get; set; } public bool? m45 { get; set; } public bool? n45 { get; set; } public bool? o45 { get; set; } public bool? p45 { get; set; } public bool? q45 { get; set; } public bool? r45 { get; set; } public bool? s45 { get; set; } public bool? t45 { get; set; } public bool? u45 { get; set; } public bool? v45 { get; set; } public bool? w45 { get; set; } public bool? x45 { get; set; } public bool? y45 { get; set; } public bool? z45 { get; set; } 
            public bool? A46 { get; set; } public bool? B46 { get; set; } public bool? C46 { get; set; } public bool? D46 { get; set; } public bool? E46 { get; set; } public bool? F46 { get; set; } public bool? G46 { get; set; } public bool? H46 { get; set; } public bool? I46 { get; set; } public bool? J46 { get; set; } public bool? K46 { get; set; } public bool? L46 { get; set; } public bool? M46 { get; set; } public bool? N46 { get; set; } public bool? O46 { get; set; } public bool? P46 { get; set; } public bool? Q46 { get; set; } public bool? R46 { get; set; } public bool? S46 { get; set; } public bool? T46 { get; set; } public bool? U46 { get; set; } public bool? V46 { get; set; } public bool? W46 { get; set; } public bool? X46 { get; set; } public bool? Y46 { get; set; } public bool? Z46 { get; set; } public bool? a46 { get; set; } public bool? b46 { get; set; } public bool? c46 { get; set; } public bool? d46 { get; set; } public bool? e46 { get; set; } public bool? f46 { get; set; } public bool? g46 { get; set; } public bool? h46 { get; set; } public bool? i46 { get; set; } public bool? j46 { get; set; } public bool? k46 { get; set; } public bool? l46 { get; set; } public bool? m46 { get; set; } public bool? n46 { get; set; } public bool? o46 { get; set; } public bool? p46 { get; set; } public bool? q46 { get; set; } public bool? r46 { get; set; } public bool? s46 { get; set; } public bool? t46 { get; set; } public bool? u46 { get; set; } public bool? v46 { get; set; } public bool? w46 { get; set; } public bool? x46 { get; set; } public bool? y46 { get; set; } public bool? z46 { get; set; } 
            public bool? A47 { get; set; } public bool? B47 { get; set; } public bool? C47 { get; set; } public bool? D47 { get; set; } public bool? E47 { get; set; } public bool? F47 { get; set; } public bool? G47 { get; set; } public bool? H47 { get; set; } public bool? I47 { get; set; } public bool? J47 { get; set; } public bool? K47 { get; set; } public bool? L47 { get; set; } public bool? M47 { get; set; } public bool? N47 { get; set; } public bool? O47 { get; set; } public bool? P47 { get; set; } public bool? Q47 { get; set; } public bool? R47 { get; set; } public bool? S47 { get; set; } public bool? T47 { get; set; } public bool? U47 { get; set; } public bool? V47 { get; set; } public bool? W47 { get; set; } public bool? X47 { get; set; } public bool? Y47 { get; set; } public bool? Z47 { get; set; } public bool? a47 { get; set; } public bool? b47 { get; set; } public bool? c47 { get; set; } public bool? d47 { get; set; } public bool? e47 { get; set; } public bool? f47 { get; set; } public bool? g47 { get; set; } public bool? h47 { get; set; } public bool? i47 { get; set; } public bool? j47 { get; set; } public bool? k47 { get; set; } public bool? l47 { get; set; } public bool? m47 { get; set; } public bool? n47 { get; set; } public bool? o47 { get; set; } public bool? p47 { get; set; } public bool? q47 { get; set; } public bool? r47 { get; set; } public bool? s47 { get; set; } public bool? t47 { get; set; } public bool? u47 { get; set; } public bool? v47 { get; set; } public bool? w47 { get; set; } public bool? x47 { get; set; } public bool? y47 { get; set; } public bool? z47 { get; set; } 
            public bool? A48 { get; set; } public bool? B48 { get; set; } public bool? C48 { get; set; } public bool? D48 { get; set; } public bool? E48 { get; set; } public bool? F48 { get; set; } public bool? G48 { get; set; } public bool? H48 { get; set; } public bool? I48 { get; set; } public bool? J48 { get; set; } public bool? K48 { get; set; } public bool? L48 { get; set; } public bool? M48 { get; set; } public bool? N48 { get; set; } public bool? O48 { get; set; } public bool? P48 { get; set; } public bool? Q48 { get; set; } public bool? R48 { get; set; } public bool? S48 { get; set; } public bool? T48 { get; set; } public bool? U48 { get; set; } public bool? V48 { get; set; } public bool? W48 { get; set; } public bool? X48 { get; set; } public bool? Y48 { get; set; } public bool? Z48 { get; set; } public bool? a48 { get; set; } public bool? b48 { get; set; } public bool? c48 { get; set; } public bool? d48 { get; set; } public bool? e48 { get; set; } public bool? f48 { get; set; } public bool? g48 { get; set; } public bool? h48 { get; set; } public bool? i48 { get; set; } public bool? j48 { get; set; } public bool? k48 { get; set; } public bool? l48 { get; set; } public bool? m48 { get; set; } public bool? n48 { get; set; } public bool? o48 { get; set; } public bool? p48 { get; set; } public bool? q48 { get; set; } public bool? r48 { get; set; } public bool? s48 { get; set; } public bool? t48 { get; set; } public bool? u48 { get; set; } public bool? v48 { get; set; } public bool? w48 { get; set; } public bool? x48 { get; set; } public bool? y48 { get; set; } public bool? z48 { get; set; } 
            public bool? A49 { get; set; } public bool? B49 { get; set; } public bool? C49 { get; set; } public bool? D49 { get; set; } public bool? E49 { get; set; } public bool? F49 { get; set; } public bool? G49 { get; set; } public bool? H49 { get; set; } public bool? I49 { get; set; } public bool? J49 { get; set; } public bool? K49 { get; set; } public bool? L49 { get; set; } public bool? M49 { get; set; } public bool? N49 { get; set; } public bool? O49 { get; set; } public bool? P49 { get; set; } public bool? Q49 { get; set; } public bool? R49 { get; set; } public bool? S49 { get; set; } public bool? T49 { get; set; } public bool? U49 { get; set; } public bool? V49 { get; set; } public bool? W49 { get; set; } public bool? X49 { get; set; } public bool? Y49 { get; set; } public bool? Z49 { get; set; } public bool? a49 { get; set; } public bool? b49 { get; set; } public bool? c49 { get; set; } public bool? d49 { get; set; } public bool? e49 { get; set; } public bool? f49 { get; set; } public bool? g49 { get; set; } public bool? h49 { get; set; } public bool? i49 { get; set; } public bool? j49 { get; set; } public bool? k49 { get; set; } public bool? l49 { get; set; } public bool? m49 { get; set; } public bool? n49 { get; set; } public bool? o49 { get; set; } public bool? p49 { get; set; } public bool? q49 { get; set; } public bool? r49 { get; set; } public bool? s49 { get; set; } public bool? t49 { get; set; } public bool? u49 { get; set; } public bool? v49 { get; set; } public bool? w49 { get; set; } public bool? x49 { get; set; } public bool? y49 { get; set; } public bool? z49 { get; set; } 
            public bool? A51 { get; set; } public bool? B51 { get; set; } public bool? C51 { get; set; } public bool? D51 { get; set; } public bool? E51 { get; set; } public bool? F51 { get; set; } public bool? G51 { get; set; } public bool? H51 { get; set; } public bool? I51 { get; set; } public bool? J51 { get; set; } public bool? K51 { get; set; } public bool? L51 { get; set; } public bool? M51 { get; set; } public bool? N51 { get; set; } public bool? O51 { get; set; } public bool? P51 { get; set; } public bool? Q51 { get; set; } public bool? R51 { get; set; } public bool? S51 { get; set; } public bool? T51 { get; set; } public bool? U51 { get; set; } public bool? V51 { get; set; } public bool? W51 { get; set; } public bool? X51 { get; set; } public bool? Y51 { get; set; } public bool? Z51 { get; set; } public bool? a51 { get; set; } public bool? b51 { get; set; } public bool? c51 { get; set; } public bool? d51 { get; set; } public bool? e51 { get; set; } public bool? f51 { get; set; } public bool? g51 { get; set; } public bool? h51 { get; set; } public bool? i51 { get; set; } public bool? j51 { get; set; } public bool? k51 { get; set; } public bool? l51 { get; set; } public bool? m51 { get; set; } public bool? n51 { get; set; } public bool? o51 { get; set; } public bool? p51 { get; set; } public bool? q51 { get; set; } public bool? r51 { get; set; } public bool? s51 { get; set; } public bool? t51 { get; set; } public bool? u51 { get; set; } public bool? v51 { get; set; } public bool? w51 { get; set; } public bool? x51 { get; set; } public bool? y51 { get; set; } public bool? z51 { get; set; } 
            public bool? A52 { get; set; } public bool? B52 { get; set; } public bool? C52 { get; set; } public bool? D52 { get; set; } public bool? E52 { get; set; } public bool? F52 { get; set; } public bool? G52 { get; set; } public bool? H52 { get; set; } public bool? I52 { get; set; } public bool? J52 { get; set; } public bool? K52 { get; set; } public bool? L52 { get; set; } public bool? M52 { get; set; } public bool? N52 { get; set; } public bool? O52 { get; set; } public bool? P52 { get; set; } public bool? Q52 { get; set; } public bool? R52 { get; set; } public bool? S52 { get; set; } public bool? T52 { get; set; } public bool? U52 { get; set; } public bool? V52 { get; set; } public bool? W52 { get; set; } public bool? X52 { get; set; } public bool? Y52 { get; set; } public bool? Z52 { get; set; } public bool? a52 { get; set; } public bool? b52 { get; set; } public bool? c52 { get; set; } public bool? d52 { get; set; } public bool? e52 { get; set; } public bool? f52 { get; set; } public bool? g52 { get; set; } public bool? h52 { get; set; } public bool? i52 { get; set; } public bool? j52 { get; set; } public bool? k52 { get; set; } public bool? l52 { get; set; } public bool? m52 { get; set; } public bool? n52 { get; set; } public bool? o52 { get; set; } public bool? p52 { get; set; } public bool? q52 { get; set; } public bool? r52 { get; set; } public bool? s52 { get; set; } public bool? t52 { get; set; } public bool? u52 { get; set; } public bool? v52 { get; set; } public bool? w52 { get; set; } public bool? x52 { get; set; } public bool? y52 { get; set; } public bool? z52 { get; set; } 
            public bool? A53 { get; set; } public bool? B53 { get; set; } public bool? C53 { get; set; } public bool? D53 { get; set; } public bool? E53 { get; set; } public bool? F53 { get; set; } public bool? G53 { get; set; } public bool? H53 { get; set; } public bool? I53 { get; set; } public bool? J53 { get; set; } public bool? K53 { get; set; } public bool? L53 { get; set; } public bool? M53 { get; set; } public bool? N53 { get; set; } public bool? O53 { get; set; } public bool? P53 { get; set; } public bool? Q53 { get; set; } public bool? R53 { get; set; } public bool? S53 { get; set; } public bool? T53 { get; set; } public bool? U53 { get; set; } public bool? V53 { get; set; } public bool? W53 { get; set; } public bool? X53 { get; set; } public bool? Y53 { get; set; } public bool? Z53 { get; set; } public bool? a53 { get; set; } public bool? b53 { get; set; } public bool? c53 { get; set; } public bool? d53 { get; set; } public bool? e53 { get; set; } public bool? f53 { get; set; } public bool? g53 { get; set; } public bool? h53 { get; set; } public bool? i53 { get; set; } public bool? j53 { get; set; } public bool? k53 { get; set; } public bool? l53 { get; set; } public bool? m53 { get; set; } public bool? n53 { get; set; } public bool? o53 { get; set; } public bool? p53 { get; set; } public bool? q53 { get; set; } public bool? r53 { get; set; } public bool? s53 { get; set; } public bool? t53 { get; set; } public bool? u53 { get; set; } public bool? v53 { get; set; } public bool? w53 { get; set; } public bool? x53 { get; set; } public bool? y53 { get; set; } public bool? z53 { get; set; } 
            public bool? A54 { get; set; } public bool? B54 { get; set; } public bool? C54 { get; set; } public bool? D54 { get; set; } public bool? E54 { get; set; } public bool? F54 { get; set; } public bool? G54 { get; set; } public bool? H54 { get; set; } public bool? I54 { get; set; } public bool? J54 { get; set; } public bool? K54 { get; set; } public bool? L54 { get; set; } public bool? M54 { get; set; } public bool? N54 { get; set; } public bool? O54 { get; set; } public bool? P54 { get; set; } public bool? Q54 { get; set; } public bool? R54 { get; set; } public bool? S54 { get; set; } public bool? T54 { get; set; } public bool? U54 { get; set; } public bool? V54 { get; set; } public bool? W54 { get; set; } public bool? X54 { get; set; } public bool? Y54 { get; set; } public bool? Z54 { get; set; } public bool? a54 { get; set; } public bool? b54 { get; set; } public bool? c54 { get; set; } public bool? d54 { get; set; } public bool? e54 { get; set; } public bool? f54 { get; set; } public bool? g54 { get; set; } public bool? h54 { get; set; } public bool? i54 { get; set; } public bool? j54 { get; set; } public bool? k54 { get; set; } public bool? l54 { get; set; } public bool? m54 { get; set; } public bool? n54 { get; set; } public bool? o54 { get; set; } public bool? p54 { get; set; } public bool? q54 { get; set; } public bool? r54 { get; set; } public bool? s54 { get; set; } public bool? t54 { get; set; } public bool? u54 { get; set; } public bool? v54 { get; set; } public bool? w54 { get; set; } public bool? x54 { get; set; } public bool? y54 { get; set; } public bool? z54 { get; set; } 
            public bool? A55 { get; set; } public bool? B55 { get; set; } public bool? C55 { get; set; } public bool? D55 { get; set; } public bool? E55 { get; set; } public bool? F55 { get; set; } public bool? G55 { get; set; } public bool? H55 { get; set; } public bool? I55 { get; set; } public bool? J55 { get; set; } public bool? K55 { get; set; } public bool? L55 { get; set; } public bool? M55 { get; set; } public bool? N55 { get; set; } public bool? O55 { get; set; } public bool? P55 { get; set; } public bool? Q55 { get; set; } public bool? R55 { get; set; } public bool? S55 { get; set; } public bool? T55 { get; set; } public bool? U55 { get; set; } public bool? V55 { get; set; } public bool? W55 { get; set; } public bool? X55 { get; set; } public bool? Y55 { get; set; } public bool? Z55 { get; set; } public bool? a55 { get; set; } public bool? b55 { get; set; } public bool? c55 { get; set; } public bool? d55 { get; set; } public bool? e55 { get; set; } public bool? f55 { get; set; } public bool? g55 { get; set; } public bool? h55 { get; set; } public bool? i55 { get; set; } public bool? j55 { get; set; } public bool? k55 { get; set; } public bool? l55 { get; set; } public bool? m55 { get; set; } public bool? n55 { get; set; } public bool? o55 { get; set; } public bool? p55 { get; set; } public bool? q55 { get; set; } public bool? r55 { get; set; } public bool? s55 { get; set; } public bool? t55 { get; set; } public bool? u55 { get; set; } public bool? v55 { get; set; } public bool? w55 { get; set; } public bool? x55 { get; set; } public bool? y55 { get; set; } public bool? z55 { get; set; } 
            public bool? A56 { get; set; } public bool? B56 { get; set; } public bool? C56 { get; set; } public bool? D56 { get; set; } public bool? E56 { get; set; } public bool? F56 { get; set; } public bool? G56 { get; set; } public bool? H56 { get; set; } public bool? I56 { get; set; } public bool? J56 { get; set; } public bool? K56 { get; set; } public bool? L56 { get; set; } public bool? M56 { get; set; } public bool? N56 { get; set; } public bool? O56 { get; set; } public bool? P56 { get; set; } public bool? Q56 { get; set; } public bool? R56 { get; set; } public bool? S56 { get; set; } public bool? T56 { get; set; } public bool? U56 { get; set; } public bool? V56 { get; set; } public bool? W56 { get; set; } public bool? X56 { get; set; } public bool? Y56 { get; set; } public bool? Z56 { get; set; } public bool? a56 { get; set; } public bool? b56 { get; set; } public bool? c56 { get; set; } public bool? d56 { get; set; } public bool? e56 { get; set; } public bool? f56 { get; set; } public bool? g56 { get; set; } public bool? h56 { get; set; } public bool? i56 { get; set; } public bool? j56 { get; set; } public bool? k56 { get; set; } public bool? l56 { get; set; } public bool? m56 { get; set; } public bool? n56 { get; set; } public bool? o56 { get; set; } public bool? p56 { get; set; } public bool? q56 { get; set; } public bool? r56 { get; set; } public bool? s56 { get; set; } public bool? t56 { get; set; } public bool? u56 { get; set; } public bool? v56 { get; set; } public bool? w56 { get; set; } public bool? x56 { get; set; } public bool? y56 { get; set; } public bool? z56 { get; set; } 
            public bool? A57 { get; set; } public bool? B57 { get; set; } public bool? C57 { get; set; } public bool? D57 { get; set; } public bool? E57 { get; set; } public bool? F57 { get; set; } public bool? G57 { get; set; } public bool? H57 { get; set; } public bool? I57 { get; set; } public bool? J57 { get; set; } public bool? K57 { get; set; } public bool? L57 { get; set; } public bool? M57 { get; set; } public bool? N57 { get; set; } public bool? O57 { get; set; } public bool? P57 { get; set; } public bool? Q57 { get; set; } public bool? R57 { get; set; } public bool? S57 { get; set; } public bool? T57 { get; set; } public bool? U57 { get; set; } public bool? V57 { get; set; } public bool? W57 { get; set; } public bool? X57 { get; set; } public bool? Y57 { get; set; } public bool? Z57 { get; set; } public bool? a57 { get; set; } public bool? b57 { get; set; } public bool? c57 { get; set; } public bool? d57 { get; set; } public bool? e57 { get; set; } public bool? f57 { get; set; } public bool? g57 { get; set; } public bool? h57 { get; set; } public bool? i57 { get; set; } public bool? j57 { get; set; } public bool? k57 { get; set; } public bool? l57 { get; set; } public bool? m57 { get; set; } public bool? n57 { get; set; } public bool? o57 { get; set; } public bool? p57 { get; set; } public bool? q57 { get; set; } public bool? r57 { get; set; } public bool? s57 { get; set; } public bool? t57 { get; set; } public bool? u57 { get; set; } public bool? v57 { get; set; } public bool? w57 { get; set; } public bool? x57 { get; set; } public bool? y57 { get; set; } public bool? z57 { get; set; } 
            public bool? A58 { get; set; } public bool? B58 { get; set; } public bool? C58 { get; set; } public bool? D58 { get; set; } public bool? E58 { get; set; } public bool? F58 { get; set; } public bool? G58 { get; set; } public bool? H58 { get; set; } public bool? I58 { get; set; } public bool? J58 { get; set; } public bool? K58 { get; set; } public bool? L58 { get; set; } public bool? M58 { get; set; } public bool? N58 { get; set; } public bool? O58 { get; set; } public bool? P58 { get; set; } public bool? Q58 { get; set; } public bool? R58 { get; set; } public bool? S58 { get; set; } public bool? T58 { get; set; } public bool? U58 { get; set; } public bool? V58 { get; set; } public bool? W58 { get; set; } public bool? X58 { get; set; } public bool? Y58 { get; set; } public bool? Z58 { get; set; } public bool? a58 { get; set; } public bool? b58 { get; set; } public bool? c58 { get; set; } public bool? d58 { get; set; } public bool? e58 { get; set; } public bool? f58 { get; set; } public bool? g58 { get; set; } public bool? h58 { get; set; } public bool? i58 { get; set; } public bool? j58 { get; set; } public bool? k58 { get; set; } public bool? l58 { get; set; } public bool? m58 { get; set; } public bool? n58 { get; set; } public bool? o58 { get; set; } public bool? p58 { get; set; } public bool? q58 { get; set; } public bool? r58 { get; set; } public bool? s58 { get; set; } public bool? t58 { get; set; } public bool? u58 { get; set; } public bool? v58 { get; set; } public bool? w58 { get; set; } public bool? x58 { get; set; } public bool? y58 { get; set; } public bool? z58 { get; set; } 
            public bool? A59 { get; set; } public bool? B59 { get; set; } public bool? C59 { get; set; } public bool? D59 { get; set; } public bool? E59 { get; set; } public bool? F59 { get; set; } public bool? G59 { get; set; } public bool? H59 { get; set; } public bool? I59 { get; set; } public bool? J59 { get; set; } public bool? K59 { get; set; } public bool? L59 { get; set; } public bool? M59 { get; set; } public bool? N59 { get; set; } public bool? O59 { get; set; } public bool? P59 { get; set; } public bool? Q59 { get; set; } public bool? R59 { get; set; } public bool? S59 { get; set; } public bool? T59 { get; set; } public bool? U59 { get; set; } public bool? V59 { get; set; } public bool? W59 { get; set; } public bool? X59 { get; set; } public bool? Y59 { get; set; } public bool? Z59 { get; set; } public bool? a59 { get; set; } public bool? b59 { get; set; } public bool? c59 { get; set; } public bool? d59 { get; set; } public bool? e59 { get; set; } public bool? f59 { get; set; } public bool? g59 { get; set; } public bool? h59 { get; set; } public bool? i59 { get; set; } public bool? j59 { get; set; } public bool? k59 { get; set; } public bool? l59 { get; set; } public bool? m59 { get; set; } public bool? n59 { get; set; } public bool? o59 { get; set; } public bool? p59 { get; set; } public bool? q59 { get; set; } public bool? r59 { get; set; } public bool? s59 { get; set; } public bool? t59 { get; set; } public bool? u59 { get; set; } public bool? v59 { get; set; } public bool? w59 { get; set; } public bool? x59 { get; set; } public bool? y59 { get; set; } public bool? z59 { get; set; } 
            public bool? A61 { get; set; } public bool? B61 { get; set; } public bool? C61 { get; set; } public bool? D61 { get; set; } public bool? E61 { get; set; } public bool? F61 { get; set; } public bool? G61 { get; set; } public bool? H61 { get; set; } public bool? I61 { get; set; } public bool? J61 { get; set; } public bool? K61 { get; set; } public bool? L61 { get; set; } public bool? M61 { get; set; } public bool? N61 { get; set; } public bool? O61 { get; set; } public bool? P61 { get; set; } public bool? Q61 { get; set; } public bool? R61 { get; set; } public bool? S61 { get; set; } public bool? T61 { get; set; } public bool? U61 { get; set; } public bool? V61 { get; set; } public bool? W61 { get; set; } public bool? X61 { get; set; } public bool? Y61 { get; set; } public bool? Z61 { get; set; } public bool? a61 { get; set; } public bool? b61 { get; set; } public bool? c61 { get; set; } public bool? d61 { get; set; } public bool? e61 { get; set; } public bool? f61 { get; set; } public bool? g61 { get; set; } public bool? h61 { get; set; } public bool? i61 { get; set; } public bool? j61 { get; set; } public bool? k61 { get; set; } public bool? l61 { get; set; } public bool? m61 { get; set; } public bool? n61 { get; set; } public bool? o61 { get; set; } public bool? p61 { get; set; } public bool? q61 { get; set; } public bool? r61 { get; set; } public bool? s61 { get; set; } public bool? t61 { get; set; } public bool? u61 { get; set; } public bool? v61 { get; set; } public bool? w61 { get; set; } public bool? x61 { get; set; } public bool? y61 { get; set; } public bool? z61 { get; set; } 
            public bool? A62 { get; set; } public bool? B62 { get; set; } public bool? C62 { get; set; } public bool? D62 { get; set; } public bool? E62 { get; set; } public bool? F62 { get; set; } public bool? G62 { get; set; } public bool? H62 { get; set; } public bool? I62 { get; set; } public bool? J62 { get; set; } public bool? K62 { get; set; } public bool? L62 { get; set; } public bool? M62 { get; set; } public bool? N62 { get; set; } public bool? O62 { get; set; } public bool? P62 { get; set; } public bool? Q62 { get; set; } public bool? R62 { get; set; } public bool? S62 { get; set; } public bool? T62 { get; set; } public bool? U62 { get; set; } public bool? V62 { get; set; } public bool? W62 { get; set; } public bool? X62 { get; set; } public bool? Y62 { get; set; } public bool? Z62 { get; set; } public bool? a62 { get; set; } public bool? b62 { get; set; } public bool? c62 { get; set; } public bool? d62 { get; set; } public bool? e62 { get; set; } public bool? f62 { get; set; } public bool? g62 { get; set; } public bool? h62 { get; set; } public bool? i62 { get; set; } public bool? j62 { get; set; } public bool? k62 { get; set; } public bool? l62 { get; set; } public bool? m62 { get; set; } public bool? n62 { get; set; } public bool? o62 { get; set; } public bool? p62 { get; set; } public bool? q62 { get; set; } public bool? r62 { get; set; } public bool? s62 { get; set; } public bool? t62 { get; set; } public bool? u62 { get; set; } public bool? v62 { get; set; } public bool? w62 { get; set; } public bool? x62 { get; set; } public bool? y62 { get; set; } public bool? z62 { get; set; } 
            public bool? A63 { get; set; } public bool? B63 { get; set; } public bool? C63 { get; set; } public bool? D63 { get; set; } public bool? E63 { get; set; } public bool? F63 { get; set; } public bool? G63 { get; set; } public bool? H63 { get; set; } public bool? I63 { get; set; } public bool? J63 { get; set; } public bool? K63 { get; set; } public bool? L63 { get; set; } public bool? M63 { get; set; } public bool? N63 { get; set; } public bool? O63 { get; set; } public bool? P63 { get; set; } public bool? Q63 { get; set; } public bool? R63 { get; set; } public bool? S63 { get; set; } public bool? T63 { get; set; } public bool? U63 { get; set; } public bool? V63 { get; set; } public bool? W63 { get; set; } public bool? X63 { get; set; } public bool? Y63 { get; set; } public bool? Z63 { get; set; } public bool? a63 { get; set; } public bool? b63 { get; set; } public bool? c63 { get; set; } public bool? d63 { get; set; } public bool? e63 { get; set; } public bool? f63 { get; set; } public bool? g63 { get; set; } public bool? h63 { get; set; } public bool? i63 { get; set; } public bool? j63 { get; set; } public bool? k63 { get; set; } public bool? l63 { get; set; } public bool? m63 { get; set; } public bool? n63 { get; set; } public bool? o63 { get; set; } public bool? p63 { get; set; } public bool? q63 { get; set; } public bool? r63 { get; set; } public bool? s63 { get; set; } public bool? t63 { get; set; } public bool? u63 { get; set; } public bool? v63 { get; set; } public bool? w63 { get; set; } public bool? x63 { get; set; } public bool? y63 { get; set; } public bool? z63 { get; set; } 
            public bool? A64 { get; set; } public bool? B64 { get; set; } public bool? C64 { get; set; } public bool? D64 { get; set; } public bool? E64 { get; set; } public bool? F64 { get; set; } public bool? G64 { get; set; } public bool? H64 { get; set; } public bool? I64 { get; set; } public bool? J64 { get; set; } public bool? K64 { get; set; } public bool? L64 { get; set; } public bool? M64 { get; set; } public bool? N64 { get; set; } public bool? O64 { get; set; } public bool? P64 { get; set; } public bool? Q64 { get; set; } public bool? R64 { get; set; } public bool? S64 { get; set; } public bool? T64 { get; set; } public bool? U64 { get; set; } public bool? V64 { get; set; } public bool? W64 { get; set; } public bool? X64 { get; set; } public bool? Y64 { get; set; } public bool? Z64 { get; set; } public bool? a64 { get; set; } public bool? b64 { get; set; } public bool? c64 { get; set; } public bool? d64 { get; set; } public bool? e64 { get; set; } public bool? f64 { get; set; } public bool? g64 { get; set; } public bool? h64 { get; set; } public bool? i64 { get; set; } public bool? j64 { get; set; } public bool? k64 { get; set; } public bool? l64 { get; set; } public bool? m64 { get; set; } public bool? n64 { get; set; } public bool? o64 { get; set; } public bool? p64 { get; set; } public bool? q64 { get; set; } public bool? r64 { get; set; } public bool? s64 { get; set; } public bool? t64 { get; set; } public bool? u64 { get; set; } public bool? v64 { get; set; } public bool? w64 { get; set; } public bool? x64 { get; set; } public bool? y64 { get; set; } public bool? z64 { get; set; } 
            public bool? A65 { get; set; } public bool? B65 { get; set; } public bool? C65 { get; set; } public bool? D65 { get; set; } public bool? E65 { get; set; } public bool? F65 { get; set; } public bool? G65 { get; set; } public bool? H65 { get; set; } public bool? I65 { get; set; } public bool? J65 { get; set; } public bool? K65 { get; set; } public bool? L65 { get; set; } public bool? M65 { get; set; } public bool? N65 { get; set; } public bool? O65 { get; set; } public bool? P65 { get; set; } public bool? Q65 { get; set; } public bool? R65 { get; set; } public bool? S65 { get; set; } public bool? T65 { get; set; } public bool? U65 { get; set; } public bool? V65 { get; set; } public bool? W65 { get; set; } public bool? X65 { get; set; } public bool? Y65 { get; set; } public bool? Z65 { get; set; } public bool? a65 { get; set; } public bool? b65 { get; set; } public bool? c65 { get; set; } public bool? d65 { get; set; } public bool? e65 { get; set; } public bool? f65 { get; set; } public bool? g65 { get; set; } public bool? h65 { get; set; } public bool? i65 { get; set; } public bool? j65 { get; set; } public bool? k65 { get; set; } public bool? l65 { get; set; } public bool? m65 { get; set; } public bool? n65 { get; set; } public bool? o65 { get; set; } public bool? p65 { get; set; } public bool? q65 { get; set; } public bool? r65 { get; set; } public bool? s65 { get; set; } public bool? t65 { get; set; } public bool? u65 { get; set; } public bool? v65 { get; set; } public bool? w65 { get; set; } public bool? x65 { get; set; } public bool? y65 { get; set; } public bool? z65 { get; set; } 
            public bool? A66 { get; set; } public bool? B66 { get; set; } public bool? C66 { get; set; } public bool? D66 { get; set; } public bool? E66 { get; set; } public bool? F66 { get; set; } public bool? G66 { get; set; } public bool? H66 { get; set; } public bool? I66 { get; set; } public bool? J66 { get; set; } public bool? K66 { get; set; } public bool? L66 { get; set; } public bool? M66 { get; set; } public bool? N66 { get; set; } public bool? O66 { get; set; } public bool? P66 { get; set; } public bool? Q66 { get; set; } public bool? R66 { get; set; } public bool? S66 { get; set; } public bool? T66 { get; set; } public bool? U66 { get; set; } public bool? V66 { get; set; } public bool? W66 { get; set; } public bool? X66 { get; set; } public bool? Y66 { get; set; } public bool? Z66 { get; set; } public bool? a66 { get; set; } public bool? b66 { get; set; } public bool? c66 { get; set; } public bool? d66 { get; set; } public bool? e66 { get; set; } public bool? f66 { get; set; } public bool? g66 { get; set; } public bool? h66 { get; set; } public bool? i66 { get; set; } public bool? j66 { get; set; } public bool? k66 { get; set; } public bool? l66 { get; set; } public bool? m66 { get; set; } public bool? n66 { get; set; } public bool? o66 { get; set; } public bool? p66 { get; set; } public bool? q66 { get; set; } public bool? r66 { get; set; } public bool? s66 { get; set; } public bool? t66 { get; set; } public bool? u66 { get; set; } public bool? v66 { get; set; } public bool? w66 { get; set; } public bool? x66 { get; set; } public bool? y66 { get; set; } public bool? z66 { get; set; } 
            public bool? A67 { get; set; } public bool? B67 { get; set; } public bool? C67 { get; set; } public bool? D67 { get; set; } public bool? E67 { get; set; } public bool? F67 { get; set; } public bool? G67 { get; set; } public bool? H67 { get; set; } public bool? I67 { get; set; } public bool? J67 { get; set; } public bool? K67 { get; set; } public bool? L67 { get; set; } public bool? M67 { get; set; } public bool? N67 { get; set; } public bool? O67 { get; set; } public bool? P67 { get; set; } public bool? Q67 { get; set; } public bool? R67 { get; set; } public bool? S67 { get; set; } public bool? T67 { get; set; } public bool? U67 { get; set; } public bool? V67 { get; set; } public bool? W67 { get; set; } public bool? X67 { get; set; } public bool? Y67 { get; set; } public bool? Z67 { get; set; } public bool? a67 { get; set; } public bool? b67 { get; set; } public bool? c67 { get; set; } public bool? d67 { get; set; } public bool? e67 { get; set; } public bool? f67 { get; set; } public bool? g67 { get; set; } public bool? h67 { get; set; } public bool? i67 { get; set; } public bool? j67 { get; set; } public bool? k67 { get; set; } public bool? l67 { get; set; } public bool? m67 { get; set; } public bool? n67 { get; set; } public bool? o67 { get; set; } public bool? p67 { get; set; } public bool? q67 { get; set; } public bool? r67 { get; set; } public bool? s67 { get; set; } public bool? t67 { get; set; } public bool? u67 { get; set; } public bool? v67 { get; set; } public bool? w67 { get; set; } public bool? x67 { get; set; } public bool? y67 { get; set; } public bool? z67 { get; set; } 
            public bool? A68 { get; set; } public bool? B68 { get; set; } public bool? C68 { get; set; } public bool? D68 { get; set; } public bool? E68 { get; set; } public bool? F68 { get; set; } public bool? G68 { get; set; } public bool? H68 { get; set; } public bool? I68 { get; set; } public bool? J68 { get; set; } public bool? K68 { get; set; } public bool? L68 { get; set; } public bool? M68 { get; set; } public bool? N68 { get; set; } public bool? O68 { get; set; } public bool? P68 { get; set; } public bool? Q68 { get; set; } public bool? R68 { get; set; } public bool? S68 { get; set; } public bool? T68 { get; set; } public bool? U68 { get; set; } public bool? V68 { get; set; } public bool? W68 { get; set; } public bool? X68 { get; set; } public bool? Y68 { get; set; } public bool? Z68 { get; set; } public bool? a68 { get; set; } public bool? b68 { get; set; } public bool? c68 { get; set; } public bool? d68 { get; set; } public bool? e68 { get; set; } public bool? f68 { get; set; } public bool? g68 { get; set; } public bool? h68 { get; set; } public bool? i68 { get; set; } public bool? j68 { get; set; } public bool? k68 { get; set; } public bool? l68 { get; set; } public bool? m68 { get; set; } public bool? n68 { get; set; } public bool? o68 { get; set; } public bool? p68 { get; set; } public bool? q68 { get; set; } public bool? r68 { get; set; } public bool? s68 { get; set; } public bool? t68 { get; set; } public bool? u68 { get; set; } public bool? v68 { get; set; } public bool? w68 { get; set; } public bool? x68 { get; set; } public bool? y68 { get; set; } public bool? z68 { get; set; } 
            public bool? A69 { get; set; } public bool? B69 { get; set; } public bool? C69 { get; set; } public bool? D69 { get; set; } public bool? E69 { get; set; } public bool? F69 { get; set; } public bool? G69 { get; set; } public bool? H69 { get; set; } public bool? I69 { get; set; } public bool? J69 { get; set; } public bool? K69 { get; set; } public bool? L69 { get; set; } public bool? M69 { get; set; } public bool? N69 { get; set; } public bool? O69 { get; set; } public bool? P69 { get; set; } public bool? Q69 { get; set; } public bool? R69 { get; set; } public bool? S69 { get; set; } public bool? T69 { get; set; } public bool? U69 { get; set; } public bool? V69 { get; set; } public bool? W69 { get; set; } public bool? X69 { get; set; } public bool? Y69 { get; set; } public bool? Z69 { get; set; } public bool? a69 { get; set; } public bool? b69 { get; set; } public bool? c69 { get; set; } public bool? d69 { get; set; } public bool? e69 { get; set; } public bool? f69 { get; set; } public bool? g69 { get; set; } public bool? h69 { get; set; } public bool? i69 { get; set; } public bool? j69 { get; set; } public bool? k69 { get; set; } public bool? l69 { get; set; } public bool? m69 { get; set; } public bool? n69 { get; set; } public bool? o69 { get; set; } public bool? p69 { get; set; } public bool? q69 { get; set; } public bool? r69 { get; set; } public bool? s69 { get; set; } public bool? t69 { get; set; } public bool? u69 { get; set; } public bool? v69 { get; set; } public bool? w69 { get; set; } public bool? x69 { get; set; } public bool? y69 { get; set; } public bool? z69 { get; set; } 
            public bool? A71 { get; set; } public bool? B71 { get; set; } public bool? C71 { get; set; } public bool? D71 { get; set; } public bool? E71 { get; set; } public bool? F71 { get; set; } public bool? G71 { get; set; } public bool? H71 { get; set; } public bool? I71 { get; set; } public bool? J71 { get; set; } public bool? K71 { get; set; } public bool? L71 { get; set; } public bool? M71 { get; set; } public bool? N71 { get; set; } public bool? O71 { get; set; } public bool? P71 { get; set; } public bool? Q71 { get; set; } public bool? R71 { get; set; } public bool? S71 { get; set; } public bool? T71 { get; set; } public bool? U71 { get; set; } public bool? V71 { get; set; } public bool? W71 { get; set; } public bool? X71 { get; set; } public bool? Y71 { get; set; } public bool? Z71 { get; set; } public bool? a71 { get; set; } public bool? b71 { get; set; } public bool? c71 { get; set; } public bool? d71 { get; set; } public bool? e71 { get; set; } public bool? f71 { get; set; } public bool? g71 { get; set; } public bool? h71 { get; set; } public bool? i71 { get; set; } public bool? j71 { get; set; } public bool? k71 { get; set; } public bool? l71 { get; set; } public bool? m71 { get; set; } public bool? n71 { get; set; } public bool? o71 { get; set; } public bool? p71 { get; set; } public bool? q71 { get; set; } public bool? r71 { get; set; } public bool? s71 { get; set; } public bool? t71 { get; set; } public bool? u71 { get; set; } public bool? v71 { get; set; } public bool? w71 { get; set; } public bool? x71 { get; set; } public bool? y71 { get; set; } public bool? z71 { get; set; } 
            public bool? A72 { get; set; } public bool? B72 { get; set; } public bool? C72 { get; set; } public bool? D72 { get; set; } public bool? E72 { get; set; } public bool? F72 { get; set; } public bool? G72 { get; set; } public bool? H72 { get; set; } public bool? I72 { get; set; } public bool? J72 { get; set; } public bool? K72 { get; set; } public bool? L72 { get; set; } public bool? M72 { get; set; } public bool? N72 { get; set; } public bool? O72 { get; set; } public bool? P72 { get; set; } public bool? Q72 { get; set; } public bool? R72 { get; set; } public bool? S72 { get; set; } public bool? T72 { get; set; } public bool? U72 { get; set; } public bool? V72 { get; set; } public bool? W72 { get; set; } public bool? X72 { get; set; } public bool? Y72 { get; set; } public bool? Z72 { get; set; } public bool? a72 { get; set; } public bool? b72 { get; set; } public bool? c72 { get; set; } public bool? d72 { get; set; } public bool? e72 { get; set; } public bool? f72 { get; set; } public bool? g72 { get; set; } public bool? h72 { get; set; } public bool? i72 { get; set; } public bool? j72 { get; set; } public bool? k72 { get; set; } public bool? l72 { get; set; } public bool? m72 { get; set; } public bool? n72 { get; set; } public bool? o72 { get; set; } public bool? p72 { get; set; } public bool? q72 { get; set; } public bool? r72 { get; set; } public bool? s72 { get; set; } public bool? t72 { get; set; } public bool? u72 { get; set; } public bool? v72 { get; set; } public bool? w72 { get; set; } public bool? x72 { get; set; } public bool? y72 { get; set; } public bool? z72 { get; set; } 
            public bool? A73 { get; set; } public bool? B73 { get; set; } public bool? C73 { get; set; } public bool? D73 { get; set; } public bool? E73 { get; set; } public bool? F73 { get; set; } public bool? G73 { get; set; } public bool? H73 { get; set; } public bool? I73 { get; set; } public bool? J73 { get; set; } public bool? K73 { get; set; } public bool? L73 { get; set; } public bool? M73 { get; set; } public bool? N73 { get; set; } public bool? O73 { get; set; } public bool? P73 { get; set; } public bool? Q73 { get; set; } public bool? R73 { get; set; } public bool? S73 { get; set; } public bool? T73 { get; set; } public bool? U73 { get; set; } public bool? V73 { get; set; } public bool? W73 { get; set; } public bool? X73 { get; set; } public bool? Y73 { get; set; } public bool? Z73 { get; set; } public bool? a73 { get; set; } public bool? b73 { get; set; } public bool? c73 { get; set; } public bool? d73 { get; set; } public bool? e73 { get; set; } public bool? f73 { get; set; } public bool? g73 { get; set; } public bool? h73 { get; set; } public bool? i73 { get; set; } public bool? j73 { get; set; } public bool? k73 { get; set; } public bool? l73 { get; set; } public bool? m73 { get; set; } public bool? n73 { get; set; } public bool? o73 { get; set; } public bool? p73 { get; set; } public bool? q73 { get; set; } public bool? r73 { get; set; } public bool? s73 { get; set; } public bool? t73 { get; set; } public bool? u73 { get; set; } public bool? v73 { get; set; } public bool? w73 { get; set; } public bool? x73 { get; set; } public bool? y73 { get; set; } public bool? z73 { get; set; } 
            public bool? A74 { get; set; } public bool? B74 { get; set; } public bool? C74 { get; set; } public bool? D74 { get; set; } public bool? E74 { get; set; } public bool? F74 { get; set; } public bool? G74 { get; set; } public bool? H74 { get; set; } public bool? I74 { get; set; } public bool? J74 { get; set; } public bool? K74 { get; set; } public bool? L74 { get; set; } public bool? M74 { get; set; } public bool? N74 { get; set; } public bool? O74 { get; set; } public bool? P74 { get; set; } public bool? Q74 { get; set; } public bool? R74 { get; set; } public bool? S74 { get; set; } public bool? T74 { get; set; } public bool? U74 { get; set; } public bool? V74 { get; set; } public bool? W74 { get; set; } public bool? X74 { get; set; } public bool? Y74 { get; set; } public bool? Z74 { get; set; } public bool? a74 { get; set; } public bool? b74 { get; set; } public bool? c74 { get; set; } public bool? d74 { get; set; } public bool? e74 { get; set; } public bool? f74 { get; set; } public bool? g74 { get; set; } public bool? h74 { get; set; } public bool? i74 { get; set; } public bool? j74 { get; set; } public bool? k74 { get; set; } public bool? l74 { get; set; } public bool? m74 { get; set; } public bool? n74 { get; set; } public bool? o74 { get; set; } public bool? p74 { get; set; } public bool? q74 { get; set; } public bool? r74 { get; set; } public bool? s74 { get; set; } public bool? t74 { get; set; } public bool? u74 { get; set; } public bool? v74 { get; set; } public bool? w74 { get; set; } public bool? x74 { get; set; } public bool? y74 { get; set; } public bool? z74 { get; set; } 
            public bool? A75 { get; set; } public bool? B75 { get; set; } public bool? C75 { get; set; } public bool? D75 { get; set; } public bool? E75 { get; set; } public bool? F75 { get; set; } public bool? G75 { get; set; } public bool? H75 { get; set; } public bool? I75 { get; set; } public bool? J75 { get; set; } public bool? K75 { get; set; } public bool? L75 { get; set; } public bool? M75 { get; set; } public bool? N75 { get; set; } public bool? O75 { get; set; } public bool? P75 { get; set; } public bool? Q75 { get; set; } public bool? R75 { get; set; } public bool? S75 { get; set; } public bool? T75 { get; set; } public bool? U75 { get; set; } public bool? V75 { get; set; } public bool? W75 { get; set; } public bool? X75 { get; set; } public bool? Y75 { get; set; } public bool? Z75 { get; set; } public bool? a75 { get; set; } public bool? b75 { get; set; } public bool? c75 { get; set; } public bool? d75 { get; set; } public bool? e75 { get; set; } public bool? f75 { get; set; } public bool? g75 { get; set; } public bool? h75 { get; set; } public bool? i75 { get; set; } public bool? j75 { get; set; } public bool? k75 { get; set; } public bool? l75 { get; set; } public bool? m75 { get; set; } public bool? n75 { get; set; } public bool? o75 { get; set; } public bool? p75 { get; set; } public bool? q75 { get; set; } public bool? r75 { get; set; } public bool? s75 { get; set; } public bool? t75 { get; set; } public bool? u75 { get; set; } public bool? v75 { get; set; } public bool? w75 { get; set; } public bool? x75 { get; set; } public bool? y75 { get; set; } public bool? z75 { get; set; } 
            public bool? A76 { get; set; } public bool? B76 { get; set; } public bool? C76 { get; set; } public bool? D76 { get; set; } public bool? E76 { get; set; } public bool? F76 { get; set; } public bool? G76 { get; set; } public bool? H76 { get; set; } public bool? I76 { get; set; } public bool? J76 { get; set; } public bool? K76 { get; set; } public bool? L76 { get; set; } public bool? M76 { get; set; } public bool? N76 { get; set; } public bool? O76 { get; set; } public bool? P76 { get; set; } public bool? Q76 { get; set; } public bool? R76 { get; set; } public bool? S76 { get; set; } public bool? T76 { get; set; } public bool? U76 { get; set; } public bool? V76 { get; set; } public bool? W76 { get; set; } public bool? X76 { get; set; } public bool? Y76 { get; set; } public bool? Z76 { get; set; } public bool? a76 { get; set; } public bool? b76 { get; set; } public bool? c76 { get; set; } public bool? d76 { get; set; } public bool? e76 { get; set; } public bool? f76 { get; set; } public bool? g76 { get; set; } public bool? h76 { get; set; } public bool? i76 { get; set; } public bool? j76 { get; set; } public bool? k76 { get; set; } public bool? l76 { get; set; } public bool? m76 { get; set; } public bool? n76 { get; set; } public bool? o76 { get; set; } public bool? p76 { get; set; } public bool? q76 { get; set; } public bool? r76 { get; set; } public bool? s76 { get; set; } public bool? t76 { get; set; } public bool? u76 { get; set; } public bool? v76 { get; set; } public bool? w76 { get; set; } public bool? x76 { get; set; } public bool? y76 { get; set; } public bool? z76 { get; set; } 
            public bool? A77 { get; set; } public bool? B77 { get; set; } public bool? C77 { get; set; } public bool? D77 { get; set; } public bool? E77 { get; set; } public bool? F77 { get; set; } public bool? G77 { get; set; } public bool? H77 { get; set; } public bool? I77 { get; set; } public bool? J77 { get; set; } public bool? K77 { get; set; } public bool? L77 { get; set; } public bool? M77 { get; set; } public bool? N77 { get; set; } public bool? O77 { get; set; } public bool? P77 { get; set; } public bool? Q77 { get; set; } public bool? R77 { get; set; } public bool? S77 { get; set; } public bool? T77 { get; set; } public bool? U77 { get; set; } public bool? V77 { get; set; } public bool? W77 { get; set; } public bool? X77 { get; set; } public bool? Y77 { get; set; } public bool? Z77 { get; set; } public bool? a77 { get; set; } public bool? b77 { get; set; } public bool? c77 { get; set; } public bool? d77 { get; set; } public bool? e77 { get; set; } public bool? f77 { get; set; } public bool? g77 { get; set; } public bool? h77 { get; set; } public bool? i77 { get; set; } public bool? j77 { get; set; } public bool? k77 { get; set; } public bool? l77 { get; set; } public bool? m77 { get; set; } public bool? n77 { get; set; } public bool? o77 { get; set; } public bool? p77 { get; set; } public bool? q77 { get; set; } public bool? r77 { get; set; } public bool? s77 { get; set; } public bool? t77 { get; set; } public bool? u77 { get; set; } public bool? v77 { get; set; } public bool? w77 { get; set; } public bool? x77 { get; set; } public bool? y77 { get; set; } public bool? z77 { get; set; } 
            public bool? A78 { get; set; } public bool? B78 { get; set; } public bool? C78 { get; set; } public bool? D78 { get; set; } public bool? E78 { get; set; } public bool? F78 { get; set; } public bool? G78 { get; set; } public bool? H78 { get; set; } public bool? I78 { get; set; } public bool? J78 { get; set; } public bool? K78 { get; set; } public bool? L78 { get; set; } public bool? M78 { get; set; } public bool? N78 { get; set; } public bool? O78 { get; set; } public bool? P78 { get; set; } public bool? Q78 { get; set; } public bool? R78 { get; set; } public bool? S78 { get; set; } public bool? T78 { get; set; } public bool? U78 { get; set; } public bool? V78 { get; set; } public bool? W78 { get; set; } public bool? X78 { get; set; } public bool? Y78 { get; set; } public bool? Z78 { get; set; } public bool? a78 { get; set; } public bool? b78 { get; set; } public bool? c78 { get; set; } public bool? d78 { get; set; } public bool? e78 { get; set; } public bool? f78 { get; set; } public bool? g78 { get; set; } public bool? h78 { get; set; } public bool? i78 { get; set; } public bool? j78 { get; set; } public bool? k78 { get; set; } public bool? l78 { get; set; } public bool? m78 { get; set; } public bool? n78 { get; set; } public bool? o78 { get; set; } public bool? p78 { get; set; } public bool? q78 { get; set; } public bool? r78 { get; set; } public bool? s78 { get; set; } public bool? t78 { get; set; } public bool? u78 { get; set; } public bool? v78 { get; set; } public bool? w78 { get; set; } public bool? x78 { get; set; } public bool? y78 { get; set; } public bool? z78 { get; set; } 
            public bool? A79 { get; set; } public bool? B79 { get; set; } public bool? C79 { get; set; } public bool? D79 { get; set; } public bool? E79 { get; set; } public bool? F79 { get; set; } public bool? G79 { get; set; } public bool? H79 { get; set; } public bool? I79 { get; set; } public bool? J79 { get; set; } public bool? K79 { get; set; } public bool? L79 { get; set; } public bool? M79 { get; set; } public bool? N79 { get; set; } public bool? O79 { get; set; } public bool? P79 { get; set; } public bool? Q79 { get; set; } public bool? R79 { get; set; } public bool? S79 { get; set; } public bool? T79 { get; set; } public bool? U79 { get; set; } public bool? V79 { get; set; } public bool? W79 { get; set; } public bool? X79 { get; set; } public bool? Y79 { get; set; } public bool? Z79 { get; set; } public bool? a79 { get; set; } public bool? b79 { get; set; } public bool? c79 { get; set; } public bool? d79 { get; set; } public bool? e79 { get; set; } public bool? f79 { get; set; } public bool? g79 { get; set; } public bool? h79 { get; set; } public bool? i79 { get; set; } public bool? j79 { get; set; } public bool? k79 { get; set; } public bool? l79 { get; set; } public bool? m79 { get; set; } public bool? n79 { get; set; } public bool? o79 { get; set; } public bool? p79 { get; set; } public bool? q79 { get; set; } public bool? r79 { get; set; } public bool? s79 { get; set; } public bool? t79 { get; set; } public bool? u79 { get; set; } public bool? v79 { get; set; } public bool? w79 { get; set; } public bool? x79 { get; set; } public bool? y79 { get; set; } public bool? z79 { get; set; } 
            public bool? A81 { get; set; } public bool? B81 { get; set; } public bool? C81 { get; set; } public bool? D81 { get; set; } public bool? E81 { get; set; } public bool? F81 { get; set; } public bool? G81 { get; set; } public bool? H81 { get; set; } public bool? I81 { get; set; } public bool? J81 { get; set; } public bool? K81 { get; set; } public bool? L81 { get; set; } public bool? M81 { get; set; } public bool? N81 { get; set; } public bool? O81 { get; set; } public bool? P81 { get; set; } public bool? Q81 { get; set; } public bool? R81 { get; set; } public bool? S81 { get; set; } public bool? T81 { get; set; } public bool? U81 { get; set; } public bool? V81 { get; set; } public bool? W81 { get; set; } public bool? X81 { get; set; } public bool? Y81 { get; set; } public bool? Z81 { get; set; } public bool? a81 { get; set; } public bool? b81 { get; set; } public bool? c81 { get; set; } public bool? d81 { get; set; } public bool? e81 { get; set; } public bool? f81 { get; set; } public bool? g81 { get; set; } public bool? h81 { get; set; } public bool? i81 { get; set; } public bool? j81 { get; set; } public bool? k81 { get; set; } public bool? l81 { get; set; } public bool? m81 { get; set; } public bool? n81 { get; set; } public bool? o81 { get; set; } public bool? p81 { get; set; } public bool? q81 { get; set; } public bool? r81 { get; set; } public bool? s81 { get; set; } public bool? t81 { get; set; } public bool? u81 { get; set; } public bool? v81 { get; set; } public bool? w81 { get; set; } public bool? x81 { get; set; } public bool? y81 { get; set; } public bool? z81 { get; set; } 
            public bool? A82 { get; set; } public bool? B82 { get; set; } public bool? C82 { get; set; } public bool? D82 { get; set; } public bool? E82 { get; set; } public bool? F82 { get; set; } public bool? G82 { get; set; } public bool? H82 { get; set; } public bool? I82 { get; set; } public bool? J82 { get; set; } public bool? K82 { get; set; } public bool? L82 { get; set; } public bool? M82 { get; set; } public bool? N82 { get; set; } public bool? O82 { get; set; } public bool? P82 { get; set; } public bool? Q82 { get; set; } public bool? R82 { get; set; } public bool? S82 { get; set; } public bool? T82 { get; set; } public bool? U82 { get; set; } public bool? V82 { get; set; } public bool? W82 { get; set; } public bool? X82 { get; set; } public bool? Y82 { get; set; } public bool? Z82 { get; set; } public bool? a82 { get; set; } public bool? b82 { get; set; } public bool? c82 { get; set; } public bool? d82 { get; set; } public bool? e82 { get; set; } public bool? f82 { get; set; } public bool? g82 { get; set; } public bool? h82 { get; set; } public bool? i82 { get; set; } public bool? j82 { get; set; } public bool? k82 { get; set; } public bool? l82 { get; set; } public bool? m82 { get; set; } public bool? n82 { get; set; } public bool? o82 { get; set; } public bool? p82 { get; set; } public bool? q82 { get; set; } public bool? r82 { get; set; } public bool? s82 { get; set; } public bool? t82 { get; set; } public bool? u82 { get; set; } public bool? v82 { get; set; } public bool? w82 { get; set; } public bool? x82 { get; set; } public bool? y82 { get; set; } public bool? z82 { get; set; } 
            public bool? A83 { get; set; } public bool? B83 { get; set; } public bool? C83 { get; set; } public bool? D83 { get; set; } public bool? E83 { get; set; } public bool? F83 { get; set; } public bool? G83 { get; set; } public bool? H83 { get; set; } public bool? I83 { get; set; } public bool? J83 { get; set; } public bool? K83 { get; set; } public bool? L83 { get; set; } public bool? M83 { get; set; } public bool? N83 { get; set; } public bool? O83 { get; set; } public bool? P83 { get; set; } public bool? Q83 { get; set; } public bool? R83 { get; set; } public bool? S83 { get; set; } public bool? T83 { get; set; } public bool? U83 { get; set; } public bool? V83 { get; set; } public bool? W83 { get; set; } public bool? X83 { get; set; } public bool? Y83 { get; set; } public bool? Z83 { get; set; } public bool? a83 { get; set; } public bool? b83 { get; set; } public bool? c83 { get; set; } public bool? d83 { get; set; } public bool? e83 { get; set; } public bool? f83 { get; set; } public bool? g83 { get; set; } public bool? h83 { get; set; } public bool? i83 { get; set; } public bool? j83 { get; set; } public bool? k83 { get; set; } public bool? l83 { get; set; } public bool? m83 { get; set; } public bool? n83 { get; set; } public bool? o83 { get; set; } public bool? p83 { get; set; } public bool? q83 { get; set; } public bool? r83 { get; set; } public bool? s83 { get; set; } public bool? t83 { get; set; } public bool? u83 { get; set; } public bool? v83 { get; set; } public bool? w83 { get; set; } public bool? x83 { get; set; } public bool? y83 { get; set; } public bool? z83 { get; set; } 
            public bool? A84 { get; set; } public bool? B84 { get; set; } public bool? C84 { get; set; } public bool? D84 { get; set; } public bool? E84 { get; set; } public bool? F84 { get; set; } public bool? G84 { get; set; } public bool? H84 { get; set; } public bool? I84 { get; set; } public bool? J84 { get; set; } public bool? K84 { get; set; } public bool? L84 { get; set; } public bool? M84 { get; set; } public bool? N84 { get; set; } public bool? O84 { get; set; } public bool? P84 { get; set; } public bool? Q84 { get; set; } public bool? R84 { get; set; } public bool? S84 { get; set; } public bool? T84 { get; set; } public bool? U84 { get; set; } public bool? V84 { get; set; } public bool? W84 { get; set; } public bool? X84 { get; set; } public bool? Y84 { get; set; } public bool? Z84 { get; set; } public bool? a84 { get; set; } public bool? b84 { get; set; } public bool? c84 { get; set; } public bool? d84 { get; set; } public bool? e84 { get; set; } public bool? f84 { get; set; } public bool? g84 { get; set; } public bool? h84 { get; set; } public bool? i84 { get; set; } public bool? j84 { get; set; } public bool? k84 { get; set; } public bool? l84 { get; set; } public bool? m84 { get; set; } public bool? n84 { get; set; } public bool? o84 { get; set; } public bool? p84 { get; set; } public bool? q84 { get; set; } public bool? r84 { get; set; } public bool? s84 { get; set; } public bool? t84 { get; set; } public bool? u84 { get; set; } public bool? v84 { get; set; } public bool? w84 { get; set; } public bool? x84 { get; set; } public bool? y84 { get; set; } public bool? z84 { get; set; } 
            public bool? A85 { get; set; } public bool? B85 { get; set; } public bool? C85 { get; set; } public bool? D85 { get; set; } public bool? E85 { get; set; } public bool? F85 { get; set; } public bool? G85 { get; set; } public bool? H85 { get; set; } public bool? I85 { get; set; } public bool? J85 { get; set; } public bool? K85 { get; set; } public bool? L85 { get; set; } public bool? M85 { get; set; } public bool? N85 { get; set; } public bool? O85 { get; set; } public bool? P85 { get; set; } public bool? Q85 { get; set; } public bool? R85 { get; set; } public bool? S85 { get; set; } public bool? T85 { get; set; } public bool? U85 { get; set; } public bool? V85 { get; set; } public bool? W85 { get; set; } public bool? X85 { get; set; } public bool? Y85 { get; set; } public bool? Z85 { get; set; } public bool? a85 { get; set; } public bool? b85 { get; set; } public bool? c85 { get; set; } public bool? d85 { get; set; } public bool? e85 { get; set; } public bool? f85 { get; set; } public bool? g85 { get; set; } public bool? h85 { get; set; } public bool? i85 { get; set; } public bool? j85 { get; set; } public bool? k85 { get; set; } public bool? l85 { get; set; } public bool? m85 { get; set; } public bool? n85 { get; set; } public bool? o85 { get; set; } public bool? p85 { get; set; } public bool? q85 { get; set; } public bool? r85 { get; set; } public bool? s85 { get; set; } public bool? t85 { get; set; } public bool? u85 { get; set; } public bool? v85 { get; set; } public bool? w85 { get; set; } public bool? x85 { get; set; } public bool? y85 { get; set; } public bool? z85 { get; set; } 
            public bool? A86 { get; set; } public bool? B86 { get; set; } public bool? C86 { get; set; } public bool? D86 { get; set; } public bool? E86 { get; set; } public bool? F86 { get; set; } public bool? G86 { get; set; } public bool? H86 { get; set; } public bool? I86 { get; set; } public bool? J86 { get; set; } public bool? K86 { get; set; } public bool? L86 { get; set; } public bool? M86 { get; set; } public bool? N86 { get; set; } public bool? O86 { get; set; } public bool? P86 { get; set; } public bool? Q86 { get; set; } public bool? R86 { get; set; } public bool? S86 { get; set; } public bool? T86 { get; set; } public bool? U86 { get; set; } public bool? V86 { get; set; } public bool? W86 { get; set; } public bool? X86 { get; set; } public bool? Y86 { get; set; } public bool? Z86 { get; set; } public bool? a86 { get; set; } public bool? b86 { get; set; } public bool? c86 { get; set; } public bool? d86 { get; set; } public bool? e86 { get; set; } public bool? f86 { get; set; } public bool? g86 { get; set; } public bool? h86 { get; set; } public bool? i86 { get; set; } public bool? j86 { get; set; } public bool? k86 { get; set; } public bool? l86 { get; set; } public bool? m86 { get; set; } public bool? n86 { get; set; } public bool? o86 { get; set; } public bool? p86 { get; set; } public bool? q86 { get; set; } public bool? r86 { get; set; } public bool? s86 { get; set; } public bool? t86 { get; set; } public bool? u86 { get; set; } public bool? v86 { get; set; } public bool? w86 { get; set; } public bool? x86 { get; set; } public bool? y86 { get; set; } public bool? z86 { get; set; } 
            public bool? A87 { get; set; } public bool? B87 { get; set; } public bool? C87 { get; set; } public bool? D87 { get; set; } public bool? E87 { get; set; } public bool? F87 { get; set; } public bool? G87 { get; set; } public bool? H87 { get; set; } public bool? I87 { get; set; } public bool? J87 { get; set; } public bool? K87 { get; set; } public bool? L87 { get; set; } public bool? M87 { get; set; } public bool? N87 { get; set; } public bool? O87 { get; set; } public bool? P87 { get; set; } public bool? Q87 { get; set; } public bool? R87 { get; set; } public bool? S87 { get; set; } public bool? T87 { get; set; } public bool? U87 { get; set; } public bool? V87 { get; set; } public bool? W87 { get; set; } public bool? X87 { get; set; } public bool? Y87 { get; set; } public bool? Z87 { get; set; } public bool? a87 { get; set; } public bool? b87 { get; set; } public bool? c87 { get; set; } public bool? d87 { get; set; } public bool? e87 { get; set; } public bool? f87 { get; set; } public bool? g87 { get; set; } public bool? h87 { get; set; } public bool? i87 { get; set; } public bool? j87 { get; set; } public bool? k87 { get; set; } public bool? l87 { get; set; } public bool? m87 { get; set; } public bool? n87 { get; set; } public bool? o87 { get; set; } public bool? p87 { get; set; } public bool? q87 { get; set; } public bool? r87 { get; set; } public bool? s87 { get; set; } public bool? t87 { get; set; } public bool? u87 { get; set; } public bool? v87 { get; set; } public bool? w87 { get; set; } public bool? x87 { get; set; } public bool? y87 { get; set; } public bool? z87 { get; set; } 
            public bool? A88 { get; set; } public bool? B88 { get; set; } public bool? C88 { get; set; } public bool? D88 { get; set; } public bool? E88 { get; set; } public bool? F88 { get; set; } public bool? G88 { get; set; } public bool? H88 { get; set; } public bool? I88 { get; set; } public bool? J88 { get; set; } public bool? K88 { get; set; } public bool? L88 { get; set; } public bool? M88 { get; set; } public bool? N88 { get; set; } public bool? O88 { get; set; } public bool? P88 { get; set; } public bool? Q88 { get; set; } public bool? R88 { get; set; } public bool? S88 { get; set; } public bool? T88 { get; set; } public bool? U88 { get; set; } public bool? V88 { get; set; } public bool? W88 { get; set; } public bool? X88 { get; set; } public bool? Y88 { get; set; } public bool? Z88 { get; set; } public bool? a88 { get; set; } public bool? b88 { get; set; } public bool? c88 { get; set; } public bool? d88 { get; set; } public bool? e88 { get; set; } public bool? f88 { get; set; } public bool? g88 { get; set; } public bool? h88 { get; set; } public bool? i88 { get; set; } public bool? j88 { get; set; } public bool? k88 { get; set; } public bool? l88 { get; set; } public bool? m88 { get; set; } public bool? n88 { get; set; } public bool? o88 { get; set; } public bool? p88 { get; set; } public bool? q88 { get; set; } public bool? r88 { get; set; } public bool? s88 { get; set; } public bool? t88 { get; set; } public bool? u88 { get; set; } public bool? v88 { get; set; } public bool? w88 { get; set; } public bool? x88 { get; set; } public bool? y88 { get; set; } public bool? z88 { get; set; } 
            public bool? A89 { get; set; } public bool? B89 { get; set; } public bool? C89 { get; set; } public bool? D89 { get; set; } public bool? E89 { get; set; } public bool? F89 { get; set; } public bool? G89 { get; set; } public bool? H89 { get; set; } public bool? I89 { get; set; } public bool? J89 { get; set; } public bool? K89 { get; set; } public bool? L89 { get; set; } public bool? M89 { get; set; } public bool? N89 { get; set; } public bool? O89 { get; set; } public bool? P89 { get; set; } public bool? Q89 { get; set; } public bool? R89 { get; set; } public bool? S89 { get; set; } public bool? T89 { get; set; } public bool? U89 { get; set; } public bool? V89 { get; set; } public bool? W89 { get; set; } public bool? X89 { get; set; } public bool? Y89 { get; set; } public bool? Z89 { get; set; } public bool? a89 { get; set; } public bool? b89 { get; set; } public bool? c89 { get; set; } public bool? d89 { get; set; } public bool? e89 { get; set; } public bool? f89 { get; set; } public bool? g89 { get; set; } public bool? h89 { get; set; } public bool? i89 { get; set; } public bool? j89 { get; set; } public bool? k89 { get; set; } public bool? l89 { get; set; } public bool? m89 { get; set; } public bool? n89 { get; set; } public bool? o89 { get; set; } public bool? p89 { get; set; } public bool? q89 { get; set; } public bool? r89 { get; set; } public bool? s89 { get; set; } public bool? t89 { get; set; } public bool? u89 { get; set; } public bool? v89 { get; set; } public bool? w89 { get; set; } public bool? x89 { get; set; } public bool? y89 { get; set; } public bool? z89 { get; set; } 
            public bool? A91 { get; set; } public bool? B91 { get; set; } public bool? C91 { get; set; } public bool? D91 { get; set; } public bool? E91 { get; set; } public bool? F91 { get; set; } public bool? G91 { get; set; } public bool? H91 { get; set; } public bool? I91 { get; set; } public bool? J91 { get; set; } public bool? K91 { get; set; } public bool? L91 { get; set; } public bool? M91 { get; set; } public bool? N91 { get; set; } public bool? O91 { get; set; } public bool? P91 { get; set; } public bool? Q91 { get; set; } public bool? R91 { get; set; } public bool? S91 { get; set; } public bool? T91 { get; set; } public bool? U91 { get; set; } public bool? V91 { get; set; } public bool? W91 { get; set; } public bool? X91 { get; set; } public bool? Y91 { get; set; } public bool? Z91 { get; set; } public bool? a91 { get; set; } public bool? b91 { get; set; } public bool? c91 { get; set; } public bool? d91 { get; set; } public bool? e91 { get; set; } public bool? f91 { get; set; } public bool? g91 { get; set; } public bool? h91 { get; set; } public bool? i91 { get; set; } public bool? j91 { get; set; } public bool? k91 { get; set; } public bool? l91 { get; set; } public bool? m91 { get; set; } public bool? n91 { get; set; } public bool? o91 { get; set; } public bool? p91 { get; set; } public bool? q91 { get; set; } public bool? r91 { get; set; } public bool? s91 { get; set; } public bool? t91 { get; set; } public bool? u91 { get; set; } public bool? v91 { get; set; } public bool? w91 { get; set; } public bool? x91 { get; set; } public bool? y91 { get; set; } public bool? z91 { get; set; } 
            public bool? A92 { get; set; } public bool? B92 { get; set; } public bool? C92 { get; set; } public bool? D92 { get; set; } public bool? E92 { get; set; } public bool? F92 { get; set; } public bool? G92 { get; set; } public bool? H92 { get; set; } public bool? I92 { get; set; } public bool? J92 { get; set; } public bool? K92 { get; set; } public bool? L92 { get; set; } public bool? M92 { get; set; } public bool? N92 { get; set; } public bool? O92 { get; set; } public bool? P92 { get; set; } public bool? Q92 { get; set; } public bool? R92 { get; set; } public bool? S92 { get; set; } public bool? T92 { get; set; } public bool? U92 { get; set; } public bool? V92 { get; set; } public bool? W92 { get; set; } public bool? X92 { get; set; } public bool? Y92 { get; set; } public bool? Z92 { get; set; } public bool? a92 { get; set; } public bool? b92 { get; set; } public bool? c92 { get; set; } public bool? d92 { get; set; } public bool? e92 { get; set; } public bool? f92 { get; set; } public bool? g92 { get; set; } public bool? h92 { get; set; } public bool? i92 { get; set; } public bool? j92 { get; set; } public bool? k92 { get; set; } public bool? l92 { get; set; } public bool? m92 { get; set; } public bool? n92 { get; set; } public bool? o92 { get; set; } public bool? p92 { get; set; } public bool? q92 { get; set; } public bool? r92 { get; set; } public bool? s92 { get; set; } public bool? t92 { get; set; } public bool? u92 { get; set; } public bool? v92 { get; set; } public bool? w92 { get; set; } public bool? x92 { get; set; } public bool? y92 { get; set; } public bool? z92 { get; set; } 
            public bool? A93 { get; set; } public bool? B93 { get; set; } public bool? C93 { get; set; } public bool? D93 { get; set; } public bool? E93 { get; set; } public bool? F93 { get; set; } public bool? G93 { get; set; } public bool? H93 { get; set; } public bool? I93 { get; set; } public bool? J93 { get; set; } public bool? K93 { get; set; } public bool? L93 { get; set; } public bool? M93 { get; set; } public bool? N93 { get; set; } public bool? O93 { get; set; } public bool? P93 { get; set; } public bool? Q93 { get; set; } public bool? R93 { get; set; } public bool? S93 { get; set; } public bool? T93 { get; set; } public bool? U93 { get; set; } public bool? V93 { get; set; } public bool? W93 { get; set; } public bool? X93 { get; set; } public bool? Y93 { get; set; } public bool? Z93 { get; set; } public bool? a93 { get; set; } public bool? b93 { get; set; } public bool? c93 { get; set; } public bool? d93 { get; set; } public bool? e93 { get; set; } public bool? f93 { get; set; } public bool? g93 { get; set; } public bool? h93 { get; set; } public bool? i93 { get; set; } public bool? j93 { get; set; } public bool? k93 { get; set; } public bool? l93 { get; set; } public bool? m93 { get; set; } public bool? n93 { get; set; } public bool? o93 { get; set; } public bool? p93 { get; set; } public bool? q93 { get; set; } public bool? r93 { get; set; } public bool? s93 { get; set; } public bool? t93 { get; set; } public bool? u93 { get; set; } public bool? v93 { get; set; } public bool? w93 { get; set; } public bool? x93 { get; set; } public bool? y93 { get; set; } public bool? z93 { get; set; } 
            public bool? A94 { get; set; } public bool? B94 { get; set; } public bool? C94 { get; set; } public bool? D94 { get; set; } public bool? E94 { get; set; } public bool? F94 { get; set; } public bool? G94 { get; set; } public bool? H94 { get; set; } public bool? I94 { get; set; } public bool? J94 { get; set; } public bool? K94 { get; set; } public bool? L94 { get; set; } public bool? M94 { get; set; } public bool? N94 { get; set; } public bool? O94 { get; set; } public bool? P94 { get; set; } public bool? Q94 { get; set; } public bool? R94 { get; set; } public bool? S94 { get; set; } public bool? T94 { get; set; } public bool? U94 { get; set; } public bool? V94 { get; set; } public bool? W94 { get; set; } public bool? X94 { get; set; } public bool? Y94 { get; set; } public bool? Z94 { get; set; } public bool? a94 { get; set; } public bool? b94 { get; set; } public bool? c94 { get; set; } public bool? d94 { get; set; } public bool? e94 { get; set; } public bool? f94 { get; set; } public bool? g94 { get; set; } public bool? h94 { get; set; } public bool? i94 { get; set; } public bool? j94 { get; set; } public bool? k94 { get; set; } public bool? l94 { get; set; } public bool? m94 { get; set; } public bool? n94 { get; set; } public bool? o94 { get; set; } public bool? p94 { get; set; } public bool? q94 { get; set; } public bool? r94 { get; set; } public bool? s94 { get; set; } public bool? t94 { get; set; } public bool? u94 { get; set; } public bool? v94 { get; set; } public bool? w94 { get; set; } public bool? x94 { get; set; } public bool? y94 { get; set; } public bool? z94 { get; set; } 
            public bool? A95 { get; set; } public bool? B95 { get; set; } public bool? C95 { get; set; } public bool? D95 { get; set; } public bool? E95 { get; set; } public bool? F95 { get; set; } public bool? G95 { get; set; } public bool? H95 { get; set; } public bool? I95 { get; set; } public bool? J95 { get; set; } public bool? K95 { get; set; } public bool? L95 { get; set; } public bool? M95 { get; set; } public bool? N95 { get; set; } public bool? O95 { get; set; } public bool? P95 { get; set; } public bool? Q95 { get; set; } public bool? R95 { get; set; } public bool? S95 { get; set; } public bool? T95 { get; set; } public bool? U95 { get; set; } public bool? V95 { get; set; } public bool? W95 { get; set; } public bool? X95 { get; set; } public bool? Y95 { get; set; } public bool? Z95 { get; set; } public bool? a95 { get; set; } public bool? b95 { get; set; } public bool? c95 { get; set; } public bool? d95 { get; set; } public bool? e95 { get; set; } public bool? f95 { get; set; } public bool? g95 { get; set; } public bool? h95 { get; set; } public bool? i95 { get; set; } public bool? j95 { get; set; } public bool? k95 { get; set; } public bool? l95 { get; set; } public bool? m95 { get; set; } public bool? n95 { get; set; } public bool? o95 { get; set; } public bool? p95 { get; set; } public bool? q95 { get; set; } public bool? r95 { get; set; } public bool? s95 { get; set; } public bool? t95 { get; set; } public bool? u95 { get; set; } public bool? v95 { get; set; } public bool? w95 { get; set; } public bool? x95 { get; set; } public bool? y95 { get; set; } public bool? z95 { get; set; } 
            public bool? A96 { get; set; } public bool? B96 { get; set; } public bool? C96 { get; set; } public bool? D96 { get; set; } public bool? E96 { get; set; } public bool? F96 { get; set; } public bool? G96 { get; set; } public bool? H96 { get; set; } public bool? I96 { get; set; } public bool? J96 { get; set; } public bool? K96 { get; set; } public bool? L96 { get; set; } public bool? M96 { get; set; } public bool? N96 { get; set; } public bool? O96 { get; set; } public bool? P96 { get; set; } public bool? Q96 { get; set; } public bool? R96 { get; set; } public bool? S96 { get; set; } public bool? T96 { get; set; } public bool? U96 { get; set; } public bool? V96 { get; set; } public bool? W96 { get; set; } public bool? X96 { get; set; } public bool? Y96 { get; set; } public bool? Z96 { get; set; } public bool? a96 { get; set; } public bool? b96 { get; set; } public bool? c96 { get; set; } public bool? d96 { get; set; } public bool? e96 { get; set; } public bool? f96 { get; set; } public bool? g96 { get; set; } public bool? h96 { get; set; } public bool? i96 { get; set; } public bool? j96 { get; set; } public bool? k96 { get; set; } public bool? l96 { get; set; } public bool? m96 { get; set; } public bool? n96 { get; set; } public bool? o96 { get; set; } public bool? p96 { get; set; } public bool? q96 { get; set; } public bool? r96 { get; set; } public bool? s96 { get; set; } public bool? t96 { get; set; } public bool? u96 { get; set; } public bool? v96 { get; set; } public bool? w96 { get; set; } public bool? x96 { get; set; } public bool? y96 { get; set; } public bool? z96 { get; set; } 
            public bool? A97 { get; set; } public bool? B97 { get; set; } public bool? C97 { get; set; } public bool? D97 { get; set; } public bool? E97 { get; set; } public bool? F97 { get; set; } public bool? G97 { get; set; } public bool? H97 { get; set; } public bool? I97 { get; set; } public bool? J97 { get; set; } public bool? K97 { get; set; } public bool? L97 { get; set; } public bool? M97 { get; set; } public bool? N97 { get; set; } public bool? O97 { get; set; } public bool? P97 { get; set; } public bool? Q97 { get; set; } public bool? R97 { get; set; } public bool? S97 { get; set; } public bool? T97 { get; set; } public bool? U97 { get; set; } public bool? V97 { get; set; } public bool? W97 { get; set; } public bool? X97 { get; set; } public bool? Y97 { get; set; } public bool? Z97 { get; set; } public bool? a97 { get; set; } public bool? b97 { get; set; } public bool? c97 { get; set; } public bool? d97 { get; set; } public bool? e97 { get; set; } public bool? f97 { get; set; } public bool? g97 { get; set; } public bool? h97 { get; set; } public bool? i97 { get; set; } public bool? j97 { get; set; } public bool? k97 { get; set; } public bool? l97 { get; set; } public bool? m97 { get; set; } public bool? n97 { get; set; } public bool? o97 { get; set; } public bool? p97 { get; set; } public bool? q97 { get; set; } public bool? r97 { get; set; } public bool? s97 { get; set; } public bool? t97 { get; set; } public bool? u97 { get; set; } public bool? v97 { get; set; } public bool? w97 { get; set; } public bool? x97 { get; set; } public bool? y97 { get; set; } public bool? z97 { get; set; } 
            public bool? A98 { get; set; } public bool? B98 { get; set; } public bool? C98 { get; set; } public bool? D98 { get; set; } public bool? E98 { get; set; } public bool? F98 { get; set; } public bool? G98 { get; set; } public bool? H98 { get; set; } public bool? I98 { get; set; } public bool? J98 { get; set; } public bool? K98 { get; set; } public bool? L98 { get; set; } public bool? M98 { get; set; } public bool? N98 { get; set; } public bool? O98 { get; set; } public bool? P98 { get; set; } public bool? Q98 { get; set; } public bool? R98 { get; set; } public bool? S98 { get; set; } public bool? T98 { get; set; } public bool? U98 { get; set; } public bool? V98 { get; set; } public bool? W98 { get; set; } public bool? X98 { get; set; } public bool? Y98 { get; set; } public bool? Z98 { get; set; } public bool? a98 { get; set; } public bool? b98 { get; set; } public bool? c98 { get; set; } public bool? d98 { get; set; } public bool? e98 { get; set; } public bool? f98 { get; set; } public bool? g98 { get; set; } public bool? h98 { get; set; } public bool? i98 { get; set; } public bool? j98 { get; set; } public bool? k98 { get; set; } public bool? l98 { get; set; } public bool? m98 { get; set; } public bool? n98 { get; set; } public bool? o98 { get; set; } public bool? p98 { get; set; } public bool? q98 { get; set; } public bool? r98 { get; set; } public bool? s98 { get; set; } public bool? t98 { get; set; } public bool? u98 { get; set; } public bool? v98 { get; set; } public bool? w98 { get; set; } public bool? x98 { get; set; } public bool? y98 { get; set; } public bool? z98 { get; set; } 
            public bool? A99 { get; set; } public bool? B99 { get; set; } public bool? C99 { get; set; } public bool? D99 { get; set; } public bool? E99 { get; set; } public bool? F99 { get; set; } public bool? G99 { get; set; } public bool? H99 { get; set; } public bool? I99 { get; set; } public bool? J99 { get; set; } public bool? K99 { get; set; } public bool? L99 { get; set; } public bool? M99 { get; set; } public bool? N99 { get; set; } public bool? O99 { get; set; } public bool? P99 { get; set; } public bool? Q99 { get; set; } public bool? R99 { get; set; } public bool? S99 { get; set; } public bool? T99 { get; set; } public bool? U99 { get; set; } public bool? V99 { get; set; } public bool? W99 { get; set; } public bool? X99 { get; set; } public bool? Y99 { get; set; } public bool? Z99 { get; set; } public bool? a99 { get; set; } public bool? b99 { get; set; } public bool? c99 { get; set; } public bool? d99 { get; set; } public bool? e99 { get; set; } public bool? f99 { get; set; } public bool? g99 { get; set; } public bool? h99 { get; set; } public bool? i99 { get; set; } public bool? j99 { get; set; } public bool? k99 { get; set; } public bool? l99 { get; set; } public bool? m99 { get; set; } public bool? n99 { get; set; } public bool? o99 { get; set; } public bool? p99 { get; set; } public bool? q99 { get; set; } public bool? r99 { get; set; } public bool? s99 { get; set; } public bool? t99 { get; set; } public bool? u99 { get; set; } public bool? v99 { get; set; } public bool? w99 { get; set; } public bool? x99 { get; set; } public bool? y99 { get; set; } public bool? z99 { get; set; } 
            public bool? A101 { get; set; } public bool? B101 { get; set; } public bool? C101 { get; set; } public bool? D101 { get; set; } public bool? E101 { get; set; } public bool? F101 { get; set; } public bool? G101 { get; set; } public bool? H101 { get; set; } public bool? I101 { get; set; } public bool? J101 { get; set; } public bool? K101 { get; set; } public bool? L101 { get; set; } public bool? M101 { get; set; } public bool? N101 { get; set; } public bool? O101 { get; set; } public bool? P101 { get; set; } public bool? Q101 { get; set; } public bool? R101 { get; set; } public bool? S101 { get; set; } public bool? T101 { get; set; } public bool? U101 { get; set; } public bool? V101 { get; set; } public bool? W101 { get; set; } public bool? X101 { get; set; } public bool? Y101 { get; set; } public bool? Z101 { get; set; } public bool? a101 { get; set; } public bool? b101 { get; set; } public bool? c101 { get; set; } public bool? d101 { get; set; } public bool? e101 { get; set; } public bool? f101 { get; set; } public bool? g101 { get; set; } public bool? h101 { get; set; } public bool? i101 { get; set; } public bool? j101 { get; set; } public bool? k101 { get; set; } public bool? l101 { get; set; } public bool? m101 { get; set; } public bool? n101 { get; set; } public bool? o101 { get; set; } public bool? p101 { get; set; } public bool? q101 { get; set; } public bool? r101 { get; set; } public bool? s101 { get; set; } public bool? t101 { get; set; } public bool? u101 { get; set; } public bool? v101 { get; set; } public bool? w101 { get; set; } public bool? x101 { get; set; } public bool? y101 { get; set; } public bool? z101 { get; set; } 
            public bool? A102 { get; set; } public bool? B102 { get; set; } public bool? C102 { get; set; } public bool? D102 { get; set; } public bool? E102 { get; set; } public bool? F102 { get; set; } public bool? G102 { get; set; } public bool? H102 { get; set; } public bool? I102 { get; set; } public bool? J102 { get; set; } public bool? K102 { get; set; } public bool? L102 { get; set; } public bool? M102 { get; set; } public bool? N102 { get; set; } public bool? O102 { get; set; } public bool? P102 { get; set; } public bool? Q102 { get; set; } public bool? R102 { get; set; } public bool? S102 { get; set; } public bool? T102 { get; set; } public bool? U102 { get; set; } public bool? V102 { get; set; } public bool? W102 { get; set; } public bool? X102 { get; set; } public bool? Y102 { get; set; } public bool? Z102 { get; set; } public bool? a102 { get; set; } public bool? b102 { get; set; } public bool? c102 { get; set; } public bool? d102 { get; set; } public bool? e102 { get; set; } public bool? f102 { get; set; } public bool? g102 { get; set; } public bool? h102 { get; set; } public bool? i102 { get; set; } public bool? j102 { get; set; } public bool? k102 { get; set; } public bool? l102 { get; set; } public bool? m102 { get; set; } public bool? n102 { get; set; } public bool? o102 { get; set; } public bool? p102 { get; set; } public bool? q102 { get; set; } public bool? r102 { get; set; } public bool? s102 { get; set; } public bool? t102 { get; set; } public bool? u102 { get; set; } public bool? v102 { get; set; } public bool? w102 { get; set; } public bool? x102 { get; set; } public bool? y102 { get; set; } public bool? z102 { get; set; } 
            public bool? A103 { get; set; } public bool? B103 { get; set; } public bool? C103 { get; set; } public bool? D103 { get; set; } public bool? E103 { get; set; } public bool? F103 { get; set; } public bool? G103 { get; set; } public bool? H103 { get; set; } public bool? I103 { get; set; } public bool? J103 { get; set; } public bool? K103 { get; set; } public bool? L103 { get; set; } public bool? M103 { get; set; } public bool? N103 { get; set; } public bool? O103 { get; set; } public bool? P103 { get; set; } public bool? Q103 { get; set; } public bool? R103 { get; set; } public bool? S103 { get; set; } public bool? T103 { get; set; } public bool? U103 { get; set; } public bool? V103 { get; set; } public bool? W103 { get; set; } public bool? X103 { get; set; } public bool? Y103 { get; set; } public bool? Z103 { get; set; } public bool? a103 { get; set; } public bool? b103 { get; set; } public bool? c103 { get; set; } public bool? d103 { get; set; } public bool? e103 { get; set; } public bool? f103 { get; set; } public bool? g103 { get; set; } public bool? h103 { get; set; } public bool? i103 { get; set; } public bool? j103 { get; set; } public bool? k103 { get; set; } public bool? l103 { get; set; } public bool? m103 { get; set; } public bool? n103 { get; set; } public bool? o103 { get; set; } public bool? p103 { get; set; } public bool? q103 { get; set; } public bool? r103 { get; set; } public bool? s103 { get; set; } public bool? t103 { get; set; } public bool? u103 { get; set; } public bool? v103 { get; set; } public bool? w103 { get; set; } public bool? x103 { get; set; } public bool? y103 { get; set; } public bool? z103 { get; set; } 
            public bool? A104 { get; set; } public bool? B104 { get; set; } public bool? C104 { get; set; } public bool? D104 { get; set; } public bool? E104 { get; set; } public bool? F104 { get; set; } public bool? G104 { get; set; } public bool? H104 { get; set; } public bool? I104 { get; set; } public bool? J104 { get; set; } public bool? K104 { get; set; } public bool? L104 { get; set; } public bool? M104 { get; set; } public bool? N104 { get; set; } public bool? O104 { get; set; } public bool? P104 { get; set; } public bool? Q104 { get; set; } public bool? R104 { get; set; } public bool? S104 { get; set; } public bool? T104 { get; set; } public bool? U104 { get; set; } public bool? V104 { get; set; } public bool? W104 { get; set; } public bool? X104 { get; set; } public bool? Y104 { get; set; } public bool? Z104 { get; set; } public bool? a104 { get; set; } public bool? b104 { get; set; } public bool? c104 { get; set; } public bool? d104 { get; set; } public bool? e104 { get; set; } public bool? f104 { get; set; } public bool? g104 { get; set; } public bool? h104 { get; set; } public bool? i104 { get; set; } public bool? j104 { get; set; } public bool? k104 { get; set; } public bool? l104 { get; set; } public bool? m104 { get; set; } public bool? n104 { get; set; } public bool? o104 { get; set; } public bool? p104 { get; set; } public bool? q104 { get; set; } public bool? r104 { get; set; } public bool? s104 { get; set; } public bool? t104 { get; set; } public bool? u104 { get; set; } public bool? v104 { get; set; } public bool? w104 { get; set; } public bool? x104 { get; set; } public bool? y104 { get; set; } public bool? z104 { get; set; } 
            public bool? A105 { get; set; } public bool? B105 { get; set; } public bool? C105 { get; set; } public bool? D105 { get; set; } public bool? E105 { get; set; } public bool? F105 { get; set; } public bool? G105 { get; set; } public bool? H105 { get; set; } public bool? I105 { get; set; } public bool? J105 { get; set; } public bool? K105 { get; set; } public bool? L105 { get; set; } public bool? M105 { get; set; } public bool? N105 { get; set; } public bool? O105 { get; set; } public bool? P105 { get; set; } public bool? Q105 { get; set; } public bool? R105 { get; set; } public bool? S105 { get; set; } public bool? T105 { get; set; } public bool? U105 { get; set; } public bool? V105 { get; set; } public bool? W105 { get; set; } public bool? X105 { get; set; } public bool? Y105 { get; set; } public bool? Z105 { get; set; } public bool? a105 { get; set; } public bool? b105 { get; set; } public bool? c105 { get; set; } public bool? d105 { get; set; } public bool? e105 { get; set; } public bool? f105 { get; set; } public bool? g105 { get; set; } public bool? h105 { get; set; } public bool? i105 { get; set; } public bool? j105 { get; set; } public bool? k105 { get; set; } public bool? l105 { get; set; } public bool? m105 { get; set; } public bool? n105 { get; set; } public bool? o105 { get; set; } public bool? p105 { get; set; } public bool? q105 { get; set; } public bool? r105 { get; set; } public bool? s105 { get; set; } public bool? t105 { get; set; } public bool? u105 { get; set; } public bool? v105 { get; set; } public bool? w105 { get; set; } public bool? x105 { get; set; } public bool? y105 { get; set; } public bool? z105 { get; set; } 
            public bool? A106 { get; set; } public bool? B106 { get; set; } public bool? C106 { get; set; } public bool? D106 { get; set; } public bool? E106 { get; set; } public bool? F106 { get; set; } public bool? G106 { get; set; } public bool? H106 { get; set; } public bool? I106 { get; set; } public bool? J106 { get; set; } public bool? K106 { get; set; } public bool? L106 { get; set; } public bool? M106 { get; set; } public bool? N106 { get; set; } public bool? O106 { get; set; } public bool? P106 { get; set; } public bool? Q106 { get; set; } public bool? R106 { get; set; } public bool? S106 { get; set; } public bool? T106 { get; set; } public bool? U106 { get; set; } public bool? V106 { get; set; } public bool? W106 { get; set; } public bool? X106 { get; set; } public bool? Y106 { get; set; } public bool? Z106 { get; set; } public bool? a106 { get; set; } public bool? b106 { get; set; } public bool? c106 { get; set; } public bool? d106 { get; set; } public bool? e106 { get; set; } public bool? f106 { get; set; } public bool? g106 { get; set; } public bool? h106 { get; set; } public bool? i106 { get; set; } public bool? j106 { get; set; } public bool? k106 { get; set; } public bool? l106 { get; set; } public bool? m106 { get; set; } public bool? n106 { get; set; } public bool? o106 { get; set; } public bool? p106 { get; set; } public bool? q106 { get; set; } public bool? r106 { get; set; } public bool? s106 { get; set; } public bool? t106 { get; set; } public bool? u106 { get; set; } public bool? v106 { get; set; } public bool? w106 { get; set; } public bool? x106 { get; set; } public bool? y106 { get; set; } public bool? z106 { get; set; } 
            public bool? A107 { get; set; } public bool? B107 { get; set; } public bool? C107 { get; set; } public bool? D107 { get; set; } public bool? E107 { get; set; } public bool? F107 { get; set; } public bool? G107 { get; set; } public bool? H107 { get; set; } public bool? I107 { get; set; } public bool? J107 { get; set; } public bool? K107 { get; set; } public bool? L107 { get; set; } public bool? M107 { get; set; } public bool? N107 { get; set; } public bool? O107 { get; set; } public bool? P107 { get; set; } public bool? Q107 { get; set; } public bool? R107 { get; set; } public bool? S107 { get; set; } public bool? T107 { get; set; } public bool? U107 { get; set; } public bool? V107 { get; set; } public bool? W107 { get; set; } public bool? X107 { get; set; } public bool? Y107 { get; set; } public bool? Z107 { get; set; } public bool? a107 { get; set; } public bool? b107 { get; set; } public bool? c107 { get; set; } public bool? d107 { get; set; } public bool? e107 { get; set; } public bool? f107 { get; set; } public bool? g107 { get; set; } public bool? h107 { get; set; } public bool? i107 { get; set; } public bool? j107 { get; set; } public bool? k107 { get; set; } public bool? l107 { get; set; } public bool? m107 { get; set; } public bool? n107 { get; set; } public bool? o107 { get; set; } public bool? p107 { get; set; } public bool? q107 { get; set; } public bool? r107 { get; set; } public bool? s107 { get; set; } public bool? t107 { get; set; } public bool? u107 { get; set; } public bool? v107 { get; set; } public bool? w107 { get; set; } public bool? x107 { get; set; } public bool? y107 { get; set; } public bool? z107 { get; set; } 
            public bool? A108 { get; set; } public bool? B108 { get; set; } public bool? C108 { get; set; } public bool? D108 { get; set; } public bool? E108 { get; set; } public bool? F108 { get; set; } public bool? G108 { get; set; } public bool? H108 { get; set; } public bool? I108 { get; set; } public bool? J108 { get; set; } public bool? K108 { get; set; } public bool? L108 { get; set; } public bool? M108 { get; set; } public bool? N108 { get; set; } public bool? O108 { get; set; } public bool? P108 { get; set; } public bool? Q108 { get; set; } public bool? R108 { get; set; } public bool? S108 { get; set; } public bool? T108 { get; set; } public bool? U108 { get; set; } public bool? V108 { get; set; } public bool? W108 { get; set; } public bool? X108 { get; set; } public bool? Y108 { get; set; } public bool? Z108 { get; set; } public bool? a108 { get; set; } public bool? b108 { get; set; } public bool? c108 { get; set; } public bool? d108 { get; set; } public bool? e108 { get; set; } public bool? f108 { get; set; } public bool? g108 { get; set; } public bool? h108 { get; set; } public bool? i108 { get; set; } public bool? j108 { get; set; } public bool? k108 { get; set; } public bool? l108 { get; set; } public bool? m108 { get; set; } public bool? n108 { get; set; } public bool? o108 { get; set; } public bool? p108 { get; set; } public bool? q108 { get; set; } public bool? r108 { get; set; } public bool? s108 { get; set; } public bool? t108 { get; set; } public bool? u108 { get; set; } public bool? v108 { get; set; } public bool? w108 { get; set; } public bool? x108 { get; set; } public bool? y108 { get; set; } public bool? z108 { get; set; } 
            public bool? A109 { get; set; } public bool? B109 { get; set; } public bool? C109 { get; set; } public bool? D109 { get; set; } public bool? E109 { get; set; } public bool? F109 { get; set; } public bool? G109 { get; set; } public bool? H109 { get; set; } public bool? I109 { get; set; } public bool? J109 { get; set; } public bool? K109 { get; set; } public bool? L109 { get; set; } public bool? M109 { get; set; } public bool? N109 { get; set; } public bool? O109 { get; set; } public bool? P109 { get; set; } public bool? Q109 { get; set; } public bool? R109 { get; set; } public bool? S109 { get; set; } public bool? T109 { get; set; } public bool? U109 { get; set; } public bool? V109 { get; set; } public bool? W109 { get; set; } public bool? X109 { get; set; } public bool? Y109 { get; set; } public bool? Z109 { get; set; } public bool? a109 { get; set; } public bool? b109 { get; set; } public bool? c109 { get; set; } public bool? d109 { get; set; } public bool? e109 { get; set; } public bool? f109 { get; set; } public bool? g109 { get; set; } public bool? h109 { get; set; } public bool? i109 { get; set; } public bool? j109 { get; set; } public bool? k109 { get; set; } public bool? l109 { get; set; } public bool? m109 { get; set; } public bool? n109 { get; set; } public bool? o109 { get; set; } public bool? p109 { get; set; } public bool? q109 { get; set; } public bool? r109 { get; set; } public bool? s109 { get; set; } public bool? t109 { get; set; } public bool? u109 { get; set; } public bool? v109 { get; set; } public bool? w109 { get; set; } public bool? x109 { get; set; } public bool? y109 { get; set; } public bool? z109 { get; set; } 
        }

        [Fact]
        public void DegeneratelyLargeFlat()
        {
            // string
            {
                _DegeneratelyLargeFlat res = null;
                var t =
                    new Thread(
                        () =>
                        {
                            res = JSON.Deserialize<_DegeneratelyLargeFlat>("{ \"Success\": true }");
                        }
                    )
                    {
                        IsBackground = true
                    };

                t.Start();
                t.Join(30 * 1000);

                Assert.False(t.IsAlive);
                Assert.NotNull(res);
                Assert.True(res.Success);
            }

            // string
            {
                _DegeneratelyLargeFlat res = null;
                var t =
                    new Thread(
                        () =>
                        {
                            using (var str = new StringReader("{ \"Success\": true }"))
                            {
                                res = JSON.Deserialize<_DegeneratelyLargeFlat>(str);
                            }
                        }
                    )
                    {
                        IsBackground = true
                    };

                t.Start();
                t.Join(30 * 1000);

                Assert.False(t.IsAlive);
                Assert.NotNull(res);
                Assert.True(res.Success);
            }
        }
#endif

        //struct _AllFloatsStruct
        //{
        //    public float Float;
        //    public string AsString;
        //    public string Format;
        //    public uint I;
        //}

        //static readonly string[] _AllFloatsFormats = new[] { "F", /*"F1", "F2", "F3", "F4", "F5",*/ "G", "R" };
        //static IEnumerable<_AllFloatsStruct> _AllFloats()
        //{
        //    var byteArr = new byte[4];

        //    for (ulong i = 0; i <= uint.MaxValue; i++)
        //    {
        //        var asInt = (uint)i;
        //        byteArr[0] = (byte)((asInt) & 0xFF);
        //        byteArr[1] = (byte)((asInt >> 8) & 0xFF);
        //        byteArr[2] = (byte)((asInt >> 16) & 0xFF);
        //        byteArr[3] = (byte)((asInt >> 24) & 0xFF);
        //        var f = BitConverter.ToSingle(byteArr, 0);

        //        if (float.IsNaN(f) || float.IsInfinity(f)) continue;

        //        for (var j = 0; j < _AllFloatsFormats.Length; j++)
        //        {
        //            var format = _AllFloatsFormats[j];
        //            var asStr = f.ToString(format);

        //            yield return new _AllFloatsStruct { AsString = asStr, Float = f, Format = format, I = asInt };
        //        }
        //    }
        //}

        //class _AllFloatsPartitioner : Partitioner<_AllFloatsStruct>
        //{
        //    IEnumerable<_AllFloatsStruct> Underlying;

        //    public _AllFloatsPartitioner(IEnumerable<_AllFloatsStruct> underlying)
        //        : base()
        //    {
        //        Underlying = underlying;
        //    }

        //    public override bool SupportsDynamicPartitions
        //    {
        //        get
        //        {
        //            return true;
        //        }
        //    }

        //    public override IEnumerable<_AllFloatsStruct> GetDynamicPartitions()
        //    {
        //        return new DynamicPartition(Underlying);
        //    }

        //    public override IList<IEnumerator<_AllFloatsStruct>> GetPartitions(int partitionCount)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    class DynamicPartition : IEnumerable<_AllFloatsStruct>
        //    {
        //        internal IEnumerator<_AllFloatsStruct> All;

        //        public DynamicPartition(IEnumerable<_AllFloatsStruct> all)
        //        {
        //            All = all.GetEnumerator();
        //        }

        //        public IEnumerator<_AllFloatsStruct> GetEnumerator()
        //        {
        //            return new DynamicEnumerator(this);
        //        }

        //        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //        {
        //            return this.GetEnumerator();
        //        }

        //        class DynamicEnumerator : IEnumerator<_AllFloatsStruct>
        //        {
        //            const int Capacity = 100;
        //            DynamicPartition Outer;
        //            Queue<_AllFloatsStruct> Pending;

        //            public DynamicEnumerator(DynamicPartition outer)
        //            {
        //                Outer = outer;
        //                Pending = new Queue<_AllFloatsStruct>(Capacity);
        //            }

        //            public _AllFloatsStruct Current
        //            {
        //                get;
        //                private set;
        //            }

        //            public void Dispose()
        //            {
        //                // Don't care
        //            }

        //            object System.Collections.IEnumerator.Current
        //            {
        //                get { return this.Current; }
        //            }

        //            public bool MoveNext()
        //            {
        //                if(Pending.Count == 0)
        //                {
        //                    lock (Outer.All)
        //                    {
        //                        while (Outer.All.MoveNext() && Pending.Count < Capacity)
        //                        {
        //                            Pending.Enqueue(Outer.All.Current);
        //                        }
        //                    }
        //                }

        //                if (Pending.Count == 0) return false;

        //                Current = Pending.Dequeue();
        //                return true;
        //            }

        //            public void Reset()
        //            {
        //                throw new NotSupportedException();
        //            }
        //        }
        //    }
        //}

        //[Fact]
        //public void AllFloats()
        //{
        //    var e = _AllFloats();
        //    var partitioner = new _AllFloatsPartitioner(e);

        //    var options = new ParallelOptions();
        //    options.MaxDegreeOfParallelism = Environment.ProcessorCount - 1;

        //    Parallel.ForEach(
        //        partitioner,
        //        options,
        //        part =>
        //        {
        //            try
        //            {
        //                var i = part.I;
        //                var format = part.Format;
        //                var asStr = part.AsString;
        //                var res = JSON.Deserialize<float>(asStr);
        //                var reStr = res.ToString(format);

        //                var delta = Math.Abs((float.Parse(asStr) - float.Parse(reStr)));

        //                var closeEnough = asStr == reStr || delta <= float.Epsilon;

        //                Assert.True(closeEnough, "For i=" + i + " format=" + format + " delta=" + delta + " epsilon=" + float.Epsilon);
        //            }
        //            catch (Exception x)
        //            {
        //                throw new Exception(part.AsString, x);
        //            }
        //        }
        //    );
        //}
    }
}
