using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class TagScore
    {
        public ShallowUser user { get; set; }
        public int? score { get; set; }
        public int? post_count { get; set; }
    }
}
