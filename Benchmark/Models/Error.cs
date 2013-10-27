using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Error : IGenericEquality<Error>
    {
        [ProtoMember(1)]
        public int? error_id { get; set; }
        [ProtoMember(2)]
        public string error_name { get; set; }
        [ProtoMember(3)]
        public string description { get; set; }

        public bool Equals(Error obj)
        {
            return
                this.error_id.TrueEquals(obj.error_id) &&
                this.error_name.TrueEqualsString(obj.error_name) &&
                this.description.TrueEqualsString(obj.description);
        }
    }
}
