﻿using System;
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
        static Random Rand;

        static void ResetRand()
        {
            Rand = new Random(314159265);
        }

        static List<Type> GetModels()
        {
            var ret =
                Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.Namespace == "Benchmark.Models" && !t.IsEnum && !t.IsInterface)
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

            var ret = asList.RandomValue(Rand);

            // top level can't be null
            if (ret == null)
            {
                return MakeListObject(t);
            }

            return ret;
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

        static MethodInfo _CheckEqualityDictionary = typeof(Program).GetMethod("CheckEqualityDictionary", BindingFlags.Static | BindingFlags.NonPublic);
        static void CheckEqualityDictionary<T>(Dictionary<string, T> a, Dictionary<string, T> b)
            where T : class, IGenericEquality<T>
        {
            if (!a.TrueEqualsDictionary(b))
            {
                throw new Exception("Jil produced JSON couldn't be deserialized");
            }
        }

        static MethodInfo _CheckEqualityList = typeof(Program).GetMethod("CheckEqualityList", BindingFlags.Static | BindingFlags.NonPublic);
        static void CheckEqualityList<T>(List<T> a, List<T> b)
            where T : class, IGenericEquality<T>
        {
            if (!a.TrueEqualsList(b))
            {
                throw new Exception("Jil produced JSON couldn't be deserialized");
            }
        }

        static MethodInfo _CheckEquality = typeof(Program).GetMethod("CheckEquality", BindingFlags.Static | BindingFlags.NonPublic);
        static void CheckEquality<T>(T a, T b)
            where T : class, IGenericEquality<T>
        {
            if (!a.TrueEquals(b))
            {
                throw new Exception("Jil produced JSON couldn't be deserialized");
            }
        }

        static MethodInfo _DoSpeedTest = typeof(Program).GetMethod("DoSpeedTest", BindingFlags.Static | BindingFlags.NonPublic);
        static List<Result> DoSpeedTest<T>(string serializerName, string niceTypeName, Func<T, string> serializeFunc, T obj)
            where T : class
        {
            const int TestRuns = 100;

            string data = null;

            var testGroup = new TestGroup(niceTypeName + " - " + serializerName);

            Console.Write("\t" + serializerName + "... ");

            var result =
                testGroup
                    .Plan("Serialization", () => data = serializeFunc(obj), TestRuns)
                    .GetResult();

            Console.WriteLine(result.Outcomes.Select(s => s.Elapsed.TotalMilliseconds).Average() + "ms");

#if DEBUG
            if (serializerName == "Jil")
            {
                var equalCheckable = obj is IGenericEquality<T>;
                if (equalCheckable)
                {
                    var copy = JsonConvert.DeserializeObject<T>(data);
                    var jilCopy = JilDeserialize<T>(data);

                    _CheckEquality.MakeGenericMethod(typeof(T)).Invoke(null, new object[] { obj, copy });
                    _CheckEquality.MakeGenericMethod(typeof(T)).Invoke(null, new object[] { obj, jilCopy });
                }
                else
                {
                    var equalCheckableList = typeof(T).IsList();
                    if (equalCheckableList)
                    {
                        var copy = JsonConvert.DeserializeObject<T>(data);
                        var jilCopy = JilDeserialize<T>(data);

                        var checkMethod = _CheckEqualityList.MakeGenericMethod(typeof(T).GetListInterface().GetGenericArguments()[0]);
                        checkMethod.Invoke(null, new object[] { obj, copy });
                        checkMethod.Invoke(null, new object[] { obj, jilCopy });
                    }
                    else
                    {

                        var equalCheckableDict = typeof(T).IsDictionary();
                        if (equalCheckableDict)
                        {
                            var copy = JsonConvert.DeserializeObject<T>(data);
                            var jilCopy = JilDeserialize<T>(data);

                            var checkMethod = _CheckEqualityDictionary.MakeGenericMethod(typeof(T).GetDictionaryInterface().GetGenericArguments()[1]);
                            checkMethod.Invoke(null, new object[] { obj, copy });
                            checkMethod.Invoke(null, new object[] { obj, jilCopy });
                        }
                        else
                        {
                            throw new Exception("Couldn't correctness-check Jil's serialization of a type: " + typeof(T));
                        }
                    }
                }


            }
#endif

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
                JSON.Serialize<T>(obj, str, Options.ISO8601);

                return str.ToString();
            }
        }

        static T JilDeserialize<T>(string data)
        {
            using (var str = new StringReader(data))
            {
                return JSON.Deserialize<T>(str, Options.ISO8601);
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

        static MethodInfo _ProtobufSerialize = typeof(Program).GetMethod("ProtobufSerialize", BindingFlags.NonPublic | BindingFlags.Static);
        static string ProtobufSerialize<T>(T obj)
        {
            using (var mem = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(mem, obj);

                return "";
            }
        }

        static object GetProtobufSerializer(Type forType)
        {
            var mtd = _ProtobufSerialize.MakeGenericMethod(forType);

            var funcType = typeof(Func<,>).MakeGenericType(forType, typeof(string));
            var ret = Delegate.CreateDelegate(funcType, mtd);

            ret.DynamicInvoke(new object[] { null });

            return ret;
        }

        static int[][] Permutations = 
            new int[][] 
            {
                new [] {0, 1, 2, 3},
                new [] {0, 1, 3, 2},
                new [] {0, 2, 1, 3},
                new [] {0, 2, 3, 1},
                new [] {0, 3, 1, 2},
                new [] {0, 3, 2, 1},
                
                new [] {1, 0, 2, 3},
                new [] {1, 0, 3, 2},
                new [] {1, 2, 0, 3},
                new [] {1, 2, 3, 0},
                new [] {1, 3, 0, 2},
                new [] {1, 3, 2, 0},

                new [] {2, 0, 1, 3},
                new [] {2, 0, 3, 1},
                new [] {2, 1, 0, 3},
                new [] {2, 1, 3, 0},
                new [] {2, 3, 0, 1},
                new [] {2, 3, 1, 0},

                new [] {3, 0, 1, 2},
                new [] {3, 0, 2, 1},
                new [] {3, 1, 0, 2},
                new [] {3, 1, 2, 0},
                new [] {3, 2, 0, 1},
                new [] {3, 2, 1, 0}
            };

        [Flags]
        enum SpeedTestMode
        {
            Single = 1,
            List = 2,
            Dictionary = 4,
            All = Single | List | Dictionary
        }

        static List<Result> DoSpeedTestsFor(Type model, SpeedTestMode mode = SpeedTestMode.All)
        {
            var ret = new List<Result>();

            // single objects
            if(mode.HasFlag(SpeedTestMode.Single))
            {
                var typeName = model.Name;

                var serialize = _DoSpeedTest.MakeGenericMethod(model);

                var newtonsoftSerializer = GetNewtonsoftSerializer(model);
                var serviceStackSerializer = GetServiceStackSerializer(model);
                var jilSerializer = GetJilSerializer(model);
                var protoSerializer = GetProtobufSerializer(model);

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
                            case 3: name = "Protobuf-net"; serializer = protoSerializer; break;
                            default: throw new InvalidOperationException();
                        }

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, singleObj });

                        ret.AddRange(results);
                    }
                }
            }

            // lists
            if (mode.HasFlag(SpeedTestMode.List))
            {
                var typeName = "List<" + model.Name + ">";

                var asList = typeof(List<>).MakeGenericType(model);

                var serialize = _DoSpeedTest.MakeGenericMethod(asList);

                var newtonsoftSerializer = GetNewtonsoftSerializer(asList);
                var serviceStackSerializer = GetServiceStackSerializer(asList);
                var jilSerializer = GetJilSerializer(asList);
                var protoSerializer = GetProtobufSerializer(asList);

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
                            case 3: name = "Protobuf-net"; serializer = protoSerializer; break;
                            default: throw new InvalidOperationException();
                        }

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, listObj });

                        ret.AddRange(results);
                    }
                }
            }

            // dictionaries
            if (mode.HasFlag(SpeedTestMode.Dictionary))
            {
                var typeName = "Dictionary<string, " + model.Name + ">";

                var asDict = typeof(Dictionary<,>).MakeGenericType(typeof(string), model);

                var serialize = _DoSpeedTest.MakeGenericMethod(asDict);

                var newtonsoftSerializer = GetNewtonsoftSerializer(asDict);
                var serviceStackSerializer = GetServiceStackSerializer(asDict);
                var jilSerializer = GetJilSerializer(asDict);
                var protoSerializer = GetProtobufSerializer(asDict);

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
                            case 3: name = "Protobuf-net"; serializer = protoSerializer; break;
                            default: throw new InvalidOperationException();
                        }

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, dictObj });

                        ret.AddRange(results);
                    }
                }
            }

            return ret;
        }

        static void Report(List<Result> results, out List<string> jilMinFailures, out List<string> jilMedFailures, out List<string> jilMinBeatPB, out int distinctTypeCount)
        {
            jilMinFailures = new List<string>();
            jilMedFailures = new List<string>();
            jilMinBeatPB = new List<string>();

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
            distinctTypeCount = 0;

            foreach (var type in allTypes)
            {
                var newtonsoft = stats.Single(s => s.TypeName == type && s.Serializer == "Json.NET");
                var serviceStack = stats.Single(s => s.TypeName == type && s.Serializer == "ServiceStack.Text");
                var jil = stats.Single(s => s.TypeName == type && s.Serializer == "Jil");
                var proto = stats.Single(s => s.TypeName == type && s.Serializer == "Protobuf-net");

                if (jil.Min == 0 || jil.Median == 0 || jil.Average == 0 || jil.Max == 0)
                {
                    Console.WriteLine(type);
                    Console.WriteLine("\t***INCONCLUSIVE, Jil elapsed time was 0ms***");
                    continue;
                }

                // don't count types if they were inclusive
                distinctTypeCount++;

                if (!(jil.Min <= newtonsoft.Min && jil.Min <= serviceStack.Min))
                {
                    jilMinFailures.Add(type);
                }

                if (!(jil.Median <= newtonsoft.Median && jil.Median <= serviceStack.Median))
                {
                    jilMedFailures.Add(type);
                }

                if (jil.Min < proto.Min)
                {
                    jilMinBeatPB.Add(type);
                }

                Action<string, Func<dynamic, double>> print =
                    (name, getter) =>
                    {
                        Console.Write("\t" + name + ": ");
                        var nD = getter(newtonsoft);
                        var sD = getter(serviceStack);
                        var jD = getter(jil);

                        var pD = getter(proto);

                        var jilVsProtobuf = jD / pD;

                        string tail;
                        if (jD < pD)
                        {
                            tail = (pD / jD) + "x faster than Protobuf-net]";
                        }
                        else
                        {
                            tail = (jD / pD) + "x slower than Protobuf-net]";
                        }

                        if (jD <= nD && jD <= sD)
                        {
                            var nextBest = Math.Min(nD, sD);

                            var timesFaster = nextBest / jD;

                            Console.WriteLine("Jil @" + jD + "ms [" + timesFaster + "x faster than next best; "+tail);
                            return;
                        }

                        if (sD <= nD && sD <= jD)
                        {
                            Console.WriteLine("ServiceStack.Text @" + sD + "ms [vs Jil @" + jD + "ms; " + tail);
                            return;
                        }

                        if (nD <= sD && nD <= jD)
                        {
                            Console.WriteLine("Newtonsoft @" + nD + "ms [vs Jil @" + jD + "ms; " + tail);
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

        static void QuickGraph(List<Result> results)
        {
            Console.WriteLine();

            var typeName = results.Select(r => r.TypeName).Distinct().Single();

            Console.WriteLine(typeName + ":");

            var medianTimes = results.GroupBy(g => g.Serializer).ToDictionary(g => g.Key, g => g.Select(r => r.Ellapsed.TotalMilliseconds).Median());

            foreach (var kv in medianTimes.OrderBy(r => r.Key))
            {
                Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
            }
        }

        static void DoQuickGraph()
        {
            const int runCount = 10;

            List<Result> question, answer, user, questionList, answerList, userList, questionDict, answerDict, userDict;

            Console.WriteLine("Running...");
            var oldOut = Console.Out;
            using (var ignored = new StringWriter())
            {
                Console.SetOut(ignored);

                ResetRand();
                question = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.Question), SpeedTestMode.Single)).ToList();

                ResetRand();
                answer = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.Answer), SpeedTestMode.Single)).ToList();

                ResetRand();
                user = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.User), SpeedTestMode.Single)).ToList();

                ResetRand();
                questionList = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.Question), SpeedTestMode.List)).ToList();

                ResetRand();
                answerList = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.Answer), SpeedTestMode.List)).ToList();

                ResetRand();
                userList = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.User), SpeedTestMode.List)).ToList();

                ResetRand();
                questionDict = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.Question), SpeedTestMode.Dictionary)).ToList();

                ResetRand();
                answerDict = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.Answer), SpeedTestMode.Dictionary)).ToList();

                ResetRand();
                userDict = Enumerable.Range(0, runCount).SelectMany(_ => DoSpeedTestsFor(typeof(Benchmark.Models.User), SpeedTestMode.Dictionary)).ToList();
            }
            Console.SetOut(oldOut);

            Func<List<Result>, double> jil = r => r.Where(w => w.Serializer == "Jil").Select(x => x.Ellapsed.TotalMilliseconds).Median();
            Func<List<Result>, double> jsonNet = r => r.Where(w => w.Serializer == "Json.NET").Select(x => x.Ellapsed.TotalMilliseconds).Median();
            Func<List<Result>, double> protobufNet = r => r.Where(w => w.Serializer == "Protobuf-net").Select(x => x.Ellapsed.TotalMilliseconds).Median();
            Func<List<Result>, double> serviceStackText = r => r.Where(w => w.Serializer == "ServiceStack.Text").Select(x => x.Ellapsed.TotalMilliseconds).Median();

            Console.WriteLine("Single:");
            Console.WriteLine("Type\tJil MS\tJil\tJson.NET MS\tJson.NET\tprotobuf-net MS\tprotobuf-net\tServiceStack.Text MS\tServiceStack.Text");
            Console.WriteLine("User\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(user), jsonNet(user), protobufNet(user), serviceStackText(user));
            Console.WriteLine("Answer\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(answer), jsonNet(answer), protobufNet(answer), serviceStackText(answer));
            Console.WriteLine("Question\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(question), jsonNet(question), protobufNet(question), serviceStackText(question));

            Console.WriteLine();

            Console.WriteLine("Lists:");
            Console.WriteLine("Type\tJil MS\tJil\tJson.NET MS\tJson.NET\tprotobuf-net MS\tprotobuf-net\tServiceStack.Text MS\tServiceStack.Text");
            Console.WriteLine("User\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(userList), jsonNet(userList), protobufNet(userList), serviceStackText(userList));
            Console.WriteLine("Answer\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(answerList), jsonNet(answerList), protobufNet(answerList), serviceStackText(answerList));
            Console.WriteLine("Question\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(questionList), jsonNet(questionList), protobufNet(questionList), serviceStackText(questionList));

            Console.WriteLine();

            Console.WriteLine("Dictionaries of strings to:");
            Console.WriteLine("Type\tJil MS\tJil\tJson.NET MS\tJson.NET\tprotobuf-net MS\tprotobuf-net\tServiceStack.Text MS\tServiceStack.Text");
            Console.WriteLine("User\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(userDict), jsonNet(userDict), protobufNet(userDict), serviceStackText(userDict));
            Console.WriteLine("Answer\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(answerDict), jsonNet(answerDict), protobufNet(answerDict), serviceStackText(answerDict));
            Console.WriteLine("Question\t{0}\t\t{1}\t\t{2}\t\t{3}", jil(questionDict), jsonNet(questionDict), protobufNet(questionDict), serviceStackText(questionDict));

            /*QuickGraph(question);
            QuickGraph(answer);
            QuickGraph(user);
            QuickGraph(questionList);
            QuickGraph(answerList);
            QuickGraph(userList);
            QuickGraph(questionDict);
            QuickGraph(answerDict);
            QuickGraph(userDict);*/
        }

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                ResetRand();

                var models = GetModels();

                var results = new List<Result>();

                foreach (var model in models)
                {
                    Console.WriteLine("* " + model.Name);
                    results.AddRange(DoSpeedTestsFor(model));
                }

                Console.WriteLine();
                Console.WriteLine();

                var oldOut = Console.Out;
                Console.SetOut(new StreamWriter(File.Create(args[0])));

                List<string> minFailures, medianFailures, beatProtobuf;
                int typeCount;
                Report(results, out minFailures, out medianFailures, out beatProtobuf, out typeCount);

                Console.WriteLine();

                Console.WriteLine("Jil wasn't the absolute fastest {0} times (out of {1} total types considered)", minFailures.Count, typeCount);
                minFailures.OrderBy(_ => _).ForEach(f => Console.WriteLine("\t" + f));
                Console.WriteLine();

                Console.WriteLine("Jil wasn't the median fastest {0} times (out of {1} total types considered)", medianFailures.Count, typeCount);
                medianFailures.OrderBy(_ => _).ForEach(f => Console.WriteLine("\t" + f));
                Console.WriteLine();

                Console.WriteLine("Jil beat protobuf-net (somehow) {0} times (out of {1} total types considered)", beatProtobuf.Count, typeCount);
                beatProtobuf.OrderBy(_ => _).ForEach(f => Console.WriteLine("\t" + f));

                Console.Out.Flush();
                Console.Out.Close();
                Console.Out.Dispose();
                Console.SetOut(oldOut);
            }
            else
            {
                Console.WriteLine("== Quick Graph " + DateTime.UtcNow + " ==");
                DoQuickGraph();
            }

            Console.WriteLine("== Finished ==");

            Console.ReadKey();
        }
    }
}
