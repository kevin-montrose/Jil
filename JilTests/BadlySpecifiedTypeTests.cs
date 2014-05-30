using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace JilTests
{
    [TestClass]
    public class BadlySpecifiedTypeTests
    {
        [TestMethod]
        public void SerializeString()
        {
            object obj = new { Foo = 123, Bar = "abc" };
            string s = Jil.JSON.Serialize(obj), t = Jil.JSON.SerializeDynamic(obj);
            Assert.AreEqual(t, s);
        }

        [TestMethod]
        public void SerializeWriter()
        {
            object obj = new { Foo = 123, Bar = "abc" };
            StringWriter s = new StringWriter(), t = new StringWriter();
            Jil.JSON.Serialize(obj, s);
            Jil.JSON.SerializeDynamic(obj, t);
            Assert.AreEqual(t.ToString(), s.ToString());
        }

        [TestMethod]
        public void DeserializeString()
        {
            string json = Jil.JSON.SerializeDynamic(new { Foo = 123, Bar = "abc" });
            dynamic s = Jil.JSON.Deserialize<object>(json),
                t = Jil.JSON.DeserializeDynamic(json);
            Assert.AreEqual((int)t.Foo, (int)s.Foo);
            Assert.AreEqual((string)t.Bar, (string)s.Bar);
        }

        [TestMethod]
        public void DeserializeReader()
        {
            string json = Jil.JSON.SerializeDynamic(new { Foo = 123, Bar = "abc" });
            dynamic s = Jil.JSON.Deserialize<object>(new StringReader(json)),
                t = Jil.JSON.DeserializeDynamic(new StringReader(json));
            Assert.AreEqual((int)t.Foo, (int)s.Foo);
            Assert.AreEqual((string)t.Bar, (string)s.Bar);
        }
    }
}
