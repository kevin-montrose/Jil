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
        public static readonly Action<TextWriter, T, int> Thunk;

        static NoneTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NoneTypeCache<>), pretty: false, excludeNulls: false);
        }
    }

    static class PrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static PrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(PrettyPrintTypeCache<>), pretty: true, excludeNulls: false);
        }
    }

    static class ExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ExcludeNullsTypeCache<>), pretty: false, excludeNulls: true);
        }
    }

    static class PrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static PrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(PrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true);
        }
    }
}
