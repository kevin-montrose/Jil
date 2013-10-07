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
    static class NewtonSoftStyleTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonSoftStyleTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonSoftStyleTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class NewtonsoftStylePrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class NewtonsoftStyleExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
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
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsTypeCache<>), dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class SecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsTypeCache<>), excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class SecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintTypeCache<>), pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class SecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class MillisecondsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsTypeCache<>), dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }

    static class MillisecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsTypeCache<>), excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }

    static class MillisecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintTypeCache<>), pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }
}
