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
    public class DeserializeDynamicTests
    {
        [TestMethod]
        public void Bool()
        {
            using (var str = new StringReader("true"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsTrue((bool)res);
            }

            using (var str = new StringReader("false"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsFalse((bool)res);
            }
        }

        [TestMethod]
        public void Null()
        {
            using (var str = new StringReader("null"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsNull(res);
            }
        }

        [TestMethod]
        public void Guids()
        {
            var guid = Guid.NewGuid();

            using (var str = new StringReader("\"" + guid + "\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(guid, (Guid)res);
            }
        }

        enum _Enums
        {
            Hello,
            World
        }

        [TestMethod]
        public void Enums()
        {
            using (var str = new StringReader("\"Hello\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.Hello, (_Enums)res);
            }

            using (var str = new StringReader("\"hello\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.Hello, (_Enums)res);
            }

            using (var str = new StringReader("\"World\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.World, (_Enums)res);
            }

            using (var str = new StringReader("\"world\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.World, (_Enums)res);
            }
        }

        [TestMethod]
        public void Number()
        {
            using (var str = new StringReader("1"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual(1.0, val);
            }

            using (var str = new StringReader("1.234"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual(1.234, val);
            }

            using (var str = new StringReader("-10e4"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual(-100000, val);
            }

            using (var str = new StringReader("-1.3e-4"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual(-0.00013, val);
            }
        }

        [TestMethod]
        public void String()
        {
            using (var str = new StringReader("\"hello world\""))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (string)res;
                Assert.AreEqual("hello world", val);
            }

            using (var str = new StringReader("\"H\\u0065llo\""))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (string)res;
                Assert.AreEqual("Hello", val);
            }
        }

        [TestMethod]
        public void Array()
        {
            using (var str = new StringReader("[123, \"hello\", true]"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(3, (int)res.Length);
                Assert.AreEqual(123, (int)res[0]);
                Assert.AreEqual("hello", (string)res[1]);
                Assert.AreEqual(true, (bool)res[2]);
            }

            using (var str = new StringReader("[]"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(0, (int)res.Length);
            }
        }

        [TestMethod]
        public void Object()
        {
            using (var str = new StringReader("{\"hello\": 123, \"world\":[1,2,3]}"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(123, (int)res["hello"]);
                var arr = res["world"];
                Assert.AreEqual(3, (int)arr.Length);
                Assert.AreEqual(1, (int)arr[0]);
                Assert.AreEqual(2, (int)arr[1]);
                Assert.AreEqual(3, (int)arr[2]);
            }

            using (var str = new StringReader("{}"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsNotNull(res);
                var c = 0;
                foreach (var x in res)
                {
                    c++;
                }
                Assert.AreEqual(0, c);
            }
        }

        [TestMethod]
        public void ObjectEnumeration()
        {
            using (var str = new StringReader("{\"hello\":123, \"world\":456, \"foo\":789}"))
            {
                var c = 0;
                var res = JSON.DeserializeDynamic(str);
                foreach (var kv in res)
                {
                    string key = kv.Key;
                    dynamic val = kv.Value;

                    switch(c){
                        case 0: 
                            Assert.AreEqual("hello", key);
                            Assert.AreEqual(123, (int)val);
                            break;
                        case 1:
                            Assert.AreEqual("world", key);
                            Assert.AreEqual(456, (int)val);
                            break;
                        case 2:
                            Assert.AreEqual("foo", key);
                            Assert.AreEqual(789, (int)val);
                            break;
                        default: throw new Exception();
                    }
                    c++;
                }
                Assert.AreEqual(3, c);
            }
        }

        [TestMethod]
        public void ArrayEnumerator()
        {
            using (var str = new StringReader("[\"abcd\", \"efgh\", \"ijkl\"]"))
            {
                var c = 0;
                var res = JSON.DeserializeDynamic(str);
                foreach (var val in res)
                {
                    switch (c)
                    {
                        case 0:
                            Assert.AreEqual("abcd", (string)val);
                            break;
                        case 1:
                            Assert.AreEqual("efgh", (string)val);
                            break;
                        case 2:
                            Assert.AreEqual("ijkl", (string)val);
                            break;
                        default: throw new Exception();
                    }
                    c++;
                }
                Assert.AreEqual(3, c);
            }
        }

        [TestMethod]
        public void ArrayCoercion()
        {
            using (var str = new StringReader("[123, 456, 789]"))
            {
                var res = JSON.DeserializeDynamic(str);
                IEnumerable<int> asArr = res;
                Assert.AreEqual(3, asArr.Count());
                Assert.AreEqual(123, asArr.ElementAt(0));
                Assert.AreEqual(456, asArr.ElementAt(1));
                Assert.AreEqual(789, asArr.ElementAt(2));
            }
        }

        enum _DictionaryCoercion
        {
            a,
            b,
            c
        }

        [TestMethod]
        public void DictionaryCoercion()
        {
            using (var str = new StringReader("{\"a\": 123, \"b\": 456, \"c\": 789}"))
            {
                var res = JSON.DeserializeDynamic(str);
                IDictionary<string, int> asDict = res;
                Assert.AreEqual(3, asDict.Count());
                Assert.IsTrue(asDict.ContainsKey("a"));
                Assert.AreEqual(123, asDict["a"]);
                Assert.IsTrue(asDict.ContainsKey("b"));
                Assert.AreEqual(456, asDict["b"]);
                Assert.IsTrue(asDict.ContainsKey("c"));
                Assert.AreEqual(789, asDict["c"]);
            }

            using (var str = new StringReader("{\"a\": 123, \"b\": 456, \"c\": 789}"))
            {
                var res = JSON.DeserializeDynamic(str);
                IDictionary<_DictionaryCoercion, int> asDict = res;
                Assert.AreEqual(3, asDict.Count());
                Assert.IsTrue(asDict.ContainsKey(_DictionaryCoercion.a));
                Assert.AreEqual(123, asDict[_DictionaryCoercion.a]);
                Assert.IsTrue(asDict.ContainsKey(_DictionaryCoercion.b));
                Assert.AreEqual(456, asDict[_DictionaryCoercion.b]);
                Assert.IsTrue(asDict.ContainsKey(_DictionaryCoercion.c));
                Assert.AreEqual(789, asDict[_DictionaryCoercion.c]);
            }
        }
    }
}
