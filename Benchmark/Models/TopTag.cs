using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class TopTag
    {
        [ProtoMember(1)]
        public string tag_name { get; set; }
        [ProtoMember(2)]
        public int? question_score { get; set; }
        [ProtoMember(3)]
        public int? question_count { get; set; }
        [ProtoMember(4)]
        public int? answer_score { get; set; }
        [ProtoMember(5)]
        public int? answer_count { get; set; }
        [ProtoMember(6)]
        public int? user_id { get; set; }
    }
}
