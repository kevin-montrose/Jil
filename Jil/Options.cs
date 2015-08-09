using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jil
{
    /// <summary>
    /// Configuration options for Jil serialization, passed to the JSON.Serialize method.
    /// </summary>
    public sealed class Options
    {
#pragma warning disable 1591
        // Start OptionsGeneration.linq generated content
        public static readonly Options Default = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch);
        public static readonly Options PrettyPrint = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true);
        public static readonly Options ExcludeNulls = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options JSONP = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options IncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: true);
        public static readonly Options Utc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true);
        public static readonly Options PrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true);
        public static readonly Options PrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true);
        public static readonly Options PrettyPrintUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options ExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true);
        public static readonly Options ExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options JSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options JSONPUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options IncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options PrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options PrettyPrintExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, includeInherited: true);
        public static readonly Options PrettyPrintJSONPUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options ExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options JSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options PrettyPrintExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options PrettyPrintExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601 = new Options(dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrint = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true);
        public static readonly Options ISO8601ExcludeNulls = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true);
        public static readonly Options ISO8601JSONP = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601IncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true);
        public static readonly Options ISO8601Utc = new Options(dateFormat: DateTimeFormat.ISO8601, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true);
        public static readonly Options ISO8601PrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, jsonp: true);
        public static readonly Options ISO8601PrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601ExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true);
        public static readonly Options ISO8601ExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, includeInherited: true);
        public static readonly Options ISO8601ExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601JSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601JSONPUtc = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601IncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintJSONPUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601ExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601ExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601ExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601JSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601ExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrint = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNulls = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options MillisecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochJSONPUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONPUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123 = new Options(dateFormat: DateTimeFormat.RFC1123);
        public static readonly Options RFC1123PrettyPrint = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true);
        public static readonly Options RFC1123ExcludeNulls = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true);
        public static readonly Options RFC1123JSONP = new Options(dateFormat: DateTimeFormat.RFC1123, jsonp: true);
        public static readonly Options RFC1123IncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, includeInherited: true);
        public static readonly Options RFC1123Utc = new Options(dateFormat: DateTimeFormat.RFC1123, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true);
        public static readonly Options RFC1123PrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, jsonp: true);
        public static readonly Options RFC1123PrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, includeInherited: true);
        public static readonly Options RFC1123PrettyPrintUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123ExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true, jsonp: true);
        public static readonly Options RFC1123ExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true, includeInherited: true);
        public static readonly Options RFC1123ExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123JSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, jsonp: true, includeInherited: true);
        public static readonly Options RFC1123JSONPUtc = new Options(dateFormat: DateTimeFormat.RFC1123, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123IncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options RFC1123PrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options RFC1123PrettyPrintExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, jsonp: true, includeInherited: true);
        public static readonly Options RFC1123PrettyPrintJSONPUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123ExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options RFC1123ExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123ExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123JSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options RFC1123PrettyPrintExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123ExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options RFC1123PrettyPrintExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.RFC1123, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrint = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNulls = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options SecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochJSONPUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONPUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPIncludeInheritedUtc = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC);
        // End OptionsGeneration.linq generated content
