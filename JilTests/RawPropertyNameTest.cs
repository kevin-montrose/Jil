using System.IO;
using Jil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JilTests
{
    [TestClass]
    public class RawPropertyNameTest
    {
        [TestMethod]
        public void RawPropertyFromString()
        {
            var item1Json = "{\"id\":\"1\", \"additional\":90}";
            var item2Json = "{\"id\":\"2\"}";
            var dataToParse = $"{{\"data\":[{item1Json}, {item2Json}]}}";
            var parsed = JSON.Deserialize<Items>(dataToParse);
            Assert.AreEqual(item1Json,parsed.Data[0].Raw);
            Assert.AreEqual(item2Json,parsed.Data[1].Raw);
            Assert.AreEqual("1",parsed.Data[0].Id);
            Assert.AreEqual("2",parsed.Data[1].Id);
        }

        [TestMethod]
        public void RawPropertyFromStream()
        {
            var item1Json = "{\"id\":\"1\", \"additional\":90}";
            var item2Json = "{\"id\":\"2\"}";
            var dataToParse =new StringReader( $"{{\"data\":[{item1Json}, {item2Json}]}}");
            
            var parsed = JSON.Deserialize<Items>(dataToParse);
            Assert.AreEqual(item1Json,parsed.Data[0].Raw);
            Assert.AreEqual(item2Json,parsed.Data[1].Raw);
            Assert.AreEqual("1",parsed.Data[0].Id);
            Assert.AreEqual("2",parsed.Data[1].Id);
        }

        public class Items
        {
            [JilDirective(Name = "data")]
            public Item[] Data { get; set; }
        }

        [JilClassDirective("Raw")]
        public class Item
        {
            [JilDirective(Name = "id")]
            public string Id { get; set; }
            [JilDirective(Ignore = true)]
            public string Raw { get; set; }
        }
    }
}