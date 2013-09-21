using Sigil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static class TypeCache<T>
    {
        public static readonly Action<TextWriter, T> Thunk;

        static TypeCache()
        {
            Thunk = InlineSerializer.Build<T>();
        }
    }
}
