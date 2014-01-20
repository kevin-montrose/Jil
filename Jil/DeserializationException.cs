using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// An exception thrown when Jil encounters an error when deserializing a stream.
    /// </summary>
    public class DeserializationException : Exception
    {
        internal DeserializationException(string msg) : base(msg) { }

        internal DeserializationException(string msg, Exception inner) : base(msg, inner) { }
    }
}
