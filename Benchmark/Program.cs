using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleSpeedTester.Core;
using System.Reflection;
using Newtonsoft.Json;
using Jil;
using System.IO;

namespace Benchmark
{
    class Program
    {
        // "Nothing up my sleeves" number, first 9 digits of PI
        static Random Rand = new Random(314159265);

        static List<Type> GetModels()
        {
            var ret =
                Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.Namespace == "Benchmark.Models" && !t.IsEnum)
                    .ToList();

            return ret;
        }

        static object MakeSingleObject(Type t)
        {
            var ret = Activator.CreateInstance(t);
            foreach (var p in t.GetProperties())
            {
                var propType = p.PropertyType;
                var val = propType.RandomValue(Rand);

                p.SetValue(ret, val);
            }

            return ret;
        }

        static object MakeListObject(Type t)
        {
            var asList = typeof(List<>).MakeGenericType(t);

            return asList.RandomValue(Rand);
        }

        static object MakeDictionaryObject(Type t)
        {
            var asDictionary = typeof(Dictionary<,>).MakeGenericType(typeof(string), t);
            var ret = Activator.CreateInstance(asDictionary);
            var add = asDictionary.GetMethod("Add");

            var len = Rand.Next(30) + 20;
            for (var i = 0; i < len; i++)
            {
                var key = (string)typeof(string).RandomValue(Rand);
                if (key == null)
                {
                    i--;
                    continue;
                }

                var val = t.RandomValue(Rand);

                add.Invoke(ret, new object[] { key, val });
            }

            return ret;
        }

        static MethodInfo _DoSpeedTest = typeof(Program).GetMethod("DoSpeedTest", BindingFlags.Static | BindingFlags.NonPublic);
        static List<Result> DoSpeedTest<T>(string serializerName, string niceTypeName, Func<T, string> serializeFunc, T obj)
        {
            const int TestRuns = 5;

            string data = null;

            var testGroup = new TestGroup(niceTypeName + " - " + serializerName);

            Console.Write("\t" + serializerName + "... ");

            var result =
                testGroup
                    .Plan("Serialization", () => data = serializeFunc(obj), TestRuns)
                    .GetResult();

            Console.WriteLine(result.Outcomes.Select(s => s.Elapsed.TotalMilliseconds).Average() + "ms");

            return
                result.Outcomes.Select(
                    o =>
                        new Result
                        {
                            Serializer = serializerName,
                            TypeName = niceTypeName,
                            Ellapsed = o.Elapsed
                        }
                ).ToList();
        }

        static MethodInfo _NewtonsoftSerialize = typeof(Program).GetMethod("NewtonsoftSerialize", BindingFlags.NonPublic | BindingFlags.Static);
        static string NewtonsoftSerialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        static object GetNewtonsoftSerializer(Type forType)
        {
            var mtd = _NewtonsoftSerialize.MakeGenericMethod(forType);

            var funcType = typeof(Func<,>).MakeGenericType(forType, typeof(string));
            var ret = Delegate.CreateDelegate(funcType, mtd);

            ret.DynamicInvoke(new object[] { null });

            return ret;
        }

