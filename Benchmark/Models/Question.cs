using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Question
    {
        [ProtoContract]
        public class ClosedDetails
        {
            [ProtoContract]
            public class OriginalQuestion
            {
                [ProtoMember(1)]
                public int? question_id { get; set; }
                [ProtoMember(2)]
                public string title { get; set; }
                [ProtoMember(3)]
                public int? answer_count { get; set; }
                [ProtoMember(4)]
                public int? accepted_answer_id { get; set; }
            }

            [ProtoMember(1)]
            public bool? on_hold { get; set; }
            [ProtoMember(2)]
            public string reason { get; set; }
            [ProtoMember(3)]
            public string description { get; set; }
            [ProtoMember(4)]
            public List<ShallowUser> by_users { get; set; }
            [ProtoMember(5)]
            public List<OriginalQuestion> original_questions { get; set; }
        }

        [ProtoContract]
        public class Notice
        {
            [ProtoMember(1)]
            public string body { get; set; }
            [ProtoMember(2)]
            public DateTime? creation_date { get; set; }
            [ProtoMember(3)]
            public int? owner_user_id { get; set; }
        }

        [ProtoContract]
        public class MigrationInfo
        {
            [ProtoMember(1)]
            public int? question_id { get; set; }
            [ProtoMember(2)]
            public Info.Site other_site { get; set; }
            [ProtoMember(3)]
            public DateTime? on_date { get; set; }
        }

        [ProtoMember(1)]
        public int? question_id { get; set; }
        [ProtoMember(2)]
        public DateTime? last_edit_date { get; set; }
        [ProtoMember(3)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(4)]
        public DateTime? last_activity_date { get; set; }
        [ProtoMember(5)]
        public DateTime? locked_date { get; set; }
        [ProtoMember(6)]
        public int? score { get; set; }
        [ProtoMember(7)]
        public DateTime? community_owned_date { get; set; }
        [ProtoMember(8)]
        public int? answer_count { get; set; }
        [ProtoMember(9)]
        public int? accepted_answer_id { get; set; }
        [ProtoMember(10)]
        public MigrationInfo migrated_to { get; set; }
        [ProtoMember(11)]
        public MigrationInfo migrated_from { get; set; }
        [ProtoMember(12)]
        public DateTime? bounty_closes_date { get; set; }
        [ProtoMember(13)]
        public int? bounty_amount { get; set; }
        [ProtoMember(14)]
        public DateTime? closed_date { get; set; }
        [ProtoMember(15)]
        public DateTime? protected_date { get; set; }
        [ProtoMember(16)]
        public string body { get; set; }
        [ProtoMember(17)]
        public string title { get; set; }
        [ProtoMember(18)]
        public List<string> tags { get; set; }
        [ProtoMember(19)]
        public string closed_reason { get; set; }
        [ProtoMember(20)]
        public int? up_vote_count { get; set; }
        [ProtoMember(21)]
        public int? down_vote_count { get; set; }
        [ProtoMember(22)]
        public int? favorite_count { get; set; }
        [ProtoMember(23)]
        public int? view_count { get; set; }
        [ProtoMember(24)]
        public ShallowUser owner { get; set; }
        [ProtoMember(25)]
        public List<Comment> comments { get; set; }
        [ProtoMember(26)]
        public List<Answer> answers { get; set; }
        [ProtoMember(27)]
        public string link { get; set; }
        [ProtoMember(28)]
        public bool? is_answered { get; set; }
        [ProtoMember(29)]
        public int? close_vote_count { get; set; }
        [ProtoMember(30)]
        public int? reopen_vote_count { get; set; }
        [ProtoMember(31)]
        public int? delete_vote_count { get; set; }
        [ProtoMember(32)]
        public Notice notice { get; set; }
        [ProtoMember(33)]
        public bool? upvoted { get; set; }
        [ProtoMember(34)]
        public bool? downvoted { get; set; }
        [ProtoMember(35)]
        public bool? favorited { get; set; }
        [ProtoMember(36)]
        public ShallowUser last_editor { get; set; }
        [ProtoMember(37)]
        public int? comment_count { get; set; }
        [ProtoMember(38)]
        public string body_markdown { get; set; }
        [ProtoMember(39)]
        public ClosedDetails closed_details { get; set; }
        [ProtoMember(40)]
        public string share_link { get; set; }
    }
}
