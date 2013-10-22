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
    class Revision
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
    }
}
