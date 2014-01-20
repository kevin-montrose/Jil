﻿using Jil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JilTests
{
    [TestClass]
    public class SerializeTests
    {
        public class _SimpleObject
        {
            public int Foo;
        }

        [TestMethod]
        public void SimpleObject()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _SimpleObject { Foo = 123 }, str);

                var res = str.ToString();

                Assert.AreEqual("{\"Foo\":123}", res);
            }
        }

        [TestMethod]
        public void SimpleObject_ToString()
        {
            using (var str = new StringWriter())
            {
                var res = JSON.Serialize(new _SimpleObject { Foo = 123 });

                Assert.AreEqual("{\"Foo\":123}", res);
            }
        }

        public class _Cyclical
        {
            public int Foo;

            public _Cyclical Next;
        }

        [TestMethod]
        public void Cyclical()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _Cyclical { Foo = 123, Next = new _Cyclical { Foo = 456 } }, str);
                var res = str.ToString();
                Assert.AreEqual("{\"Foo\":123,\"Next\":{\"Foo\":456,\"Next\":null}}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new[] { new _Cyclical { Foo = 123, Next = new _Cyclical { Foo = 456 } }, new _Cyclical { Foo = 456 } }, str);
                var res = str.ToString();
                Assert.AreEqual("[{\"Foo\":123,\"Next\":{\"Foo\":456,\"Next\":null}},{\"Foo\":456,\"Next\":null}]", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new Dictionary<string, _Cyclical> { { "hello", new _Cyclical { Foo = 123, Next = new _Cyclical { Foo = 456 } } }, {"world", new _Cyclical { Foo = 456 } } }, str);
                var res = str.ToString();
                Assert.AreEqual("{\"hello\":{\"Foo\":123,\"Next\":{\"Foo\":456,\"Next\":null}},\"world\":{\"Foo\":456,\"Next\":null}}", res);
            }
        }

        [TestMethod]
        public void Primitive()
        {
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<byte>(123, str);

                    var res = str.ToString();

                    Assert.AreEqual("123", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<sbyte>(-123, str);

                    var res = str.ToString();

                    Assert.AreEqual("-123", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<short>(-1000, str);

                    var res = str.ToString();

                    Assert.AreEqual("-1000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ushort>(5000, str);

                    var res = str.ToString();

                    Assert.AreEqual("5000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<int>(-123, str);

                    var res = str.ToString();

                    Assert.AreEqual("-123", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<uint>(123456789, str);

                    var res = str.ToString();

                    Assert.AreEqual("123456789", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<long>(-5000000000, str);

                    var res = str.ToString();

                    Assert.AreEqual("-5000000000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ulong>(8000000000, str);

                    var res = str.ToString();

                    Assert.AreEqual("8000000000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<string>("hello world", str);

                    var res = str.ToString();

                    Assert.AreEqual("\"hello world\"", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<char>('c', str);

                    var res = str.ToString();

                    Assert.AreEqual("\"c\"", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<float>(1.234f, str);

                    var res = str.ToString();

                    Assert.AreEqual("1.234", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<double>(1.1, str);

                    var res = str.ToString();

                    Assert.AreEqual("1.1", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<decimal>(4.56m, str);

                    var res = str.ToString();

                    Assert.AreEqual("4.56", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<bool>(true, str);

                    var res = str.ToString();

                    Assert.AreEqual("true", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(new DateTime(1999, 1, 2, 3, 4, 5, 6, DateTimeKind.Utc), str);
                    Assert.AreEqual("\"\\/Date(915246245006)\\/\"", str.ToString());
                }
            }
        }

#pragma warning disable 0649
        public class _StringsAndChars
        {
            public class _One
            {
                public string Single;
            }
            
            public class _Two
            {
                public int _;
                public string Trailing;
            }

            public class _Three
            {
                public string Leading;
                public int _;
            }

            public _One One;
            public _Two Two;
            public _Three Three;
        }
#pragma warning restore 0649

        [TestMethod]
        public void StringsAndChars()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _StringsAndChars
                    {
                        One = new _StringsAndChars._One
                        {
                            Single = "Hello World"
                        },
                        Two = new _StringsAndChars._Two
                        {
                            _ = 123,
                            Trailing = "Fizz Buzz"
                        },
                        Three = new _StringsAndChars._Three
                        {
                            Leading = "Foo Bar",
                            _ = 456
                        }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"One\":{\"Single\":\"Hello World\"},\"Two\":{\"_\":123,\"Trailing\":\"Fizz Buzz\"},\"Three\":{\"_\":456,\"Leading\":\"Foo Bar\"}}", res);
            }
        }

        [TestMethod]
        public void Dictionary()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, int>
                    {
                        { "hello world", 123 },
                        { "fizz buzz", 456 },
                        { "indeed", 789 }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"hello world\":123,\"fizz buzz\":456,\"indeed\":789}", res);
            }
        }

#pragma warning disable 0649
        public class _List
        {
            public string Key;
            public int Val;
        }
#pragma warning restore 0649

        [TestMethod]
        public void List()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new []
                    {
                        new _List { Key = "whatever", Val = 123 },
                        new _List { Key = "indeed", Val = 456 }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("[{\"Val\":123,\"Key\":\"whatever\"},{\"Val\":456,\"Key\":\"indeed\"}]", res);
            }
        }

        public class _Properties
        {
            public int Foo { get; set; }
            public string Bar { get; set; }
        }

        [TestMethod]
        public void Properties()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _Properties { Foo = 123, Bar = "hello" },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"Foo\":123,\"Bar\":\"hello\"}", res);
            }
        }

        public class _InnerLists
        {
            public class _WithList
            {
                public List<int> List;
            }

            public _WithList WithList;
        }

        [TestMethod]
        public void InnerLists()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _InnerLists
                    {
                        WithList = new _InnerLists._WithList
                        {
                            List = new List<int> { 1, 2, 3 }
                        }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"WithList\":{\"List\":[1,2,3]}}", res);
            }
        }

        class _CharacterEncoding
        {
            public char Char;
        }

        [TestMethod]
        public void CharacterEncoding()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0000' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0000\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0001' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0001\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0002' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0002\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0003' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0003\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0004' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0004\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0005' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0005\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0006' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0006\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0007' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0007\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0008' }, str);
                Assert.AreEqual("{\"Char\":\"\\b\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0009' }, str);
                Assert.AreEqual("{\"Char\":\"\\t\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000A' }, str);
                Assert.AreEqual("{\"Char\":\"\\n\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000B' }, str);
                Assert.AreEqual("{\"Char\":\"\\u000B\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000C' }, str);
                Assert.AreEqual("{\"Char\":\"\\f\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000D' }, str);
                Assert.AreEqual("{\"Char\":\"\\r\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000E' }, str);
                Assert.AreEqual("{\"Char\":\"\\u000E\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000F' }, str);
                Assert.AreEqual("{\"Char\":\"\\u000F\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0010' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0010\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0011' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0011\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0012' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0012\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0013' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0013\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0014' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0014\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0015' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0015\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0016' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0016\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0017' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0017\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0018' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0018\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0019' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0019\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001A' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001A\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001B' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001B\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001C' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001C\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001D' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001D\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001E' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001E\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001F' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001F\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\\' }, str);
                Assert.AreEqual("{\"Char\":\"\\\\\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '"' }, str);
                Assert.AreEqual("{\"Char\":\"\\\"\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize("hello\b\f\r\n\tworld", str);
                Assert.AreEqual("\"hello\\b\\f\\r\\n\\tworld\"", str.ToString());
            }
        }

        [TestMethod]
        public void NullablePrimitives()
        {
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<byte?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<byte?>(123, str);
                    Assert.AreEqual("123", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<sbyte?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<sbyte?>(-123, str);
                    Assert.AreEqual("-123", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<short?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<short?>(-1024, str);
                    Assert.AreEqual("-1024", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ushort?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<ushort?>(2048, str);
                    Assert.AreEqual("2048", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<int?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<int?>(-1234567, str);
                    Assert.AreEqual("-1234567", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<uint?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<uint?>(123456789, str);
                    Assert.AreEqual("123456789", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<long?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<long?>(long.MinValue, str);
                    Assert.AreEqual(long.MinValue.ToString(), str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ulong?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<ulong?>(ulong.MaxValue, str);
                    Assert.AreEqual(ulong.MaxValue.ToString(), str.ToString());
                }
            }
        }

        [TestMethod]
        public void NullableMembers()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new List<int?> { 0, null, 1, null, 2, null, 3 }, str);
                Assert.AreEqual("[0,null,1,null,2,null,3]", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new Dictionary<string, double?> { { "hello", null }, {"world", 3.21}}, str);
                Assert.AreEqual("{\"hello\":null,\"world\":3.21}", str.ToString());
            }
        }

        public struct _ValueTypes
        {
            public string A;
            public char B;
            public List<int> C;
        }

        [TestMethod]
        public void ValueTypes()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _ValueTypes { A = "hello world", B = 'C', C = new List<int> { 3, 1, 4, 1, 5, 9 } }, str);
                Assert.AreEqual("{\"A\":\"hello world\",\"B\":\"C\",\"C\":[3,1,4,1,5,9]}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new[] { new _ValueTypes { A = "hello world", B = 'C', C = new List<int> { 3, 1, 4, 1, 5, 9 } } }, str);
                Assert.AreEqual("[{\"A\":\"hello world\",\"B\":\"C\",\"C\":[3,1,4,1,5,9]}]", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new Dictionary<string, _ValueTypes> { { "hello", new _ValueTypes { A = "hello world", B = 'C', C = new List<int> { 3, 1, 4, 1, 5, 9 } } }, { "world", new _ValueTypes { A = "foo bar", B = 'D', C = new List<int> { 1, 3, 1, 8 } } } }, str);
                Assert.AreEqual("{\"hello\":{\"A\":\"hello world\",\"B\":\"C\",\"C\":[3,1,4,1,5,9]},\"world\":{\"A\":\"foo bar\",\"B\":\"D\",\"C\":[1,3,1,8]}}", str.ToString());
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<_ValueTypes?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<_ValueTypes?>(new _ValueTypes { A = "bizz", B = '\0', C = null }, str);
                    Assert.AreEqual("{\"A\":\"bizz\",\"B\":\"\\u0000\",\"C\":null}", str.ToString());
                }
            }
        }

        public struct _CyclicalValueTypes
        {
            public class _One
            {
                public _CyclicalValueTypes? Inner;
            }

            public _One A;
            public long B;
            public double C;
        }

        [TestMethod]
        public void CyclicalValueTypes()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CyclicalValueTypes { A = new _CyclicalValueTypes._One { Inner = new _CyclicalValueTypes { B = 123, C = 4.56 } }, B = long.MaxValue, C = 78.90 }, str);
                Assert.AreEqual("{\"A\":{\"Inner\":{\"A\":null,\"B\":123,\"C\":4.56}},\"B\":9223372036854775807,\"C\":78.9}", str.ToString());
            }
        }

        public class _ExcludeNulls
        {
            public string A;
            public string B;
            public int? C;
            public int? D;
            public _ExcludeNulls E;
            public _ExcludeNulls F;
        }

        [TestMethod]
        public void ExcludeNulls()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _ExcludeNulls
                    {
                        A = "hello",
                        B = null,
                        C = 123,
                        D = null,
                        E = null,
                        F = new _ExcludeNulls
                        {
                            A = null,
                            B = "world",
                            C = null,
                            D = 456,
                            E = new _ExcludeNulls
                            {
                                C = 999
                            },
                            F = null
                        }
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();

                Assert.AreEqual("{\"F\":{\"E\":{\"C\":999},\"B\":\"world\",\"D\":456},\"A\":\"hello\",\"C\":123}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "world" },
                        { "foo", null },
                        { "fizz", "buzz" }
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();

                Assert.AreEqual("{\"hello\":\"world\",\"fizz\":\"buzz\"}", res);
            }
        }

        public class _PrettyPrint
        {
            public string A;
            public string B;
            public int? C;
            public int? D;
            public _PrettyPrint E;
            public _PrettyPrint F;
        }

        [TestMethod]
        public void PrettyPrint()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _PrettyPrint
                    {
                        A = "hello",
                        B = null,
                        C = 123,
                        D = null,
                        E = null,
                        F = new _PrettyPrint
                        {
                            A = null,
                            B = "world",
                            C = null,
                            D = 456,
                            E = new _PrettyPrint
                            {
                                C = 999
                            },
                            F = null
                        }
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();

                Assert.AreEqual("{\n \"F\": {\n  \"F\": null,\n  \"E\": {\n   \"F\": null,\n   \"E\": null,\n   \"A\": null,\n   \"B\": null,\n   \"C\": 999,\n   \"D\": null\n  },\n  \"A\": null,\n  \"B\": \"world\",\n  \"C\": null,\n  \"D\": 456\n },\n \"E\": null,\n \"A\": \"hello\",\n \"B\": null,\n \"C\": 123,\n \"D\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _PrettyPrint
                    {
                        A = "hello",
                        B = null,
                        C = 123,
                        D = null,
                        E = null,
                        F = new _PrettyPrint
                        {
                            A = null,
                            B = "world",
                            C = null,
                            D = 456,
                            E = new _PrettyPrint
                            {
                                C = 999
                            },
                            F = null
                        }
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();

                Assert.AreEqual("{\n \"F\": {\n  \"E\": {\n   \"C\": 999\n  },\n  \"B\": \"world\",\n  \"D\": 456\n },\n \"A\": \"hello\",\n \"C\": 123\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, int?>
                    {
                        {"hello world", 31415926 },
                        {"fizz buzz", null },
                        {"foo bar", 1318 }
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();

                Assert.AreEqual("{\n \"hello world\": 31415926,\n \"fizz buzz\": null,\n \"foo bar\": 1318\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, int?>
                    {
                        {"hello world", 31415926 },
                        {"fizz buzz", null },
                        {"foo bar", 1318 }
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();

                Assert.AreEqual("{\n \"hello world\": 31415926,\n \"foo bar\": 1318\n}", res);
            }
        }

        [TestMethod]
        public void DictionaryEncoding()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello\nworld", "fizz\0buzz" },
                        { "\r\t\f\n", "\0\0\0\0\0\0\0\0\0\0" },
                        { "\0", "\b\b\b\b\b" }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual(@"{""hello\nworld"":""fizz\u0000buzz"",""\r\t\f\n"":""\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000"",""\u0000"":""\b\b\b\b\b""}", res);
            }
        }

        [TestMethod]
        public void DateTimeFormats()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1),
                    str,
                    Options.Default
                );

                var res = str.ToString();
                Assert.AreEqual("\"\\/Date(315532800000)\\/\"", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1),
                    str,
                    Options.MillisecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("315532800000", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1),
                    str,
                    Options.SecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("315532800", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1),
                    str,
                    Options.ISO8601
                );

                var res = str.ToString();
                Assert.AreEqual("\"1980-01-01T05:00:00Z\"", res);
            }
        }

        [TestMethod]
        public void ISODateTimes()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var rand = new Random(8337586);

            for (var i = 0; i < 10000; i++)
            {
                var rndDt = epoch;
                switch (rand.Next(6))
                {
                    case 0: rndDt += TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                    case 1: rndDt += TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                    case 2: rndDt += TimeSpan.FromSeconds(rand.Next()); break;
                    case 3: rndDt -= TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                    case 4: rndDt -= TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                    case 5: rndDt -= TimeSpan.FromSeconds(rand.Next()); break;
                }

                var expected = "\"" + rndDt.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\"";
                string actual;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(rndDt, str, Options.ISO8601);
                    actual = str.ToString();
                }

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void AllOptions()
        {
            using(var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.Default
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000,\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000,\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511,\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpochExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511,\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\"\n}", res);
            }

            // JSONP
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.JSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\",\"C\":\"hello\\u2028\\u2029world\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\",\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.PrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\",\n \"C\": \"hello\\u2028\\u2029world\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.PrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\",\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000,\"C\":\"hello\\u2028\\u2029world\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000,\n \"C\": \"hello\\u2028\\u2029world\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511,\"C\":\"hello\\u2028\\u2029world\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511,\n \"C\": \"hello\\u2028\\u2029world\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601JSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\",\"C\":\"hello\\u2028\\u2029world\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601ExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\",\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601PrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\",\n \"C\": \"hello\\u2028\\u2029world\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601PrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\",\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }
        }

        public class _InfiniteRecursion
        {
            public int A;
            public _InfiniteRecursion Next;
        }

        [TestMethod]
        public void InfiniteRecursion()
        {
            using(var str = new StringWriter())
            {
                var root = new _InfiniteRecursion { A = 123 };
                root.Next = root;

                try
                {
                    JSON.Serialize(root, str);
                    Assert.Fail();
                }
                catch (InfiniteRecursionException)
                {
                    // check that it failed at the right time...
                    var failed = str.ToString();
                    Assert.AreEqual("{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":", failed);
                }
            }
        }

        public class _ConditionalSerialization1
        {
            public string Val { get; set; }

            public string AlwaysNull { get; set; }

            public int AlwaysHasValue { get { return 4; } }

            internal bool ShouldSerializeVal()
            {
                return Val != null && (Val.Length % 2) == 0;
            }

            public static _ConditionalSerialization1 Random(Random rand)
            {
                return
                    new _ConditionalSerialization1
                    {
                        Val = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public class _ConditionalSerialization2
        {
            public string Foo { get; set; }

            public string AlwaysNull { get; set; }

            internal bool ShouldSerializeFoo()
            {
                return Foo == null || (Foo.Length % 2) == 1;
            }

            public static _ConditionalSerialization2 Random(Random rand)
            {
                return
                    new _ConditionalSerialization2
                    {
                        Foo = SpeedProofTests._RandString(rand)
                    };
            }
        }

        [TestMethod]
        public void ConditionalSerialization()
        {
            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                using (var str = new StringWriter())
                {
                    var obj = _ConditionalSerialization1.Random(rand);

                    JSON.Serialize(obj, str, Options.ExcludeNulls);

                    var res = str.ToString();

                    if (res.Contains("\"AlwaysNull\""))
                    {
                        Assert.Fail(res);
                    }

                    if (!res.Contains("\"AlwaysHasValue\":4"))
                    {
                        Assert.Fail(res);
                    }

                    if (obj.ShouldSerializeVal() && !res.Contains("\"Val\":"))
                    {
                        Assert.Fail(res);
                    }

                    if (!obj.ShouldSerializeVal() && res.Contains("\"Val\":"))
                    {
                        Assert.Fail(res);
                    }
                }
            }

            for (var j = 0; j < 1000; j++)
            {
                using (var str = new StringWriter())
                {
                    var obj = _ConditionalSerialization2.Random(rand);

                    JSON.Serialize(obj, str, Options.Default);

                    var res = str.ToString();

                    if (!res.Contains("\"AlwaysNull\":null"))
                    {
                        Assert.Fail(res);
                    }

                    if (obj.ShouldSerializeFoo() && !res.Contains("\"Foo\":"))
                    {
                        Assert.Fail(res);
                    }

                    if (!obj.ShouldSerializeFoo() && res.Contains("\"Foo\":"))
                    {
                        Assert.Fail(res);
                    }
                }
            }
        }

        enum _Enums
        {
            A = 1,
            B = 2,
            C = 3
        }

        enum _Enums2 : sbyte
        {
            A = -1,
            B = 22,
            C = -104,
            D = 66
        }

        [TestMethod]
        public void Enums()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(_Enums.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(_Enums.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(_Enums.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(null, str);
                Assert.AreEqual("null", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums2.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums2.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums2.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(_Enums2.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(_Enums2.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(_Enums2.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(null, str);
                Assert.AreEqual("null", str.ToString());
            }
        }

        enum _EnumMembers : long
        {
            Foo = 1,
            Bar = 2,
            World = 3,
            Fizz = 4
        }

        [TestMethod]
        public void EnumMembers()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = _EnumMembers.Bar,
                        B = (_EnumMembers?)null
                    },
                    str
                );

                Assert.AreEqual("{\"A\":\"Bar\",\"B\":null}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, _EnumMembers?>
                    {
                        {"A",  _EnumMembers.Bar },
                        {"B", (_EnumMembers?)null }
                    },
                    str
                );

                Assert.AreEqual("{\"A\":\"Bar\",\"B\":null}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new [] { _EnumMembers.Bar, _EnumMembers.World, _EnumMembers.Fizz, _EnumMembers.Foo, _EnumMembers.Fizz },
                    str
                );

                Assert.AreEqual("[\"Bar\",\"World\",\"Fizz\",\"Foo\",\"Fizz\"]", str.ToString());
            }
        }

        enum _EnumDictionaryKeys
        {
            A = 3,
            B = 4,
            C = 11,
            D = 28
        }

        [TestMethod]
        public void EnumDictionaryKeys()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, "hello" },
                        { _EnumDictionaryKeys.B, "world" },
                        { _EnumDictionaryKeys.C, "fizz" },
                        { _EnumDictionaryKeys.D, "buzz" },
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"hello\",\"B\":\"world\",\"C\":\"fizz\",\"D\":\"buzz\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, "hello" },
                        { _EnumDictionaryKeys.B, "world" },
                        { _EnumDictionaryKeys.C, "fizz" },
                        { _EnumDictionaryKeys.D, "buzz" },
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"hello\",\n \"B\": \"world\",\n \"C\": \"fizz\",\n \"D\": \"buzz\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, "hello" },
                        { _EnumDictionaryKeys.B, null },
                        { _EnumDictionaryKeys.C, "fizz" },
                        { _EnumDictionaryKeys.D, null },
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"hello\",\"C\":\"fizz\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, null },
                        { _EnumDictionaryKeys.B, "world" },
                        { _EnumDictionaryKeys.C, null },
                        { _EnumDictionaryKeys.D, "buzz" },
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"B\": \"world\",\n \"D\": \"buzz\"\n}", res);
            }
        }

        enum _EnumVariations1 : byte
        {
            A = 0,
            B = 127,
            C = 255
        }

        enum _EnumVariations2 : sbyte
        {
            A = -128,
            B = 0,
            C = 127
        }

        enum _EnumVariations3 : short
        {
            A = short.MinValue,
            B = 0,
            C = short.MaxValue
        }

        enum _EnumVariations4 : ushort
        {
            A = ushort.MinValue,
            B = 32767,
            C = ushort.MaxValue
        }

        enum _EnumVariations5 : int
        {
            A = int.MinValue,
            B = 0,
            C = int.MaxValue
        }

        enum _EnumVariations6 : uint
        {
            A = uint.MinValue,
            B = 2147483647,
            C = uint.MaxValue
        }

        enum _EnumVariations7 : long
        {
            A = long.MinValue,
            B = 0,
            C = long.MaxValue
        }

        enum _EnumVariations8 : ulong
        {
            A = ulong.MinValue,
            B = 9223372036854775807L,
            C = ulong.MaxValue
        }

        enum _EnumVariations9 : byte
        {
            AA = 0, AB = 1, AC = 2, AD = 3, AE = 4, AF = 5, AG = 6, AH = 7, AI = 8, AJ = 9, AK = 10, AL = 11, AM = 12, AN = 13, AO = 14, AP = 15, AQ = 16, AR = 17, AS = 18, AT = 19, AU = 20, AV = 21, AW = 22, AX = 23, AY = 24, AZ = 25, BA = 26, BB = 27, BC = 28, BD = 29, BE = 30, BF = 31, BG = 32, BH = 33, BI = 34, BJ = 35, BK = 36, BL = 37, BM = 38, BN = 39, BO = 40, BP = 41, BQ = 42, BR = 43, BS = 44, BT = 45, BU = 46, BV = 47, BW = 48, BX = 49, BY = 50, BZ = 51, CA = 52, CB = 53, CC = 54, CD = 55, CE = 56, CF = 57, CG = 58, CH = 59, CI = 60, CJ = 61, CK = 62, CL = 63, CM = 64, CN = 65, CO = 66, CP = 67, CQ = 68, CR = 69, CS = 70, CT = 71, CU = 72, CV = 73, CW = 74, CX = 75, CY = 76, CZ = 77, DA = 78, DB = 79, DC = 80, DD = 81, DE = 82, DF = 83, DG = 84, DH = 85, DI = 86, DJ = 87, DK = 88, DL = 89, DM = 90, DN = 91, DO = 92, DP = 93, DQ = 94, DR = 95, DS = 96, DT = 97, DU = 98, DV = 99, DW = 100, DX = 101, DY = 102, DZ = 103, EA = 104, EB = 105, EC = 106, ED = 107, EE = 108, EF = 109, EG = 110, EH = 111, EI = 112, EJ = 113, EK = 114, EL = 115, EM = 116, EN = 117, EO = 118, EP = 119, EQ = 120, ER = 121, ES = 122, ET = 123, EU = 124, EV = 125, EW = 126, EX = 127, EY = 128, EZ = 129, FA = 130, FB = 131, FC = 132, FD = 133, FE = 134, FF = 135, FG = 136, FH = 137, FI = 138, FJ = 139, FK = 140, FL = 141, FM = 142, FN = 143, FO = 144, FP = 145, FQ = 146, FR = 147, FS = 148, FT = 149, FU = 150, FV = 151, FW = 152, FX = 153, FY = 154, FZ = 155, GA = 156, GB = 157, GC = 158, GD = 159, GE = 160, GF = 161, GG = 162, GH = 163, GI = 164, GJ = 165, GK = 166, GL = 167, GM = 168, GN = 169, GO = 170, GP = 171, GQ = 172, GR = 173, GS = 174, GT = 175, GU = 176, GV = 177, GW = 178, GX = 179, GY = 180, GZ = 181, HA = 182, HB = 183, HC = 184, HD = 185, HE = 186, HF = 187, HG = 188, HH = 189, HI = 190, HJ = 191, HK = 192, HL = 193, HM = 194, HN = 195, HO = 196, HP = 197, HQ = 198, HR = 199, HS = 200, HT = 201, HU = 202, HV = 203, HW = 204, HX = 205, HY = 206, HZ = 207, IA = 208, IB = 209, IC = 210, ID = 211, IE = 212, IF = 213, IG = 214, IH = 215, II = 216, IJ = 217, IK = 218, IL = 219, IM = 220, IN = 221, IO = 222, IP = 223, IQ = 224, IR = 225, IS = 226, IT = 227, IU = 228, IV = 229, IW = 230, IX = 231, IY = 232, IZ = 233, JA = 234, JB = 235, JC = 236, JD = 237, JE = 238, JF = 239, JG = 240, JH = 241, JI = 242, JJ = 243, JK = 244, JL = 245, JM = 246, JN = 247, JO = 248, JP = 249, JQ = 250, JR = 251, JS = 252, JT = 253, JU = 254, JV = 255,
        }

        enum _EnumVariations10 : sbyte
        {
            AA = -128, AB = -127, AC = -126, AD = -125, AE = -124, AF = -123, AG = -122, AH = -121, AI = -120, AJ = -119, AK = -118, AL = -117, AM = -116, AN = -115, AO = -114, AP = -113, AQ = -112, AR = -111, AS = -110, AT = -109, AU = -108, AV = -107, AW = -106, AX = -105, AY = -104, AZ = -103, BA = -102, BB = -101, BC = -100, BD = -99, BE = -98, BF = -97, BG = -96, BH = -95, BI = -94, BJ = -93, BK = -92, BL = -91, BM = -90, BN = -89, BO = -88, BP = -87, BQ = -86, BR = -85, BS = -84, BT = -83, BU = -82, BV = -81, BW = -80, BX = -79, BY = -78, BZ = -77, CA = -76, CB = -75, CC = -74, CD = -73, CE = -72, CF = -71, CG = -70, CH = -69, CI = -68, CJ = -67, CK = -66, CL = -65, CM = -64, CN = -63, CO = -62, CP = -61, CQ = -60, CR = -59, CS = -58, CT = -57, CU = -56, CV = -55, CW = -54, CX = -53, CY = -52, CZ = -51, DA = -50, DB = -49, DC = -48, DD = -47, DE = -46, DF = -45, DG = -44, DH = -43, DI = -42, DJ = -41, DK = -40, DL = -39, DM = -38, DN = -37, DO = -36, DP = -35, DQ = -34, DR = -33, DS = -32, DT = -31, DU = -30, DV = -29, DW = -28, DX = -27, DY = -26, DZ = -25, EA = -24, EB = -23, EC = -22, ED = -21, EE = -20, EF = -19, EG = -18, EH = -17, EI = -16, EJ = -15, EK = -14, EL = -13, EM = -12, EN = -11, EO = -10, EP = -9, EQ = -8, ER = -7, ES = -6, ET = -5, EU = -4, EV = -3, EW = -2, EX = -1, EY = 0, EZ = 1, FA = 2, FB = 3, FC = 4, FD = 5, FE = 6, FF = 7, FG = 8, FH = 9, FI = 10, FJ = 11, FK = 12, FL = 13, FM = 14, FN = 15, FO = 16, FP = 17, FQ = 18, FR = 19, FS = 20, FT = 21, FU = 22, FV = 23, FW = 24, FX = 25, FY = 26, FZ = 27, GA = 28, GB = 29, GC = 30, GD = 31, GE = 32, GF = 33, GG = 34, GH = 35, GI = 36, GJ = 37, GK = 38, GL = 39, GM = 40, GN = 41, GO = 42, GP = 43, GQ = 44, GR = 45, GS = 46, GT = 47, GU = 48, GV = 49, GW = 50, GX = 51, GY = 52, GZ = 53, HA = 54, HB = 55, HC = 56, HD = 57, HE = 58, HF = 59, HG = 60, HH = 61, HI = 62, HJ = 63, HK = 64, HL = 65, HM = 66, HN = 67, HO = 68, HP = 69, HQ = 70, HR = 71, HS = 72, HT = 73, HU = 74, HV = 75, HW = 76, HX = 77, HY = 78, HZ = 79, IA = 80, IB = 81, IC = 82, ID = 83, IE = 84, IF = 85, IG = 86, IH = 87, II = 88, IJ = 89, IK = 90, IL = 91, IM = 92, IN = 93, IO = 94, IP = 95, IQ = 96, IR = 97, IS = 98, IT = 99, IU = 100, IV = 101, IW = 102, IX = 103, IY = 104, IZ = 105, JA = 106, JB = 107, JC = 108, JD = 109, JE = 110, JF = 111, JG = 112, JH = 113, JI = 114, JJ = 115, JK = 116, JL = 117, JM = 118, JN = 119, JO = 120, JP = 121, JQ = 122, JR = 123, JS = 124, JT = 125, JU = 126, JV = 127
        }

        [TestMethod]
        public void EnumVariations()
        {
            // 1
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations1.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations1.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations1.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 2
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations2.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations2.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations2.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 3
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations3.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations3.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations3.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 4
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations4.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations4.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations4.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 5
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations5.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations5.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations5.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 6
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations6.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations6.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations6.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 7
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations7.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations7.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations7.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 8
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations8.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations8.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations8.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 9
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AA, str);
                    Assert.AreEqual("\"AA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AB, str);
                    Assert.AreEqual("\"AB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AC, str);
                    Assert.AreEqual("\"AC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AD, str);
                    Assert.AreEqual("\"AD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AE, str);
                    Assert.AreEqual("\"AE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AF, str);
                    Assert.AreEqual("\"AF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AG, str);
                    Assert.AreEqual("\"AG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AH, str);
                    Assert.AreEqual("\"AH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AI, str);
                    Assert.AreEqual("\"AI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AJ, str);
                    Assert.AreEqual("\"AJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AK, str);
                    Assert.AreEqual("\"AK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AL, str);
                    Assert.AreEqual("\"AL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AM, str);
                    Assert.AreEqual("\"AM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AN, str);
                    Assert.AreEqual("\"AN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AO, str);
                    Assert.AreEqual("\"AO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AP, str);
                    Assert.AreEqual("\"AP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AQ, str);
                    Assert.AreEqual("\"AQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AR, str);
                    Assert.AreEqual("\"AR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AS, str);
                    Assert.AreEqual("\"AS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AT, str);
                    Assert.AreEqual("\"AT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AU, str);
                    Assert.AreEqual("\"AU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AV, str);
                    Assert.AreEqual("\"AV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AW, str);
                    Assert.AreEqual("\"AW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AX, str);
                    Assert.AreEqual("\"AX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AY, str);
                    Assert.AreEqual("\"AY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AZ, str);
                    Assert.AreEqual("\"AZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BA, str);
                    Assert.AreEqual("\"BA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BB, str);
                    Assert.AreEqual("\"BB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BC, str);
                    Assert.AreEqual("\"BC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BD, str);
                    Assert.AreEqual("\"BD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BE, str);
                    Assert.AreEqual("\"BE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BF, str);
                    Assert.AreEqual("\"BF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BG, str);
                    Assert.AreEqual("\"BG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BH, str);
                    Assert.AreEqual("\"BH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BI, str);
                    Assert.AreEqual("\"BI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BJ, str);
                    Assert.AreEqual("\"BJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BK, str);
                    Assert.AreEqual("\"BK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BL, str);
                    Assert.AreEqual("\"BL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BM, str);
                    Assert.AreEqual("\"BM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BN, str);
                    Assert.AreEqual("\"BN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BO, str);
                    Assert.AreEqual("\"BO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BP, str);
                    Assert.AreEqual("\"BP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BQ, str);
                    Assert.AreEqual("\"BQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BR, str);
                    Assert.AreEqual("\"BR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BS, str);
                    Assert.AreEqual("\"BS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BT, str);
                    Assert.AreEqual("\"BT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BU, str);
                    Assert.AreEqual("\"BU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BV, str);
                    Assert.AreEqual("\"BV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BW, str);
                    Assert.AreEqual("\"BW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BX, str);
                    Assert.AreEqual("\"BX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BY, str);
                    Assert.AreEqual("\"BY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BZ, str);
                    Assert.AreEqual("\"BZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CA, str);
                    Assert.AreEqual("\"CA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CB, str);
                    Assert.AreEqual("\"CB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CC, str);
                    Assert.AreEqual("\"CC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CD, str);
                    Assert.AreEqual("\"CD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CE, str);
                    Assert.AreEqual("\"CE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CF, str);
                    Assert.AreEqual("\"CF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CG, str);
                    Assert.AreEqual("\"CG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CH, str);
                    Assert.AreEqual("\"CH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CI, str);
                    Assert.AreEqual("\"CI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CJ, str);
                    Assert.AreEqual("\"CJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CK, str);
                    Assert.AreEqual("\"CK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CL, str);
                    Assert.AreEqual("\"CL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CM, str);
                    Assert.AreEqual("\"CM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CN, str);
                    Assert.AreEqual("\"CN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CO, str);
                    Assert.AreEqual("\"CO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CP, str);
                    Assert.AreEqual("\"CP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CQ, str);
                    Assert.AreEqual("\"CQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CR, str);
                    Assert.AreEqual("\"CR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CS, str);
                    Assert.AreEqual("\"CS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CT, str);
                    Assert.AreEqual("\"CT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CU, str);
                    Assert.AreEqual("\"CU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CV, str);
                    Assert.AreEqual("\"CV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CW, str);
                    Assert.AreEqual("\"CW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CX, str);
                    Assert.AreEqual("\"CX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CY, str);
                    Assert.AreEqual("\"CY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CZ, str);
                    Assert.AreEqual("\"CZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DA, str);
                    Assert.AreEqual("\"DA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DB, str);
                    Assert.AreEqual("\"DB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DC, str);
                    Assert.AreEqual("\"DC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DD, str);
                    Assert.AreEqual("\"DD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DE, str);
                    Assert.AreEqual("\"DE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DF, str);
                    Assert.AreEqual("\"DF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DG, str);
                    Assert.AreEqual("\"DG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DH, str);
                    Assert.AreEqual("\"DH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DI, str);
                    Assert.AreEqual("\"DI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DJ, str);
                    Assert.AreEqual("\"DJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DK, str);
                    Assert.AreEqual("\"DK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DL, str);
                    Assert.AreEqual("\"DL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DM, str);
                    Assert.AreEqual("\"DM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DN, str);
                    Assert.AreEqual("\"DN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DO, str);
                    Assert.AreEqual("\"DO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DP, str);
                    Assert.AreEqual("\"DP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DQ, str);
                    Assert.AreEqual("\"DQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DR, str);
                    Assert.AreEqual("\"DR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DS, str);
                    Assert.AreEqual("\"DS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DT, str);
                    Assert.AreEqual("\"DT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DU, str);
                    Assert.AreEqual("\"DU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DV, str);
                    Assert.AreEqual("\"DV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DW, str);
                    Assert.AreEqual("\"DW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DX, str);
                    Assert.AreEqual("\"DX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DY, str);
                    Assert.AreEqual("\"DY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DZ, str);
                    Assert.AreEqual("\"DZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EA, str);
                    Assert.AreEqual("\"EA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EB, str);
                    Assert.AreEqual("\"EB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EC, str);
                    Assert.AreEqual("\"EC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ED, str);
                    Assert.AreEqual("\"ED\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EE, str);
                    Assert.AreEqual("\"EE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EF, str);
                    Assert.AreEqual("\"EF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EG, str);
                    Assert.AreEqual("\"EG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EH, str);
                    Assert.AreEqual("\"EH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EI, str);
                    Assert.AreEqual("\"EI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EJ, str);
                    Assert.AreEqual("\"EJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EK, str);
                    Assert.AreEqual("\"EK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EL, str);
                    Assert.AreEqual("\"EL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EM, str);
                    Assert.AreEqual("\"EM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EN, str);
                    Assert.AreEqual("\"EN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EO, str);
                    Assert.AreEqual("\"EO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EP, str);
                    Assert.AreEqual("\"EP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EQ, str);
                    Assert.AreEqual("\"EQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ER, str);
                    Assert.AreEqual("\"ER\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ES, str);
                    Assert.AreEqual("\"ES\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ET, str);
                    Assert.AreEqual("\"ET\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EU, str);
                    Assert.AreEqual("\"EU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EV, str);
                    Assert.AreEqual("\"EV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EW, str);
                    Assert.AreEqual("\"EW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EX, str);
                    Assert.AreEqual("\"EX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EY, str);
                    Assert.AreEqual("\"EY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EZ, str);
                    Assert.AreEqual("\"EZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FA, str);
                    Assert.AreEqual("\"FA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FB, str);
                    Assert.AreEqual("\"FB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FC, str);
                    Assert.AreEqual("\"FC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FD, str);
                    Assert.AreEqual("\"FD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FE, str);
                    Assert.AreEqual("\"FE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FF, str);
                    Assert.AreEqual("\"FF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FG, str);
                    Assert.AreEqual("\"FG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FH, str);
                    Assert.AreEqual("\"FH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FI, str);
                    Assert.AreEqual("\"FI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FJ, str);
                    Assert.AreEqual("\"FJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FK, str);
                    Assert.AreEqual("\"FK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FL, str);
                    Assert.AreEqual("\"FL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FM, str);
                    Assert.AreEqual("\"FM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FN, str);
                    Assert.AreEqual("\"FN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FO, str);
                    Assert.AreEqual("\"FO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FP, str);
                    Assert.AreEqual("\"FP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FQ, str);
                    Assert.AreEqual("\"FQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FR, str);
                    Assert.AreEqual("\"FR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FS, str);
                    Assert.AreEqual("\"FS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FT, str);
                    Assert.AreEqual("\"FT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FU, str);
                    Assert.AreEqual("\"FU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FV, str);
                    Assert.AreEqual("\"FV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FW, str);
                    Assert.AreEqual("\"FW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FX, str);
                    Assert.AreEqual("\"FX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FY, str);
                    Assert.AreEqual("\"FY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FZ, str);
                    Assert.AreEqual("\"FZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GA, str);
                    Assert.AreEqual("\"GA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GB, str);
                    Assert.AreEqual("\"GB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GC, str);
                    Assert.AreEqual("\"GC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GD, str);
                    Assert.AreEqual("\"GD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GE, str);
                    Assert.AreEqual("\"GE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GF, str);
                    Assert.AreEqual("\"GF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GG, str);
                    Assert.AreEqual("\"GG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GH, str);
                    Assert.AreEqual("\"GH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GI, str);
                    Assert.AreEqual("\"GI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GJ, str);
                    Assert.AreEqual("\"GJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GK, str);
                    Assert.AreEqual("\"GK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GL, str);
                    Assert.AreEqual("\"GL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GM, str);
                    Assert.AreEqual("\"GM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GN, str);
                    Assert.AreEqual("\"GN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GO, str);
                    Assert.AreEqual("\"GO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GP, str);
                    Assert.AreEqual("\"GP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GQ, str);
                    Assert.AreEqual("\"GQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GR, str);
                    Assert.AreEqual("\"GR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GS, str);
                    Assert.AreEqual("\"GS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GT, str);
                    Assert.AreEqual("\"GT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GU, str);
                    Assert.AreEqual("\"GU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GV, str);
                    Assert.AreEqual("\"GV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GW, str);
                    Assert.AreEqual("\"GW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GX, str);
                    Assert.AreEqual("\"GX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GY, str);
                    Assert.AreEqual("\"GY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GZ, str);
                    Assert.AreEqual("\"GZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HA, str);
                    Assert.AreEqual("\"HA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HB, str);
                    Assert.AreEqual("\"HB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HC, str);
                    Assert.AreEqual("\"HC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HD, str);
                    Assert.AreEqual("\"HD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HE, str);
                    Assert.AreEqual("\"HE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HF, str);
                    Assert.AreEqual("\"HF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HG, str);
                    Assert.AreEqual("\"HG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HH, str);
                    Assert.AreEqual("\"HH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HI, str);
                    Assert.AreEqual("\"HI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HJ, str);
                    Assert.AreEqual("\"HJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HK, str);
                    Assert.AreEqual("\"HK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HL, str);
                    Assert.AreEqual("\"HL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HM, str);
                    Assert.AreEqual("\"HM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HN, str);
                    Assert.AreEqual("\"HN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HO, str);
                    Assert.AreEqual("\"HO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HP, str);
                    Assert.AreEqual("\"HP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HQ, str);
                    Assert.AreEqual("\"HQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HR, str);
                    Assert.AreEqual("\"HR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HS, str);
                    Assert.AreEqual("\"HS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HT, str);
                    Assert.AreEqual("\"HT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HU, str);
                    Assert.AreEqual("\"HU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HV, str);
                    Assert.AreEqual("\"HV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HW, str);
                    Assert.AreEqual("\"HW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HX, str);
                    Assert.AreEqual("\"HX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HY, str);
                    Assert.AreEqual("\"HY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HZ, str);
                    Assert.AreEqual("\"HZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IA, str);
                    Assert.AreEqual("\"IA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IB, str);
                    Assert.AreEqual("\"IB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IC, str);
                    Assert.AreEqual("\"IC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ID, str);
                    Assert.AreEqual("\"ID\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IE, str);
                    Assert.AreEqual("\"IE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IF, str);
                    Assert.AreEqual("\"IF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IG, str);
                    Assert.AreEqual("\"IG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IH, str);
                    Assert.AreEqual("\"IH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.II, str);
                    Assert.AreEqual("\"II\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IJ, str);
                    Assert.AreEqual("\"IJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IK, str);
                    Assert.AreEqual("\"IK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IL, str);
                    Assert.AreEqual("\"IL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IM, str);
                    Assert.AreEqual("\"IM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IN, str);
                    Assert.AreEqual("\"IN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IO, str);
                    Assert.AreEqual("\"IO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IP, str);
                    Assert.AreEqual("\"IP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IQ, str);
                    Assert.AreEqual("\"IQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IR, str);
                    Assert.AreEqual("\"IR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IS, str);
                    Assert.AreEqual("\"IS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IT, str);
                    Assert.AreEqual("\"IT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IU, str);
                    Assert.AreEqual("\"IU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IV, str);
                    Assert.AreEqual("\"IV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IW, str);
                    Assert.AreEqual("\"IW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IX, str);
                    Assert.AreEqual("\"IX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IY, str);
                    Assert.AreEqual("\"IY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IZ, str);
                    Assert.AreEqual("\"IZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JA, str);
                    Assert.AreEqual("\"JA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JB, str);
                    Assert.AreEqual("\"JB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JC, str);
                    Assert.AreEqual("\"JC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JD, str);
                    Assert.AreEqual("\"JD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JE, str);
                    Assert.AreEqual("\"JE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JF, str);
                    Assert.AreEqual("\"JF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JG, str);
                    Assert.AreEqual("\"JG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JH, str);
                    Assert.AreEqual("\"JH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JI, str);
                    Assert.AreEqual("\"JI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JJ, str);
                    Assert.AreEqual("\"JJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JK, str);
                    Assert.AreEqual("\"JK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JL, str);
                    Assert.AreEqual("\"JL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JM, str);
                    Assert.AreEqual("\"JM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JN, str);
                    Assert.AreEqual("\"JN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JO, str);
                    Assert.AreEqual("\"JO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JP, str);
                    Assert.AreEqual("\"JP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JQ, str);
                    Assert.AreEqual("\"JQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JR, str);
                    Assert.AreEqual("\"JR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JS, str);
                    Assert.AreEqual("\"JS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JT, str);
                    Assert.AreEqual("\"JT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JU, str);
                    Assert.AreEqual("\"JU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JV, str);
                    Assert.AreEqual("\"JV\"", str.ToString());
                }
            }
            // 10
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AA, str);
                    Assert.AreEqual("\"AA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AB, str);
                    Assert.AreEqual("\"AB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AC, str);
                    Assert.AreEqual("\"AC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AD, str);
                    Assert.AreEqual("\"AD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AE, str);
                    Assert.AreEqual("\"AE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AF, str);
                    Assert.AreEqual("\"AF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AG, str);
                    Assert.AreEqual("\"AG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AH, str);
                    Assert.AreEqual("\"AH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AI, str);
                    Assert.AreEqual("\"AI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AJ, str);
                    Assert.AreEqual("\"AJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AK, str);
                    Assert.AreEqual("\"AK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AL, str);
                    Assert.AreEqual("\"AL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AM, str);
                    Assert.AreEqual("\"AM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AN, str);
                    Assert.AreEqual("\"AN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AO, str);
                    Assert.AreEqual("\"AO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AP, str);
                    Assert.AreEqual("\"AP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AQ, str);
                    Assert.AreEqual("\"AQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AR, str);
                    Assert.AreEqual("\"AR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AS, str);
                    Assert.AreEqual("\"AS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AT, str);
                    Assert.AreEqual("\"AT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AU, str);
                    Assert.AreEqual("\"AU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AV, str);
                    Assert.AreEqual("\"AV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AW, str);
                    Assert.AreEqual("\"AW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AX, str);
                    Assert.AreEqual("\"AX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AY, str);
                    Assert.AreEqual("\"AY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AZ, str);
                    Assert.AreEqual("\"AZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BA, str);
                    Assert.AreEqual("\"BA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BB, str);
                    Assert.AreEqual("\"BB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BC, str);
                    Assert.AreEqual("\"BC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BD, str);
                    Assert.AreEqual("\"BD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BE, str);
                    Assert.AreEqual("\"BE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BF, str);
                    Assert.AreEqual("\"BF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BG, str);
                    Assert.AreEqual("\"BG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BH, str);
                    Assert.AreEqual("\"BH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BI, str);
                    Assert.AreEqual("\"BI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BJ, str);
                    Assert.AreEqual("\"BJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BK, str);
                    Assert.AreEqual("\"BK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BL, str);
                    Assert.AreEqual("\"BL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BM, str);
                    Assert.AreEqual("\"BM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BN, str);
                    Assert.AreEqual("\"BN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BO, str);
                    Assert.AreEqual("\"BO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BP, str);
                    Assert.AreEqual("\"BP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BQ, str);
                    Assert.AreEqual("\"BQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BR, str);
                    Assert.AreEqual("\"BR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BS, str);
                    Assert.AreEqual("\"BS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BT, str);
                    Assert.AreEqual("\"BT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BU, str);
                    Assert.AreEqual("\"BU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BV, str);
                    Assert.AreEqual("\"BV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BW, str);
                    Assert.AreEqual("\"BW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BX, str);
                    Assert.AreEqual("\"BX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BY, str);
                    Assert.AreEqual("\"BY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BZ, str);
                    Assert.AreEqual("\"BZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CA, str);
                    Assert.AreEqual("\"CA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CB, str);
                    Assert.AreEqual("\"CB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CC, str);
                    Assert.AreEqual("\"CC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CD, str);
                    Assert.AreEqual("\"CD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CE, str);
                    Assert.AreEqual("\"CE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CF, str);
                    Assert.AreEqual("\"CF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CG, str);
                    Assert.AreEqual("\"CG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CH, str);
                    Assert.AreEqual("\"CH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CI, str);
                    Assert.AreEqual("\"CI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CJ, str);
                    Assert.AreEqual("\"CJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CK, str);
                    Assert.AreEqual("\"CK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CL, str);
                    Assert.AreEqual("\"CL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CM, str);
                    Assert.AreEqual("\"CM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CN, str);
                    Assert.AreEqual("\"CN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CO, str);
                    Assert.AreEqual("\"CO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CP, str);
                    Assert.AreEqual("\"CP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CQ, str);
                    Assert.AreEqual("\"CQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CR, str);
                    Assert.AreEqual("\"CR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CS, str);
                    Assert.AreEqual("\"CS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CT, str);
                    Assert.AreEqual("\"CT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CU, str);
                    Assert.AreEqual("\"CU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CV, str);
                    Assert.AreEqual("\"CV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CW, str);
                    Assert.AreEqual("\"CW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CX, str);
                    Assert.AreEqual("\"CX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CY, str);
                    Assert.AreEqual("\"CY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CZ, str);
                    Assert.AreEqual("\"CZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DA, str);
                    Assert.AreEqual("\"DA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DB, str);
                    Assert.AreEqual("\"DB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DC, str);
                    Assert.AreEqual("\"DC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DD, str);
                    Assert.AreEqual("\"DD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DE, str);
                    Assert.AreEqual("\"DE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DF, str);
                    Assert.AreEqual("\"DF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DG, str);
                    Assert.AreEqual("\"DG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DH, str);
                    Assert.AreEqual("\"DH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DI, str);
                    Assert.AreEqual("\"DI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DJ, str);
                    Assert.AreEqual("\"DJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DK, str);
                    Assert.AreEqual("\"DK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DL, str);
                    Assert.AreEqual("\"DL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DM, str);
                    Assert.AreEqual("\"DM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DN, str);
                    Assert.AreEqual("\"DN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DO, str);
                    Assert.AreEqual("\"DO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DP, str);
                    Assert.AreEqual("\"DP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DQ, str);
                    Assert.AreEqual("\"DQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DR, str);
                    Assert.AreEqual("\"DR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DS, str);
                    Assert.AreEqual("\"DS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DT, str);
                    Assert.AreEqual("\"DT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DU, str);
                    Assert.AreEqual("\"DU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DV, str);
                    Assert.AreEqual("\"DV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DW, str);
                    Assert.AreEqual("\"DW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DX, str);
                    Assert.AreEqual("\"DX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DY, str);
                    Assert.AreEqual("\"DY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DZ, str);
                    Assert.AreEqual("\"DZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EA, str);
                    Assert.AreEqual("\"EA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EB, str);
                    Assert.AreEqual("\"EB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EC, str);
                    Assert.AreEqual("\"EC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ED, str);
                    Assert.AreEqual("\"ED\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EE, str);
                    Assert.AreEqual("\"EE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EF, str);
                    Assert.AreEqual("\"EF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EG, str);
                    Assert.AreEqual("\"EG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EH, str);
                    Assert.AreEqual("\"EH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EI, str);
                    Assert.AreEqual("\"EI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EJ, str);
                    Assert.AreEqual("\"EJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EK, str);
                    Assert.AreEqual("\"EK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EL, str);
                    Assert.AreEqual("\"EL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EM, str);
                    Assert.AreEqual("\"EM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EN, str);
                    Assert.AreEqual("\"EN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EO, str);
                    Assert.AreEqual("\"EO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EP, str);
                    Assert.AreEqual("\"EP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EQ, str);
                    Assert.AreEqual("\"EQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ER, str);
                    Assert.AreEqual("\"ER\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ES, str);
                    Assert.AreEqual("\"ES\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ET, str);
                    Assert.AreEqual("\"ET\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EU, str);
                    Assert.AreEqual("\"EU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EV, str);
                    Assert.AreEqual("\"EV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EW, str);
                    Assert.AreEqual("\"EW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EX, str);
                    Assert.AreEqual("\"EX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EY, str);
                    Assert.AreEqual("\"EY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EZ, str);
                    Assert.AreEqual("\"EZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FA, str);
                    Assert.AreEqual("\"FA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FB, str);
                    Assert.AreEqual("\"FB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FC, str);
                    Assert.AreEqual("\"FC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FD, str);
                    Assert.AreEqual("\"FD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FE, str);
                    Assert.AreEqual("\"FE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FF, str);
                    Assert.AreEqual("\"FF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FG, str);
                    Assert.AreEqual("\"FG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FH, str);
                    Assert.AreEqual("\"FH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FI, str);
                    Assert.AreEqual("\"FI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FJ, str);
                    Assert.AreEqual("\"FJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FK, str);
                    Assert.AreEqual("\"FK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FL, str);
                    Assert.AreEqual("\"FL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FM, str);
                    Assert.AreEqual("\"FM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FN, str);
                    Assert.AreEqual("\"FN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FO, str);
                    Assert.AreEqual("\"FO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FP, str);
                    Assert.AreEqual("\"FP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FQ, str);
                    Assert.AreEqual("\"FQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FR, str);
                    Assert.AreEqual("\"FR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FS, str);
                    Assert.AreEqual("\"FS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FT, str);
                    Assert.AreEqual("\"FT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FU, str);
                    Assert.AreEqual("\"FU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FV, str);
                    Assert.AreEqual("\"FV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FW, str);
                    Assert.AreEqual("\"FW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FX, str);
                    Assert.AreEqual("\"FX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FY, str);
                    Assert.AreEqual("\"FY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FZ, str);
                    Assert.AreEqual("\"FZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GA, str);
                    Assert.AreEqual("\"GA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GB, str);
                    Assert.AreEqual("\"GB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GC, str);
                    Assert.AreEqual("\"GC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GD, str);
                    Assert.AreEqual("\"GD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GE, str);
                    Assert.AreEqual("\"GE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GF, str);
                    Assert.AreEqual("\"GF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GG, str);
                    Assert.AreEqual("\"GG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GH, str);
                    Assert.AreEqual("\"GH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GI, str);
                    Assert.AreEqual("\"GI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GJ, str);
                    Assert.AreEqual("\"GJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GK, str);
                    Assert.AreEqual("\"GK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GL, str);
                    Assert.AreEqual("\"GL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GM, str);
                    Assert.AreEqual("\"GM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GN, str);
                    Assert.AreEqual("\"GN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GO, str);
                    Assert.AreEqual("\"GO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GP, str);
                    Assert.AreEqual("\"GP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GQ, str);
                    Assert.AreEqual("\"GQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GR, str);
                    Assert.AreEqual("\"GR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GS, str);
                    Assert.AreEqual("\"GS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GT, str);
                    Assert.AreEqual("\"GT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GU, str);
                    Assert.AreEqual("\"GU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GV, str);
                    Assert.AreEqual("\"GV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GW, str);
                    Assert.AreEqual("\"GW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GX, str);
                    Assert.AreEqual("\"GX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GY, str);
                    Assert.AreEqual("\"GY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GZ, str);
                    Assert.AreEqual("\"GZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HA, str);
                    Assert.AreEqual("\"HA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HB, str);
                    Assert.AreEqual("\"HB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HC, str);
                    Assert.AreEqual("\"HC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HD, str);
                    Assert.AreEqual("\"HD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HE, str);
                    Assert.AreEqual("\"HE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HF, str);
                    Assert.AreEqual("\"HF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HG, str);
                    Assert.AreEqual("\"HG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HH, str);
                    Assert.AreEqual("\"HH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HI, str);
                    Assert.AreEqual("\"HI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HJ, str);
                    Assert.AreEqual("\"HJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HK, str);
                    Assert.AreEqual("\"HK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HL, str);
                    Assert.AreEqual("\"HL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HM, str);
                    Assert.AreEqual("\"HM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HN, str);
                    Assert.AreEqual("\"HN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HO, str);
                    Assert.AreEqual("\"HO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HP, str);
                    Assert.AreEqual("\"HP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HQ, str);
                    Assert.AreEqual("\"HQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HR, str);
                    Assert.AreEqual("\"HR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HS, str);
                    Assert.AreEqual("\"HS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HT, str);
                    Assert.AreEqual("\"HT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HU, str);
                    Assert.AreEqual("\"HU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HV, str);
                    Assert.AreEqual("\"HV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HW, str);
                    Assert.AreEqual("\"HW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HX, str);
                    Assert.AreEqual("\"HX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HY, str);
                    Assert.AreEqual("\"HY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HZ, str);
                    Assert.AreEqual("\"HZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IA, str);
                    Assert.AreEqual("\"IA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IB, str);
                    Assert.AreEqual("\"IB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IC, str);
                    Assert.AreEqual("\"IC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ID, str);
                    Assert.AreEqual("\"ID\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IE, str);
                    Assert.AreEqual("\"IE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IF, str);
                    Assert.AreEqual("\"IF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IG, str);
                    Assert.AreEqual("\"IG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IH, str);
                    Assert.AreEqual("\"IH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.II, str);
                    Assert.AreEqual("\"II\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IJ, str);
                    Assert.AreEqual("\"IJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IK, str);
                    Assert.AreEqual("\"IK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IL, str);
                    Assert.AreEqual("\"IL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IM, str);
                    Assert.AreEqual("\"IM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IN, str);
                    Assert.AreEqual("\"IN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IO, str);
                    Assert.AreEqual("\"IO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IP, str);
                    Assert.AreEqual("\"IP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IQ, str);
                    Assert.AreEqual("\"IQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IR, str);
                    Assert.AreEqual("\"IR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IS, str);
                    Assert.AreEqual("\"IS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IT, str);
                    Assert.AreEqual("\"IT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IU, str);
                    Assert.AreEqual("\"IU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IV, str);
                    Assert.AreEqual("\"IV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IW, str);
                    Assert.AreEqual("\"IW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IX, str);
                    Assert.AreEqual("\"IX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IY, str);
                    Assert.AreEqual("\"IY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IZ, str);
                    Assert.AreEqual("\"IZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JA, str);
                    Assert.AreEqual("\"JA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JB, str);
                    Assert.AreEqual("\"JB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JC, str);
                    Assert.AreEqual("\"JC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JD, str);
                    Assert.AreEqual("\"JD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JE, str);
                    Assert.AreEqual("\"JE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JF, str);
                    Assert.AreEqual("\"JF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JG, str);
                    Assert.AreEqual("\"JG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JH, str);
                    Assert.AreEqual("\"JH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JI, str);
                    Assert.AreEqual("\"JI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JJ, str);
                    Assert.AreEqual("\"JJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JK, str);
                    Assert.AreEqual("\"JK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JL, str);
                    Assert.AreEqual("\"JL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JM, str);
                    Assert.AreEqual("\"JM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JN, str);
                    Assert.AreEqual("\"JN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JO, str);
                    Assert.AreEqual("\"JO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JP, str);
                    Assert.AreEqual("\"JP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JQ, str);
                    Assert.AreEqual("\"JQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JR, str);
                    Assert.AreEqual("\"JR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JS, str);
                    Assert.AreEqual("\"JS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JT, str);
                    Assert.AreEqual("\"JT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JU, str);
                    Assert.AreEqual("\"JU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JV, str);
                    Assert.AreEqual("\"JV\"", str.ToString());
                }
            }
        }

        [TestMethod]
        public void IntegerDictionaryKeys()
        {
            // byte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<byte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { byte.MinValue, "foo" },
                            { byte.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"255\":\"bar\"}", str.ToString());
                }
            }

            // sbyte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<sbyte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { sbyte.MinValue, "foo" },
                            { sbyte.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-128\":\"foo\",\"127\":\"bar\"}", str.ToString());
                }
            }

            // short
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<short, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { short.MinValue, "foo" },
                            { short.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-32768\":\"foo\",\"32767\":\"bar\"}", str.ToString());
                }
            }

            // ushort
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ushort, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ushort.MinValue, "foo" },
                            { ushort.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"65535\":\"bar\"}", str.ToString());
                }
            }

            // int
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<int, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { int.MinValue, "foo" },
                            { int.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-2147483648\":\"foo\",\"2147483647\":\"bar\"}", str.ToString());
                }
            }

            // uint
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<uint, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { uint.MinValue, "foo" },
                            { uint.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"4294967295\":\"bar\"}", str.ToString());
                }
            }

            // long
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<long, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { long.MinValue, "foo" },
                            { long.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-9223372036854775808\":\"foo\",\"9223372036854775807\":\"bar\"}", str.ToString());
                }
            }

            // ulong
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ulong, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ulong.MinValue, "foo" },
                            { ulong.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"18446744073709551615\":\"bar\"}", str.ToString());
                }
            }
        }

        [TestMethod]
        public void IntegerDictionaryKeysWithoutNulls()
        {
            // byte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<byte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { byte.MinValue, "foo" },
                            { byte.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"255\":\"bar\"}", str.ToString());
                }
            }

            // sbyte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<sbyte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { sbyte.MinValue, "foo" },
                            { sbyte.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-128\":\"foo\",\"127\":\"bar\"}", str.ToString());
                }
            }

            // short
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<short, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { short.MinValue, "foo" },
                            { short.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-32768\":\"foo\",\"32767\":\"bar\"}", str.ToString());
                }
            }

            // ushort
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ushort, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ushort.MinValue, "foo" },
                            { ushort.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"65535\":\"bar\"}", str.ToString());
                }
            }

            // int
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<int, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { int.MinValue, "foo" },
                            { int.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-2147483648\":\"foo\",\"2147483647\":\"bar\"}", str.ToString());
                }
            }

            // uint
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<uint, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { uint.MinValue, "foo" },
                            { uint.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"4294967295\":\"bar\"}", str.ToString());
                }
            }

            // long
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<long, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { long.MinValue, "foo" },
                            { long.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-9223372036854775808\":\"foo\",\"9223372036854775807\":\"bar\"}", str.ToString());
                }
            }

            // ulong
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ulong, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ulong.MinValue, "foo" },
                            { ulong.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"18446744073709551615\":\"bar\"}", str.ToString());
                }
            }
        }

        [TestMethod]
        public void Guids()
        {
            // defaults
            {
                using (var str = new StringWriter())
                {
                    var guid = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD");

                    JSON.Serialize(guid, str);

                    Assert.AreEqual("\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\"", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidLists = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") };

                    JSON.Serialize(guidLists, str);

                    Assert.AreEqual("[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"]", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidDict = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } };

                    JSON.Serialize(guidDict, str);

                    Assert.AreEqual("{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"}", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new
                        {
                            A = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"),
                            B = (Guid?)null,
                            C = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") },
                            D = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } }
                        },
                        str
                    );

                    Assert.AreEqual("{\"C\":[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"],\"D\":{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"},\"A\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"B\":null}", str.ToString());
                }
            }

            // exclude nulls
            {
                using (var str = new StringWriter())
                {
                    var guid = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD");

                    JSON.Serialize(guid, str, Options.ExcludeNulls);

                    Assert.AreEqual("\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\"", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidLists = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") };

                    JSON.Serialize(guidLists, str, Options.ExcludeNulls);

                    Assert.AreEqual("[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"]", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidDict = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } };

                    JSON.Serialize(guidDict, str, Options.ExcludeNulls);

                    Assert.AreEqual("{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"}", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new
                        {
                            A = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"),
                            B = (Guid?)null,
                            C = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") },
                            D = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"C\":[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"],\"D\":{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"},\"A\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\"}", str.ToString());
                }
            }

            // pretty print
            {
                using (var str = new StringWriter())
                {
                    var guid = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD");

                    JSON.Serialize(guid, str, Options.PrettyPrint);

                    Assert.AreEqual("\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\"", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidLists = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") };

                    JSON.Serialize(guidLists, str, Options.PrettyPrint);

                    Assert.AreEqual("[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"]", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidDict = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } };

                    JSON.Serialize(guidDict, str, Options.PrettyPrint);

                    Assert.AreEqual("{\n \"hello\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\n \"world\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"\n}", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new
                        {
                            A = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"),
                            B = (Guid?)null,
                            C = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") },
                            D = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } }
                        },
                        str,
                        Options.PrettyPrint
                    );

                    Assert.AreEqual("{\n \"C\": [\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"],\n \"D\": {\n  \"hello\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\n  \"world\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"\n },\n \"A\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\n \"B\": null\n}", str.ToString());
                }
            }
        }

        [TestMethod]
        public void StringEscapes()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize("\"sup\b\t\f\n\r\0\"", str);
                Assert.AreEqual("\"\\\"sup\\b\\t\\f\\n\\r\\u0000\\\"\"", str.ToString());
            }

            // Don't waste time in JSON-mode
            using (var str = new StringWriter())
            {
                JSON.Serialize("\"sup\b\t\f\n\r\0\"\u2028\u2029", str);
                Assert.AreEqual("\"\\\"sup\\b\\t\\f\\n\\r\\u0000\\\"\u2028\u2029\"", str.ToString());
            }

            // But if this is JSONP, we have to spend some time encoding
            using (var str = new StringWriter())
            {
                JSON.Serialize("\"sup\b\t\f\n\r\0\"\u2028\u2029", str, Options.JSONP);
                Assert.AreEqual("\"\\\"sup\\b\\t\\f\\n\\r\\u0000\\\"\\u2028\\u2029\"", str.ToString());
            }
        }

        struct _SerializeDynamicStruct
        {
            public int A;
            public bool B;
        }

        [TestMethod]
        public void SerializeDynamic()
        {
            using (var str = new StringWriter())
            {
                JSON.SerializeDynamic(
                    new
                    {
                        A = 1,
                        B = (int?)null,
                        C = "hello world"
                    },
                    str
                );

                Assert.AreEqual("{\"A\":1,\"C\":\"hello world\",\"B\":null}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.SerializeDynamic(
                    new _SerializeDynamicStruct
                    {
                        A = 1,
                        B = false
                    },
                    str
                );

                Assert.AreEqual("{\"A\":1,\"B\":false}", str.ToString());
            }
        }

        class _LotsOfStrings
        {
            public string A;
            public string B;
            public string C;
            public string D;
            public string E;
            public string F;
            public string G;
            public string H;
            public string I;
            public string J;
            public string K;
        }

        [TestMethod]
        public void LotsOfStrings()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings { },
                    str
                );

                Assert.AreEqual("{\"A\":null,\"B\":null,\"C\":null,\"D\":null,\"E\":null,\"F\":null,\"G\":null,\"H\":null,\"I\":null,\"J\":null,\"K\":null}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings 
                    {
                        A = "hello",
                        C = "world",
                        E = "fizz",
                        G = "buzz",
                        I = "foo",
                        K = "bar"
                    },
                    str
                );

                Assert.AreEqual("{\"A\":\"hello\",\"B\":null,\"C\":\"world\",\"D\":null,\"E\":\"fizz\",\"F\":null,\"G\":\"buzz\",\"H\":null,\"I\":\"foo\",\"J\":null,\"K\":\"bar\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings
                    {
                        A = "hello",
                        B = "world",
                        D = "fizz",
                        E = "buzz",
                        G = "foo",
                        H = "bar",
                        J = "syn",
                        K = "ack"
                    },
                    str
                );

                Assert.AreEqual("{\"A\":\"hello\",\"B\":\"world\",\"C\":null,\"D\":\"fizz\",\"E\":\"buzz\",\"F\":null,\"G\":\"foo\",\"H\":\"bar\",\"I\":null,\"J\":\"syn\",\"K\":\"ack\"}", str.ToString());
            }
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings
                    {
                        B = "hello",
                        C = "world",
                        E = "fizz",
                        F = "buzz",
                        H = "foo",
                        I = "bar",
                        K = "syn"
                    },
                    str
                );

                Assert.AreEqual("{\"A\":null,\"B\":\"hello\",\"C\":\"world\",\"D\":null,\"E\":\"fizz\",\"F\":\"buzz\",\"G\":null,\"H\":\"foo\",\"I\":\"bar\",\"J\":null,\"K\":\"syn\"}", str.ToString());
            }
        }
    }
}
