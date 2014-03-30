using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Post : IGenericEquality<Post>
    {
        [ProtoMember(1)]
        public int? post_id { get; set; }
        [ProtoMember(2)]
        public PostType? post_type { get; set; }
        [ProtoMember(3)]
        public string body { get; set; }
        [ProtoMember(4)]
        public ShallowUser owner { get; set; }
        [ProtoMember(5)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(6)]
        public DateTime? last_activity_date { get; set; }
        [ProtoMember(7)]
        public DateTime? last_edit_date { get; set; }
        [ProtoMember(8)]
        public int? score { get; set; }
        [ProtoMember(9)]
        public int? up_vote_count { get; set; }
        [ProtoMember(10)]
        public int? down_vote_count { get; set; }
        [ProtoMember(11)]
        public List<Comment> comments { get; set; }
        [ProtoMember(12)]
        public string link { get; set; }
        [ProtoMember(13)]
        public bool? upvoted { get; set; }
        [ProtoMember(14)]
        public bool? downvoted { get; set; }
        [ProtoMember(15)]
        public ShallowUser last_editor { get; set; }
        [ProtoMember(16)]
        public int? comment_count { get; set; }
        [ProtoMember(17)]
        public string body_markdown { get; set; }
        [ProtoMember(18)]
        public string share_link { get; set; }

        public bool Equals(Post obj)
        {
            return
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.owner.TrueEquals(obj.owner) &&
                this.post_id.TrueEquals(obj.post_id) &&
                this.post_type.TrueEquals(obj.post_type) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.body.TrueEqualsString((string)obj.body) &&
                this.body_markdown.TrueEqualsString((string)obj.body_markdown) &&
                this.comment_count.TrueEquals((int?)obj.comment_count) &&
                this.comments.TrueEqualsListDynamic((IEnumerable<dynamic>)obj.comments) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.down_vote_count.TrueEquals((int?)obj.down_vote_count) &&
                this.downvoted.TrueEquals((bool?)obj.downvoted) &&
                this.last_activity_date.TrueEquals((DateTime?)obj.last_activity_date) &&
                this.last_edit_date.TrueEquals((DateTime?)obj.last_edit_date) &&
                (this.last_editor == null && obj.last_editor == null || this.last_editor.EqualsDynamic(obj.last_editor)) &&
                this.link.TrueEqualsString((string)obj.link) &&
                (this.owner == null && obj.owner == null || this.owner.EqualsDynamic(obj.owner)) &&
                this.post_id.TrueEquals((int?)obj.post_id) &&
                this.post_type.TrueEquals((PostType?)obj.post_type) &&
                this.score.TrueEquals((int?)obj.score) &&
                this.share_link.TrueEqualsString((string)obj.share_link) &&
                this.up_vote_count.TrueEquals((int?)obj.up_vote_count) &&
                this.upvoted.TrueEquals((bool?)obj.upvoted);
        }
    }
}
