using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Error
    {
        [ProtoMember(1)]
        public int? error_id { get; set; }
        [ProtoMember(2)]
        public string error_name { get; set; }
        [ProtoMember(3)]
        public string description { get; set; }
    }
}
