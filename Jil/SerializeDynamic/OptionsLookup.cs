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
                default: throw new Exception("Unexpected DateTimeFormat: " + opts.UseDateTimeFormat);
            }
        }

        static Type GetNewtonsoftTypeCache(Options opts)
        {
            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintExcludeNullsTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStylePrettyPrintInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.NewtonsoftStyleExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStyleExcludeNullsInheritedTypeCache<>);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.NewtonsoftStyleJSONPInheritedTypeCache<>);
            }

            return typeof(Serialize.NewtonsoftStyleTypeCache<>);
        }

        static Type GetSecondsTypeCache(Options opts)
        {
            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNullsInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.SecondsPrettyPrintExcludeNullsTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.SecondsPrettyPrintJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsPrettyPrintInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.SecondsExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsExcludeNullsInheritedTypeCache<>);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.SecondsJSONPInheritedTypeCache<>);
            }

            return typeof(Serialize.SecondsTypeCache<>);
        }

        static Type GetMillisecondsTypeCache(Options opts)
        {
            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.MillisecondsPrettyPrintExcludeNullsTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.MillisecondsPrettyPrintJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsPrettyPrintInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.MillisecondsExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsExcludeNullsInheritedTypeCache<>);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.MillisecondsJSONPInheritedTypeCache<>);
            }

            return typeof(Serialize.MillisecondsTypeCache<>);
        }

        static Type GetISO8601TypeCache(Options opts)
        {
            if(opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNullsInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601ExcludeNullsJSONPInheritedTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldExcludeNulls)
            {
                return typeof(Serialize.ISO8601PrettyPrintExcludeNullsTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.IsJSONP)
            {
                return typeof(Serialize.ISO8601PrettyPrintJSONPTypeCache<>);
            }

            if (opts.ShouldPrettyPrint && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601PrettyPrintInheritedTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.IsJSONP)
            {
                return typeof(Serialize.ISO8601ExcludeNullsJSONPTypeCache<>);
            }

            if (opts.ShouldExcludeNulls && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601ExcludeNullsInheritedTypeCache<>);
            }

            if (opts.IsJSONP && opts.ShouldIncludeInherited)
            {
                return typeof(Serialize.ISO8601JSONPInheritedTypeCache<>);
            }

            return typeof(Serialize.ISO8601TypeCache<>);
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
