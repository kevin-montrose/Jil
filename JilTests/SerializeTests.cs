using Jil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace JilTests
{
    [TestClass]
    public class SerializeTests
    {
#if !DEBUG
        public class MobileFeed
        {
            public List<MobileQuestion> hot_questions { get; set; }
            public List<MobileInboxItem> inbox_items { get; set; }
            public List<MobileQuestion> likely_to_answer_questions { get; set; }
            public List<MobileRepChange> reputation_events { get; set; }
            public List<MobileQuestion> cross_site_interesting_questions { get; set; }
            public List<MobileBadgeAward> badges { get; set; }
            public List<MobilePrivilege> earned_privileges { get; set; }
            public List<MobilePrivilege> upcoming_privileges { get; set; }
            public List<MobileCommunityBulletin> community_bulletins { get; set; }
            public List<MobileAssociationBonus> association_bonuses { get; set; }
            public List<MobileCareersJobAd> careers_job_ads { get; set; }
            public List<MobileBannerAd> banner_ads { get; set; }

            public long before { get; set; }
            public long since { get; set; }

            public int account_id { get; set; }

            public MobileUpdateNotice update_notice { get; set; }

            public static MobileFeed For(Random rand)
            {
                var hq = new List<MobileQuestion>();
                var ii = new List<MobileInboxItem>();
                var ltaq = new List<MobileQuestion>();
                var re = new List<MobileRepChange>();
                var csiq = new List<MobileQuestion>();
                var b = new List<MobileBadgeAward>();
                var ep = new List<MobilePrivilege>();
                var up = new List<MobilePrivilege>();
                var cb = new List<MobileCommunityBulletin>();
                var ab = new List<MobileAssociationBonus>();
                var cja = new List<MobileCareersJobAd>();
                var ba = new List<MobileBannerAd>();

                for (var i = 0; i < 5; i++)
                {
                    hq.Add(MobileQuestion.For(rand));
                    ii.Add(MobileInboxItem.For(rand));
                    ltaq.Add(MobileQuestion.For(rand));
                    re.Add(MobileRepChange.For(rand));
                    csiq.Add(MobileQuestion.For(rand));
                    b.Add(MobileBadgeAward.For(rand));
                    ep.Add(MobilePrivilege.For(rand));
                    up.Add(MobilePrivilege.For(rand));
                    cb.Add(MobileCommunityBulletin.For(rand));
                    ab.Add(MobileAssociationBonus.For(rand));
                    cja.Add(MobileCareersJobAd.For(rand));
                    ba.Add(MobileBannerAd.For(rand));
                }

                return
                    new MobileFeed
                    {
                        account_id = rand.Next(),
                        association_bonuses = ab,
                        badges = b,
                        banner_ads = ba,
                        before = rand.Next(),
                        careers_job_ads = cja,
                        community_bulletins = cb,
                        cross_site_interesting_questions = csiq,
                        earned_privileges = ep,
                        hot_questions = hq,
                        inbox_items = ii,
                        likely_to_answer_questions = ltaq,
                        reputation_events = re,
                        since = rand.Next(),
                        upcoming_privileges = up,
                        update_notice = MobileUpdateNotice.For(rand)
                    };
            }
        }

        public interface IMobileFeedBase
        {
            int group_id { get; set; }
            long added_date { get; set; }
        }

        public sealed class MobileQuestion : IMobileFeedBase
        {
            public int group_id { get; set; }
            public long added_date { get; set; }

            public int question_id { get; set; }
            public long question_creation_date { get; set; }
            public string title { get; set; }
            public long last_activity_date { get; set; }
            public string[] tags { get; set; }
            public string site { get; set; }

            public bool is_deleted { get; set; }
            public bool has_accepted_answer { get; set; }
            public int answer_count { get; set; }

            public static MobileQuestion For(Random rand)
            {
                return
                    new MobileQuestion
                    {
                        added_date = rand.Next(),
                        answer_count = rand.Next(),
                        group_id = rand.Next(),
                        has_accepted_answer = rand.Next() % 2 == 0,
                        is_deleted = rand.Next() % 2 == 0,
                        last_activity_date = rand.Next(),
                        question_creation_date = rand.Next(),
                        question_id = rand.Next(),
                        site = SpeedProofTests._RandString(rand),
                        tags = Enumerable.Repeat(SpeedProofTests._RandString(rand), 5).ToArray(),
                        title = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobileRepChange : IMobileFeedBase
        {
            public int group_id { get; set; }
            public long added_date { get; set; }

            public string site { get; set; }

            public string title { get; set; }
            public string link { get; set; }
            public int rep_change { get; set; }

            public static MobileRepChange For(Random rand)
            {
                return
                    new MobileRepChange
                    {
                        added_date = rand.Next(),
                        group_id = rand.Next(),
                        link = SpeedProofTests._RandString(rand),
                        rep_change = rand.Next(),
                        site = SpeedProofTests._RandString(rand),
                        title = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobileInboxItem : IMobileFeedBase
        {
            public int group_id { get; set; }
            public long added_date { get; set; }

            public int? answer_id { get; set; }
            public string body { get; set; }
            public int? comment_id { get; set; }
            public long creation_date { get; set; }
            public string item_type { get; set; }
            public string link { get; set; }
            public int? question_id { get; set; }
            public string title { get; set; }
            public string site { get; set; }

            public static MobileInboxItem For(Random rand)
            {
                return
                    new MobileInboxItem
                    {
                        added_date = rand.Next(),
                        answer_id = rand.Next(),
                        body = SpeedProofTests._RandString(rand),
                        comment_id = rand.Next(),
                        creation_date = rand.Next(),
                        group_id = rand.Next(),
                        item_type = SpeedProofTests._RandString(rand),
                        link = SpeedProofTests._RandString(rand),
                        question_id = rand.Next(),
                        site = SpeedProofTests._RandString(rand),
                        title = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobileBadgeAward : IMobileFeedBase
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

            public int group_id { get; set; }
            public long added_date { get; set; }

            public string site { get; set; }
            public string badge_name { get; set; }
            public string badge_description { get; set; }
            public int badge_id { get; set; }

            public int? post_id { get; set; }
            public string link { get; set; }

            public BadgeRank rank { get; set; }
            public BadgeType badge_type { get; set; }

            public static MobileBadgeAward For(Random rand)
            {
                return
                    new MobileBadgeAward
                    {
                        added_date = rand.Next(),
                        badge_description = SpeedProofTests._RandString(rand),
                        badge_id = rand.Next(),
                        badge_name = SpeedProofTests._RandString(rand),
                        badge_type = (BadgeType)(rand.Next(2) + 1),
                        group_id = rand.Next(),
                        link = SpeedProofTests._RandString(rand),
                        post_id = rand.Next(),
                        rank = (BadgeRank)(rand.Next(3) + 1),
                        site = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobilePrivilege : IMobileFeedBase
        {
            public int group_id { get; set; }
            public long added_date { get; set; }

            public string site { get; set; }
            public string privilege_short_description { get; set; }
            public string privilege_long_description { get; set; }
            public int privilege_id { get; set; }

            public int reputation_required { get; set; }
            public string link { get; set; }

            public static MobilePrivilege For(Random rand)
            {
                return
                    new MobilePrivilege
                    {
                        added_date = rand.Next(),
                        group_id = rand.Next(),
                        link = SpeedProofTests._RandString(rand),
                        privilege_id = rand.Next(),
                        privilege_long_description = SpeedProofTests._RandString(rand),
                        privilege_short_description = SpeedProofTests._RandString(rand),
                        reputation_required = rand.Next(),
                        site = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobileCommunityBulletin : IMobileFeedBase
        {
            public enum CommunityBulletinType : byte
            {
                blog_post = 1,
                featured_meta_question = 2,
                upcoming_event = 3
            }

            public int group_id { get; set; }
            public long added_date { get; set; }

            public string site { get; set; }

            public string title { get; set; }
            public string link { get; set; }

            public CommunityBulletinType bulletin_type { get; set; }

            public long? begin_date { get; set; }
            public long? end_date { get; set; }
            public string custom_date_string { get; set; }

            public List<string> tags { get; set; }
            public bool is_deleted { get; set; }
            public bool has_accepted_answer { get; set; }
            public int answer_count { get; set; }

            public bool is_promoted { get; set; }

            public static MobileCommunityBulletin For(Random rand)
            {
                return
                    new MobileCommunityBulletin
                    {
                        added_date = rand.Next(),
                        answer_count = rand.Next(),
                        begin_date = rand.Next(),
                        bulletin_type = (CommunityBulletinType)(rand.Next(3) + 1),
                        custom_date_string = SpeedProofTests._RandString(rand),
                        end_date = rand.Next(),
                        group_id = rand.Next(),
                        has_accepted_answer = rand.Next() % 2 == 0,
                        is_deleted = rand.Next() % 2 == 0,
                        is_promoted = rand.Next() % 2 == 0,
                        link = SpeedProofTests._RandString(rand),
                        site = SpeedProofTests._RandString(rand),
                        tags = Enumerable.Repeat(SpeedProofTests._RandString(rand), 5).ToList(),
                        title = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobileAssociationBonus : IMobileFeedBase
        {
            public int group_id { get; set; }
            public long added_date { get; set; }

            public string site { get; set; }
            public int amount { get; set; }

            public static MobileAssociationBonus For(Random rand)
            {
                return
                    new MobileAssociationBonus
                    {
                        added_date = rand.Next(),
                        amount = rand.Next(),
                        site = SpeedProofTests._RandString(rand),
                        group_id = rand.Next()
                    };
            }
        }


        public sealed class MobileCareersJobAd : IMobileFeedBase
        {
            public int group_id { get; set; }
            public long added_date { get; set; }

            public int job_id { get; set; }
            public string link { get; set; }
            public string company_name { get; set; }
            public string location { get; set; }
            public string title { get; set; }

            public static MobileCareersJobAd For(Random rand)
            {
                return
                    new MobileCareersJobAd
                    {
                        added_date = rand.Next(),
                        company_name = SpeedProofTests._RandString(rand),
                        group_id = rand.Next(),
                        job_id = rand.Next(),
                        link = SpeedProofTests._RandString(rand),
                        location = SpeedProofTests._RandString(rand),
                        title = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobileBannerAd : IMobileFeedBase
        {
            public sealed class MobileBannerAdImage
            {
                public string image_url { get; set; }
                public int width { get; set; }
                public int height { get; set; }

                public static MobileBannerAdImage For(Random rand)
                {
                    return new MobileBannerAdImage
                    {
                        height = rand.Next(),
                        image_url = SpeedProofTests._RandString(rand),
                        width = rand.Next()
                    };
                }
            }

            public int group_id { get; set; }
            public long added_date { get; set; }

            public string link { get; set; }

            // these should be kept in order such that the "best" image is first in the list
            public List<MobileBannerAdImage> images { get; set; }

            public static MobileBannerAd For(Random rand)
            {
                return
                    new MobileBannerAd
                    {
                        added_date = rand.Next(),
                        group_id = rand.Next(),
                        images = Enumerable.Repeat(MobileBannerAdImage.For(rand), 3).ToList(),
                        link = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public sealed class MobileUpdateNotice
        {
            public bool should_update { get; set; }
            public string message { get; set; }
            public string minimum_supported_version { get; set; }

            public static MobileUpdateNotice For(Random rand)
            {
                return
                    new MobileUpdateNotice
                    {
                        should_update = rand.Next() % 2 == 0,
                        message = SpeedProofTests._RandString(rand),
                        minimum_supported_version = SpeedProofTests._RandString(rand)
                    };
            }
        }

        [TestMethod]
        public void FirstCallTime()
        {
            const int acceptableMS = 1000;

            var random = new Random();

            var feed = MobileFeed.For(random);

            var timer = new Stopwatch();

            using (var str = new StringWriter())
            {
                timer.Start();
                JSON.Serialize(feed, str);
                timer.Stop();
            }

            Assert.IsTrue(timer.ElapsedMilliseconds <= acceptableMS, "Took longer than " + acceptableMS + "ms to build a serializer for MobileFeed; unacceptable, was " + timer.ElapsedMilliseconds + "ms");
        }
#endif
        class _JilDirectiveAttributeTest
        {
            [JilDirective(Name = "AOverride")]
            public string A;
            [JilDirective(Ignore = true)]
            public string B;

            [JilDirective(Name = null, Ignore = true)]
            public string C;
            [JilDirective(Name = "DOverride", Ignore = false)]
            public string D;
        }

        [TestMethod]
        public void JilDirectiveAttributeTest()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _JilDirectiveAttributeTest { A = "123", B = "456", C = "789", D = "0AB" }, str);
                var res = str.ToString();
                Assert.AreEqual("{\"DOverride\":\"0AB\",\"AOverride\":\"123\"}", res);
            }
        }


        public class _SimpleObject
        {
            public int Foo;
        }

        [TestMethod]
        public void SimpleObject()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _SimpleObject { Foo = 123 }, str);

                var res = str.ToString();

                Assert.AreEqual("{\"Foo\":123}", res);
            }
        }

        [TestMethod]
        public void SerializeToString()
        {
            using (var str = new StringWriter())
            {
                var res = JSON.Serialize(new { Foo = 123 });

                Assert.AreEqual("{\"Foo\":123}", res);
            }
        }

        public class _Cyclical
        {
            public int Foo;

            public _Cyclical Next;
        }

        [TestMethod]
        public void Cyclical()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _Cyclical { Foo = 123, Next = new _Cyclical { Foo = 456 } }, str);
                var res = str.ToString();
                Assert.AreEqual("{\"Foo\":123,\"Next\":{\"Foo\":456,\"Next\":null}}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new[] { new _Cyclical { Foo = 123, Next = new _Cyclical { Foo = 456 } }, new _Cyclical { Foo = 456 } }, str);
                var res = str.ToString();
                Assert.AreEqual("[{\"Foo\":123,\"Next\":{\"Foo\":456,\"Next\":null}},{\"Foo\":456,\"Next\":null}]", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new Dictionary<string, _Cyclical> { { "hello", new _Cyclical { Foo = 123, Next = new _Cyclical { Foo = 456 } } }, { "world", new _Cyclical { Foo = 456 } } }, str);
                var res = str.ToString();
                Assert.AreEqual("{\"hello\":{\"Foo\":123,\"Next\":{\"Foo\":456,\"Next\":null}},\"world\":{\"Foo\":456,\"Next\":null}}", res);
            }
        }

        [TestMethod]
        public void Primitive()
        {
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<byte>(123, str);

                    var res = str.ToString();

                    Assert.AreEqual("123", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<sbyte>(-123, str);

                    var res = str.ToString();

                    Assert.AreEqual("-123", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<short>(-1000, str);

                    var res = str.ToString();

                    Assert.AreEqual("-1000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ushort>(5000, str);

                    var res = str.ToString();

                    Assert.AreEqual("5000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<int>(-123, str);

                    var res = str.ToString();

                    Assert.AreEqual("-123", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<uint>(123456789, str);

                    var res = str.ToString();

                    Assert.AreEqual("123456789", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<long>(-5000000000, str);

                    var res = str.ToString();

                    Assert.AreEqual("-5000000000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ulong>(8000000000, str);

                    var res = str.ToString();

                    Assert.AreEqual("8000000000", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<string>("hello world", str);

                    var res = str.ToString();

                    Assert.AreEqual("\"hello world\"", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<char>('c', str);

                    var res = str.ToString();

                    Assert.AreEqual("\"c\"", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<float>(1.234f, str);

                    var res = str.ToString();

                    Assert.AreEqual("1.234", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<double>(1.1, str);

                    var res = str.ToString();

                    Assert.AreEqual("1.1", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<decimal>(4.56m, str);

                    var res = str.ToString();

                    Assert.AreEqual("4.56", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<bool>(true, str);

                    var res = str.ToString();

                    Assert.AreEqual("true", res);
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(new DateTime(1999, 1, 2, 3, 4, 5, 6, DateTimeKind.Utc), str);
                    Assert.AreEqual("\"\\/Date(915246245006)\\/\"", str.ToString());
                }
            }
        }

#pragma warning disable 0649
        public class _StringsAndChars
        {
            public class _One
            {
                public string Single;
            }

            public class _Two
            {
                public int _;
                public string Trailing;
            }

            public class _Three
            {
                public string Leading;
                public int _;
            }

            public _One One;
            public _Two Two;
            public _Three Three;
        }
#pragma warning restore 0649

        [TestMethod]
        public void StringsAndChars()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _StringsAndChars
                    {
                        One = new _StringsAndChars._One
                        {
                            Single = "Hello World"
                        },
                        Two = new _StringsAndChars._Two
                        {
                            _ = 123,
                            Trailing = "Fizz Buzz"
                        },
                        Three = new _StringsAndChars._Three
                        {
                            Leading = "Foo Bar",
                            _ = 456
                        }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"One\":{\"Single\":\"Hello World\"},\"Two\":{\"_\":123,\"Trailing\":\"Fizz Buzz\"},\"Three\":{\"_\":456,\"Leading\":\"Foo Bar\"}}", res);
            }
        }

        [TestMethod]
        public void Dictionary()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, int>
                    {
                        { "hello world", 123 },
                        { "fizz buzz", 456 },
                        { "indeed", 789 }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"hello world\":123,\"fizz buzz\":456,\"indeed\":789}", res);
            }
        }

        [TestMethod]
        public void IReadOnlyDictionary()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    (IReadOnlyDictionary<string, int>)new Dictionary<string, int>
                    {
                        { "hello world", 123 },
                        { "fizz buzz", 456 },
                        { "indeed", 789 }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"hello world\":123,\"fizz buzz\":456,\"indeed\":789}", res);
            }
        }

#pragma warning disable 0649
        public class _List
        {
            public string Key;
            public int Val;
        }
#pragma warning restore 0649

        [TestMethod]
        public void List()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new[]
                    {
                        new _List { Key = "whatever", Val = 123 },
                        new _List { Key = "indeed", Val = 456 }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("[{\"Val\":123,\"Key\":\"whatever\"},{\"Val\":456,\"Key\":\"indeed\"}]", res);
            }
        }

        [TestMethod]
        public void IReadOnlyList()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    (IReadOnlyList<_List>)new[]
                    {
                        new _List { Key = "whatever", Val = 123 },
                        new _List { Key = "indeed", Val = 456 }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("[{\"Val\":123,\"Key\":\"whatever\"},{\"Val\":456,\"Key\":\"indeed\"}]", res);
            }
        }

        public class _Properties
        {
            public int Foo { get; set; }
            public string Bar { get; set; }
        }

        [TestMethod]
        public void Properties()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _Properties { Foo = 123, Bar = "hello" },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"Foo\":123,\"Bar\":\"hello\"}", res);
            }
        }

        public class _InnerLists
        {
            public class _WithList
            {
                public List<int> List;
            }

            public _WithList WithList;
        }

        [TestMethod]
        public void InnerLists()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _InnerLists
                    {
                        WithList = new _InnerLists._WithList
                        {
                            List = new List<int> { 1, 2, 3 }
                        }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual("{\"WithList\":{\"List\":[1,2,3]}}", res);
            }
        }

        class _CharacterEncoding
        {
            public char Char;
        }

        [TestMethod]
        public void CharacterEncoding()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0000' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0000\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0001' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0001\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0002' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0002\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0003' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0003\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0004' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0004\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0005' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0005\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0006' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0006\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0007' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0007\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0008' }, str);
                Assert.AreEqual("{\"Char\":\"\\b\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0009' }, str);
                Assert.AreEqual("{\"Char\":\"\\t\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000A' }, str);
                Assert.AreEqual("{\"Char\":\"\\n\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000B' }, str);
                Assert.AreEqual("{\"Char\":\"\\u000B\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000C' }, str);
                Assert.AreEqual("{\"Char\":\"\\f\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000D' }, str);
                Assert.AreEqual("{\"Char\":\"\\r\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000E' }, str);
                Assert.AreEqual("{\"Char\":\"\\u000E\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u000F' }, str);
                Assert.AreEqual("{\"Char\":\"\\u000F\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0010' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0010\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0011' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0011\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0012' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0012\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0013' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0013\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0014' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0014\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0015' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0015\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0016' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0016\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0017' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0017\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0018' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0018\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u0019' }, str);
                Assert.AreEqual("{\"Char\":\"\\u0019\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001A' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001A\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001B' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001B\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001C' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001C\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001D' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001D\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001E' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001E\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\u001F' }, str);
                Assert.AreEqual("{\"Char\":\"\\u001F\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '\\' }, str);
                Assert.AreEqual("{\"Char\":\"\\\\\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CharacterEncoding { Char = '"' }, str);
                Assert.AreEqual("{\"Char\":\"\\\"\"}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize("hello\b\f\r\n\tworld", str);
                Assert.AreEqual("\"hello\\b\\f\\r\\n\\tworld\"", str.ToString());
            }
        }

        [TestMethod]
        public void NullablePrimitives()
        {
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<byte?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<byte?>(123, str);
                    Assert.AreEqual("123", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<sbyte?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<sbyte?>(-123, str);
                    Assert.AreEqual("-123", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<short?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<short?>(-1024, str);
                    Assert.AreEqual("-1024", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ushort?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<ushort?>(2048, str);
                    Assert.AreEqual("2048", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<int?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<int?>(-1234567, str);
                    Assert.AreEqual("-1234567", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<uint?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<uint?>(123456789, str);
                    Assert.AreEqual("123456789", str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<long?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<long?>(long.MinValue, str);
                    Assert.AreEqual(long.MinValue.ToString(), str.ToString());
                }
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<ulong?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<ulong?>(ulong.MaxValue, str);
                    Assert.AreEqual(ulong.MaxValue.ToString(), str.ToString());
                }
            }
        }

        [TestMethod]
        public void NullableMembers()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new List<int?> { 0, null, 1, null, 2, null, 3 }, str);
                Assert.AreEqual("[0,null,1,null,2,null,3]", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new Dictionary<string, double?> { { "hello", null }, { "world", 3.21 } }, str);
                Assert.AreEqual("{\"hello\":null,\"world\":3.21}", str.ToString());
            }
        }

        public struct _ValueTypes
        {
            public string A;
            public char B;
            public List<int> C;
        }

        [TestMethod]
        public void ValueTypes()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _ValueTypes { A = "hello world", B = 'C', C = new List<int> { 3, 1, 4, 1, 5, 9 } }, str);
                Assert.AreEqual("{\"A\":\"hello world\",\"B\":\"C\",\"C\":[3,1,4,1,5,9]}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new[] { new _ValueTypes { A = "hello world", B = 'C', C = new List<int> { 3, 1, 4, 1, 5, 9 } } }, str);
                Assert.AreEqual("[{\"A\":\"hello world\",\"B\":\"C\",\"C\":[3,1,4,1,5,9]}]", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(new Dictionary<string, _ValueTypes> { { "hello", new _ValueTypes { A = "hello world", B = 'C', C = new List<int> { 3, 1, 4, 1, 5, 9 } } }, { "world", new _ValueTypes { A = "foo bar", B = 'D', C = new List<int> { 1, 3, 1, 8 } } } }, str);
                Assert.AreEqual("{\"hello\":{\"A\":\"hello world\",\"B\":\"C\",\"C\":[3,1,4,1,5,9]},\"world\":{\"A\":\"foo bar\",\"B\":\"D\",\"C\":[1,3,1,8]}}", str.ToString());
            }

            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<_ValueTypes?>(null, str);
                    Assert.AreEqual("null", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize<_ValueTypes?>(new _ValueTypes { A = "bizz", B = '\0', C = null }, str);
                    Assert.AreEqual("{\"A\":\"bizz\",\"B\":\"\\u0000\",\"C\":null}", str.ToString());
                }
            }
        }

        public struct _CyclicalValueTypes
        {
            public class _One
            {
                public _CyclicalValueTypes? Inner;
            }

            public _One A;
            public long B;
            public double C;
        }

        [TestMethod]
        public void CyclicalValueTypes()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _CyclicalValueTypes { A = new _CyclicalValueTypes._One { Inner = new _CyclicalValueTypes { B = 123, C = 4.56 } }, B = long.MaxValue, C = 78.90 }, str);
                Assert.AreEqual("{\"A\":{\"Inner\":{\"A\":null,\"B\":123,\"C\":4.56}},\"B\":9223372036854775807,\"C\":78.9}", str.ToString());
            }
        }

        public class _ExcludeNulls
        {
            public string A;
            public string B;
            public int? C;
            public int? D;
            public _ExcludeNulls E;
            public _ExcludeNulls F;
        }

        [TestMethod]
        public void ExcludeNulls()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _ExcludeNulls
                    {
                        A = "hello",
                        B = null,
                        C = 123,
                        D = null,
                        E = null,
                        F = new _ExcludeNulls
                        {
                            A = null,
                            B = "world",
                            C = null,
                            D = 456,
                            E = new _ExcludeNulls
                            {
                                C = 999
                            },
                            F = null
                        }
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"F\":{\"E\":{\"C\":999},\"D\":456,\"B\":\"world\"},\"C\":123,\"A\":\"hello\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello", "world" },
                        { "foo", null },
                        { "fizz", "buzz" }
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();

                Assert.AreEqual("{\"hello\":\"world\",\"fizz\":\"buzz\"}", res);
            }
        }

        public class _PrettyPrint
        {
            public string A;
            public string B;
            public int? C;
            public int? D;
            public _PrettyPrint E;
            public _PrettyPrint F;
        }

        [TestMethod]
        public void PrettyPrint()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _PrettyPrint
                    {
                        A = "hello",
                        B = null,
                        C = 123,
                        D = null,
                        E = null,
                        F = new _PrettyPrint
                        {
                            A = null,
                            B = "world",
                            C = null,
                            D = 456,
                            E = new _PrettyPrint
                            {
                                C = 999
                            },
                            F = null
                        }
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"E\": null,\n \"F\": {\n  \"E\": {\n   \"E\": null,\n   \"F\": null,\n   \"D\": null,\n   \"C\": 999,\n   \"B\": null,\n   \"A\": null\n  },\n  \"F\": null,\n  \"D\": 456,\n  \"C\": null,\n  \"B\": \"world\",\n  \"A\": null\n },\n \"D\": null,\n \"C\": 123,\n \"B\": null,\n \"A\": \"hello\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _PrettyPrint
                    {
                        A = "hello",
                        B = null,
                        C = 123,
                        D = null,
                        E = null,
                        F = new _PrettyPrint
                        {
                            A = null,
                            B = "world",
                            C = null,
                            D = 456,
                            E = new _PrettyPrint
                            {
                                C = 999
                            },
                            F = null
                        }
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"F\": {\n  \"E\": {\n   \"C\": 999\n  },\n  \"D\": 456,\n  \"B\": \"world\"\n },\n \"C\": 123,\n \"A\": \"hello\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, int?>
                    {
                        {"hello world", 31415926 },
                        {"fizz buzz", null },
                        {"foo bar", 1318 }
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"hello world\": 31415926,\n \"fizz buzz\": null,\n \"foo bar\": 1318\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, int?>
                    {
                        {"hello world", 31415926 },
                        {"fizz buzz", null },
                        {"foo bar", 1318 }
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"hello world\": 31415926,\n \"foo bar\": 1318\n}", res);
            }
        }

        [TestMethod]
        public void DictionaryEncoding()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, string>
                    {
                        { "hello\nworld", "fizz\0buzz" },
                        { "\r\t\f\n", "\0\0\0\0\0\0\0\0\0\0" },
                        { "\0", "\b\b\b\b\b" }
                    },
                    str
                );

                var res = str.ToString();

                Assert.AreEqual(@"{""hello\nworld"":""fizz\u0000buzz"",""\r\t\f\n"":""\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000"",""\u0000"":""\b\b\b\b\b""}", res);
            }
        }

        [TestMethod]
        public void DateTimeFormats()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    str,
                    Options.Default
                );

                var res = str.ToString();
                Assert.AreEqual("\"\\/Date(315532800000)\\/\"", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    str,
                    Options.MillisecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("315532800000", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    str,
                    Options.SecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("315532800", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    str,
                    Options.ISO8601
                );

                var res = str.ToString();
                Assert.AreEqual("\"1980-01-01T00:00:00Z\"", res);
            }

            using(var str = new StringWriter())
            {
                JSON.Serialize(
                    new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    str,
                    Options.RFC1123
                );

                var res = str.ToString();
                Assert.AreEqual("\"Tue, 01 Jan 1980 00:00:00 GMT\"", res);
            }
        }

        [TestMethod]
        public void ISODateTimes()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var rand = new Random(8337586);

            for (var i = 0; i < 10000; i++)
            {
                var rndDt = epoch;
                switch (rand.Next(6))
                {
                    case 0: rndDt += TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                    case 1: rndDt += TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                    case 2: rndDt += TimeSpan.FromSeconds(rand.Next()); break;
                    case 3: rndDt -= TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                    case 4: rndDt -= TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                    case 5: rndDt -= TimeSpan.FromSeconds(rand.Next()); break;
                }

                var expected = "\"" + rndDt.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\"";
                string actual;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(rndDt, str, Options.ISO8601);
                    actual = str.ToString();
                }

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void RFC1123DateTimes()
        {
            // stream
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var rand = new Random(15030661);

                for (var i = 0; i < 10000; i++)
                {
                    var rndDt = epoch;
                    switch (rand.Next(6))
                    {
                        case 0: rndDt += TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                        case 1: rndDt += TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                        case 2: rndDt += TimeSpan.FromSeconds(rand.Next()); break;
                        case 3: rndDt -= TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                        case 4: rndDt -= TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                        case 5: rndDt -= TimeSpan.FromSeconds(rand.Next()); break;
                    }

                    var expected = "\"" + rndDt.ToString("R") + "\"";
                    string actual;
                    using (var str = new StringWriter())
                    {
                        JSON.Serialize(rndDt, str, Options.RFC1123);
                        actual = str.ToString();
                    }

                    Assert.AreEqual(expected, actual);
                }
            }

            // string
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var rand = new Random(15030661);

                for (var i = 0; i < 10000; i++)
                {
                    var rndDt = epoch;
                    switch (rand.Next(6))
                    {
                        case 0: rndDt += TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                        case 1: rndDt += TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                        case 2: rndDt += TimeSpan.FromSeconds(rand.Next()); break;
                        case 3: rndDt -= TimeSpan.FromDays(rand.Next(ushort.MaxValue)); break;
                        case 4: rndDt -= TimeSpan.FromHours(rand.Next(ushort.MaxValue)); break;
                        case 5: rndDt -= TimeSpan.FromSeconds(rand.Next()); break;
                    }

                    var expected = "\"" + rndDt.ToString("R") + "\"";
                    var actual =JSON.Serialize(rndDt, Options.RFC1123);

                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestMethod]
        public void AllOptions()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.Default
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000,\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000,\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpoch
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511,\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpochExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511,\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\",\"B\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\",\n \"B\": null\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null
                    },
                    str,
                    Options.ISO8601PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\"\n}", res);
            }

            // JSONP
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.JSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\",\"B\":null,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"\\/Date(-23215049511000)\\/\",\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.PrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\",\n \"B\": null,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.PrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"\\/Date(-23215049511000)\\/\",\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000,\"B\":null,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511000,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000,\n \"B\": null,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511000,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511,\"B\":null,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":-23215049511,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511,\n \"B\": null,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": -23215049511,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601JSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\",\"B\":null,\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601ExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"1234-05-06T07:08:09Z\",\"C\":\"hello\\u2028\\u2029world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601PrettyPrintJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\",\n \"B\": null,\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = new DateTime(1234, 5, 6, 7, 8, 9, DateTimeKind.Utc),
                        B = (DateTime?)null,
                        C = "hello\u2028\u2029world"
                    },
                    str,
                    Options.ISO8601PrettyPrintExcludeNullsJSONP
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"1234-05-06T07:08:09Z\",\n \"C\": \"hello\\u2028\\u2029world\"\n}", res);
            }
        }

        public class _InfiniteRecursion
        {
            public int A;
            public _InfiniteRecursion Next;
        }

        [TestMethod]
        public void InfiniteRecursion()
        {
            using (var str = new StringWriter())
            {
                var root = new _InfiniteRecursion { A = 123 };
                root.Next = root;

                try
                {
                    JSON.Serialize(root, str);
                    Assert.Fail();
                }
                catch (InfiniteRecursionException)
                {
                    // check that it failed at the right time...
                    var failed = str.ToString();
                    Assert.AreEqual("{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":{\"A\":123,\"Next\":", failed);
                }
            }
        }

        public class _ConditionalSerialization1
        {
            public string Val { get; set; }

            public string AlwaysNull { get; set; }

            public int AlwaysHasValue { get { return 4; } }

            internal bool ShouldSerializeVal()
            {
                return Val != null && (Val.Length % 2) == 0;
            }

            public static _ConditionalSerialization1 Random(Random rand)
            {
                return
                    new _ConditionalSerialization1
                    {
                        Val = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public class _ConditionalSerialization2
        {
            public string Foo { get; set; }

            public string AlwaysNull { get; set; }

            internal bool ShouldSerializeFoo()
            {
                return Foo == null || (Foo.Length % 2) == 1;
            }

            public static _ConditionalSerialization2 Random(Random rand)
            {
                return
                    new _ConditionalSerialization2
                    {
                        Foo = SpeedProofTests._RandString(rand)
                    };
            }
        }

        public class _ConditionalSerialization3
        {
            [IgnoreDataMember]
            public string Foo { get; set; }

            public string AlwaysNull { get; set; }
        }

        public class _ConditionalSerialization4
        {
            [IgnoreDataMember]
            public string Foo;

            public string Bar;
        }

        [TestMethod]
        public void ConditionalSerialization()
        {
            var rand = new Random();

            for (var i = 0; i < 1000; i++)
            {
                using (var str = new StringWriter())
                {
                    var obj = _ConditionalSerialization1.Random(rand);

                    JSON.Serialize(obj, str, Options.ExcludeNulls);

                    var res = str.ToString();

                    if (res.Contains("\"AlwaysNull\""))
                    {
                        Assert.Fail(res);
                    }

                    if (!res.Contains("\"AlwaysHasValue\":4"))
                    {
                        Assert.Fail(res);
                    }

                    if (obj.ShouldSerializeVal() && !res.Contains("\"Val\":"))
                    {
                        Assert.Fail(res);
                    }

                    if (!obj.ShouldSerializeVal() && res.Contains("\"Val\":"))
                    {
                        Assert.Fail(res);
                    }
                }
            }

            for (var j = 0; j < 1000; j++)
            {
                using (var str = new StringWriter())
                {
                    var obj = _ConditionalSerialization2.Random(rand);

                    JSON.Serialize(obj, str, Options.Default);

                    var res = str.ToString();

                    if (!res.Contains("\"AlwaysNull\":null"))
                    {
                        Assert.Fail(res);
                    }

                    if (obj.ShouldSerializeFoo() && !res.Contains("\"Foo\":"))
                    {
                        Assert.Fail(res);
                    }

                    if (!obj.ShouldSerializeFoo() && res.Contains("\"Foo\":"))
                    {
                        Assert.Fail(res);
                    }
                }
            }

            using (var str = new StringWriter())
            {
                var obj = new _ConditionalSerialization3();

                JSON.Serialize(obj, str, Options.Default);

                var res = str.ToString();
                Assert.AreEqual("{\"AlwaysNull\":null}", res);
            }

            using (var str = new StringWriter())
            {
                var obj = new _ConditionalSerialization4 { Foo = "Ignored", Bar = "Included" };
                JSON.Serialize(obj, str);
                var res = str.ToString();
                Assert.AreEqual("{\"Bar\":\"Included\"}", res);
            }
        }

        enum _Enums
        {
            A = 1,
            B = 2,
            C = 3
        }

        enum _Enums2 : sbyte
        {
            A = -1,
            B = 22,
            C = -104,
            D = 66
        }

        [TestMethod]
        public void Enums()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(_Enums.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(_Enums.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(_Enums.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums?>(null, str);
                Assert.AreEqual("null", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums2.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums2.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(_Enums2.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(_Enums2.A, str);
                Assert.AreEqual("\"A\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(_Enums2.B, str);
                Assert.AreEqual("\"B\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(_Enums2.C, str);
                Assert.AreEqual("\"C\"", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<_Enums2?>(null, str);
                Assert.AreEqual("null", str.ToString());
            }
        }

        enum _EnumMembers : long
        {
            Foo = 1,
            Bar = 2,
            World = 3,
            Fizz = 4
        }

        [TestMethod]
        public void EnumMembers()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        A = _EnumMembers.Bar,
                        B = (_EnumMembers?)null
                    },
                    str
                );

                Assert.AreEqual("{\"A\":\"Bar\",\"B\":null}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<string, _EnumMembers?>
                    {
                        {"A",  _EnumMembers.Bar },
                        {"B", (_EnumMembers?)null }
                    },
                    str
                );

                Assert.AreEqual("{\"A\":\"Bar\",\"B\":null}", str.ToString());
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new[] { _EnumMembers.Bar, _EnumMembers.World, _EnumMembers.Fizz, _EnumMembers.Foo, _EnumMembers.Fizz },
                    str
                );

                Assert.AreEqual("[\"Bar\",\"World\",\"Fizz\",\"Foo\",\"Fizz\"]", str.ToString());
            }
        }

        enum _EnumDictionaryKeys
        {
            A = 3,
            B = 4,
            C = 11,
            D = 28
        }

        [TestMethod]
        public void EnumDictionaryKeys()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, "hello" },
                        { _EnumDictionaryKeys.B, "world" },
                        { _EnumDictionaryKeys.C, "fizz" },
                        { _EnumDictionaryKeys.D, "buzz" },
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"hello\",\"B\":\"world\",\"C\":\"fizz\",\"D\":\"buzz\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, "hello" },
                        { _EnumDictionaryKeys.B, "world" },
                        { _EnumDictionaryKeys.C, "fizz" },
                        { _EnumDictionaryKeys.D, "buzz" },
                    },
                    str,
                    Options.PrettyPrint
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"A\": \"hello\",\n \"B\": \"world\",\n \"C\": \"fizz\",\n \"D\": \"buzz\"\n}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, "hello" },
                        { _EnumDictionaryKeys.B, null },
                        { _EnumDictionaryKeys.C, "fizz" },
                        { _EnumDictionaryKeys.D, null },
                    },
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":\"hello\",\"C\":\"fizz\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new Dictionary<_EnumDictionaryKeys, string>
                    {
                        { _EnumDictionaryKeys.A, null },
                        { _EnumDictionaryKeys.B, "world" },
                        { _EnumDictionaryKeys.C, null },
                        { _EnumDictionaryKeys.D, "buzz" },
                    },
                    str,
                    Options.PrettyPrintExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\n \"B\": \"world\",\n \"D\": \"buzz\"\n}", res);
            }
        }

        enum _EnumVariations1 : byte
        {
            A = 0,
            B = 127,
            C = 255
        }

        enum _EnumVariations2 : sbyte
        {
            A = -128,
            B = 0,
            C = 127
        }

        enum _EnumVariations3 : short
        {
            A = short.MinValue,
            B = 0,
            C = short.MaxValue
        }

        enum _EnumVariations4 : ushort
        {
            A = ushort.MinValue,
            B = 32767,
            C = ushort.MaxValue
        }

        enum _EnumVariations5 : int
        {
            A = int.MinValue,
            B = 0,
            C = int.MaxValue
        }

        enum _EnumVariations6 : uint
        {
            A = uint.MinValue,
            B = 2147483647,
            C = uint.MaxValue
        }

        enum _EnumVariations7 : long
        {
            A = long.MinValue,
            B = 0,
            C = long.MaxValue
        }

        enum _EnumVariations8 : ulong
        {
            A = ulong.MinValue,
            B = 9223372036854775807L,
            C = ulong.MaxValue
        }

        enum _EnumVariations9 : byte
        {
            AA = 0, AB = 1, AC = 2, AD = 3, AE = 4, AF = 5, AG = 6, AH = 7, AI = 8, AJ = 9, AK = 10, AL = 11, AM = 12, AN = 13, AO = 14, AP = 15, AQ = 16, AR = 17, AS = 18, AT = 19, AU = 20, AV = 21, AW = 22, AX = 23, AY = 24, AZ = 25, BA = 26, BB = 27, BC = 28, BD = 29, BE = 30, BF = 31, BG = 32, BH = 33, BI = 34, BJ = 35, BK = 36, BL = 37, BM = 38, BN = 39, BO = 40, BP = 41, BQ = 42, BR = 43, BS = 44, BT = 45, BU = 46, BV = 47, BW = 48, BX = 49, BY = 50, BZ = 51, CA = 52, CB = 53, CC = 54, CD = 55, CE = 56, CF = 57, CG = 58, CH = 59, CI = 60, CJ = 61, CK = 62, CL = 63, CM = 64, CN = 65, CO = 66, CP = 67, CQ = 68, CR = 69, CS = 70, CT = 71, CU = 72, CV = 73, CW = 74, CX = 75, CY = 76, CZ = 77, DA = 78, DB = 79, DC = 80, DD = 81, DE = 82, DF = 83, DG = 84, DH = 85, DI = 86, DJ = 87, DK = 88, DL = 89, DM = 90, DN = 91, DO = 92, DP = 93, DQ = 94, DR = 95, DS = 96, DT = 97, DU = 98, DV = 99, DW = 100, DX = 101, DY = 102, DZ = 103, EA = 104, EB = 105, EC = 106, ED = 107, EE = 108, EF = 109, EG = 110, EH = 111, EI = 112, EJ = 113, EK = 114, EL = 115, EM = 116, EN = 117, EO = 118, EP = 119, EQ = 120, ER = 121, ES = 122, ET = 123, EU = 124, EV = 125, EW = 126, EX = 127, EY = 128, EZ = 129, FA = 130, FB = 131, FC = 132, FD = 133, FE = 134, FF = 135, FG = 136, FH = 137, FI = 138, FJ = 139, FK = 140, FL = 141, FM = 142, FN = 143, FO = 144, FP = 145, FQ = 146, FR = 147, FS = 148, FT = 149, FU = 150, FV = 151, FW = 152, FX = 153, FY = 154, FZ = 155, GA = 156, GB = 157, GC = 158, GD = 159, GE = 160, GF = 161, GG = 162, GH = 163, GI = 164, GJ = 165, GK = 166, GL = 167, GM = 168, GN = 169, GO = 170, GP = 171, GQ = 172, GR = 173, GS = 174, GT = 175, GU = 176, GV = 177, GW = 178, GX = 179, GY = 180, GZ = 181, HA = 182, HB = 183, HC = 184, HD = 185, HE = 186, HF = 187, HG = 188, HH = 189, HI = 190, HJ = 191, HK = 192, HL = 193, HM = 194, HN = 195, HO = 196, HP = 197, HQ = 198, HR = 199, HS = 200, HT = 201, HU = 202, HV = 203, HW = 204, HX = 205, HY = 206, HZ = 207, IA = 208, IB = 209, IC = 210, ID = 211, IE = 212, IF = 213, IG = 214, IH = 215, II = 216, IJ = 217, IK = 218, IL = 219, IM = 220, IN = 221, IO = 222, IP = 223, IQ = 224, IR = 225, IS = 226, IT = 227, IU = 228, IV = 229, IW = 230, IX = 231, IY = 232, IZ = 233, JA = 234, JB = 235, JC = 236, JD = 237, JE = 238, JF = 239, JG = 240, JH = 241, JI = 242, JJ = 243, JK = 244, JL = 245, JM = 246, JN = 247, JO = 248, JP = 249, JQ = 250, JR = 251, JS = 252, JT = 253, JU = 254, JV = 255,
        }

        enum _EnumVariations10 : sbyte
        {
            AA = -128, AB = -127, AC = -126, AD = -125, AE = -124, AF = -123, AG = -122, AH = -121, AI = -120, AJ = -119, AK = -118, AL = -117, AM = -116, AN = -115, AO = -114, AP = -113, AQ = -112, AR = -111, AS = -110, AT = -109, AU = -108, AV = -107, AW = -106, AX = -105, AY = -104, AZ = -103, BA = -102, BB = -101, BC = -100, BD = -99, BE = -98, BF = -97, BG = -96, BH = -95, BI = -94, BJ = -93, BK = -92, BL = -91, BM = -90, BN = -89, BO = -88, BP = -87, BQ = -86, BR = -85, BS = -84, BT = -83, BU = -82, BV = -81, BW = -80, BX = -79, BY = -78, BZ = -77, CA = -76, CB = -75, CC = -74, CD = -73, CE = -72, CF = -71, CG = -70, CH = -69, CI = -68, CJ = -67, CK = -66, CL = -65, CM = -64, CN = -63, CO = -62, CP = -61, CQ = -60, CR = -59, CS = -58, CT = -57, CU = -56, CV = -55, CW = -54, CX = -53, CY = -52, CZ = -51, DA = -50, DB = -49, DC = -48, DD = -47, DE = -46, DF = -45, DG = -44, DH = -43, DI = -42, DJ = -41, DK = -40, DL = -39, DM = -38, DN = -37, DO = -36, DP = -35, DQ = -34, DR = -33, DS = -32, DT = -31, DU = -30, DV = -29, DW = -28, DX = -27, DY = -26, DZ = -25, EA = -24, EB = -23, EC = -22, ED = -21, EE = -20, EF = -19, EG = -18, EH = -17, EI = -16, EJ = -15, EK = -14, EL = -13, EM = -12, EN = -11, EO = -10, EP = -9, EQ = -8, ER = -7, ES = -6, ET = -5, EU = -4, EV = -3, EW = -2, EX = -1, EY = 0, EZ = 1, FA = 2, FB = 3, FC = 4, FD = 5, FE = 6, FF = 7, FG = 8, FH = 9, FI = 10, FJ = 11, FK = 12, FL = 13, FM = 14, FN = 15, FO = 16, FP = 17, FQ = 18, FR = 19, FS = 20, FT = 21, FU = 22, FV = 23, FW = 24, FX = 25, FY = 26, FZ = 27, GA = 28, GB = 29, GC = 30, GD = 31, GE = 32, GF = 33, GG = 34, GH = 35, GI = 36, GJ = 37, GK = 38, GL = 39, GM = 40, GN = 41, GO = 42, GP = 43, GQ = 44, GR = 45, GS = 46, GT = 47, GU = 48, GV = 49, GW = 50, GX = 51, GY = 52, GZ = 53, HA = 54, HB = 55, HC = 56, HD = 57, HE = 58, HF = 59, HG = 60, HH = 61, HI = 62, HJ = 63, HK = 64, HL = 65, HM = 66, HN = 67, HO = 68, HP = 69, HQ = 70, HR = 71, HS = 72, HT = 73, HU = 74, HV = 75, HW = 76, HX = 77, HY = 78, HZ = 79, IA = 80, IB = 81, IC = 82, ID = 83, IE = 84, IF = 85, IG = 86, IH = 87, II = 88, IJ = 89, IK = 90, IL = 91, IM = 92, IN = 93, IO = 94, IP = 95, IQ = 96, IR = 97, IS = 98, IT = 99, IU = 100, IV = 101, IW = 102, IX = 103, IY = 104, IZ = 105, JA = 106, JB = 107, JC = 108, JD = 109, JE = 110, JF = 111, JG = 112, JH = 113, JI = 114, JJ = 115, JK = 116, JL = 117, JM = 118, JN = 119, JO = 120, JP = 121, JQ = 122, JR = 123, JS = 124, JT = 125, JU = 126, JV = 127
        }

        [TestMethod]
        public void EnumVariations()
        {
            // 1
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations1.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations1.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations1.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 2
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations2.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations2.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations2.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 3
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations3.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations3.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations3.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 4
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations4.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations4.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations4.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 5
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations5.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations5.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations5.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 6
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations6.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations6.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations6.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 7
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations7.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations7.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations7.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 8
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations8.A, str);
                    Assert.AreEqual("\"A\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations8.B, str);
                    Assert.AreEqual("\"B\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations8.C, str);
                    Assert.AreEqual("\"C\"", str.ToString());
                }
            }
            // 9
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AA, str);
                    Assert.AreEqual("\"AA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AB, str);
                    Assert.AreEqual("\"AB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AC, str);
                    Assert.AreEqual("\"AC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AD, str);
                    Assert.AreEqual("\"AD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AE, str);
                    Assert.AreEqual("\"AE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AF, str);
                    Assert.AreEqual("\"AF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AG, str);
                    Assert.AreEqual("\"AG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AH, str);
                    Assert.AreEqual("\"AH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AI, str);
                    Assert.AreEqual("\"AI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AJ, str);
                    Assert.AreEqual("\"AJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AK, str);
                    Assert.AreEqual("\"AK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AL, str);
                    Assert.AreEqual("\"AL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AM, str);
                    Assert.AreEqual("\"AM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AN, str);
                    Assert.AreEqual("\"AN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AO, str);
                    Assert.AreEqual("\"AO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AP, str);
                    Assert.AreEqual("\"AP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AQ, str);
                    Assert.AreEqual("\"AQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AR, str);
                    Assert.AreEqual("\"AR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AS, str);
                    Assert.AreEqual("\"AS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AT, str);
                    Assert.AreEqual("\"AT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AU, str);
                    Assert.AreEqual("\"AU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AV, str);
                    Assert.AreEqual("\"AV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AW, str);
                    Assert.AreEqual("\"AW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AX, str);
                    Assert.AreEqual("\"AX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AY, str);
                    Assert.AreEqual("\"AY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.AZ, str);
                    Assert.AreEqual("\"AZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BA, str);
                    Assert.AreEqual("\"BA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BB, str);
                    Assert.AreEqual("\"BB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BC, str);
                    Assert.AreEqual("\"BC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BD, str);
                    Assert.AreEqual("\"BD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BE, str);
                    Assert.AreEqual("\"BE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BF, str);
                    Assert.AreEqual("\"BF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BG, str);
                    Assert.AreEqual("\"BG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BH, str);
                    Assert.AreEqual("\"BH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BI, str);
                    Assert.AreEqual("\"BI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BJ, str);
                    Assert.AreEqual("\"BJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BK, str);
                    Assert.AreEqual("\"BK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BL, str);
                    Assert.AreEqual("\"BL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BM, str);
                    Assert.AreEqual("\"BM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BN, str);
                    Assert.AreEqual("\"BN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BO, str);
                    Assert.AreEqual("\"BO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BP, str);
                    Assert.AreEqual("\"BP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BQ, str);
                    Assert.AreEqual("\"BQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BR, str);
                    Assert.AreEqual("\"BR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BS, str);
                    Assert.AreEqual("\"BS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BT, str);
                    Assert.AreEqual("\"BT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BU, str);
                    Assert.AreEqual("\"BU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BV, str);
                    Assert.AreEqual("\"BV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BW, str);
                    Assert.AreEqual("\"BW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BX, str);
                    Assert.AreEqual("\"BX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BY, str);
                    Assert.AreEqual("\"BY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.BZ, str);
                    Assert.AreEqual("\"BZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CA, str);
                    Assert.AreEqual("\"CA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CB, str);
                    Assert.AreEqual("\"CB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CC, str);
                    Assert.AreEqual("\"CC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CD, str);
                    Assert.AreEqual("\"CD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CE, str);
                    Assert.AreEqual("\"CE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CF, str);
                    Assert.AreEqual("\"CF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CG, str);
                    Assert.AreEqual("\"CG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CH, str);
                    Assert.AreEqual("\"CH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CI, str);
                    Assert.AreEqual("\"CI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CJ, str);
                    Assert.AreEqual("\"CJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CK, str);
                    Assert.AreEqual("\"CK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CL, str);
                    Assert.AreEqual("\"CL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CM, str);
                    Assert.AreEqual("\"CM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CN, str);
                    Assert.AreEqual("\"CN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CO, str);
                    Assert.AreEqual("\"CO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CP, str);
                    Assert.AreEqual("\"CP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CQ, str);
                    Assert.AreEqual("\"CQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CR, str);
                    Assert.AreEqual("\"CR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CS, str);
                    Assert.AreEqual("\"CS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CT, str);
                    Assert.AreEqual("\"CT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CU, str);
                    Assert.AreEqual("\"CU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CV, str);
                    Assert.AreEqual("\"CV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CW, str);
                    Assert.AreEqual("\"CW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CX, str);
                    Assert.AreEqual("\"CX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CY, str);
                    Assert.AreEqual("\"CY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.CZ, str);
                    Assert.AreEqual("\"CZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DA, str);
                    Assert.AreEqual("\"DA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DB, str);
                    Assert.AreEqual("\"DB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DC, str);
                    Assert.AreEqual("\"DC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DD, str);
                    Assert.AreEqual("\"DD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DE, str);
                    Assert.AreEqual("\"DE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DF, str);
                    Assert.AreEqual("\"DF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DG, str);
                    Assert.AreEqual("\"DG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DH, str);
                    Assert.AreEqual("\"DH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DI, str);
                    Assert.AreEqual("\"DI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DJ, str);
                    Assert.AreEqual("\"DJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DK, str);
                    Assert.AreEqual("\"DK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DL, str);
                    Assert.AreEqual("\"DL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DM, str);
                    Assert.AreEqual("\"DM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DN, str);
                    Assert.AreEqual("\"DN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DO, str);
                    Assert.AreEqual("\"DO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DP, str);
                    Assert.AreEqual("\"DP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DQ, str);
                    Assert.AreEqual("\"DQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DR, str);
                    Assert.AreEqual("\"DR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DS, str);
                    Assert.AreEqual("\"DS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DT, str);
                    Assert.AreEqual("\"DT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DU, str);
                    Assert.AreEqual("\"DU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DV, str);
                    Assert.AreEqual("\"DV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DW, str);
                    Assert.AreEqual("\"DW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DX, str);
                    Assert.AreEqual("\"DX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DY, str);
                    Assert.AreEqual("\"DY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.DZ, str);
                    Assert.AreEqual("\"DZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EA, str);
                    Assert.AreEqual("\"EA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EB, str);
                    Assert.AreEqual("\"EB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EC, str);
                    Assert.AreEqual("\"EC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ED, str);
                    Assert.AreEqual("\"ED\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EE, str);
                    Assert.AreEqual("\"EE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EF, str);
                    Assert.AreEqual("\"EF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EG, str);
                    Assert.AreEqual("\"EG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EH, str);
                    Assert.AreEqual("\"EH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EI, str);
                    Assert.AreEqual("\"EI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EJ, str);
                    Assert.AreEqual("\"EJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EK, str);
                    Assert.AreEqual("\"EK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EL, str);
                    Assert.AreEqual("\"EL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EM, str);
                    Assert.AreEqual("\"EM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EN, str);
                    Assert.AreEqual("\"EN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EO, str);
                    Assert.AreEqual("\"EO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EP, str);
                    Assert.AreEqual("\"EP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EQ, str);
                    Assert.AreEqual("\"EQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ER, str);
                    Assert.AreEqual("\"ER\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ES, str);
                    Assert.AreEqual("\"ES\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ET, str);
                    Assert.AreEqual("\"ET\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EU, str);
                    Assert.AreEqual("\"EU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EV, str);
                    Assert.AreEqual("\"EV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EW, str);
                    Assert.AreEqual("\"EW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EX, str);
                    Assert.AreEqual("\"EX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EY, str);
                    Assert.AreEqual("\"EY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.EZ, str);
                    Assert.AreEqual("\"EZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FA, str);
                    Assert.AreEqual("\"FA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FB, str);
                    Assert.AreEqual("\"FB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FC, str);
                    Assert.AreEqual("\"FC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FD, str);
                    Assert.AreEqual("\"FD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FE, str);
                    Assert.AreEqual("\"FE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FF, str);
                    Assert.AreEqual("\"FF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FG, str);
                    Assert.AreEqual("\"FG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FH, str);
                    Assert.AreEqual("\"FH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FI, str);
                    Assert.AreEqual("\"FI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FJ, str);
                    Assert.AreEqual("\"FJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FK, str);
                    Assert.AreEqual("\"FK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FL, str);
                    Assert.AreEqual("\"FL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FM, str);
                    Assert.AreEqual("\"FM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FN, str);
                    Assert.AreEqual("\"FN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FO, str);
                    Assert.AreEqual("\"FO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FP, str);
                    Assert.AreEqual("\"FP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FQ, str);
                    Assert.AreEqual("\"FQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FR, str);
                    Assert.AreEqual("\"FR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FS, str);
                    Assert.AreEqual("\"FS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FT, str);
                    Assert.AreEqual("\"FT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FU, str);
                    Assert.AreEqual("\"FU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FV, str);
                    Assert.AreEqual("\"FV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FW, str);
                    Assert.AreEqual("\"FW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FX, str);
                    Assert.AreEqual("\"FX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FY, str);
                    Assert.AreEqual("\"FY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.FZ, str);
                    Assert.AreEqual("\"FZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GA, str);
                    Assert.AreEqual("\"GA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GB, str);
                    Assert.AreEqual("\"GB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GC, str);
                    Assert.AreEqual("\"GC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GD, str);
                    Assert.AreEqual("\"GD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GE, str);
                    Assert.AreEqual("\"GE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GF, str);
                    Assert.AreEqual("\"GF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GG, str);
                    Assert.AreEqual("\"GG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GH, str);
                    Assert.AreEqual("\"GH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GI, str);
                    Assert.AreEqual("\"GI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GJ, str);
                    Assert.AreEqual("\"GJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GK, str);
                    Assert.AreEqual("\"GK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GL, str);
                    Assert.AreEqual("\"GL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GM, str);
                    Assert.AreEqual("\"GM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GN, str);
                    Assert.AreEqual("\"GN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GO, str);
                    Assert.AreEqual("\"GO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GP, str);
                    Assert.AreEqual("\"GP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GQ, str);
                    Assert.AreEqual("\"GQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GR, str);
                    Assert.AreEqual("\"GR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GS, str);
                    Assert.AreEqual("\"GS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GT, str);
                    Assert.AreEqual("\"GT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GU, str);
                    Assert.AreEqual("\"GU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GV, str);
                    Assert.AreEqual("\"GV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GW, str);
                    Assert.AreEqual("\"GW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GX, str);
                    Assert.AreEqual("\"GX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GY, str);
                    Assert.AreEqual("\"GY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.GZ, str);
                    Assert.AreEqual("\"GZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HA, str);
                    Assert.AreEqual("\"HA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HB, str);
                    Assert.AreEqual("\"HB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HC, str);
                    Assert.AreEqual("\"HC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HD, str);
                    Assert.AreEqual("\"HD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HE, str);
                    Assert.AreEqual("\"HE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HF, str);
                    Assert.AreEqual("\"HF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HG, str);
                    Assert.AreEqual("\"HG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HH, str);
                    Assert.AreEqual("\"HH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HI, str);
                    Assert.AreEqual("\"HI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HJ, str);
                    Assert.AreEqual("\"HJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HK, str);
                    Assert.AreEqual("\"HK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HL, str);
                    Assert.AreEqual("\"HL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HM, str);
                    Assert.AreEqual("\"HM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HN, str);
                    Assert.AreEqual("\"HN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HO, str);
                    Assert.AreEqual("\"HO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HP, str);
                    Assert.AreEqual("\"HP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HQ, str);
                    Assert.AreEqual("\"HQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HR, str);
                    Assert.AreEqual("\"HR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HS, str);
                    Assert.AreEqual("\"HS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HT, str);
                    Assert.AreEqual("\"HT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HU, str);
                    Assert.AreEqual("\"HU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HV, str);
                    Assert.AreEqual("\"HV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HW, str);
                    Assert.AreEqual("\"HW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HX, str);
                    Assert.AreEqual("\"HX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HY, str);
                    Assert.AreEqual("\"HY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.HZ, str);
                    Assert.AreEqual("\"HZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IA, str);
                    Assert.AreEqual("\"IA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IB, str);
                    Assert.AreEqual("\"IB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IC, str);
                    Assert.AreEqual("\"IC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.ID, str);
                    Assert.AreEqual("\"ID\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IE, str);
                    Assert.AreEqual("\"IE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IF, str);
                    Assert.AreEqual("\"IF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IG, str);
                    Assert.AreEqual("\"IG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IH, str);
                    Assert.AreEqual("\"IH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.II, str);
                    Assert.AreEqual("\"II\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IJ, str);
                    Assert.AreEqual("\"IJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IK, str);
                    Assert.AreEqual("\"IK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IL, str);
                    Assert.AreEqual("\"IL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IM, str);
                    Assert.AreEqual("\"IM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IN, str);
                    Assert.AreEqual("\"IN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IO, str);
                    Assert.AreEqual("\"IO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IP, str);
                    Assert.AreEqual("\"IP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IQ, str);
                    Assert.AreEqual("\"IQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IR, str);
                    Assert.AreEqual("\"IR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IS, str);
                    Assert.AreEqual("\"IS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IT, str);
                    Assert.AreEqual("\"IT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IU, str);
                    Assert.AreEqual("\"IU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IV, str);
                    Assert.AreEqual("\"IV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IW, str);
                    Assert.AreEqual("\"IW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IX, str);
                    Assert.AreEqual("\"IX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IY, str);
                    Assert.AreEqual("\"IY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.IZ, str);
                    Assert.AreEqual("\"IZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JA, str);
                    Assert.AreEqual("\"JA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JB, str);
                    Assert.AreEqual("\"JB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JC, str);
                    Assert.AreEqual("\"JC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JD, str);
                    Assert.AreEqual("\"JD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JE, str);
                    Assert.AreEqual("\"JE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JF, str);
                    Assert.AreEqual("\"JF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JG, str);
                    Assert.AreEqual("\"JG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JH, str);
                    Assert.AreEqual("\"JH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JI, str);
                    Assert.AreEqual("\"JI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JJ, str);
                    Assert.AreEqual("\"JJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JK, str);
                    Assert.AreEqual("\"JK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JL, str);
                    Assert.AreEqual("\"JL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JM, str);
                    Assert.AreEqual("\"JM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JN, str);
                    Assert.AreEqual("\"JN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JO, str);
                    Assert.AreEqual("\"JO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JP, str);
                    Assert.AreEqual("\"JP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JQ, str);
                    Assert.AreEqual("\"JQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JR, str);
                    Assert.AreEqual("\"JR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JS, str);
                    Assert.AreEqual("\"JS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JT, str);
                    Assert.AreEqual("\"JT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JU, str);
                    Assert.AreEqual("\"JU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations9.JV, str);
                    Assert.AreEqual("\"JV\"", str.ToString());
                }
            }
            // 10
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AA, str);
                    Assert.AreEqual("\"AA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AB, str);
                    Assert.AreEqual("\"AB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AC, str);
                    Assert.AreEqual("\"AC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AD, str);
                    Assert.AreEqual("\"AD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AE, str);
                    Assert.AreEqual("\"AE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AF, str);
                    Assert.AreEqual("\"AF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AG, str);
                    Assert.AreEqual("\"AG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AH, str);
                    Assert.AreEqual("\"AH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AI, str);
                    Assert.AreEqual("\"AI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AJ, str);
                    Assert.AreEqual("\"AJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AK, str);
                    Assert.AreEqual("\"AK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AL, str);
                    Assert.AreEqual("\"AL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AM, str);
                    Assert.AreEqual("\"AM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AN, str);
                    Assert.AreEqual("\"AN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AO, str);
                    Assert.AreEqual("\"AO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AP, str);
                    Assert.AreEqual("\"AP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AQ, str);
                    Assert.AreEqual("\"AQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AR, str);
                    Assert.AreEqual("\"AR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AS, str);
                    Assert.AreEqual("\"AS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AT, str);
                    Assert.AreEqual("\"AT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AU, str);
                    Assert.AreEqual("\"AU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AV, str);
                    Assert.AreEqual("\"AV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AW, str);
                    Assert.AreEqual("\"AW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AX, str);
                    Assert.AreEqual("\"AX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AY, str);
                    Assert.AreEqual("\"AY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.AZ, str);
                    Assert.AreEqual("\"AZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BA, str);
                    Assert.AreEqual("\"BA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BB, str);
                    Assert.AreEqual("\"BB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BC, str);
                    Assert.AreEqual("\"BC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BD, str);
                    Assert.AreEqual("\"BD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BE, str);
                    Assert.AreEqual("\"BE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BF, str);
                    Assert.AreEqual("\"BF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BG, str);
                    Assert.AreEqual("\"BG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BH, str);
                    Assert.AreEqual("\"BH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BI, str);
                    Assert.AreEqual("\"BI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BJ, str);
                    Assert.AreEqual("\"BJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BK, str);
                    Assert.AreEqual("\"BK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BL, str);
                    Assert.AreEqual("\"BL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BM, str);
                    Assert.AreEqual("\"BM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BN, str);
                    Assert.AreEqual("\"BN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BO, str);
                    Assert.AreEqual("\"BO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BP, str);
                    Assert.AreEqual("\"BP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BQ, str);
                    Assert.AreEqual("\"BQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BR, str);
                    Assert.AreEqual("\"BR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BS, str);
                    Assert.AreEqual("\"BS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BT, str);
                    Assert.AreEqual("\"BT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BU, str);
                    Assert.AreEqual("\"BU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BV, str);
                    Assert.AreEqual("\"BV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BW, str);
                    Assert.AreEqual("\"BW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BX, str);
                    Assert.AreEqual("\"BX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BY, str);
                    Assert.AreEqual("\"BY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.BZ, str);
                    Assert.AreEqual("\"BZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CA, str);
                    Assert.AreEqual("\"CA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CB, str);
                    Assert.AreEqual("\"CB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CC, str);
                    Assert.AreEqual("\"CC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CD, str);
                    Assert.AreEqual("\"CD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CE, str);
                    Assert.AreEqual("\"CE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CF, str);
                    Assert.AreEqual("\"CF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CG, str);
                    Assert.AreEqual("\"CG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CH, str);
                    Assert.AreEqual("\"CH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CI, str);
                    Assert.AreEqual("\"CI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CJ, str);
                    Assert.AreEqual("\"CJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CK, str);
                    Assert.AreEqual("\"CK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CL, str);
                    Assert.AreEqual("\"CL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CM, str);
                    Assert.AreEqual("\"CM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CN, str);
                    Assert.AreEqual("\"CN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CO, str);
                    Assert.AreEqual("\"CO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CP, str);
                    Assert.AreEqual("\"CP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CQ, str);
                    Assert.AreEqual("\"CQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CR, str);
                    Assert.AreEqual("\"CR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CS, str);
                    Assert.AreEqual("\"CS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CT, str);
                    Assert.AreEqual("\"CT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CU, str);
                    Assert.AreEqual("\"CU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CV, str);
                    Assert.AreEqual("\"CV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CW, str);
                    Assert.AreEqual("\"CW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CX, str);
                    Assert.AreEqual("\"CX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CY, str);
                    Assert.AreEqual("\"CY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.CZ, str);
                    Assert.AreEqual("\"CZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DA, str);
                    Assert.AreEqual("\"DA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DB, str);
                    Assert.AreEqual("\"DB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DC, str);
                    Assert.AreEqual("\"DC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DD, str);
                    Assert.AreEqual("\"DD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DE, str);
                    Assert.AreEqual("\"DE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DF, str);
                    Assert.AreEqual("\"DF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DG, str);
                    Assert.AreEqual("\"DG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DH, str);
                    Assert.AreEqual("\"DH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DI, str);
                    Assert.AreEqual("\"DI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DJ, str);
                    Assert.AreEqual("\"DJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DK, str);
                    Assert.AreEqual("\"DK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DL, str);
                    Assert.AreEqual("\"DL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DM, str);
                    Assert.AreEqual("\"DM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DN, str);
                    Assert.AreEqual("\"DN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DO, str);
                    Assert.AreEqual("\"DO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DP, str);
                    Assert.AreEqual("\"DP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DQ, str);
                    Assert.AreEqual("\"DQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DR, str);
                    Assert.AreEqual("\"DR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DS, str);
                    Assert.AreEqual("\"DS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DT, str);
                    Assert.AreEqual("\"DT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DU, str);
                    Assert.AreEqual("\"DU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DV, str);
                    Assert.AreEqual("\"DV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DW, str);
                    Assert.AreEqual("\"DW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DX, str);
                    Assert.AreEqual("\"DX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DY, str);
                    Assert.AreEqual("\"DY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.DZ, str);
                    Assert.AreEqual("\"DZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EA, str);
                    Assert.AreEqual("\"EA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EB, str);
                    Assert.AreEqual("\"EB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EC, str);
                    Assert.AreEqual("\"EC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ED, str);
                    Assert.AreEqual("\"ED\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EE, str);
                    Assert.AreEqual("\"EE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EF, str);
                    Assert.AreEqual("\"EF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EG, str);
                    Assert.AreEqual("\"EG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EH, str);
                    Assert.AreEqual("\"EH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EI, str);
                    Assert.AreEqual("\"EI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EJ, str);
                    Assert.AreEqual("\"EJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EK, str);
                    Assert.AreEqual("\"EK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EL, str);
                    Assert.AreEqual("\"EL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EM, str);
                    Assert.AreEqual("\"EM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EN, str);
                    Assert.AreEqual("\"EN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EO, str);
                    Assert.AreEqual("\"EO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EP, str);
                    Assert.AreEqual("\"EP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EQ, str);
                    Assert.AreEqual("\"EQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ER, str);
                    Assert.AreEqual("\"ER\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ES, str);
                    Assert.AreEqual("\"ES\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ET, str);
                    Assert.AreEqual("\"ET\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EU, str);
                    Assert.AreEqual("\"EU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EV, str);
                    Assert.AreEqual("\"EV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EW, str);
                    Assert.AreEqual("\"EW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EX, str);
                    Assert.AreEqual("\"EX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EY, str);
                    Assert.AreEqual("\"EY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.EZ, str);
                    Assert.AreEqual("\"EZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FA, str);
                    Assert.AreEqual("\"FA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FB, str);
                    Assert.AreEqual("\"FB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FC, str);
                    Assert.AreEqual("\"FC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FD, str);
                    Assert.AreEqual("\"FD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FE, str);
                    Assert.AreEqual("\"FE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FF, str);
                    Assert.AreEqual("\"FF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FG, str);
                    Assert.AreEqual("\"FG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FH, str);
                    Assert.AreEqual("\"FH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FI, str);
                    Assert.AreEqual("\"FI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FJ, str);
                    Assert.AreEqual("\"FJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FK, str);
                    Assert.AreEqual("\"FK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FL, str);
                    Assert.AreEqual("\"FL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FM, str);
                    Assert.AreEqual("\"FM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FN, str);
                    Assert.AreEqual("\"FN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FO, str);
                    Assert.AreEqual("\"FO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FP, str);
                    Assert.AreEqual("\"FP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FQ, str);
                    Assert.AreEqual("\"FQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FR, str);
                    Assert.AreEqual("\"FR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FS, str);
                    Assert.AreEqual("\"FS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FT, str);
                    Assert.AreEqual("\"FT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FU, str);
                    Assert.AreEqual("\"FU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FV, str);
                    Assert.AreEqual("\"FV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FW, str);
                    Assert.AreEqual("\"FW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FX, str);
                    Assert.AreEqual("\"FX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FY, str);
                    Assert.AreEqual("\"FY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.FZ, str);
                    Assert.AreEqual("\"FZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GA, str);
                    Assert.AreEqual("\"GA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GB, str);
                    Assert.AreEqual("\"GB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GC, str);
                    Assert.AreEqual("\"GC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GD, str);
                    Assert.AreEqual("\"GD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GE, str);
                    Assert.AreEqual("\"GE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GF, str);
                    Assert.AreEqual("\"GF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GG, str);
                    Assert.AreEqual("\"GG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GH, str);
                    Assert.AreEqual("\"GH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GI, str);
                    Assert.AreEqual("\"GI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GJ, str);
                    Assert.AreEqual("\"GJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GK, str);
                    Assert.AreEqual("\"GK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GL, str);
                    Assert.AreEqual("\"GL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GM, str);
                    Assert.AreEqual("\"GM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GN, str);
                    Assert.AreEqual("\"GN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GO, str);
                    Assert.AreEqual("\"GO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GP, str);
                    Assert.AreEqual("\"GP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GQ, str);
                    Assert.AreEqual("\"GQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GR, str);
                    Assert.AreEqual("\"GR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GS, str);
                    Assert.AreEqual("\"GS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GT, str);
                    Assert.AreEqual("\"GT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GU, str);
                    Assert.AreEqual("\"GU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GV, str);
                    Assert.AreEqual("\"GV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GW, str);
                    Assert.AreEqual("\"GW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GX, str);
                    Assert.AreEqual("\"GX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GY, str);
                    Assert.AreEqual("\"GY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.GZ, str);
                    Assert.AreEqual("\"GZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HA, str);
                    Assert.AreEqual("\"HA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HB, str);
                    Assert.AreEqual("\"HB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HC, str);
                    Assert.AreEqual("\"HC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HD, str);
                    Assert.AreEqual("\"HD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HE, str);
                    Assert.AreEqual("\"HE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HF, str);
                    Assert.AreEqual("\"HF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HG, str);
                    Assert.AreEqual("\"HG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HH, str);
                    Assert.AreEqual("\"HH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HI, str);
                    Assert.AreEqual("\"HI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HJ, str);
                    Assert.AreEqual("\"HJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HK, str);
                    Assert.AreEqual("\"HK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HL, str);
                    Assert.AreEqual("\"HL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HM, str);
                    Assert.AreEqual("\"HM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HN, str);
                    Assert.AreEqual("\"HN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HO, str);
                    Assert.AreEqual("\"HO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HP, str);
                    Assert.AreEqual("\"HP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HQ, str);
                    Assert.AreEqual("\"HQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HR, str);
                    Assert.AreEqual("\"HR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HS, str);
                    Assert.AreEqual("\"HS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HT, str);
                    Assert.AreEqual("\"HT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HU, str);
                    Assert.AreEqual("\"HU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HV, str);
                    Assert.AreEqual("\"HV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HW, str);
                    Assert.AreEqual("\"HW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HX, str);
                    Assert.AreEqual("\"HX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HY, str);
                    Assert.AreEqual("\"HY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.HZ, str);
                    Assert.AreEqual("\"HZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IA, str);
                    Assert.AreEqual("\"IA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IB, str);
                    Assert.AreEqual("\"IB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IC, str);
                    Assert.AreEqual("\"IC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.ID, str);
                    Assert.AreEqual("\"ID\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IE, str);
                    Assert.AreEqual("\"IE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IF, str);
                    Assert.AreEqual("\"IF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IG, str);
                    Assert.AreEqual("\"IG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IH, str);
                    Assert.AreEqual("\"IH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.II, str);
                    Assert.AreEqual("\"II\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IJ, str);
                    Assert.AreEqual("\"IJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IK, str);
                    Assert.AreEqual("\"IK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IL, str);
                    Assert.AreEqual("\"IL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IM, str);
                    Assert.AreEqual("\"IM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IN, str);
                    Assert.AreEqual("\"IN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IO, str);
                    Assert.AreEqual("\"IO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IP, str);
                    Assert.AreEqual("\"IP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IQ, str);
                    Assert.AreEqual("\"IQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IR, str);
                    Assert.AreEqual("\"IR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IS, str);
                    Assert.AreEqual("\"IS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IT, str);
                    Assert.AreEqual("\"IT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IU, str);
                    Assert.AreEqual("\"IU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IV, str);
                    Assert.AreEqual("\"IV\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IW, str);
                    Assert.AreEqual("\"IW\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IX, str);
                    Assert.AreEqual("\"IX\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IY, str);
                    Assert.AreEqual("\"IY\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.IZ, str);
                    Assert.AreEqual("\"IZ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JA, str);
                    Assert.AreEqual("\"JA\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JB, str);
                    Assert.AreEqual("\"JB\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JC, str);
                    Assert.AreEqual("\"JC\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JD, str);
                    Assert.AreEqual("\"JD\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JE, str);
                    Assert.AreEqual("\"JE\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JF, str);
                    Assert.AreEqual("\"JF\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JG, str);
                    Assert.AreEqual("\"JG\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JH, str);
                    Assert.AreEqual("\"JH\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JI, str);
                    Assert.AreEqual("\"JI\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JJ, str);
                    Assert.AreEqual("\"JJ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JK, str);
                    Assert.AreEqual("\"JK\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JL, str);
                    Assert.AreEqual("\"JL\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JM, str);
                    Assert.AreEqual("\"JM\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JN, str);
                    Assert.AreEqual("\"JN\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JO, str);
                    Assert.AreEqual("\"JO\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JP, str);
                    Assert.AreEqual("\"JP\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JQ, str);
                    Assert.AreEqual("\"JQ\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JR, str);
                    Assert.AreEqual("\"JR\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JS, str);
                    Assert.AreEqual("\"JS\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JT, str);
                    Assert.AreEqual("\"JT\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JU, str);
                    Assert.AreEqual("\"JU\"", str.ToString());
                }
                using (var str = new StringWriter())
                {
                    JSON.Serialize(_EnumVariations10.JV, str);
                    Assert.AreEqual("\"JV\"", str.ToString());
                }
            }
        }

        [TestMethod]
        public void IntegerDictionaryKeys()
        {
            // byte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<byte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { byte.MinValue, "foo" },
                            { byte.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"255\":\"bar\"}", str.ToString());
                }
            }

            // sbyte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<sbyte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { sbyte.MinValue, "foo" },
                            { sbyte.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-128\":\"foo\",\"127\":\"bar\"}", str.ToString());
                }
            }

            // short
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<short, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { short.MinValue, "foo" },
                            { short.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-32768\":\"foo\",\"32767\":\"bar\"}", str.ToString());
                }
            }

            // ushort
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ushort, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ushort.MinValue, "foo" },
                            { ushort.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"65535\":\"bar\"}", str.ToString());
                }
            }

            // int
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<int, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { int.MinValue, "foo" },
                            { int.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-2147483648\":\"foo\",\"2147483647\":\"bar\"}", str.ToString());
                }
            }

            // uint
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<uint, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { uint.MinValue, "foo" },
                            { uint.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"4294967295\":\"bar\"}", str.ToString());
                }
            }

            // long
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<long, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { long.MinValue, "foo" },
                            { long.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"-9223372036854775808\":\"foo\",\"9223372036854775807\":\"bar\"}", str.ToString());
                }
            }

            // ulong
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ulong, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ulong.MinValue, "foo" },
                            { ulong.MaxValue, "bar" }
                        },
                        str
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"3\":null,\"0\":\"foo\",\"18446744073709551615\":\"bar\"}", str.ToString());
                }
            }
        }

        [TestMethod]
        public void IntegerDictionaryKeysWithoutNulls()
        {
            // byte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<byte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { byte.MinValue, "foo" },
                            { byte.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"255\":\"bar\"}", str.ToString());
                }
            }

            // sbyte
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<sbyte, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { sbyte.MinValue, "foo" },
                            { sbyte.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-128\":\"foo\",\"127\":\"bar\"}", str.ToString());
                }
            }

            // short
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<short, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { short.MinValue, "foo" },
                            { short.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-32768\":\"foo\",\"32767\":\"bar\"}", str.ToString());
                }
            }

            // ushort
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ushort, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ushort.MinValue, "foo" },
                            { ushort.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"65535\":\"bar\"}", str.ToString());
                }
            }

            // int
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<int, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { int.MinValue, "foo" },
                            { int.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-2147483648\":\"foo\",\"2147483647\":\"bar\"}", str.ToString());
                }
            }

            // uint
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<uint, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { uint.MinValue, "foo" },
                            { uint.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"4294967295\":\"bar\"}", str.ToString());
                }
            }

            // long
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<long, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { long.MinValue, "foo" },
                            { long.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"-9223372036854775808\":\"foo\",\"9223372036854775807\":\"bar\"}", str.ToString());
                }
            }

            // ulong
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new Dictionary<ulong, string>
                        {
                            { 1, "hello" },
                            { 2, "world" },
                            { 3, null },
                            { ulong.MinValue, "foo" },
                            { ulong.MaxValue, "bar" }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    Assert.AreEqual("{\"1\":\"hello\",\"2\":\"world\",\"0\":\"foo\",\"18446744073709551615\":\"bar\"}", str.ToString());
                }
            }
        }

        [TestMethod]
        public void Guids()
        {
            // defaults
            {
                using (var str = new StringWriter())
                {
                    var guid = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD");

                    JSON.Serialize(guid, str);

                    Assert.AreEqual("\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\"", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidLists = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") };

                    JSON.Serialize(guidLists, str);

                    Assert.AreEqual("[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"]", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidDict = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } };

                    JSON.Serialize(guidDict, str);

                    Assert.AreEqual("{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"}", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new
                        {
                            A = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"),
                            B = (Guid?)null,
                            C = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") },
                            D = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } }
                        },
                        str
                    );

                    var res = str.ToString();
                    Assert.AreEqual("{\"A\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"C\":[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"],\"D\":{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"},\"B\":null}", res);
                }
            }

            // exclude nulls
            {
                using (var str = new StringWriter())
                {
                    var guid = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD");

                    JSON.Serialize(guid, str, Options.ExcludeNulls);

                    Assert.AreEqual("\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\"", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidLists = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") };

                    JSON.Serialize(guidLists, str, Options.ExcludeNulls);

                    Assert.AreEqual("[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"]", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidDict = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } };

                    JSON.Serialize(guidDict, str, Options.ExcludeNulls);

                    Assert.AreEqual("{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"}", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new
                        {
                            A = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"),
                            B = (Guid?)null,
                            C = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") },
                            D = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } }
                        },
                        str,
                        Options.ExcludeNulls
                    );

                    var res = str.ToString();
                    Assert.AreEqual("{\"A\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"C\":[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\",\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"],\"D\":{\"hello\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\"world\":\"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"}}", res);
                }
            }

            // pretty print
            {
                using (var str = new StringWriter())
                {
                    var guid = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD");

                    JSON.Serialize(guid, str, Options.PrettyPrint);

                    Assert.AreEqual("\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\"", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidLists = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") };

                    JSON.Serialize(guidLists, str, Options.PrettyPrint);

                    Assert.AreEqual("[\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"]", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    var guidDict = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } };

                    JSON.Serialize(guidDict, str, Options.PrettyPrint);

                    Assert.AreEqual("{\n \"hello\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\n \"world\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"\n}", str.ToString());
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(
                        new
                        {
                            A = new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"),
                            B = (Guid?)null,
                            C = new List<Guid> { new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCC"), new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") },
                            D = new Dictionary<string, Guid> { { "hello", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCD") }, { "world", new Guid("DE01D5B0-069B-47EE-BFF2-8A1C10A32FCB") } }
                        },
                        str,
                        Options.PrettyPrint
                    );

                    var res = str.ToString();
                    Assert.AreEqual("{\n \"A\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\n \"C\": [\"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcc\", \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"],\n \"D\": {\n  \"hello\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcd\",\n  \"world\": \"de01d5b0-069b-47ee-bff2-8a1c10a32fcb\"\n },\n \"B\": null\n}", res);
                }
            }
        }

        [TestMethod]
        public void StringEscapes()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize("\"sup\b\t\f\n\r\0\"", str);
                Assert.AreEqual("\"\\\"sup\\b\\t\\f\\n\\r\\u0000\\\"\"", str.ToString());
            }

            // Don't waste time in JSON-mode
            using (var str = new StringWriter())
            {
                JSON.Serialize("\"sup\b\t\f\n\r\0\"\u2028\u2029", str);
                Assert.AreEqual("\"\\\"sup\\b\\t\\f\\n\\r\\u0000\\\"\u2028\u2029\"", str.ToString());
            }

            // But if this is JSONP, we have to spend some time encoding
            using (var str = new StringWriter())
            {
                JSON.Serialize("\"sup\b\t\f\n\r\0\"\u2028\u2029", str, Options.JSONP);
                Assert.AreEqual("\"\\\"sup\\b\\t\\f\\n\\r\\u0000\\\"\\u2028\\u2029\"", str.ToString());
            }
        }

        struct _SerializeDynamicStruct
        {
            public int A;
            public bool B;
        }

        [TestMethod]
        public void SerializeDynamic()
        {
            using (var str = new StringWriter())
            {
                JSON.SerializeDynamic(
                    null,
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("null", res);
            }

            using (var str = new StringWriter())
            {
                JSON.SerializeDynamic(
                    new
                    {
                        A = 1,
                        B = (int?)null,
                        C = "hello world"
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":1,\"B\":null,\"C\":\"hello world\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.SerializeDynamic(
                    new _SerializeDynamicStruct
                    {
                        A = 1,
                        B = false
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":1,\"B\":false}", res);
            }
        }

        [TestMethod]
        public void SerializeDynamicToString()
        {
            {
                var str = JSON.SerializeDynamic(null);

                Assert.AreEqual("null", str);
            }

            {
                var str = JSON.SerializeDynamic(
                    new
                    {
                        A = 1,
                        B = (int?)null,
                        C = "hello world"
                    }
                );

                Assert.AreEqual("{\"A\":1,\"B\":null,\"C\":\"hello world\"}", str);
            }

            {
                var str = JSON.SerializeDynamic(
                    new _SerializeDynamicStruct
                    {
                        A = 1,
                        B = false
                    }
                );

                Assert.AreEqual("{\"A\":1,\"B\":false}", str);
            }
        }

        class _LotsOfStrings
        {
            public string A;
            public string B;
            public string C;
            public string D;
            public string E;
            public string F;
            public string G;
            public string H;
            public string I;
            public string J;
            public string K;
        }

        [TestMethod]
        public void LotsOfStrings()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings { },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"K\":null,\"J\":null,\"I\":null,\"H\":null,\"G\":null,\"F\":null,\"E\":null,\"D\":null,\"C\":null,\"B\":null,\"A\":null}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings
                    {
                        A = "hello",
                        C = "world",
                        E = "fizz",
                        G = "buzz",
                        I = "foo",
                        K = "bar"
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"K\":\"bar\",\"J\":null,\"I\":\"foo\",\"H\":null,\"G\":\"buzz\",\"F\":null,\"E\":\"fizz\",\"D\":null,\"C\":\"world\",\"B\":null,\"A\":\"hello\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings
                    {
                        A = "hello",
                        B = "world",
                        D = "fizz",
                        E = "buzz",
                        G = "foo",
                        H = "bar",
                        J = "syn",
                        K = "ack"
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"K\":\"ack\",\"J\":\"syn\",\"I\":null,\"H\":\"bar\",\"G\":\"foo\",\"F\":null,\"E\":\"buzz\",\"D\":\"fizz\",\"C\":null,\"B\":\"world\",\"A\":\"hello\"}", res);
            }
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _LotsOfStrings
                    {
                        B = "hello",
                        C = "world",
                        E = "fizz",
                        F = "buzz",
                        H = "foo",
                        I = "bar",
                        K = "syn"
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"K\":\"syn\",\"J\":null,\"I\":\"bar\",\"H\":\"foo\",\"G\":null,\"F\":\"buzz\",\"E\":\"fizz\",\"D\":null,\"C\":\"world\",\"B\":\"hello\",\"A\":null}", res);
            }
        }

        // Type fairly similar to one in the Stack Exchange API that gave Jil some trouble
        class _Inherited<T> : System.Web.Mvc.ContentResult
            where T : class
        {
            public int? total { get; set; }
            public int? page_size { get; set; }
            public int? page { get; set; }
            public string type { get; set; }
            public List<T> items { get; set; }

            public int? quota_remaining { get; set; }
            public int? quota_max { get; set; }
            public int? backoff { get; set; }

            public int? error_id { get; set; }
            public string error_name { get; set; }
            public string error_message { get; set; }

            public bool? has_more { get; set; }
        }

        [TestMethod]
        public void Inherited()
        {
            var obj =
                new _Inherited<string>
                    {
                        ContentEncoding = Encoding.UTF8,

                        total = 1,
                        page_size = 2,
                        page = 3,
                        type = "foo",
                        items = new List<string> { "bar", "bizz", "buzz", "baz" },
                        quota_remaining = 4,
                        quota_max = 5,
                        backoff = 6,
                        error_id = 7,
                        error_message = "you don goofed",
                        has_more = true
                    };

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    obj,
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"items\":[\"bar\",\"bizz\",\"buzz\",\"baz\"],\"has_more\":true,\"error_id\":7,\"backoff\":6,\"quota_max\":5,\"quota_remaining\":4,\"page\":3,\"page_size\":2,\"total\":1,\"error_message\":\"you don goofed\",\"error_name\":null,\"type\":\"foo\"}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    obj,
                    str,
                    new Options(includeInherited: true)
                );

                var res = str.ToString();
                Assert.AreEqual("{\"items\":[\"bar\",\"bizz\",\"buzz\",\"baz\"],\"ContentEncoding\":{\"WindowsCodePage\":1200,\"IsBrowserDisplay\":true,\"IsBrowserSave\":true,\"IsMailNewsDisplay\":true,\"IsMailNewsSave\":true,\"IsSingleByte\":false,\"IsReadOnly\":true,\"CodePage\":65001,\"EncoderFallback\":{\"MaxCharCount\":1},\"DecoderFallback\":{\"MaxCharCount\":1},\"BodyName\":\"utf-8\",\"EncodingName\":\"Unicode (UTF-8)\",\"HeaderName\":\"utf-8\",\"WebName\":\"utf-8\"},\"Content\":null,\"ContentType\":null,\"has_more\":true,\"error_id\":7,\"backoff\":6,\"quota_max\":5,\"quota_remaining\":4,\"page\":3,\"page_size\":2,\"total\":1,\"error_message\":\"you don goofed\",\"error_name\":null,\"type\":\"foo\"}", res);
            }
        }

        [TestMethod]
        public void AllocationlessVsNormalDictionaries()
        {
            var data =
                new
                {
                    A =
                        new Dictionary<string, string>
                        {
                            { "hello", "world" },
                            { "fizz", null },
                            { "foo", "bar" },
                            { "init", "d" },
                            { "dev", null }
                        },
                    B = (IDictionary<string, string>)
                        new Dictionary<string, string>
                        {
                            { "hello", "world" },
                            { "fizz", null },
                            { "foo", "bar" },
                            { "init", "d" },
                            { "dev", null }
                        }
                };

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    data,
                    str,
                    Options.Default
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":{\"hello\":\"world\",\"fizz\":null,\"foo\":\"bar\",\"init\":\"d\",\"dev\":null},\"B\":{\"hello\":\"world\",\"fizz\":null,\"foo\":\"bar\",\"init\":\"d\",\"dev\":null}}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    data,
                    str,
                    Options.ExcludeNulls
                );

                var res = str.ToString();
                Assert.AreEqual("{\"A\":{\"hello\":\"world\",\"foo\":\"bar\",\"init\":\"d\"},\"B\":{\"hello\":\"world\",\"foo\":\"bar\",\"init\":\"d\"}}", res);
            }
        }

        [TestMethod]
        public void LessThan100()
        {
            for (var i = 0; i <= 100; i++)
            {
                var str = JSON.Serialize(i);

                Assert.AreEqual(i.ToString(), str);
            }
        }

        private class simplePoco
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }

        [TestMethod]
        public void SerializeNestedDictionary()
        {
            var items = new Dictionary<string, Dictionary<string, simplePoco>>();
            items.Add("a", new Dictionary<string, simplePoco>
            {
                {"a", new simplePoco {Id = 1, Name = "a"}}, 
                {"b", new simplePoco {Id = 2, Name = "b"}}
            });
            var json = JSON.Serialize(items);
            string expectedJson = "{\"a\":{\"a\":{\"Id\":1,\"Name\":\"a\"},\"b\":{\"Id\":2,\"Name\":\"b\"}}}";
            Assert.AreEqual(expectedJson, json);
        }

        class _DataMemberName
        {
            public string Plain { get; set; }

            [DataMember(Name = "FakeName")]
            public string RealName { get; set; }

            [DataMember(Name = "NotSoSecretName")]
            public int SecretName;
        }

        [TestMethod]
        public void DataMemberName()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _DataMemberName
                    {
                        Plain = "hello world",
                        RealName = "Really RealName",
                        SecretName = 314159
                    },
                    str
                );

                var res = str.ToString();
                Assert.AreEqual("{\"NotSoSecretName\":314159,\"FakeName\":\"Really RealName\",\"Plain\":\"hello world\"}", res);
            }
        }

        [TestMethod]
        public void BadDictionaryType()
        {
            try
            {
                JSON.Serialize(new Dictionary<double, int>());
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Error occurred building a serializer for System.Collections.Generic.Dictionary`2[System.Double,System.Int32]", e.Message);
                Assert.IsNotNull(e.InnerException);
                Assert.AreEqual("JSON dictionaries must have strings, enums, or integers as keys, found: System.Double", e.InnerException.Message);
            }
        }

        class _NoNameDataMember
        {
            [DataMember(Order = 1)]
            public int Id { get; set; }
        }

        [TestMethod]
        public void NoNameDataMember()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(new _NoNameDataMember { Id = 1234 }, str);
                var res = str.ToString();
                Assert.AreEqual("{\"Id\":1234}", res);
            }
        }

        class _DoubleWeirdCulture : IDisposable
        {
            CultureInfo RestoreToCulture;

            public _DoubleWeirdCulture(CultureInfo culture)
            {
                RestoreToCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = culture;
            }

            public void Dispose()
            {
                if (RestoreToCulture != null)
                {
                    Thread.CurrentThread.CurrentCulture = RestoreToCulture;
                    RestoreToCulture = null;
                }
            }
        }

        [TestMethod]
        public void DoubleWeirdCulture()
        {
            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var currentCulture = CultureInfo.CurrentCulture;
            var weirdCulture = allCultures.Where(c => c.NumberFormat.CurrencyDecimalSeparator != "." && c != currentCulture).First();

            using (new _DoubleWeirdCulture(weirdCulture))
            {
                Assert.AreEqual("123.456", JSON.Serialize(123.456));

                using (var str = new StringWriter())
                {
                    JSON.Serialize(123.456, str);
                    var res = str.ToString();
                    Assert.AreEqual("123.456", res);
                }
            }
        }

        class _DoubleConstantWeirdCulture
        {
            public double Const { get { return 3.14159; } }
        }

        [TestMethod]
        public void DoubleConstantWeirdCulture()
        {
            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var currentCulture = CultureInfo.CurrentCulture;
            var weirdCulture = allCultures.Where(c => c.NumberFormat.CurrencyDecimalSeparator != "." && c != currentCulture).First();

            using (new _DoubleWeirdCulture(weirdCulture))
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize(new _DoubleConstantWeirdCulture(), str);
                    var res = str.ToString();
                    Assert.AreEqual("{\"Const\":3.14159}", res);
                }
            }
        }

        class _Enumerables
        {
            public IEnumerable<int> A;
            public Dictionary<int, IEnumerable<int>> B;
            public List<IEnumerable<double>> C;
            public IEnumerable<IEnumerable<string>> D;
        }

        [TestMethod]
        public void Enumerables()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _Enumerables
                    {
                        A = new[] { 1, 2, 3 },
                        B = new Dictionary<int, IEnumerable<int>> { { 1, new[] { 2, 3 } }, { 2, new[] { 4, 5 } } },
                        C = new List<IEnumerable<double>> { new[] { 1.1, 2.2, 3.3 }, new[] { 4.4, 5.5, 6.6 } },
                        D = new[] { new[] { "hello", "world" }, new[] { "foo", "bar" } }
                    },
                    str
                );
                var res = str.ToString();
                Assert.AreEqual("{\"A\":[1,2,3],\"B\":{\"1\":[2,3],\"2\":[4,5]},\"C\":[[1.1,2.2,3.3],[4.4,5.5,6.6]],\"D\":[[\"hello\",\"world\"],[\"foo\",\"bar\"]]}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<IEnumerable<int>>(new[] { 1, 2, 3 }, str);
                var res = str.ToString();
                Assert.AreEqual("[1,2,3]", res);
            }
        }

        class _ReadOnlyLists
        {
            public IReadOnlyList<int> A;
            public Dictionary<int, IReadOnlyList<int>> B1;
            public IReadOnlyDictionary<int, IReadOnlyList<int>> B2;
            public List<IReadOnlyList<double>> C;
            public IReadOnlyList<IReadOnlyList<string>> D;
        }

        [TestMethod]
        public void ReadOnlyLists()
        {
            using (var str = new StringWriter())
            {
                JSON.Serialize(
                    new _ReadOnlyLists
                    {
                        A = new[] { 1, 2, 3 },
                        B1 = new Dictionary<int, IReadOnlyList<int>> { { 1, new[] { 2, 3 } }, { 2, new[] { 4, 5 } } },
                        B2 = new Dictionary<int, IReadOnlyList<int>> { { 1, new[] { 2, 3 } }, { 2, new[] { 4, 5 } } },
                        C = new List<IReadOnlyList<double>> { new[] { 1.1, 2.2, 3.3 }, new[] { 4.4, 5.5, 6.6 } },
                        D = new[] { new[] { "hello", "world" }, new[] { "foo", "bar" } }
                    },
                    str
                );
                var res = str.ToString();
                Assert.AreEqual("{\"A\":[1,2,3],\"B1\":{\"1\":[2,3],\"2\":[4,5]},\"B2\":{\"1\":[2,3],\"2\":[4,5]},\"C\":[[1.1,2.2,3.3],[4.4,5.5,6.6]],\"D\":[[\"hello\",\"world\"],[\"foo\",\"bar\"]]}", res);
            }

            using (var str = new StringWriter())
            {
                JSON.Serialize<IReadOnlyList<int>>(new[] { 1, 2, 3 }, str);
                var res = str.ToString();
                Assert.AreEqual("[1,2,3]", res);
            }
        }

        [Flags]
        enum _FlagsEnum
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8
        }

        [TestMethod]
        public void FlagsEnum()
        {
            {
                var a = JSON.Serialize(_FlagsEnum.A, Options.PrettyPrint);
                Assert.AreEqual(@"""A""", a);
                var b = JSON.Serialize(_FlagsEnum.B, Options.PrettyPrint);
                Assert.AreEqual(@"""B""", b);
                var c = JSON.Serialize(_FlagsEnum.C, Options.PrettyPrint);
                Assert.AreEqual(@"""C""", c);
                var d = JSON.Serialize(_FlagsEnum.D, Options.PrettyPrint);
                Assert.AreEqual(@"""D""", d);
                var ab = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.B, Options.PrettyPrint);
                Assert.AreEqual(@"""A, B""", ab);
                var ac = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.C, Options.PrettyPrint);
                Assert.AreEqual(@"""A, C""", ac);
                var ad = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.D, Options.PrettyPrint);
                Assert.AreEqual(@"""A, D""", ad);
                var bc = JSON.Serialize(_FlagsEnum.B | _FlagsEnum.C, Options.PrettyPrint);
                Assert.AreEqual(@"""B, C""", bc);
                var bd = JSON.Serialize(_FlagsEnum.B | _FlagsEnum.D, Options.PrettyPrint);
                Assert.AreEqual(@"""B, D""", bd);
                var cd = JSON.Serialize(_FlagsEnum.C | _FlagsEnum.D, Options.PrettyPrint);
                Assert.AreEqual(@"""C, D""", cd);
                var abc = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, Options.PrettyPrint);
                Assert.AreEqual(@"""A, B, C""", abc);
                var abd = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.D, Options.PrettyPrint);
                Assert.AreEqual(@"""A, B, D""", abd);
                var acd = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.C | _FlagsEnum.D, Options.PrettyPrint);
                Assert.AreEqual(@"""A, C, D""", acd);
                var bcd = JSON.Serialize(_FlagsEnum.B | _FlagsEnum.C | _FlagsEnum.D, Options.PrettyPrint);
                Assert.AreEqual(@"""B, C, D""", bcd);
            }

            {
                var a = JSON.Serialize(_FlagsEnum.A);
                Assert.AreEqual(@"""A""", a);
                var b = JSON.Serialize(_FlagsEnum.B);
                Assert.AreEqual(@"""B""", b);
                var c = JSON.Serialize(_FlagsEnum.C);
                Assert.AreEqual(@"""C""", c);
                var d = JSON.Serialize(_FlagsEnum.D);
                Assert.AreEqual(@"""D""", d);
                var ab = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.B);
                Assert.AreEqual(@"""A,B""", ab);
                var ac = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.C);
                Assert.AreEqual(@"""A,C""", ac);
                var ad = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.D);
                Assert.AreEqual(@"""A,D""", ad);
                var bc = JSON.Serialize(_FlagsEnum.B | _FlagsEnum.C);
                Assert.AreEqual(@"""B,C""", bc);
                var bd = JSON.Serialize(_FlagsEnum.B | _FlagsEnum.D);
                Assert.AreEqual(@"""B,D""", bd);
                var cd = JSON.Serialize(_FlagsEnum.C | _FlagsEnum.D);
                Assert.AreEqual(@"""C,D""", cd);
                var abc = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C);
                Assert.AreEqual(@"""A,B,C""", abc);
                var abd = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.D);
                Assert.AreEqual(@"""A,B,D""", abd);
                var acd = JSON.Serialize(_FlagsEnum.A | _FlagsEnum.C | _FlagsEnum.D);
                Assert.AreEqual(@"""A,C,D""", acd);
                var bcd = JSON.Serialize(_FlagsEnum.B | _FlagsEnum.C | _FlagsEnum.D);
                Assert.AreEqual(@"""B,C,D""", bcd);
            }
        }

        [Flags]
        enum _FlagsEnumWithZero
        {
            None = 0,
            A = 1,
            B = 2,
            C = 4,
            D = 8
        }

        [TestMethod]
        public void FlagsEnumWithZero()
        {
            {
                var none = JSON.Serialize(_FlagsEnumWithZero.None, Options.PrettyPrint);
                Assert.AreEqual(@"""None""", none);
                var a = JSON.Serialize(_FlagsEnumWithZero.A, Options.PrettyPrint);
                Assert.AreEqual(@"""A""", a);
                var b = JSON.Serialize(_FlagsEnumWithZero.B, Options.PrettyPrint);
                Assert.AreEqual(@"""B""", b);
                var c = JSON.Serialize(_FlagsEnumWithZero.C, Options.PrettyPrint);
                Assert.AreEqual(@"""C""", c);
                var d = JSON.Serialize(_FlagsEnumWithZero.D, Options.PrettyPrint);
                Assert.AreEqual(@"""D""", d);
                var ab = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.B, Options.PrettyPrint);
                Assert.AreEqual(@"""A, B""", ab);
                var ac = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.C, Options.PrettyPrint);
                Assert.AreEqual(@"""A, C""", ac);
                var ad = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.D, Options.PrettyPrint);
                Assert.AreEqual(@"""A, D""", ad);
                var bc = JSON.Serialize(_FlagsEnumWithZero.B | _FlagsEnumWithZero.C, Options.PrettyPrint);
                Assert.AreEqual(@"""B, C""", bc);
                var bd = JSON.Serialize(_FlagsEnumWithZero.B | _FlagsEnumWithZero.D, Options.PrettyPrint);
                Assert.AreEqual(@"""B, D""", bd);
                var cd = JSON.Serialize(_FlagsEnumWithZero.C | _FlagsEnumWithZero.D, Options.PrettyPrint);
                Assert.AreEqual(@"""C, D""", cd);
                var abc = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.B | _FlagsEnumWithZero.C, Options.PrettyPrint);
                Assert.AreEqual(@"""A, B, C""", abc);
                var abd = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.B | _FlagsEnumWithZero.D, Options.PrettyPrint);
                Assert.AreEqual(@"""A, B, D""", abd);
                var acd = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.C | _FlagsEnumWithZero.D, Options.PrettyPrint);
                Assert.AreEqual(@"""A, C, D""", acd);
                var bcd = JSON.Serialize(_FlagsEnumWithZero.B | _FlagsEnumWithZero.C | _FlagsEnumWithZero.D, Options.PrettyPrint);
                Assert.AreEqual(@"""B, C, D""", bcd);
            }

            {
                var none = JSON.Serialize(_FlagsEnumWithZero.None);
                Assert.AreEqual(@"""None""", none);
                var a = JSON.Serialize(_FlagsEnumWithZero.A);
                Assert.AreEqual(@"""A""", a);
                var b = JSON.Serialize(_FlagsEnumWithZero.B);
                Assert.AreEqual(@"""B""", b);
                var c = JSON.Serialize(_FlagsEnumWithZero.C);
                Assert.AreEqual(@"""C""", c);
                var d = JSON.Serialize(_FlagsEnumWithZero.D);
                Assert.AreEqual(@"""D""", d);
                var ab = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.B);
                Assert.AreEqual(@"""A,B""", ab);
                var ac = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.C);
                Assert.AreEqual(@"""A,C""", ac);
                var ad = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.D);
                Assert.AreEqual(@"""A,D""", ad);
                var bc = JSON.Serialize(_FlagsEnumWithZero.B | _FlagsEnumWithZero.C);
                Assert.AreEqual(@"""B,C""", bc);
                var bd = JSON.Serialize(_FlagsEnumWithZero.B | _FlagsEnumWithZero.D);
                Assert.AreEqual(@"""B,D""", bd);
                var cd = JSON.Serialize(_FlagsEnumWithZero.C | _FlagsEnumWithZero.D);
                Assert.AreEqual(@"""C,D""", cd);
                var abc = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.B | _FlagsEnumWithZero.C);
                Assert.AreEqual(@"""A,B,C""", abc);
                var abd = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.B | _FlagsEnumWithZero.D);
                Assert.AreEqual(@"""A,B,D""", abd);
                var acd = JSON.Serialize(_FlagsEnumWithZero.A | _FlagsEnumWithZero.C | _FlagsEnumWithZero.D);
                Assert.AreEqual(@"""A,C,D""", acd);
                var bcd = JSON.Serialize(_FlagsEnumWithZero.B | _FlagsEnumWithZero.C | _FlagsEnumWithZero.D);
                Assert.AreEqual(@"""B,C,D""", bcd);
            }
        }

        enum _EnumMemberAttributeOverride
        {
            [EnumMember(Value = "1")]
            A,
            [EnumMember(Value = "2")]
            B,
            [EnumMember(Value = "3")]
            C
        }

        [TestMethod]
        public void EnumMemberAttributeOverride()
        {
            Assert.AreEqual("\"1\"", JSON.Serialize(_EnumMemberAttributeOverride.A));
            Assert.AreEqual("\"2\"", JSON.Serialize(_EnumMemberAttributeOverride.B));
            Assert.AreEqual("\"3\"", JSON.Serialize(_EnumMemberAttributeOverride.C));
        }

        [Flags]
        enum _EnumMemberAttributeOverrideFlags
        {
            [EnumMember(Value = "1")]
            A = 1,
            [EnumMember(Value = "2")]
            B = 2,
            [EnumMember(Value = "4")]
            C = 4
        }

        [TestMethod]
        public void EnumMemberAttributeOverrideFlags()
        {
            {
                Assert.AreEqual("\"1\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A, Options.PrettyPrint));
                Assert.AreEqual("\"2\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.B, Options.PrettyPrint));
                Assert.AreEqual("\"4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.C, Options.PrettyPrint));

                Assert.AreEqual("\"1, 2\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, Options.PrettyPrint));
                Assert.AreEqual("\"1, 4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, Options.PrettyPrint));
                Assert.AreEqual("\"2, 4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, Options.PrettyPrint));

                Assert.AreEqual("\"1, 2, 4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, Options.PrettyPrint));
            }

            {
                Assert.AreEqual("\"1\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A));
                Assert.AreEqual("\"2\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.B));
                Assert.AreEqual("\"4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.C));

                Assert.AreEqual("\"1,2\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B));
                Assert.AreEqual("\"1,4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C));
                Assert.AreEqual("\"2,4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C));

                Assert.AreEqual("\"1,2,4\"", JSON.Serialize(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C));
            }
        }

        [Flags]
        enum _EnumMemberAttributeOverrideWithNoneFlags
        {
            [EnumMember(Value = "0")]
            None = 0,
            [EnumMember(Value = "1")]
            A = 1,
            [EnumMember(Value = "2")]
            B = 2,
            [EnumMember(Value = "4")]
            C = 4
        }

        [TestMethod]
        public void EnumMemberAttributeOverrideWithNoneFlags()
        {
            {
                Assert.AreEqual("\"0\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.None, Options.PrettyPrint));

                Assert.AreEqual("\"1\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A, Options.PrettyPrint));
                Assert.AreEqual("\"2\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.B, Options.PrettyPrint));
                Assert.AreEqual("\"4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.C, Options.PrettyPrint));

                Assert.AreEqual("\"1, 2\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A | _EnumMemberAttributeOverrideWithNoneFlags.B, Options.PrettyPrint));
                Assert.AreEqual("\"1, 4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A | _EnumMemberAttributeOverrideWithNoneFlags.C, Options.PrettyPrint));
                Assert.AreEqual("\"2, 4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.B | _EnumMemberAttributeOverrideWithNoneFlags.C, Options.PrettyPrint));

                Assert.AreEqual("\"1, 2, 4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A | _EnumMemberAttributeOverrideWithNoneFlags.B | _EnumMemberAttributeOverrideWithNoneFlags.C, Options.PrettyPrint));
            }

            {
                Assert.AreEqual("\"0\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.None));

                Assert.AreEqual("\"1\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A));
                Assert.AreEqual("\"2\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.B));
                Assert.AreEqual("\"4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.C));

                Assert.AreEqual("\"1,2\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A | _EnumMemberAttributeOverrideWithNoneFlags.B));
                Assert.AreEqual("\"1,4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A | _EnumMemberAttributeOverrideWithNoneFlags.C));
                Assert.AreEqual("\"2,4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.B | _EnumMemberAttributeOverrideWithNoneFlags.C));

                Assert.AreEqual("\"1,2,4\"", JSON.Serialize(_EnumMemberAttributeOverrideWithNoneFlags.A | _EnumMemberAttributeOverrideWithNoneFlags.B | _EnumMemberAttributeOverrideWithNoneFlags.C));
            }
        }

        class _ReuseTypeSerializers1
        {
            public class _ReuseTypeSerializers2
            {
                public string A { get; set; }
                public int B { get; set; }
            }

            public _ReuseTypeSerializers2 A { get; set; }
            public _ReuseTypeSerializers2 B { get; set; }
        }

        [TestMethod]
        public void ReuseTypeSerializers()
        {
            var str = JSON.Serialize(new _ReuseTypeSerializers1 { A = new _ReuseTypeSerializers1._ReuseTypeSerializers2 { A = "hello", B = 123 }, B = new _ReuseTypeSerializers1._ReuseTypeSerializers2 { A = "world", B = 456 } });
            Assert.AreEqual("{\"A\":{\"B\":123,\"A\":\"hello\"},\"B\":{\"B\":456,\"A\":\"world\"}}", str);
        }

        public class _ApiResult<T>
        {
            public int? total { get; private set; }
            public int? page_size { get; private set; }
            public int? page { get; private set; }
            public string type { get; private set; }
            public List<T> items { get; internal set; }
            public int? quota_remaining { get; private set; }
            public int? quota_max { get; private set; }
            public int? backoff { get; private set; }
            public int? error_id { get; private set; }
            public string error_name { get; private set; }
            public string error_message { get; private set; }
            public bool? has_more { get; private set; }
        }

        class _FlagOption
        {
            public int? option_id { get; set; }
            public bool? requires_comment { get; set; }
            public bool? requires_site { get; set; }
            public bool? requires_question_id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public List<_FlagOption> sub_options { get; set; }
            public bool? has_flagged { get; set; }
            public int? count { get; set; }
            public string dialog_title { get; set; }
        }

        [TestMethod]
        public void FlagOption()
        {
            {
                var foo = JSON.Serialize(new _ApiResult<_FlagOption>());
                Assert.IsNotNull(foo);
            }

            {
                var asStr = "{\"total\":6,\"page_size\":30,\"page\":1,\"type\":\"flag_option\",\"items\":[{\"option_id\":46534,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"it is spam\",\"description\":\"This question is effectively an advertisement with no disclosure. It is not useful or relevant, but promotional.\",\"sub_options\":null,\"has_flagged\":false,\"count\":null,\"dialog_title\":\"I am flagging this question because\"},{\"option_id\":7852,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"it is offensive, abusive, or hate speech\",\"description\":\"This question contains content that a reasonable person would deem inappropriate for respectful discourse.\",\"sub_options\":null,\"has_flagged\":false,\"count\":null,\"dialog_title\":\"I am flagging this question because\"},{\"option_id\":null,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"it is a duplicate...\",\"description\":\"This question has been asked before and already has an answer.\",\"sub_options\":[{\"option_id\":8762,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":true,\"title\":\"\\\"How can I get a HOT network questions week digest?\\\" is a duplicate of:\",\"description\":null,\"sub_options\":null,\"has_flagged\":null,\"count\":null,\"dialog_title\":\"Duplicate\"},{\"option_id\":42580,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Don't let questions stick to the top of the hot questions list forever;\\r\\nduplicate of...\",\"description\":\"I've noticed that some very highly upvoted questions stay in the list of hot questions (also displayed in the Stack Exchange menu on the top left) for a very long time, often for several days. One ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":22396,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"In “network hot” questions formula, discard answers when voting evidence indicates that these are not good data points;\\r\\nduplicate of...\",\"description\":\"TL;DR When votes of 20... 30... 100 users clearly indicate that only one or two answers are popular, it does not make sense to pretend that other answers are popular too.\\n\\n\\n\\nIn current version of ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":34792,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Prevent specific sites from being overrepresented in the hot questions list;\\r\\nduplicate of...\",\"description\":\"Some sites appear far more often in the hot questions list than other sites. I don't have any hard data on that, as that kind of data is just not publicly available, but I'm pretty sure my subjective ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":53441,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"What is the Goal of \\\"Hot Network Questions\\\"?;\\r\\nduplicate of...\",\"description\":\"There has been a tug-of-war in the hot-questions list.\\n\\nCommunity members like JonW seem to be unhappy with the traffic that it brings to their site:\\n\\n\\n  'But we want to encourage people to post, ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":54702,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Filtering \\\"hot\\\" questions;\\r\\nduplicate of...\",\"description\":\"Is there a way to filter the \\\"hot questions\\\" displayed in the Stack Exchange menu?\\n\\n\\n\\nI have a profile on Stack Overflow and that's pretty much it. Therefore the list as it is now is pretty ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":5162,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Instant e-mail notifications of answers to questions;\\r\\nduplicate of...\",\"description\":\"I'd like to have Instant notifications by email of answers to my questions.  Can this be implemented?\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":8641,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Extend the new Hot Questions sidebar;\\r\\nduplicate of...\",\"description\":\"I really like the new sidebar. With the new top bar eliminating the \\\"Hot Questions\\\" area there, I think this is the perfect place for it. One-click access to time-wasting questions about topics I may ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":42868,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Hot Questions, inability to filter sites... and now German questions? Too much \\\"noise\\\"!;\\r\\nduplicate of...\",\"description\":\"We used to have the ability to filter Stack Exchange wide stories. I was sad to see the ability go away. I'll admit it isn't the end of the world. I do like seeing questions on occasion that I ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"}],\"has_flagged\":false,\"count\":null,\"dialog_title\":\"I am flagging this question because\"},{\"option_id\":null,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"it should be closed for another reason...\",\"description\":\"This question does not meet this site's standards and should be closed.\",\"sub_options\":[{\"option_id\":null,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"duplicate of...\",\"description\":\"This question has been asked before and already has an answer.\",\"sub_options\":[{\"option_id\":58780,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":true,\"title\":\"\\\"How can I get a HOT network questions week digest?\\\" is a duplicate of:\",\"description\":null,\"sub_options\":null,\"has_flagged\":null,\"count\":null,\"dialog_title\":\"Duplicate\"},{\"option_id\":60782,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Don't let questions stick to the top of the hot questions list forever;\\r\\nduplicate of...\",\"description\":\"I've noticed that some very highly upvoted questions stay in the list of hot questions (also displayed in the Stack Exchange menu on the top left) for a very long time, often for several days. One ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":10563,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"In “network hot” questions formula, discard answers when voting evidence indicates that these are not good data points;\\r\\nduplicate of...\",\"description\":\"TL;DR When votes of 20... 30... 100 users clearly indicate that only one or two answers are popular, it does not make sense to pretend that other answers are popular too.\\n\\n\\n\\nIn current version of ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":47075,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Prevent specific sites from being overrepresented in the hot questions list;\\r\\nduplicate of...\",\"description\":\"Some sites appear far more often in the hot questions list than other sites. I don't have any hard data on that, as that kind of data is just not publicly available, but I'm pretty sure my subjective ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":11110,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"What is the Goal of \\\"Hot Network Questions\\\"?;\\r\\nduplicate of...\",\"description\":\"There has been a tug-of-war in the hot-questions list.\\n\\nCommunity members like JonW seem to be unhappy with the traffic that it brings to their site:\\n\\n\\n  'But we want to encourage people to post, ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":31605,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Filtering \\\"hot\\\" questions;\\r\\nduplicate of...\",\"description\":\"Is there a way to filter the \\\"hot questions\\\" displayed in the Stack Exchange menu?\\n\\n\\n\\nI have a profile on Stack Overflow and that's pretty much it. Therefore the list as it is now is pretty ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":1812,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Instant e-mail notifications of answers to questions;\\r\\nduplicate of...\",\"description\":\"I'd like to have Instant notifications by email of answers to my questions.  Can this be implemented?\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":61901,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Extend the new Hot Questions sidebar;\\r\\nduplicate of...\",\"description\":\"I really like the new sidebar. With the new top bar eliminating the \\\"Hot Questions\\\" area there, I think this is the perfect place for it. One-click access to time-wasting questions about topics I may ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"},{\"option_id\":345,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"Hot Questions, inability to filter sites... and now German questions? Too much \\\"noise\\\"!;\\r\\nduplicate of...\",\"description\":\"We used to have the ability to filter Stack Exchange wide stories. I was sad to see the ability go away. I'll admit it isn't the end of the world. I do like seeing questions on occasion that I ...\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Duplicate\"}],\"has_flagged\":false,\"count\":0,\"dialog_title\":\"Why should this question be closed?\"},{\"option_id\":null,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"off-topic because...\",\"description\":\"This question does not appear to be about Stack Overflow or the software that powers the Stack Exchange <a href=\\\"http://stackexchange.com/sites\\\">network</a> within the scope defined in the help center.\",\"sub_options\":[{\"option_id\":55230,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":null,\"description\":\"This question <b>does not appear to seek input and discussion</b> from the community. If you have encountered a problem on one of our sites, please describe it in detail. See also: <a href=\\\"http://meta.stackexchange.com/help/whats-meta\\\">What is \\\"meta\\\"? How does it work?</a>\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Off-Topic\"},{\"option_id\":19261,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":null,\"description\":\"The problem described here <b>can no longer be reproduced</b>. Changes to the system or to the circumstances affecting the asker have rendered it obsolete. If you encounter a similar problem, please post a new question.\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Off-Topic\"},{\"option_id\":22924,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":null,\"description\":\"This question <b>pertains only to a specific site</b> in the Stack Exchange Network. Questions on Meta Stack Exchange should pertain to our network or software that drives it as a whole, within the guidelines defined in <a href=\\\"http://meta.stackexchange.com/help/on-topic\\\">the help center</a>. You should ask this question on the meta site where your concern originated.\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Off-Topic\"},{\"option_id\":20878,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":null,\"description\":\"This question does not appear to be about Stack Overflow or the software that powers the Stack Exchange <a href=\\\"http://stackexchange.com/sites\\\">network</a>, within the scope defined in the <a href=\\\"http://local.mso.com/help\\\">help center</a>.\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Off-Topic\"},{\"option_id\":null,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":null,\"description\":\"This question belongs on another site in the Stack Exchange network\",\"sub_options\":[{\"option_id\":43510,\"requires_comment\":false,\"requires_site\":true,\"requires_question_id\":false,\"title\":\"belongs on another site in the Stack Exchange network\",\"description\":null,\"sub_options\":null,\"has_flagged\":null,\"count\":null,\"dialog_title\":\"Migration\"}],\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Off-Topic\"},{\"option_id\":47170,\"requires_comment\":true,\"requires_site\":false,\"requires_question_id\":false,\"title\":null,\"description\":\"Other (add a comment explaining what is wrong)\",\"sub_options\":null,\"has_flagged\":null,\"count\":0,\"dialog_title\":\"Off-Topic\"}],\"has_flagged\":false,\"count\":0,\"dialog_title\":\"Why should this question be closed?\"},{\"option_id\":11866,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"unclear what you're asking\",\"description\":\"Please clarify your specific problem or add additional details to highlight exactly what you need. As it's currently written, it’s hard to tell exactly what you're asking.\",\"sub_options\":null,\"has_flagged\":false,\"count\":0,\"dialog_title\":\"Why should this question be closed?\"},{\"option_id\":18198,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"too broad\",\"description\":\"There are either too many possible answers, or good answers would be too long for this format. Please add details to narrow the answer set or to isolate an issue that can be answered in a few paragraphs.\",\"sub_options\":null,\"has_flagged\":false,\"count\":0,\"dialog_title\":\"Why should this question be closed?\"},{\"option_id\":8528,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"primarily opinion-based\",\"description\":\"Many good questions generate some degree of opinion based on expert experience, but answers to this question will tend to be almost entirely based on opinions, rather than facts, references, or specific expertise.\",\"sub_options\":null,\"has_flagged\":false,\"count\":0,\"dialog_title\":\"Why should this question be closed?\"}],\"has_flagged\":false,\"count\":null,\"dialog_title\":\"I am flagging this question because\"},{\"option_id\":36772,\"requires_comment\":false,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"it is very low quality\",\"description\":\"This question has severe formatting or content problems. This question is unlikely to be salvageable through editing, and might need to be removed.\",\"sub_options\":null,\"has_flagged\":null,\"count\":null,\"dialog_title\":\"I am flagging this question because\"},{\"option_id\":34976,\"requires_comment\":true,\"requires_site\":false,\"requires_question_id\":false,\"title\":\"other (needs ♦ moderator attention)\",\"description\":\"This question needs a moderator's attention. Please describe exactly what's wrong.\",\"sub_options\":null,\"has_flagged\":null,\"count\":null,\"dialog_title\":\"I am flagging this question because\"}],\"quota_remaining\":9996,\"quota_max\":10000,\"backoff\":10,\"error_id\":null,\"error_name\":null,\"error_message\":null,\"has_more\":false,\"Content\":null,\"ContentEncoding\":null,\"ContentType\":null}";
                var asObj = JSON.Deserialize<_ApiResult<_FlagOption>>(asStr);
                var foo = JSON.Serialize(asObj);
                Assert.AreEqual("{\"items\":[{\"sub_options\":null,\"count\":null,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":46534,\"dialog_title\":\"I am flagging this question because\",\"description\":\"This question is effectively an advertisement with no disclosure. It is not useful or relevant, but promotional.\",\"title\":\"it is spam\"},{\"sub_options\":null,\"count\":null,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":7852,\"dialog_title\":\"I am flagging this question because\",\"description\":\"This question contains content that a reasonable person would deem inappropriate for respectful discourse.\",\"title\":\"it is offensive, abusive, or hate speech\"},{\"sub_options\":[{\"sub_options\":null,\"count\":null,\"has_flagged\":null,\"requires_question_id\":true,\"requires_site\":false,\"requires_comment\":false,\"option_id\":8762,\"dialog_title\":\"Duplicate\",\"description\":null,\"title\":\"\\\"How can I get a HOT network questions week digest?\\\" is a duplicate of:\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":42580,\"dialog_title\":\"Duplicate\",\"description\":\"I've noticed that some very highly upvoted questions stay in the list of hot questions (also displayed in the Stack Exchange menu on the top left) for a very long time, often for several days. One ...\",\"title\":\"Don't let questions stick to the top of the hot questions list forever;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":22396,\"dialog_title\":\"Duplicate\",\"description\":\"TL;DR When votes of 20... 30... 100 users clearly indicate that only one or two answers are popular, it does not make sense to pretend that other answers are popular too.\\n\\n\\n\\nIn current version of ...\",\"title\":\"In “network hot” questions formula, discard answers when voting evidence indicates that these are not good data points;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":34792,\"dialog_title\":\"Duplicate\",\"description\":\"Some sites appear far more often in the hot questions list than other sites. I don't have any hard data on that, as that kind of data is just not publicly available, but I'm pretty sure my subjective ...\",\"title\":\"Prevent specific sites from being overrepresented in the hot questions list;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":53441,\"dialog_title\":\"Duplicate\",\"description\":\"There has been a tug-of-war in the hot-questions list.\\n\\nCommunity members like JonW seem to be unhappy with the traffic that it brings to their site:\\n\\n\\n  'But we want to encourage people to post, ...\",\"title\":\"What is the Goal of \\\"Hot Network Questions\\\"?;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":54702,\"dialog_title\":\"Duplicate\",\"description\":\"Is there a way to filter the \\\"hot questions\\\" displayed in the Stack Exchange menu?\\n\\n\\n\\nI have a profile on Stack Overflow and that's pretty much it. Therefore the list as it is now is pretty ...\",\"title\":\"Filtering \\\"hot\\\" questions;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":5162,\"dialog_title\":\"Duplicate\",\"description\":\"I'd like to have Instant notifications by email of answers to my questions.  Can this be implemented?\",\"title\":\"Instant e-mail notifications of answers to questions;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":8641,\"dialog_title\":\"Duplicate\",\"description\":\"I really like the new sidebar. With the new top bar eliminating the \\\"Hot Questions\\\" area there, I think this is the perfect place for it. One-click access to time-wasting questions about topics I may ...\",\"title\":\"Extend the new Hot Questions sidebar;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":42868,\"dialog_title\":\"Duplicate\",\"description\":\"We used to have the ability to filter Stack Exchange wide stories. I was sad to see the ability go away. I'll admit it isn't the end of the world. I do like seeing questions on occasion that I ...\",\"title\":\"Hot Questions, inability to filter sites... and now German questions? Too much \\\"noise\\\"!;\\r\\nduplicate of...\"}],\"count\":null,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":null,\"dialog_title\":\"I am flagging this question because\",\"description\":\"This question has been asked before and already has an answer.\",\"title\":\"it is a duplicate...\"},{\"sub_options\":[{\"sub_options\":[{\"sub_options\":null,\"count\":null,\"has_flagged\":null,\"requires_question_id\":true,\"requires_site\":false,\"requires_comment\":false,\"option_id\":58780,\"dialog_title\":\"Duplicate\",\"description\":null,\"title\":\"\\\"How can I get a HOT network questions week digest?\\\" is a duplicate of:\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":60782,\"dialog_title\":\"Duplicate\",\"description\":\"I've noticed that some very highly upvoted questions stay in the list of hot questions (also displayed in the Stack Exchange menu on the top left) for a very long time, often for several days. One ...\",\"title\":\"Don't let questions stick to the top of the hot questions list forever;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":10563,\"dialog_title\":\"Duplicate\",\"description\":\"TL;DR When votes of 20... 30... 100 users clearly indicate that only one or two answers are popular, it does not make sense to pretend that other answers are popular too.\\n\\n\\n\\nIn current version of ...\",\"title\":\"In “network hot” questions formula, discard answers when voting evidence indicates that these are not good data points;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":47075,\"dialog_title\":\"Duplicate\",\"description\":\"Some sites appear far more often in the hot questions list than other sites. I don't have any hard data on that, as that kind of data is just not publicly available, but I'm pretty sure my subjective ...\",\"title\":\"Prevent specific sites from being overrepresented in the hot questions list;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":11110,\"dialog_title\":\"Duplicate\",\"description\":\"There has been a tug-of-war in the hot-questions list.\\n\\nCommunity members like JonW seem to be unhappy with the traffic that it brings to their site:\\n\\n\\n  'But we want to encourage people to post, ...\",\"title\":\"What is the Goal of \\\"Hot Network Questions\\\"?;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":31605,\"dialog_title\":\"Duplicate\",\"description\":\"Is there a way to filter the \\\"hot questions\\\" displayed in the Stack Exchange menu?\\n\\n\\n\\nI have a profile on Stack Overflow and that's pretty much it. Therefore the list as it is now is pretty ...\",\"title\":\"Filtering \\\"hot\\\" questions;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":1812,\"dialog_title\":\"Duplicate\",\"description\":\"I'd like to have Instant notifications by email of answers to my questions.  Can this be implemented?\",\"title\":\"Instant e-mail notifications of answers to questions;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":61901,\"dialog_title\":\"Duplicate\",\"description\":\"I really like the new sidebar. With the new top bar eliminating the \\\"Hot Questions\\\" area there, I think this is the perfect place for it. One-click access to time-wasting questions about topics I may ...\",\"title\":\"Extend the new Hot Questions sidebar;\\r\\nduplicate of...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":345,\"dialog_title\":\"Duplicate\",\"description\":\"We used to have the ability to filter Stack Exchange wide stories. I was sad to see the ability go away. I'll admit it isn't the end of the world. I do like seeing questions on occasion that I ...\",\"title\":\"Hot Questions, inability to filter sites... and now German questions? Too much \\\"noise\\\"!;\\r\\nduplicate of...\"}],\"count\":0,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":null,\"dialog_title\":\"Why should this question be closed?\",\"description\":\"This question has been asked before and already has an answer.\",\"title\":\"duplicate of...\"},{\"sub_options\":[{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":55230,\"dialog_title\":\"Off-Topic\",\"description\":\"This question <b>does not appear to seek input and discussion</b> from the community. If you have encountered a problem on one of our sites, please describe it in detail. See also: <a href=\\\"http://meta.stackexchange.com/help/whats-meta\\\">What is \\\"meta\\\"? How does it work?</a>\",\"title\":null},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":19261,\"dialog_title\":\"Off-Topic\",\"description\":\"The problem described here <b>can no longer be reproduced</b>. Changes to the system or to the circumstances affecting the asker have rendered it obsolete. If you encounter a similar problem, please post a new question.\",\"title\":null},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":22924,\"dialog_title\":\"Off-Topic\",\"description\":\"This question <b>pertains only to a specific site</b> in the Stack Exchange Network. Questions on Meta Stack Exchange should pertain to our network or software that drives it as a whole, within the guidelines defined in <a href=\\\"http://meta.stackexchange.com/help/on-topic\\\">the help center</a>. You should ask this question on the meta site where your concern originated.\",\"title\":null},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":20878,\"dialog_title\":\"Off-Topic\",\"description\":\"This question does not appear to be about Stack Overflow or the software that powers the Stack Exchange <a href=\\\"http://stackexchange.com/sites\\\">network</a>, within the scope defined in the <a href=\\\"http://local.mso.com/help\\\">help center</a>.\",\"title\":null},{\"sub_options\":[{\"sub_options\":null,\"count\":null,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":true,\"requires_comment\":false,\"option_id\":43510,\"dialog_title\":\"Migration\",\"description\":null,\"title\":\"belongs on another site in the Stack Exchange network\"}],\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":null,\"dialog_title\":\"Off-Topic\",\"description\":\"This question belongs on another site in the Stack Exchange network\",\"title\":null},{\"sub_options\":null,\"count\":0,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":true,\"option_id\":47170,\"dialog_title\":\"Off-Topic\",\"description\":\"Other (add a comment explaining what is wrong)\",\"title\":null}],\"count\":0,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":null,\"dialog_title\":\"Why should this question be closed?\",\"description\":\"This question does not appear to be about Stack Overflow or the software that powers the Stack Exchange <a href=\\\"http://stackexchange.com/sites\\\">network</a> within the scope defined in the help center.\",\"title\":\"off-topic because...\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":11866,\"dialog_title\":\"Why should this question be closed?\",\"description\":\"Please clarify your specific problem or add additional details to highlight exactly what you need. As it's currently written, it’s hard to tell exactly what you're asking.\",\"title\":\"unclear what you're asking\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":18198,\"dialog_title\":\"Why should this question be closed?\",\"description\":\"There are either too many possible answers, or good answers would be too long for this format. Please add details to narrow the answer set or to isolate an issue that can be answered in a few paragraphs.\",\"title\":\"too broad\"},{\"sub_options\":null,\"count\":0,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":8528,\"dialog_title\":\"Why should this question be closed?\",\"description\":\"Many good questions generate some degree of opinion based on expert experience, but answers to this question will tend to be almost entirely based on opinions, rather than facts, references, or specific expertise.\",\"title\":\"primarily opinion-based\"}],\"count\":null,\"has_flagged\":false,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":null,\"dialog_title\":\"I am flagging this question because\",\"description\":\"This question does not meet this site's standards and should be closed.\",\"title\":\"it should be closed for another reason...\"},{\"sub_options\":null,\"count\":null,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":false,\"option_id\":36772,\"dialog_title\":\"I am flagging this question because\",\"description\":\"This question has severe formatting or content problems. This question is unlikely to be salvageable through editing, and might need to be removed.\",\"title\":\"it is very low quality\"},{\"sub_options\":null,\"count\":null,\"has_flagged\":null,\"requires_question_id\":false,\"requires_site\":false,\"requires_comment\":true,\"option_id\":34976,\"dialog_title\":\"I am flagging this question because\",\"description\":\"This question needs a moderator's attention. Please describe exactly what's wrong.\",\"title\":\"other (needs ♦ moderator attention)\"}],\"has_more\":false,\"error_id\":null,\"backoff\":10,\"quota_max\":10000,\"quota_remaining\":9996,\"page\":1,\"page_size\":30,\"total\":6,\"error_message\":null,\"error_name\":null,\"type\":\"flag_option\"}", foo);
            }
        }

        class _OddNullRefOuter
        {
            public List<_OddNullRef> Outer { get; set; }
        }

        class _OddNullRef
        {
            public List<_OddNullRef> Inner { get; set; }
        }

        [TestMethod]
        public void OddNullRef()
        {
            var template = new _OddNullRefOuter { Outer = new List<_OddNullRef> { new _OddNullRef(), new _OddNullRef(), new _OddNullRef { Inner = new List<_OddNullRef> { new _OddNullRef() } } } };
            var json = JSON.Serialize(template);

            var res = JSON.Deserialize<_OddNullRefOuter>(json);
            Assert.IsNotNull(res);
        }

        class _Issue27
        {
            public DateTimeOffset TestDate { get; set; }
        }

        [TestMethod]
        public void Issue27()
        {
            {
                var dto1 = new DateTimeOffset();
                var str1 = Jil.JSON.Serialize(new _Issue27 { TestDate = dto1 });
                var str2 = Jil.JSON.Serialize(new { TestDate = dto1 });
                Assert.AreEqual(str1, str2);
                Assert.AreEqual("{\"TestDate\":\"\\/Date(-62135596800000+0000)\\/\"}", str1);
            }

            {
                var dto1 = new DateTimeOffset();
                var str1 = Jil.JSON.Serialize(new _Issue27 { TestDate = dto1 }, Options.ExcludeNulls);
                var str2 = Jil.JSON.Serialize(new { TestDate = dto1 }, Options.ExcludeNulls);
                Assert.AreEqual(str1, str2);
                Assert.AreEqual("{\"TestDate\":\"\\/Date(-62135596800000+0000)\\/\"}", str1);
            }

            {
                var dto1 = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                var str1 = Jil.JSON.Serialize(new _Issue27 { TestDate = dto1 }, Options.ExcludeNulls);
                var str2 = Jil.JSON.Serialize(new { TestDate = dto1 }, Options.ExcludeNulls);
                Assert.AreEqual(str1, str2);
                Assert.AreEqual("{\"TestDate\":\"\\/Date(0+0000)\\/\"}", str1);
            }
        }

        [TestMethod]
        public void Issue42()
        {
            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5339, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4265339Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5330, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4265330Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5101, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4265101Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5011, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4265011Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 0511, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4260511Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 420, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 3511, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4203511Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 407, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 3511, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4073511Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 047, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 3511, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.0473511Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5300, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.42653Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5005, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4265005Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 0055, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4260055Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 420, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 0555, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4200555Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 400, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5555, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.4005555Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 01, 009, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5555, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:01.0095555Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 10, 089, DateTimeKind.Utc);
                dt = new DateTime(dt.Ticks + 5555, DateTimeKind.Utc); // 5339 = 0.5339 milliseconds

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:10.0895555Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 10, 089, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:10.089Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 10, 90, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:10.090Z\"";
                Assert.AreEqual(shouldMatch, str);
            }

            {
                var dt = new DateTime(2014, 08, 08, 14, 04, 10, 100, DateTimeKind.Utc);

                var str = JSON.Serialize(dt, Options.ISO8601);
                var shouldMatch = "\"2014-08-08T14:04:10.1Z\"";
                Assert.AreEqual(shouldMatch, str);
            }
        }

        public class _Issue53
        {
            [JilDirective(Ignore = true)]
            public DateTime NotSerializedProperty { get; set; }

            [JilDirective(Name = "NotSerializedProperty")]
            public string SerializedProperty { get; set; }
        }


        [TestMethod]
        public void Issue53()
        {
            var empty = JSON.Serialize(new _Issue53 { }, Options.ExcludeNulls);
            Assert.AreEqual("{}", empty);

            var data1 = JSON.Serialize(new _Issue53 { SerializedProperty = "a value!" }, Options.ExcludeNulls);
            Assert.AreEqual("{\"NotSerializedProperty\":\"a value!\"}", data1);

            var data2 = JSON.Serialize(new _Issue53 { NotSerializedProperty = DateTime.UtcNow }, Options.ExcludeNulls);
            Assert.AreEqual("{}", data2);
        }

        [TestMethod]
        public void ConfigDefaultOptions()
        {
            try
            {
                JSON.SetDefaultOptions(Options.ExcludeNulls);

                var obj = new { a = (object)null, b = (object)null };

                var @default = JSON.Serialize(obj);
                var @explicit = JSON.Serialize(obj, Options.Default);

                Assert.AreNotEqual(@default, @explicit);
                Assert.AreEqual(@default, "{}");
            }
            finally
            {
                JSON.SetDefaultOptions(Options.Default);
            }
        }

        class _Issue89
        {
            public DateTime DateField { get; set; }
            public int IntField { get; set; }
            public Dictionary<string, object> DictField { get; set; }
            public Exception ExceptionField { get; set; }

        }

        [TestMethod]
        public void Issue89()
        {
            Exception e = null;
            try
            {
                var x = int.Parse("330") / int.Parse("0");
            }
            catch (Exception _) { e = _; }

            Assert.IsNotNull(e);

            var obj =
                new _Issue89
                {
                    DateField = DateTime.UtcNow,
                    IntField = 123,
                    DictField = new Dictionary<string, object> { { "foo", "bar" } },
                    ExceptionField = e
                };

            var str = JSON.Serialize(obj);
            Assert.IsNotNull(str);
        }

#if EXHAUSTIVE_TEST
        [TestMethod]
        public void MicrosoftTimeSpans_Exhaustive()
        {
            Enumerable
                .Range(0, 365)
                .AsParallel()
                .ForAll(
                    d =>
                    {
                        for (var h = 0; h < 24; h++)
                        {
                            for (var m = 0; m < 60; m++)
                            {
                                for (var s = 0; s < 60; s++)
                                {
                                    for (var ms = 0; ms < 1000; ms++)
                                    {
                                        var ts1 = new TimeSpan(d, h, m, s, ms);
                                        var ts2 = ts1.Negate();

                                        string json1, json2;

                                        using (var str = new StringWriter())
                                        {
                                            JSON.Serialize(ts1, str);
                                            json1 = str.ToString();
                                        }

                                        using (var str = new StringWriter())
                                        {
                                            JSON.Serialize(ts2, str);
                                            json2 = str.ToString();
                                        }

                                        var str1 = ts1.ToString();
                                        var str2 = ts2.ToString();

                                        if (str1.IndexOf('.') != -1) str1 = str1.TrimEnd('0');
                                        if (str2.IndexOf('.') != -1) str2 = str2.TrimEnd('0');

                                        var jsonStr1 = json1.Substring(1, json1.Length - 2);
                                        var jsonStr2 = json2.Substring(1, json2.Length - 2);

                                        if (jsonStr1.IndexOf('.') != -1) jsonStr1 = jsonStr1.TrimEnd('0');
                                        if (jsonStr2.IndexOf('.') != -1) jsonStr2 = jsonStr2.TrimEnd('0');

                                        Assert.AreEqual(str1, jsonStr1);
                                        Assert.AreEqual(str2, jsonStr2);
                                    }
                                }
                            }
                        }
                    }
                );
        }
#endif

        [TestMethod]
        public void MicrosoftTimeSpans()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.Add(default(TimeSpan));

            foreach (var ts in timeSpans)
            {
                string streamJson, stringJson;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(ts, str, Options.Default);
                    streamJson = str.ToString();
                }

                {
                    stringJson = JSON.Serialize(ts, Options.Default);
                }

                Assert.IsTrue(streamJson.StartsWith("\""));
                Assert.IsTrue(streamJson.EndsWith("\""));
                Assert.IsTrue(stringJson.StartsWith("\""));
                Assert.IsTrue(stringJson.EndsWith("\""));

                var dotNetStr = ts.ToString();

                streamJson = streamJson.Trim('"');
                stringJson = stringJson.Trim('"');

                if (dotNetStr.IndexOf('.') != -1) dotNetStr = dotNetStr.TrimEnd('0');
                if (streamJson.IndexOf('.') != -1) streamJson = streamJson.TrimEnd('0');
                if (stringJson.IndexOf('.') != -1) stringJson = stringJson.TrimEnd('0');

                Assert.AreEqual(dotNetStr, streamJson);
                Assert.AreEqual(dotNetStr, stringJson);
            }
        }

        [TestMethod]
        public void RFC1123TimeSpans()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.Add(default(TimeSpan));

            foreach (var ts in timeSpans)
            {
                string streamJson, stringJson;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(ts, str, Options.RFC1123);
                    streamJson = str.ToString();
                }

                {
                    stringJson = JSON.Serialize(ts, Options.RFC1123);
                }

                Assert.IsTrue(streamJson.StartsWith("\""));
                Assert.IsTrue(streamJson.EndsWith("\""));
                Assert.IsTrue(stringJson.StartsWith("\""));
                Assert.IsTrue(stringJson.EndsWith("\""));

                var dotNetStr = ts.ToString();

                streamJson = streamJson.Trim('"');
                stringJson = stringJson.Trim('"');

                if (dotNetStr.IndexOf('.') != -1) dotNetStr = dotNetStr.TrimEnd('0');
                if (streamJson.IndexOf('.') != -1) streamJson = streamJson.TrimEnd('0');
                if (stringJson.IndexOf('.') != -1) stringJson = stringJson.TrimEnd('0');

                Assert.AreEqual(dotNetStr, streamJson);
                Assert.AreEqual(dotNetStr, stringJson);
            }
        }

        [TestMethod]
        public void SecondsTimeSpans()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.Add(default(TimeSpan));

            foreach (var ts in timeSpans)
            {
                string streamJson, stringJson;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(ts, str, Options.SecondsSinceUnixEpoch);
                    streamJson = str.ToString();
                }

                {
                    stringJson = JSON.Serialize(ts, Options.SecondsSinceUnixEpoch);
                }

                var dotNetStr = ts.TotalSeconds.ToString(CultureInfo.InvariantCulture);

                if (dotNetStr.IndexOf('.') != -1) dotNetStr = dotNetStr.TrimEnd('0');
                if (streamJson.IndexOf('.') != -1) streamJson = streamJson.TrimEnd('0');
                if (stringJson.IndexOf('.') != -1) stringJson = stringJson.TrimEnd('0');

                Assert.AreEqual(dotNetStr, streamJson);
                Assert.AreEqual(dotNetStr, stringJson);
            }
        }

        [TestMethod]
        public void MillsecondsTimeSpans()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.Add(default(TimeSpan));

            foreach (var ts in timeSpans)
            {
                string streamJson, stringJson;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(ts, str, Options.MillisecondsSinceUnixEpoch);
                    streamJson = str.ToString();
                }

                {
                    stringJson = JSON.Serialize(ts, Options.MillisecondsSinceUnixEpoch);
                }

                var dotNetStr = ts.TotalMilliseconds.ToString();

                if (dotNetStr.IndexOf('.') != -1) dotNetStr = dotNetStr.TrimEnd('0');
                if (streamJson.IndexOf('.') != -1) streamJson = streamJson.TrimEnd('0');
                if (stringJson.IndexOf('.') != -1) stringJson = stringJson.TrimEnd('0');

                Assert.AreEqual(dotNetStr, streamJson);
                Assert.AreEqual(dotNetStr, stringJson);
            }
        }

        [TestMethod]
        public void ISO8601TimeSpans()
        {
            var rand = new Random();
            var timeSpans = new List<TimeSpan>();

            for (var i = 0; i < 1000; i++)
            {
                var d = rand.Next(10675199 - 1);
                var h = rand.Next(24);
                var m = rand.Next(60);
                var s = rand.Next(60);
                var ms = rand.Next(1000);

                var ts = new TimeSpan(d, h, m, s, ms);
                if (rand.Next(2) == 0)
                {
                    ts = ts.Negate();
                }

                timeSpans.Add(ts);
            }

            timeSpans.Add(TimeSpan.MaxValue);
            timeSpans.Add(TimeSpan.MinValue);
            timeSpans.Add(default(TimeSpan));

            foreach (var ts in timeSpans)
            {
                string streamJson, stringJson;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(ts, str, Options.ISO8601);
                    streamJson = str.ToString();
                }

                {
                    stringJson = JSON.Serialize(ts, Options.ISO8601);
                }

                Assert.IsTrue(streamJson == stringJson);

                var dotNetStr = XmlConvert.ToString(ts);

                streamJson = streamJson.Trim('"');
                stringJson = stringJson.Trim('"');

                if (streamJson.IndexOf('.') != -1)
                {
                    var lastChar = streamJson[streamJson.Length - 1];
                    streamJson = streamJson.Substring(0, streamJson.Length - 1).TrimEnd('0') + lastChar;
                }

                if (stringJson.IndexOf('.') != -1)
                {
                    var lastChar = stringJson[stringJson.Length - 1];
                    stringJson = stringJson.Substring(0, stringJson.Length - 1).TrimEnd('0') + lastChar;
                }

                Assert.AreEqual(dotNetStr, streamJson);
                Assert.AreEqual(dotNetStr, stringJson);
            }
        }

        [TestMethod]
        public void NullArrayElements()
        {
            using (var str = new StringWriter())
            {
                var obj =
                    new
                    {
                        ids = new string[] { null, "US", "HI" }
                    };
                JSON.Serialize(obj, str, new Options(excludeNulls: true));

                var res = str.ToString();
                Assert.AreEqual("{\"ids\":[null,\"US\",\"HI\"]}", res);
            }
        }

        [TestMethod]
        public void ExcludingNulls()
        {
            // to stream tests
            {
                using (var str = new StringWriter())
                {
                    JSON.Serialize<object>(null, str, Options.Default);
                    var res = str.ToString();

                    Assert.AreEqual("null", res);
                }


                using (var str = new StringWriter())
                {
                    JSON.Serialize<object>(null, str, Options.ExcludeNulls);
                    var res = str.ToString();

                    // it's not a member, it should be written
                    Assert.AreEqual("null", res);
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(new[] { null, "hello", "world" }, str, Options.Default);
                    var res = str.ToString();

                    Assert.AreEqual("[null,\"hello\",\"world\"]", res);
                }

                using (var str = new StringWriter())
                {
                    JSON.Serialize(new[] { null, "hello", "world" }, str, Options.ExcludeNulls);
                    var res = str.ToString();

                    // it's not a member, it should be written
                    Assert.AreEqual("[null,\"hello\",\"world\"]", res);
                }

                using (var str = new StringWriter())
                {
                    var data = new Dictionary<string, int?>();
                    data["hello"] = 123;
                    data["world"] = null;

                    JSON.Serialize(data, str, Options.Default);
                    var res = str.ToString();

                    Assert.AreEqual("{\"hello\":123,\"world\":null}", res);
                }

                using (var str = new StringWriter())
                {
                    var data = new Dictionary<string, int?>();
                    data["hello"] = 123;
                    data["world"] = null;

                    JSON.Serialize(data, str, Options.ExcludeNulls);
                    var res = str.ToString();

                    Assert.AreEqual("{\"hello\":123}", res);
                }

                using (var str = new StringWriter())
                {
                    var data =
                        new
                        {
                            hello = 123,
                            world = default(object)
                        };

                    JSON.Serialize(data, str, Options.Default);
                    var res = str.ToString();

                    Assert.AreEqual("{\"hello\":123,\"world\":null}", res);
                }

                using (var str = new StringWriter())
                {
                    var data =
                        new
                        {
                            hello = 123,
                            world = default(object)
                        };

                    JSON.Serialize(data, str, Options.ExcludeNulls);
                    var res = str.ToString();

                    Assert.AreEqual("{\"hello\":123}", res);
                }
            }

            // to string tests
            {
                {
                    var res = JSON.Serialize<object>(null, Options.Default);

                    Assert.AreEqual("null", res);
                }


                {
                    var res = JSON.Serialize<object>(null, Options.ExcludeNulls);

                    // it's not a member, it should be written
                    Assert.AreEqual("null", res);
                }

                {
                    var res = JSON.Serialize(new[] { null, "hello", "world" }, Options.Default);

                    Assert.AreEqual("[null,\"hello\",\"world\"]", res);
                }

                {
                    var res = JSON.Serialize(new[] { null, "hello", "world" }, Options.ExcludeNulls);

                    // it's not a member, it should be written
                    Assert.AreEqual("[null,\"hello\",\"world\"]", res);
                }

                {
                    var data = new Dictionary<string, int?>();
                    data["hello"] = 123;
                    data["world"] = null;

                    var res = JSON.Serialize(data, Options.Default);

                    Assert.AreEqual("{\"hello\":123,\"world\":null}", res);
                }

                {
                    var data = new Dictionary<string, int?>();
                    data["hello"] = 123;
                    data["world"] = null;

                    var res = JSON.Serialize(data, Options.ExcludeNulls);

                    Assert.AreEqual("{\"hello\":123}", res);
                }

                {
                    var data =
                        new
                        {
                            hello = 123,
                            world = default(object)
                        };

                    var res = JSON.Serialize(data, Options.Default);

                    Assert.AreEqual("{\"hello\":123,\"world\":null}", res);
                }

                {
                    var data =
                        new
                        {
                            hello = 123,
                            world = default(object)
                        };

                    var res = JSON.Serialize(data, Options.ExcludeNulls);

                    Assert.AreEqual("{\"hello\":123}", res);
                }
            }
        }

        class _ConvertEnumsToPrimitives
        {
            public enum A : byte { X1, Y1, Z1 }
            public enum B : sbyte { X2, Y2, Z2 }
            public enum C : short { X3, Y3, Z3 }
            public enum D : ushort { X4, Y4, Z4 }
            public enum E : int { X5, Y5, Z5 }
            public enum F : uint { X6, Y6, Z6 }
            public enum G : long { X7, Y7, Z7 }
            public enum H : ulong { X8, Y8, Z8 }

            public A A1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(byte))]
            public A A2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(short))]
            public A A3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ushort))]
            public A A4 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public A A5 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(uint))]
            public A A6 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public A A7 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public A A8 { get; set; }

            public B B1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(sbyte))]
            public B B2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(short))]
            public B B3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public B B4 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public B B5 { get; set; }

            public C C1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(short))]
            public C C2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public C C3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public C C4 { get; set; }

            public D D1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ushort))]
            public D D2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public D D3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(uint))]
            public D D4 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public D D5 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public D D6 { get; set; }

            public E E1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(int))]
            public E E2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public E E3 { get; set; }

            public F F1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(uint))]
            public F F2 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public F F3 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public F F4 { get; set; }

            public G G1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(long))]
            public G G2 { get; set; }

            public H H1 { get; set; }
            [JilDirective(TreatEnumerationAs = typeof(ulong))]
            public H H2 { get; set; }
        }

        [TestMethod]
        public void ConvertEnumsToPrimitives()
        {
            var res =
                JSON.Serialize(
                    new _ConvertEnumsToPrimitives
                    {
                        A1 = _ConvertEnumsToPrimitives.A.X1,
                        A2 = _ConvertEnumsToPrimitives.A.Y1,
                        A3 = _ConvertEnumsToPrimitives.A.Z1,
                        A4 = _ConvertEnumsToPrimitives.A.X1,
                        A5 = _ConvertEnumsToPrimitives.A.Y1,
                        A6 = _ConvertEnumsToPrimitives.A.Z1,
                        A7 = _ConvertEnumsToPrimitives.A.X1,
                        A8 = _ConvertEnumsToPrimitives.A.Y1,

                        B1 = _ConvertEnumsToPrimitives.B.X2,
                        B2 = _ConvertEnumsToPrimitives.B.Y2,
                        B3 = _ConvertEnumsToPrimitives.B.Z2,
                        B4 = _ConvertEnumsToPrimitives.B.X2,
                        B5 = _ConvertEnumsToPrimitives.B.Y2,

                        C1 = _ConvertEnumsToPrimitives.C.X3,
                        C2 = _ConvertEnumsToPrimitives.C.Y3,
                        C3 = _ConvertEnumsToPrimitives.C.Z3,
                        C4 = _ConvertEnumsToPrimitives.C.X3,

                        D1 = _ConvertEnumsToPrimitives.D.X4,
                        D2 = _ConvertEnumsToPrimitives.D.Y4,
                        D3 = _ConvertEnumsToPrimitives.D.Z4,
                        D4 = _ConvertEnumsToPrimitives.D.X4,
                        D5 = _ConvertEnumsToPrimitives.D.Y4,
                        D6 = _ConvertEnumsToPrimitives.D.Z4,

                        E1 = _ConvertEnumsToPrimitives.E.X5,
                        E2 = _ConvertEnumsToPrimitives.E.Y5,
                        E3 = _ConvertEnumsToPrimitives.E.Z5,

                        F1 = _ConvertEnumsToPrimitives.F.X6,
                        F2 = _ConvertEnumsToPrimitives.F.Y6,
                        F3 = _ConvertEnumsToPrimitives.F.Z6,
                        F4 = _ConvertEnumsToPrimitives.F.X6,

                        G1 = _ConvertEnumsToPrimitives.G.X7,
                        G2 = _ConvertEnumsToPrimitives.G.Y7,

                        H1 = _ConvertEnumsToPrimitives.H.X8,
                        H2 = _ConvertEnumsToPrimitives.H.Y8
                    },
                    Options.PrettyPrint
                );

            Assert.AreEqual("{\n \"G1\": \"X7\",\n \"G2\": 1,\n \"H1\": \"X8\",\n \"H2\": 1,\n \"E1\": \"X5\",\n \"E2\": 1,\n \"E3\": 2,\n \"F1\": \"X6\",\n \"F2\": 1,\n \"F3\": 2,\n \"F4\": 0,\n \"C1\": \"X3\",\n \"C2\": 1,\n \"C3\": 2,\n \"C4\": 0,\n \"D1\": \"X4\",\n \"D2\": 1,\n \"D3\": 2,\n \"D4\": 0,\n \"D5\": 1,\n \"D6\": 2,\n \"A1\": \"X1\",\n \"A2\": 1,\n \"A3\": 2,\n \"A4\": 0,\n \"A5\": 1,\n \"A6\": 2,\n \"A7\": 0,\n \"A8\": 1,\n \"B1\": \"X2\",\n \"B2\": 1,\n \"B3\": 2,\n \"B4\": 0,\n \"B5\": 1\n}", res);
        }

        class _Issue95
        {
            public _Issue95Enum Flags { get; set; }
        }

        [Flags]
        enum _Issue95Enum
        {
            Foo = 0,
            Bar = 1,
            Baz = 2
        }

        [TestMethod]
        public void Issue95()
        {
            var items =
                Enumerable
                    .Range(0, 10)
                    .Select(
                        _ =>
                        {
                            var t = new _Issue95();
                            var values = (_Issue95Enum[])Enum.GetValues(typeof(_Issue95Enum));
                            t.Flags = values[_ % values.Length];
                            return t;
                        }
                    );

            var serialized = JSON.Serialize(items, Options.ISO8601PrettyPrint);
            Assert.AreEqual("[{\n \"Flags\": \"Foo\"\n}, {\n \"Flags\": \"Bar\"\n}, {\n \"Flags\": \"Baz\"\n}, {\n \"Flags\": \"Foo\"\n}, {\n \"Flags\": \"Bar\"\n}, {\n \"Flags\": \"Baz\"\n}, {\n \"Flags\": \"Foo\"\n}, {\n \"Flags\": \"Bar\"\n}, {\n \"Flags\": \"Baz\"\n}, {\n \"Flags\": \"Foo\"\n}]", serialized);
        }

        enum _Issue101
        {
            [EnumMember]
            Foo,
            [EnumMember(Value = "BAR")]
            Bar
        }

        [TestMethod]
        public void Issue101()
        {
            var serialized = JSON.Serialize(new { foo = _Issue101.Foo, bar = _Issue101.Bar });
            Assert.AreEqual("{\"foo\":\"Foo\",\"bar\":\"BAR\"}", serialized);
        }

        [TestMethod]
        public void UnspecifiedToUtc()
        {
            var unspecified = new DateTime(1970, 1, 2, 3, 4, 5, DateTimeKind.Unspecified);
            var specified = new DateTime(1970, 1, 2, 3, 4, 5, DateTimeKind.Utc);

            var iso8601 = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.ISO8601, false, UnspecifiedDateTimeKindBehavior.IsUTC));
            var iso8601Control = JSON.Serialize(specified, Options.ISO8601);

            var ms = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.MillisecondsSinceUnixEpoch, false, UnspecifiedDateTimeKindBehavior.IsUTC));
            var msControl = JSON.Serialize(specified, Options.MillisecondsSinceUnixEpoch);

            var s = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.SecondsSinceUnixEpoch, false, UnspecifiedDateTimeKindBehavior.IsUTC));
            var sControl = JSON.Serialize(specified, Options.SecondsSinceUnixEpoch);

            var microsoft = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, false, UnspecifiedDateTimeKindBehavior.IsUTC));
            var microsoftControl = JSON.Serialize(specified, Options.Default);

            Assert.AreEqual(iso8601Control, iso8601);
            Assert.AreEqual(msControl, ms);
            Assert.AreEqual(sControl, s);
            Assert.AreEqual(microsoftControl, microsoft);
        }

        [TestMethod]
        public void UnspecifiedToLocal()
        {
            var unspecified = new DateTime(1970, 1, 2, 3, 4, 5, DateTimeKind.Unspecified);
            var specified = new DateTime(1970, 1, 2, 3, 4, 5, DateTimeKind.Local);

            var iso8601 = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.ISO8601, false, UnspecifiedDateTimeKindBehavior.IsLocal));
            var iso8601Control = JSON.Serialize(specified, Options.ISO8601);

            var ms = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.MillisecondsSinceUnixEpoch, false, UnspecifiedDateTimeKindBehavior.IsLocal));
            var msControl = JSON.Serialize(specified, Options.MillisecondsSinceUnixEpoch);

            var s = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.SecondsSinceUnixEpoch, false, UnspecifiedDateTimeKindBehavior.IsLocal));
            var sControl = JSON.Serialize(specified, Options.SecondsSinceUnixEpoch);

            var microsoft = JSON.Serialize(unspecified, new Options(false, false, false, Jil.DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, false, UnspecifiedDateTimeKindBehavior.IsLocal));
            var microsoftControl = JSON.Serialize(specified, Options.Default);

            Assert.AreEqual(iso8601Control, iso8601);
            Assert.AreEqual(msControl, ms);
            Assert.AreEqual(sControl, s);
            Assert.AreEqual(microsoftControl, microsoft);
        }

        [TestMethod]
        public void ISO8601WithOffset()
        {
            var toTest = new List<DateTimeOffset>();
            toTest.Add(DateTimeOffset.Now);

            for (var h = 0; h <= 14; h++)
            {
                for (var m = 0; m < 60; m++)
                {
                    if (h == 0 && m == 0) continue;
                    if (h == 14 && m > 0) continue;

                    var offsetPos = new TimeSpan(h, m, 0);
                    var offsetNeg = offsetPos.Negate();

                    var now = DateTime.Now;
                    now = DateTime.SpecifyKind(now, DateTimeKind.Unspecified);

                    toTest.Add(new DateTimeOffset(now, offsetPos));
                    toTest.Add(new DateTimeOffset(now, offsetNeg));
                }
            }

            foreach(var testDto in toTest)
            {
                var shouldMatch = "\"" + testDto.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz") + "\"";
                var strStr = JSON.Serialize(testDto, Options.ISO8601);
                string streamStr;
                using (var str = new StringWriter())
                {
                    JSON.Serialize(testDto, str, Options.ISO8601);
                    streamStr = str.ToString();
                }

                Assert.AreEqual(shouldMatch, strStr);
                Assert.AreEqual(shouldMatch, streamStr);
            }
        }

        class _DictionaryDictionary
        {
            public Dictionary<string, Dictionary<string, int>> A { get; set; }
        }

        [TestMethod]
        public void DictionaryDictionary()
        {
            var str = JSON.Serialize<Dictionary<string, Dictionary<string, int>>>(new Dictionary<string, Dictionary<string, int>>(), Options.ExcludeNulls);
            Assert.IsFalse(string.IsNullOrEmpty(str));
        }

        [TestMethod]
        public void NaNFails()
        {
            // double
            {
                try
                {
                    JSON.Serialize(double.NaN);
                    Assert.Fail("Should be impossible");
                }
                catch (Exception e)
                {
                    Assert.AreEqual("NaN is not a permitted JSON number value", e.Message);
                }

                try
                {
                    JSON.Serialize(double.NegativeInfinity);
                    Assert.Fail("Should be impossible");
                }
                catch (Exception e)
                {
                    Assert.AreEqual("-Infinity is not a permitted JSON number value", e.Message);
                }

                try
                {
                    JSON.Serialize(double.PositiveInfinity);
                    Assert.Fail("Should be impossible");
                }
                catch (Exception e)
                {
                    Assert.AreEqual("Infinity is not a permitted JSON number value", e.Message);
                }
            }

            // float
            {
                try
                {
                    JSON.Serialize(float.NaN);
                    Assert.Fail("Should be impossible");
                }
                catch (Exception e)
                {
                    Assert.AreEqual("NaN is not a permitted JSON number value", e.Message);
                }

                try
                {
                    JSON.Serialize(float.NegativeInfinity);
                    Assert.Fail("Should be impossible");
                }
                catch (Exception e)
                {
                    Assert.AreEqual("-Infinity is not a permitted JSON number value", e.Message);
                }

                try
                {
                    JSON.Serialize(float.PositiveInfinity);
                    Assert.Fail("Should be impossible");
                }
                catch (Exception e)
                {
                    Assert.AreEqual("Infinity is not a permitted JSON number value", e.Message);
                }
            }
        }

        public class _Issue127_A
        {
            public int A { get; set; }
            public Dictionary<int, int?> B { get; set; }
        }

        public class _Issue127_B
        {
            public int? A { get; set; }
            public Dictionary<int, int?> B { get; set; }
        }
        
        [TestMethod]
        public void Issue127()
        {
            var a = new _Issue127_A { A = 1, B = new Dictionary<int, int?> { { 2, 3 } } };
            var b = new _Issue127_B { A = 1, B = new Dictionary<int, int?> { { 2, 3 } } };
            var jsonA = JSON.Serialize(a);
            var jsonB = JSON.Serialize(b);
            Assert.AreEqual("{\"A\":1,\"B\":{\"2\":3}}", jsonA);
            Assert.AreEqual("{\"B\":{\"2\":3},\"A\":1}", jsonB);
        }

        [TestMethod]
        public void MicrosoftDateTimeOffsets()
        {
            // While *DateTimes* in Microsoft format don't do anything with the offset (they just write it, then ignore it),
            //   DateTimeOffsets do.

            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            var dtos = 
                new[] 
                { 
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero), 
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(1)), 
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(-1)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(2)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(-2)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(3)),
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(-3)),
                    new DateTimeOffset(1234, 5, 6, 7, 8, 9, new TimeSpan(4, 30, 00)),
                    new DateTimeOffset(1234, 5, 6, 7, 8, 9, new TimeSpan(-4, -30, 00)),
                };

            foreach(var dto in dtos)
            {
                var val = Newtonsoft.Json.JsonConvert.SerializeObject(dto, settings);

                // stream style
                using (var str = new StringWriter())
                {
                    JSON.Serialize(dto, str);

                    var res = str.ToString();
                    Assert.AreEqual(val, res);
                }

                // string style
                {
                    var res = JSON.Serialize(dto);
                    Assert.AreEqual(val, res);
                }
            }
        }


        [TestMethod]
        public void Issue147()
        {
            var res = JSON.Serialize(long.MinValue);
            Assert.AreEqual("-9223372036854775808", res);

            res = JSON.Serialize(int.MinValue);
            Assert.AreEqual("-2147483648", res);
        }

        public class SerilaizationTestObj
        {
            [DataMember(Name="ExplicitMember")]
            public string MemberProperty { get; set; }

            [JilDirective(Name = "Directive")]
            public string DirectiveProperty { get; set; }
            
            public string NekkidProperty { get; set; }
        }


        [TestMethod]
        public void TestSerializationNameFormatsSerialization()
        {
            using (var str = new StringWriter())
            {
                var obj =
                    new
                    {
                        oneTwo = "1",
                        ThreeFour = "2",
                        FIVESIX = "3"
                    };
                JSON.Serialize(obj, str, new Options(serializationNameFormat: SerializationNameFormat.Verbatim));

                var res = str.ToString();
                Assert.AreEqual("{\"FIVESIX\":\"3\",\"ThreeFour\":\"2\",\"oneTwo\":\"1\"}", res);
            }

            using (var str = new StringWriter())
            {
                var obj =
                    new
                    {
                        oneTwo = "1",
                        ThreeFour = "2",
                        FIVESIX = "3"
                    };
                JSON.Serialize(obj, str, new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

                var res = str.ToString();
                Assert.AreEqual("{\"fivesix\":\"3\",\"threeFour\":\"2\",\"oneTwo\":\"1\"}", res);
            }

            using (var str = new StringWriter())
            {
                var obj = new SerilaizationTestObj
                {
                    DirectiveProperty = "DirectiveValue",
                    MemberProperty = "MemberValue",
                    NekkidProperty = "NekkidValie"
                };
                JSON.Serialize(obj, str, new Options(serializationNameFormat: SerializationNameFormat.Verbatim));

                var res = str.ToString();
                Assert.AreEqual("{\"NekkidProperty\":\"NekkidValie\",\"Directive\":\"DirectiveValue\",\"ExplicitMember\":\"MemberValue\"}", res);
            }

            using (var str = new StringWriter())
            {
                var obj = new SerilaizationTestObj
                {
                    DirectiveProperty = "DirectiveValue",
                    MemberProperty = "MemberValue",
                    NekkidProperty = "NekkidValie"
                };
                JSON.Serialize(obj, str, new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

                var res = str.ToString();
                Assert.AreEqual("{\"nekkidProperty\":\"NekkidValie\",\"Directive\":\"DirectiveValue\",\"ExplicitMember\":\"MemberValue\"}", res);
            }
        }

        // Also see DeserializeTests._Issue150
        class _Issue150
        {
            public enum A { A, B, C }

            public class _InArray<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public T[] ArrayOfEnum;
            }

            public class _InList<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public List<T> ListOfEnum;
            }

            public class _InListProp<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public List<T> ListOfEnum { get; set; }
            }

            public class _InEnumerable<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public IEnumerable<T> EnumerableOfEnum;
            }

            public class _AsDictionaryKey<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public Dictionary<T, int> DictionaryWithEnumKey;
            }

            public class _AsDictionaryValue<T>
            {
                [JilDirective(TreatEnumerationAs = typeof(int))]
                public Dictionary<int, T> DictionaryWithEnumValue;
            }
        }

        [TestMethod]
        public void Issue150()
        {
            {
                var obj = new _Issue150._InArray<_Issue150.A>();
                obj.ArrayOfEnum = new[] { _Issue150.A.A, _Issue150.A.B };

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"ArrayOfEnum\":[0,1]}", str);
            }

            {
                var obj = new _Issue150._InArray<_Issue150.A?>();
                obj.ArrayOfEnum = new _Issue150.A?[] { _Issue150.A.A, null };

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"ArrayOfEnum\":[0,null]}", str);
            }

            {
                var obj = new _Issue150._InList<_Issue150.A>();
                obj.ListOfEnum = new List<_Issue150.A>(new[] { _Issue150.A.A, _Issue150.A.B });

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"ListOfEnum\":[0,1]}", str);
            }

            {
                var obj = new _Issue150._InList<_Issue150.A?>();
                obj.ListOfEnum = new List<_Issue150.A?>(new _Issue150.A?[] { _Issue150.A.A, null });

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"ListOfEnum\":[0,null]}", str);
            }

            {
                var obj = new _Issue150._InListProp<_Issue150.A>();
                obj.ListOfEnum = new List<_Issue150.A>(new[] { _Issue150.A.A, _Issue150.A.B });

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"ListOfEnum\":[0,1]}", str);
            }

            {
                var obj = new _Issue150._InListProp<_Issue150.A?>();
                obj.ListOfEnum = new List<_Issue150.A?>(new _Issue150.A?[] { _Issue150.A.A, null });

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"ListOfEnum\":[0,null]}", str);
            }

            {
                var obj = new _Issue150._InEnumerable<_Issue150.A>();
                obj.EnumerableOfEnum = new HashSet<_Issue150.A> { _Issue150.A.A, _Issue150.A.B };

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"EnumerableOfEnum\":[0,1]}", str);
            }

            {
                var obj = new _Issue150._InEnumerable<_Issue150.A?>();
                obj.EnumerableOfEnum = new HashSet<_Issue150.A?> { _Issue150.A.A, null };

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"EnumerableOfEnum\":[0,null]}", str);
            }

            {
                var obj = new _Issue150._AsDictionaryKey<_Issue150.A>();
                obj.DictionaryWithEnumKey = new Dictionary<_Issue150.A, int>
                {
                    { _Issue150.A.A, 10 },
                    { _Issue150.A.B, 20 }
                };

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"DictionaryWithEnumKey\":{\"0\":10,\"1\":20}}", str);
            }

            {
                var obj = new _Issue150._AsDictionaryValue<_Issue150.A>();
                obj.DictionaryWithEnumValue = new Dictionary<int, _Issue150.A>
                {
                    { 10, _Issue150.A.A },
                    { 20, _Issue150.A.B }
                };

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"DictionaryWithEnumValue\":{\"10\":0,\"20\":1}}", str);
            }

            {
                var obj = new _Issue150._AsDictionaryValue<_Issue150.A?>();
                obj.DictionaryWithEnumValue = new Dictionary<int, _Issue150.A?>
                {
                    { 10, _Issue150.A.A },
                    { 20, null }
                };

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"DictionaryWithEnumValue\":{\"10\":0,\"20\":null}}", str);
            }
        }

        // Also see DeserializeTests._Issue151
        class _Issue151
        {
            public enum A { A, B, C }

            [JilDirective(TreatEnumerationAs = typeof(int))]
            public A? NullableEnum;
        }

        [TestMethod]
        public void Issue151()
        {
            {
                var obj = new _Issue151();
                obj.NullableEnum = _Issue151.A.B;

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"NullableEnum\":1}", str);
            }

            {
                var obj = new _Issue151();
                obj.NullableEnum = null;

                var str = JSON.Serialize(obj);
                Assert.AreEqual("{\"NullableEnum\":null}", str);
            }
        }

        [TestMethod]
        public void Issue165()
        {
            var dto = new DateTimeOffset(2015, 9, 9, 18, 37, 40, TimeSpan.FromHours(2));
            var str = JSON.Serialize(dto, Options.ISO8601);
            Assert.AreEqual("\"2015-09-09T18:37:40+02:00\"", str);
        }

        [TestMethod]
        public void Issue165_2()
        {
            var dto = new DateTimeOffset(2015, 9, 9, 18, 37, 40, TimeSpan.FromHours(2));
            var str = JSON.Serialize(dto, Options.ISO8601);
            var dt = JSON.Deserialize<DateTime>(str, Options.ISO8601);

            Assert.AreEqual(dto.UtcDateTime, dt);
        }

        public enum _Issue159_Enum
        {
            Unknown,
            None,
            RadioButtonVertical,
            RadioButtonHorizontal,
            ComboList,
            EditableComboList,
            List,
            Slider,
            Numeric,
            Rating,
            Inherited,
            ImageHorizontal,
            ImageVertical,
            Text,
            Html,
            TreeView,
            CheckboxList,
            CheckBox,
            HourMinute
        }

        public class _Issue159
        {
            public bool? AllowMultipleSelection { get; set; }
            public _Issue159_Enum? DefaultUserControlMode { get; set; }
            public int? MaxSelectedValues { get; set; }
            public bool? EnableKwManagement { get; set; }
        }

        [TestMethod]
        public void Issue159()
        {
            var json = 
                JSON.Serialize(
                    new _Issue159
                    {
                        DefaultUserControlMode = _Issue159_Enum.CheckBox
                    }
                );

            var bean = JSON.Deserialize<_Issue159>(json);
            Assert.AreEqual(bean.DefaultUserControlMode, _Issue159_Enum.CheckBox);
        }
    }
}