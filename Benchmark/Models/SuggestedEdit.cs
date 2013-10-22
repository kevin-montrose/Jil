using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class SuggestedEdit
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
    }
}
