using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class SuggestedEdit
    {
        public int? suggested_edit_id { get; set; }
        public int? post_id { get; set; }
        public PostType? post_type { get; set; }
        private int? _owner_user_id { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public List<string> tags { get; set; }
        public string comment { get; set; }
        public DateTime? creation_date { get; set; }
        public DateTime? approval_date { get; set; }
        public DateTime? rejection_date { get; set; }
        public ShallowUser proposing_user { get; set; }
    }
}
