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
    interface ISerializeOptions
    {
        bool PrettyPrint { get; }
        bool ExcludeNulls { get; }
        DateTimeFormat DateFormat { get; }
        bool JSONP { get; }
        bool IncludeInherited { get; }
        UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get; }
    }

    static class TypeCache<TOptions, T>
        where TOptions : ISerializeOptions, new()
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                var opts = new TOptions();

                Thunk = InlineSerializerHelper.Build<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, dateTimeBehavior: opts.DateTimeKindBehavior, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                var opts = new TOptions();

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, dateTimeBehavior: opts.DateTimeKindBehavior, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    class NewtonsoftStyle : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } } 
    }

    class NewtonsoftStyleUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStyleJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStyleJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStyleExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStyleExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStyleJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStyleJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStyleInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStyleInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStyleExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStyleExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStyleExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class Seconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class Milliseconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601 : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601Utc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601JSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601JSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601JSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601JSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601Inherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601InheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }
}