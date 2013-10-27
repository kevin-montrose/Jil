using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Question : IGenericEquality<Question>
    {
        [ProtoContract]
        public class ClosedDetails : IGenericEquality<ClosedDetails>
        {
            [ProtoContract]
            public class OriginalQuestion : IGenericEquality<OriginalQuestion>
            {
                [ProtoMember(1)]
                public int? question_id { get; set; }
                [ProtoMember(2)]
                public string title { get; set; }
                [ProtoMember(3)]
                public int? answer_count { get; set; }
                [ProtoMember(4)]
                public int? accepted_answer_id { get; set; }

                public bool Equals(OriginalQuestion obj)
                {
                    return
                        this.accepted_answer_id.TrueEquals(obj.accepted_answer_id) &&
                        this.answer_count.TrueEquals(obj.answer_count) &&
                        this.question_id.TrueEquals(obj.question_id) &&
                        this.title.TrueEqualsString(obj.title);
                }
            }

            [ProtoMember(1)]
            public bool? on_hold { get; set; }
            [ProtoMember(2)]
            public string reason { get; set; }
            [ProtoMember(3)]
            public string description { get; set; }
            [ProtoMember(4)]
            public List<ShallowUser> by_users { get; set; }
            [ProtoMember(5)]
            public List<OriginalQuestion> original_questions { get; set; }

            public bool Equals(ClosedDetails obj)
            {
                return
                    this.by_users.TrueEqualsList(obj.by_users) &&
                    this.description.TrueEqualsString(obj.description) &&
                    this.on_hold.TrueEquals(obj.on_hold) &&
                    this.original_questions.TrueEqualsList(obj.original_questions) &&
                    this.reason.TrueEqualsString(obj.reason);
            }
        }

        [ProtoContract]
        public class Notice : IGenericEquality<Notice>
        {
            [ProtoMember(1)]
            public string body { get; set; }
            [ProtoMember(2)]
            public DateTime? creation_date { get; set; }
            [ProtoMember(3)]
            public int? owner_user_id { get; set; }

            public bool Equals(Notice obj)
            {
                return
                    this.body.TrueEqualsString(obj.body) &&
                    this.creation_date.TrueEquals(obj.creation_date) &&
                    this.owner_user_id.TrueEquals(obj.owner_user_id);
            }
        }

        [ProtoContract]
        public class MigrationInfo : IGenericEquality<MigrationInfo>
        {
            [ProtoMember(1)]
            public int? question_id { get; set; }
            [ProtoMember(2)]
            public Info.Site other_site { get; set; }
            [ProtoMember(3)]
            public DateTime? on_date { get; set; }

            public bool Equals(MigrationInfo obj)
            {
                return
                    this.on_date.TrueEquals(obj.on_date) &&
                    this.other_site.TrueEquals(obj.other_site) &&
                    this.question_id.TrueEquals(obj.question_id);
            }
        }

        [ProtoMember(1)]
        public int? question_id { get; set; }
        [ProtoMember(2)]
        public DateTime? last_edit_date { get; set; }
        [ProtoMember(3)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(4)]
        public DateTime? last_activity_date { get; set; }
        [ProtoMember(5)]
        public DateTime? locked_date { get; set; }
        [ProtoMember(6)]
        public int? score { get; set; }
        [ProtoMember(7)]
        public DateTime? community_owned_date { get; set; }
        [ProtoMember(8)]
        public int? answer_count { get; set; }
        [ProtoMember(9)]
        public int? accepted_answer_id { get; set; }
        [ProtoMember(10)]
        public MigrationInfo migrated_to { get; set; }
        [ProtoMember(11)]
        public MigrationInfo migrated_from { get; set; }
        [ProtoMember(12)]
        public DateTime? bounty_closes_date { get; set; }
        [ProtoMember(13)]
        public int? bounty_amount { get; set; }
        [ProtoMember(14)]
        public DateTime? closed_date { get; set; }
        [ProtoMember(15)]
        public DateTime? protected_date { get; set; }
        [ProtoMember(16)]
        public string body { get; set; }
        [ProtoMember(17)]
        public string title { get; set; }
        [ProtoMember(18)]
        public List<string> tags { get; set; }
        [ProtoMember(19)]
        public string closed_reason { get; set; }
        [ProtoMember(20)]
        public int? up_vote_count { get; set; }
        [ProtoMember(21)]
        public int? down_vote_count { get; set; }
        [ProtoMember(22)]
        public int? favorite_count { get; set; }
        [ProtoMember(23)]
        public int? view_count { get; set; }
        [ProtoMember(24)]
        public ShallowUser owner { get; set; }
        [ProtoMember(25)]
        public List<Comment> comments { get; set; }
        [ProtoMember(26)]
        public List<Answer> answers { get; set; }
        [ProtoMember(27)]
        public string link { get; set; }
        [ProtoMember(28)]
        public bool? is_answered { get; set; }
        [ProtoMember(29)]
        public int? close_vote_count { get; set; }
        [ProtoMember(30)]
        public int? reopen_vote_count { get; set; }
        [ProtoMember(31)]
        public int? delete_vote_count { get; set; }
        [ProtoMember(32)]
        public Notice notice { get; set; }
        [ProtoMember(33)]
        public bool? upvoted { get; set; }
        [ProtoMember(34)]
        public bool? downvoted { get; set; }
        [ProtoMember(35)]
        public bool? favorited { get; set; }
        [ProtoMember(36)]
        public ShallowUser last_editor { get; set; }
        [ProtoMember(37)]
        public int? comment_count { get; set; }
        [ProtoMember(38)]
        public string body_markdown { get; set; }
        [ProtoMember(39)]
        public ClosedDetails closed_details { get; set; }
        [ProtoMember(40)]
        public string share_link { get; set; }

        public bool Equals(Question obj)
        {
            return
                this.accepted_answer_id.TrueEquals(obj.accepted_answer_id) &&
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.answers.TrueEqualsList(obj.answers) &&
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.bounty_amount.TrueEquals(obj.bounty_amount) &&
                this.bounty_closes_date.TrueEquals(obj.bounty_closes_date) &&
                this.close_vote_count.TrueEquals(obj.close_vote_count) &&
                this.closed_date.TrueEquals(obj.closed_date) &&
                this.closed_details.TrueEquals(obj.closed_details) &&
                this.closed_reason.TrueEqualsString(obj.closed_reason) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.delete_vote_count.TrueEquals(obj.delete_vote_count) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.favorite_count.TrueEquals(obj.favorite_count) &&
                this.favorited.TrueEquals(obj.favorited) &&
                this.is_answered.TrueEquals(obj.is_answered) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.migrated_from.TrueEquals(obj.migrated_from) &&
                this.migrated_to.TrueEquals(obj.migrated_to) &&
                this.notice.TrueEquals(obj.notice) &&
                this.owner.TrueEquals(obj.owner) &&
                this.protected_date.TrueEquals(obj.protected_date) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.reopen_vote_count.TrueEquals(obj.reopen_vote_count) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted) &&
                this.view_count.TrueEquals(obj.view_count);
        }
    }
}
