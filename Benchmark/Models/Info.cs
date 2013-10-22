using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Info
    {
        [ProtoContract]
        public class Site
        {
            public enum SiteState
            {
                normal,
                closed_beta,
                open_beta,
                linked_meta
            }

            [ProtoContract]
            public class Styling
            {
                [ProtoMember(1)]
                public string link_color { get; set; }
                [ProtoMember(2)]
                public string tag_foreground_color { get; set; }
                [ProtoMember(3)]
                public string tag_background_color { get; set; }
            }

            [ProtoMember(1)]
            public string site_type { get; set; }
            [ProtoMember(2)]
            public string name { get; set; }
            [ProtoMember(3)]
            public string logo_url { get; set; }
            [ProtoMember(4)]
            public string api_site_parameter { get; set; }
            [ProtoMember(5)]
            public string site_url { get; set; }
            [ProtoMember(6)]
            public string audience { get; set; }
            [ProtoMember(7)]
            public string icon_url { get; set; }
            [ProtoMember(8)]
            public List<string> aliases { get; set; }
            [ProtoMember(9)]
            public SiteState? site_state { get; set; }
            [ProtoMember(10)]
            public Styling styling { get; set; }
            [ProtoMember(11)]
            public DateTime? closed_beta_date { get; set; }
            [ProtoMember(12)]
            public DateTime? open_beta_date { get; set; }
            [ProtoMember(13)]
            public DateTime? launch_date { get; set; }
            [ProtoMember(14)]
            public string favicon_url { get; set; }
            [ProtoMember(15)]
            public List<RelatedSite> related_sites { get; set; }
            [ProtoMember(16)]
            public string twitter_account { get; set; }
            [ProtoMember(17)]
            public List<string> markdown_extensions { get; set; }
            [ProtoMember(18)]
            public string high_resolution_icon_url { get; set; }
        }

        [ProtoContract]
        public class RelatedSite
        {
            public enum SiteRelation
            {
                parent,
                meta,
                chat
            }

            [ProtoMember(1)]
            public string name { get; set; }
            [ProtoMember(2)]
            public string site_url { get; set; }
            [ProtoMember(3)]
            public SiteRelation? relation { get; set; }
            [ProtoMember(4)]
            public string api_site_parameter { get; set; }
        }

        [ProtoMember(1)]
        public int? total_questions { get; set; }
        [ProtoMember(2)]
        public int? total_unanswered { get; set; }
        [ProtoMember(3)]
        public int? total_accepted { get; set; }
        [ProtoMember(4)]
        public int? total_answers { get; set; }
        [ProtoMember(5)]
        public decimal? questions_per_minute { get; set; }
        [ProtoMember(6)]
        public decimal? answers_per_minute { get; set; }
        [ProtoMember(7)]
        public int? total_comments { get; set; }
        [ProtoMember(8)]
        public int? total_votes { get; set; }
        [ProtoMember(9)]
        public int? total_badges { get; set; }
        [ProtoMember(10)]
        public decimal? badges_per_minute { get; set; }
        [ProtoMember(11)]
        public int? total_users { get; set; }
        [ProtoMember(12)]
        public int? new_active_users { get; set; }
        [ProtoMember(13)]
        public string api_revision { get; set; }
        [ProtoMember(14)]
        public Site site { get; set; }
    }
}
