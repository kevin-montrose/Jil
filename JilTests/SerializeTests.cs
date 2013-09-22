using Jil;
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

                Assert.AreEqual("{\"Three\":{\"_\":456,\"Leading\":\"Foo Bar\"},\"Two\":{\"_\":123,\"Trailing\":\"Fizz Buzz\"},\"One\":{\"Single\":\"Hello World\"}}", res);
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
                        { "fizz buzz", 456 }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"hello world\":123,\"fizz buzz\":456}", res);
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
                Assert.AreEqual("", str.ToString());
            }
        }
    }
}
