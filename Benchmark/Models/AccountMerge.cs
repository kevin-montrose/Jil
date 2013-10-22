using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class AccountMerge
    {
        [ProtoMember(1)]
        public int? old_account_id { get; set; }
        [ProtoMember(2)]
        public int? new_account_id { get; set; }
        [ProtoMember(3)]
        public DateTime? merge_date { get; set; }
    }
}
