using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum InboxItemType
    {
        comment = 1,
        chat_message = 2,
        new_answer = 3,
        careers_message = 4,
        careers_invitations = 5,
        meta_question = 6,
        post_notice = 7,
        moderator_message = 8
    }

    [ProtoContract]
    class InboxItem : IGenericEquality<InboxItem>
    {
        [ProtoMember(1)]
        public InboxItemType? item_type { get; set; }
        [ProtoMember(2)]
        public int? question_id { get; set; }
        [ProtoMember(3)]
        public int? answer_id { get; set; }
        [ProtoMember(4)]
        public int? comment_id { get; set; }
        [ProtoMember(5)]
        public string title { get; set; }
        [ProtoMember(6)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(7)]
        public bool? is_unread { get; set; }
        [ProtoMember(8)]
        public Info.Site site { get; set; }
        [ProtoMember(9)]
        public string body { get; set; }
        [ProtoMember(10)]
        public string link { get; set; }

        public bool Equals(InboxItem obj)
        {
            return
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.comment_id.TrueEquals(obj.comment_id) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.is_unread.TrueEquals(obj.is_unread) &&
                this.item_type.TrueEquals(obj.item_type) &&
                this.link.TrueEqualsString(obj.link) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.site.TrueEquals(obj.site) &&
                this.title.TrueEqualsString(obj.title);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.answer_id.TrueEquals((int?)obj.answer_id) &&
                this.body.TrueEqualsString((string)obj.body) &&
                this.comment_id.TrueEquals((int?)obj.comment_id) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.is_unread.TrueEquals((bool?)obj.is_unread) &&
                this.item_type.TrueEquals((InboxItemType?)obj.item_type) &&
                this.link.TrueEqualsString((string)obj.link) &&
                this.question_id.TrueEquals((int?)obj.question_id) &&
                (this.site == null && obj.site == null || this.site.EqualsDynamic(obj.site)) &&
                this.title.TrueEqualsString((string)obj.title);
        }
    }
}
