using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiments
{
    static class ExtensionMethods
    {
        public static int? NextNullableInt(this Random rand)
        {
            if (rand.Next(2) == 0) return null;

            return rand.Next();
        }

        static readonly DateTime Epoch = new DateTime(1970, 1, 1);
        public static DateTime NextDateTime(this Random rand)
        {   
            var ret = Epoch;

            if (rand.Next(2) == 0)
            {
                ret = ret.AddYears(rand.Next(500));
            }
            else
            {
                ret = ret.AddYears(-rand.Next(500));
            }

            if (rand.Next(2) == 0)
            {
                ret = ret.AddMonths(rand.Next(13));
            }
            else
            {
                ret = ret.AddMonths(-rand.Next(13));
            }

            if (rand.Next(2) == 0)
            {
                ret = ret.AddDays(rand.Next(32));
            }
            else
            {
                ret = ret.AddDays(-rand.Next(32));
            }

            if (rand.Next(2) == 0)
            {
                ret = ret.AddHours(rand.Next(25));
            }
            else
            {
                ret = ret.AddHours(-rand.Next(25));
            }

            if (rand.Next(2) == 0)
            {
                ret = ret.AddMinutes(rand.Next(61));
            }
            else
            {
                ret = ret.AddMinutes(-rand.Next(61));
            }

            if (rand.Next(2) == 0)
            {
                ret = ret.AddSeconds(rand.Next(61));
            }
            else
            {
                ret = ret.AddSeconds(-rand.Next(61));
            }

            if (rand.Next(2) == 0)
            {
                ret = ret.AddMilliseconds(rand.Next(1001));
            }
            else
            {
                ret = ret.AddMilliseconds(-rand.Next(1001));
            }

            return ret;
        }

        public static DateTime? NextNullableDateTime(this Random rand)
        {
            if (rand.Next(2) == 0) return null;

            return NextDateTime(rand);
        }

        public static string NextString(this Random rand)
        {
            var len = rand.Next(4098);
            var ret = new char[len];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = NextChar(rand);
            }

            return new string(ret);
        }

        public static char NextChar(this Random rand)
        {
            var ret = (char)rand.Next(ushort.MaxValue);

            // generate sensible utf-16
            if (char.IsHighSurrogate(ret) || char.IsLowSurrogate(ret)) return NextChar(rand);

            return ret;
        }

        public static char? NextNullableChar(this Random rand)
        {
            if (rand.Next(2) == 0) return null;

            return NextChar(rand);
        }

        public static IEnumerable<int> NextIntList(this Random rand)
        {
            var len = rand.Next(256);

            return Enumerable.Range(0, len).Select(_ => rand.Next()).ToList();
        }

        public static IEnumerable<double> NextDoubleList(this Random rand)
        {
            var len = rand.Next(256);

            return Enumerable.Range(0, len).Select(_ => rand.NextDouble()).ToList();
        }

        public static double Median(this IEnumerable<double> e)
        {
            var len = e.Count();
            var inOrder = e.OrderBy(_ => _);

            if (len % 2 == 0)
            {
                return (inOrder.ElementAt(len / 2) + inOrder.ElementAt(len / 2 + 1)) / 2;
            }

            return inOrder.ElementAt(len / 2 + 1);
        }

        public static void ForEach<T>(this IEnumerable<T> e, Action<T> a)
        {
            foreach (var x in e)
            {
                a(x);
            }
        }
    }
}
