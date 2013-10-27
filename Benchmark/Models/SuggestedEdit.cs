using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class SuggestedEdit : IGenericEquality<SuggestedEdit>
    {
        [ProtoMember(1)]
        public int? suggested_edit_id { get; set; }
        [ProtoMember(2)]
        public int? post_id { get; set; }
        [ProtoMember(3)]
        public PostType? post_type { get; set; }
        [ProtoMember(4)]
        public string body { get; set; }
        [ProtoMember(5)]
        public string title { get; set; }
        [ProtoMember(6)]
        public List<string> tags { get; set; }
        [ProtoMember(7)]
        public string comment { get; set; }
        [ProtoMember(8)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(9)]
        public DateTime? approval_date { get; set; }
        [ProtoMember(10)]
        public DateTime? rejection_date { get; set; }
        [ProtoMember(11)]
        public ShallowUser proposing_user { get; set; }

        public bool Equals(SuggestedEdit obj)
        {
            return
                this.approval_date.TrueEquals(obj.approval_date) &&
                this.body.TrueEqualsString(obj.body) &&
                this.comment.TrueEqualsString(obj.comment) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.post_id.TrueEquals(obj.post_id) &&
                this.post_type.TrueEquals(obj.post_type) &&
                this.proposing_user.TrueEquals(obj.proposing_user) &&
                this.rejection_date.TrueEquals(obj.rejection_date) &&
                this.suggested_edit_id.TrueEquals(obj.suggested_edit_id) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title);
        }
    }
}
