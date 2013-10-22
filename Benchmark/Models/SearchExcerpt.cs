using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum SearchExcerptItemType : byte
    {
        question = 1,
        answer = 2
    }

    [ProtoContract]
    class SearchExcerpt
    {
        [ProtoMember(1)]
        public string title { get; set; }
        [ProtoMember(2)]
        public string excerpt { get; set; }
        [ProtoMember(3)]
        public DateTime? community_owned_date { get; set; }
        [ProtoMember(4)]
        public DateTime? locked_date { get; set; }
        [ProtoMember(5)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(6)]
        public DateTime? last_activity_date { get; set; }
        [ProtoMember(7)]
        public ShallowUser owner { get; set; }
        [ProtoMember(8)]
        public ShallowUser last_activity_user { get; set; }
        [ProtoMember(9)]
        public int? score { get; set; }
        [ProtoMember(10)]
        public SearchExcerptItemType? item_type { get; set; }
        [ProtoMember(11)]
        public string body { get; set; }
        [ProtoMember(12)]
        public int? question_id { get; set; }
        [ProtoMember(13)]
        public bool? is_answered { get; set; }
        [ProtoMember(14)]
        public int? answer_count { get; set; }
        [ProtoMember(15)]
        public List<string> tags { get; set; }
        [ProtoMember(16)]
        public DateTime? closed_date { get; set; }
        [ProtoMember(17)]
        public int? answer_id { get; set; }
        [ProtoMember(18)]
        public bool? is_accepted { get; set; }
    }
}
