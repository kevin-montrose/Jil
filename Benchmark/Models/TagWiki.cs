using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class TagWiki
    {
        [ProtoMember(1)]
        public string tag_name { get; set; }
        [ProtoMember(2)]
        public string body { get; set; }
        [ProtoMember(3)]
        public string excerpt { get; set; }
        [ProtoMember(4)]
        public DateTime? body_last_edit_date { get; set; }
        [ProtoMember(5)]
        public DateTime? excerpt_last_edit_date { get; set; }
        [ProtoMember(6)]
        public ShallowUser last_body_editor { get; set; }
        [ProtoMember(7)]
        public ShallowUser last_excerpt_editor { get; set; }
    }
}
