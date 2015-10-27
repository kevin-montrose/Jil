using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    class Result
    {
        public string Serializer { get; set; }
        public string TypeName { get; set; }
        public TimeSpan Elapsed { get; set; }
        public int[] GCCounts { get; set; }
    }
}
