using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Answer
    {
        [ProtoMember(1)]
        public int? question_id { get; set; }
        [ProtoMember(2)]
        public int? answer_id { get; set; }
        [ProtoMember(3)]
        public DateTime? locked_date { get; set; }
        [ProtoMember(4)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(5)]
        public DateTime? last_edit_date { get; set; }
        [ProtoMember(6)]
        public DateTime? last_activity_date { get; set; }
        [ProtoMember(7)]
        public int? score { get; set; }
        [ProtoMember(8)]
        public DateTime? community_owned_date { get; set; }
        [ProtoMember(9)]
        public bool? is_accepted { get; set; }
        [ProtoMember(10)]
        public string body { get; set; }
        [ProtoMember(11)]
        public ShallowUser owner { get; set; }
        [ProtoMember(12)]
        public string title { get; set; }
        [ProtoMember(13)]
        public int? up_vote_count { get; set; }
        [ProtoMember(14)]
        public int? down_vote_count { get; set; }
        [ProtoMember(15)]
        public List<Comment> comments { get; set; }
        [ProtoMember(16)]
        public string link { get; set; }
        [ProtoMember(17)]
        public List<string> tags { get; set; }
        [ProtoMember(18)]
        public bool? upvoted { get; set; }
        [ProtoMember(19)]
        public bool? downvoted { get; set; }
        [ProtoMember(20)]
        public bool? accepted { get; set; }
        [ProtoMember(21)]
        public ShallowUser last_editor { get; set; }
        [ProtoMember(22)]
        public int? comment_count { get; set; }
        [ProtoMember(23)]
        public string body_markdown { get; set; }
        [ProtoMember(24)]
        public string share_link { get; set; }
    }
}
