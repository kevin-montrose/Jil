using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class FlagOption
    {
        [ProtoMember(1)]
        public int? option_id { get; set; }
        [ProtoMember(2)]
        public bool? requires_comment { get; set; }
        [ProtoMember(3)]
        public bool? requires_site { get; set; }
        [ProtoMember(4)]
        public bool? requires_question_id { get; set; }
        [ProtoMember(5)]
        public string title { get; set; }
        [ProtoMember(6)]
        public string description { get; set; }
        [ProtoMember(7)]
        public List<FlagOption> sub_options { get; set; }
        [ProtoMember(8)]
        public bool? has_flagged { get; set; }
        [ProtoMember(9)]
        public int? count { get; set; }
    }
}
