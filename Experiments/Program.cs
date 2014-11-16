using Jil.Common;
using Jil.Serialize;
using System;
using System.Collections.Concurrent;
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
        class ExampleObj
        {
            public string String { get; set; }
            public char Char { get; set; }

            public long Long { get; set; }
            public int Int { get; set; }
            public short Short { get; set; }
            public byte Byte { get; set; }

            public ulong ULong { get; set; }
            public uint UInt { get; set; }
            public ushort UShort { get; set; }
            public sbyte SByte { get; set; }

            public List<string> List { get; set; }
            public string[] Array { get; set; }

            public Dictionary<string, string> Dictionary { get; set; }

            public ExampleObj RecurseObj { get; set; }

            public ExampleObj() { }

            public ExampleObj(Random rand)
            {
                String = RandomString(rand);
                Char = RandomChar(rand);
                
                Long = RandomLong(rand);
                Int = RandomInt(rand);
                Short = RandomShort(rand);
                Byte = RandomByte(rand);

                ULong = (ulong)RandomLong(rand);
                UInt = (uint)RandomInt(rand);
                UShort = (ushort)RandomShort(rand);
                SByte = (sbyte)RandomByte(rand);

                List = Enumerable.Range(0, rand.Next(10)).Select(_ => RandomString(rand)).ToList();
                Array = Enumerable.Range(0, rand.Next(10)).Select(_ => RandomString(rand)).ToArray();

                Dictionary = Enumerable.Range(0, rand.Next(10)).ToDictionary(_ => RandomString(rand), _ => RandomString(rand));

                RecurseObj = rand.Next(2) == 0 ? new ExampleObj(rand) : null;
            }

            static string RandomString(Random rand)
            {
                var len = rand.Next(10);

                return new string(Enumerable.Range(0, len).Select(_ => RandomChar(rand)).ToArray());
            }

            static char RandomChar(Random rand)
            {
                var next = rand.Next() % char.MaxValue;

                var ret = (char)next;
                if(char.IsLowSurrogate(ret) || char.IsHighSurrogate(ret))
                {
                    return RandomChar(rand);
                }

                return ret;
            }

            static long RandomLong(Random rand)
            {
                var bs = new byte[8];
                rand.NextBytes(bs);

                return BitConverter.ToInt64(bs, 0);
            }

            static int RandomInt(Random rand)
            {
                var bs = new byte[4];
                rand.NextBytes(bs);

                return BitConverter.ToInt32(bs, 0);
            }

            static short RandomShort(Random rand)
            {
                var bs = new byte[2];
                rand.NextBytes(bs);

                return BitConverter.ToInt16(bs, 0);
            }

            static byte RandomByte(Random rand)
            {
                return (byte)(rand.Next() % byte.MaxValue);
            }
        }

        static void Main(string[] args)
        {
            var random = new Random(27291702);

            var obj = new ExampleObj(random);

            //var obj = new Dictionary<string, string> { {"hello", "world"}, {"world", "hello"} };

            Func<string> stream =
                () =>
                {
                    using (var writer = new StringWriter())
                    {
                        Jil.JSON.Serialize(obj, writer);
                        return writer.ToString();
                    }
                };

            Func<string> str =
                () =>
                {
                    return Jil.JSON.Serialize(obj);
                };

            // prime
            for (var i = 0; i < 5; i++)
            {
                var streamRes = stream();
                var strRes = str();

                if (streamRes != strRes) throw new Exception("Uhhhhh");
            }

            const int Iterations = 1000000;

            TimeSpan streamTime;
            {
                System.GC.Collect(3, GCCollectionMode.Forced, true);
                var streamTimer = new Stopwatch();
                streamTimer.Start();
                for (var i = 0; i < Iterations; i++)
                {
                    stream();
                }
                streamTimer.Stop();
                streamTime = streamTimer.Elapsed;
            }

            TimeSpan strTime;
            {
                System.GC.Collect(3, GCCollectionMode.Forced, true);
                var strTimer = new Stopwatch();
                strTimer.Start();
                for (var i = 0; i < Iterations; i++)
                {
                    str();
                }
                strTimer.Stop();
                strTime = strTimer.Elapsed;
            }

            Console.WriteLine("Stream: " + streamTime.TotalMilliseconds + "ms");
            Console.WriteLine("String: " + strTime.TotalMilliseconds + "ms");
            Console.WriteLine();
            Console.WriteLine("Delta: " + (streamTime - strTime).TotalMilliseconds + "ms");
        }
    }
}
