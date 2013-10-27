using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class Privilege : IGenericEquality<Privilege>
    {
        [ProtoMember(1)]
        public string short_description { get; set; }
        [ProtoMember(2)]
        public string description { get; set; }
        [ProtoMember(3)]
        public int? reputation { get; set; }

        public bool Equals(Privilege obj)
        {
            return
                this.description.TrueEqualsString(obj.description) &&
                this.reputation.TrueEquals(obj.reputation) &&
                this.short_description.TrueEqualsString(obj.short_description);
        }
    }
}
