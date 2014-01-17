using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    public class DeserializationException : Exception
    {
        internal DeserializationException(string msg) : base(msg) { }

        internal DeserializationException(string msg, Exception inner) : base(msg, inner) { }
    }
}
