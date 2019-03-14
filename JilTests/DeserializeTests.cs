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

    public partial class DeserializeTests
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

        [Fact]
        public void FloatRoundTrip() // also issue #320
        {
            const float EXACT = 100921.688f;
            
            // stream
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(EXACT, str);
                    var val = str.ToString();
                    Assert.Equal(EXACT.ToString("R"), val);
                }
            }

            // string
            {
                var val = JSON.Serialize(EXACT);
                Assert.Equal(EXACT.ToString("R"), val);
            }
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
