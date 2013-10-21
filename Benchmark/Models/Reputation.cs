using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum VoteType : byte
    {
        up_votes = 2,
        down_votes = 3,
        spam = 12,
        accepts = 1,
        bounties_won = 9,
        bounties_offered = 8,
        suggested_edits = 16
    }

    class Reputation
    {
        public int? user_id { get; set; }
        public int? post_id { get; set; }
        public PostType? post_type { get; set; }
        public VoteType? vote_type { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public int? reputation_change { get; set; }
        public DateTime? on_date { get; set; }
    }
}
