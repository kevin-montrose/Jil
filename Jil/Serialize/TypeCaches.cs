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

    // Start OptionsGeneration.linq generated content
    class MicrosoftStyle : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrint : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }
    class MicrosoftStylePrettyPrintJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601 : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrint : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601JSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601Inherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601Utc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601JSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601JSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601InheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601ExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601JSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601ExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class Milliseconds : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrint : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123 : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrint : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123JSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123Inherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123Utc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123JSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123JSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123InheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123JSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class Seconds : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrint : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }
    // End OptionsGeneration.linq generated content
}