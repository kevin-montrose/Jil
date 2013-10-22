using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class NetworkUser
    {
        [ProtoMember(1)]
        public string site_name { get; set; }
        [ProtoMember(2)]
        public string site_url { get; set; }
        [ProtoMember(3)]
        public int? user_id { get; set; }
        [ProtoMember(4)]
        public int? reputation { get; set; }
        [ProtoMember(5)]
        public int? account_id { get; set; }
        [ProtoMember(6)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(7)]
        public UserType? user_type { get; set; }
        [ProtoMember(8)]
        public User.BadgeCount badge_counts { get; set; }
        [ProtoMember(9)]
        public DateTime? last_access_date { get; set; }
        [ProtoMember(10)]
        public int? answer_count { get; set; }
        [ProtoMember(11)]
        public int? question_count { get; set; }
    }
}
