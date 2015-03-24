using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static partial class Methods
    {
        public static MethodInfo GetConsumeWhiteSpace(bool readingFromString)
        {
            return
                !readingFromString ?
                    ConsumeWhiteSpace :
                    ConsumeWhiteSpaceThunkReader;
        }
    }
}
