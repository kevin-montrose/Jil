using Jil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JilTests
{
    [TestClass]
    public class DynamicSerializationTests
    {
        [TestMethod]
        public void ToStringJSON()
        {
            {
                var dyn = JSON.DeserializeDynamic("{\"Hello\":1}");
                var res = dyn.ToString();
                var shouldMatch = JSON.Serialize(new { Hello = 1 }, Options.ISO8601PrettyPrintNoHashing);
                Assert.AreEqual(shouldMatch, res);
            }

            {
                var dyn1 = JSON.DeserializeDynamic(long.MaxValue.ToString());
                var dyn2 = JSON.DeserializeDynamic(ulong.MaxValue.ToString());
                var dyn3 = JSON.DeserializeDynamic(long.MinValue.ToString());
                var res1 = dyn1.ToString();
                var res2 = dyn2.ToString();
                var res3 = dyn3.ToString();
                Assert.AreEqual(long.MaxValue.ToString(), res1);
                Assert.AreEqual(ulong.MaxValue.ToString(), res2);
                Assert.AreEqual(long.MinValue.ToString(), res3);
            }

            {
                var dyn = JSON.DeserializeDynamic("1.23456");
                var res = dyn.ToString();
                Assert.AreEqual("1.23456", res);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("true");
                var dyn2 = JSON.DeserializeDynamic("false");
                var res1 = dyn1.ToString();
                var res2 = dyn2.ToString();
                Assert.AreEqual("true", res1);
                Assert.AreEqual("false", res2);
            }

            {
                var now = DateTime.UtcNow;
                var str = JSON.Serialize(now, Options.ISO8601);
                var dyn = JSON.DeserializeDynamic(str, Options.ISO8601);
                var res = dyn.ToString();
                Assert.AreEqual(str, res);
            }

            {
                var g = Guid.NewGuid();
                var str = JSON.Serialize(g);
                var dyn = JSON.DeserializeDynamic(str);
                var res = dyn.ToString();
                Assert.AreEqual(str, res);
            }

            {
                var dyn = JSON.DeserializeDynamic("\"how are you today?\"");
                var str = dyn.ToString();
                Assert.AreEqual("\"how are you today?\"", str);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("[1,2,3]");
                var dyn2 = JSON.DeserializeDynamic("[]");
                var dyn3 = JSON.DeserializeDynamic("[1, \"hello\", {}, 456]");
                var res1 = dyn1.ToString();
                var res2 = dyn2.ToString();
                var res3 = dyn3.ToString();
                var shouldMatch1 = JSON.Serialize(new[] { 1, 2, 3 }, Options.ISO8601PrettyPrintNoHashing);
                var shouldMatch2 = JSON.Serialize(new object[0], Options.ISO8601PrettyPrintNoHashing);
                var shouldMatch3 =
                    "[" +
                    JSON.Serialize(1, Options.ISO8601PrettyPrintNoHashing) +
                    ", " +
                    JSON.Serialize("hello", Options.ISO8601PrettyPrintNoHashing) +
                    ", " +
                    JSON.Serialize(new { }, Options.ISO8601PrettyPrintNoHashing) +
                    ", " +
                    JSON.Serialize(456, Options.ISO8601PrettyPrintNoHashing) +
                    "]";
                Assert.AreEqual(shouldMatch1, res1);
                Assert.AreEqual(shouldMatch2, res2);
                Assert.AreEqual(shouldMatch3, res3);
            }
        }

        [TestMethod]
        public void HeterogenousCollection()
        {
            using (var str = new StringWriter())
            {
                var dict = (dynamic)new ExpandoObject();
                dict.Fizz = "Buzz";
                var arr = new object[] { 123, "hello", new { Foo = "bar" }, dict };

                JSON.SerializeDynamic(arr, str);
                var res = str.ToString();

                Assert.AreEqual("[123,\"hello\",{\"Foo\":\"bar\"},{\"Fizz\":\"Buzz\"}]", res);
            }
        }

        [TestMethod]
        public void Objects()
        {
            using (var str = new StringWriter())
            {
                var dict = (dynamic)new ExpandoObject();
                dict.Foo = 123;
                dict.Bar = "hello";
                JSON.SerializeDynamic(dict, str);
                var res = str.ToString();
                Assert.AreEqual("{\"Foo\":123,\"Bar\":\"hello\"}", res);
            }
        }

        [TestMethod]
        public void Simple()
        {
            using(var str = new StringWriter())
            {
                JSON.SerializeDynamic(123, str);
                var res = str.ToString();
                Assert.AreEqual("123", res);
            }

            using (var str = new StringWriter())
            {
                JSON.SerializeDynamic("hello", str);
                var res = str.ToString();
                Assert.AreEqual("\"hello\"", res);
            }

            using (var str = new StringWriter())
            {
                JSON.SerializeDynamic(null, str);
                var res = str.ToString();
                Assert.AreEqual("null", res);
            }

            using (var str = new StringWriter())
            {
                var now = DateTime.UtcNow;
                JSON.SerializeDynamic(now, str);
                var res = str.ToString();
                var dt = JSON.Deserialize<DateTime>(res);
                Assert.IsTrue((now - dt).Duration() < TimeSpan.FromMilliseconds(1));
            }
        }
    }
}
