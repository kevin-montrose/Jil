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
    static class DefaultTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static DefaultTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(DefaultTypeCache<>), pretty: false, excludeNulls: false);
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

    static class ISO8601TypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601TypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601TypeCache<>), dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class ISO8601ExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601ExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsTypeCache<>), excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class ISO8601PrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintTypeCache<>), pretty: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class SecondsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsTypeCache<>), dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class SecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsTypeCache<>), excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class SecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintTypeCache<>), pretty: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class SecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }




    static class MillisecondsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsTypeCache<>), dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class MillisecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsTypeCache<>), excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class MillisecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintTypeCache<>), pretty: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }
}
