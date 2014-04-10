using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    public class User : IGenericEquality<User>
    {
        [ProtoContract]
        public class BadgeCount : IGenericEquality<BadgeCount>
        {
            [ProtoMember(1)]
            public int? gold { get; set; }
            [ProtoMember(2)]
            public int? silver { get; set; }
            [ProtoMember(3)]
            public int? bronze { get; set; }

            public bool Equals(BadgeCount obj)
            {
                return
                    this.bronze.TrueEquals(obj.bronze) &&
                    this.silver.TrueEquals(obj.silver) &&
                    this.gold.TrueEquals(obj.gold);
            }

            public bool EqualsDynamic(dynamic obj)
            {
                return
                    this.bronze.TrueEquals((int?)obj.bronze) &&
                    this.silver.TrueEquals((int?)obj.silver) &&
                    this.gold.TrueEquals((int?)obj.gold);
            }
        }

        [ProtoMember(1)]
        public int? user_id { get; set; }
        [ProtoMember(2)]
        public UserType? user_type { get; set; }
        [ProtoMember(3)]
        public DateTime? creation_date { get; set; }
        [ProtoMember(4)]
        public string display_name { get; set; }
        [ProtoMember(5)]
        public string profile_image { get; set; }
        [ProtoMember(6)]
        public int? reputation { get; set; }
        [ProtoMember(7)]
        public int? reputation_change_day { get; set; }
        [ProtoMember(8)]
        public int? reputation_change_week { get; set; }
        [ProtoMember(9)]
        public int? reputation_change_month { get; set; }
        [ProtoMember(10)]
        public int? reputation_change_quarter { get; set; }
        [ProtoMember(11)]
        public int? reputation_change_year { get; set; }
        [ProtoMember(12)]
        public int? age { get; set; }
        [ProtoMember(13)]
        public DateTime? last_access_date { get; set; }
        [ProtoMember(14)]
        public DateTime? last_modified_date { get; set; }
        [ProtoMember(15)]
        public bool? is_employee { get; set; }
        [ProtoMember(16)]
        public string link { get; set; }
        [ProtoMember(17)]
        public string website_url { get; set; }
        [ProtoMember(18)]
        public string location { get; set; }
        [ProtoMember(19)]
        public int? account_id { get; set; }
        [ProtoMember(20)]       
        public DateTime? timed_penalty_date { get; set; }
        [ProtoMember(21)]
        public BadgeCount badge_counts { get; set; }
        [ProtoMember(22)]       
        public int? question_count { get; set; }
        [ProtoMember(23)]
        public int? answer_count { get; set; }
        [ProtoMember(24)]
        public int? up_vote_count { get; set; }
        [ProtoMember(25)]
        public int? down_vote_count { get; set; }
        [ProtoMember(26)]
        public string about_me { get; set; }
        [ProtoMember(27)]
        public int? view_count { get; set; }
        [ProtoMember(28)]
        public int? accept_rate { get; set; }

        public bool Equals(User obj)
        {
            return
                this.about_me.TrueEqualsString(obj.about_me) &&
                this.accept_rate.TrueEquals(obj.accept_rate) &&
                this.account_id.TrueEquals(obj.account_id) &&
                this.age.TrueEquals(obj.age) &&
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.badge_counts.TrueEquals(obj.badge_counts) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.display_name.TrueEqualsString(obj.display_name) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.is_employee.TrueEquals(obj.is_employee) &&
                this.last_access_date.TrueEquals(obj.last_access_date) &&
                this.last_modified_date.TrueEquals(obj.last_modified_date) &&
                this.link.TrueEqualsString(obj.link) &&
                this.location.TrueEqualsString(obj.location) &&
                this.profile_image.TrueEqualsString(obj.profile_image) &&
                this.question_count.TrueEquals(obj.question_count) &&
                this.reputation.TrueEquals(obj.reputation) &&
                this.reputation_change_day.TrueEquals(obj.reputation_change_day) &&
                this.reputation_change_month.TrueEquals(obj.reputation_change_month) &&
                this.reputation_change_quarter.TrueEquals(obj.reputation_change_quarter) &&
                this.reputation_change_week.TrueEquals(obj.reputation_change_week) &&
                this.reputation_change_year.TrueEquals(obj.reputation_change_year) &&
                this.timed_penalty_date.TrueEquals(obj.timed_penalty_date) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.user_id.TrueEquals(obj.user_id) &&
                this.user_type.TrueEquals(obj.user_type) &&
                this.view_count.TrueEquals(obj.view_count) &&
                this.website_url.TrueEqualsString(obj.website_url);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.about_me.TrueEqualsString((string)obj.about_me) &&
                this.accept_rate.TrueEquals((int?)obj.accept_rate) &&
                this.account_id.TrueEquals((int?)obj.account_id) &&
                this.age.TrueEquals((int?)obj.age) &&
                this.answer_count.TrueEquals((int?)obj.answer_count) &&
                (this.badge_counts == null && obj.badge_counts == null || this.badge_counts.EqualsDynamic(obj.badge_counts)) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.display_name.TrueEqualsString((string)obj.display_name) &&
                this.down_vote_count.TrueEquals((int?)obj.down_vote_count) &&
                this.is_employee.TrueEquals((bool?)obj.is_employee) &&
                this.last_access_date.TrueEquals((DateTime?)obj.last_access_date) &&
                this.last_modified_date.TrueEquals((DateTime?)obj.last_modified_date) &&
                this.link.TrueEqualsString((string)obj.link) &&
                this.location.TrueEqualsString((string)obj.location) &&
                this.profile_image.TrueEqualsString((string)obj.profile_image) &&
                this.question_count.TrueEquals((int?)obj.question_count) &&
                this.reputation.TrueEquals((int?)obj.reputation) &&
                this.reputation_change_day.TrueEquals((int?)obj.reputation_change_day) &&
                this.reputation_change_month.TrueEquals((int?)obj.reputation_change_month) &&
                this.reputation_change_quarter.TrueEquals((int?)obj.reputation_change_quarter) &&
                this.reputation_change_week.TrueEquals((int?)obj.reputation_change_week) &&
                this.reputation_change_year.TrueEquals((int?)obj.reputation_change_year) &&
                this.timed_penalty_date.TrueEquals((DateTime?)obj.timed_penalty_date) &&
                this.up_vote_count.TrueEquals((int?)obj.up_vote_count) &&
                this.user_id.TrueEquals((int?)obj.user_id) &&
                this.user_type.TrueEquals((UserType?)obj.user_type) &&
                this.view_count.TrueEquals((int?)obj.view_count) &&
                this.website_url.TrueEqualsString((string)obj.website_url);
        }
    }
}
