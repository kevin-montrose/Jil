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
    class SearchExcerpt : IGenericEquality<SearchExcerpt>
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

        public bool Equals(SearchExcerpt obj)
        {
            return
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.closed_date.TrueEquals(obj.closed_date) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.excerpt.TrueEqualsString(obj.excerpt) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.is_answered.TrueEquals(obj.is_answered) &&
                this.item_type.TrueEquals(obj.item_type) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_activity_user.TrueEquals(obj.last_activity_user) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title);
        }
    }
}
