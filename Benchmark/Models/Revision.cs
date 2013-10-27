using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum RevisionType : byte
    {
        single_user = 1,
        vote_based = 2
    }

    [ProtoContract]
    class Revision : IGenericEquality<Revision>
    {
        [ProtoMember(1)]
        public string revision_guid { get; set; }
        [ProtoMember(2)]
        public int? revision_number { get; set; }
        [ProtoMember(3)]
        public RevisionType? revision_type { get; set; }
        [ProtoMember(4)]
        public PostType? post_type { get; set; }
        [ProtoMember(5)]
        public int? post_id { get; set; }
        [ProtoMember(6)]
        public string comment { get; set; }
        [ProtoMember(7)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(8)]
        public bool? is_rollback { get; set; }
        [ProtoMember(9)]
        public string last_body { get; set; }
        [ProtoMember(10)]
        public string last_title { get; set; }
        [ProtoMember(11)]
        public List<string> last_tags { get; set; }
        [ProtoMember(12)]
        public string body { get; set; }
        [ProtoMember(13)]
        public string title { get; set; }
        [ProtoMember(14)]
        public List<string> tags { get; set; }
        [ProtoMember(15)]
        public bool? set_community_wiki { get; set; }
        [ProtoMember(16)]
        public ShallowUser user { get; set; }

        public bool Equals(Revision obj)
        {
            return
                this.body.TrueEqualsString(obj.body) &&
                this.comment.TrueEqualsString(obj.comment) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.is_rollback.TrueEquals(obj.is_rollback) &&
                this.last_body.TrueEqualsString(obj.last_body) &&
                this.last_tags.TrueEqualsString(obj.last_tags) &&
                this.last_title.TrueEqualsString(obj.last_title) &&
                this.post_id.TrueEquals(obj.post_id) &&
                this.post_type.TrueEquals(obj.post_type) &&
                this.revision_guid.TrueEqualsString(obj.revision_guid) &&
                this.revision_number.TrueEquals(obj.revision_number) &&
                this.revision_type.TrueEquals(obj.revision_type) &&
                this.set_community_wiki.TrueEquals(obj.set_community_wiki) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.user.TrueEquals(obj.user);
        }
    }
}
