using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum PostType : byte
    {
        question = 1,
        answer = 2
    }

    [ProtoContract]
    class Comment
    {
        [ProtoMember(1)]
        public int? comment_id { get; set; }
        [ProtoMember(2)]
        public int? post_id { get; set; }
        [ProtoMember(3)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(4)]
        public PostType? post_type { get; set; }
        [ProtoMember(5)]
        public int? score { get; set; }
        [ProtoMember(6)]
        public bool? edited { get; set; }
        [ProtoMember(7)]
        public string body { get; set; }
        [ProtoMember(8)]
        public ShallowUser owner { get; set; }
        [ProtoMember(9)]
        public ShallowUser reply_to_user { get; set; }
        [ProtoMember(10)]
        public string link { get; set; }
        [ProtoMember(11)]
        public string body_markdown { get; set; }
        [ProtoMember(12)]
        public bool? upvoted { get; set; }
    }
}
