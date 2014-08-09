﻿using Jil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

    [TestClass]
    public class DeserializeTests
    {
#pragma warning disable 0649
        struct _ValueTypes
        {
            public string A;
            public int B { get; set; }
        }
#pragma warning restore 0649

        [TestMethod]
        public void ValueTypes()
        {
            using (var str = new StringReader("{\"A\":\"hello\\u0000world\", \"B\":12345}"))
            {
                var res = JSON.Deserialize<_ValueTypes>(str);
                Assert.AreEqual("hello\0world", res.A);
                Assert.AreEqual(12345, res.B);
            }
        }

#pragma warning disable 0649
        class _LargeCharBuffer
        {
            public DateTime Date;
            public string String;
        }
#pragma warning restore 0649

        [TestMethod]
        public void LargeCharBuffer()
        {
            using (var str = new StringReader("{\"Date\": \"2013-12-30T04:17:21Z\", \"String\": \"hello world\"}"))
            {
                var res = JSON.Deserialize<_LargeCharBuffer>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(2013, 12, 30, 4, 17, 21, DateTimeKind.Utc), res.Date);
                Assert.AreEqual("hello world", res.String);
            }
        }

#pragma warning disable 0649
        class _SmallCharBuffer
        {
            public DateTime Date;
            public string String;
        }
