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
    static class NewtonsoftStyleTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class NewtonsoftStyleJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class NewtonsoftStyleExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class NewtonsoftStylePrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
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

    static class ISO8601JSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601JSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601JSONPTypeCache<>), dateFormat: DateTimeFormat.ISO8601, jsonp: true);
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

    static class ISO8601PrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintJSONPTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class ISO8601ExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601ExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsJSONPTypeCache<>), jsonp: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: true);
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

    static class SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class SecondsExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class SecondsPrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class SecondsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
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

    static class MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }

    static class MillisecondsExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }

    static class MillisecondsPrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }

    static class MillisecondsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }
}
