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
