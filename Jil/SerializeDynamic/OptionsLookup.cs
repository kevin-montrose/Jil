using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.SerializeDynamic
{
    class OptionsLookup
    {
        #region initialization
        static readonly Dictionary<Options, FieldInfo> FieldLookup = new Dictionary<Options, FieldInfo>();
        static readonly Dictionary<Options, Type> TypeCacheLookup = new Dictionary<Options, Type>();
        static OptionsLookup()
        {
            var precalced = typeof(Options).GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in precalced)
            {
                var opts = (Options)field.GetValue(null);
                FieldLookup[opts] = field;
                TypeCacheLookup[opts] = CalculateTypeCacheFor(opts);
            }
        }

        static Type CalculateTypeCacheFor(Options opts)
        {
            switch(opts.UseDateTimeFormat)
            {
                case DateTimeFormat.ISO8601: return GetISO8601TypeCache(opts);
                case DateTimeFormat.MillisecondsSinceUnixEpoch: return GetMillisecondsTypeCache(opts);
                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch: return GetNewtonsoftTypeCache(opts);
                case DateTimeFormat.SecondsSinceUnixEpoch: return GetSecondsTypeCache(opts);
                case DateTimeFormat.RFC1123: return GetRFC1123TypeCache(opts);
                default: throw new Exception("Unexpected DateTimeFormat: " + opts.UseDateTimeFormat);
            }
        }

        static Type GetNewtonsoftTypeCache(Options opts)
        {
            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNullsJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNullsInherited);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintJSONPInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStyleExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNulls);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.NewtonsoftStyleExcludeNullsJSONP);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStyleExcludeNullsInherited);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStyleJSONPInherited);
            }

            return typeof(Serialize.NewtonsoftStyle);
        }

        static Type GetSecondsTypeCache(Options opts)
        {
            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNullsJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNullsInherited);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintJSONPInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNulls);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.SecondsPrettyPrintJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.SecondsExcludeNullsJSONP);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsExcludeNullsInherited);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsJSONPInherited);
            }

            return typeof(Serialize.Seconds);
        }

        static Type GetRFC1123TypeCache(Options opts)
        {
            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.RFC1123PrettyPrintExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.RFC1123PrettyPrintExcludeNullsJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.RFC1123PrettyPrintExcludeNullsInherited);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.RFC1123PrettyPrintJSONPInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.RFC1123ExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.RFC1123PrettyPrintExcludeNulls);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.RFC1123PrettyPrintJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.RFC1123PrettyPrintInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.RFC1123ExcludeNullsJSONP);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.RFC1123ExcludeNullsInherited);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.RFC1123JSONPInherited);
            }

            return typeof(Serialize.RFC1123);
        }

        static Type GetMillisecondsTypeCache(Options opts)
        {
            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNullsJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNullsInherited);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintJSONPInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNulls);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.MillisecondsPrettyPrintJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.MillisecondsExcludeNullsJSONP);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsExcludeNullsInherited);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsJSONPInherited);
            }

            return typeof(Serialize.Milliseconds);
        }

        static Type GetISO8601TypeCache(Options opts)
        {
            if(opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNullsJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNullsInherited);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintJSONPInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601ExcludeNullsJSONPInherited);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNulls);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.ISO8601PrettyPrintJSONP);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintInherited);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.ISO8601ExcludeNullsJSONP);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601ExcludeNullsInherited);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601JSONPInherited);
            }

            return typeof(Serialize.ISO8601);
        }
        #endregion

        public static FieldInfo GetOptionsFieldFor(Options opts)
        {
            return FieldLookup[opts];
        }

        public static Type GetTypeCacheFor(Options opts)
        {
            return TypeCacheLookup[opts];
        }
    }
}
