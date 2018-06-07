using System.IO;
using Xunit;

namespace JilTests
{
    public class BadlySpecifiedTypeTests
    {
        [Fact]
        public void SerializeString()
        {
            object obj = new { Foo = 123, Bar = "abc" };
            string s = Jil.JSON.Serialize(obj), t = Jil.JSON.SerializeDynamic(obj);
            Assert.Equal(t, s);
        }

        [Fact]
        public void SerializeWriter()
        {
            object obj = new { Foo = 123, Bar = "abc" };
            StringWriter s = new StringWriter(), t = new StringWriter();
            Jil.JSON.Serialize(obj, s);
            Jil.JSON.SerializeDynamic(obj, t);
            Assert.Equal(t.ToString(), s.ToString());
        }

        [Fact]
        public void DeserializeString()
        {
            string json = Jil.JSON.SerializeDynamic(new { Foo = 123, Bar = "abc" });
            dynamic s = Jil.JSON.Deserialize<object>(json),
                t = Jil.JSON.DeserializeDynamic(json);
            Assert.Equal((int)t.Foo, (int)s.Foo);
            Assert.Equal((string)t.Bar, (string)s.Bar);
        }

        [Fact]
        public void DeserializeReader()
        {
            string json = Jil.JSON.SerializeDynamic(new { Foo = 123, Bar = "abc" });
            dynamic s = Jil.JSON.Deserialize<object>(new StringReader(json)),
                t = Jil.JSON.DeserializeDynamic(new StringReader(json));
            Assert.Equal((int)t.Foo, (int)s.Foo);
            Assert.Equal((string)t.Bar, (string)s.Bar);
        }
    }
}
