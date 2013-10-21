using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class Question
    {
        public class ClosedDetails
        {
            public class OriginalQuestion
            {
                public int? question_id { get; set; }
                public string title { get; set; }
                public int? answer_count { get; set; }
                public int? accepted_answer_id { get; set; }
            }

            public bool? on_hold { get; set; }
            public string reason { get; set; }
            public string description { get; set; }
            public List<ShallowUser> by_users { get; set; }
            public List<OriginalQuestion> original_questions { get; set; }
        }

        public class Notice
        {
            public string body { get; set; }
            public DateTime? creation_date { get; set; }
            public int? owner_user_id { get; set; }
        }

        public class MigrationInfo
        {
            internal bool _is_from { get; set; }
            internal bool _is_to { get; set; }
            public int? question_id { get; set; }
            public Info.Site other_site { get; set; }
            public DateTime? on_date { get; set; }
        }

        public int? question_id { get; set; }
        public DateTime? last_edit_date { get; set; }
        public DateTime? creation_date { get; set; }
        public DateTime? last_activity_date { get; set; }
        public DateTime? locked_date { get; set; }
        public int? score { get; set; }
        public DateTime? community_owned_date { get; set; }
        public int? answer_count { get; set; }
        public int? accepted_answer_id { get; set; }
        internal DateTime? _MigrationDate { get; set; }
        public MigrationInfo migrated_to { get; set; }
        public MigrationInfo migrated_from { get; set; }
        public DateTime? bounty_closes_date { get; set; }
        public int? bounty_amount { get; set; }
        public DateTime? closed_date { get; set; }
        public DateTime? protected_date { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public List<string> tags { get; set; }
        public string closed_reason { get; set; }
        public int? up_vote_count { get; set; }
        public int? down_vote_count { get; set; }
        public int? favorite_count { get; set; }
        public int? view_count { get; set; }
        public ShallowUser owner { get; set; }
        public List<Comment> comments { get; set; }
        public List<Answer> answers { get; set; }
        public string link { get; set; }
        public bool? is_answered { get; set; }
        public int? close_vote_count { get; set; }
        public int? reopen_vote_count { get; set; }
        public int? delete_vote_count { get; set; }
        public Notice notice { get; set; }
        public bool? upvoted { get; set; }
        public bool? downvoted { get; set; }
        public bool? favorited { get; set; }
        public ShallowUser last_editor { get; set; }
        public int? comment_count { get; set; }
        public string body_markdown { get; set; }
        public ClosedDetails closed_details { get; set; }
        public string share_link { get; set; }
    }
}
