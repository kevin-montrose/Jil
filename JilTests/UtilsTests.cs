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
            var buffer = new char[18];

            using (var str = new StringWriter())
            {
                Utils.DoubleToAscii(str, 100.0, buffer);
                Assert.AreEqual("100", str.ToString());
            }
            using (var str = new StringWriter())
            {
                Utils.DoubleToAscii(str, 123, buffer);
                Assert.AreEqual("123", str.ToString());
            }
            using (var str = new StringWriter())
            {
                Utils.DoubleToAscii(str, 1.23, buffer);
                Assert.AreEqual("1.23", str.ToString());
            }
            using (var str = new StringWriter())
            {
                Utils.DoubleToAscii(str, 0.0101, buffer);
                Assert.AreEqual("0.0101", str.ToString());
            }
            using (var str = new StringWriter())
            {
                Utils.DoubleToAscii(str, -123.456789, buffer);
                Assert.AreEqual("-123.456789", str.ToString());
            }

            Func<double, string, bool> closeEnough =
                (actual, asStr) =>
                {
                    var reparsed = double.Parse(asStr);

                    return reparsed.ToString() == actual.ToString();
                };

            int failures = 0;
            int total = 0;

            for (double i = 0; i < 100; i += 0.1)
            {
                for (double j = 1; j < 100; j += 0.321)
                {
                    var m = i * j;
                    using (var str = new StringWriter())
                    {
                        if (!Utils.DoubleToAscii(str, m, buffer))
                        {
                            failures++;
                        }
                        Assert.IsTrue(closeEnough(m, str.ToString()), m + " != " + str.ToString());

                        total++;
                    }
                }
            }

            double ratio = (((double)failures) / (double)total) * 100.0;

            // Expected rate of failure
            Assert.IsTrue(ratio <= 0.5);
        }
    }
}
