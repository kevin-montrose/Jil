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
        bool ConvertToUtc { get; }
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

                Thunk = InlineSerializerHelper.Build<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, shouldConvertToUtc: opts.ConvertToUtc, exceptionDuringBuild: out ThunkExceptionDuringBuild);
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

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, shouldConvertToUtc: opts.ConvertToUtc, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
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
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class NewtonsoftStyleNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class Milliseconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class MillisecondsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class Seconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class SecondsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601 : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601JSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601ExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601ExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601JSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601Inherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601ExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601ExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601PrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601NotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601JSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601ExcludeNullsJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintJSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return true; } }
    }

    class ISO8601ExcludeNullsInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601JSONPInheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601InheritedNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601ExcludeNullsJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintJSONPNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601ExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsNotConvertToUtc : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ConvertToUtc { get { return false; } }
    }
}
