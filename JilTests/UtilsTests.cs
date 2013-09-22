using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jil.Serialize;
using System.Reflection;
using System.IO;

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

        [TestMethod]
        public void FastDouble()
        {
            for (var i = 0.1; i <= 1.0; i += 0.1)
            {
                for (var j = 1; j <= 10000; j++)
                {
                    var tryD = i * j;

                    using (var str = new StringWriter())
                    {
                        Utils.FormatDouble(str, tryD);

                        var asStr = str.ToString();

                        Assert.AreEqual(tryD.ToString(), double.Parse(asStr).ToString());
                    }
                }
            }

            for (var i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                for (ushort j = 1; j < (ushort)Math.Sqrt(i); j++)
                {
                    double trial = i;
                    trial /= (double)j;

                    using (var a = new StringWriter())
                    {
                        Utils.FormatDouble(a, trial);
                        var aStr = a.ToString();
                        Assert.AreEqual(trial.ToString(), double.Parse(aStr).ToString());
                    }

                    trial = -trial;

                    using (var b = new StringWriter())
                    {
                        Utils.FormatDouble(b, trial);
                        var bStr = b.ToString();
                        Assert.AreEqual(trial.ToString(), double.Parse(bStr).ToString());
                    }
                }
            }
        }
    }
}
