using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class TagScore
    {
        [ProtoMember(1)]
        public ShallowUser user { get; set; }
        [ProtoMember(2)]
        public int? score { get; set; }
        [ProtoMember(3)]
        public int? post_count { get; set; }
    }
}
