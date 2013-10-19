using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// An exception throw when Jil suspects it's in an infinite recursive case.
    /// 
    /// Note that this is detected heuristically, exactly how many recursions must occur
    /// before it is thrown depends on the configuration and version of Jil, as well as the object being serialized.
    /// </summary>
    public class InfiniteRecursionException : Exception
    {
        internal InfiniteRecursionException() { }
    }
}
