using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class WritePermission
    {
        [ProtoMember(1)]
        public int? user_id { get; set; }
        [ProtoMember(2)]
        public string object_type { get; set; }
        [ProtoMember(3)]
        public bool? can_add { get; set; }
        [ProtoMember(4)]
        public bool? can_edit { get; set; }
        [ProtoMember(5)]
        public bool? can_delete { get; set; }
        [ProtoMember(6)]
        public int? max_daily_actions { get; set; }
        [ProtoMember(7)]
        public int? min_seconds_between_actions { get; set; }
    }
}
