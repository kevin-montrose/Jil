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
        class _ConstantProperties
        {
            public enum ByteEnum : byte
            {
                A = 127
            }

            public enum SByteEnum : sbyte
            {
                A = -3
            }

            public enum ShortEnum : short
            {
                A = 1891
            }

            public enum UShortEnum : ushort
            {
                A = 12381
            }

            public enum IntEnum : int
            {
                A = 1238123
            }

            public enum UIntEnum : uint
            {
                A = 9128123
            }

            public enum LongEnum : long
            {
                A = 1381261112332
            }

            public enum ULongEnum : ulong
            {
                A = 128971891
            }

            public char C1 { get { return ' '; } }
            public char C2 { get { return '"'; } }

            public string STR1 { get { return null; } }
            public string STR2 { get { return "hello world"; } }
            public string STR3 { get { return "\r\n\f"; } }

            public bool BOOL1 { get { return true; } }
            public bool BOOL2 { get { return false; } }

            public byte B1 { get { return 0; } }
            public byte B2 { get { return 127; } }
            public byte B3 { get { return 255; } }

            public sbyte SB1 { get { return -128; } }
            public sbyte SB2 { get { return 0; } }
            public sbyte SB3 { get { return 127; } }

            public short S1 { get { return short.MinValue; } }
            public short S2 { get { return 0; } }
            public short S3 { get { return short.MaxValue; } }

            public ushort US1 { get { return 0; } }
            public ushort US2 { get { return ushort.MaxValue / 2; } }
            public ushort US3 { get { return ushort.MaxValue; } }

            public int I1 { get { return int.MinValue; } }
            public int I2 { get { return 0; } }
            public int I3 { get { return int.MaxValue; } }

            public uint UI1 { get { return 0; } }
            public uint UI2 { get { return uint.MaxValue / 2; } }
            public uint UI3 { get { return uint.MaxValue; } }

            public long L1 { get { return long.MinValue; } }
            public long L2 { get { return 0; } }
            public long L3 { get { return long.MaxValue; } }

            public ulong UL1 { get { return 0; } }
            public ulong UL2 { get { return ulong.MaxValue / 2; } }
            public ulong UL3 { get { return ulong.MaxValue; } }
            public ulong UL4 { get { return ulong.MaxValue - 1; } }

            public float F1 { get { return -1234.56f; } }
            public float F2 { get { return 0; } }
            public float F3 { get { return 1234.56f; } }

            public double D1 { get { return -1234.56; } }
            public double D2 { get { return 0; } }
            public double D3 { get { return 1234.56; } }

            public ByteEnum BE { get { return ByteEnum.A; } }
            public SByteEnum SBE { get { return SByteEnum.A; } }
            public ShortEnum SE { get { return ShortEnum.A; } }
            public UShortEnum USE { get { return UShortEnum.A; } }
            public IntEnum IE { get { return IntEnum.A; } }
            public UIntEnum UIE { get { return UIntEnum.A; } }
            public LongEnum LE { get { return LongEnum.A; } }
            public ULongEnum ULE { get { return ULongEnum.A; } }
        }

        [TestMethod]
        public void ConstantProperties()
        {
            Assert.AreEqual("\" \"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("C1"), false));
            Assert.AreEqual("\"\\\"\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("C2"), false));

            Assert.AreEqual("null", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("STR1"), false));
            Assert.AreEqual("\"hello world\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("STR2"), false));
            Assert.AreEqual(@"""\r\n\f""", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("STR3"), false));

            Assert.AreEqual("true", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("BOOL1"), false));
            Assert.AreEqual("false", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("BOOL2"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("B1"), false));
            Assert.AreEqual("127", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("B2"), false));
            Assert.AreEqual("255", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("B3"), false));

            Assert.AreEqual("-128", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("SB1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("SB2"), false));
            Assert.AreEqual("127", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("SB3"), false));

            Assert.AreEqual("-32768", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("S1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("S2"), false));
            Assert.AreEqual("32767", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("S3"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("US1"), false));
            Assert.AreEqual("32767", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("US2"), false));
            Assert.AreEqual("65535", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("US3"), false));

            Assert.AreEqual("-2147483648", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("I1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("I2"), false));
            Assert.AreEqual("2147483647", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("I3"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UI1"), false));
            Assert.AreEqual("2147483647", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UI2"), false));
            Assert.AreEqual("4294967295", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UI3"), false));

            Assert.AreEqual("-9223372036854775808", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("L1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("L2"), false));
            Assert.AreEqual("9223372036854775807", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("L3"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UL1"), false));
            Assert.AreEqual("9223372036854775807", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UL2"), false));
            Assert.AreEqual("18446744073709551615", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UL3"), false));
            Assert.AreEqual("18446744073709551614", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UL4"), false));

            Assert.AreEqual("-1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("F1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("F2"), false));
            Assert.AreEqual("1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("F3"), false));

            Assert.AreEqual("-1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("D1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("D2"), false));
            Assert.AreEqual("1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("D3"), false));

            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("BE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("SBE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("SE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("USE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("IE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("UIE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("LE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantProperties).GetProperty("ULE"), false));
        }

        class _ConstantFields
        {
            public enum ByteEnum : byte
            {
                A = 127
            }

            public enum SByteEnum : sbyte
            {
                A = -3
            }

            public enum ShortEnum : short
            {
                A = 1891
            }

            public enum UShortEnum : ushort
            {
                A = 12381
            }

            public enum IntEnum : int
            {
                A = 1238123
            }

            public enum UIntEnum : uint
            {
                A = 9128123
            }

            public enum LongEnum : long
            {
                A = 1381261112332
            }

            public enum ULongEnum : ulong
            {
                A = 128971891
            }

            public const char C1 = ' ';
            public const char C2 = '"';

            public const string STR1 = null;
            public const string STR2 = "hello world";
            public const string STR3 = "\r\n\f";

            public const bool BOOL1 = true;
            public const bool BOOL2 = false;

            public const byte B1 = 0;
            public const byte B2 = 127;
            public const byte B3 = 255;

            public const sbyte SB1 = -128;
            public const sbyte SB2 = 0;
            public const sbyte SB3 = 127;

            public const short S1 = short.MinValue;
            public const short S2 = 0;
            public const short S3 = short.MaxValue;

            public const ushort US1 = 0;
            public const ushort US2 = ushort.MaxValue / 2;
            public const ushort US3 = ushort.MaxValue;

            public const int I1 = int.MinValue;
            public const int I2 = 0;
            public const int I3 = int.MaxValue;

            public const uint UI1 = 0;
            public const uint UI2 = uint.MaxValue / 2;
            public const uint UI3 = uint.MaxValue;

            public const long L1 = long.MinValue;
            public const long L2 = 0;
            public const long L3 = long.MaxValue;

            public const ulong UL1 = 0;
            public const ulong UL2 = ulong.MaxValue / 2;
            public const ulong UL3 = ulong.MaxValue;

            public const float F1 = -1234.56f;
            public const float F2 = 0;
            public const float F3 = 1234.56f;

            public const double D1 = -1234.56;
            public const double D2 = 0;
            public const double D3 = 1234.56;

            public const ByteEnum BE = ByteEnum.A;
            public const SByteEnum SBE = SByteEnum.A;
            public const ShortEnum SE = ShortEnum.A;
            public const UShortEnum USE = UShortEnum.A;
            public const IntEnum IE = IntEnum.A;
            public const UIntEnum UIE = UIntEnum.A;
            public const LongEnum LE = LongEnum.A;
            public const ULongEnum ULE = ULongEnum.A;
        }

        [TestMethod]
        public void ConstantFields()
        {
            Assert.AreEqual("\" \"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("C1"), false));
            Assert.AreEqual("\"\\\"\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("C2"), false));

            Assert.AreEqual("null", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("STR1"), false));
            Assert.AreEqual("\"hello world\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("STR2"), false));
            Assert.AreEqual(@"""\r\n\f""", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("STR3"), false));

            Assert.AreEqual("true", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("BOOL1"), false));
            Assert.AreEqual("false", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("BOOL2"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("B1"), false));
            Assert.AreEqual("127", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("B2"), false));
            Assert.AreEqual("255", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("B3"), false));

            Assert.AreEqual("-128", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("SB1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("SB2"), false));
            Assert.AreEqual("127", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("SB3"), false));

            Assert.AreEqual("-32768", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("S1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("S2"), false));
            Assert.AreEqual("32767", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("S3"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("US1"), false));
            Assert.AreEqual("32767", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("US2"), false));
            Assert.AreEqual("65535", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("US3"), false));

            Assert.AreEqual("-2147483648", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("I1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("I2"), false));
            Assert.AreEqual("2147483647", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("I3"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("UI1"), false));
            Assert.AreEqual("2147483647", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("UI2"), false));
            Assert.AreEqual("4294967295", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("UI3"), false));

            Assert.AreEqual("-9223372036854775808", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("L1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("L2"), false));
            Assert.AreEqual("9223372036854775807", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("L3"), false));

            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("UL1"), false));
            Assert.AreEqual("9223372036854775807", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("UL2"), false));
            Assert.AreEqual("18446744073709551615", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("UL3"), false));

            Assert.AreEqual("-1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("F1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("F2"), false));
            Assert.AreEqual("1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("F3"), false));

            Assert.AreEqual("-1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("D1"), false));
            Assert.AreEqual("0", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("D2"), false));
            Assert.AreEqual("1234.56", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("D3"), false));

            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("BE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("SBE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("SE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("USE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("IE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("UIE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("LE"), false));
            Assert.AreEqual("\"A\"", Jil.Common.ExtensionMethods.GetConstantJSONStringEquivalent(typeof(_ConstantFields).GetField("ULE"), false));
        }

#pragma warning disable 0649
        class _IsConstant
        {
            public bool ConstBoolProp { get { return true; } }
            public bool NonConstBoolProp { get { return bool.Parse("False"); } }
            public const bool ConstBoolField = true;
            public bool NonConstBoolField;

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
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstBoolProp").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstBoolProp").Single()));
            Assert.IsTrue(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("ConstBoolField").Single()));
            Assert.IsFalse(Jil.Common.ExtensionMethods.IsConstant(typeof(_IsConstant).GetMember("NonConstBoolField").Single()));

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

        class _MemberMatcher
        {
            public string Hello { get; set; }
            public string World { get; set; }
            public string Fizz { get; set; }
            public string Buzz { get; set; }
            public string Foo { get; set; }
            public string Bar { get; set; }
        }

        [TestMethod]
        public void MemberMatcher()
        {
            Assert.IsTrue(Jil.Deserialize.MemberMatcher<_MemberMatcher>.IsAvailable);
            Assert.AreEqual(6, Jil.Deserialize.MemberMatcher<_MemberMatcher>.HashLookup.Count);

            var allTestTypes = Assembly.GetAssembly(this.GetType()).GetTypes().Where(t => t.Name.StartsWith("_")).ToList();

            var fails = 0;
            var success = 0;

            foreach (var type in allTestTypes)
            {
                var matcher = typeof(Jil.Deserialize.MemberMatcher<>).MakeGenericType(type);
                if (matcher.ContainsGenericParameters) continue;

                var isAvailable = (bool)matcher.GetField("IsAvailable").GetValue(null);
                if (!isAvailable) fails++;
                if (isAvailable) success++;
            }

            Assert.IsTrue(success > fails);
        }
    }
}
