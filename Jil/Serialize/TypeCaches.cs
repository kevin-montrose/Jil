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
    }

    static class TypeCache<TOptions, T>
        where TOptions : ISerializeOptions, new()
    {
        static readonly object InitLock = new object();
        static volatile bool BeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (InitLock)
            {
                if (Thunk != null || BeingBuilt) return;
                BeingBuilt = true;

                var opts = new TOptions();

                Thunk = InlineSerializerHelper.Build<T>(typeof(TOptions), pretty: opts.PrettyPrint, excludeNulls: opts.ExcludeNulls, dateFormat: opts.DateFormat, jsonp: opts.JSONP, includeInherited: opts.IncludeInherited, exceptionDuringBuild: out ExceptionDuringBuild);
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
    }

    class NewtonsoftStyleJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStyleExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStyleExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStylePrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStyleJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStyleInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class NewtonsoftStyleExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class NewtonsoftStylePrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class NewtonsoftStyleExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class NewtonsoftStylePrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class Milliseconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class MillisecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class MillisecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class MillisecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class MillisecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class MillisecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class MillisecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class Seconds : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class SecondsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsPrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsPrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsPrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class SecondsExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class SecondsPrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class SecondsPrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class SecondsExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class SecondsPrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601 : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601JSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601ExcludeNullsJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601PrettyPrintJSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601PrettyPrintExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601ExcludeNullsInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601PrettyPrintInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601JSONPInherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601Inherited : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return true; } }
    }

    class ISO8601ExcludeNullsJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601PrettyPrintJSONP : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return true; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601PrettyPrint : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return false; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601ExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return false; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }

    class ISO8601PrettyPrintExcludeNulls : ISerializeOptions
    {
        public bool PrettyPrint { get { return true; } }
        public bool ExcludeNulls { get { return true; } }
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
        public bool JSONP { get { return false; } }
        public bool IncludeInherited { get { return false; } }
    }
}