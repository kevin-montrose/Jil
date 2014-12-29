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
using Benchmark.Models;
using System.Diagnostics;

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
                    .Where(t => t.Namespace == "Benchmark.Models" && !t.IsEnum && !t.IsInterface && !t.IsAbstract)
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
        static List<Result> DoSpeedTest<T, V>(string serializerName, string niceTypeName, Func<T, V> serializeFunc, Func<V, T, dynamic> deserializeFunc, T obj)
            where T : class
            where V : class
        {
            const int TestRuns = 100;

            V data = null;

            var testGroup = new TestGroup(niceTypeName + " - " + serializerName);

            Console.WriteLine(serializerName);

            var serializeResult =
                testGroup
                    .Plan("Serialization", () => data = serializeFunc(obj), TestRuns)
                    .GetResult();

            if (serializeResult.Outcomes.Any(o => o.Exception != null))
            {
                throw new Exception("Serialization failed w/ " + serializerName);
            }

            Console.WriteLine("\t" + serializeResult.Outcomes.Select(s => s.Elapsed.TotalMilliseconds).Average() + "ms");

            var deserializeResult =
                testGroup
                    .Plan("Deserialization", () => deserializeFunc(data, obj), TestRuns)
                    .GetResult();

            if (deserializeResult.Outcomes.Any(o => o.Exception != null))
            {
                throw new Exception("Deserialization failed w/ " + serializerName);
            }

            Console.WriteLine("\t" + deserializeResult.Outcomes.Select(s => s.Elapsed.TotalMilliseconds).Average() + "ms");

            return
                serializeResult.Outcomes.Select(
                    o =>
                        new Result
                        {
                            Serializer = serializerName + " (Serialize)",
                            TypeName = niceTypeName,
                            Ellapsed = o.Elapsed
                        }
                ).Concat(
                    deserializeResult.Outcomes.Select(
                        o =>
                            new Result
                            {
                                Serializer = serializerName + " (Deserialize)",
                                TypeName = niceTypeName,
                                Ellapsed = o.Elapsed
                            }
                    )
                ).ToList();
        }

        static byte[] ProtobufSerialize<T>(T obj)
        {
            using (var mem = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(mem, obj);
                return mem.ToArray();
            }
        }

        static T ProtobufDeserialize<T>(byte[] bytes)
        {
            using (var mem = new MemoryStream(bytes))
            {
                return ProtoBuf.Serializer.Deserialize<T>(mem);
            }
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

        static MethodInfo _NewtonsoftDeserializeDynamic = typeof(Program).GetMethod("NewtonsoftDeserializeDynamic", BindingFlags.NonPublic | BindingFlags.Static);
        static dynamic NewtonsoftDeserializeDynamic<T>(string str, T shouldMatch)
            //where T : IGenericEquality<T>
        {
            dynamic ret = JsonConvert.DeserializeObject(str);
            //if ((ret == null && shouldMatch == null) || shouldMatch.Equals(ret))
            //{
                return ret;
            //}

            //throw new Exception("Deserialization failed");
        }

        static MethodInfo _NewtonsoftDeserialize = typeof(Program).GetMethod("NewtonsoftDeserialize", BindingFlags.NonPublic | BindingFlags.Static);
        static T NewtonsoftDeserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        static object GetNewtonsoftDeserializer(Type forType, string defaultVal)
        {
            var mtd = _NewtonsoftDeserializeDynamic.MakeGenericMethod(forType);
            var funcType = typeof(Func<,,>).MakeGenericType(typeof(string), forType, typeof(object));
            var ret = Delegate.CreateDelegate(funcType, mtd);

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

        static MethodInfo _ServiceStackDeserializeDynamic = typeof(Program).GetMethod("ServiceStackDeserializeDynamic", BindingFlags.NonPublic | BindingFlags.Static);
        static dynamic ServiceStackDeserializeDynamic<T>(string str, T shouldMatch)
            where T : IGenericEquality<T>
        {
            dynamic ret = ServiceStack.Text.JsonObject.Parse(str);
            if ((ret == null && shouldMatch == null) || shouldMatch.Equals(ret))
            {
                return ret;
            }

            throw new Exception("Deserialization failed");
        }

        static MethodInfo _ServiceStackDeserialize = typeof(Program).GetMethod("ServiceStackDeserialize", BindingFlags.NonPublic | BindingFlags.Static);
        static T ServiceStackDeserialize<T>(string str)
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(str);
        }

        static object GetServiceStackDeserializer(Type forType, string defaultVal)
        {
            var mtd = _ServiceStackDeserializeDynamic.MakeGenericMethod(forType);
            var funcType = typeof(Func<,,>).MakeGenericType(typeof(string), forType, typeof(object));
            var ret = Delegate.CreateDelegate(funcType, mtd);

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

        static object GetJilSerializer(Type forType)
        {
            var mtd = _JilSerialize.MakeGenericMethod(forType);

            var funcType = typeof(Func<,>).MakeGenericType(forType, typeof(string));
            var ret = Delegate.CreateDelegate(funcType, mtd);

            ret.DynamicInvoke(new object[] { null });

            return ret;
        }

        static MethodInfo _JilDeserializeDynamic = typeof(Program).GetMethod("JilDeserializeDynamic", BindingFlags.NonPublic | BindingFlags.Static);
        static dynamic JilDeserializeDynamic<T>(string data, T shouldMatch)
            //where T : IGenericEquality<T>
        {
            using (var str = new StringReader(data))
            {
                var ret = JSON.DeserializeDynamic(str, Options.ISO8601);

#if DEBUG
                {
                    var asComparable1 = shouldMatch as IGenericEquality<T>;

                    if (asComparable1 != null)
                    {
                        if (!asComparable1.EqualsDynamic(ret))
                        {
                            throw new Exception("Results didn't match");
                        }
                    }
                }

                {
                    if (shouldMatch is System.Collections.IList)
                    {
                        var e1 = (System.Collections.IEnumerator)((dynamic)shouldMatch).GetEnumerator();
                        var e2 = (System.Collections.IEnumerator)ret.GetEnumerator();

                        while (e1.MoveNext())
                        {
                            if (!e2.MoveNext())
                            {
                                throw new Exception("Results didn't match");
                            }

                            var i1 = (dynamic)e1.Current;
                            var i2 = (dynamic)e2.Current;

                            if (!i1.EqualsDynamic(i2))
                            {
                                throw new Exception("Results didn't match");
                            }
                        }
                    }
                }

                {
                    if (shouldMatch is System.Collections.IDictionary)
                    {
                        foreach (var kv in ((dynamic)shouldMatch))
                        {
                            var key = (string)kv.Key;
                            var v1 = kv.Value;
                            var v2 = ret[key];

                            if (!v1.EqualsDynamic(v2))
                            {
                                throw new Exception("Results didn't match");
                            }
                        }
                    }
                }
#endif

                //if ((ret == null && shouldMatch == null) || shouldMatch.EqualsDynamic(ret))
                //{
                return ret;
                //}

                //throw new Exception("Deserialization failed");
            }
        }

        static MethodInfo _JilDeserialize = typeof(Program).GetMethod("JilDeserialize", BindingFlags.NonPublic | BindingFlags.Static);
        static T JilDeserialize<T>(string data, T shouldMatch)
        {
            using (var str = new StringReader(data))
            {
                return JSON.Deserialize<T>(str, Options.ISO8601);
            }
        }

        static object GetJilStaticDeserializer(Type forType, string defaultVal)
        {
            var mtd = _JilDeserialize.MakeGenericMethod(forType);
            var funcType = typeof(Func<,,>).MakeGenericType(typeof(string), forType, forType);
            var ret = Delegate.CreateDelegate(funcType, mtd);

            return ret;
        }

        static object GetJilDynamicDeserializer(Type forType, string defaultVal)
        {
            var mtd = _JilDeserializeDynamic.MakeGenericMethod(forType);
            var funcType = typeof(Func<,,>).MakeGenericType(typeof(string), forType, typeof(object));
            var ret = Delegate.CreateDelegate(funcType, mtd);

            return ret;
        }

        static int[][] Permutations = 
            new int[][] 
            {
                new [] {0, 1},
                new [] {1, 0}
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

                var jilSerializer = GetJilSerializer(model);

                var jilStaticDeserializer = GetJilStaticDeserializer(model, "{}");
                var jilDynamicDeserializer = GetJilDynamicDeserializer(model, "{}");

                System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);

                var singleObj = MakeSingleObject(model);

                foreach (var perm in Permutations.Random(Rand))
                {
                    foreach (var p in perm)
                    {
                        string name;
                        object serializer;
                        object deserializer;
                        Type resultType;

                        switch (p)
                        {
                            case 0:name = "Jil Static"; serializer = jilSerializer; deserializer = jilStaticDeserializer; resultType = typeof(string); break;
                            case 1: name = "Jil Dynamic"; serializer = jilSerializer; deserializer = jilDynamicDeserializer; resultType = typeof(string); break;
                            default: throw new InvalidOperationException();
                        }

                        var serialize = _DoSpeedTest.MakeGenericMethod(model, resultType);

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, deserializer, singleObj });

                        ret.AddRange(results);
                    }
                }
            }

            // lists
            if (mode.HasFlag(SpeedTestMode.List))
            {
                var typeName = "List<" + model.Name + ">";

                var asList = typeof(List<>).MakeGenericType(model);

                var jilSerializer = GetJilSerializer(asList);

                var jilStaticDeserializer = GetJilStaticDeserializer(asList, "[]");
                var jilDynamicDeserializer = GetJilDynamicDeserializer(asList, "[]");

                System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);

                var listObj = MakeListObject(model);

                foreach (var perm in Permutations.Random(Rand))
                {
                    foreach (var p in perm)
                    {
                        string name;
                        object serializer;
                        object deserializer;
                        Type resultType;

                        switch (p)
                        {
                            case 0: name = "Jil Static"; serializer = jilSerializer; deserializer = jilStaticDeserializer; resultType = typeof(string); break;
                            case 1: name = "Jil Dynamic"; serializer = jilSerializer; deserializer = jilDynamicDeserializer; resultType = typeof(string); break;
                            default: throw new InvalidOperationException();
                        }

                        var serialize = _DoSpeedTest.MakeGenericMethod(asList, resultType);

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, deserializer, listObj });

                        ret.AddRange(results);
                    }
                }
            }

            // dictionaries
            if (mode.HasFlag(SpeedTestMode.Dictionary))
            {
                var typeName = "Dictionary<string, " + model.Name + ">";

                var asDict = typeof(Dictionary<,>).MakeGenericType(typeof(string), model);

                var jilSerializer = GetJilSerializer(asDict);

                var jilStaticDeserializer = GetJilStaticDeserializer(asDict, "{}");
                var jilDynamicDeserializer = GetJilDynamicDeserializer(asDict, "{}");

                System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);

                var dictObj = MakeDictionaryObject(model);

                foreach (var perm in Permutations.Random(Rand))
                {
                    foreach (var p in perm)
                    {
                        string name;
                        object serializer;
                        object deserializer;
                        Type resultType;

                        switch (p)
                        {
                            case 0: name = "Jil Static"; serializer = jilSerializer; deserializer = jilStaticDeserializer; resultType = typeof(string); break;
                            case 1: name = "Jil Dynamic"; serializer = jilSerializer; deserializer = jilDynamicDeserializer; resultType = typeof(string); break;
                            default: throw new InvalidOperationException();
                        }

                        var serialize = _DoSpeedTest.MakeGenericMethod(asDict, resultType);

                        var results = (List<Result>)serialize.Invoke(null, new object[] { name, typeName, serializer, deserializer, dictObj });

                        ret.AddRange(results);
                    }
                }
            }

            return ret;
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

            Func<List<Result>, Func<Result, bool>, double> jilStatic = (r, f) => r.Where(w => f(w) && w.Serializer.StartsWith("Jil Static")).Select(x => x.Ellapsed.TotalMilliseconds).Median();
            Func<List<Result>, Func<Result, bool>, double> jilDynamic = (r, f) => r.Where(w => f(w) && w.Serializer.StartsWith("Jil Dynamic")).Select(x => x.Ellapsed.TotalMilliseconds).Median();

            {
                Func<Result, bool> serialize = r => r.Serializer.Contains("(Serialize)");

                Console.WriteLine("Serializing");
                Console.WriteLine("===========");

                Console.WriteLine("Single:");
                Console.WriteLine("Type\tJil Static MS\tJil Static\tJil Dynamic MS\tJil Dynamic");
                Console.WriteLine("User\t{0}\t\t{1}", jilStatic(user, serialize), jilDynamic(user, serialize));
                Console.WriteLine("Answer\t{0}\t\t{1}", jilStatic(answer, serialize), jilDynamic(answer, serialize));
                Console.WriteLine("Question\t{0}\t\t{1}", jilStatic(question, serialize), jilDynamic(question, serialize));

                Console.WriteLine();

                Console.WriteLine("Lists:");
                Console.WriteLine("Type\tJil Static MS\tJil Static\tJil Dynamic MS\tJil Dynamic");
                Console.WriteLine("User\t{0}\t\t{1}", jilStatic(userList, serialize), jilDynamic(userList, serialize));
                Console.WriteLine("Answer\t{0}\t\t{1}", jilStatic(answerList, serialize), jilDynamic(answerList, serialize));
                Console.WriteLine("Question\t{0}\t\t{1}", jilStatic(questionList, serialize), jilDynamic(questionList, serialize));

                Console.WriteLine();

                Console.WriteLine("Dictionaries of strings to:");
                Console.WriteLine("Type\tJil Static MS\tJil Static\tJil Dynamic MS\tJil Dynamic");
                Console.WriteLine("User\t{0}\t\t{1}", jilStatic(userDict, serialize), jilDynamic(userDict, serialize));
                Console.WriteLine("Answer\t{0}\t\t{1}", jilStatic(answerDict, serialize), jilDynamic(answerDict, serialize));
                Console.WriteLine("Question\t{0}\t\t{1}", jilStatic(questionDict, serialize), jilDynamic(questionDict, serialize));
            }

            Console.WriteLine();

            {
                Func<Result, bool> deserialize = r => r.Serializer.Contains("(Deserialize)");

                Console.WriteLine("Deserializing");
                Console.WriteLine("=============");

                Console.WriteLine("Single:");
                Console.WriteLine("Type\tJil Static MS\tJil Static\tJil Dynamic MS\tJil Dynamic");
                Console.WriteLine("User\t{0}\t\t{1}", jilStatic(user, deserialize), jilDynamic(user, deserialize));
                Console.WriteLine("Answer\t{0}\t\t{1}", jilStatic(answer, deserialize), jilDynamic(answer, deserialize));
                Console.WriteLine("Question\t{0}\t\t{1}", jilStatic(question, deserialize), jilDynamic(question, deserialize));

                Console.WriteLine();

                Console.WriteLine("Lists:");
                Console.WriteLine("Type\tJil Static MS\tJil Static\tJil Dynamic MS\tJil Dynamic");
                Console.WriteLine("User\t{0}\t\t{1}", jilStatic(userList, deserialize), jilDynamic(userList, deserialize));
                Console.WriteLine("Answer\t{0}\t\t{1}", jilStatic(answerList, deserialize), jilDynamic(answerList, deserialize));
                Console.WriteLine("Question\t{0}\t\t{1}", jilStatic(questionList, deserialize), jilDynamic(questionList, deserialize));

                Console.WriteLine();

                Console.WriteLine("Dictionaries of strings to:");
                Console.WriteLine("Type\tJil Static MS\tJil Static\tJil Dynamic MS\tJil Dynamic");
                Console.WriteLine("User\t{0}\t\t{1}", jilStatic(userDict, deserialize), jilDynamic(userDict, deserialize));
                Console.WriteLine("Answer\t{0}\t\t{1}", jilStatic(answerDict, deserialize), jilDynamic(answerDict, deserialize));
                Console.WriteLine("Question\t{0}\t\t{1}", jilStatic(questionDict, deserialize), jilDynamic(questionDict, deserialize));
            }
        }

        const int JilIndex = 0;
        const int NewtonSoftIndex = 1;
        const int ProtobufIndex = 2;
        const int ServiceStackIndex = 3;

        static void DoComparisonGraph()
        {
            const int TestRuns = 10000;

            // serializers
            {
                Console.WriteLine("== Serializers == ");
                Console.WriteLine("\tJil\tNewtonSoft\tProtobuf\tServiceStack");

                // single
                {
                    ResetRand();
                    double[] answerSpeed = CompareSerializers((Answer)MakeSingleObject(typeof(Answer)), TestRuns);
                    Console.WriteLine("Answer\t" + answerSpeed[JilIndex] + "\t" + answerSpeed[NewtonSoftIndex] + "\t" + answerSpeed[ProtobufIndex] + "\t" + answerSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] questionSpeed = CompareSerializers((Question)MakeSingleObject(typeof(Question)), TestRuns);
                    Console.WriteLine("Question\t" + questionSpeed[JilIndex] + "\t" + questionSpeed[NewtonSoftIndex] + "\t" + questionSpeed[ProtobufIndex] + "\t" + questionSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] userSpeed = CompareSerializers((User)MakeSingleObject(typeof(User)), TestRuns);
                    Console.WriteLine("User\t" + userSpeed[JilIndex] + "\t" + userSpeed[NewtonSoftIndex] + "\t" + userSpeed[ProtobufIndex] + "\t" + userSpeed[ServiceStackIndex]);
                }

                Console.WriteLine();

                // list
                {
                    ResetRand();
                    double[] answerListSpeed = CompareSerializers((List<Answer>)MakeListObject(typeof(Answer)), TestRuns);
                    Console.WriteLine("List<Answer>\t" + answerListSpeed[JilIndex] + "\t" + answerListSpeed[NewtonSoftIndex] + "\t" + answerListSpeed[ProtobufIndex] + "\t" + answerListSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] questionListSpeed = CompareSerializers((List<Question>)MakeListObject(typeof(Question)), TestRuns);
                    Console.WriteLine("List<Question>\t" + questionListSpeed[JilIndex] + "\t" + questionListSpeed[NewtonSoftIndex] + "\t" + questionListSpeed[ProtobufIndex] + "\t" + questionListSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] userListSpeed = CompareSerializers((List<User>)MakeListObject(typeof(User)), TestRuns);
                    Console.WriteLine("List<User>\t" + userListSpeed[JilIndex] + "\t" + userListSpeed[NewtonSoftIndex] + "\t" + userListSpeed[ProtobufIndex] + "\t" + userListSpeed[ServiceStackIndex]);
                }

                Console.WriteLine();

                // dictionary
                {
                    ResetRand();
                    double[] answerDictSpeed = CompareSerializers((Dictionary<string, Answer>)MakeDictionaryObject(typeof(Answer)), TestRuns);
                    Console.WriteLine("Dictionary<string,Answer>\t" + answerDictSpeed[JilIndex] + "\t" + answerDictSpeed[NewtonSoftIndex] + "\t" + answerDictSpeed[ProtobufIndex] + "\t" + answerDictSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] questionDictSpeed = CompareSerializers((Dictionary<string, Question>)MakeDictionaryObject(typeof(Question)), TestRuns);
                    Console.WriteLine("Dictionary<string,Question>\t" + questionDictSpeed[JilIndex] + "\t" + questionDictSpeed[NewtonSoftIndex] + "\t" + questionDictSpeed[ProtobufIndex] + "\t" + questionDictSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] userDictSpeed = CompareSerializers((Dictionary<string, User>)MakeDictionaryObject(typeof(User)), TestRuns);
                    Console.WriteLine("Dictionary<string,User>\t" + userDictSpeed[JilIndex] + "\t" + userDictSpeed[NewtonSoftIndex] + "\t" + userDictSpeed[ProtobufIndex] + "\t" + userDictSpeed[ServiceStackIndex]);
                }
            }

            Console.WriteLine();

            // deserializers
            {
                Console.WriteLine("== Deserializers == ");
                Console.WriteLine("\tJil\tNewtonSoft\tProtobuf\tServiceStack");

                // single
                {
                    ResetRand();
                    double[] answerSpeed = CompareDeserializers((Answer)MakeSingleObject(typeof(Answer)), TestRuns);
                    Console.WriteLine("Answer\t" + answerSpeed[JilIndex] + "\t" + answerSpeed[NewtonSoftIndex] + "\t" + answerSpeed[ProtobufIndex] + "\t" + answerSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] questionSpeed = CompareDeserializers((Question)MakeSingleObject(typeof(Question)), TestRuns);
                    Console.WriteLine("Question\t" + questionSpeed[JilIndex] + "\t" + questionSpeed[NewtonSoftIndex] + "\t" + questionSpeed[ProtobufIndex] + "\t" + questionSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] userSpeed = CompareDeserializers((User)MakeSingleObject(typeof(User)), TestRuns);
                    Console.WriteLine("User\t" + userSpeed[JilIndex] + "\t" + userSpeed[NewtonSoftIndex] + "\t" + userSpeed[ProtobufIndex] + "\t" + userSpeed[ServiceStackIndex]);
                }

                Console.WriteLine();

                // list
                {
                    ResetRand();
                    double[] answerListSpeed = CompareDeserializers((List<Answer>)MakeListObject(typeof(Answer)), TestRuns);
                    Console.WriteLine("List<Answer>\t" + answerListSpeed[JilIndex] + "\t" + answerListSpeed[NewtonSoftIndex] + "\t" + answerListSpeed[ProtobufIndex] + "\t" + answerListSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] questionListSpeed = CompareDeserializers((List<Question>)MakeListObject(typeof(Question)), TestRuns);
                    Console.WriteLine("List<Question>\t" + questionListSpeed[JilIndex] + "\t" + questionListSpeed[NewtonSoftIndex] + "\t" + questionListSpeed[ProtobufIndex] + "\t" + questionListSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] userListSpeed = CompareDeserializers((List<User>)MakeListObject(typeof(User)), TestRuns);
                    Console.WriteLine("List<User>\t" + userListSpeed[JilIndex] + "\t" + userListSpeed[NewtonSoftIndex] + "\t" + userListSpeed[ProtobufIndex] + "\t" + userListSpeed[ServiceStackIndex]);
                }

                Console.WriteLine();

                // dictionary
                {
                    ResetRand();
                    double[] answerDictSpeed = CompareDeserializers((Dictionary<string, Answer>)MakeDictionaryObject(typeof(Answer)), TestRuns);
                    Console.WriteLine("Dictionary<string,Answer>\t" + answerDictSpeed[JilIndex] + "\t" + answerDictSpeed[NewtonSoftIndex] + "\t" + answerDictSpeed[ProtobufIndex] + "\t" + answerDictSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] questionDictSpeed = CompareDeserializers((Dictionary<string, Question>)MakeDictionaryObject(typeof(Question)), TestRuns);
                    Console.WriteLine("Dictionary<string,Question>\t" + questionDictSpeed[JilIndex] + "\t" + questionDictSpeed[NewtonSoftIndex] + "\t" + questionDictSpeed[ProtobufIndex] + "\t" + questionDictSpeed[ServiceStackIndex]);
                }

                {
                    ResetRand();
                    double[] userDictSpeed = CompareDeserializers((Dictionary<string, User>)MakeDictionaryObject(typeof(User)), TestRuns);
                    Console.WriteLine("Dictionary<string,User>\t" + userDictSpeed[JilIndex] + "\t" + userDictSpeed[NewtonSoftIndex] + "\t" + userDictSpeed[ProtobufIndex] + "\t" + userDictSpeed[ServiceStackIndex]);
                }
            }
        }

        static double[] CompareDeserializers<T>(T obj, int testRuns)
        {
            Action<string> jilDeserializer = a => JilDeserialize(a, obj);
            Action<string> newtonSoftDeserializer = a => NewtonsoftDeserialize<T>(a);
            Action<byte[]> protobufDeserializer = a => ProtobufDeserialize<T>(a);
            Action<string> serviceStackDeserializer = a => ServiceStackDeserialize<T>(a);

            var json = JilSerialize(obj);
            var bytes = ProtobufSerialize(obj);

            jilDeserializer(json);
            newtonSoftDeserializer(json);
            protobufDeserializer(bytes);
            serviceStackDeserializer(json);

            double[] ret = null;

            // Jil
            Action jil =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[JilIndex] = AverageRuntime(() => jilDeserializer(json), testRuns);
                };

            // NewtonSoft
            Action newtonSoft =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[NewtonSoftIndex] = AverageRuntime(() => newtonSoftDeserializer(json), testRuns);
                };

            // Protobuf
            Action protobuf =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[ProtobufIndex] = AverageRuntime(() => protobufDeserializer(bytes), testRuns);
                };

            // ServiceStack
            Action serviceStack =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[ServiceStackIndex] = AverageRuntime(() => serviceStackDeserializer(json), testRuns);
                };

            var allRuns = new List<double[]>();

            foreach (var perm in Permutate(4))
            {
                ret = new double[4];
                foreach (var ix in perm)
                {
                    switch (ix)
                    {
                        case 0: jil(); break;
                        case 1: newtonSoft(); break;
                        case 2: protobuf(); break;
                        case 3: serviceStack(); break;
                        default: throw new InvalidOperationException();
                    }
                }

                allRuns.Add(ret);
            }

            // we've got all combinations of runs, pick the *median* ones
            //   since those should be the most representive controling for surrounding
            //   code
            var medians = new double[4];

            for (var i = 0; i < 4; i++)
            {
                var allForI = allRuns.Select(run => run[i]).OrderBy(_ => _).ToArray();
                medians[i] = (allForI[1] + allForI[2]) / 2.0;
            }

            return medians;
        }

        static double[] CompareSerializers<T>(T obj, int testRuns)
        {
            Action<T> jilSerializer = a => JilSerialize(a);
            Action<T> newtonSoftSerializer = a => NewtonsoftSerialize(a);
            Action<T> protobufSerializer = a => ProtobufSerialize(a);
            Action<T> serviceStackSerializer = a => ServiceStackSerialize(a);

            jilSerializer(obj);
            newtonSoftSerializer(obj);
            protobufSerializer(obj);
            serviceStackSerializer(obj);
            
            double[] ret = null;
            
            // Jil
            Action jil =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[JilIndex] = AverageRuntime(() => jilSerializer(obj), testRuns);
                };

            // NewtonSoft
            Action newtonSoft =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[NewtonSoftIndex] = AverageRuntime(() => newtonSoftSerializer(obj), testRuns);
                };

            // Protobuf
            Action protobuf =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[ProtobufIndex] = AverageRuntime(() => protobufSerializer(obj), testRuns);
                };

            // ServiceStack
            Action serviceStack =
                () =>
                {
                    System.GC.Collect(2, GCCollectionMode.Forced, blocking: true);
                    ret[ServiceStackIndex] = AverageRuntime(() => serviceStackSerializer(obj), testRuns);
                };

            var allRuns = new List<double[]>();

            foreach (var perm in Permutate(4))
            {
                ret = new double[4];
                foreach(var ix in perm)
                {
                    switch (ix)
                    {
                        case 0: jil(); break;
                        case 1: newtonSoft(); break;
                        case 2: protobuf(); break;
                        case 3: serviceStack(); break;
                        default: throw new InvalidOperationException();
                    }
                }

                allRuns.Add(ret);
            }

            // we've got all combinations of runs, pick the *median* ones
            //   since those should be the most representive controling for surrounding
            //   code
            var medians = new double[4];

            for (var i = 0; i < 4; i++)
            {
                var allForI = allRuns.Select(run => run[i]).OrderBy(_ => _).ToArray();

                if (allForI.Length % 2 == 1)
                {
                    medians[i] = allForI[allForI.Length / 2];
                }
                else
                {
                    // 4 -> 1, 2
                    medians[i] = (allForI[allForI.Length / 2 - 1] + allForI[allForI.Length / 2]) / 2.0;
                }
            }

            return medians;
        }

        static IEnumerable<int[]> Permutate(int upTo)
        {
            var asArr = Enumerable.Range(0, upTo);

            return Permutate(asArr.ToArray());
        }

        static IEnumerable<T[]> Permutate<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                yield return new T[0];
            }
            else
            {
                for (int pick = 0; pick < array.Length; ++pick)
                {
                    T item = array[pick];
                    int i = -1;
                    T[] rest = Array.FindAll<T>(
                        array, delegate(T p) { return ++i != pick; }
                    );
                    foreach (T[] restPermuted in Permutate(rest))
                    {
                        i = -1;
                        yield return Array.ConvertAll<T, T>(
                            array,
                            delegate(T p)
                            {
                                return ++i == 0 ? item : restPermuted[i - 1];
                            }
                        );
                    }
                }
            }
        }

        static double AverageRuntime(Action act, int runs)
        {
            var watch = new Stopwatch();

            watch.Start();
            for (var i = 0; i < runs; i++)
            {
                act();
            }
            watch.Stop();

            return watch.ElapsedMilliseconds / (double)runs;
        }

        static void Main(string[] args)
        {
            // single core
            var proc = Process.GetCurrentProcess();
            proc.ProcessorAffinity = (IntPtr)1;             // only use the first processor

            Console.WriteLine("1. Quick Graph");
            Console.WriteLine("2. Comparison Graph");

            Console.WriteLine();
            Console.Write("Selection? ");
            var k = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (k == '1')
            {
                Console.WriteLine("== Quick Graph " + DateTime.UtcNow + " ==");
                DoQuickGraph();
            }
            else
            {
                if (k == '2')
                {
                    Console.WriteLine("== Comparison Graph " + DateTime.UtcNow + " ==");
                    DoComparisonGraph();
                }
                else
                {
                    Console.WriteLine("Unknown Options");
                    return;
                }
            }

            Console.WriteLine("== Finished ==");

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
