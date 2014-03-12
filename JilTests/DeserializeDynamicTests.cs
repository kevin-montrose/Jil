using Jil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JilTests
{
    [TestClass]
    public class DeserializeDynamicTests
    {
        [TestMethod]
        public void Bool()
        {
            using (var str = new StringReader("true"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsTrue((bool)res);
            }

            using (var str = new StringReader("false"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsFalse((bool)res);
            }
        }

        [TestMethod]
        public void Null()
        {
            using (var str = new StringReader("null"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsNull(res);
            }
        }

        [TestMethod]
        public void Guids()
        {
            var guid = Guid.NewGuid();

            using (var str = new StringReader("\"" + guid + "\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(guid, (Guid)res);
            }
        }

        enum _Enums
        {
            Hello,
            World
        }

        [TestMethod]
        public void Enums()
        {
            using (var str = new StringReader("\"Hello\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.Hello, (_Enums)res);
            }

            using (var str = new StringReader("\"hello\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.Hello, (_Enums)res);
            }

            using (var str = new StringReader("\"World\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.World, (_Enums)res);
            }

            using (var str = new StringReader("\"world\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(_Enums.World, (_Enums)res);
            }
        }

        [TestMethod]
        public void Number()
        {
            using (var str = new StringReader("1"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual((1.0).ToString(), val.ToString());
            }

            using (var str = new StringReader("1.234"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual((1.234).ToString(), val.ToString());
            }

            using (var str = new StringReader("-10e4"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual((-100000).ToString(), val.ToString());
            }

            using (var str = new StringReader("-1.3e-4"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.AreEqual((-0.00013).ToString(), val.ToString());
            }
        }

        [TestMethod]
        public void String()
        {
            using (var str = new StringReader("\"hello world\""))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (string)res;
                Assert.AreEqual("hello world", val);
            }

            using (var str = new StringReader("\"H\\u0065llo\""))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (string)res;
                Assert.AreEqual("Hello", val);
            }
        }

        [TestMethod]
        public void Array()
        {
            using (var str = new StringReader("[123, \"hello\", true]"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(3, (int)res.Length);
                Assert.AreEqual(123, (int)res[0]);
                Assert.AreEqual("hello", (string)res[1]);
                Assert.AreEqual(true, (bool)res[2]);
            }

            using (var str = new StringReader("[]"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(0, (int)res.Length);
            }
        }

        [TestMethod]
        public void Object()
        {
            using (var str = new StringReader("{\"hello\": 123, \"world\":[1,2,3]}"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.AreEqual(123, (int)res["hello"]);
                var arr = res["world"];
                Assert.AreEqual(3, (int)arr.Length);
                Assert.AreEqual(1, (int)arr[0]);
                Assert.AreEqual(2, (int)arr[1]);
                Assert.AreEqual(3, (int)arr[2]);
            }

            using (var str = new StringReader("{}"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.IsNotNull(res);
                var c = 0;
                foreach (var x in res)
                {
                    c++;
                }
                Assert.AreEqual(0, c);
            }
        }

        [TestMethod]
        public void ObjectEnumeration()
        {
            using (var str = new StringReader("{\"hello\":123, \"world\":456, \"foo\":789}"))
            {
                var c = 0;
                var res = JSON.DeserializeDynamic(str);
                foreach (var kv in res)
                {
                    string key = kv.Key;
                    dynamic val = kv.Value;

                    switch(c){
                        case 0: 
                            Assert.AreEqual("hello", key);
                            Assert.AreEqual(123, (int)val);
                            break;
                        case 1:
                            Assert.AreEqual("world", key);
                            Assert.AreEqual(456, (int)val);
                            break;
                        case 2:
                            Assert.AreEqual("foo", key);
                            Assert.AreEqual(789, (int)val);
                            break;
                        default: throw new Exception();
                    }
                    c++;
                }
                Assert.AreEqual(3, c);
            }
        }

        [TestMethod]
        public void ArrayEnumerator()
        {
            using (var str = new StringReader("[\"abcd\", \"efgh\", \"ijkl\"]"))
            {
                var c = 0;
                var res = JSON.DeserializeDynamic(str);
                foreach (var val in res)
                {
                    switch (c)
                    {
                        case 0:
                            Assert.AreEqual("abcd", (string)val);
                            break;
                        case 1:
                            Assert.AreEqual("efgh", (string)val);
                            break;
                        case 2:
                            Assert.AreEqual("ijkl", (string)val);
                            break;
                        default: throw new Exception();
                    }
                    c++;
                }
                Assert.AreEqual(3, c);
            }
        }

        [TestMethod]
        public void ArrayCoercion()
        {
            using (var str = new StringReader("[123, 456, 789]"))
            {
                var res = JSON.DeserializeDynamic(str);
                IEnumerable<int> asArr = res;
                Assert.AreEqual(3, asArr.Count());
                Assert.AreEqual(123, asArr.ElementAt(0));
                Assert.AreEqual(456, asArr.ElementAt(1));
                Assert.AreEqual(789, asArr.ElementAt(2));
            }
        }

        enum _DictionaryCoercion
        {
            a,
            b,
            c
        }

        [TestMethod]
        public void DictionaryCoercion()
        {
            using (var str = new StringReader("{\"a\": 123, \"b\": 456, \"c\": 789}"))
            {
                var res = JSON.DeserializeDynamic(str);
                IDictionary<string, int> asDict = res;
                Assert.AreEqual(3, asDict.Count());
                Assert.IsTrue(asDict.ContainsKey("a"));
                Assert.AreEqual(123, asDict["a"]);
                Assert.IsTrue(asDict.ContainsKey("b"));
                Assert.AreEqual(456, asDict["b"]);
                Assert.IsTrue(asDict.ContainsKey("c"));
                Assert.AreEqual(789, asDict["c"]);
            }

            using (var str = new StringReader("{\"a\": 123, \"b\": 456, \"c\": 789}"))
            {
                var res = JSON.DeserializeDynamic(str);
                IDictionary<_DictionaryCoercion, int> asDict = res;
                Assert.AreEqual(3, asDict.Count());
                Assert.IsTrue(asDict.ContainsKey(_DictionaryCoercion.a));
                Assert.AreEqual(123, asDict[_DictionaryCoercion.a]);
                Assert.IsTrue(asDict.ContainsKey(_DictionaryCoercion.b));
                Assert.AreEqual(456, asDict[_DictionaryCoercion.b]);
                Assert.IsTrue(asDict.ContainsKey(_DictionaryCoercion.c));
                Assert.AreEqual(789, asDict[_DictionaryCoercion.c]);
            }
        }

        [TestMethod]
        public void TrickyFloats()
        {
            {
                uint i = 1276679976;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("R"), Format = "R", I = i });
            }

            {
                uint i = 1343554351;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("R"), Format = "R", I = i });
            }

            {
                uint i = 1593835550;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("F"), Format = "F", I = i });
            }

            {
                uint i = 1602224307;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("F"), Format = "F", I = i });
            }
        }

        struct _AllFloatsStruct
        {
            public float Float;
            public string AsString;
            public string Format;
            public uint I;
        }

        static float ULongToFloat(ulong i, byte[] byteArr)
        {
            var asInt = (uint)i;
            byteArr[0] = (byte)((asInt) & 0xFF);
            byteArr[1] = (byte)((asInt >> 8) & 0xFF);
            byteArr[2] = (byte)((asInt >> 16) & 0xFF);
            byteArr[3] = (byte)((asInt >> 24) & 0xFF);
            var f = BitConverter.ToSingle(byteArr, 0);

            return f;
        }

        static void CheckFloat(_AllFloatsStruct part)
        {
            var i = part.I;
            var format = part.Format;
            var asStr = part.AsString;
            var dyn = JSON.DeserializeDynamic(asStr);
            var res = (float)dyn;
            var reStr = res.ToString(format);

            var delta = Math.Abs((float.Parse(asStr) - float.Parse(reStr)));

            var closeEnough = part.Float.ToString() == res.ToString() || asStr == reStr || delta <= float.Epsilon;

            Assert.IsTrue(closeEnough, "For i=" + i + " format=" + format + " delta=" + delta + " epsilon=" + float.Epsilon);
        }

        [TestMethod]
        public void SignedSmallNumberTests()
        {
            for (long i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                try
                {
                    var asNum = (sbyte)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (sbyte)dyn;
                        Assert.AreEqual(asNum, v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (sbyte)i, e);
                }
            }

            for (long i = short.MinValue; i <= short.MaxValue; i++)
            {
                try
                {
                    var asNum = (short)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (short)dyn;
                        Assert.AreEqual(asNum, v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (short)i, e);
                }
            }
        }

        [TestMethod]
        public void UnsignedSmallNumberTests()
        {
            for (long i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                try
                {
                    var asNum = (byte)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (byte)dyn;
                        Assert.AreEqual(asNum, v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (byte)i, e);
                }
            }

            for (long i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                try
                {
                    var asNum = (ushort)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (ushort)dyn;
                        Assert.AreEqual(asNum, v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (ushort)i, e);
                }
            }
        }

        [TestMethod]
        public void ULongSampling()
        {
            Action<ulong> test =
                l =>
                {
                    using (var str = new StringReader(l.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var res = (ulong)dyn;
                        Assert.AreEqual(l, res);
                    }
                };

            test(ulong.MaxValue);
            test(ulong.MinValue);

            const ulong step = 8589934596;
            var i = ulong.MinValue;

            while (true)
            {
                test(i);
                test(i + 1);

                try
                {
                    checked
                    {
                        i += step;
                    }
                }
                catch { break; }

                if (i == ulong.MaxValue) break;
            }
        }

        //[TestMethod]
        //public void LongSampling()
        //{
        //    Action<long> test =
        //        l =>
        //        {
        //            using (var str = new StringReader(l.ToString()))
        //            {
        //                var dyn = JSON.DeserializeDynamic(str);
        //                var res = (long)dyn;
        //                Assert.AreEqual(l, res);
        //            }
        //        };

        //    test(long.MaxValue);
        //    test(long.MinValue);

        //    const long step = 8589934596;
        //    var i = long.MinValue;

        //    while (true)
        //    {
        //        test(i);
        //        test(i + 1);

        //        try
        //        {
        //            checked
        //            {
        //                i += step;
        //            }
        //        }
        //        catch { break; }

        //        if (i == long.MaxValue) break;
        //    }
        //}

        //[TestMethod]
        //public void AllUInts()
        //{
        //    for (long i = uint.MinValue; i <= uint.MaxValue; i++)
        //    {
        //        try
        //        {
        //            var asUInt = (uint)i;
        //            using (var str = new StringReader(asUInt.ToString()))
        //            {
        //                var dyn = JSON.DeserializeDynamic(str);
        //                var v = (uint)dyn;
        //                Assert.AreEqual(asUInt, v, "Failed on i=" + asUInt);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception("Failed on i = " + (uint)i, e);
        //        }
        //    }
        //}

        //[TestMethod]
        //public void AllInts()
        //{
        //    for (long i = int.MinValue; i <= int.MaxValue; i++)
        //    {
        //        try
        //        {
        //            var asInt = (int)i;
        //            using (var str = new StringReader(asInt.ToString()))
        //            {
        //                var dyn = JSON.DeserializeDynamic(str);
        //                var v = (int)dyn;
        //                Assert.AreEqual(asInt, v, "Failed on i=" + asInt);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception("Failed on i = " + (int)i, e);
        //        }
        //    }
        //}

        //static readonly string[] _AllFloatsFormats = new[] { "F", "G", "R" };
        //static IEnumerable<_AllFloatsStruct> _AllFloats()
        //{
        //    var byteArr = new byte[4];

        //    for (ulong i = 0; i <= uint.MaxValue; i++)
        //    {
        //        var f = ULongToFloat(i, byteArr);

        //        if (float.IsNaN(f) || float.IsInfinity(f)) continue;

        //        for (var j = 0; j < _AllFloatsFormats.Length; j++)
        //        {
        //            var format = _AllFloatsFormats[j];
        //            var asStr = f.ToString(format);

        //            yield return new _AllFloatsStruct { AsString = asStr, Float = f, Format = format, I = (uint)i };
        //        }
        //    }
        //}

        //class _AllFloatsPartitioner : Partitioner<_AllFloatsStruct>
        //{
        //    IEnumerable<_AllFloatsStruct> Underlying;

        //    public _AllFloatsPartitioner(IEnumerable<_AllFloatsStruct> underlying)
        //        : base()
        //    {
        //        Underlying = underlying;
        //    }

        //    public override bool SupportsDynamicPartitions
        //    {
        //        get
        //        {
        //            return true;
        //        }
        //    }

        //    public override IEnumerable<_AllFloatsStruct> GetDynamicPartitions()
        //    {
        //        return new DynamicPartition(Underlying);
        //    }

        //    public override IList<IEnumerator<_AllFloatsStruct>> GetPartitions(int partitionCount)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    class DynamicPartition : IEnumerable<_AllFloatsStruct>
        //    {
        //        internal IEnumerator<_AllFloatsStruct> All;

        //        public DynamicPartition(IEnumerable<_AllFloatsStruct> all)
        //        {
        //            All = all.GetEnumerator();
        //        }

        //        public IEnumerator<_AllFloatsStruct> GetEnumerator()
        //        {
        //            return new DynamicEnumerator(this);
        //        }

        //        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //        {
        //            return this.GetEnumerator();
        //        }

        //        class DynamicEnumerator : IEnumerator<_AllFloatsStruct>
        //        {
        //            const int Capacity = 100;
        //            DynamicPartition Outer;
        //            Queue<_AllFloatsStruct> Pending;

        //            public DynamicEnumerator(DynamicPartition outer)
        //            {
        //                Outer = outer;
        //                Pending = new Queue<_AllFloatsStruct>(Capacity);
        //            }

        //            public _AllFloatsStruct Current
        //            {
        //                get;
        //                private set;
        //            }

        //            public void Dispose()
        //            {
        //                // Don't care
        //            }

        //            object System.Collections.IEnumerator.Current
        //            {
        //                get { return this.Current; }
        //            }

        //            public bool MoveNext()
        //            {
        //                if (Pending.Count == 0)
        //                {
        //                    lock (Outer.All)
        //                    {
        //                        while (Outer.All.MoveNext() && Pending.Count < Capacity)
        //                        {
        //                            Pending.Enqueue(Outer.All.Current);
        //                        }
        //                    }
        //                }

        //                if (Pending.Count == 0) return false;

        //                Current = Pending.Dequeue();
        //                return true;
        //            }

        //            public void Reset()
        //            {
        //                throw new NotSupportedException();
        //            }
        //        }
        //    }
        //}

        //[TestMethod]
        //public void AllFloats()
        //{
        //    var e = _AllFloats();
        //    var partitioner = new _AllFloatsPartitioner(e);

        //    var options = new ParallelOptions();
        //    options.MaxDegreeOfParallelism = Environment.ProcessorCount - 1;

        //    Parallel.ForEach(
        //        partitioner,
        //        options,
        //        part =>
        //        {
        //            try
        //            {
        //                CheckFloat(part);
        //            }
        //            catch (Exception x)
        //            {
        //                throw new Exception(part.AsString, x);
        //            }
        //        }
        //    );
        //}
    }
}
