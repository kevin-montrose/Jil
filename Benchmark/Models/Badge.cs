using ProtoBuf;
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

    [ProtoContract]
    class Badge
    {
        [ProtoMember(1)]
        public int? badge_id { get; set; }
        [ProtoMember(2)]
        public BadgeRank? rank { get; set; }
        [ProtoMember(3)]
        public string name { get; set; }
        [ProtoMember(4)]
        public string description { get; set; }
        [ProtoMember(5)]
        public int? award_count { get; set; }
        [ProtoMember(6)]
        public BadgeType? badge_type { get; set; }
        [ProtoMember(7)]
        public ShallowUser user { get; set; }
        [ProtoMember(8)]
        public string link { get; set; }
    }
}
