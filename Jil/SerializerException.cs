using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// An exception thrown when Jil encounters an error while serializing an object.
    /// </summary>
    public class SerializerException : Exception
    {
        internal SerializerException(string message, Exception innerException) :
            base(message + ": " + innerException.Message, innerException)
        { }

        internal static readonly ConstructorInfo Constructor_String = typeof(SerializerException).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new [] { typeof(string) }, null);
        internal SerializerException(string message) : base(message)
        {
        }
    }
}