#pragma warning restore 0649

        [TestMethod]
        public void SmallCharBuffer()
        {
            using (var str = new StringReader("{\"Date\": 1388377041, \"String\": \"hello world\"}"))
            {
                var res = JSON.Deserialize<_SmallCharBuffer>(str, Options.SecondsSinceUnixEpoch);
                Assert.AreEqual(new DateTime(2013, 12, 30, 4, 17, 21, DateTimeKind.Utc), res.Date);
                Assert.AreEqual("hello world", res.String);
            }
        }

        [TestMethod]
        public void IDictionaryIntToInt()
        {
            using (var str = new StringReader("{\"1\":2, \"3\":4, \"5\": 6}"))
            {
                var res = JSON.Deserialize<IDictionary<int, int>>(str);
                Assert.AreEqual(3, res.Count);
                Assert.AreEqual(2, res[1]);
                Assert.AreEqual(4, res[3]);
                Assert.AreEqual(6, res[5]);
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

        [TestMethod]
        public void DictionaryEnumKeys()
        {
            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys1, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys1.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys1, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys1.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys1.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys2, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys2.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys2, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys2.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys2.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys3, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys3.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys3, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys3.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys3.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys4, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys4.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys4, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys4.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys4.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys5, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys5.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys5, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys5.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys5.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys6, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys6.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys6, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys6.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys6.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys7, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys7.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys7, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys7.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys7.B]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys8, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys8.A]);
            }

            using (var str = new StringReader("{\"A\":\"hello world\",\"B\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<_DictionaryEnumKeys8, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[_DictionaryEnumKeys8.A]);
                Assert.AreEqual("fizz buzz", res[_DictionaryEnumKeys8.B]);
            }
        }

        [TestMethod]
        public void DictionaryNumberKeys()
        {
            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<byte, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<byte, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[1]);
                Assert.AreEqual("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"-1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<sbyte, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[-1]);
            }

            using (var str = new StringReader("{\"-1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<sbyte, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[-1]);
                Assert.AreEqual("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<short, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"-22\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<short, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[1]);
                Assert.AreEqual("fizz buzz", res[-22]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ushort, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"234\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ushort, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[1]);
                Assert.AreEqual("fizz buzz", res[234]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<int, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<int, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[1]);
                Assert.AreEqual("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<uint, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"2456789\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<uint, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[1]);
                Assert.AreEqual("fizz buzz", res[2456789]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<long, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[1]);
            }

            using (var str = new StringReader("{\"-1234567890\":\"hello world\",\"2\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<long, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[-1234567890]);
                Assert.AreEqual("fizz buzz", res[2]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ulong, string>>(str);
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual("hello world", res[1]);
            }

            using (var str = new StringReader("{\"1\":\"hello world\",\"" + ulong.MaxValue + "\":\"fizz buzz\"}"))
            {
                var res = JSON.Deserialize<Dictionary<ulong, string>>(str);
                Assert.AreEqual(2, res.Count);
                Assert.AreEqual("hello world", res[1]);
                Assert.AreEqual("fizz buzz", res[ulong.MaxValue]);
            }
        }

#pragma warning disable 0649
        class _IDictionaries
        {
            public IDictionary<string, sbyte> StringToBytes;
            public IDictionary<string, IDictionary<string, int>> StringToStringToBytes;
        }
#pragma warning restore 0649

        [TestMethod]
        public void IDictionaries()
        {
            using (var str = new StringReader("{\"StringToBytes\":{\"a\":-1,\"b\":127,\"c\":8},\"StringToStringToBytes\":{\"foo\":{\"bar\":123}, \"fizz\":{\"buzz\":456, \"bar\":789}}}"))
            {
                var res = JSON.Deserialize<_IDictionaries>(str);
                Assert.IsNotNull(res);

                Assert.IsNotNull(res.StringToBytes);
                Assert.AreEqual(3, res.StringToBytes.Count);
                Assert.IsTrue(res.StringToBytes.ContainsKey("a"));
                Assert.AreEqual((sbyte)-1, res.StringToBytes["a"]);
                Assert.IsTrue(res.StringToBytes.ContainsKey("b"));
                Assert.AreEqual((sbyte)127, res.StringToBytes["b"]);
                Assert.IsTrue(res.StringToBytes.ContainsKey("c"));
                Assert.AreEqual((sbyte)8, res.StringToBytes["c"]);

                Assert.IsNotNull(res.StringToStringToBytes);
                Assert.AreEqual(2, res.StringToStringToBytes.Count);
                Assert.IsTrue(res.StringToStringToBytes.ContainsKey("foo"));
                Assert.AreEqual(1, res.StringToStringToBytes["foo"].Count);
                Assert.IsTrue(res.StringToStringToBytes["foo"].ContainsKey("bar"));
                Assert.AreEqual(123, res.StringToStringToBytes["foo"]["bar"]);
                Assert.IsTrue(res.StringToStringToBytes.ContainsKey("fizz"));
                Assert.AreEqual(2, res.StringToStringToBytes["fizz"].Count);
                Assert.IsTrue(res.StringToStringToBytes["fizz"].ContainsKey("buzz"));
                Assert.AreEqual(456, res.StringToStringToBytes["fizz"]["buzz"]);
                Assert.IsTrue(res.StringToStringToBytes["fizz"].ContainsKey("bar"));
                Assert.AreEqual(789, res.StringToStringToBytes["fizz"]["bar"]);
            }
        }

#pragma warning disable 0649
        class _ILists
        {
            public IList<byte> Bytes;
            public IList<IList<int>> IntsOfInts;
        }
#pragma warning restore 0649

        [TestMethod]
        public void ILists()
        {
            using (var str = new StringReader("{\"Bytes\":[255,0,128],\"IntsOfInts\":[[1,2,3],[4,5,6],[7,8,9]]}"))
            {
                var res = JSON.Deserialize<_ILists>(str);
                Assert.IsNotNull(res);

                Assert.IsNotNull(res.Bytes);
                Assert.AreEqual(3, res.Bytes.Count);
                Assert.AreEqual(255, res.Bytes[0]);
                Assert.AreEqual(0, res.Bytes[1]);
                Assert.AreEqual(128, res.Bytes[2]);

                Assert.IsNotNull(res.IntsOfInts);
                Assert.AreEqual(3, res.IntsOfInts.Count);
                Assert.IsNotNull(res.IntsOfInts[0]);
                Assert.AreEqual(3, res.IntsOfInts[0].Count);
                Assert.AreEqual(1, res.IntsOfInts[0][0]);
                Assert.AreEqual(2, res.IntsOfInts[0][1]);
                Assert.AreEqual(3, res.IntsOfInts[0][2]);
                Assert.AreEqual(3, res.IntsOfInts[1].Count);
                Assert.AreEqual(4, res.IntsOfInts[1][0]);
                Assert.AreEqual(5, res.IntsOfInts[1][1]);
                Assert.AreEqual(6, res.IntsOfInts[1][2]);
                Assert.AreEqual(3, res.IntsOfInts[2].Count);
                Assert.AreEqual(7, res.IntsOfInts[2][0]);
                Assert.AreEqual(8, res.IntsOfInts[2][1]);
                Assert.AreEqual(9, res.IntsOfInts[2][2]);
            }
        }

#pragma warning disable 0649
        class _InfoFailure
        {
            public decimal? questions_per_minute;
            public decimal? answers_per_minute;
        }
#pragma warning restore 0649

        [TestMethod]
        public void InfoFailure()
        {
            {
                var data = "{\"questions_per_minute\":0,\"answers_per_minute\":null}";
                using (var str = new StringReader(data))
                {
                    var res = JSON.Deserialize<_InfoFailure>(str, Options.ISO8601);
                    Assert.IsNotNull(res);
                    Assert.AreEqual(0, res.questions_per_minute);
                    Assert.AreEqual(null, res.answers_per_minute);
                }
            }

            {
                var data = "{\"questions_per_minute\":0}";
                using (var str = new StringReader(data))
                {
                    var res = JSON.Deserialize<_InfoFailure>(str, Options.ISO8601);
                    Assert.IsNotNull(res);
                    Assert.AreEqual(0, res.questions_per_minute);
                }
            }
        }

        [TestMethod]
        public void Zeros()
        {
            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<int>(str);
                Assert.AreEqual(0, ret);
            }

            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<float>(str);
                Assert.AreEqual(0f, ret);
            }

            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<double>(str);
                Assert.AreEqual(0.0, ret);
            }

            using (var str = new StringReader("0"))
            {
                var ret = JSON.Deserialize<decimal>(str);
                Assert.AreEqual(0m, ret);
            }
        }

        [TestMethod]
        public void Arrays()
        {
            using (var str = new StringReader("[0,1,2,3,4,5]"))
            {
                var ret = JSON.Deserialize<int[]>(str);
                Assert.AreEqual(6, ret.Length);
                Assert.AreEqual(0, ret[0]);
                Assert.AreEqual(1, ret[1]);
                Assert.AreEqual(2, ret[2]);
                Assert.AreEqual(3, ret[3]);
                Assert.AreEqual(4, ret[4]);
                Assert.AreEqual(5, ret[5]);
            }
        }

        [TestMethod]
        public void ParseISO8601()
        {
            using (var str = new StringReader("\"1900\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1991-02\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1991, 02, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 0 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5+01:23\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 0 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5+0123\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 15, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5-11:45\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 15, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5-1145\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30Z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01t12:30+00\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30z\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2004-366\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"2004366\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }
        }

        [TestMethod]
        public void MalformedISO8601()
        {
            using (var str = new StringReader("\"99\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("ISO8601 date must begin with a 4 character year", e.Message);
                }
            }

            using (var str = new StringReader("\"0000\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("ISO8601 year 0000 cannot be converted to a DateTime", e.Message);
                }
            }

            using (var str = new StringReader("\"1999-13\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected month to be between 01 and 12", e.Message);
                }
            }

            using (var str = new StringReader("\"1999-12-00\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected day to be between 01 and 31", e.Message);
                }
            }

            using (var str = new StringReader("\"19991200\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected day to be between 01 and 31", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-01T12:34:56.123456789+00:00\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("ISO8601 date is too long, expected " + Jil.Deserialize.Methods.CharBufferSize + " characters or less", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-01T1234\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected :", e.Message);
                }
            }

            using (var str = new StringReader("\"19000101T12:34\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Unexpected separator", e.Message);
                }
            }

            using (var str = new StringReader("\"19000101T1234:56\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Unexpected separator in ISO8601 time", e.Message);
                }
            }

            using (var str = new StringReader("\"19000101T123456+00:30\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Unexpected separator in ISO8601 timezone offset", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-01+00:30\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Unexpected date string length", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Unexpected date string length", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected digit", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-01T\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("ISO8601 time must begin with a 2 character hour", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-01T19:\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected minute part of ISO8601 time", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-01T19:19+\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected hour part of ISO8601 timezone offset", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-01-01T19:19+00:\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected digit", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-366\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Ordinal day can only be 366 in a leap year", e.Message);
                }
            }

            using (var str = new StringReader("\"1900366\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Ordinal day can only be 366 in a leap year", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-999\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected ordinal day to be between 001 and 366", e.Message);
                }
            }

            using (var str = new StringReader("\"1900999\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected ordinal day to be between 001 and 366", e.Message);
                }
            }

            using (var str = new StringReader("\"1900-000\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected ordinal day to be between 001 and 366", e.Message);
                }
            }

            using (var str = new StringReader("\"1900000\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected ordinal day to be between 001 and 366", e.Message);
                }
            }

            using (var str = new StringReader("\"1999-02-29\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("ISO8601 date could not be mapped to DateTime", e.Message);
                    Assert.IsNotNull(e.InnerException);
                }
            }

            using (var str = new StringReader("\"1999-W01-8\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected day to be a digit between 1 and 7", e.Message);
                }
            }

            using (var str = new StringReader("\"1999-W01-0\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected day to be a digit between 1 and 7", e.Message);
                }
            }

            using (var str = new StringReader("\"1999W018\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected day to be a digit between 1 and 7", e.Message);
                }
            }

            using (var str = new StringReader("\"1999W010\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected day to be a digit between 1 and 7", e.Message);
                }
            }

            using (var str = new StringReader("\"1999-W1\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Unexpected date string length", e.Message);
                }
            }

            using (var str = new StringReader("\"1999W1\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Unexpected date string length", e.Message);
                }
            }

            using (var str = new StringReader("\"1999-W00\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected week to be between 01 and 53", e.Message);
                }
            }

            using (var str = new StringReader("\"1999W00\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected week to be between 01 and 53", e.Message);
                }
            }

            using (var str = new StringReader("\"1999-W54\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected week to be between 01 and 53", e.Message);
                }
            }

            using (var str = new StringReader("\"1999W54\""))
            {
                try
                {
                    JSON.Deserialize<DateTime>(str, Options.ISO8601);
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("Expected week to be between 01 and 53", e.Message);
                }
            }
        }

        [TestMethod]
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
                Assert.IsTrue(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.Deserialize<DateTime>(str, Options.SecondsSinceUnixEpoch);
                var delta = (nowRet - now).Duration().TotalSeconds;
                Assert.IsTrue(delta < 1);
            }
        }

        [TestMethod]
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
                Assert.IsTrue(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.Deserialize<DateTime>(str, Options.MillisecondsSinceUnixEpoch);
                var delta = (nowRet - now).Duration().TotalMilliseconds;
                Assert.IsTrue(delta < 1);
            }
        }

        [TestMethod]
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
                Assert.IsTrue(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.Deserialize<DateTime>(str);
                var delta = (nowRet - now).Duration().TotalMilliseconds;
                Assert.IsTrue(delta < 1);
            }
        }

        [TestMethod]
        public void NewtonsoftDateTimesWithTimeZones()
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
                    Assert.IsTrue(asStr.Contains('-') || asStr.Contains('+'));
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

                Assert.IsTrue((dtUtc - shouldMatchUtc).Duration().TotalMilliseconds < 1);
                Assert.IsTrue((dtUtc - jilDtUtc).Duration().TotalMilliseconds < 1);
                Assert.IsTrue((shouldMatchUtc - jilDtUtc).Duration().TotalMilliseconds == 0);
            }
        }

        [TestMethod]
        public void Guids()
        {
            var guid = Guid.NewGuid();

            using (var str = new StringReader("\"" + guid.ToString("d").ToUpper() + "\""))
            {
                var g = JSON.Deserialize<Guid>(str);
                Assert.AreEqual(guid, g);
            }

            using (var str = new StringReader("\"" + guid.ToString("d").ToLower() + "\""))
            {
                var g = JSON.Deserialize<Guid>(str);
                Assert.AreEqual(guid, g);
            }
        }

        [TestMethod]
        public void Overflow()
        {
            // byte
            {
                try
                {
                    using (var str = new StringReader("1234"))
                    {
                        JSON.Deserialize<byte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("257"))
                    {
                        JSON.Deserialize<byte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // sbyte
            {
                try
                {
                    using (var str = new StringReader("1234"))
                    {
                        JSON.Deserialize<sbyte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("128"))
                    {
                        JSON.Deserialize<sbyte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-129"))
                    {
                        JSON.Deserialize<sbyte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // short
            {
                try
                {
                    using (var str = new StringReader("320000"))
                    {
                        JSON.Deserialize<short>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("32768"))
                    {
                        JSON.Deserialize<short>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-32769"))
                    {
                        JSON.Deserialize<short>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // ushort
            {
                try
                {
                    using (var str = new StringReader("320000"))
                    {
                        JSON.Deserialize<ushort>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("65536"))
                    {
                        JSON.Deserialize<ushort>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // int
            {
                try
                {
                    using (var str = new StringReader("21474830000"))
                    {
                        JSON.Deserialize<int>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("2147483648"))
                    {
                        JSON.Deserialize<int>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-2147483649"))
                    {
                        JSON.Deserialize<int>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // uint
            {
                try
                {
                    using (var str = new StringReader("42949670000"))
                    {
                        JSON.Deserialize<uint>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("4294967296"))
                    {
                        JSON.Deserialize<uint>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // long
            {
                try
                {
                    using (var str = new StringReader("92233720368547750000"))
                    {
                        JSON.Deserialize<long>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("9223372036854775808"))
                    {
                        JSON.Deserialize<long>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-9223372036854775809"))
                    {
                        JSON.Deserialize<long>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // ulong
            {
                try
                {
                    using (var str = new StringReader("184467440737095510000"))
                    {
                        JSON.Deserialize<ulong>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("18446744073709551616"))
                    {
                        JSON.Deserialize<ulong>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (DeserializationException e)
                {
                    Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }
        }

        [TestMethod]
        public void Decimals()
        {
            for (var i = -11.1m; i <= 22.2m; i += 0.03m)
            {
                var asStr = i.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<decimal>(str);

                    Assert.AreEqual(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<decimal>(str);

                Assert.AreEqual(0m, res);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<decimal>(str);

                Assert.AreEqual(0m, res);
                Assert.AreEqual(-1, str.Peek());
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

                    Assert.AreEqual(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(-1, str.Peek());
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
            string sep = ".";
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

                    Assert.AreEqual(decimal.Parse(i, NumberStyles.Float, CultureInfo.InvariantCulture), res);
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        [TestMethod]
        public void Doubles()
        {
            for (var i = -11.1; i <= 22.2; i += 0.03)
            {
                var asStr = i.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<double>(str);

                    Assert.AreEqual(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<double>(str);

                Assert.AreEqual(0.0, res);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<double>(str);

                Assert.AreEqual(0.0, res);
                Assert.AreEqual(-1, str.Peek());
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

                    Assert.AreEqual(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(-1, str.Peek());
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
            string sep = ".";
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

                    Assert.AreEqual(double.Parse(i, CultureInfo.InvariantCulture), res);
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        [TestMethod]
        public void Floats()
        {
            for (var i = -11.1f; i <= 22.2f; i += 0.03f)
            {
                var asStr = i.ToString(CultureInfo.InvariantCulture);
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<float>(str);

                    Assert.AreEqual(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<float>(str);

                Assert.AreEqual(0f, res);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<float>(str);

                Assert.AreEqual(0f, res);
                Assert.AreEqual(-1, str.Peek());
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

                    Assert.AreEqual(asStr, res.ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(-1, str.Peek());
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
            string sep = ".";
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

                    Assert.AreEqual(float.Parse(i, CultureInfo.InvariantCulture), res);
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        [TestMethod]
        public void Longs()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.AreEqual(0L, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(long.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.AreEqual(long.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(long.MinValue.ToString()))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.AreEqual(long.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void ULongs()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.AreEqual((ulong)0, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(ulong.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.AreEqual(ulong.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(ulong.MinValue.ToString()))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.AreEqual(ulong.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Ints()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.AreEqual(0, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(int.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.AreEqual(int.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(int.MinValue.ToString()))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.AreEqual(int.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void UInts()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.AreEqual((uint)0, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(uint.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.AreEqual(uint.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(uint.MinValue.ToString()))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.AreEqual(uint.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Shorts()
        {
            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<short>(str);

                    Assert.AreEqual((short)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<short>(str);

                        Assert.AreEqual((short)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((short)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((short)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void UShort()
        {
            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<ushort>(str);

                    Assert.AreEqual((ushort)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<ushort>(str);

                        Assert.AreEqual((ushort)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((ushort)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((ushort)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void Bytes()
        {
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<byte>(str);

                    Assert.AreEqual((byte)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<byte>(str);

                        Assert.AreEqual((byte)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((byte)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((byte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void SBytes()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<sbyte>(str);

                    Assert.AreEqual((sbyte)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<sbyte>(str);

                        Assert.AreEqual((sbyte)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((sbyte)i, b);
                        Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual((sbyte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void Strings()
        {
            using (var str = new StringReader("null"))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.IsNull(c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"a\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("a", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\\\\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\\", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\/\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("/", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\b", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\f\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\f", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\r\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\r", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\n\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\n", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\t\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\t", c);
                Assert.AreEqual(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asStr = "\"\\u" + i.ToString("X4") + "\"";

                using (var str = new StringReader(asStr))
                {
                    var c = JSON.Deserialize<string>(str);

                    var shouldBe = "" + (char)i;

                    Assert.AreEqual(shouldBe, c);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("\"abc\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("abc", s);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"abcd\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("abcd", s);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\\f\\t\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("\b\f\t", s);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\\f\\t\\r\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("\b\f\t\r", s);
                Assert.AreEqual(-1, str.Peek());
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

                        Assert.AreEqual(shouldBe, c);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void Chars()
        {
            using (var str = new StringReader("\"a\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('a', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\\\\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\\', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\/\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('/', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\b', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\f\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\f', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\r\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\r', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\n\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\n', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\t\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\t', c);
                Assert.AreEqual(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asStr = "\"\\u" + i.ToString("X4") + "\"";

                using (var str = new StringReader(asStr))
                {
                    var c = JSON.Deserialize<char>(str);

                    Assert.AreEqual(i, (int)c);
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        enum _Enums : int
        {
            Hello,
            World,
            Foo
        }

        [TestMethod]
        public void Enums()
        {
            using (var str = new StringReader("\"Hello\""))
            {
                var val = JSON.Deserialize<_Enums>(str);

                Assert.AreEqual(_Enums.Hello, val);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Nullables()
        {
            using (var str = new StringReader("1"))
            {
                var val = JSON.Deserialize<int?>(str);

                Assert.AreEqual((int?)1, val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<int?>(str);

                Assert.AreEqual((int?)null, val);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Bools()
        {
            using (var str = new StringReader("true"))
            {
                var val = JSON.Deserialize<bool>(str);

                Assert.IsTrue(val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("false"))
            {
                var val = JSON.Deserialize<bool>(str);

                Assert.IsFalse(val);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Lists()
        {
            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<List<int>>(str);

                Assert.IsNull(val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(0, val.Count);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(" [     ] "))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(0, val.Count);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[1]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(1, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[1,2]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(2, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(2, val[1]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[1,2,3]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(2, val[1]);
                Assert.AreEqual(3, val[2]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(" [ 1,2 ,3   ]    "))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(2, val[1]);
                Assert.AreEqual(3, val[2]);
                Assert.AreEqual(-1, str.Peek());
            }
        }

#pragma warning disable 0649
        class _Objects
        {
            public int A;
            public string B { get; set; }
        }
#pragma warning restore 0649

        [TestMethod]
        public void Objects()
        {
            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNull(val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(123, val.A);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\": \"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 456, \"B\": \"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\": \"hello\", \"A\": 456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\":\"hello\",\"A\":456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("   {  \"B\"    :   \"hello\"    ,    \"A\"   :   456   }  "))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void ObjectsSkipMembers()
        {
            using (var str = new StringReader("{\"C\":123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":123.456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }



            using (var str = new StringReader("{\"C\":-123.456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1E12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1E12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1E-12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1E-12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1.1E2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1.1E2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1.1E+2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1.1E+2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":\"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": []}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1,2]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1,2,3]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123, \"B\": 456}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123, \"B\": 456, \"C\": \"hello world\"}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": null, \"CC\": null}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [null], \"CC\": [null]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\":null}, \"CC\": {\"B\":null}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }
        }

#pragma warning disable 0649
        class _RecursiveObjects
        {
            public string A;
            public _RecursiveObjects B;
        }
#pragma warning restore 0649

        [TestMethod]
        public void RecursiveObjects()
        {
            using (var str = new StringReader("{\"A\": \"hello world\", \"B\": { \"A\": \"foo bar\", \"B\": {\"A\": \"fizz buzz\"}}}"))
            {
                var val = JSON.Deserialize<_RecursiveObjects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual("hello world", val.A);
                Assert.AreEqual("foo bar", val.B.A);
                Assert.AreEqual("fizz buzz", val.B.B.A);
                Assert.IsNull(val.B.B.B);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Dictionaries()
        {
            using (var str = new StringReader("{\"A\": 123}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(1, val.Count);
                Assert.AreEqual(123, val["A"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123, \"B\": 456}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(2, val.Count);
                Assert.AreEqual(123, val["A"]);
                Assert.AreEqual(456, val["B"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123, \"B\": 456, \"C\": 789}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(123, val["A"]);
                Assert.AreEqual(456, val["B"]);
                Assert.AreEqual(789, val["C"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": null}"))
            {
                var val = JSON.Deserialize<Dictionary<string, string>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(1, val.Count);
                Assert.IsNull(val["A"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": \"hello\", \"B\": \"world\"}"))
            {
                var val = JSON.Deserialize<Dictionary<string, string>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(2, val.Count);
                Assert.AreEqual("hello", val["A"]);
                Assert.AreEqual("world", val["B"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": {\"A\":123}, \"B\": {\"B\": \"abc\"}, \"C\": {\"A\":456, \"B\":\"fizz\"} }"))
            {
                var val = JSON.Deserialize<Dictionary<string, _Objects>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(123, val["A"].A);
                Assert.AreEqual("abc", val["B"].B);
                Assert.AreEqual(456, val["C"].A);
                Assert.AreEqual("fizz", val["C"].B);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void ISO8601WeekDates()
        {
            using (var str = new StringReader("\"2009-W01-1\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(2008, 12, 29, 0, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2009W011\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(2008, 12, 29, 0, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2009-W53-7\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(2010, 1, 3, 0, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2009W537\""))
            {
                var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);
                Assert.AreEqual(new DateTime(2010, 1, 3, 0, 0, 0, DateTimeKind.Utc), dt);
            }
        }

        [TestMethod]
        public void Surrogates()
        {
            var data = "abc" + Char.ConvertFromUtf32(Int32.Parse("2A601", System.Globalization.NumberStyles.HexNumber)) + "def";

            Assert.IsTrue(data.Any(c => char.IsHighSurrogate(c)));
            Assert.IsTrue(data.Any(c => char.IsLowSurrogate(c)));

            using (var str = new StringReader("\"" + data + "\""))
            {
                var res = JSON.Deserialize<string>(str);
                Assert.AreEqual(data, res);
            }
        }

        class _HashEscapedSequences
        {
            public string Hello { get; set; }
            public string World { get; set; }
        }

        [TestMethod]
        public void HashedEscapedSequences()
        {
            Assert.IsTrue(Jil.Deserialize.MemberMatcher<_HashEscapedSequences>.IsAvailable);

            // Hello
            {
                using (var str = new StringReader("{\"\\u0048ello\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"H\\u0065llo\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"He\\u006Clo\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"He\\u006clo\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hel\\u006Co\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hel\\u006co\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hell\\u006F\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hell\\u006f\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"\\u0048\\u0065\\u006C\\u006C\\u006F\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"\\u0048\\u0065\\u006c\\u006c\\u006f\":\"foo\",\"World\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }
            }

            // World
            {
                using (var str = new StringReader("{\"Hello\":\"foo\",\"\\u0057orld\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hello\":\"foo\",\"W\\u006Frld\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hello\":\"foo\",\"W\\u006frld\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hello\":\"foo\",\"Wo\\u0072ld\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hello\":\"foo\",\"Wor\\u006Cd\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hello\":\"foo\",\"Worl\\u0064\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hello\":\"foo\",\"\\u0057\\u006F\\u0072\\u006C\\u0064\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }

                using (var str = new StringReader("{\"Hello\":\"foo\",\"\\u0057\\u006f\\u0072\\u006c\\u0064\":\"bar\"}"))
                {
                    var res = JSON.Deserialize<_HashEscapedSequences>(str);
                    Assert.IsNotNull(res);
                    Assert.AreEqual("foo", res.Hello);
                    Assert.AreEqual("bar", res.World);
                }
            }
        }

        List<T> AnonObjectByExample<T>(T example, string str, bool allowHashing)
        {
            var opts = new Options(allowHashFunction: allowHashing, dateFormat: Jil.DateTimeFormat.ISO8601);
            return JSON.Deserialize<List<T>>(str, opts);
        }

        [TestMethod]
        public void AnonNulls()
        {
            {
                var example = new { A = 1 };

                var a = AnonObjectByExample(example, "[null, {\"A\":1234}, null]", false);
                Assert.AreEqual(3, a.Count);
                Assert.IsNull(a[0]);
                Assert.IsNotNull(a[1]);
                Assert.AreEqual(1234, a[1].A);
                Assert.IsNull(a[2]);
            }

            {
                var example = new { A = 1 };

                var a = AnonObjectByExample(example, "[null, {\"A\":1234}, null]", true);
                Assert.AreEqual(3, a.Count);
                Assert.IsNull(a[0]);
                Assert.IsNotNull(a[1]);
                Assert.AreEqual(1234, a[1].A);
                Assert.IsNull(a[2]);
            }
        }

        [TestMethod]
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

                var res = AnonObjectByExample(example, str, false);
                Assert.IsNotNull(res);
                Assert.AreEqual(3, res.Count);
                var first = res[0];
                Assert.AreEqual(1234, first.A);
                Assert.AreEqual(123.45, first.B);
                Assert.AreEqual(678.90f, first.C);
                Assert.AreEqual(0m, first.D);
                Assert.AreEqual("hello world", first.E);
                Assert.AreEqual('c', first.F);
                Assert.AreEqual(Guid.Parse("EB29803F-A68D-4647-8512-5F0EE906CC90"), first.G);
                Assert.AreEqual(new DateTime(1999, 12, 31, 0, 0, 0, DateTimeKind.Utc), first.H);
                Assert.IsNotNull(first.I);
                Assert.AreEqual(10, first.I.Length);

                for (var i = 0; i < 10; i++)
                {
                    Assert.AreEqual(i + 1, first.I[i]);
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

                var res = AnonObjectByExample(example, str, false);
                Assert.IsNotNull(res);
                Assert.AreEqual(1, res.Count);
                var first = res[0];
                Assert.AreEqual(1, first.A);
                Assert.AreEqual(2, first.B);
                Assert.AreEqual(3, first.C);
                Assert.AreEqual(0, first.D);
                Assert.AreEqual(4, first.E);
                Assert.AreEqual(5, first.F);
                Assert.AreEqual(6, first.G);
                Assert.AreEqual(7, first.H);
                Assert.AreEqual(8, first.I);
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

                var res = AnonObjectByExample(example, str, true);
                Assert.IsNotNull(res);
                Assert.AreEqual(3, res.Count);
                var first = res[0];
                Assert.AreEqual(1234, first.A);
                Assert.AreEqual(123.45, first.B);
                Assert.AreEqual(678.90f, first.C);
                Assert.AreEqual(0m, first.D);
                Assert.AreEqual("hello world", first.E);
                Assert.AreEqual('c', first.F);
                Assert.AreEqual(Guid.Parse("EB29803F-A68D-4647-8512-5F0EE906CC90"), first.G);
                Assert.AreEqual(new DateTime(1999, 12, 31, 0, 0, 0, DateTimeKind.Utc), first.H);
                Assert.IsNotNull(first.I);
                Assert.AreEqual(10, first.I.Length);

                for (var i = 0; i < 10; i++)
                {
                    Assert.AreEqual(i + 1, first.I[i]);
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

                var res = AnonObjectByExample(example, str, true);
                Assert.IsNotNull(res);
                Assert.AreEqual(1, res.Count);
                var first = res[0];
                Assert.AreEqual(1, first.A);
                Assert.AreEqual(2, first.B);
                Assert.AreEqual(3, first.C);
                Assert.AreEqual(0, first.D);
                Assert.AreEqual(4, first.E);
                Assert.AreEqual(5, first.F);
                Assert.AreEqual(6, first.G);
                Assert.AreEqual(7, first.H);
                Assert.AreEqual(8, first.I);
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

        [TestMethod]
        public void DataMemberName()
        {
            using (var str = new StringReader("{\"NotSoSecretName\":314159,\"FakeName\":\"Really RealName\",\"Plain\":\"hello world\"}"))
            {
                var obj = JSON.Deserialize<_DataMemberName>(str);

                Assert.IsNotNull(obj);
                Assert.AreEqual("hello world", obj.Plain);
                Assert.AreEqual("Really RealName", obj.RealName);
                Assert.AreEqual(314159, obj.SecretName);
            }
        }

        static T _EmptyAnonymousObject<T>(T example, string str, Options opts)
        {
            return JSON.Deserialize<T>(str, opts);
        }

        [TestMethod]
        public void EmptyAnonymousObject()
        {
            var ex = new { };

            {
                {
                    var obj = _EmptyAnonymousObject(ex, "null", Options.Default);
                    Assert.IsNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{}", Options.Default);
                    Assert.IsNotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234}", Options.Default);
                    Assert.IsNotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234,\"B\":5678}", Options.Default);
                    Assert.IsNotNull(obj);
                }
            }

            {
                {
                    var obj = _EmptyAnonymousObject(ex, "null", new Options(allowHashFunction: false));
                    Assert.IsNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{}", new Options(allowHashFunction: false));
                    Assert.IsNotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234}", new Options(allowHashFunction: false));
                    Assert.IsNotNull(obj);
                }

                {
                    var obj = _EmptyAnonymousObject(ex, "{\"A\":1234,\"B\":5678}", new Options(allowHashFunction: false));
                    Assert.IsNotNull(obj);
                }
            }
        }

        [TestMethod]
        public void SystemObject()
        {
            {
                using (var str = new StringReader("null"))
                {
                    var obj = JSON.Deserialize<object>(str);

                    Assert.IsNull(obj);
                }

                using (var str = new StringReader("{}"))
                {
                    var obj = JSON.Deserialize<object>(str);

                    Assert.IsNotNull(obj);
                }

                using (var str = new StringReader("{\"A\":1234}"))
                {
                    var obj = JSON.Deserialize<object>(str);

                    Assert.IsNotNull(obj);
                }
            }

            {
                using (var str = new StringReader("null"))
                {
                    var obj = JSON.Deserialize<object>(str, new Options(allowHashFunction: false));

                    Assert.IsNull(obj);
                }

                using (var str = new StringReader("{}"))
                {
                    var obj = JSON.Deserialize<object>(str, new Options(allowHashFunction: false));

                    Assert.IsNotNull(obj);
                }

                using (var str = new StringReader("{\"A\":1234}"))
                {
                    var obj = JSON.Deserialize<object>(str, new Options(allowHashFunction: false));

                    Assert.IsNotNull(obj);
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

        [TestMethod]
        public void MissingConstructor()
        {
            try
            {
                JSON.Deserialize<_MissingConstructor>("null");
                Assert.Fail();
            }
            catch (DeserializationException e)
            {
                Assert.AreEqual("Error occurred building a deserializer for JilTests.DeserializeTests+_MissingConstructor: Expected a parameterless constructor for JilTests.DeserializeTests+_MissingConstructor", e.Message);
                Assert.IsInstanceOfType(e.InnerException, typeof(Jil.Common.ConstructionException));
            }

            try
            {
                JSON.Deserialize<_MissingConstructor>("null", new Options(allowHashFunction: false));
                Assert.Fail();
            }
            catch (DeserializationException e)
            {
                Assert.AreEqual("Error occurred building a deserializer for JilTests.DeserializeTests+_MissingConstructor: Expected a parameterless constructor for JilTests.DeserializeTests+_MissingConstructor", e.Message);
                Assert.IsInstanceOfType(e.InnerException, typeof(Jil.Common.ConstructionException));
            }
        }

        class _NoNameDataMember
        {
            [DataMember(Order = 1)]
            public int Id { get; set; }
        }

        [TestMethod]
        public void NoNameDataMember()
        {
            using (var str = new StringReader("{\"Id\":1234}"))
            {
                var res = JSON.Deserialize<_NoNameDataMember>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(1234, res.Id);
            }
        }

        [TestMethod]
        public void BadDouble()
        {
            using (var str = new StringReader("1.2E10E10"))
            {
                try
                {
                    JSON.Deserialize<double>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.AreEqual("E10", rest);
                }
            }

            using (var str = new StringReader("1.2.3.4.5.6"))
            {
                try
                {
                    JSON.Deserialize<double>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.AreEqual(".3.4.5.6", rest);
                }
            }

            using (var str = new StringReader("1.2E++10"))
            {
                try
                {
                    JSON.Deserialize<double>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.AreEqual("+10", rest);
                }
            }
        }

        [TestMethod]
        public void BadFloat()
        {
            using (var str = new StringReader("1.2E10E10"))
            {
                try
                {
                    JSON.Deserialize<float>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.IsTrue(rest.Length > 0);
                }
            }

            using (var str = new StringReader("1.2.3.4.5.6"))
            {
                try
                {
                    JSON.Deserialize<float>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.IsTrue(rest.Length > 0);
                }
            }

            using (var str = new StringReader("1.2E++10"))
            {
                try
                {
                    JSON.Deserialize<float>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.IsTrue(rest.Length > 0);
                }
            }
        }

        [TestMethod]
        public void BadDecimal()
        {
            using (var str = new StringReader("1.2E10E10"))
            {
                try
                {
                    JSON.Deserialize<decimal>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.AreEqual("E10", rest);
                }
            }

            using (var str = new StringReader("1.2.3.4.5.6"))
            {
                try
                {
                    JSON.Deserialize<decimal>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.AreEqual(".3.4.5.6", rest);
                }
            }

            using (var str = new StringReader("1.2E++10"))
            {
                try
                {
                    JSON.Deserialize<decimal>(str);
                    Assert.Fail();
                }
                catch (DeserializationException e)
                {
                    var rest = e.SnippetAfterError;
                    Assert.AreEqual("+10", rest);
                }
            }
        }

        class _IEnumerableMember
        {
            public IEnumerable<string> A { get; set; }
        }

        [TestMethod]
        public void IEnumerableMember()
        {
            using (var str = new StringReader("{\"A\":[\"abcd\", \"efgh\"]}"))
            {
                var res = JSON.Deserialize<_IEnumerableMember>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(2, res.A.Count());
                Assert.AreEqual("abcd", res.A.ElementAt(0));
                Assert.AreEqual("efgh", res.A.ElementAt(1));
            }
        }

        [TestMethod]
        public void Interface()
        {
            using (var str = new StringReader("{\"A\":1234, \"B\": \"hello world\"}"))
            {
                var res = JSON.Deserialize<_Interface1>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(1234, res.A);
                Assert.AreEqual("hello world", res.B);
            }

            using (var str = new StringReader("{\"A\":1234, \"B\": \"hello world\", \"C\": 3.14159}"))
            {
                var res = JSON.Deserialize<_Interface2>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(1234, res.A);
                Assert.AreEqual("hello world", res.B);
                Assert.AreEqual(3.14159, res.C);
            }

            using (var str = new StringReader("{\"A\":1234, \"B\":4567, \"C\":890}"))
            {
                var res = JSON.Deserialize<_Interface3>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(1234, res.A);
                Assert.AreEqual(890, res.C);
            }
        }

        class _Issue19
        {
            public bool B { get; set; }
        }

        [TestMethod]
        public void Issue19()
        {
            using (var str = new StringReader("{\"A\":true}"))
            {
                var res = JSON.Deserialize<_Issue19>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(false, res.B);
            }

            using (var str = new StringReader("{\"A\":true, \"B\":false}"))
            {
                var res = JSON.Deserialize<_Issue19>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(false, res.B);
            }

            using (var str = new StringReader("{\"A\":false, \"B\":true}"))
            {
                var res = JSON.Deserialize<_Issue19>(str);
                Assert.IsNotNull(res);
                Assert.AreEqual(true, res.B);
            }
        }

        [Flags]
        enum _FlagsEnum
        {
            A = 1,
            B = 2,
            C = 4
        }

        [TestMethod]
        public void FlagsEnum()
        {
            Assert.AreEqual(_FlagsEnum.A, JSON.Deserialize<_FlagsEnum>("\"A\""));
            Assert.AreEqual(_FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"B\""));
            Assert.AreEqual(_FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"A, B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"A,B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"B, A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B, JSON.Deserialize<_FlagsEnum>("\"B,A\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,A\""));

            Assert.AreEqual(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, C\""));
            Assert.AreEqual(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,C\""));
            Assert.AreEqual(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, B\""));
            Assert.AreEqual(_FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,B\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, B, C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, B,C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,B, C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,B,C\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, C, B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A, C,B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,C, B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"A,C,B\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, A, C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, A,C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,A, C\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,A,C\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, C, A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B, C,A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,C, A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"B,C,A\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, A, B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, A,B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,A, B\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,A,B\""));

            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, B, A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C, B,A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,B, A\""));
            Assert.AreEqual(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, JSON.Deserialize<_FlagsEnum>("\"C,B,A\""));
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

        [TestMethod]
        public void EnumMemberAttributeOverride()
        {
            Assert.AreEqual(_EnumMemberAttributeOverride.A, JSON.Deserialize<_EnumMemberAttributeOverride>("\"1\""));
            Assert.AreEqual(_EnumMemberAttributeOverride.B, JSON.Deserialize<_EnumMemberAttributeOverride>("\"2\""));
            Assert.AreEqual(_EnumMemberAttributeOverride.C, JSON.Deserialize<_EnumMemberAttributeOverride>("\"4\""));
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

        [TestMethod]
        public void EnumMemberAttributeOverrideFlags()
        {
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3\""));

            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,1\""));

            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,1\""));

            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,2\""));

            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 2, 3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 2,3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,2, 3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,2,3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1, 3, 2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,3, 2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"1,3,2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 1, 3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 1,3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,1, 3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,1,3\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 3, 1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2, 3,1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,3, 1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"2,3,1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 1, 2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 1,2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,1, 2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,1,2\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 2, 1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3, 2,1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,2, 1\""));
            Assert.AreEqual(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, JSON.Deserialize<_EnumMemberAttributeOverrideFlags>("\"3,2,1\""));
        }

        enum _CaseInsensitiveEnums
        {
            Foo,
            Bar,
            Fizz
        }

        [TestMethod]
        public void CaseInsensitiveEnums()
        {
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"foo\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"Foo\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"fOo\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"foO\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"FOo\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"FoO\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"fOO\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Foo, JSON.Deserialize<_CaseInsensitiveEnums>("\"FOO\""));

            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"bar\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"Bar\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"bAr\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"baR\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"BAr\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"BaR\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"bAR\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Bar, JSON.Deserialize<_CaseInsensitiveEnums>("\"BAR\""));

            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fizz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"Fizz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIzz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fiZz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fizZ\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIzz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FiZz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FizZ\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIZz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIzZ\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fiZZ\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIZz\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIzZ\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"fIZZ\""));
            Assert.AreEqual(_CaseInsensitiveEnums.Fizz, JSON.Deserialize<_CaseInsensitiveEnums>("\"FIZZ\""));
        }

        [TestMethod]
        public void DynamicMembers()
        {
            var json = @"{
                  ""index.analysis.analyzer.stem.tokenizer"" : ""standard"",
                  ""index.analysis.analyzer.exact.filter.0"" : ""lowercase"",
                  ""index.refresh_interval"" : ""1s"",
                  ""index.analysis.analyzer.exact.type"" : ""custom"",
                  ""test-dummy-obj"": { ""hello"": 123 }
	        }";

            var dyn = JSON.Deserialize<Dictionary<string, dynamic>>(json);
            Assert.IsNotNull(dyn);
            Assert.AreEqual(5, dyn.Count);
            Assert.AreEqual("standard", (string)dyn["index.analysis.analyzer.stem.tokenizer"]);
            Assert.AreEqual("lowercase", (string)dyn["index.analysis.analyzer.exact.filter.0"]);
            Assert.AreEqual("1s", (string)dyn["index.refresh_interval"]);
            Assert.AreEqual("custom", (string)dyn["index.analysis.analyzer.exact.type"]);
            Assert.IsNotNull(dyn["test-dummy-obj"]);
            var testDummyObj = dyn["test-dummy-obj"];

            var count = 0;
            foreach (var kv in testDummyObj)
            {
                var key = kv.Key;
                var val = kv.Value;
                count++;

                Assert.AreEqual("hello", (string)key);
                Assert.AreEqual(123, (int)val);
            }

            Assert.AreEqual(1, count);
        }

        class _Issue25
        {
            public int Id { get; set; }
            public __Issue25 Foo { get; set; }
        }

        class __Issue25 {  /* nothing here .. yet */ }

        static T Issue25DeserializeByExample<T>(T example, string json, Options opts)
        {
            return JSON.Deserialize<T>(json, opts);
        }

        [TestMethod]
        public void Issue25()
        {
            var json = "{ \"Id\" : 17, \"Foo\" : { \"Bar\" : 17} }";

            {
                var res = JSON.Deserialize<_Issue25>(json);

                Assert.AreEqual(17, res.Id);
                Assert.IsNotNull(res.Foo);
            }

            {
                var res = JSON.Deserialize<_Issue25>(json, new Options(allowHashFunction: false));

                Assert.AreEqual(17, res.Id);
                Assert.IsNotNull(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = Issue25DeserializeByExample(example, json, Options.Default);

                Assert.AreEqual(17, res.Id);
                Assert.IsNotNull(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = Issue25DeserializeByExample(example, json, new Options(allowHashFunction: false));

                Assert.AreEqual(17, res.Id);
                Assert.IsNotNull(res.Foo);
            }
        }

        class _EmptyMembers { }

        [TestMethod]
        public void EmptyMembers()
        {
            {
                var str1 = "{}";
                var str2 = "{\"foo\":0}";
                var str3 = "{\"foo\":0, \"bar\":0}";
                var str4 = "{\"foo\":0, \"bar\":0, \"fizz\":0}";

                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str1));
                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str2));
                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str3));
                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str4));
            }

            {
                var str1 = "{}";
                var str2 = "{\"foo\":0}";
                var str3 = "{\"foo\":0, \"bar\":0}";
                var str4 = "{\"foo\":0, \"bar\":0, \"fizz\":0}";

                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str1, new Options(allowHashFunction: false)));
                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str2, new Options(allowHashFunction: false)));
                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str3, new Options(allowHashFunction: false)));
                Assert.IsNotNull(JSON.Deserialize<_EmptyMembers>(str4, new Options(allowHashFunction: false)));
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

        [TestMethod]
        public void NullEmptyMembers()
        {
            var json = "{ \"Id\" : 17, \"Foo\" : null }";

            {
                var res = JSON.Deserialize<_NullEmptyMembers>(json);

                Assert.AreEqual(17, res.Id);
                Assert.IsNull(res.Foo);
            }

            {
                var res = JSON.Deserialize<_NullEmptyMembers>(json, new Options(allowHashFunction: false));

                Assert.AreEqual(17, res.Id);
                Assert.IsNull(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = NullEmptyMembersDeserializeByExample(example, json, Options.Default);

                Assert.AreEqual(17, res.Id);
                Assert.IsNull(res.Foo);
            }

            {
                var example = new { Id = 17, Foo = new { } };
                var res = NullEmptyMembersDeserializeByExample(example, json, new Options(allowHashFunction: false));

                Assert.AreEqual(17, res.Id);
                Assert.IsNull(res.Foo);
            }
        }

        [TestMethod]
        public void DateTimeOffsets()
        {
            // ISO8601
            using (var str = new StringReader("\"1900-01-01 12:30Z\""))
            {
                var dto = new DateTimeOffset(1900, 1, 1, 12, 30, 0, TimeSpan.Zero);
                var res = JSON.Deserialize<DateTimeOffset>(str, Options.ISO8601);
                Assert.AreEqual(dto, res);
            }

            // Newtsonsoft
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
                    Assert.IsTrue(delta < TimeSpan.FromMilliseconds(1));
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
                    Assert.IsTrue(delta < TimeSpan.FromMilliseconds(1));
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
                    Assert.IsTrue(delta < TimeSpan.FromSeconds(1));
                }
            }
        }

        [TestMethod]
        public void Issue43()
        {
            var shouldMatch = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
            shouldMatch = new DateTime(shouldMatch.Ticks + 5339, DateTimeKind.Utc);
            var dt = JSON.Deserialize<DateTime>("\"2014-08-08T14:04:01.4265339+00:00\"", Options.ISO8601);
            Assert.AreEqual(shouldMatch, dt);
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

        [TestMethod]
        public void SlowSpinUp()
        {
            var json = @"{""cluster_name"":""ml-elastic-cluster"",""nodes"":{""CHtYMjlNRJWGzVGGhxxN-w"":{""name"":""Jimmy Woo"",""transport_address"":""inet[/10.7.3.182:9300]"",""host"":""ny-mlelastic03.ds.stackexchange.com"",""ip"":""10.7.3.182"",""version"":""1.2.1"",""build"":""6c95b75"",""http_address"":""inet[/10.7.3.182:9200]"",""settings"":{""path"":{""data"":""/cassandra/elasticsearch/data"",""work"":""/tmp/elasticsearch"",""home"":""/usr/share/elasticsearch"",""conf"":""/etc/elasticsearch"",""logs"":""/cassandra/elasticsearch/log""},""pidfile"":""/var/run/elasticsearch/elasticsearch.pid"",""cluster"":{""name"":""ml-elastic-cluster""},""script"":{""disable_dynamic"":""false""},""discovery"":{""zen"":{""ping"":{""unicast"":{""hosts"":[""ny-mlelastic01"",""ny-mlelastic02"",""ny-mlelastic03""]},""multicast"":{""enabled"":""false""}}}},""name"":""Jimmy Woo""},""os"":{""refresh_interval_in_millis"":1000,""available_processors"":32,""cpu"":{""vendor"":""Intel"",""model"":""Xeon"",""mhz"":2600,""total_cores"":32,""total_sockets"":1,""cores_per_socket"":32,""cache_size_in_bytes"":20480},""mem"":{""total_in_bytes"":101392883712},""swap"":{""total_in_bytes"":137438945280}},""process"":{""refresh_interval_in_millis"":1000,""id"":36752,""max_file_descriptors"":65535,""mlockall"":false},""jvm"":{""pid"":36752,""version"":""1.7.0_51"",""vm_name"":""Java HotSpot(TM) 64-Bit Server VM"",""vm_version"":""24.51-b03"",""vm_vendor"":""Oracle Corporation"",""start_time_in_millis"":1403026987431,""mem"":{""heap_init_in_bytes"":268435456,""heap_max_in_bytes"":1037959168,""non_heap_init_in_bytes"":24313856,""non_heap_max_in_bytes"":136314880,""direct_max_in_bytes"":1037959168},""gc_collectors"":[""ParNew"",""ConcurrentMarkSweep""],""memory_pools"":[""Code Cache"",""Par Eden Space"",""Par Survivor Space"",""CMS Old Gen"",""CMS Perm Gen""]},""thread_pool"":{""generic"":{""type"":""cached"",""keep_alive"":""30s""},""index"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""200""},""snapshot_data"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""bench"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""get"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""snapshot"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""merge"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""suggest"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""bulk"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""50""},""optimize"":{""type"":""fixed"",""min"":1,""max"":1},""warmer"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""flush"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""search"":{""type"":""fixed"",""min"":96,""max"":96,""queue_size"":""1k""},""percolate"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""management"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""refresh"":{""type"":""scaling"",""min"":1,""max"":10,""keep_alive"":""5m""}},""network"":{""refresh_interval_in_millis"":5000,""primary_interface"":{""address"":""10.7.3.182"",""name"":""em4"",""mac_address"":""B8:CA:3A:70:A8:3D""}},""transport"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9300]"",""publish_address"":""inet[/10.7.3.182:9300]""},""http"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9200]"",""publish_address"":""inet[/10.7.3.182:9200]"",""max_content_length_in_bytes"":104857600},""plugins"":[]},""y2D2HmdTTIGSD8zc74Oz7g"":{""name"":""Tony Stark"",""transport_address"":""inet[/10.7.3.181:9300]"",""host"":""ny-mlelastic02.ds.stackexchange.com"",""ip"":""10.7.3.181"",""version"":""1.2.1"",""build"":""6c95b75"",""http_address"":""inet[/10.7.3.181:9200]"",""settings"":{""path"":{""data"":""/cassandra/elasticsearch/data"",""work"":""/tmp/elasticsearch"",""home"":""/usr/share/elasticsearch"",""conf"":""/etc/elasticsearch"",""logs"":""/cassandra/elasticsearch/log""},""pidfile"":""/var/run/elasticsearch/elasticsearch.pid"",""cluster"":{""name"":""ml-elastic-cluster""},""script"":{""disable_dynamic"":""false""},""discovery"":{""zen"":{""ping"":{""unicast"":{""hosts"":[""ny-mlelastic01"",""ny-mlelastic02"",""ny-mlelastic03""]},""multicast"":{""enabled"":""false""}}}},""name"":""Tony Stark""},""os"":{""refresh_interval_in_millis"":1000,""available_processors"":32,""cpu"":{""vendor"":""Intel"",""model"":""Xeon"",""mhz"":2600,""total_cores"":32,""total_sockets"":1,""cores_per_socket"":32,""cache_size_in_bytes"":20480},""mem"":{""total_in_bytes"":101392883712},""swap"":{""total_in_bytes"":137438945280}},""process"":{""refresh_interval_in_millis"":1000,""id"":13069,""max_file_descriptors"":65535,""mlockall"":false},""jvm"":{""pid"":13069,""version"":""1.7.0_51"",""vm_name"":""Java HotSpot(TM) 64-Bit Server VM"",""vm_version"":""24.51-b03"",""vm_vendor"":""Oracle Corporation"",""start_time_in_millis"":1403026959588,""mem"":{""heap_init_in_bytes"":268435456,""heap_max_in_bytes"":1037959168,""non_heap_init_in_bytes"":24313856,""non_heap_max_in_bytes"":136314880,""direct_max_in_bytes"":1037959168},""gc_collectors"":[""ParNew"",""ConcurrentMarkSweep""],""memory_pools"":[""Code Cache"",""Par Eden Space"",""Par Survivor Space"",""CMS Old Gen"",""CMS Perm Gen""]},""thread_pool"":{""generic"":{""type"":""cached"",""keep_alive"":""30s""},""index"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""200""},""snapshot_data"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""bench"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""get"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""snapshot"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""merge"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""suggest"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""bulk"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""50""},""optimize"":{""type"":""fixed"",""min"":1,""max"":1},""warmer"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""flush"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""search"":{""type"":""fixed"",""min"":96,""max"":96,""queue_size"":""1k""},""percolate"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""management"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""refresh"":{""type"":""scaling"",""min"":1,""max"":10,""keep_alive"":""5m""}},""network"":{""refresh_interval_in_millis"":5000,""primary_interface"":{""address"":""10.7.3.181"",""name"":""em4"",""mac_address"":""B8:CA:3A:70:AB:6D""}},""transport"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9300]"",""publish_address"":""inet[/10.7.3.181:9300]""},""http"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0%0:9200]"",""publish_address"":""inet[/10.7.3.181:9200]"",""max_content_length_in_bytes"":104857600},""plugins"":[]},""b3UvPCMmS-67jl6OWxFShw"":{""name"":""Invisible Girl"",""transport_address"":""inet[/10.7.3.180:9300]"",""host"":""ny-mlelastic01"",""ip"":""10.7.3.180"",""version"":""1.2.1"",""build"":""6c95b75"",""http_address"":""inet[/10.7.3.180:9200]"",""settings"":{""path"":{""data"":""/cassandra/elasticsearch/data"",""work"":""/tmp/elasticsearch"",""home"":""/usr/share/elasticsearch"",""conf"":""/etc/elasticsearch"",""logs"":""/cassandra/elasticsearch/log""},""pidfile"":""/var/run/elasticsearch/elasticsearch.pid"",""cluster"":{""name"":""ml-elastic-cluster""},""script"":{""disable_dynamic"":""false""},""discovery"":{""zen"":{""ping"":{""unicast"":{""hosts"":[""ny-mlelastic01"",""ny-mlelastic02"",""ny-mlelastic03""]},""multicast"":{""enabled"":""false""}}}},""name"":""Invisible Girl""},""os"":{""refresh_interval_in_millis"":1000,""available_processors"":32,""cpu"":{""vendor"":""Intel"",""model"":""Xeon"",""mhz"":2599,""total_cores"":32,""total_sockets"":1,""cores_per_socket"":32,""cache_size_in_bytes"":20480},""mem"":{""total_in_bytes"":101392883712},""swap"":{""total_in_bytes"":137438945280}},""process"":{""refresh_interval_in_millis"":1000,""id"":4764,""max_file_descriptors"":65535,""mlockall"":false},""jvm"":{""pid"":4764,""version"":""1.7.0_51"",""vm_name"":""Java HotSpot(TM) 64-Bit Server VM"",""vm_version"":""24.51-b03"",""vm_vendor"":""Oracle Corporation"",""start_time_in_millis"":1403026924332,""mem"":{""heap_init_in_bytes"":268435456,""heap_max_in_bytes"":1037959168,""non_heap_init_in_bytes"":24313856,""non_heap_max_in_bytes"":136314880,""direct_max_in_bytes"":1037959168},""gc_collectors"":[""ParNew"",""ConcurrentMarkSweep""],""memory_pools"":[""Code Cache"",""Par Eden Space"",""Par Survivor Space"",""CMS Old Gen"",""CMS Perm Gen""]},""thread_pool"":{""generic"":{""type"":""cached"",""keep_alive"":""30s""},""index"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""200""},""snapshot_data"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""bench"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""get"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""snapshot"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""merge"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""suggest"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""bulk"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""50""},""optimize"":{""type"":""fixed"",""min"":1,""max"":1},""warmer"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""flush"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""search"":{""type"":""fixed"",""min"":96,""max"":96,""queue_size"":""1k""},""percolate"":{""type"":""fixed"",""min"":32,""max"":32,""queue_size"":""1k""},""management"":{""type"":""scaling"",""min"":1,""max"":5,""keep_alive"":""5m""},""refresh"":{""type"":""scaling"",""min"":1,""max"":10,""keep_alive"":""5m""}},""network"":{""refresh_interval_in_millis"":5000,""primary_interface"":{""address"":""10.7.3.180"",""name"":""em4"",""mac_address"":""B8:CA:3A:70:D8:A5""}},""transport"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0:9300]"",""publish_address"":""inet[/10.7.3.180:9300]""},""http"":{""bound_address"":""inet[/0:0:0:0:0:0:0:0:9200]"",""publish_address"":""inet[/10.7.3.180:9200]"",""max_content_length_in_bytes"":104857600},""plugins"":[]}}}";

            var timer = new Stopwatch();
            timer.Start();
            var result = JSON.Deserialize<ClusterNodeInfo>(json, Options.SecondsSinceUnixEpochExcludeNulls);
            timer.Stop();

            Assert.IsTrue(timer.ElapsedMilliseconds < 1500, "Took: " + timer.ElapsedMilliseconds + "ms");
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

        //[TestMethod]
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

        //                Assert.IsTrue(closeEnough, "For i=" + i + " format=" + format + " delta=" + delta + " epsilon=" + float.Epsilon);
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
