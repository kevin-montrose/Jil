using System;
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

        class _CapacityEstimator
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
                Assert.IsTrue(str.Length < 16 || str.Length > cap / 2); // special case
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(int), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<_CapacityEstimator>();
                var cap = CapacityCache.Get<_CapacityEstimator>(Jil.Options.Default);
                Assert.AreEqual(32, cap);
                var str = CapacityEstimatorToString(serialize, new _CapacityEstimator { Hello = 456, World = 10.2 });
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(_CapacityEstimator), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<List<int>>();
                var cap = CapacityCache.Get<List<int>>(Jil.Options.Default);
                Assert.AreEqual(64, cap);
                var str = CapacityEstimatorToString(serialize, new List<int> { 123, 456, 789, -12, 345, 678, 901, 234, 567, 890 });
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(List<int>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<List<Guid>>();
                var cap = CapacityCache.Get<List<Guid>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.ListMultiplier).Select(_ => Guid.NewGuid()).ToList());
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(List<Guid>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<List<DateTime>>();
                var cap = CapacityCache.Get<List<DateTime>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.ListMultiplier).Select(_ => DateTime.UtcNow).ToList());
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(List<DateTime>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<Dictionary<int, int>>();
                var cap = CapacityCache.Get<Dictionary<int, int>>(Jil.Options.Default);
                Assert.AreEqual(128, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.DictionaryMultiplier).ToDictionary(_ => 100 + _, _ => 100 + _));
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(Dictionary<int, int>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<Dictionary<string, int>>();
                var cap = CapacityCache.Get<Dictionary<string, int>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.DictionaryMultiplier).ToDictionary(_ => "1234567890123456789" + _.ToString("X"), _ => 100 + _));
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(Dictionary<string, int>), Jil.Options.Default, 0));
            }

            {
                var serialize = InlineSerializerHelper.Build<Dictionary<string, _CapacityEstimator>>();
                var cap = CapacityCache.Get<Dictionary<string, _CapacityEstimator>>(Jil.Options.Default);
                Assert.AreEqual(512, cap);
                var str = CapacityEstimatorToString(serialize, Enumerable.Range(0, Jil.Serialize.CapacityEstimator.DictionaryMultiplier).ToDictionary(_ => "1234567890123456789" + _.ToString("X"), _ => new _CapacityEstimator { Hello = 456, World = 10.2 }));
                Assert.IsTrue(str.Length <= cap);
                Assert.IsTrue(str.Length > cap / 2);
                Assert.AreEqual(str.Length, Jil.Serialize.CapacityEstimator.For(typeof(Dictionary<string, _CapacityEstimator>), Jil.Options.Default, 0));
            }
        }

#if !DEBUG
#pragma warning disable 0649
        class _IsConstant
        {
            public char ConstCharProp { get { return 'c'; } }
            public char NonConstCharProp { get { return char.Parse("c"); } }
            public const char ConstCharField = 'c';
            public char NonConstCharField;

            public string ConstStringProp { get { return "hello world"; } }
            public string NonConstStringProp { get { return 3.ToString(); } }
            public const string ConstStringField = "fizz buzz";
            public string NonConstStringField;

            public byte ConstByteProp { get { return 10; } }
            public byte NonConstByteProp { get { return byte.Parse("2"); } }
            public const byte ConstByteField = 255;
            public byte NonConstByteField;

            public sbyte ConstSByteProp { get { return 10; } }
            public sbyte NonConstSByteProp { get { return sbyte.Parse("2"); } }
            public const sbyte ConstSByteField = 127;
            public sbyte NonConstSByteField;

            public short ConstShortProp { get { return 10; } }
            public short NonConstShortProp { get { return short.Parse("2"); } }
            public const short ConstShortField = 10000;
            public short NonConstShortField;

            public ushort ConstUShortProp { get { return 10; } }
            public ushort NonConstUShortProp { get { return ushort.Parse("2"); } }
            public const ushort ConstUShortField = 10000;
            public ushort NonConstUShortField;

            public int ConstIntProp { get { return 10; } }
            public int NonConstIntProp { get { return int.Parse("2"); } }
            public const int ConstIntField = 456;
            public int NonConstIntField;

            public uint ConstUIntProp { get { return 10; } }
            public uint NonConstUIntProp { get { return uint.Parse("2"); } }
            public const uint ConstUIntField = 456;
            public uint NonConstUIntField;

            public long ConstLongProp { get { return 10L; } }
            public long NonConstLongProp { get { return long.Parse("2"); } }
            public const long ConstLongField = 456;
            public long NonConstLongField;

            public ulong ConstULongProp { get { return 10UL; } }
            public ulong NonConstULongProp { get { return ulong.Parse("2"); } }
            public const ulong ConstULongField = 456;
            public ulong NonConstULongField;

            public float ConstFloatProp { get { return 123; } }
            public float NonConstFloatProp { get { return float.Parse("2"); } }
            public const float ConstFloatField = 456;
            public float NonConstFloatField;

            public double ConstDoubleProp { get { return 123; } }
            public double NonConstDoubleProp { get { return double.Parse("2"); } }
            public const double ConstDoubleField = 456;
            public double NonConstDoubleField;
        }
#pragma warning restore 0649

        [TestMethod]
        public void IsConstant()
        {
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstCharProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstCharProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstCharField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstCharField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstStringProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstStringProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstStringField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstStringField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstByteProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstByteProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstByteField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstByteField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstSByteProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstSByteProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstSByteField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstSByteField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstShortProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstShortProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstShortField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstShortField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstUShortProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstUShortProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstUShortField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstUShortField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstIntProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstIntProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstIntField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstIntField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstUIntProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstUIntProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstUIntField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstUIntField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstLongProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstLongProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstLongField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstLongField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstULongProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstULongProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstULongField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstULongField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstFloatProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstFloatProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstFloatField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstFloatField").Single()));

            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstDoubleProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstDoubleProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstDoubleField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstDoubleField").Single()));
        }
#endif
    }
}
