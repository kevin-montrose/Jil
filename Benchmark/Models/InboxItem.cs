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

    class InboxItem
    {
        public InboxItemType? item_type { get; set; }
        public int? question_id { get; set; }
        public int? answer_id { get; set; }
        public int? comment_id { get; set; }
        public string title { get; set; }
        public DateTime? creation_date { get; set; }
        public bool? is_unread { get; set; }
        public Info.Site site { get; set; }
        public string body { get; set; }
        public string link { get; set; }
    }
}
