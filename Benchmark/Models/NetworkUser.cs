using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class NetworkUser
    {
        public string site_name { get; set; }
        public string site_url { get; set; }
        public int? user_id { get; set; }
        public int? reputation { get; set; }
        public int? account_id { get; set; }
        public DateTime? creation_date { get; set; }
        public UserType? user_type { get; set; }
        public User.BadgeCount badge_counts { get; set; }
        public DateTime? last_access_date { get; set; }
        public int? answer_count { get; set; }
        public int? question_count { get; set; }
    }
}
