using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum EventType : byte
    {
        question_posted = 1,
        answer_posted = 2,
        comment_posted = 3,
        post_edited = 4,
        user_created = 5
    }

    class Event
    {
        public EventType? event_type { get; set; }
        public int? event_id { get; set; }
        public DateTime? creation_date { get; set; }
        public string link { get; set; }
        public string excerpt { get; set; }
    }
}
