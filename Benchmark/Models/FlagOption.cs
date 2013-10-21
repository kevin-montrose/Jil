using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class FlagOption
    {
        public int? option_id { get; set; }
        public bool? requires_comment { get; set; }
        public bool? requires_site { get; set; }
        public bool? requires_question_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<FlagOption> sub_options { get; set; }
        public bool? has_flagged { get; set; }
        public int? count { get; set; }
    }
}
