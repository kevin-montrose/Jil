using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class Tag
    {
        public string name { get; set; }
        public int? count { get; set; }
        public bool? is_required { get; set; }
        public bool? is_moderator_only { get; set; }
        public int? user_id { get; set; }
        public bool? has_synonyms { get; set; }
        public DateTime? last_activity_date { get; set; }
        public List<string> synonyms { get; set; }
    }
}
