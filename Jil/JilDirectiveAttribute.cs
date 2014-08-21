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
    /// When applied to an property or field, allows configuration
    /// of the name (de)serialized and whether to (de)serialize at all.
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
    }
}