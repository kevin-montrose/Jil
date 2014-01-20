﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jil.Serialize;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

        private static string CapacityEstimatorToString<T>(Action<TextWriter, T, int> act, T data)
        {
            using (var str = new StringWriter())
            {
                act(str, data, 0);

                return str.ToString();
            }
        }

        class _CapacityEstimator1
        {
            public int Hello { get; set; }
            public double World { get; set; }
        }

        [TestMethod]
        public void CapacityEstimator()
        {
            {
                var serialize = InlineSerializerHelper.Build<int>();
                var cap = CapacityCache.Get<int>(Jil.Options.Default);
                Assert.AreEqual(16, cap);
                var str = CapacityEstimatorToString(serialize, 123);
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length < 16 || str.Length > cap / 2); // special case, as 2
            }

            {
                var serialize = InlineSerializerHelper.Build<_CapacityEstimator1>();
                var cap = CapacityCache.Get<_CapacityEstimator1>(Jil.Options.Default);
                Assert.AreEqual(32, cap);
                var str = CapacityEstimatorToString(serialize, new _CapacityEstimator1 { Hello = 456, World = 10.2 });
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
            }

            {
                var serialize = InlineSerializerHelper.Build<List<int>>();
                var cap = CapacityCache.Get<List<int>>(Jil.Options.Default);
                Assert.AreEqual(128, cap);
                var str = CapacityEstimatorToString(serialize, new List<int> { 123, 456, 789, 012, 345, 678, 901, 234, 567, 890 });
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
            }
        }
    }
}
