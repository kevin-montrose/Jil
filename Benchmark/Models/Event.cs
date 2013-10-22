using ProtoBuf;
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

    [ProtoContract]
    class Event
    {
        [ProtoMember(1)]
        public EventType? event_type { get; set; }
        [ProtoMember(2)]
        public int? event_id { get; set; }
        [ProtoMember(3)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(4)]
        public string link { get; set; }
        [ProtoMember(5)]
        public string excerpt { get; set; }
    }
}
