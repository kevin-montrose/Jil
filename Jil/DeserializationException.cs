using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    class DeserializationException : Exception
    {
        public DeserializationException(string msg) : base(msg) { }
    }
}
