using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum BadgeRank : byte
    {
        bronze = 3,
        silver = 2,
        gold = 1
    }

    enum BadgeType
    {
        named = 1,
        tag_based = 2
    }

    class Badge
    {
        public int? badge_id { get; set; }
        public BadgeRank? rank { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? award_count { get; set; }
        public BadgeType? badge_type { get; set; }
        public ShallowUser user { get; set; }
        public string link { get; set; }
    }
}
