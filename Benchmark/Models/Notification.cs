using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum NotificationType : byte
    {
        generic = 1,
        accounts_associated = 8,
        badge_earned = 5,
        profile_activity = 2,
        bounty_expired = 3,
        bounty_expires_in_one_day = 4,
        bounty_expires_in_three_days = 6,
        edit_suggested = 22,
        new_privilege = 9,
        post_migrated = 10,
        moderator_message = 11,
        registration_reminder = 12,
        substantive_edit = 23,
        reputation_bonus = 7,
        bounty_grace_period_started = 24
    }

    class Notification
    {
        public NotificationType? notification_type { get; set; }
        public Info.Site site { get; set; }
        public DateTime? creation_date { get; set; }
        public string body { get; set; }
        public int? post_id { get; set; }
        public bool? is_unread { get; set; }
    }
}
