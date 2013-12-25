using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    public class MobileFeed : IGenericEquality<MobileFeed>
    {
        [ProtoMember(1)]
        public List<MobileQuestion> hot_questions { get; set; }
        [ProtoMember(2)]
        public List<MobileInboxItem> inbox_items { get; set; }
        [ProtoMember(3)]
        public List<MobileQuestion> likely_to_answer_questions { get; set; }
        [ProtoMember(4)]
        public List<MobileRepChange> reputation_events { get; set; }
        [ProtoMember(5)]
        public List<MobileQuestion> cross_site_interesting_questions { get; set; }
        [ProtoMember(6)]
        public List<MobileBadgeAward> badges { get; set; }
        [ProtoMember(7)]
        public List<MobilePrivilege> earned_privileges { get; set; }
        [ProtoMember(8)]
        public List<MobilePrivilege> upcoming_privileges { get; set; }
        [ProtoMember(9)]
        public List<MobileCommunityBulletin> community_bulletins { get; set; }
        [ProtoMember(10)]
        public List<MobileAssociationBonus> association_bonuses { get; set; }
        [ProtoMember(11)]
        public List<MobileCareersJobAd> careers_job_ads { get; set; }
        [ProtoMember(12)]
        public List<MobileBannerAd> banner_ads { get; set; }

        [ProtoMember(13)]
        public long? before { get; set; }
        [ProtoMember(14)]
        public long? since { get; set; }

        [ProtoMember(15)]
        public int? account_id { get; set; }

        [ProtoMember(16)]
        public MobileUpdateNotice update_notice { get; set; }

        public bool Equals(MobileFeed obj)
        {
            return
                this.account_id == obj.account_id &&
                this.association_bonuses.TrueEqualsList(obj.association_bonuses) &&
                this.badges.TrueEqualsList(obj.badges) &&
                this.banner_ads.TrueEqualsList(obj.banner_ads) &&
                this.before == obj.before &&
                this.careers_job_ads.TrueEqualsList(obj.careers_job_ads) &&
                this.community_bulletins.TrueEqualsList(obj.community_bulletins) &&
                this.cross_site_interesting_questions.TrueEqualsList(obj.cross_site_interesting_questions) &&
                this.earned_privileges.TrueEqualsList(obj.earned_privileges) &&
                this.hot_questions.TrueEqualsList(obj.hot_questions) &&
                this.inbox_items.TrueEqualsList(obj.inbox_items) &&
                this.likely_to_answer_questions.TrueEqualsList(obj.likely_to_answer_questions) &&
                this.reputation_events.TrueEqualsList(obj.reputation_events) &&
                this.since == obj.since &&
                this.upcoming_privileges.TrueEqualsList(obj.upcoming_privileges) &&
                this.update_notice.TrueEquals(obj.update_notice);
        }
    }

    interface IMobileFeedBase<T> : IGenericEquality<T>
    {
        int? group_id { get; set; }
        long? added_date { get; set; }
    }

    [ProtoContract]
    public sealed class MobileQuestion : IMobileFeedBase<MobileQuestion>
    {
        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public int? question_id { get; set; }
        [ProtoMember(4)]
        public long? question_creation_date { get; set; }
        [ProtoMember(5)]
        public string title { get; set; }
        [ProtoMember(6)]
        public long? last_activity_date { get; set; }
        [ProtoMember(7)]
        public List<string> tags { get; set; }
        [ProtoMember(8)]
        public string site { get; set; }

        [ProtoMember(9)]
        public bool? is_deleted { get; set; }
        [ProtoMember(10)]
        public bool? has_accepted_answer { get; set; }
        [ProtoMember(11)]
        public int? answer_count { get; set; }

        public bool Equals(MobileQuestion obj)
        {
            return
                this.added_date == obj.added_date &&
                this.answer_count == obj.answer_count &&
                this.group_id == obj.group_id &&
                this.has_accepted_answer == obj.has_accepted_answer &&
                this.is_deleted == obj.is_deleted &&
                this.last_activity_date == obj.last_activity_date &&
                this.question_creation_date == obj.question_creation_date &&
                this.question_id == obj.question_id &&
                this.site == obj.site &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title == obj.title;
        }
    }

    [ProtoContract]
    public sealed class MobileRepChange : IMobileFeedBase<MobileRepChange>
    {
        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public string site { get; set; }

        [ProtoMember(4)]
        public string title { get; set; }
        [ProtoMember(5)]
        public string link { get; set; }
        [ProtoMember(6)]
        public int? rep_change { get; set; }

        public bool Equals(MobileRepChange obj)
        {
            return
                this.added_date == obj.added_date &&
                this.group_id == obj.group_id &&
                this.link == obj.link &&
                this.rep_change == obj.rep_change &&
                this.site == obj.site &&
                this.title == obj.title;
        }
    }

    [ProtoContract]
    public sealed class MobileInboxItem : IMobileFeedBase<MobileInboxItem>
    {
        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public int? answer_id { get; set; }
        [ProtoMember(4)]
        public string body { get; set; }
        [ProtoMember(5)]
        public int? comment_id { get; set; }
        [ProtoMember(6)]
        public long? creation_date { get; set; }
        [ProtoMember(7)]
        public string item_type { get; set; }
        [ProtoMember(8)]
        public string link { get; set; }
        [ProtoMember(9)]
        public int? question_id { get; set; }
        [ProtoMember(10)]
        public string title { get; set; }
        [ProtoMember(11)]
        public string site { get; set; }

        public bool Equals(MobileInboxItem obj)
        {
            return
                this.added_date == obj.added_date &&
                this.answer_id == obj.answer_id &&
                this.body == obj.body &&
                this.comment_id == obj.comment_id &&
                this.creation_date == obj.creation_date &&
                this.group_id == obj.group_id &&
                this.item_type == obj.item_type &&
                this.link == obj.link &&
                this.question_id == obj.question_id &&
                this.site == obj.site &&
                this.title == obj.title;
        }
    }

    [ProtoContract]
    public sealed class MobileBadgeAward : IMobileFeedBase<MobileBadgeAward>
    {
        public enum BadgeRank : byte
        {
            bronze = 1,
            silver = 2,
            gold = 3
        }

        public enum BadgeType
        {
            named = 1,
            tag_based = 2
        }

        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public string site { get; set; }
        [ProtoMember(4)]
        public string badge_name { get; set; }
        [ProtoMember(5)]
        public string badge_description { get; set; }
        [ProtoMember(6)]
        public int badge_id { get; set; }

        [ProtoMember(7)]
        public int? post_id { get; set; }
        [ProtoMember(8)]
        public string link { get; set; }

        [ProtoMember(9)]
        public BadgeRank? rank { get; set; }
        [ProtoMember(10)]
        public BadgeType? badge_type { get; set; }

        public bool Equals(MobileBadgeAward obj)
        {
            return
                this.added_date == obj.added_date &&
                this.badge_description == obj.badge_description &&
                this.badge_id == obj.badge_id &&
                this.badge_name == obj.badge_name &&
                this.badge_type == obj.badge_type &&
                this.group_id == obj.group_id &&
                this.link == obj.link &&
                this.post_id == obj.post_id &&
                this.rank == obj.rank &&
                this.site == obj.site;
        }
    }

    [ProtoContract]
    public sealed class MobilePrivilege : IMobileFeedBase<MobilePrivilege>
    {
        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public string site { get; set; }
        [ProtoMember(4)]
        public string privilege_short_description { get; set; }
        [ProtoMember(5)]
        public string privilege_long_description { get; set; }
        [ProtoMember(6)]
        public int? privilege_id { get; set; }

        [ProtoMember(7)]
        public int? reputation_required { get; set; }
        [ProtoMember(8)]
        public string link { get; set; }

        public bool Equals(MobilePrivilege obj)
        {
            return
                this.added_date == obj.added_date &&
                this.group_id == obj.group_id &&
                this.link == obj.link &&
                this.privilege_id == obj.privilege_id &&
                this.privilege_long_description == obj.privilege_long_description &&
                this.privilege_short_description == obj.privilege_short_description &&
                this.reputation_required == obj.reputation_required &&
                this.site == obj.site;
        }
    }

    [ProtoContract]
    public sealed class MobileCommunityBulletin : IMobileFeedBase<MobileCommunityBulletin>
    {
        public enum CommunityBulletinType : byte
        {
            blog_post = 1,
            featured_meta_question = 2,
            upcoming_event = 3
        }

        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public string site { get; set; }

        [ProtoMember(4)]
        public string title { get; set; }
        [ProtoMember(5)]
        public string link { get; set; }

        [ProtoMember(6)]
        public CommunityBulletinType? bulletin_type { get; set; }

        [ProtoMember(7)]
        public long? begin_date { get; set; }
        [ProtoMember(8)]
        public long? end_date { get; set; }
        [ProtoMember(9)]
        public string custom_date_string { get; set; }

        [ProtoMember(10)]
        public List<string> tags { get; set; }
        [ProtoMember(11)]
        public bool? is_deleted { get; set; }
        [ProtoMember(12)]
        public bool? has_accepted_answer { get; set; }
        [ProtoMember(13)]
        public int? answer_count { get; set; }

        [ProtoMember(14)]
        public bool? is_promoted { get; set; }

        public bool Equals(MobileCommunityBulletin obj)
        {
            return
                this.added_date == obj.added_date &&
                this.answer_count == obj.answer_count &&
                this.begin_date == obj.begin_date &&
                this.bulletin_type == obj.bulletin_type &&
                this.custom_date_string == obj.custom_date_string &&
                this.end_date == obj.end_date &&
                this.group_id == obj.group_id &&
                this.has_accepted_answer == obj.has_accepted_answer &&
                this.is_deleted == obj.is_deleted &&
                this.is_promoted == obj.is_promoted &&
                this.link == obj.link &&
                this.site == obj.site &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title == obj.title;
        }
    }

    [ProtoContract]
    public sealed class MobileAssociationBonus : IMobileFeedBase<MobileAssociationBonus>
    {
        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public string site { get; set; }
        [ProtoMember(4)]
        public int? amount { get; set; }

        public bool Equals(MobileAssociationBonus obj)
        {
            return
                this.added_date == obj.added_date &&
                this.amount == obj.amount &&
                this.group_id == obj.group_id &&
                this.site == obj.site;
        }
    }

    [ProtoContract]
    public sealed class MobileCareersJobAd : IMobileFeedBase<MobileCareersJobAd>
    {
        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public int? job_id { get; set; }
        [ProtoMember(4)]
        public string link { get; set; }
        [ProtoMember(5)]
        public string company_name { get; set; }
        [ProtoMember(6)]
        public string location { get; set; }
        [ProtoMember(7)]
        public string title { get; set; }

        public bool Equals(MobileCareersJobAd obj)
        {
            return
                this.added_date == obj.added_date &&
                this.company_name == obj.company_name &&
                this.group_id == obj.group_id &&
                this.job_id == obj.job_id &&
                this.link == obj.link &&
                this.location == obj.location &&
                this.title == obj.title;
        }
    }

    [ProtoContract]
    public sealed class MobileBannerAd : IMobileFeedBase<MobileBannerAd>
    {
        [ProtoContract]
        public sealed class MobileBannerAdImage : IGenericEquality<MobileBannerAdImage>
        {
            [ProtoMember(1)]
            public string image_url { get; set; }
            [ProtoMember(2)]
            public int? width { get; set; }
            [ProtoMember(3)]
            public int? height { get; set; }

            public bool Equals(MobileBannerAdImage obj)
            {
                return
                    this.height == obj.height &&
                    this.image_url == obj.image_url &&
                    this.width == obj.width;
            }
        }

        [ProtoMember(1)]
        public int? group_id { get; set; }
        [ProtoMember(2)]
        public long? added_date { get; set; }

        [ProtoMember(3)]
        public string link { get; set; }

        [ProtoMember(4)]
        public List<MobileBannerAdImage> images { get; set; }

        public bool Equals(MobileBannerAd obj)
        {
            return
                this.added_date == obj.added_date &&
                this.group_id == obj.group_id &&
                this.images.TrueEqualsList(obj.images) &&
                this.link == obj.link;
        }
    }

    [ProtoContract]
    public sealed class MobileUpdateNotice : IGenericEquality<MobileUpdateNotice>
    {
        [ProtoMember(1)]
        public bool? should_update { get; set; }
        [ProtoMember(2)]
        public string message { get; set; }
        [ProtoMember(3)]
        public string minimum_supported_version { get; set; }

        public bool Equals(MobileUpdateNotice obj)
        {
            return
                this.message == obj.message &&
                this.minimum_supported_version == obj.minimum_supported_version &&
                this.should_update == obj.should_update;
        }
    }
}
