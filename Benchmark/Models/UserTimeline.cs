using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum UserTimelineType : byte
    {
        commented = 1,
        asked = 2,
        answered = 3,
        badge = 4,
        revision = 5,
        accepted = 6,
        reviewed = 7,
        suggested = 8
    }

    [ProtoContract]
    class UserTimeline
    {
        [ProtoMember(1)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(2)]
        public PostType? post_type { get; set; }
        [ProtoMember(3)]
        public UserTimelineType? timeline_type { get; set; }
        [ProtoMember(4)]
        public int? user_id { get; set; }
        [ProtoMember(5)]
        public int? post_id { get; set; }
        [ProtoMember(6)]
        public int? comment_id { get; set; }
        [ProtoMember(7)]
        public int? suggested_edit_id { get; set; }
        [ProtoMember(8)]
        public int? badge_id { get; set; }
        [ProtoMember(9)]
        public string title { get; set; }
        [ProtoMember(10)]
        public string detail { get; set; }
        [ProtoMember(11)]
        public string link { get; set; }
    }
}
