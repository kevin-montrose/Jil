using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Answer : IGenericEquality<Answer>
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

        public bool Equals(Answer obj)
        {
            return
                this.accepted.TrueEquals(obj.accepted) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted);
        }
    }
}
