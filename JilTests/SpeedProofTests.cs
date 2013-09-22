using Jil.Serialize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sigil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JilTests
{
    // These tests make *no sense* in debug
#if !DEBUG

    [TestClass]
    public class SpeedProofTests
    {
        private static char _RandChar(Random rand)
        {
            var lower = rand.Next(2) == 0;

            var ret = (char)('A' + rand.Next('Z' - 'A'));

            if (lower) ret = char.ToLower(ret);

            return ret;
        }

        private static string _RandString(Random rand)
        {
            var len = rand.Next(20);
            var ret = new char[len];

            for (var i = 0; i < len; i++)
            {
                ret[i] = _RandChar(rand);
            }

            return new string(ret);
        }

        private static void CompareTimes<T>(List<T> toSerialize, Action<TextWriter, T> a, Action<TextWriter, T> b, out double aTimeMS, out double bTimeMS)
        {
            var aTimer = new Stopwatch();
            var bTimer = new Stopwatch();

            Action timeA =
                () =>
                {
                    aTimer.Start();
                    for (var i = 0; i < toSerialize.Count; i++)
                    {
                        using (var str = new StringWriter())
                        {
                            a(str, toSerialize[i]);
                        }
                    }
                    aTimer.Stop();
                };

            Action timeB =
                () =>
                {
                    bTimer.Start();
                    for (var i = 0; i < toSerialize.Count; i++)
                    {
                        using (var str = new StringWriter())
                        {
                            b(str, toSerialize[i]);
                        }
                    }
                    bTimer.Stop();
                };

            for (var i = 0; i < 5; i++)
            {
                timeA();
                timeB();
            }

            bTimer.Reset();
            aTimer.Reset();

            for (var i = 0; i < 100; i++)
            {
                var order = (i % 2) == 0;

                if (order)
                {
                    timeA();
                    timeB();
                }
                else
                {
                    timeB();
                    timeA();
                }
            }

            aTimeMS = aTimer.ElapsedMilliseconds;
            bTimeMS = bTimer.ElapsedMilliseconds;
        }

        public class _ReorderMembers
        {
            public int Foo;
            public string Bar;
            public double Fizz;
            public decimal Buzz;
            public char Hello;
            public string[] World;
        }

        [TestMethod]
        public void ReorderMembers()
        {
            Action<TextWriter, _ReorderMembers> memoryOrder;
            Action<TextWriter, _ReorderMembers> normalOrder;

            try
            {
                {
                    InlineSerializer.ReorderMembers = true;

                    // Build the *actual* serializer method
                    memoryOrder = InlineSerializer.Build<_ReorderMembers>();
                }

                {
                    InlineSerializer.ReorderMembers = false;

                    // Build the *actual* serializer method
                    normalOrder = InlineSerializer.Build<_ReorderMembers>();
                }
            }
            finally
            {
                InlineSerializer.ReorderMembers = true;
            }

            var rand = new Random(1160428);

            var toSerialize = new List<_ReorderMembers>();
            for (var i = 0; i < 10000; i++)
            {
                toSerialize.Add(
                    new _ReorderMembers
                    {
                        Bar = _RandString(rand),
                        Buzz = ((decimal)rand.NextDouble()) * decimal.MaxValue,
                        Fizz = rand.NextDouble() * double.MaxValue,
                        Foo = rand.Next(int.MaxValue),
                        Hello = _RandChar(rand),
                        World = Enumerable.Range(0, rand.Next(100)).Select(s => _RandString(rand)).ToArray()
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double reorderedTime, normalOrderTime;
            CompareTimes(toSerialize, memoryOrder, normalOrder, out reorderedTime, out normalOrderTime);

            Assert.IsTrue(reorderedTime < normalOrderTime, "reorderedTime = " + reorderedTime + ", normalOrderTime = " + normalOrderTime);
        }

        public class _SkipNumberFormatting
        {
            public byte A;
            public sbyte B;
            public short C;
            public ushort D;
            public int E;
        }

        [TestMethod]
        public void SkipNumberFormatting()
        {
            Action<TextWriter, _SkipNumberFormatting> skipping;
            Action<TextWriter, _SkipNumberFormatting> normal;

            try
            {
                {
                    InlineSerializer.SkipNumberFormatting = true;

                    // Build the *actual* serializer method
                    skipping = InlineSerializer.Build<_SkipNumberFormatting>();
                }

                {
                    InlineSerializer.SkipNumberFormatting = false;

                    // Build the *actual* serializer method
                    normal = InlineSerializer.Build<_SkipNumberFormatting>();
                }
            }
            finally
            {
                InlineSerializer.SkipNumberFormatting = true;
            }

            var rand = new Random(141090045);

            var toSerialize = new List<_SkipNumberFormatting>();
            for (var i = 0; i < 10000; i++)
            {
                toSerialize.Add(
                    new _SkipNumberFormatting
                    {
                        A = (byte)rand.Next(101),
                        B = (sbyte)rand.Next(101),
                        C = (short)rand.Next(101),
                        D = (ushort)rand.Next(101),
                        E = rand.Next(101),
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double skippingTime, normalTime;
            CompareTimes(toSerialize, skipping, normal, out skippingTime, out normalTime);

            Assert.IsTrue(skippingTime < normalTime, "skippingTime = " + skippingTime + ", normalTime = " + normalTime);
        }

        public class _CustomNumberToString
        {
            public byte A;
            public sbyte B;
            public short C;
            public ushort D;
            public int E;
            public uint F;
            public long G;
            public ulong H;
        }

        [TestMethod]
        public void CustomNumberToString()
        {
            Action<TextWriter, _CustomNumberToString> custom;
            Action<TextWriter, _CustomNumberToString> normal;

            try
            {
                {
                    InlineSerializer.UseCustomNumberToString = true;

                    // Build the *actual* serializer method
                    custom = InlineSerializer.Build<_CustomNumberToString>();
                }

                {
                    InlineSerializer.UseCustomNumberToString = false;

                    // Build the *actual* serializer method
                    normal = InlineSerializer.Build<_CustomNumberToString>();
                }
            }
            finally
            {
                InlineSerializer.UseCustomNumberToString = true;
            }

            var rand = new Random(139426720);

            var toSerialize = new List<_CustomNumberToString>();
            for (var i = 0; i < 10000; i++)
            {
                toSerialize.Add(
                    new _CustomNumberToString
                    {
                        A = (byte)(101 + rand.Next(155)),
                        B = (sbyte)(101 + rand.Next(27)),
                        C = (short)(101 + rand.Next(1000)),
                        D = (ushort)(101 + rand.Next(1000)),
                        E = 101 + rand.Next(int.MaxValue - 101),
                        F = (uint)(101 + rand.Next(int.MaxValue - 101)),
                        G = (long)(101 + rand.Next(int.MaxValue)),
                        H = (ulong)(101 + rand.Next(int.MaxValue))
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double customTime, normalTime;
            CompareTimes(toSerialize, custom, normal, out customTime, out normalTime);

            Assert.IsTrue(customTime < normalTime, "customTime = " + customTime + ", normalTime = " + normalTime);
        }
    }
#endif
}
