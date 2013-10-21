using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    enum UserType : byte
    {
        unregistered = 2,
        registered = 3,
        moderator = 4,
        does_not_exist = 255
    }
    
    class ShallowUser
    {
        public int? user_id { get; set; }
        public string display_name { get; set; }
        public int? reputation { get; set; }
        public UserType? user_type { get; set; }
        public string profile_image { get; set; }
        public string link { get; set; }
        public int? accept_rate { get; set; }
        public User.BadgeCount badge_counts { get; set; }
    }
}
