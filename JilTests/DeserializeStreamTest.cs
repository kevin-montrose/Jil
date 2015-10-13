using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jil;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace JilTests
{
    [TestClass]
    public class DeserializeStreamTest
    {

#pragma warning disable 0649
        struct _ValueTypes
        {
            public string A;
            public int B { get; set; }
        }
#pragma warning restore 0649

        [TestMethod]
        public void ArrayStream()
        {
            using (var str = new StringReader("[0,1,2,3,4,5][6,7,8]\r\n[10,11,12,13,14,15] [20,21,22,23,24,25]"))
            {
                var ret = JSON.DeserializeStream<int[]>(str).ToList();
                Assert.AreEqual(4, ret.Count);

                Assert.AreEqual(6, ret[0].Length);
                Assert.AreEqual(0, ret[0][0]);
                Assert.AreEqual(1, ret[0][1]);
                Assert.AreEqual(2, ret[0][2]);
                Assert.AreEqual(3, ret[0][3]);
                Assert.AreEqual(4, ret[0][4]);
                Assert.AreEqual(5, ret[0][5]);

                Assert.AreEqual(3, ret[1].Length);
                Assert.AreEqual(6, ret[1][0]);
                Assert.AreEqual(7, ret[1][1]);
                Assert.AreEqual(8, ret[1][2]);

                Assert.AreEqual(6, ret[2].Length);
                Assert.AreEqual(10, ret[2][0]);
                Assert.AreEqual(11, ret[2][1]);
                Assert.AreEqual(12, ret[2][2]);
                Assert.AreEqual(13, ret[2][3]);
                Assert.AreEqual(14, ret[2][4]);
                Assert.AreEqual(15, ret[2][5]);

                Assert.AreEqual(6, ret[3].Length);
                Assert.AreEqual(20, ret[3][0]);
                Assert.AreEqual(21, ret[3][1]);
                Assert.AreEqual(22, ret[3][2]);
                Assert.AreEqual(23, ret[3][3]);
                Assert.AreEqual(24, ret[3][4]);
                Assert.AreEqual(25, ret[3][5]);
            }
        }



        [TestMethod]
        public void ISO8601Stream()
        {
            using (var str = new StringReader("\"1900\"\"1991-02\"\"1989-01-31\" \"1989-01-31T12.5\" \"1989-01-31T12:34:56,5\" \"1989-01-31T12Z\" \"1989-01-31T12+01:23\" \"19890131T12-1145\" \"2004366\" "))
            using (var enumerator = JSON.DeserializeStream<DateTime>(str, Options.ISO8601).GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                var dt = enumerator.Current;
                Assert.AreEqual(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                Assert.AreEqual(new DateTime(1991, 02, 1, 0, 0, 0, DateTimeKind.Local), dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 00, 0, new TimeSpan(01, 23, 00));
                Assert.AreEqual(shouldMatch.UtcDateTime, dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.AreEqual(shouldMatch.UtcDateTime, dt);

                Assert.IsTrue(enumerator.MoveNext());
                dt = enumerator.Current;
                Assert.AreEqual(new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dt);

                // no more items expected
                Assert.IsFalse(enumerator.MoveNext());
            }
        }


        [TestMethod]
        public void MalformedISO8601Stream()
        {
            using (var str = new StringReader("\"1900\"\"99\"\"1989-01-31\""))
            using (var enumerator = JSON.DeserializeStream<DateTime>(str, Options.ISO8601).GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                var dt = enumerator.Current;
                Assert.AreEqual(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), dt);

                try
                {
                    Assert.IsTrue(enumerator.MoveNext());
                    Assert.Fail("Shouldn't be possible");
                }
                catch (DeserializationException e)
                {
                    Assert.AreEqual("ISO8601 date must begin with a 4 character year", e.Message);
                }
            }
        }

        [TestMethod]
        public void ValueTypeStream()
        {
            using (var str = new StringReader("{\"A\":\"hello\\u0000world\", \"B\":12345}\r\n" +
                "{\"A\":\"foobar\", \"B\":678}\r\n" +
                "{\"A\":null, \"B\":91011}\r\n"))
            {
                var res = JSON.DeserializeStream<_ValueTypes>(str).ToList();

                Assert.AreEqual(3, res.Count);

                Assert.AreEqual("hello\0world", res[0].A);
                Assert.AreEqual(12345, res[0].B);

                Assert.AreEqual("foobar", res[1].A);
                Assert.AreEqual(678, res[1].B);

                Assert.IsNull(res[2].A);
                Assert.AreEqual(91011, res[2].B);
            }
        }

        [TestMethod]
        public void DictionaryStream()
        {
            #region test data

            var firstDict = new Dictionary<string, string>
            {
                {"A", "test" },
                {"B", null },
                {"C", DateTime.Now.ToShortDateString() }
            };
            var secondDict = new Dictionary<string, string>
            {
                {"D", "foo" },
                {"E", "bar" },
                {"F", DateTime.UtcNow.ToLongTimeString() }
            };
            var thirdDict = new Dictionary<string, string>
            {
                {"A", "lorem" },
                {"E", "ipsum" },
                {"G", "..." }
            };

            #endregion

            var options = new Options(true, false, false, DateTimeFormat.ISO8601, false, UnspecifiedDateTimeKindBehavior.IsLocal);

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.Default, 1024, true))
                {
                    JSON.Serialize(firstDict, writer, Options.ISO8601CamelCase);
                    JSON.Serialize(secondDict, writer, options);
                    writer.WriteLine();
                    JSON.Serialize(thirdDict, writer, options);
                }

                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    var res = JSON.DeserializeStream<Dictionary<String, string>>(reader, options).ToList();
                    Assert.AreEqual(3, res.Count);
                    CollectionAssert.AreEqual(firstDict, res[0], "1st Dictionary");
                    CollectionAssert.AreEqual(secondDict, res[1], "2nd Dictionary");
                    CollectionAssert.AreEqual(thirdDict, res[2], "3rd Dictionary");
                }
            }
        }


        [TestMethod]
        [ExpectedException(typeof(DeserializationException))]
        [Description("When a stream contains data, at least one object is expected.")]
        public void WhitespaceStream()
        {
            using (var str = new StringReader("\t \r\n "))
            {
                var res = JSON.DeserializeStream<_ValueTypes>(str).ToList();                
            }
        }

        [TestMethod]        
        public void SingleValueStream()
        {
            using (var str = new StringReader("{\"A\":\"hello world\", \"B\":12345}"))
            {
                var res = JSON.DeserializeStream<_ValueTypes>(str).Single();
                Assert.AreEqual("hello world", res.A);
                Assert.AreEqual(12345, res.B);
            }
        }

        [TestMethod]
        public void EmptyStream()
        {
            using (var str = new StringReader(String.Empty))
            {
                var res = JSON.DeserializeStream<_ValueTypes>(str).ToList();
                Assert.AreEqual(0, res.Count);
            }
        }
    }
}
