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
    public class Serialize
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
    }
}