        static MethodInfo _ServiceStackSerialize = typeof(Program).GetMethod("ServiceStackSerialize", BindingFlags.NonPublic | BindingFlags.Static);
        static string ServiceStackSerialize<T>(T obj)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString<T>(obj);
        }

        static object GetServiceStackSerializer(Type forType)
        {
            var mtd = _ServiceStackSerialize.MakeGenericMethod(forType);

            var funcType = typeof(Func<,>).MakeGenericType(forType, typeof(string));
            var ret = Delegate.CreateDelegate(funcType, mtd);

            ret.DynamicInvoke(new object[] { null });

            return ret;
        }

        static MethodInfo _JilSerialize = typeof(Program).GetMethod("JilSerialize", BindingFlags.NonPublic | BindingFlags.Static);
        static string JilSerialize<T>(T obj)
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize<T>(obj, str);

                return str.ToString();
            }
        }

        static object GetJilSerializer(Type forType)
        {
            var mtd = _JilSerialize.MakeGenericMethod(forType);

            var funcType = typeof(Func<,>).MakeGenericType(forType, typeof(string));
            var ret = Delegate.CreateDelegate(funcType, mtd);

            ret.DynamicInvoke(new object[] { null });

            return ret;
        }

        static int[][] Permutations = 
            new int[][] 
            {
                new [] {0, 1, 2},
                new [] {0, 2, 1},
                new [] {1, 0, 2},
                new [] {1, 2, 0},
                new [] {2, 0, 1},
                new [] {2, 1, 0}
            };

        static List<Result> DoSpeedTestsFor(Type model)
        {
            var ret = new List<Result>();

            // single objects
            {
                var typeName = model.Name;

                var serialize = _DoSpeedTest.MakeGenericMethod(model);

                var newtonsoftSerializer = GetNewtonsoftSerializer(model);
                var serviceStackSerializer = GetServiceStackSerializer(model);
                var jilSerializer = GetJilSerializer(model);

                System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);

                var singleObj = MakeSingleObject(model);

                foreach (var perm in Permutations.Random(Rand))
                {
                    foreach (var p in perm)
                    {
                        string name;
                        object serializer;

                        switch (p)
                        {
                            case 0: name = "Json.NET"; serializer = newtonsoftSerializer; break;
                            case 1: name = "ServiceStack.Text"; serializer = serviceStackSerializer; break;
                            case 2: name = "Jil"; serializer = jilSerializer; break;
                            default: throw new InvalidOperationException();
                        }

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, singleObj });

                        ret.AddRange(results);
                    }
                }
            }

            // lists
            {
                var typeName = "List<" + model.Name + ">";

                var asList = typeof(List<>).MakeGenericType(model);

                var serialize = _DoSpeedTest.MakeGenericMethod(asList);

                var newtonsoftSerializer = GetNewtonsoftSerializer(asList);
                var serviceStackSerializer = GetServiceStackSerializer(asList);
                var jilSerializer = GetJilSerializer(asList);

                System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);

                var listObj = MakeListObject(model);

                foreach (var perm in Permutations.Random(Rand))
                {
                    foreach (var p in perm)
                    {
                        string name;
                        object serializer;

                        switch (p)
                        {
                            case 0: name = "Json.NET"; serializer = newtonsoftSerializer; break;
                            case 1: name = "ServiceStack.Text"; serializer = serviceStackSerializer; break;
                            case 2: name = "Jil"; serializer = jilSerializer; break;
                            default: throw new InvalidOperationException();
                        }

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, listObj });

                        ret.AddRange(results);
                    }
                }
            }

            // dictionaries
            {
                var typeName = "Dictionary<string, " + model.Name + ">";

                var asDict = typeof(Dictionary<,>).MakeGenericType(typeof(string), model);

                var serialize = _DoSpeedTest.MakeGenericMethod(asDict);

                var newtonsoftSerializer = GetNewtonsoftSerializer(asDict);
                var serviceStackSerializer = GetServiceStackSerializer(asDict);
                var jilSerializer = GetJilSerializer(asDict);

                System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);

                var dictObj = MakeDictionaryObject(model);

                foreach (var perm in Permutations.Random(Rand))
                {
                    foreach (var p in perm)
                    {
                        string name;
                        object serializer;

                        switch (p)
                        {
                            case 0: name = "Json.NET"; serializer = newtonsoftSerializer; break;
                            case 1: name = "ServiceStack.Text"; serializer = serviceStackSerializer; break;
                            case 2: name = "Jil"; serializer = jilSerializer; break;
                            default: throw new InvalidOperationException();
                        }

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, dictObj });

                        ret.AddRange(results);
                    }
                }
            }

            return ret;
        }

        static void Report(List<Result> results)
        {
            // remove outliers (high and low)
            var woOutliers =
                results
                    .GroupBy(g => new { g.TypeName, g.Serializer })
                    .SelectMany(
                        g =>
                        {
                            var inOrder = g.OrderBy(_ => _.Ellapsed).ToList();

                            var notSkipped = inOrder.Skip(1).Take(inOrder.Count - 2);

                            return notSkipped;
                        }
                    ).ToList();

            // min / max / average / median
            var stats =
                woOutliers
                    .GroupBy(g => new { g.TypeName, g.Serializer })
                    .Select(
                        g =>
                        {
                            var min = g.Select(_ => _.Ellapsed.TotalMilliseconds).Min();
                            var max = g.Select(_ => _.Ellapsed.TotalMilliseconds).Max();
                            var avg = g.Select(_ => _.Ellapsed.TotalMilliseconds).Average();
                            var med = g.Select(_ => _.Ellapsed.TotalMilliseconds).Median();

                            return
                                new
                                {
                                    g.Key.TypeName,
                                    g.Key.Serializer,
                                    Min = min,
                                    Max = max,
                                    Average = avg,
                                    Median = med
                                };
                        }
                    ).ToList();

            var allTypes = stats.Select(s => s.TypeName).Distinct().OrderBy(o => o).ToList();

            foreach (var type in allTypes)
            {
                var newtonsoft = stats.Single(s => s.TypeName == type && s.Serializer == "Json.NET");
                var serviceStack = stats.Single(s => s.TypeName == type && s.Serializer == "ServiceStack.Text");
                var jil = stats.Single(s => s.TypeName == type && s.Serializer == "Jil");

                Action<string, Func<dynamic, double>> print =
                    (name, getter) =>
                    {
                        Console.Write("\t" + name + ": ");
                        var nD = getter(newtonsoft);
                        var sD = getter(serviceStack);
                        var jD = getter(jil);

                        if (nD < sD && nD < jD)
                        {
                            Console.WriteLine("Newtonsoft @" + nD + "ms [vs Jil @" + jD + "ms");
                            return;
                        }

                        if (sD < nD && sD < jD)
                        {
                            Console.WriteLine("ServiceStack.Text @" + sD + "ms [vs Jil @" + jD + "ms");
                            return;
                        }

                        if (jD < nD && jD < sD)
                        {
                            Console.WriteLine("Jil @" + jD + "ms");
                            return;
                        }
                    };

                Console.WriteLine(type);
                print("Min", d => d.Min);
                print("Max", d => d.Max);
                print("Avg", d => d.Average);
                print("Med", d => d.Median);
            }
        }

        static void Main(string[] args)
        {
            var models = GetModels();

            var results = new List<Result>();

            foreach (var model in models)
            {
                Console.WriteLine("* " + model.Name);
                results.AddRange(DoSpeedTestsFor(model));
            }

            Console.WriteLine();
            Console.WriteLine();

            Report(results);

            Console.ReadKey();
        }
    }
}
