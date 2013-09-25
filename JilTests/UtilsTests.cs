using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jil.Serialize;
using System.Reflection;
using System.IO;
using System.Linq;

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

#pragma warning disable 0649
        public class _GetSimplePropertyBackingField
        {
            public int Foo { get; set; }
            
            private double _Bar;
            public double Bar { get { return _Bar; } }
        }
#pragma warning restore 0649

        [TestMethod]
        public void GetSimplePropertyBackingField()
        {
            var foo = Utils.GetSimplePropertyBackingField(typeof(_GetSimplePropertyBackingField).GetProperty("Foo").GetMethod);
            Assert.IsNotNull(foo);

            var bar = Utils.GetSimplePropertyBackingField(typeof(_GetSimplePropertyBackingField).GetProperty("Bar").GetMethod);
            Assert.IsNotNull(bar);
        }

        [TestMethod]
        public void V8FastDouble()
        {
            var buffer = Enumerable.Range(0, 30).Select(c => (char)'0').ToList();

            bool b;
            int len, p;
            Utils.DoubleToAscii(100.0, 10, buffer, buffer.Count, out b, out len, out p);
            Utils.DoubleToAscii(123.4, 10, buffer, buffer.Count, out b, out len, out p);
            Utils.DoubleToAscii(.01234, 10, buffer, buffer.Count, out b, out len, out p);
        }
    }
}
