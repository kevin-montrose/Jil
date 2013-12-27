using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jil.Serialize;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace JilTests
{
    [TestClass]
    public class UtilsTests
    {
#pragma warning disable 0649
        class _FieldOffsetsInMemory
        {
            public int Foo;
            public string Bar;
            public double Fizz;
            public decimal Buzz;
            public char Hello;
            public object[] World;
        }
#pragma warning restore 0649

        [TestMethod]
        public void ParseISO8601()
        {
            var buffer = new char[Jil.Deserialize.Methods.CharBufferSize];

            using (var str = new StringReader("\"1900\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1991-02\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1991, 02, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12,5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12.5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 0 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5+01:23\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 0 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 30 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5+0123\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 12 + 1, 34 + 23, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 15, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5-11:45\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 15, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 45, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5-1145\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1989, 01, 31, 0, 49, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30Z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01t12:30+00\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30z\""))
            {
                str.Read(); // skip the "
                var dt = (DateTime)Jil.Deserialize.Methods.ReadISO8601Date.Invoke(null, new object[] { str, buffer });
                Assert.AreEqual(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }
        }

        [TestMethod]
        public void FieldOffsetsInMemory()
        {
            Func<string, FieldInfo> get = str => typeof(_FieldOffsetsInMemory).GetField(str);

            var offset = Utils.FieldOffsetsInMemory(typeof(_FieldOffsetsInMemory));

            Assert.IsNotNull(offset);
            Assert.IsTrue(offset.ContainsKey(get("Foo")));
            Assert.IsTrue(offset.ContainsKey(get("Bar")));
            Assert.IsTrue(offset.ContainsKey(get("Fizz")));
            Assert.IsTrue(offset.ContainsKey(get("Buzz")));
            Assert.IsTrue(offset.ContainsKey(get("Hello")));
            Assert.IsTrue(offset.ContainsKey(get("World")));
        }

#pragma warning disable 0649
        class _PropertyFieldUsage
        {
            private string _Foo;
            public string Foo
            {
                get
                {
                    return _Foo;
                }
            }

            private int _Scaler;
            public int SomeProp
            {
                get
                {
                    var x = int.Parse(_Foo);

                    var y = Console.ReadLine();

                    var sum = x + int.Parse(y);

                    return sum * _Scaler;
                }
            }
        }
#pragma warning restore 0649

        [TestMethod]
        public void PropertyFieldUsage()
        {
            var use = Utils.PropertyFieldUsage(typeof(_PropertyFieldUsage));

            Assert.IsNotNull(use);
            Assert.AreEqual(1, use[typeof(_PropertyFieldUsage).GetProperty("Foo")].Count);
            Assert.AreEqual(typeof(_PropertyFieldUsage).GetField("_Foo", BindingFlags.NonPublic | BindingFlags.Instance), use[typeof(_PropertyFieldUsage).GetProperty("Foo")][0]);

            Assert.AreEqual(2, use[typeof(_PropertyFieldUsage).GetProperty("SomeProp")].Count);
            Assert.AreEqual(typeof(_PropertyFieldUsage).GetField("_Foo", BindingFlags.NonPublic | BindingFlags.Instance), use[typeof(_PropertyFieldUsage).GetProperty("SomeProp")][0]);
            Assert.AreEqual(typeof(_PropertyFieldUsage).GetField("_Scaler", BindingFlags.NonPublic | BindingFlags.Instance), use[typeof(_PropertyFieldUsage).GetProperty("SomeProp")][1]);
        }

        private static string CapacityEstimatorToString<T>(Action<TextWriter, T, int> act, T data)
        {
            using (var str = new StringWriter())
            {
                act(str, data, 0);

                return str.ToString();
            }
        }

        class _CapacityEstimator
        {
            public int Hello { get; set; }
            public double World { get; set; }
        }

        [TestMethod]
        public void CapacityEstimator()
        {
            {
                var serialize = InlineSerializerHelper.Build<int>();
                var cap = CapacityCache.Get<int>(Jil.Options.Default);
                Assert.AreEqual(16, cap);
                var str = CapacityEstimatorToString(serialize, 123);
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length < 16 || str.Length > cap / 2); // special case
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(int), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<_CapacityEstimator>();
                var cap = CapacityCache.Get<_CapacityEstimator>(Jil.Options.Default);
                Assert.AreEqual(32, cap);
                var str = CapacityEstimatorToString(serialize, new _CapacityEstimator { Hello = 456, World = 10.2 });
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(_CapacityEstimator), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<List<int>>();
                var cap = CapacityCache.Get<List<int>>(Jil.Options.Default);
                Assert.AreEqual(64, cap);
                var str = CapacityEstimatorToString(serialize, new List<int> { 123, 456, 789, -12, 345, 678, 901, 234, 567, 890 });
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(List<int>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<List<Guid>>();
                var cap = CapacityCache.Get<List<Guid>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.ListMultiplier).Select(_ => Guid.NewGuid()).ToList());
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(List<Guid>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<List<DateTime>>();
                var cap = CapacityCache.Get<List<DateTime>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.ListMultiplier).Select(_ => DateTime.UtcNow).ToList());
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(List<DateTime>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<Dictionary<int, int>>();
                var cap = CapacityCache.Get<Dictionary<int, int>>(Jil.Options.Default);
                Assert.AreEqual(128, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.DictionaryMultiplier).ToDictionary(_ => 100 + _, _ => 100 + _));
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(Dictionary<int, int>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<Dictionary<string, int>>();
                var cap = CapacityCache.Get<Dictionary<string, int>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.DictionaryMultiplier).ToDictionary(_ => "1234567890123456789" + _.ToString("X"), _ => 100 + _));
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(Dictionary<string, int>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<Dictionary<string, _CapacityEstimator>>();
                var cap = CapacityCache.Get<Dictionary<string, _CapacityEstimator>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.DictionaryMultiplier).ToDictionary(_ => "1234567890123456789" + _.ToString("X"), _ => new _CapacityEstimator { Hello = 456, World = 10.2 }));
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(Dictionary<string, _CapacityEstimator>), Jil.Options.Default, 0));
            }
        }
    }
}
