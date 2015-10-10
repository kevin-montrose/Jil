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
        private static uint _RandUInt(Random rand)
        {
            var bytes = new byte[4];
            rand.NextBytes(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

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

        private static void CompareTimes<T>(List<string> asStrings, Jil.Options opts, Func<TextReader, int, T> a, Func<TextReader, int, T> b, out double aTimeMS, out double bTimeMS)
        {
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
                            a(str, 0);
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
                            b(str, 0);
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

        private static void CompareTimes<T>(List<T> toSerialize, Jil.Options opts, Func<TextReader, int, T> a, Func<TextReader, int, T> b, out double aTimeMS, out double bTimeMS)
        {
            var asStrings = toSerialize.Select(o => Jil.JSON.Serialize(o, opts)).ToList();

            CompareTimes(asStrings, opts, a, b, out aTimeMS, out bTimeMS);
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
                    memoryOrder = InlineSerializerHelper.Build<_ReorderMembers>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_ReorderMembers>.ReorderMembers = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normalOrder = InlineSerializerHelper.Build<_ReorderMembers>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    custom = InlineSerializerHelper.Build<_UseCustomIntegerToString>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseCustomIntegerToString>.UseCustomIntegerToString = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseCustomIntegerToString>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    skipped = InlineSerializerHelper.Build<_SkipDateTimeMathMethods>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_SkipDateTimeMathMethods>.SkipDateTimeMathMethods = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_SkipDateTimeMathMethods>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    skipped = InlineSerializerHelper.Build<_UseCustomISODateFormatting>(typeof(Jil.Serialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.ISO8601, pretty: false, excludeNulls: false, jsonp: false, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseCustomISODateFormatting>.UseCustomISODateFormatting = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseCustomISODateFormatting>(typeof(Jil.Serialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.ISO8601, pretty: false, excludeNulls: false, jsonp: false, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    fast = InlineSerializerHelper.Build<_UseFastLists>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseFastLists>.UseFastLists = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseFastLists>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    fast = InlineSerializerHelper.Build<_UseFastArrays>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseFastArrays>.UseFastArrays = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseFastArrays>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    fast = InlineSerializerHelper.Build<_UseFastGuids>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseFastGuids>.UseFastGuids = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseFastGuids>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    allocationless = InlineSerializerHelper.Build<_AllocationlessDictionaries>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_AllocationlessDictionaries>.AllocationlessDictionaries = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_AllocationlessDictionaries>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
                    propagated = InlineSerializerHelper.Build<_PropagateConstants>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_PropagateConstants>.PropagateConstants = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_PropagateConstants>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
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
        public void DynamicDeserializer_UseFastNumberParsing()
        {
            try
            {
                Func<TextReader, int, double> fast =
                    (txt, _) =>
                    {
                        Jil.DeserializeDynamic.DynamicDeserializer.UseFastNumberParsing = true;

                        var ret = Jil.JSON.DeserializeDynamic(txt); ;

                        return (double)ret;
                    };
                Func<TextReader, int, double> normal =
                    (txt, _) =>
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
                Func<TextReader, int, int> fast =
                    (txt, _) =>
                    {
                        Jil.DeserializeDynamic.DynamicDeserializer.UseFastIntegerConversion = true;

                        var ret = Jil.JSON.DeserializeDynamic(txt);

                        return (int)ret;
                    };
                Func<TextReader, int, int> normal =
                    (txt, _) =>
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

        enum _UseNameAutomataWhenMatchingEnums
        {
            Hello,
            World,
            Fizz,
            Buzz,
            Baz,
            Bar
        }

        [TestMethod]
        public void UseNameAutomataWhenMatchingEnums()
        {
            Func<TextReader, int, _UseNameAutomataWhenMatchingEnums> automata;
            Func<TextReader, int, _UseNameAutomataWhenMatchingEnums> method;


            try
            {
                {
                    InlineDeserializer<_UseNameAutomataWhenMatchingEnums>.UseNameAutomataForEnums = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    automata = InlineDeserializerHelper.BuildFromStream<_UseNameAutomataWhenMatchingEnums>(typeof(Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyle, _UseNameAutomataWhenMatchingEnums>), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineDeserializer<_UseNameAutomataWhenMatchingEnums>.UseNameAutomataForEnums = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    method = InlineDeserializerHelper.BuildFromStream<_UseNameAutomataWhenMatchingEnums>(typeof(Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyle, _UseHashWhenMatchingMembers>), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineDeserializer<_UseNameAutomataWhenMatchingEnums>.UseNameAutomataForEnums = true;
            }

            var rand = new Random(191112901);

            var toSerialize = new List<_UseNameAutomataWhenMatchingEnums>();
            for (var i = 0; i < 2000; i++)
            {
                toSerialize.Add(_RandEnum<_UseNameAutomataWhenMatchingEnums>(rand));
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double automataTime, methodTime;
            CompareTimes(toSerialize, Jil.Options.Default, automata, method, out automataTime, out methodTime);

            Assert.IsTrue(automataTime < methodTime, "automataTime = " + automataTime + ", methodTime = " + methodTime);
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
                    InlineSerializer<_UseCustomWriteIntUnrolledSigned>.UseCustomWriteIntUnrolled = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    signed = InlineSerializerHelper.Build<_UseCustomWriteIntUnrolledSigned>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_UseCustomWriteIntUnrolledSigned>.UseCustomWriteIntUnrolled = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_UseCustomWriteIntUnrolledSigned>(typeof(Jil.Serialize.MicrosoftStyle), pretty: false, excludeNulls: false, jsonp: false, dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_UseCustomWriteIntUnrolledSigned>.UseCustomWriteIntUnrolled = true;
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

        class _UseNameAutomata
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
        public void UseNameAutomata()
        {
            Func<TextReader, int, _UseNameAutomata> automata;
            Func<TextReader, int, _UseNameAutomata> dictionary;

            try
            {
                {
                    InlineDeserializer<_UseNameAutomata>.UseNameAutomata = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    automata = InlineDeserializerHelper.BuildFromStream<_UseNameAutomata>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineDeserializer<_UseNameAutomata>.UseNameAutomata = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    dictionary = InlineDeserializerHelper.BuildFromStream<_UseNameAutomata>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineDeserializer<_UseNameAutomata>.UseNameAutomata = true;
            }

            var rand = new Random(97031664);

            var toSerialize = new List<_UseNameAutomata>();
            for (var i = 0; i < 10000; i++)
            {
                toSerialize.Add(
                    new _UseNameAutomata
                    {
                        accept_rate = rand.Next(),
                        display_name = _RandString(rand),
                        link = _RandString(rand),
                        reputation = rand.Next(),
                        user_id = rand.Next(),
                        user_type = _RandEnum<_UseNameAutomata.UserType>(rand)
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double automataTime, dictionaryTime;
            CompareTimes(toSerialize, Jil.Options.Default, automata, dictionary, out automataTime, out dictionaryTime);

            Assert.IsTrue(automataTime < dictionaryTime, "automataTime = " + automataTime + ", dictionaryTime = " + dictionaryTime);
        }

        [TestMethod]
        public void UseFastFloatingPointMethods_Float_Common()
        {
            Func<TextReader, int, float> fast;
            Func<TextReader, int, float> normal;

            try
            {
                {
                    InlineDeserializer<float>.UseFastFloatingPointMethods = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    fast = InlineDeserializerHelper.BuildFromStream<float>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineDeserializer<float>.UseFastFloatingPointMethods = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    normal = InlineDeserializerHelper.BuildFromStream<float>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineDeserializer<float>.UseFastFloatingPointMethods = true;
            }

            var toSerialize = new List<float>();
            float i = -100.0f;
            while (i <= 100.0f)
            {   
                toSerialize.Add((float)Math.Round(i, 2));
                i += 0.01f;
            }

            double fastTime, normalTime;
            CompareTimes(toSerialize, Jil.Options.Default, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        [TestMethod]
        public void UseFastFloatingPointMethods_Double_Common()
        {
            Func<TextReader, int, double> fast;
            Func<TextReader, int, double> normal;

            try
            {
                {
                    InlineDeserializer<double>.UseFastFloatingPointMethods = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    fast = InlineDeserializerHelper.BuildFromStream<double>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineDeserializer<double>.UseFastFloatingPointMethods = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    normal = InlineDeserializerHelper.BuildFromStream<double>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineDeserializer<double>.UseFastFloatingPointMethods = true;
            }

            var toSerialize = new List<double>();
            double i = -100.0;
            while (i <= 100.0)
            {
                toSerialize.Add(Math.Round(i, 2));
                i += 0.01;
            }

            double fastTime, normalTime;
            CompareTimes(toSerialize, Jil.Options.Default, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        [TestMethod]
        public void UseFastFloatingPointMethods_Decimal_Common()
        {
            Func<TextReader, int, decimal> fast;
            Func<TextReader, int, decimal> normal;

            try
            {
                {
                    InlineDeserializer<decimal>.UseFastFloatingPointMethods = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    fast = InlineDeserializerHelper.BuildFromStream<decimal>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineDeserializer<decimal>.UseFastFloatingPointMethods = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    normal = InlineDeserializerHelper.BuildFromStream<decimal>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineDeserializer<decimal>.UseFastFloatingPointMethods = true;
            }

            var toSerialize = new List<decimal>();
            decimal i = -100.0m;
            while (i <= 100.0m)
            {
                toSerialize.Add(Math.Round(i, 2));
                i += 0.01m;
            }

            double fastTime, normalTime;
            CompareTimes(toSerialize, Jil.Options.Default, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        class _NameAutomataBinarySearch
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string E { get; set; }
            public string F { get; set; }
            public string G { get; set; }
            public string H { get; set; }
            public string I { get; set; }
            public string J { get; set; }
            public string K { get; set; }
            public string L { get; set; }
            public string M { get; set; }
            public string N { get; set; }
            public string O { get; set; }
            public string P { get; set; }
            public string Q { get; set; }
            public string R { get; set; }
            public string S { get; set; }
            public string T { get; set; }
            public string U { get; set; }
            public string V { get; set; }
            public string W { get; set; }
            public string X { get; set; }
            public string Y { get; set; }
            public string Z { get; set; }
        }

        [TestMethod]
        public void NameAutomataBinarySearch()
        {
            var obj =
                new _NameAutomataBinarySearch
                {
                    A = "a",
                    B = "b",
                    C = "c",
                    D = "d",
                    E = "e",
                    F = "f",
                    G = "g",
                    H = "h",
                    I = "i",
                    J = "j",
                    K = "k",
                    L = "L",
                    M = "m",
                    N = "n",
                    O = "o",
                    P = "p",
                    Q = "q",
                    R = "r",
                    S = "s",
                    T = "t",
                    U = "u",
                    V = "v",
                    W = "w",
                    X = "x",
                    Y = "y",
                    Z = "z"
                };

            Func<TextReader, int, _NameAutomataBinarySearch> binarySearch;
            Func<TextReader, int, _NameAutomataBinarySearch> normal;

            try
            {
                {
                    NameAutomataConfig.UseBinarySearch = true;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    binarySearch = InlineDeserializerHelper.BuildFromStream<_NameAutomataBinarySearch>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    NameAutomataConfig.UseBinarySearch = false;
                    Exception ignored;

                    // Build the *actual* deserializer method
                    normal = InlineDeserializerHelper.BuildFromStream<_NameAutomataBinarySearch>(typeof(Jil.Deserialize.MicrosoftStyle), dateFormat: Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                NameAutomataConfig.UseBinarySearch = true;
            }

            var toSerialize = new List<_NameAutomataBinarySearch>();
            for (var i = 0; i < 10000; i++)
            {
                toSerialize.Add(obj);
            }

            double binarySearchTime, normalTime;
            CompareTimes(toSerialize, Jil.Options.Default, binarySearch, normal, out binarySearchTime, out normalTime);

            Assert.IsTrue(binarySearchTime < normalTime, "binarySearchTime = " + binarySearchTime + ", normalTime = " + normalTime);
        }

        [TestMethod]
        public void DynamicTypeConverter()
        {
            Func<TextReader, int, object> trialCasts =
                (reader, depth) =>
                {
                    var dyn = JSON.DeserializeDynamic(reader);

                    try
                    {
                        var asInt = (int)dyn;
                        return asInt;
                    }
                    catch { }

                    try
                    {
                        var asDouble = (double)dyn;
                        return asDouble;
                    }
                    catch { }

                    try
                    {
                        var asString = (string)dyn;
                        return asString;
                    }
                    catch { }

                    return null;
                };
            Func<TextReader, int, object> typeConverter =
                (reader, depth) =>
                {
                    var dyn = JSON.DeserializeDynamic(reader);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);

                    if (tc.CanConvertTo(typeof(int))) return (int)dyn;
                    if (tc.CanConvertTo(typeof(double))) return (double)dyn;
                    if (tc.CanConvertTo(typeof(string))) return (string)dyn;

                    return null;
                };

            var rand = new Random(97031664);

            var toSerialize = new List<object>();
            for (var i = 0; i < 100; i++)
            {
                switch(rand.Next(4))
                {
                    case 0: toSerialize.Add(rand.Next()); break;
                    case 1: toSerialize.Add(rand.NextDouble() * rand.Next()); break;
                    case 2: toSerialize.Add(_RandString(rand)); break;
                    case 3: toSerialize.Add(new object()); break;
                    default: throw new Exception();
                }
            }

            double trialCastTime, typeConverterTime;
            CompareTimes(toSerialize, Jil.Options.Default, trialCasts, typeConverter, out trialCastTime, out typeConverterTime);

            Assert.IsTrue(typeConverterTime < trialCastTime, "typeConverterTime = " + typeConverterTime + ", trialCastTime = " + trialCastTime);
        }

        public class _CustomRFC1123DateFormatting
        {
            public List<DateTime> Dates;
        }

        [TestMethod]
        public void UseCustomRFC1123DateFormatting()
        {
            Action<TextWriter, _CustomRFC1123DateFormatting, int> fast;
            Action<TextWriter, _CustomRFC1123DateFormatting, int> normal;

            try
            {
                {
                    InlineSerializer<_CustomRFC1123DateFormatting>.UseCustomRFC1123DateTimeFormatting = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    fast = InlineSerializerHelper.Build<_CustomRFC1123DateFormatting>(typeof(Jil.Serialize.RFC1123), dateFormat: Jil.DateTimeFormat.RFC1123, pretty: false, excludeNulls: false, jsonp: false, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }

                {
                    InlineSerializer<_CustomRFC1123DateFormatting>.UseCustomRFC1123DateTimeFormatting = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineSerializerHelper.Build<_CustomRFC1123DateFormatting>(typeof(Jil.Serialize.RFC1123), dateFormat: Jil.DateTimeFormat.RFC1123, pretty: false, excludeNulls: false, jsonp: false, includeInherited: false, dateTimeBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, serializationNameFormat: SerializationNameFormat.Verbatim, exceptionDuringBuild: out ignored);
                }
            }
            finally
            {
                InlineSerializer<_CustomRFC1123DateFormatting>.UseCustomRFC1123DateTimeFormatting = true;
            }

            var rand = new Random(270129617);

            var toSerialize = new List<_CustomRFC1123DateFormatting>();
            for (var i = 0; i < 1000; i++)
            {
                var numDates = new DateTime[5 + rand.Next(10)];

                for (var j = 0; j < numDates.Length; j++)
                {
                    numDates[j] = _RandDateTime(rand);
                }

                toSerialize.Add(
                    new _CustomRFC1123DateFormatting
                    {
                        Dates = numDates.ToList()
                    }
                );
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double fastTime, normalTime;
            CompareTimes(toSerialize, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        [TestMethod]
        public void UseFastRFC1123Method()
        {
            Func<TextReader, int, DateTime> fast;
            Func<TextReader, int, DateTime> normal;

            try
            {
                {
                    InlineDeserializer<DateTime>.UseFastRFC1123Method = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    fast = InlineDeserializerHelper.BuildFromStream<DateTime>(typeof(Jil.Serialize.RFC1123), DateTimeFormat.RFC1123, SerializationNameFormat.Verbatim, out ignored);
                }

                {
                    InlineDeserializer<DateTime>.UseFastRFC1123Method = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineDeserializerHelper.BuildFromStream<DateTime>(typeof(Jil.Serialize.RFC1123), DateTimeFormat.RFC1123, SerializationNameFormat.Verbatim, out ignored);
                }
            }
            finally
            {
                InlineDeserializer<DateTime>.UseFastRFC1123Method = true;
            }

            var rand = new Random(270129617);

            var toDeserialize = new List<DateTime>();
            for (var i = 0; i < 1000; i++)
            {
                toDeserialize.Add(_RandDateTime(rand));
            }

            toDeserialize = toDeserialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double fastTime, normalTime;
            CompareTimes(toDeserialize, Options.RFC1123, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }

        class _DeserializeThunkReader
        {
            public string Str { get; set; }
            public int Int { get; set; }
            public List<string> List { get; set; }
            public Dictionary<string, int> Dict { get; set; }
            public _DeserializeThunkReader Recurs { get; set; }
            public double Float { get; set; }
        }

        [TestMethod]
        public void DeserializeThunkReader()
        {
            Func<TextReader, int, _DeserializeThunkReader> thunkReader;
            Func<TextReader, int, _DeserializeThunkReader> streamReader;

            thunkReader =
                (tr, ix) =>
                {
                    var str = tr.ReadToEnd();

                    return JSON.Deserialize<_DeserializeThunkReader>(str);
                };

            streamReader =
                (tr, ix) =>
                {
                    // have to ReadToEnd for symmetry with `thunkReader`
                    var str = tr.ReadToEnd();

                    using(var r = new StringReader(str))
                    {
                        return JSON.Deserialize<_DeserializeThunkReader>(r);
                    }
                };

            var rand = new Random(141026127);

            var toDeserialize = new List<_DeserializeThunkReader>();
            for (var i = 0; i < 5000; i++)
            {
                toDeserialize.Add(
                    new _DeserializeThunkReader
                    {
                        Float = rand.NextDouble(),
                        Int = rand.Next(),
                        Str = _RandString(rand),
                        Recurs = new _DeserializeThunkReader
                        {
                            Int = rand.Next(),
                            Str = _RandString(rand),
                            List = Enumerable.Range(0, rand.Next(5)).Select(r => _RandString(rand)).ToList(),
                            Dict = Enumerable.Range(0, rand.Next(5)).ToDictionary(x => x+" "+_RandString(rand), _ => rand.Next()),
                            Float = rand.NextDouble()
                        },
                        List = Enumerable.Range(0, rand.Next(5)).Select(r => _RandString(rand)).ToList(),
                        Dict = Enumerable.Range(0, rand.Next(5)).ToDictionary(x => x + " " + _RandString(rand), _ => rand.Next())
                    }
                );
            }

            toDeserialize = toDeserialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            double thunkReaderTime, streamReaderTime;
            CompareTimes(toDeserialize, Options.RFC1123, thunkReader, streamReader, out thunkReaderTime, out streamReaderTime);

            Assert.IsTrue(thunkReaderTime < streamReaderTime, "thunkReaderTime = " + thunkReaderTime + ", streamReaderTime = " + streamReaderTime);
        }

        class _UseFastUnionLookup
        {
            [JilDirective(IsUnion = true, Name = "Data")]
            public Dictionary<string, string> AsDict { get; set; }
            [JilDirective(IsUnion = true, Name = "Data")]
            public List<int> AsList { get; set; }
            [JilDirective(IsUnion = true, Name = "Data")]
            public int AsNum { get; set; }
            [JilDirective(IsUnion = true, Name = "Data")]
            public string AsStr { get; set; }
        }

        [TestMethod]
        public void UseFastUnionLookup()
        {
            Func<TextReader, int, _UseFastUnionLookup> fast;
            Func<TextReader, int, _UseFastUnionLookup> normal;

            try
            {
                {
                    InlineDeserializer<_UseFastUnionLookup>.UseFastUnionLookup = true;
                    Exception ignored;

                    // Build the *actual* serializer method
                    fast = InlineDeserializerHelper.BuildFromStream<_UseFastUnionLookup>(typeof(Jil.Serialize.RFC1123), DateTimeFormat.RFC1123, SerializationNameFormat.Verbatim, out ignored);
                }

                {
                    InlineDeserializer<_UseFastUnionLookup>.UseFastUnionLookup = false;
                    Exception ignored;

                    // Build the *actual* serializer method
                    normal = InlineDeserializerHelper.BuildFromStream<_UseFastUnionLookup>(typeof(Jil.Serialize.RFC1123), DateTimeFormat.RFC1123, SerializationNameFormat.Verbatim, out ignored);
                }
            }
            finally
            {
                InlineDeserializer<_UseFastUnionLookup>.UseFastUnionLookup = true;
            }

            var rand = new Random(270129617);

            var json = new List<string>();
            for (var i = 0; i < 100000; i++)
            {
                var part = rand.Next(4);
                switch (part)
                {
                    case 0:
                        json.Add("{\"Data\": " + JSON.Serialize(_RandString(rand)) + "}");
                        break;
                    case 1:
                        json.Add("{\"Data\": {\"Foo\": \"Bar\"}}");
                        break;
                    case 2:
                        json.Add("{\"Data\": [1,2,3]}");
                        break;
                    case 3:
                        json.Add("{\"Data\": 456}");
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            double fastTime, normalTime;
            CompareTimes(json, Options.RFC1123, fast, normal, out fastTime, out normalTime);

            Assert.IsTrue(fastTime < normalTime, "fastTime = " + fastTime + ", normalTime = " + normalTime);
        }
#endif
    }
}
