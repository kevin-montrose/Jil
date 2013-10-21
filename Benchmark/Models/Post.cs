using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class Post
    {
        public int? post_id { get; set; }
        public PostType? post_type { get; set; }
        public string body { get; set; }
        public ShallowUser owner { get; set; }
        public DateTime? creation_date { get; set; }
        public DateTime? last_activity_date { get; set; }
        public DateTime? last_edit_date { get; set; }
        public int? score { get; set; }
        public int? up_vote_count { get; set; }
        public int? down_vote_count { get; set; }
        public List<Comment> comments { get; set; }
        public string link { get; set; }
        public bool? upvoted { get; set; }
        public bool? downvoted { get; set; }
        public ShallowUser last_editor { get; set; }
        public int? comment_count { get; set; }
        public string body_markdown { get; set; }
        public string share_link { get; set; }
    }
}
