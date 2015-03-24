using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static partial class Methods
    {
        static readonly MethodInfo ConsumeWhiteSpaceThunkReader = typeof(Methods).GetMethod("_ConsumeWhiteSpaceThunkReader", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _ConsumeWhiteSpaceThunkReader(ref ThunkReader reader)
        {
            int c;
            while ((c = reader.Peek()) != -1)
            {
                if (!IsWhiteSpace(c)) return;

                reader.Read();
            }
        }
    }
}
