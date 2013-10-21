using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class WritePermission
    {
        public int? user_id { get; set; }
        public string object_type { get; set; }
        public bool? can_add { get; set; }
        public bool? can_edit { get; set; }
        public bool? can_delete { get; set; }
        public int? max_daily_actions { get; set; }
        public int? min_seconds_between_actions { get; set; }
    }
}
