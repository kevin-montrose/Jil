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
    static class NoneTypeCache<T>
    {
        public static readonly Action<TextWriter, T> Thunk;

        static NoneTypeCache()
        {
            Thunk = InlineSerializer.Build<T>(pretty: false, excludeNulls: false);
        }
    }

    static class PrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T> Thunk;

        static PrettyPrintTypeCache()
        {
            Thunk = InlineSerializer.Build<T>(pretty: true, excludeNulls: false);
        }
    }

    static class ExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T> Thunk;

        static ExcludeNullsTypeCache()
        {
            Thunk = InlineSerializer.Build<T>(pretty: false, excludeNulls: true);
        }
    }

    static class PrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T> Thunk;

        static PrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializer.Build<T>(pretty: true, excludeNulls: true);
        }
    }
}
