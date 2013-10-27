using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Info : IGenericEquality<Info>
    {
        [ProtoContract]
        public class Site : IGenericEquality<Site>
        {
            public enum SiteState
            {
                normal,
                closed_beta,
                open_beta,
                linked_meta
            }

            [ProtoContract]
            public class Styling : IGenericEquality<Styling>
            {
                [ProtoMember(1)]
                public string link_color { get; set; }
                [ProtoMember(2)]
                public string tag_foreground_color { get; set; }
                [ProtoMember(3)]
                public string tag_background_color { get; set; }

                public bool Equals(Styling obj)
                {
                    return
                        this.link_color.TrueEqualsString(obj.link_color) &&
                        this.tag_background_color.TrueEqualsString(obj.tag_background_color) &&
                        this.tag_foreground_color.TrueEqualsString(obj.tag_foreground_color);
                }
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

            public bool Equals(Site obj)
            {
                return
                    this.aliases.TrueEqualsString(obj.aliases) &&
                    this.api_site_parameter.TrueEqualsString(obj.api_site_parameter) &&
                    this.audience.TrueEqualsString(obj.audience) &&
                    this.closed_beta_date.TrueEquals(obj.closed_beta_date) &&
                    this.favicon_url.TrueEqualsString(obj.favicon_url) &&
                    this.high_resolution_icon_url.TrueEqualsString(obj.high_resolution_icon_url) &&
                    this.icon_url.TrueEqualsString(obj.icon_url) &&
                    this.launch_date.TrueEquals(obj.launch_date) &&
                    this.logo_url.TrueEqualsString(obj.logo_url) &&
                    this.markdown_extensions.TrueEqualsString(obj.markdown_extensions) &&
                    this.name.TrueEqualsString(obj.name) &&
                    this.open_beta_date.TrueEquals(obj.open_beta_date) &&
                    this.related_sites.TrueEqualsList(obj.related_sites) &&
                    this.site_state.TrueEquals(obj.site_state) &&
                    this.site_type.TrueEqualsString(obj.site_type) &&
                    this.site_url.TrueEqualsString(obj.site_url) &&
                    this.styling.TrueEquals(obj.styling) &&
                    this.twitter_account.TrueEqualsString(obj.twitter_account);
            }
        }

        [ProtoContract]
        public class RelatedSite : IGenericEquality<RelatedSite>
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

            public bool Equals(RelatedSite obj)
            {
                return
                    this.name.TrueEqualsString(obj.name) &&
                    this.relation.TrueEquals(obj.relation) &&
                    this.api_site_parameter.TrueEqualsString(obj.api_site_parameter);
            }
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

        public bool Equals(Info obj)
        {
            return
                this.answers_per_minute.TrueEquals(obj.answers_per_minute) &&
                this.api_revision.TrueEqualsString(obj.api_revision) &&
                this.badges_per_minute.TrueEquals(obj.badges_per_minute) &&
                this.new_active_users.TrueEquals(obj.new_active_users) &&
                this.questions_per_minute.TrueEquals(obj.questions_per_minute) &&
                this.site.TrueEquals(obj.site) &&
                this.total_accepted.TrueEquals(obj.total_accepted) &&
                this.total_answers.TrueEquals(obj.total_answers) &&
                this.total_badges.TrueEquals(obj.total_badges) &&
                this.total_comments.TrueEquals(obj.total_comments) &&
                this.total_questions.TrueEquals(obj.total_questions) &&
                this.total_unanswered.TrueEquals(obj.total_unanswered) &&
                this.total_users.TrueEquals(obj.total_users) &&
                this.total_votes.TrueEquals(obj.total_votes);
        }
    }
}
