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
    class Badge : IGenericEquality<Badge>
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

        public bool Equals(Badge obj)
        {
            return
                this.award_count.TrueEquals(obj.award_count) &&
                this.badge_id.TrueEquals(obj.badge_id) &&
                this.badge_type.TrueEquals(obj.badge_type) &&
                this.description.TrueEqualsString(obj.description) &&
                this.link.TrueEqualsString(obj.link) &&
                this.name.TrueEqualsString(obj.name) &&
                this.rank.TrueEquals(obj.rank) &&
                this.user.TrueEquals(obj.user);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.award_count.TrueEquals((int?)obj.award_count) &&
                this.badge_id.TrueEquals((int?)obj.badge_id) &&
                this.badge_type.TrueEquals((BadgeType?)obj.badge_type) &&
                this.description.TrueEqualsString((string)obj.description) &&
                this.link.TrueEqualsString((string)obj.link) &&
                this.name.TrueEqualsString((string)obj.name) &&
                this.rank.TrueEquals((BadgeRank?)obj.rank) &&
                (this.user == null && obj.user == null || this.user.EqualsDynamic(obj.user));
        }
    }
}
