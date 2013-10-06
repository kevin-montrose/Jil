using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// An exception throw when Jil suspects it's in an infinite recursive case.
    /// </summary>
    public class InfiniteRecursionException : Exception
    {
        public InfiniteRecursionException() { }
    }
}