#pragma warning restore 1591

        internal bool ShouldPrettyPrint { get; private set; }
        internal bool ShouldExcludeNulls { get; private set; }
        internal DateTimeFormat UseDateTimeFormat { get; private set; }
        internal bool IsJSONP { get; private set; }
        internal bool ShouldIncludeInherited { get; private set; }
        internal UnspecifiedDateTimeKindBehavior UseUnspecifiedDateTimeKindBehavior { get; private set; }

        /// <summary>
        /// Configuration for Jil serialization options.
        /// 
        /// Available options:
        ///   prettyPrint - whether or not to include whitespace and newlines for ease of reading
        ///   excludeNulls - whether or not to write object members whose value is null
        ///   jsonp - whether or not the serialized json should be valid for use with JSONP
        ///   dateFormat - the style in which to serialize DateTimes and TimeSpans
        ///   includeInherited - whether or not to serialize members declared by an objects base types
        ///   unspecifiedDateTimeKindBehavior - how to treat DateTime's with a DateTimeKind of Unspecified (Jil converts all DateTimes to Utc for serialization, use DateTimeOffset to preserve time zone information)
        /// </summary>
        public Options(bool prettyPrint = false, bool excludeNulls = false, bool jsonp = false, DateTimeFormat dateFormat = DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, bool includeInherited = false, UnspecifiedDateTimeKindBehavior unspecifiedDateTimeKindBehavior = UnspecifiedDateTimeKindBehavior.IsLocal)
        {
            ShouldPrettyPrint = prettyPrint;
            ShouldExcludeNulls = excludeNulls;
            IsJSONP = jsonp;

#pragma warning disable 618
            // upgrade from the obsolete DateTimeFormat enumeration; warning disabled to allow it, but all other references
            //  should be expunged
            if (dateFormat == DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch)
            {
                dateFormat = DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch;
            }
#pragma warning restore 618

            UseDateTimeFormat = dateFormat;
            ShouldIncludeInherited = includeInherited;
            UseUnspecifiedDateTimeKindBehavior = unspecifiedDateTimeKindBehavior;
        }

        /// <summary>
        /// Returns a string representation of this Options object.
        /// 
        /// The format of this may change at any time, it is only meant for debugging.
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "{{ ShouldPrettyPrint = {0}, ShouldExcludeNulls = {1}, UseDateTimeFormat = {2}, IsJSONP = {3}, ShouldIncludeInherited = {4}, UseUnspecifiedDateTimeKindBehavior = {5} }}",
                    ShouldPrettyPrint,
                    ShouldExcludeNulls,
                    UseDateTimeFormat,
                    IsJSONP,
                    ShouldIncludeInherited,
                    UseUnspecifiedDateTimeKindBehavior
                );
        }

        /// <summary>
        /// Returns a code that uniquely identifies this set of Options.
        /// </summary>
        public override int GetHashCode()
        {
            const int isoMask = 0x20;
            const int milliMask = isoMask * 2;
            const int microsoftMask = milliMask * 2;
            const int secondsMask = microsoftMask * 2;
            const int rfc1123Mask = secondsMask * 2;
            const int localMask = rfc1123Mask * 2;
            const int utcMask = localMask * 2;

            int dateTimeMask;
            switch (UseDateTimeFormat)
            {
                case DateTimeFormat.ISO8601: dateTimeMask = isoMask; break;
                case DateTimeFormat.MillisecondsSinceUnixEpoch: dateTimeMask = milliMask; break;

                case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch: dateTimeMask = microsoftMask; break;

                case DateTimeFormat.SecondsSinceUnixEpoch: dateTimeMask = secondsMask; break;
                case DateTimeFormat.RFC1123: dateTimeMask = rfc1123Mask; break;
                default: throw new Exception("Unexpected DateTimeFormat " + UseDateTimeFormat);
            }

            int unspecifiedMask;
            switch (UseUnspecifiedDateTimeKindBehavior)
            {
                case UnspecifiedDateTimeKindBehavior.IsLocal: unspecifiedMask = localMask; break;
                case UnspecifiedDateTimeKindBehavior.IsUTC: unspecifiedMask = utcMask; break;
                default: throw new Exception("Unexpected UnspecifiedDateTimeKindBehavior " + UseUnspecifiedDateTimeKindBehavior);
            }

            return
                (ShouldPrettyPrint ? 0x1 : 0x0) |
                (ShouldExcludeNulls ? 0x2 : 0x0) |
                (IsJSONP ? 0x4 : 0x0) |
                (ShouldIncludeInherited ? 0x8 : 0x0) |
                dateTimeMask |
                unspecifiedMask;
        }

        /// <summary>
        /// Returns true if two Options are equal, and false otherwise
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = obj as Options;
            if (other == null) return false;

            return
                other.UseDateTimeFormat == this.UseDateTimeFormat &&
                other.ShouldPrettyPrint == this.ShouldPrettyPrint &&
                other.ShouldExcludeNulls == this.ShouldExcludeNulls &&
                other.IsJSONP == this.IsJSONP &&
                other.ShouldIncludeInherited == this.ShouldIncludeInherited &&
                other.UseUnspecifiedDateTimeKindBehavior == this.UseUnspecifiedDateTimeKindBehavior;
        }
    }
}