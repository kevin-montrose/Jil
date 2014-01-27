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
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStyleJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStyleExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStyleJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStyleInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStyleExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStyleExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStyleExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static NewtonsoftStylePrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static MillisecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class SecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static SecondsPrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601TypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601TypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601TypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601JSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601JSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601JSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601ExcludeNullsJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601ExcludeNullsJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintJSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintJSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601ExcludeNullsInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601ExcludeNullsInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601JSONPInheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601JSONPInheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601JSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601InheritedTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601InheritedTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601InheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601ExcludeNullsJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601ExcludeNullsJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintJSONPTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintJSONPTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601ExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601ExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }

    static class ISO8601PrettyPrintExcludeNullsTypeCache<T>
    {
        public static readonly Action<TextWriter, T, int> Thunk;
        public static readonly Exception ExceptionDuringBuild;

        static ISO8601PrettyPrintExcludeNullsTypeCache()
        {
            Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ExceptionDuringBuild);
        }
    }
}
