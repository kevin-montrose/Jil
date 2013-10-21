using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class TopTag
    {
        public string tag_name { get; set; }
        public int? question_score { get; set; }
        public int? question_count { get; set; }
        public int? answer_score { get; set; }
        public int? answer_count { get; set; }
        public int? user_id { get; set; }
    }
}
