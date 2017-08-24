using System;

namespace Jil
{
    /// <summary>
    /// When applied to a class allows configuration
    /// of the name of the property to store raw json string
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JilClassDirectiveAttribute : Attribute
    {
        /// <summary>
        /// Name of the property to store raw json string
        /// </summary>
        public string RawPropertyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public JilClassDirectiveAttribute()
        {
            
        }
        /// <summary>
        /// Create a new JilClassDirectiveAttribute
        /// </summary>
        /// <param name="rawPropertyName">Name of the property to store raw json string</param>
        public JilClassDirectiveAttribute(string rawPropertyName)
        {
            RawPropertyName = rawPropertyName;
        }
    }
}