using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Tag
    {
        [ProtoMember(1)]
        public string name { get; set; }
        [ProtoMember(2)]
        public int? count { get; set; }
        [ProtoMember(3)]
        public bool? is_required { get; set; }
        [ProtoMember(4)]
        public bool? is_moderator_only { get; set; }
        [ProtoMember(5)]
        public int? user_id { get; set; }
        [ProtoMember(6)]
        public bool? has_synonyms { get; set; }
        [ProtoMember(7)]
        public DateTime? last_activity_date { get; set; }
        [ProtoMember(8)]
        public List<string> synonyms { get; set; }
    }
}
