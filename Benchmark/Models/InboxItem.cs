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
    class InboxItem
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
    }
}
