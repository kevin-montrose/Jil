using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class TagScore : IGenericEquality<TagScore>
    {
        [ProtoMember(1)]
        public ShallowUser user { get; set; }
        [ProtoMember(2)]
        public int? score { get; set; }
        [ProtoMember(3)]
        public int? post_count { get; set; }

        public bool Equals(TagScore obj)
        {
            return
                this.post_count.TrueEquals(obj.post_count) &&
                this.score.TrueEquals(obj.score) &&
                this.user.TrueEquals(obj.user);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.post_count.TrueEquals((int?)obj.post_count) &&
                this.score.TrueEquals((int?)obj.score) &&
                (this.user == null && obj.user == null || this.user.EqualsDynamic(obj.user));
        }
    }
}
