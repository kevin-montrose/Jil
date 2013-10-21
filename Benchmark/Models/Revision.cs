using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum RevisionType : byte
    {
        single_user = 1,
        vote_based = 2
    }

    class Revision
    {
        public string revision_guid { get; set; }
        public int? revision_number { get; set; }
        public RevisionType? revision_type { get; set; }
        public PostType? post_type { get; set; }
        public int? post_id { get; set; }
        public string comment { get; set; }
        public DateTime? creation_date { get; set; }
        public bool? is_rollback { get; set; }
        public string last_body { get; set; }
        public string last_title { get; set; }
        public List<string> last_tags { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public List<string> tags { get; set; }
        public bool? set_community_wiki { get; set; }
        public ShallowUser user { get; set; }
    }
}
