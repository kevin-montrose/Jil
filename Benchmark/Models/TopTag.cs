using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class TopTag : IGenericEquality<TopTag>
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

        public bool Equals(TopTag obj)
        {
            return
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.answer_score.TrueEquals(obj.answer_score) &&
                this.question_count.TrueEquals(obj.question_count) &&
                this.question_score.TrueEquals(obj.question_score) &&
                this.tag_name.TrueEqualsString(obj.tag_name) &&
                this.user_id.TrueEquals(obj.user_id);
        }
    }
}
