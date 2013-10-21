using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class Info
    {
        public class Site
        {
            public enum SiteState
            {
                normal,
                closed_beta,
                open_beta,
                linked_meta
            }

            public class Styling
            {
                public string link_color { get; set; }
                public string tag_foreground_color { get; set; }
                public string tag_background_color { get; set; }
            }

            public string site_type { get; set; }
            public string name { get; set; }
            public string logo_url { get; set; }
            public string api_site_parameter { get; set; }
            public string site_url { get; set; }
            public string audience { get; set; }
            public string icon_url { get; set; }
            public List<string> aliases { get; set; }
            public SiteState? site_state { get; set; }
            public Styling styling { get; set; }
            public DateTime? closed_beta_date { get; set; }
            public DateTime? open_beta_date { get; set; }
            public DateTime? launch_date { get; set; }
            public string favicon_url { get; set; }
            public List<RelatedSite> related_sites { get; set; }
            public string twitter_account { get; set; }
            public List<string> markdown_extensions { get; set; }
            public string high_resolution_icon_url { get; set; }
        }

        public class RelatedSite
        {
            public enum SiteRelation
            {
                parent,
                meta,
                chat
            }

            public string name { get; set; }
            public string site_url { get; set; }
            public SiteRelation? relation { get; set; }
            public string api_site_parameter { get; set; }
        }

        public int? total_questions { get; set; }
        public int? total_unanswered { get; set; }
        public int? total_accepted { get; set; }
        public int? total_answers { get; set; }
        public decimal? questions_per_minute { get; set; }
        public decimal? answers_per_minute { get; set; }
        public int? total_comments { get; set; }
        public int? total_votes { get; set; }
        public int? total_badges { get; set; }
        public decimal? badges_per_minute { get; set; }
        public int? total_users { get; set; }
        public int? new_active_users { get; set; }
        public string api_revision { get; set; }
        public Site site { get; set; }
    }
}
