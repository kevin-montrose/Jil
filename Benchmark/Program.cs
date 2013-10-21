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
        static Random Rand = new Random(135581624);

        static List<Type> GetModels()
        {
            return new List<Type> { typeof(Benchmark.Models.Answer) };
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

        static void AssertEquality(object original, object copy)
        {

        }

        static MethodInfo _DoSpeedTest = typeof(Program).GetMethod("DoSpeedTest", BindingFlags.Static | BindingFlags.NonPublic);
        static List<Result> DoSpeedTest<T>(string serializerName, string niceTypeName, Func<T, string> serializeFunc, T obj)
        {
            const int TestRuns = 5;

            string data = null;

            var testGroup = new TestGroup(niceTypeName + " - " + serializerName);

            var serializationTestSummary =
                testGroup
                    .Plan("Serialization", () => data = serializeFunc(obj), TestRuns)
                    .GetResult();

            var newtonsoftCopyObj = JsonConvert.DeserializeObject<T>(data);
            AssertEquality(obj, newtonsoftCopyObj);

            var serviceStackCopyObj = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(data);
            AssertEquality(obj, serviceStackCopyObj);

            return
                serializationTestSummary.Outcomes.Select(
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
            var typeName = model.Name;

            var ret = new List<Result>();

            // single objects
            {
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

        static void Main(string[] args)
        {
            var models = GetModels();

            var results = new List<Result>();

            foreach (var model in models)
            {
                results.AddRange(DoSpeedTestsFor(model));
            }
        }
    }
}
