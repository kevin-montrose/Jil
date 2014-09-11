using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    static class BufferPersistence
    {
        [ThreadStatic]
        public static char[] ThreadStaticBuffer;
        public const int ThreadStaticBufferInitialSize = 128;
    }
}
