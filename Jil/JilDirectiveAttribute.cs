using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// Alternative to using [DataMember] and [IgnoreDataMember], for 
    /// when their use isn't possible.
    /// 
    /// When applied to a property or field, allows configuration
    /// of the name (de)serialized, whether to (de)serialize at all,
    /// and the primitive type to treat an enum type as.
    /// 
    /// Takes precedence over [DataMember] and [IgnoreDataMember].
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class JilDirectiveAttribute : Attribute
    {
        /// <summary>
        /// If true, the decorated member will not be serialized or deserialized.
        /// </summary>
        public bool Ignore { get; set; }
        /// <summary>
        /// If non-null, the decorated member's name in serialization will match
        /// the value of Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// If true and the member annotated is an enum, will cause Jil to convert
        /// the enum to the appropriate primitive type before serializing; and expect
        /// that primitive type when deserializing, converting back to the enum when
        /// constructing the final object.
        /// </summary>
        public Type SerializeEnumerationAs { get; set; }

        /// <summary>
        /// Create a new JilDirectiveAttribute
        /// </summary>
        public JilDirectiveAttribute() { }

        /// <summary>
        /// Create a new JilDirectiveAttribute, with a name override.
        /// </summary>
        public JilDirectiveAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Create a new JilDirectiveAttribute, optionally ignoring the decorated member.
        /// </summary>
        public JilDirectiveAttribute(bool ignore)
        {
            Ignore = ignore;
        }

        /// <summary>
        /// Create a new JilDirectiveAttribute, treating the decorate member of an enum type
        /// as the given primitive type (ie. byte, sbyte, short, ushort, int, uint, long, or ulong).
        /// </summary>
        public JilDirectiveAttribute(Type serializeEnumAs)
        {
            SerializeEnumerationAs = serializeEnumAs;
        }
    }
}