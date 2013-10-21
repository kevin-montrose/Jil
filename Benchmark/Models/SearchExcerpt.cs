using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum SearchExcerptItemType : byte
    {
        question = 1,
        answer = 2
    }

    class SearchExcerpt
    {
        public string title { get; set; }
        public string excerpt { get; set; }
        public DateTime? community_owned_date { get; set; }
        public DateTime? locked_date { get; set; }
        public DateTime? creation_date { get; set; }
        public DateTime? last_activity_date { get; set; }
        public ShallowUser owner { get; set; }
        public ShallowUser last_activity_user { get; set; }
        public int? score { get; set; }
        public SearchExcerptItemType? item_type { get; set; }
        public string body { get; set; }
        public int? question_id { get; set; }
        public bool? is_answered { get; set; }
        public int? answer_count { get; set; }
        public List<string> tags { get; set; }
        public DateTime? closed_date { get; set; }
        public int? answer_id { get; set; }
        public bool? is_accepted { get; set; }
    }
}
