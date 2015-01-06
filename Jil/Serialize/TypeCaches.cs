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
        bool ISO8601ShouldNotConvertToUtc { get; }
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

                Thunk = InlineSerializerHelper.Build<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, iso8601ShouldNotConvertToUtc: opts.ISO8601ShouldNotConvertToUtc, exceptionDuringBuild: out ThunkExceptionDuringBuild);
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

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, iso8601ShouldNotConvertToUtc: opts.ISO8601ShouldNotConvertToUtc, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
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
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStyleExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class Milliseconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class Seconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601 : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601JSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601ExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601ExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601JSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601Inherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601ExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601ExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
        public bool ISO8601ShouldNotConvertToUtc { get { return false; } }
    }
}