using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum QuestionTimelineAction : byte
    {
        // Off of Posts
        question = 1,
        answer = 2,

        // Off of PostComments
        comment = 3,

        // Off of Posts2Votes
        unaccepted_answer = 4,
        accepted_answer = 5,

        // Aggregate off of Posts2Votes
        vote_aggregate = 6,

        // Off of PostHistory
        revision = 7,
        post_state_changed = 8
    }

    class QuestionTimeline
    {
        public QuestionTimelineAction? timeline_type { get; set; }
        public int? question_id { get; set; }
        public int? post_id { get; set; }
        public int? comment_id { get; set; }
        public string revision_guid { get; set; }
        public int? up_vote_count { get; set; }
        public int? down_vote_count { get; set; }
        public DateTime? creation_date { get; set; }
        public ShallowUser user { get; set; }
        public ShallowUser owner { get; set; }
    }
}
