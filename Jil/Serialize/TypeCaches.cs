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
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class NewtonsoftStyleJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true);
        }
    }

    static class NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true);
        }
    }

    static class NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false);
        }
    }

    static class NewtonsoftStyleExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false);
        }
    }

    static class NewtonsoftStylePrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class NewtonsoftStyleJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false);
        }
    }

    static class NewtonsoftStyleInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class NewtonsoftStyleExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class NewtonsoftStylePrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class NewtonsoftStylePrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class NewtonsoftStyleExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStyleExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static NewtonsoftStylePrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class MillisecondsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class MillisecondsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true);
        }
    }

    static class MillisecondsExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true);
        }
    }

    static class MillisecondsPrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false);
        }
    }

    static class MillisecondsExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false);
        }
    }

    static class MillisecondsPrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class MillisecondsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false);
        }
    }

    static class MillisecondsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class MillisecondsExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class MillisecondsPrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class MillisecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class MillisecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static MillisecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class SecondsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class SecondsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true);
        }
    }

    static class SecondsExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true);
        }
    }

    static class SecondsPrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false);
        }
    }

    static class SecondsPrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false);
        }
    }

    static class SecondsExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false);
        }
    }

    static class SecondsPrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class SecondsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false);
        }
    }

    static class SecondsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class SecondsExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class SecondsPrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false);
        }
    }

    static class SecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class SecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class SecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static SecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false);
        }
    }

    static class ISO8601TypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601TypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601TypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false);
        }
    }

    static class ISO8601JSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601JSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601JSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true);
        }
    }

    static class ISO8601ExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601ExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true);
        }
    }

    static class ISO8601PrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false);
        }
    }

    static class ISO8601ExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601ExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false);
        }
    }

    static class ISO8601PrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class ISO8601JSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601JSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601JSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false);
        }
    }

    static class ISO8601InheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601InheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601InheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false);
        }
    }

    static class ISO8601ExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601ExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false);
        }
    }

    static class ISO8601PrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false);
        }
    }

    static class ISO8601PrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false);
        }
    }

    static class ISO8601ExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601ExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;

        static ISO8601PrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false);
        }
    }
}
