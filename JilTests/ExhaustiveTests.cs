#if EXHAUSTIVE_TEST

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
    class ExhaustiveTests
    {
        [TestMethod]
        public void ULongSampling()
        {
            Action<ulong> test =
                ul =>
                {
                    using (var str = new StringReader(ul.ToString()))
                    {
                        try
                        {
                            var dyn = JSON.DeserializeDynamic(str);
                            var res = (ulong)dyn;
                            Assert.AreEqual(ul, res);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Failed on i = " + ul, e);
                        }
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

        [TestMethod]
        public void LongSampling()
        {
            Action<long> test =
                l =>
                {
                    using (var str = new StringReader(l.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var res = (long)dyn;
                        Assert.AreEqual(l, res);
                    }
                };

            test(long.MaxValue);
            test(long.MinValue);

            const long step = 8589934596;
            var i = long.MinValue;

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

                if (i == long.MaxValue) break;
            }
        }

        [TestMethod]
        public void AllUInts()
        {
            for (long i = uint.MinValue; i <= uint.MaxValue; i++)
            {
                try
                {
                    var asUInt = (uint)i;
                    using (var str = new StringReader(asUInt.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (uint)dyn;
                        Assert.AreEqual(asUInt, v, "Failed on i=" + asUInt);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (uint)i, e);
                }
            }
        }

        [TestMethod]
        public void AllInts()
        {
            for (long i = int.MinValue; i <= int.MaxValue; i++)
            {
                try
                {
                    var asInt = (int)i;
                    using (var str = new StringReader(asInt.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (int)dyn;
                        Assert.AreEqual(asInt, v, "Failed on i=" + asInt);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (int)i, e);
                }
            }
        }

        static readonly string[] _AllFloatsFormats = new[] { "F", "G", "R" };
        static IEnumerable<DeserializeDynamicTests._AllFloatsStruct> _AllFloats()
        {
            var byteArr = new byte[4];

            for (ulong i = 0; i <= uint.MaxValue; i++)
            {
                var f = DeserializeDynamicTests.ULongToFloat(i, byteArr);

                if (float.IsNaN(f) || float.IsInfinity(f)) continue;

                for (var j = 0; j < _AllFloatsFormats.Length; j++)
                {
                    var format = _AllFloatsFormats[j];
                    var asStr = f.ToString(format);

                    yield return new DeserializeDynamicTests._AllFloatsStruct { AsString = asStr, Float = f, Format = format, I = (uint)i };
                }
            }
        }

        class _AllFloatsPartitioner : Partitioner<DeserializeDynamicTests._AllFloatsStruct>
        {
            IEnumerable<DeserializeDynamicTests._AllFloatsStruct> Underlying;

            public _AllFloatsPartitioner(IEnumerable<DeserializeDynamicTests._AllFloatsStruct> underlying)
                : base()
            {
                Underlying = underlying;
            }

            public override bool SupportsDynamicPartitions
            {
                get
                {
                    return true;
                }
            }

            public override IEnumerable<DeserializeDynamicTests._AllFloatsStruct> GetDynamicPartitions()
            {
                return new DynamicPartition(Underlying);
            }

            public override IList<IEnumerator<DeserializeDynamicTests._AllFloatsStruct>> GetPartitions(int partitionCount)
            {
                throw new NotImplementedException();
            }

            class DynamicPartition : IEnumerable<DeserializeDynamicTests._AllFloatsStruct>
            {
                internal IEnumerator<DeserializeDynamicTests._AllFloatsStruct> All;

                public DynamicPartition(IEnumerable<DeserializeDynamicTests._AllFloatsStruct> all)
                {
                    All = all.GetEnumerator();
                }

                public IEnumerator<DeserializeDynamicTests._AllFloatsStruct> GetEnumerator()
                {
                    return new DynamicEnumerator(this);
                }

                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return this.GetEnumerator();
                }

                class DynamicEnumerator : IEnumerator<DeserializeDynamicTests._AllFloatsStruct>
                {
                    const int Capacity = 100;
                    DynamicPartition Outer;
                    Queue<DeserializeDynamicTests._AllFloatsStruct> Pending;

                    public DynamicEnumerator(DynamicPartition outer)
                    {
                        Outer = outer;
                        Pending = new Queue<DeserializeDynamicTests._AllFloatsStruct>(Capacity);
                    }

                    public DeserializeDynamicTests._AllFloatsStruct Current
                    {
                        get;
                        private set;
                    }

                    public void Dispose()
                    {
                        // Don't care
                    }

                    object System.Collections.IEnumerator.Current
                    {
                        get { return this.Current; }
                    }

                    public bool MoveNext()
                    {
                        if (Pending.Count == 0)
                        {
                            lock (Outer.All)
                            {
                                while (Outer.All.MoveNext() && Pending.Count < Capacity)
                                {
                                    Pending.Enqueue(Outer.All.Current);
                                }
                            }
                        }

                        if (Pending.Count == 0) return false;

                        Current = Pending.Dequeue();
                        return true;
                    }

                    public void Reset()
                    {
                        throw new NotSupportedException();
                    }
                }
            }
        }

        [TestMethod]
        public void AllFloats_Dynamic()
        {
            var e = _AllFloats();
            var partitioner = new _AllFloatsPartitioner(e);

            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Environment.ProcessorCount - 1;

            Parallel.ForEach(
                partitioner,
                options,
                part =>
                {
                    try
                    {
                        DeserializeDynamicTests.CheckFloat(part);
                    }
                    catch (Exception x)
                    {
                        throw new Exception(part.AsString, x);
                    }
                }
            );
        }

        [TestMethod]
        public void AllFloats_Static()
        {
            var e = _AllFloats();
            var partitioner = new _AllFloatsPartitioner(e);

            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Environment.ProcessorCount - 1;

            Parallel.ForEach(
                partitioner,
                options,
                part =>
                {
                    try
                    {
                        var i = part.I;
                        var format = part.Format;
                        var asStr = part.AsString;
                        var res = JSON.Deserialize<float>(asStr);
                        var reStr = res.ToString(format);

                        var delta = Math.Abs((float.Parse(asStr) - float.Parse(reStr)));

                        var closeEnough = asStr == reStr || delta <= float.Epsilon;

                        Assert.IsTrue(closeEnough, "For i=" + i + " format=" + format + " delta=" + delta + " epsilon=" + float.Epsilon);
                    }
                    catch (Exception x)
                    {
                        throw new Exception(part.AsString, x);
                    }
                }
            );
        }
    }
}

#endif