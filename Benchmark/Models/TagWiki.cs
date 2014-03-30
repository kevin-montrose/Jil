using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    [ProtoContract]
    class TagWiki : IGenericEquality<TagWiki>
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

        public bool Equals(TagWiki obj)
        {
            return
                this.body.TrueEqualsString(obj.body) &&
                this.body_last_edit_date.TrueEquals(obj.body_last_edit_date) &&
                this.excerpt.TrueEqualsString(obj.excerpt) &&
                this.excerpt_last_edit_date.TrueEquals(obj.excerpt_last_edit_date) &&
                this.last_body_editor.TrueEquals(obj.last_body_editor) &&
                this.last_excerpt_editor.TrueEquals(obj.last_excerpt_editor) &&
                this.tag_name.TrueEqualsString(obj.tag_name);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.body.TrueEqualsString((string)obj.body) &&
                this.body_last_edit_date.TrueEquals((DateTime?)obj.body_last_edit_date) &&
                this.excerpt.TrueEqualsString((string)obj.excerpt) &&
                this.excerpt_last_edit_date.TrueEquals((DateTime?)obj.excerpt_last_edit_date) &&
                (this.last_body_editor == null && obj.last_body_editor == null || this.last_body_editor.EqualsDynamic(obj.last_body_editor)) &&
                (this.last_excerpt_editor == null && obj.last_excerpt_editor == null || this.last_excerpt_editor.EqualsDynamic(obj.last_excerpt_editor)) &&
                this.tag_name.TrueEqualsString((string)obj.tag_name);
        }
    }
}
