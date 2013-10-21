using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class TagSynonym
    {
        public string from_tag { get; set; }
        public string to_tag { get; set; }
        public int? applied_count { get; set; }
        public DateTime? last_applied_date { get; set; }
        public DateTime? creation_date { get; set; }
    }
}
