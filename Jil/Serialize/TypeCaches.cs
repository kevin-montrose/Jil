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

    class MicrosoftStyle : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } } 
    }

    class MicrosoftStyleUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrooftStylePrettyPrintJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStyleExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStyleExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class MicrosoftStylePrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class MicrosoftStylePrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch; } }
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

    class RFC1123 : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123Utc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123JSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123JSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNullsJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintJSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNullsInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123JSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123JSONPInheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123Inherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123InheritedUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNullsJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintJSONPUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123ExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123ExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsUTC; } }
    }

    class RFC1123PrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior { get { return UnspecifiedDateTimeKindBehavior.IsLocal; } }
    }

    class RFC1123PrettyPrintExcludeNullsUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.RFC1123; } }
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