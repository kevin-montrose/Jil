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
    public class DeserializeTests
    {
        [TestMethod]
        public void Overflow()
        {
            // byte
            {
                try
                {
                    using (var str = new StringReader("1234"))
                    {
                        JSON.Deserialize<byte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("257"))
                    {
                        JSON.Deserialize<byte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // sbyte
            {
                try
                {
                    using (var str = new StringReader("1234"))
                    {
                        JSON.Deserialize<sbyte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("128"))
                    {
                        JSON.Deserialize<sbyte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-129"))
                    {
                        JSON.Deserialize<sbyte>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // short
            {
                try
                {
                    using (var str = new StringReader("320000"))
                    {
                        JSON.Deserialize<short>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("32768"))
                    {
                        JSON.Deserialize<short>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-32769"))
                    {
                        JSON.Deserialize<short>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // ushort
            {
                try
                {
                    using (var str = new StringReader("320000"))
                    {
                        JSON.Deserialize<ushort>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("65536"))
                    {
                        JSON.Deserialize<ushort>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // int
            {
                try
                {
                    using (var str = new StringReader("21474830000"))
                    {
                        JSON.Deserialize<int>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("2147483648"))
                    {
                        JSON.Deserialize<int>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-2147483649"))
                    {
                        JSON.Deserialize<int>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // uint
            {
                try
                {
                    using (var str = new StringReader("42949670000"))
                    {
                        JSON.Deserialize<uint>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("4294967296"))
                    {
                        JSON.Deserialize<uint>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // long
            {
                try
                {
                    using (var str = new StringReader("92233720368547750000"))
                    {
                        JSON.Deserialize<long>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("9223372036854775808"))
                    {
                        JSON.Deserialize<long>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }

                try
                {
                    using (var str = new StringReader("-9223372036854775809"))
                    {
                        JSON.Deserialize<long>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }

            // ulong
            {
                try
                {
                    using (var str = new StringReader("184467440737095510000"))
                    {
                        JSON.Deserialize<ulong>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Number did not end when expected, may overflow", e.Message);
                }

                try
                {
                    using (var str = new StringReader("18446744073709551616"))
                    {
                        JSON.Deserialize<ulong>(str);
                        Assert.Fail("Shouldn't be possible");
                    }
                }
                catch (OverflowException e)
                {
                    Assert.AreEqual("Arithmetic operation resulted in an overflow.", e.Message);
                }
            }
        }

        [TestMethod]
        public void Decimals()
        {
            for (var i = -11.1m; i <= 22.2m; i += 0.03m)
            {
                var asStr = i.ToString();
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<decimal>(str);

                    Assert.AreEqual(asStr, res.ToString());
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<decimal>(str);

                Assert.AreEqual(0m, res);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<decimal>(str);

                Assert.AreEqual(0m, res);
                Assert.AreEqual(-1, str.Peek());
            }

            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                decimal d = rand.Next() * (rand.Next() % 2 == 0 ? -1 : 1);
                d *= (decimal)rand.NextDouble();

                var asStr = d.ToString();
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<decimal>(str);

                    Assert.AreEqual(asStr, res.ToString());
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        [TestMethod]
        public void Doubles()
        {
            for (var i = -11.1; i <= 22.2; i += 0.03)
            {
                var asStr = i.ToString();
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<double>(str);

                    Assert.AreEqual(asStr, res.ToString());
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<double>(str);

                Assert.AreEqual(0.0, res);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<double>(str);

                Assert.AreEqual(0.0, res);
                Assert.AreEqual(-1, str.Peek());
            }

            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                double d = rand.Next() * (rand.Next() % 2 == 0 ? -1 : 1);
                d *= rand.NextDouble();

                var asStr = d.ToString();
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<double>(str);

                    Assert.AreEqual(asStr, res.ToString());
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        [TestMethod]
        public void Floats()
        {
            for (var i = -11.1f; i <= 22.2f; i += 0.03f)
            {
                var asStr = i.ToString();
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<float>(str);

                    Assert.AreEqual(asStr, res.ToString());
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("0"))
            {
                var res = JSON.Deserialize<float>(str);

                Assert.AreEqual(0f, res);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("0.0"))
            {
                var res = JSON.Deserialize<float>(str);

                Assert.AreEqual(0f, res);
                Assert.AreEqual(-1, str.Peek());
            }

            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                float f = rand.Next() * (rand.Next() % 2 == 0 ? -1 : 1);
                f *= (float)rand.NextDouble();

                var asStr = f.ToString();
                using (var str = new StringReader(asStr))
                {
                    var res = JSON.Deserialize<float>(str);

                    Assert.AreEqual(asStr, res.ToString());
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        [TestMethod]
        public void Longs()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.AreEqual(0L, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(long.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.AreEqual(long.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(long.MinValue.ToString()))
            {
                var i = JSON.Deserialize<long>(str);

                Assert.AreEqual(long.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void ULongs()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.AreEqual((ulong)0, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(ulong.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.AreEqual(ulong.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(ulong.MinValue.ToString()))
            {
                var i = JSON.Deserialize<ulong>(str);

                Assert.AreEqual(ulong.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Ints()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.AreEqual(0, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(int.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.AreEqual(int.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(int.MinValue.ToString()))
            {
                var i = JSON.Deserialize<int>(str);

                Assert.AreEqual(int.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void UInts()
        {
            using (var str = new StringReader("0"))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.AreEqual((uint)0, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(uint.MaxValue.ToString()))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.AreEqual(uint.MaxValue, i);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(uint.MinValue.ToString()))
            {
                var i = JSON.Deserialize<uint>(str);

                Assert.AreEqual(uint.MinValue, i);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Shorts()
        {
            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<short>(str);

                    Assert.AreEqual((short)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<short>(str);

                        Assert.AreEqual((short)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<short>(str);

                        Assert.AreEqual((short)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<short>(str);

                        Assert.AreEqual((short)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void UShort()
        {
            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<ushort>(str);

                    Assert.AreEqual((ushort)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<ushort>(str);

                        Assert.AreEqual((ushort)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<ushort>(str);

                        Assert.AreEqual((ushort)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<ushort>(str);

                        Assert.AreEqual((ushort)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void Bytes()
        {
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<byte>(str);

                    Assert.AreEqual((byte)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<byte>(str);

                        Assert.AreEqual((byte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<byte>(str);

                        Assert.AreEqual((byte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<byte>(str);

                        Assert.AreEqual((byte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void SBytes()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                using (var str = new StringReader(i.ToString()))
                {
                    var b = JSON.Deserialize<sbyte>(str);

                    Assert.AreEqual((sbyte)i, b);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString()))
                    {
                        var b = JSON.Deserialize<sbyte>(str);

                        Assert.AreEqual((sbyte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<sbyte>(str);

                        Assert.AreEqual((sbyte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    using (var str = new StringReader(new string(' ', j) + i.ToString() + new string(' ', j)))
                    {
                        var b = JSON.Deserialize<sbyte>(str);

                        Assert.AreEqual((sbyte)i, b);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void Strings()
        {
            using (var str = new StringReader("null"))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.IsNull(c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"a\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("a", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\\\\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\\", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\/\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("/", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\b", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\f\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\f", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\r\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\r", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\n\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\n", c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\t\""))
            {
                var c = JSON.Deserialize<string>(str);

                Assert.AreEqual("\t", c);
                Assert.AreEqual(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asStr = "\"\\u" + i.ToString("X4") + "\"";

                using (var str = new StringReader(asStr))
                {
                    var c = JSON.Deserialize<string>(str);

                    var shouldBe = "" + (char)i;

                    Assert.AreEqual(shouldBe, c);
                    Assert.AreEqual(-1, str.Peek());
                }
            }

            using (var str = new StringReader("\"abc\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("abc", s);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"abcd\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("abcd", s);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\\f\\t\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("\b\f\t", s);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\\f\\t\\r\""))
            {
                var s = JSON.Deserialize<string>(str);

                Assert.AreEqual("\b\f\t\r", s);
                Assert.AreEqual(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asEncodedChar = "\\u" + i.ToString("X4");

                for (var j = 0; j < 10; j++)
                {
                    var asStr = "\"" + string.Join("", Enumerable.Range(0, j).Select(_ => asEncodedChar)) + "\"";

                    using (var str = new StringReader(asStr))
                    {
                        var c = JSON.Deserialize<string>(str);

                        var shouldBe = new string((char)i, j);

                        Assert.AreEqual(shouldBe, c);
                        Assert.AreEqual(-1, str.Peek());
                    }
                }
            }
        }

        [TestMethod]
        public void Chars()
        {
            using (var str = new StringReader("\"a\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('a', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\\\\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\\', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\/\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('/', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\b\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\b', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\f\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\f', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\r\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\r', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\n\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\n', c);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("\"\\t\""))
            {
                var c = JSON.Deserialize<char>(str);

                Assert.AreEqual('\t', c);
                Assert.AreEqual(-1, str.Peek());
            }

            for (var i = 0; i <= 2048; i++)
            {
                var asStr = "\"\\u" + i.ToString("X4") + "\"";

                using(var str = new StringReader(asStr))
                {
                    var c = JSON.Deserialize<char>(str);

                    Assert.AreEqual(i, (int)c);
                    Assert.AreEqual(-1, str.Peek());
                }
            }
        }

        enum _Enums : int
        {
            Hello,
            World,
            Foo
        }

        [TestMethod]
        public void Enums()
        {
            using (var str = new StringReader("\"Hello\""))
            {
                var val = JSON.Deserialize<_Enums>(str);

                Assert.AreEqual(_Enums.Hello, val);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Nullables()
        {
            using (var str = new StringReader("1"))
            {
                var val = JSON.Deserialize<int?>(str);

                Assert.AreEqual((int?)1, val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<int?>(str);

                Assert.AreEqual((int?)null, val);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Bools()
        {
            using (var str = new StringReader("true"))
            {
                var val = JSON.Deserialize<bool>(str);

                Assert.IsTrue(val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("false"))
            {
                var val = JSON.Deserialize<bool>(str);

                Assert.IsFalse(val);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Lists()
        {
            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.IsNull(val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(0, val.Count);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(" [     ] "))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(0, val.Count);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[1]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(1, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[1,2]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(2, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(2, val[1]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("[1,2,3]"))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(2, val[1]);
                Assert.AreEqual(3, val[2]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader(" [ 1,2 ,3   ]    "))
            {
                var val = JSON.Deserialize<List<int>>(str);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(1, val[0]);
                Assert.AreEqual(2, val[1]);
                Assert.AreEqual(3, val[2]);
                Assert.AreEqual(-1, str.Peek());
            }
        }

#pragma warning disable 0649
        class _Objects
        {
            public int A;
            public string B { get; set; }
        }
#pragma warning restore 0649

        [TestMethod]
        public void Objects()
        {
            using (var str = new StringReader("null"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNull(val);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(123, val.A);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\": \"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 456, \"B\": \"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\": \"hello\", \"A\": 456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"B\":\"hello\",\"A\":456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("   {  \"B\"    :   \"hello\"    ,    \"A\"   :   456   }  "))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(456, val.A);
                Assert.AreEqual("hello", val.B);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void ObjectsSkipMembers()
        {
            using (var str = new StringReader("{\"C\":123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-123}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":123.456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }



            using (var str = new StringReader("{\"C\":-123.456}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1E12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1E12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1E-12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1E-12}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1.1E2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1.1E2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":1.1E+2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":-1.1E+2}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\":\"hello\"}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": []}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1,2]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [1,2,3]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123, \"B\": 456}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\": 123, \"B\": 456, \"C\": \"hello world\"}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": null, \"CC\": null}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": [null], \"CC\": [null]}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"C\": {\"A\":null}, \"CC\": {\"B\":null}}"))
            {
                var val = JSON.Deserialize<_Objects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(default(int), val.A);
                Assert.IsNull(val.B);
                Assert.AreEqual(-1, str.Peek());
            }
        }

#pragma warning disable 0649
        class _RecursiveObjects
        {
            public string A;
            public _RecursiveObjects B;
        }
#pragma warning restore 0649

        [TestMethod]
        public void RecursiveObjects()
        {
            using (var str = new StringReader("{\"A\": \"hello world\", \"B\": { \"A\": \"foo bar\", \"B\": {\"A\": \"fizz buzz\"}}}"))
            {
                var val = JSON.Deserialize<_RecursiveObjects>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual("hello world", val.A);
                Assert.AreEqual("foo bar", val.B.A);
                Assert.AreEqual("fizz buzz", val.B.B.A);
                Assert.IsNull(val.B.B.B);
                Assert.AreEqual(-1, str.Peek());
            }
        }

        [TestMethod]
        public void Dictionaries()
        {
            using (var str = new StringReader("{\"A\": 123}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(1, val.Count);
                Assert.AreEqual(123, val["A"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123, \"B\": 456}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(2, val.Count);
                Assert.AreEqual(123, val["A"]);
                Assert.AreEqual(456, val["B"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": 123, \"B\": 456, \"C\": 789}"))
            {
                var val = JSON.Deserialize<Dictionary<string, int>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(123, val["A"]);
                Assert.AreEqual(456, val["B"]);
                Assert.AreEqual(789, val["C"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": null}"))
            {
                var val = JSON.Deserialize<Dictionary<string, string>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(1, val.Count);
                Assert.IsNull(val["A"]);
                Assert.AreEqual(-1, str.Peek());
            }

            using (var str = new StringReader("{\"A\": \"hello\", \"B\": \"world\"}"))
            {
                var val = JSON.Deserialize<Dictionary<string, string>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(2, val.Count);
                Assert.AreEqual("hello", val["A"]);
                Assert.AreEqual("world", val["B"]);
                Assert.AreEqual(-1, str.Peek());
            }

            /*using (var str = new StringReader("{\"A\": {\"A\":123}, \"B\": {\"B\": \"abc\"}, \"C\": {\"A\":456, \"B\":\"fizz\"} }"))
            {
                var val = JSON.Deserialize<Dictionary<string, _Objects>>(str);
                Assert.IsNotNull(val);
                Assert.AreEqual(3, val.Count);
                Assert.AreEqual(123, val["A"].A);
                Assert.AreEqual("abc", val["B"].B);
                Assert.AreEqual(456, val["C"].A);
                Assert.AreEqual("fizz", val["C"].B);
                Assert.AreEqual(-1, str.Peek());
            }*/
        }
    }
}
