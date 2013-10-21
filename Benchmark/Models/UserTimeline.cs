using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum UserTimelineType : byte
    {
        commented = 1,
        asked = 2,
        answered = 3,
        badge = 4,
        revision = 5,
        accepted = 6,
        reviewed = 7,
        suggested = 8
    }

    class UserTimeline
    {
        public DateTime? creation_date { get; set; }
        public PostType? post_type { get; set; }
        public UserTimelineType? timeline_type { get; set; }
        public int? user_id { get; set; }
        public int? post_id { get; set; }
        public int? comment_id { get; set; }
        public int? suggested_edit_id { get; set; }
        public int? badge_id { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
        public string link { get; set; }
    }
}
