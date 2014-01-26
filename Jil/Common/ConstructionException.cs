using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    // Just a common type to act as a "couldn't build this junk!" catch-all
    sealed class ConstructionException : Exception
    {
        public ConstructionException(string msg) : base(msg) { }
    }
}
