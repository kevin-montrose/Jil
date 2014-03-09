using Jil.Common;
using Jil.Serialize;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiments
{
    class Program
    {
        class ComplexType
        {
            public class MemberType
            {
                public int A;
                public string B;

                public static MemberType Random(Random rand)
                {
                    return
                        new MemberType
                        {
                            A = rand.Next(),
                            B = rand.NextString()
                        };
                }
            }

            public int A;
            public int? B;
            public DateTime C;
            public DateTime? D;
            public string E;
            public char F;
            public char? G;

            public List<int> H;
            public double[] I;

            public MemberType J;

            public ComplexType Next;

            public static ComplexType Random(Random rand)
            {
                var ret = new ComplexType
                {
                    A = rand.Next(),
                    B = rand.NextNullableInt(),
                    C = rand.NextDateTime(),
                    D = rand.NextNullableDateTime(),
                    E = rand.NextString(),
                    F = rand.NextChar(),
                    G = rand.NextNullableChar(),
                    H = rand.NextIntList().ToList(),
                    I = rand.NextDoubleList().ToArray(),
                    J = rand.Next(2) == 0 ? MemberType.Random(rand) : null,
                    Next = rand.Next(2) == 0 ? Random(rand) : null
                };

                return ret;
            }
        }

        private static IEnumerable<IEnumerable<T>> Permutate<T>(IEnumerable<T> ixs)
        {
            var len = ixs.Count();

            if (len == 1)
            {
                yield return ixs;
            }
            else
            {
                for (var i = 0; i < len; i++)
                {
                    var head = ixs.ElementAt(i);
                    var tail = ixs.Where((_, j) => j != i);

                    var tailPerms = Permutate(tail);

                    foreach (var t in tailPerms)
                    {
                        yield return (new T[] { head }.Concat(t));
                    }
                }
            }
        }

        class FindFastOrderComparer : IComparer<Dictionary<int, int>>
        {
            public int Compare(Dictionary<int, int> x, Dictionary<int, int> y)
            {
                var allKeys = x.Keys.Concat(y.Keys).Distinct().OrderBy(_ => _).ToList();

                foreach (var key in allKeys)
                {
                    int inX, inY;

                    if (!x.TryGetValue(key, out inX)) inX = 0;
                    if (!y.TryGetValue(key, out inY)) inY = 0;

                    if (inX == inY) continue;

                    if (inX < inY) return -1;

                    return 1;
                }

                return 0;
            }
        }

        private static void CompareTime<T>(List<T> toSerialize, Action<TextWriter, T, int> mtd, out double timeMS)
        {
            var timer = new Stopwatch();

            Action time =
                () =>
                {
                    timer.Start();
                    for (var i = 0; i < toSerialize.Count; i++)
                    {
                        using (var str = new StringWriter())
                        {
                            mtd(str, toSerialize[i], 0);
                        }
                    }
                    timer.Stop();
                };


            for (var i = 0; i < 3; i++)
            {
                time();
            }

            timer.Reset();

            for (var i = 0; i < 20; i++)
            {
                time();
            }

            timeMS = timer.ElapsedMilliseconds;
        }

        const string NormalKey = "--normal--";
        private static Dictionary<string, Action<TextWriter, ComplexType, int>> MethodCache = new Dictionary<string, Action<TextWriter, ComplexType, int>>();
        private static void Compile(IEnumerable<int[]> ixs)
        {
            try
            {
                InlineSerializer<ComplexType>.ReorderMembers = false;
                Exception ignored;

                // Build the *actual* serializer method
                var normal = InlineSerializerHelper.Build<ComplexType>(typeof(NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);

                MethodCache[NormalKey] = normal;
            }
            finally
            {
                InlineSerializer<ComplexType>.ReorderMembers = true;
            }

            foreach (var ix in ixs)
            {
                foreach (var perm in Permutate(ix))
                {
                    var key = string.Join(",", perm);

                    Console.WriteLine("Compiling: " + key);

                    Utils.MemberOrdering = perm.ToArray();

                    Action<TextWriter, ComplexType, int> mtd;

                    try
                    {
                        Exception ignored;

                        // Build the *actual* serializer method
                        mtd = InlineSerializerHelper.Build<ComplexType>(typeof(NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, jsonp: false, dateFormat: Jil.DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ignored);
                    }
                    finally
                    {
                        Utils.MemberOrdering = new[] { 1, 2, 3, 4 };
                    }

                    MethodCache[key] = mtd;
                }
            }

            Console.WriteLine("\t" + MethodCache.Count + " versions to test");
        }

        private static IEnumerable<string> TopHalf(Random rand, List<double> normalTimes)
        {
            Console.WriteLine("Starting run with " + MethodCache.Count() + " starting points");

            var toSerialize = new List<ComplexType>();
            for (var i = 0; i < 500; i++)
            {
                toSerialize.Add(ComplexType.Random(rand));
            }

            toSerialize = toSerialize.Select(_ => new { _ = _, Order = rand.Next() }).OrderBy(o => o.Order).Select(o => o._).Where((o, ix) => ix % 2 == 0).ToList();

            System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);

            var scores = new Dictionary<string, Dictionary<int, int>>();
            var minTimes = new Dictionary<string, double>();
            var maxTimes = new Dictionary<string, double>();

            Console.WriteLine("\tStarting runs");

            for (var i = 1; i <= 10; i++)
            {
                Console.WriteLine("\t\tRun #" + i);

                var inOrder =
                    MethodCache
                    .Select(_ => new { _, Order = rand.Next() })
                    .OrderBy(_ => _.Order)
                    .Select(_ => _._)
                    .Select(
                        kv =>
                        {
                            var mtd = kv.Value;

                            double t;
                            CompareTime(toSerialize, mtd, out t);

                            if (kv.Key == NormalKey)
                            {
                                normalTimes.Add(t);
                            }

                            return Tuple.Create(kv.Key, t);
                        }
                    )
                    .OrderBy(t => t.Item2)
                    .ToList();

                double runMin = double.MaxValue;
                double runMax = double.MinValue;
                double runNormal = -1;

                for (var j = 0; j < inOrder.Count; j++)
                {
                    var t = inOrder[j];
                    var key = t.Item1;

                    if (t.Item2 < runMin) runMin = t.Item2;
                    if (t.Item2 > runMax) runMax = t.Item2;
                    if (key == NormalKey) runNormal = t.Item2;

                    Dictionary<int, int> inRank;
                    if (!scores.TryGetValue(key, out inRank))
                    {
                        scores[key] = inRank = new Dictionary<int, int>();
                    }

                    if (!inRank.ContainsKey(j))
                    {
                        inRank[j] = 0;
                    }

                    inRank[j]++;

                    double min, max;
                    if (!minTimes.TryGetValue(key, out min))
                    {
                        minTimes[key] = min = int.MaxValue;
                    }

                    if (!maxTimes.TryGetValue(key, out max))
                    {
                        maxTimes[key] = max = 0;
                    }

                    if (t.Item2 < min) minTimes[key] = t.Item2;
                    if (t.Item2 > max) maxTimes[key] = t.Item2;
                }

                Console.WriteLine("\t\t\tMin: " + runMin);
                Console.WriteLine("\t\t\tMax: " + runMax);
                Console.WriteLine("\t\t\tNormal Time: " + runNormal);
            }

            var minest = minTimes.Values.Min();
            var maxest = maxTimes.Values.Max();

            Console.WriteLine("\tMin: " + minest);
            Console.WriteLine("\tMax: " + maxest);
            Console.WriteLine("\tNormal Min: " + minTimes[NormalKey]);
            Console.WriteLine("\tNormal Max: " + maxTimes[NormalKey]);

            var take = scores.Count / 2;
            if (take == 0) take = 1;

            var topHalf =
                scores
                .Where(kv => kv.Key != NormalKey)
                .OrderBy(
                    kv => kv.Value,
                    new FindFastOrderComparer()
                )
                .Take(take)
                .Select(kv => kv.Key)
                .ToList();

            return topHalf;
        }

        public static int[] FindFastOrder()
        {
            var initial = new List<int[]> {
                new [] {  1,  2,  3,  4 },
                new [] { -1,  2,  3,  4 },
                new [] {  1, -2,  3,  4 },
                new [] {  1,  2, -3,  4 },
                new [] {  1,  2,  3, -4 },
                new [] { -1, -2,  3,  4 },
                new [] { -1,  2, -3,  4 },
                new [] { -1,  2,  3, -4 },
                new [] {  1, -2, -3,  4 },
                new [] {  1, -2,  3, -4 },
                new [] {  1,  2, -3, -4 },
                new [] { -1, -2, -3,  4 },
                new [] { -1, -2,  3, -4 },
                new [] { -1,  2, -3, -4 },
                new [] {  1, -2, -3, -4 },
                new [] { -1, -2, -3, -4 }
            };

            var rand = new Random(24923781);

            Compile(initial);

            IEnumerable<string> keep;
            var normalTimes = new List<double>();

            do
            {
                keep = TopHalf(rand, normalTimes);
                var toRemove = MethodCache.Keys.Where(k => !keep.Contains(k) && k != NormalKey).ToList();

                toRemove.ForEach(k => MethodCache.Remove(k));
            } while (keep.Count() != 1);

            var str = keep.Single();

            Console.WriteLine("Min Normal: " + normalTimes.Min());
            Console.WriteLine("Avg Normal: " + normalTimes.Average());
            Console.WriteLine("Median Normal: " + normalTimes.Median());
            Console.WriteLine("Max Normal: " + normalTimes.Max());

            return str.Split(',').Select(i => int.Parse(i)).ToArray();
        }

        class ForkedWriter : TextWriter
        {
            private TextWriter[] Writers;

            public ForkedWriter(params TextWriter[] writers)
            {
                Writers = writers;
            }

            public override Encoding Encoding
            {
                get { return Writers[0].Encoding; }
            }


            public override IFormatProvider FormatProvider
            {
                get
                {
                    return Writers[0].FormatProvider;
                }
            }

            public override string NewLine
            {
                get
                {
                    return Writers[0].NewLine;
                }
                set
                {
                    Writers[0].NewLine = value;
                }
            }

            public override void Close()
            {
                Writers.ForEach(w => w.Close());
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Writers.ForEach(w => w.Dispose());
                }
            }

            public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
            {
                throw new NotImplementedException();
            }

            public override void Flush()
            {
                Writers.ForEach(w => w.Flush());
            }

            public override Task FlushAsync()
            {
                return Task.WhenAll(Writers.Select(w => w.FlushAsync()));
            }

            public override void Write(bool value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(char value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(char[] buffer)
            {
                Writers.ForEach(w => w.Write(buffer));
            }

            public override void Write(char[] buffer, int index, int count)
            {
                Writers.ForEach(w => w.Write(buffer, index, count));
            }

            public override void Write(decimal value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(double value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(float value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(int value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(long value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(object value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(string format, object arg0)
            {
                Writers.ForEach(w => w.Write(format, arg0));
            }

            public override void Write(string format, object arg0, object arg1)
            {
                Writers.ForEach(w => w.Write(format, arg0, arg1));
            }

            public override void Write(string format, object arg0, object arg1, object arg2)
            {
                Writers.ForEach(w => w.Write(format, arg0, arg1, arg2));
            }

            public override void Write(string format, params object[] arg)
            {
                Writers.ForEach(w => w.Write(format, arg));
            }

            public override void Write(string value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(uint value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override void Write(ulong value)
            {
                Writers.ForEach(w => w.Write(value));
            }

            public override Task WriteAsync(char value)
            {
                return Task.WhenAll(Writers.Select(w => w.WriteAsync(value)));
            }

            public override Task WriteAsync(char[] buffer, int index, int count)
            {
                return Task.WhenAll(Writers.Select(w => w.WriteAsync(buffer, index, count)));
            }

            public override Task WriteAsync(string value)
            {
                return Task.WhenAll(Writers.Select(w => w.WriteAsync(value)));
            }

            public override void WriteLine()
            {
                Writers.ForEach(w => w.WriteLine());
            }

            public override void WriteLine(bool value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(char value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(char[] buffer)
            {
                Writers.ForEach(w => w.WriteLine(buffer));
            }

            public override void WriteLine(char[] buffer, int index, int count)
            {
                Writers.ForEach(w => w.WriteLine(buffer, index, count));
            }

            public override void WriteLine(decimal value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(double value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(float value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(int value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(long value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(object value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(string format, object arg0)
            {
                Writers.ForEach(w => w.WriteLine(format, arg0));
            }

            public override void WriteLine(string format, object arg0, object arg1)
            {
                Writers.ForEach(w => w.WriteLine(format, arg0, arg1));
            }

            public override void WriteLine(string format, object arg0, object arg1, object arg2)
            {
                Writers.ForEach(w => w.WriteLine(format, arg0, arg1, arg2));
            }

            public override void WriteLine(string format, params object[] arg)
            {
                Writers.ForEach(w => w.WriteLine(format, arg));
            }

            public override void WriteLine(string value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(uint value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override void WriteLine(ulong value)
            {
                Writers.ForEach(w => w.WriteLine(value));
            }

            public override Task WriteLineAsync()
            {
                return Task.WhenAll(Writers.Select(w => w.WriteLineAsync()));
            }

            public override Task WriteLineAsync(char value)
            {
                return Task.WhenAll(Writers.Select(w => w.WriteLineAsync(value)));
            }

            public override Task WriteLineAsync(char[] buffer, int index, int count)
            {
                return Task.WhenAll(Writers.Select(w => w.WriteLineAsync(buffer, index, count)));
            }

            public override Task WriteLineAsync(string value)
            {
                return Task.WhenAll(Writers.Select(w => w.WriteLineAsync(value)));
            }

            public override object InitializeLifetimeService()
            {
                throw new NotImplementedException();
            }

        }

        static void Main(string[] args)
        {
            /*if (args.Length == 1)
            {
                var defaultOut = Console.Out;

                var fileStream = File.Create(args[0], 1024, FileOptions.WriteThrough);
                var fileWriter = new StreamWriter(fileStream);
                fileWriter.AutoFlush = true;

                Console.SetOut(new ForkedWriter(defaultOut, fileWriter));
            }

            var best = FindFastOrder();

            Console.WriteLine();
            Console.WriteLine("**" + string.Join(", ", best) + "**");

            Console.ReadKey();*/

            for (var i = 0; i < 100000; i++)
            {
                Console.WriteLine(Jil.JSON.DeserializeDynamic("1234"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("-1.234"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("\"hello world\""));
                Console.WriteLine((string)Jil.JSON.DeserializeDynamic("null"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("true"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("false"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("[]"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("[1,2,3,4,5]"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("[1.2, -3.4, 4.5]"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("[\"hello\", \"world\", \"foo\", \"bar\"]"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("[null, true, false]"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": 123, \"world\":456}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": -1.234, \"world\": 4.567}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": \"foo\", \"world\": \"bar\"}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": null, \"world\": null}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": true, \"world\": false}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": [], \"world\": [\"hello\", \"world\", \"foo\", \"bar\"]}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": [1,2,3,4,5], \"world\": [1.2, -3.4, 4.5]}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": [null, true, false], \"world\": {}}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("{\"hello\": {\"hello\": 123, \"world\":456}, \"world\": {\"hello\": \"foo\", \"world\": \"bar\"}}"));
                Console.WriteLine(Jil.JSON.DeserializeDynamic("[{\"hello\": 123, \"world\":456}, {\"hello\": -1.234, \"world\": 4.567}, {\"hello\": \"foo\", \"world\": \"bar\"}]"));
            }
        }
    }
}
