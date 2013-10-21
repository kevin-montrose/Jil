using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum PostType : byte
    {
        question = 1,
        answer = 2
    }

    class Comment
    {
        public int? comment_id { get; set; }
        public int? post_id { get; set; }
        public DateTime? creation_date { get; set; }
        public PostType? post_type { get; set; }
        public int? score { get; set; }
        public bool? edited { get; set; }
        public string body { get; set; }
        public ShallowUser owner { get; set; }
        public ShallowUser reply_to_user { get; set; }
        public string link { get; set; }
        public string body_markdown { get; set; }
        public bool? upvoted { get; set; }
    }
}
