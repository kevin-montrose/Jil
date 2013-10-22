using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class AccessToken
    {
        [ProtoMember(1)]
        public string access_token { get; set; }
        [ProtoMember(2)]
        public DateTime? expires_on_date { get; set; }
        [ProtoMember(3)]
        public int? account_id { get; set; }
        [ProtoMember(4)]
        public List<string> scope { get; set; }
    }
}
