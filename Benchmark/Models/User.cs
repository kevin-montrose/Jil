using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class User
    {
        public class BadgeCount
        {
            public int? gold { get; set; }
            public int? silver { get; set; }
            public int? bronze { get; set; }
        }

        public int? user_id { get; set; }
        public UserType? user_type { get; set; }
        public DateTime? creation_date { get; set; }
        public string display_name { get; set; }
        public string profile_image { get; set; }
        public int? reputation { get; set; }
        public int? reputation_change_day { get; set; }
        public int? reputation_change_week { get; set; }
        public int? reputation_change_month { get; set; }
        public int? reputation_change_quarter { get; set; }
        public int? reputation_change_year { get; set; }
        public int? age { get; set; }
        public DateTime? last_access_date { get; set; }
        public DateTime? last_modified_date { get; set; }
        public bool? is_employee { get; set; }
        public string link { get; set; }
        public string website_url { get; set; }
        public string location { get; set; }
        public int? account_id { get; set; }
        public DateTime? timed_penalty_date { get; set; }
        public BadgeCount badge_counts { get; set; }
        public int? question_count { get; set; }
        public int? answer_count { get; set; }
        public int? up_vote_count { get; set; }
        public int? down_vote_count { get; set; }
        public string about_me { get; set; }
        public int? view_count { get; set; }
        public int? accept_rate { get; set; }
    }
}
