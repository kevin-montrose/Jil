using Jil;
using Jil.Deserialize;
using Jil.Serialize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JilTests
{
    [TestClass]
    public class SpeedProofTests
    {
        private static long _RandLong(Random rand)
        {
            var bytes = new byte[8];
            rand.NextBytes(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }

        private static Guid _RandGuid(Random rand)
        {
            var bytes = new byte[16];
            rand.NextBytes(bytes);

            return new Guid(bytes);
        }

        private static char _RandChar(Random rand)
        {
            var lower = rand.Next(2) == 0;

            var ret = (char)('A' + rand.Next('Z' - 'A'));

            if (lower) ret = char.ToLower(ret);

            return ret;
        }

        public static T _RandEnum<T>(Random rand)
            where T : struct
        {
            var names = Enum.GetNames(typeof(T));

            var ix = rand.Next(names.Length);

            var retName = names[ix];

            return (T)Enum.Parse(typeof(T), retName);
        }

        public static T _RandFlagEnum<T>(Random rand)
            where T : struct
        {
            var ret = default(T);

            var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();

            var take = rand.Next(values.Length) + 1;

            for (var i = 0; i < take; i++)
            {
                var val = values[rand.Next(values.Length)];

                ret = (T)Enum.ToObject(typeof(T), Convert.ToUInt64(ret) | Convert.ToUInt64(val));
            }

            return ret;
        }

        public static string _RandString(Random rand, int? maxLength = null)
        {
            var len = 1 + rand.Next(maxLength ?? 20);

            var ret = new char[len];

            for (var i = 0; i < len; i++)
            {
                ret[i] = _RandChar(rand);
            }

            return new string(ret);
        }

        private static DateTime _RandDateTime(Random rand)
        {
            var year = 1 + rand.Next(3000);
            var month = 1 + rand.Next(12);
            var day = 1 + rand.Next(28);
            var hour = rand.Next(24);
            var minute = rand.Next(60);
            var second = rand.Next(60);

            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }

        private static void CompareTimes<T>(List<T> toSerialize, Jil.Options opts, Func<TextReader, T> a, Func<TextReader, T> b, out double aTimeMS, out double bTimeMS)
        {
            var asStrings = toSerialize.Select(o => Jil.JSON.Serialize(o, opts)).ToList();

            var aTimer = new Stopwatch();
            var bTimer = new Stopwatch();

            Action timeA =
                () =>
                {
                    aTimer.Start();
                    for (var i = 0; i < asStrings.Count; i++)
                    {
                        using (var str = new StringReader(asStrings[i]))
                        {
                            a(str);
                        }
                    }
                    aTimer.Stop();
                };

            Action timeB =
                () =>
                {
                    bTimer.Start();
                    for (var i = 0; i < asStrings.Count; i++)
                    {
                        using (var str = new StringReader(asStrings[i]))
                        {
                            b(str);
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

        private static void CompareTimes<T>(List<T> toSerialize, Action<TextWriter, T, int> a, Action<TextWriter, T, int> b, out double aTimeMS, out double bTimeMS, bool checkCorrectness = true)
        {
            var aTimer = new Stopwatch();
            var bTimer = new Stopwatch();

            // Some of our optimizations change the produced string, so we can conditionally suppress this
            if (checkCorrectness)
            {
                foreach (var item in toSerialize)
                {
                    using (var aStr = new StringWriter())
                    using (var bStr = new StringWriter())
                    {
                        a(aStr, item, 0);
                        b(bStr, item, 0);

                        Assert.AreEqual(aStr.ToString(), bStr.ToString());
                    }
                }
            }

            Action timeA =
                () =>
                {
                    aTimer.Start();
                    for (var i = 0; i < toSerialize.Count; i++)
                    {
                        using (var str = new StringWriter())
                        {
                            a(str, toSerialize[i], 0);
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
                            b(str, toSerialize[i], 0);
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

        // These tests make *no sense* in debug
#if !DEBUG

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
            Action<TextWriter, _ReorderMembers, int> memoryOrder;
            Action<TextWriter, _ReorderMembers, int> normalOrder;

            try
            {
                {
                    InlineSerializer<_ReorderMembers>.ReorderMembers = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    memoryOrder = InlineSerializerHelper.Build<_ReorderMembers>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_ReorderMembers>.ReorderMembers = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normalOrder = InlineSerializerHelper.Build<_ReorderMembers>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_ReorderMembers>.ReorderMembers = true;
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
            CompareTimes(toSerialize, memoryOrder, normalOrder, out reorderedTime, out normalOrderTime, checkCorrectness: false);

            var msg = "reorderedTime = " + reorderedTime + ", normalOrderTime = " + normalOrderTime;

            Assert.IsTrue(reorderedTime < normalOrderTime, msg);
            Console.WriteLine(msg);
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
            Action<TextWriter, _SkipNumberFormatting, int> skipping;
            Action<TextWriter, _SkipNumberFormatting, int> normal;

            try
            {
                {
                    InlineSerializer<_SkipNumberFormatting>.SkipNumberFormatting = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    skipping = InlineSerializerHelper.Build<_SkipNumberFormatting>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_SkipNumberFormatting>.SkipNumberFormatting = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_SkipNumberFormatting>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_SkipNumberFormatting>.SkipNumberFormatting = true;
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

        public class _UseCustomIntegerToString
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
        public void UseCustomIntegerToString()
        {
            Action<TextWriter, _UseCustomIntegerToString, int> custom;
            Action<TextWriter, _UseCustomIntegerToString, int> normal;

            try
            {
                {
                    InlineSerializer<_UseCustomIntegerToString>.UseCustomIntegerToString = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    custom = InlineSerializerHelper.Build<_UseCustomIntegerToString>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseCustomIntegerToString>.UseCustomIntegerToString = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseCustomIntegerToString>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseCustomIntegerToString>.UseCustomIntegerToString = true;
            }

            var rand = new Random(139426720);

            var toSerialize = new List<_UseCustomIntegerToString>();
            for (var i = 0; i < 10000; i++)
            {
                toSerialize.Add(
                    new _UseCustomIntegerToString
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

        public class _SkipDateTimeMathMethods
        {
            public DateTime[] Dates;
        }

        [TestMethod]
        public void SkipDateTimeMathMethods()
        {
            Action<TextWriter, _SkipDateTimeMathMethods, int> skipped;
            Action<TextWriter, _SkipDateTimeMathMethods, int> normal;

            try
            {
                {
                    InlineSerializer<_SkipDateTimeMathMethods>.SkipDateTimeMathMethods = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    skipped = InlineSerializerHelper.Build<_SkipDateTimeMathMethods>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_SkipDateTimeMathMethods>.SkipDateTimeMathMethods = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_SkipDateTimeMathMethods>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_SkipDateTimeMathMethods>.SkipDateTimeMathMethods = true;
            }

            var rand = new Random(66262484);

            var toSerialize = new List<_SkipDateTimeMathMethods>();
            for (var i = 0; i < 1000; i++)
            {
                var numDates = new DateTime[5 + rand.Next(10)];

                for (var j = 0; j < numDates.Length; j++)
                {
                    numDates[j] = _RandDateTime(rand);
                }

                toSerialize.Add(
                    new _SkipDateTimeMathMethods
                    {
                        Dates = numDates
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double skippedTime, normalTime;
            CompareTimes(toSerialize, skipped, normal, out skippedTime, out normalTime);

            Assert.IsTrue(skippedTime < normalTime, "skippedTime = " + skippedTime + ", normalTime = " + normalTime);
        }

        public class _UseCustomISODateFormatting
        {
            public List<DateTime> Dates;
        }

        [TestMethod]
        public void UseCustomISODateFormatting()
        {
            Action<TextWriter, _UseCustomISODateFormatting, int> skipped;
            Action<TextWriter, _UseCustomISODateFormatting, int> normal;

            try
            {
                {
                    InlineSerializer<_UseCustomISODateFormatting>.UseCustomISODateFormatting = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    skipped = InlineSerializerHelper.Build<_UseCustomISODateFormatting>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.ISO8601, pretty: false, excludeNulls: false, jsonp: false, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseCustomISODateFormatting>.UseCustomISODateFormatting = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseCustomISODateFormatting>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.ISO8601, pretty: false, excludeNulls: false, jsonp: false, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseCustomISODateFormatting>.UseCustomISODateFormatting = true;
            }

            var rand = new Random(39432715);

            var toSerialize = new List<_UseCustomISODateFormatting>();
            for (var i = 0; i < 1000; i++)
            {
                var numDates = new DateTime[5 + rand.Next(10)];

                for (var j = 0; j < numDates.Length; j++)
                {
                    numDates[j] = _RandDateTime(rand);
                }

                toSerialize.Add(
                    new _UseCustomISODateFormatting
                    {
                        Dates = numDates.ToList()
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double skippedTime, normalTime;
            CompareTimes(toSerialize, skipped, normal, out skippedTime, out normalTime);

            Assert.IsTrue(skippedTime < normalTime, "skippedTime = " + skippedTime + ", normalTime = " + normalTime);
        }

        class _UseFastLists
        {
            public List<int> A { get; set; }
            public int[] B { get; set; }
            public IList<string> C { get; set; }
        }

        [TestMethod]
        public void UseFastLists()
        {
            Action<TextWriter, _UseFastLists, int> fast;
            Action<TextWriter, _UseFastLists, int> normal;

            try
            {
                {
                    InlineSerializer<_UseFastLists>.UseFastLists = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    fast = InlineSerializerHelper.Build<_UseFastLists>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseFastLists>.UseFastLists = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseFastLists>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseFastLists>.UseFastLists = true;
            }

            var rand = new Random(2323284);

            var toSerialize = new List<_UseFastLists>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(
                    new _UseFastLists
                    {
                        A = Enumerable.Range(0, 5 + rand.Next(10)).Select(_ => rand.Next()).ToList(),
                        B = Enumerable.Range(0, 10 + rand.Next(5)).Select(_ => rand.Next()).ToArray(),
                        C = Enumerable.Range(0, 7 + rand.Next(8)).Select(_ => _RandString(rand)).ToList().AsReadOnly()
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double fastTime, normalTime;
            CompareTimes(toSerialize, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        class _UseFastArrays
        {
            public int[] A { get; set; }
            public double[] B { get; set; }
            public string[] C { get; set; }
        }

        [TestMethod]
        public void UseFastArrays()
        {
            Action<TextWriter, _UseFastArrays, int> fast;
            Action<TextWriter, _UseFastArrays, int> normal;

            try
            {
                {
                    InlineSerializer<_UseFastArrays>.UseFastArrays = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    fast = InlineSerializerHelper.Build<_UseFastArrays>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseFastArrays>.UseFastArrays = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseFastArrays>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseFastArrays>.UseFastArrays = true;
            }

            var rand = new Random(2323284);

            var toSerialize = new List<_UseFastArrays>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(
                    new _UseFastArrays
                    {
                        A = Enumerable.Range(0, 5 + rand.Next(10)).Select(_ => rand.Next()).ToArray(),
                        B = Enumerable.Range(0, 10 + rand.Next(5)).Select(_ => rand.NextDouble()).ToArray(),
                        C = Enumerable.Range(0, 7 + rand.Next(8)).Select(_ => _RandString(rand)).ToArray()
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double fastTime, normalTime;
            CompareTimes(toSerialize, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        class _UseFastGuids
        {
            public Guid A;
            public Guid? B;
            public List<Guid> C;
            public Dictionary<string, Guid> D;
        }

        [TestMethod]
        public void UseFastGuids()
        {
            Action<TextWriter, _UseFastGuids, int> fast;
            Action<TextWriter, _UseFastGuids, int> normal;

            try
            {
                {
                    InlineSerializer<_UseFastGuids>.UseFastGuids = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    fast = InlineSerializerHelper.Build<_UseFastGuids>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseFastGuids>.UseFastGuids = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseFastGuids>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseFastGuids>.UseFastGuids = true;
            }

            var rand = new Random(70490340);

            var toSerialize = new List<_UseFastGuids>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(
                    new _UseFastGuids
                    {
                        A = _RandGuid(rand),
                        B = rand.Next(2) == 0 ? null : (Guid?)_RandGuid(rand),
                        C = Enumerable.Range(0, 7 + rand.Next(8)).Select(_ => _RandGuid(rand)).ToList(),
                        D = Enumerable.Range(0, 5 + rand.Next(5)).ToDictionary(d => _RandString(rand) + _RandString(rand), d => _RandGuid(rand))
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double fastTime, normalTime;
            CompareTimes(toSerialize, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        class _AllocationlessDictionaries
        {
            public Dictionary<string, int> A;
        }

        [TestMethod]
        public void AllocationlessDictionaries()
        {
            Action<TextWriter, _AllocationlessDictionaries, int> allocationless;
            Action<TextWriter, _AllocationlessDictionaries, int> normal;

            try
            {
                {
                    InlineSerializer<_AllocationlessDictionaries>.AllocationlessDictionaries = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    allocationless = InlineSerializerHelper.Build<_AllocationlessDictionaries>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_AllocationlessDictionaries>.AllocationlessDictionaries = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_AllocationlessDictionaries>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_AllocationlessDictionaries>.AllocationlessDictionaries = true;
            }

            var rand = new Random(202457890);

            var toSerialize = new List<_AllocationlessDictionaries>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(
                    new _AllocationlessDictionaries
                    {
                        A = Enumerable.Range(0, 5 + rand.Next(5)).ToDictionary(_ => new string(("" + _)[0], 5 + rand.Next(10)), _ => rand.Next())
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double allocationlessTime, normalTime;
            CompareTimes(toSerialize, allocationless, normal, out allocationlessTime, out normalTime);

            Assert.IsTrue(allocationlessTime < normalTime, "allocationlessTime = " + allocationlessTime + ", normalTime = " + normalTime);
        }

        class _PropagateConstants
        {
            public const bool F1 = true;
            public bool P1 { get { return false; } }

            public const byte F2 = 100;
            public byte P2 { get { return 200; } }

            public const sbyte F3 = -1;
            public sbyte P3 { get { return -100; } }

            public const short F4 = -1000;
            public short P4 { get { return 1000; } }

            public const ushort F5 = 6000;
            public ushort P5 { get { return 12000; } }

            public const int F6 = -1000000;
            public int P6 { get { return 2000000; } }

            public const uint F7 = 4000000000;
            public uint P7 { get { return 4000000001; } }

            public const long F8 = long.MaxValue;
            public long P8 { get { return long.MinValue; } }

            public const ulong F9 = ulong.MaxValue;
            public ulong P9 { get { return 18446744073709551614UL; } }

            public const string F10 = "hello world";
            public string P10 { get { return null; } }

            public const char F11 = '\u1234';
            public char P11 { get { return '\u5678'; } }
        }

        [TestMethod]
        public void PropagateConstants()
        {
            Action<TextWriter, _PropagateConstants, int> propagated;
            Action<TextWriter, _PropagateConstants, int> normal;

            try
            {
                {
                    InlineSerializer<_PropagateConstants>.PropagateConstants = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    propagated = InlineSerializerHelper.Build<_PropagateConstants>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_PropagateConstants>.PropagateConstants = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_PropagateConstants>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_PropagateConstants>.PropagateConstants = true;
            }

            var rand = new Random(202457890);

            var toSerialize = new List<_PropagateConstants>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(new _PropagateConstants { });
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double allocationlessTime, normalTime;
            CompareTimes(toSerialize, propagated, normal, out allocationlessTime, out normalTime);

            Assert.IsTrue(allocationlessTime < normalTime, "propagatedTime = " + allocationlessTime + ", normalTime = " + normalTime);
        }

        class _AlwaysUseCharBufferForStrings
        {
            public enum Enums { Hello, World, Foo, Bar };

            public string A { get; set; }
            public string B { get; set; }
        }

        [TestMethod]
        public void AlwaysUseCharBufferForStrings()
        {
            // objects
            {
                Func<TextReader, _AlwaysUseCharBufferForStrings> fast;
                Func<TextReader, _AlwaysUseCharBufferForStrings> normal;

                try
                {
                    {
                        InlineDeserializer<_AlwaysUseCharBufferForStrings>.AlwaysUseCharBufferForStrings = true;
                        Exception ignored;

                        // Build the *actual* deserializer method
                        fast = InlineDeserializerHelper.Build<_AlwaysUseCharBufferForStrings>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                    }

                    {
                        InlineDeserializer<_AlwaysUseCharBufferForStrings>.AlwaysUseCharBufferForStrings = false;
                        Exception ignored;

                        // Build the *actual* deserializer method
                        normal = InlineDeserializerHelper.Build<_AlwaysUseCharBufferForStrings>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                    }
                }
                finally
                {
                    InlineDeserializer<_AlwaysUseCharBufferForStrings>.AlwaysUseCharBufferForStrings = true;
                }

                var rand = new Random(23450051);

                var toSerialize = new List<_AlwaysUseCharBufferForStrings>();
                for (var i = 0; i < 2000; i++)
                {
                    toSerialize.Add(
                        new _AlwaysUseCharBufferForStrings
                        {
                            A = _RandString(rand, 32),
                            B = _RandString(rand, 64)
                        }
                    );
                }

                toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

                double fastTime, normalTime;
                CompareTimes(toSerialize, Jil.Options.Default, fast, normal, out fastTime, out normalTime);

                Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
            }

            // strings
            {
                Func<TextReader, string> fast;
                Func<TextReader, string> normal;

                try
                {
                    {
                        InlineDeserializer<string>.AlwaysUseCharBufferForStrings = true;
                        Exception ignored;

                        // Build the *actual* deserializer method
                        fast = InlineDeserializerHelper.Build<string>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                    }

                    {
                        InlineDeserializer<string>.AlwaysUseCharBufferForStrings = false;
                        Exception ignored;

                        // Build the *actual* deserializer method
                        normal = InlineDeserializerHelper.Build<string>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                    }
                }
                finally
                {
                    InlineDeserializer<string>.AlwaysUseCharBufferForStrings = true;
                }

                var rand = new Random(49139290);

                var toSerialize = new List<string>();
                for (var i = 0; i < 2000; i++)
                {
                    toSerialize.Add(_RandString(rand, 32));
                }

                toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

                double fastTime, normalTime;
                CompareTimes(toSerialize, Jil.Options.Default, fast, normal, out fastTime, out normalTime);

                Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
            }
        }

        class _UseHashWhenMatchingMembers
        {
            public enum UserType : byte
            {
                unregistered = 2,
                registered = 3,
                moderator = 4,
                does_not_exist = 255
            }

            public int? user_id { get; set; }
            public string display_name { get; set; }
            public int? reputation { get; set; }
            public UserType? user_type { get; set; }
            public string link { get; set; }
            public int? accept_rate { get; set; }
        }

        [TestMethod]
        public void UseHashWhenMatchingMembers()
        {
            Func<TextReader, _UseHashWhenMatchingMembers> hash;
            Func<TextReader, _UseHashWhenMatchingMembers> dictionary;

            Assert.IsTrue(MemberMatcher<_UseHashWhenMatchingMembers>.IsAvailable);

            try
            {
                {
                    InlineDeserializer<_UseHashWhenMatchingMembers>.UseHashWhenMatchingMembers = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    hash = InlineDeserializerHelper.Build<_UseHashWhenMatchingMembers>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                }

                {
                    InlineDeserializer<_UseHashWhenMatchingMembers>.UseHashWhenMatchingMembers = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    dictionary = InlineDeserializerHelper.Build<_UseHashWhenMatchingMembers>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineDeserializer<_UseHashWhenMatchingMembers>.UseHashWhenMatchingMembers = true;
            }

            var rand = new Random(67982477);

            var toSerialize = new List<_UseHashWhenMatchingMembers>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(
                    new _UseHashWhenMatchingMembers
                    {
                        accept_rate = rand.Next(),
                        display_name = _RandString(rand),
                        link = _RandString(rand),
                        reputation = rand.Next(),
                        user_id = rand.Next(),
                        user_type = _RandEnum<_UseHashWhenMatchingMembers.UserType>(rand)
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double hashTime, dictionaryTime;
            CompareTimes(toSerialize, Jil.Options.Default, hash, dictionary, out hashTime, out dictionaryTime);

            Assert.IsTrue(hashTime < dictionaryTime, "hashTime = " + hashTime + ", dictionaryTime = " + dictionaryTime);
        }

        [TestMethod]
        public void DynamicDeserializer_UseFastNumberParsing()
        {
            try
            {
                Func<TextReader, double> fast =
                    txt =>
                    {
                        Jil.DeserializeDynamic.DynamicDeserializer.UseFastNumberParsing = true;

                        var ret = Jil.JSON.DeserializeDynamic(txt); ;

                        return (double)ret;
                    };
                Func<TextReader, double> normal =
                    txt =>
                    {
                        Jil.DeserializeDynamic.DynamicDeserializer.UseFastNumberParsing = false;

                        var ret = Jil.JSON.DeserializeDynamic(txt); ;

                        return (double)ret;
                    };

                var rand = new Random(201523240);

                var toSerialize = new List<double>();
                for (var i = 0; i < 3000; i++)
                {
                    var val = rand.NextDouble();
                    val *= rand.Next();

                    if (rand.Next(2) == 0) val = -val;

                    toSerialize.Add(val);
                }

                toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

                double fastTime, normalTime;
                CompareTimes(toSerialize, Jil.Options.Default, fast, normal, out fastTime, out normalTime);

                Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
            }
            finally
            {
                Jil.DeserializeDynamic.DynamicDeserializer.UseFastNumberParsing = true;
            }
        }

        [TestMethod]
        public void DynamicDeserializer_UseFastIntegerConversion()
        {
            try
            {
                Func<TextReader, int> fast =
                    txt =>
                    {
                        Jil.DeserializeDynamic.DynamicDeserializer.UseFastIntegerConversion = true;

                        var ret = Jil.JSON.DeserializeDynamic(txt);

                        return (int)ret;
                    };
                Func<TextReader, int> normal =
                    txt =>
                    {
                        Jil.DeserializeDynamic.DynamicDeserializer.UseFastIntegerConversion = false;

                        var ret = Jil.JSON.DeserializeDynamic(txt); ;

                        return (int)ret;
                    };

                var rand = new Random(211641195);

                var toSerialize = new List<int>();
                for (var i = 0; i < 10000; i++)
                {
                    var val = rand.Next();
                    if (rand.Next(2) == 0) val = -val;

                    toSerialize.Add(val);
                }

                toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

                double fastTime, normalTime;
                CompareTimes(toSerialize, Jil.Options.Default, fast, normal, out fastTime, out normalTime);

                Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
            }
            finally
            {
                Jil.DeserializeDynamic.DynamicDeserializer.UseFastIntegerConversion = true;
            }
        }

        enum _UseHashWhenMatchingEnums
        {
            Hello,
            World,
            Fizz,
            Buzz,
            Baz,
            Bar
        }

        [TestMethod]
        public void UseHashWhenMatchingEnums()
        {
            Func<TextReader, _UseHashWhenMatchingEnums> hash;
            Func<TextReader, _UseHashWhenMatchingEnums> method;

            Assert.IsTrue(EnumMatcher<_UseHashWhenMatchingEnums>.IsAvailable);

            try
            {
                {
                    InlineDeserializer<_UseHashWhenMatchingEnums>.UseHashWhenMatchingEnums = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    hash = InlineDeserializerHelper.Build<_UseHashWhenMatchingEnums>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<_UseHashWhenMatchingMembers>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                }

                {
                    InlineDeserializer<_UseHashWhenMatchingEnums>.UseHashWhenMatchingEnums = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    method = InlineDeserializerHelper.Build<_UseHashWhenMatchingEnums>(typeof(Jil.Deserialize.NewtonsoftStyleTypeCache<_UseHashWhenMatchingMembers>), dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineDeserializer<_UseHashWhenMatchingEnums>.UseHashWhenMatchingEnums = true;
            }

            var rand = new Random(191112901);

            var toSerialize = new List<_UseHashWhenMatchingEnums>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(_RandEnum<_UseHashWhenMatchingEnums>(rand));
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double hashTime, methodTime;
            CompareTimes(toSerialize, Jil.Options.Default, hash, method, out hashTime, out methodTime);

            Assert.IsTrue(hashTime < methodTime, "hashTime = " + hashTime + ", methodTime = " + methodTime);
        }

        class _UseCustomWriteIntUnrolled
        {
            public List<int> A { get; set; }
        }

        [TestMethod]
        public void UseCustomWriteIntUnrolled()
        {
            Action<TextWriter, _UseCustomWriteIntUnrolled, int> unrolled;
            Action<TextWriter, _UseCustomWriteIntUnrolled, int> normal;

            try
            {
                {
                    InlineSerializer<_UseCustomWriteIntUnrolled>.UseCustomWriteIntUnrolled = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    unrolled = InlineSerializerHelper.Build<_UseCustomWriteIntUnrolled>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseCustomWriteIntUnrolled>.UseCustomWriteIntUnrolled = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseCustomWriteIntUnrolled>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseCustomWriteIntUnrolled>.UseCustomWriteIntUnrolled = true;
            }

            var rand = new Random(89210872);

            var toSerialize = new List<_UseCustomWriteIntUnrolled>();
            for (var i = 0; i < 1000; i++)
            {
                toSerialize.Add(
                    new _UseCustomWriteIntUnrolled 
                    { 
                        A = Enumerable.Range(0, rand.Next(1, 1000)).Select(_ => rand.Next(2) == 0 ? rand.Next() : -rand.Next()).ToList()
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double unrolledTime, normalTime;
            CompareTimes(toSerialize, unrolled, normal, out unrolledTime, out normalTime);

            Assert.IsTrue(unrolledTime < normalTime, "unrolledTime = " + unrolledTime + ", normalTime = " + normalTime);
        }

        class _UseCustomWriteIntUnrolledSigned
        {
            public List<int> A { get; set; }
        }

        [TestMethod]
        public void UseCustomWriteIntUnrolledSigned()
        {
            Action<TextWriter, _UseCustomWriteIntUnrolledSigned, int> signed;
            Action<TextWriter, _UseCustomWriteIntUnrolledSigned, int> normal;

            try
            {
                {
                    InlineSerializer<_UseCustomWriteIntUnrolledSigned>.UseCustomWriteIntUnrolledSigned = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    signed = InlineSerializerHelper.Build<_UseCustomWriteIntUnrolledSigned>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseCustomWriteIntUnrolledSigned>.UseCustomWriteIntUnrolledSigned = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseCustomWriteIntUnrolledSigned>(typeof(Jil.Serialize.NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseCustomWriteIntUnrolledSigned>.UseCustomWriteIntUnrolledSigned = true;
            }

            var rand = new Random(27899810);

            var toSerialize = new List<_UseCustomWriteIntUnrolledSigned>();
            for (var i = 0; i < 1000; i++)
            {
                toSerialize.Add(
                    new _UseCustomWriteIntUnrolledSigned
                    {
                        A = Enumerable.Range(0, rand.Next(1, 1000)).Select(_ => rand.Next(2) == 0 ? rand.Next() : -rand.Next()).ToList()
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double signedTime, normalTime;
            CompareTimes(toSerialize, signed, normal, out signedTime, out normalTime);

            Assert.IsTrue(signedTime < normalTime, "signedTime = " + signedTime + ", normalTime = " + normalTime);
        }
#endif
    }
}
