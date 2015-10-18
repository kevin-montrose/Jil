using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
