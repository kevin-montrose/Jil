using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class User
    {
        [ProtoContract]
        public class BadgeCount
        {
            [ProtoMember(1)]
            public int? gold { get; set; }
            [ProtoMember(2)]
            public int? silver { get; set; }
            [ProtoMember(3)]
            public int? bronze { get; set; }
        }

        [ProtoMember(1)]
        public int? user_id { get; set; }
        [ProtoMember(2)]
        public UserType? user_type { get; set; }
        [ProtoMember(3)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(4)]
        public string display_name { get; set; }
        [ProtoMember(5)]
        public string profile_image { get; set; }
        [ProtoMember(6)]
        public int? reputation { get; set; }
        [ProtoMember(7)]
        public int? reputation_change_day { get; set; }
        [ProtoMember(8)]
        public int? reputation_change_week { get; set; }
        [ProtoMember(9)]
        public int? reputation_change_month { get; set; }
        [ProtoMember(10)]
        public int? reputation_change_quarter { get; set; }
        [ProtoMember(11)]
        public int? reputation_change_year { get; set; }
        [ProtoMember(12)]
        public int? age { get; set; }
        [ProtoMember(13)]
        public DateTime? last_access_date { get; set; }
        [ProtoMember(14)]
        public DateTime? last_modified_date { get; set; }
        [ProtoMember(15)]
        public bool? is_employee { get; set; }
        [ProtoMember(16)]
        public string link { get; set; }
        [ProtoMember(17)]
        public string website_url { get; set; }
        [ProtoMember(18)]
        public string location { get; set; }
        [ProtoMember(19)]
        public int? account_id { get; set; }
        [ProtoMember(20)]       
        public DateTime? timed_penalty_date { get; set; }
        [ProtoMember(21)]
        public BadgeCount badge_counts { get; set; }
        [ProtoMember(22)]       
        public int? question_count { get; set; }
        [ProtoMember(23)]
        public int? answer_count { get; set; }
        [ProtoMember(24)]
        public int? up_vote_count { get; set; }
        [ProtoMember(25)]
        public int? down_vote_count { get; set; }
        [ProtoMember(26)]
        public string about_me { get; set; }
        [ProtoMember(27)]
        public int? view_count { get; set; }
        [ProtoMember(28)]
        public int? accept_rate { get; set; }
    }
}
