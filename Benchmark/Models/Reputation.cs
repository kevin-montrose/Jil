using ProtoBuf;
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

    [ProtoContract]
    class Reputation
    {
        [ProtoMember(1)]
        public int? user_id { get; set; }
        [ProtoMember(2)]
        public int? post_id { get; set; }
        [ProtoMember(3)]
        public PostType? post_type { get; set; }
        [ProtoMember(4)]
        public VoteType? vote_type { get; set; }
        [ProtoMember(5)]
        public string title { get; set; }
        [ProtoMember(6)]
        public string link { get; set; }
        [ProtoMember(7)]
        public int? reputation_change { get; set; }
        [ProtoMember(8)]
        public DateTime? on_date { get; set; }
    }
}
