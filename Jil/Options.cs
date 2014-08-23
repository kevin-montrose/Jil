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
        public static readonly Options Default = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        public static readonly Options PrettyPrint = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true);
        public static readonly Options ExcludeNulls = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options JSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options NoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashFunction: false);
        public static readonly Options IncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true);
        public static readonly Options PrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true);
        public static readonly Options PrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true);
        public static readonly Options PrettyPrintNoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, allowHashFunction: false);
        public static readonly Options PrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true);
        public static readonly Options ExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options ExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, allowHashFunction: false);
        public static readonly Options ExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true);
        public static readonly Options JSONPNoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false);
        public static readonly Options JSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options NoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashFunction: false, includeInherited: true);
        public static readonly Options PrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options PrettyPrintExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, allowHashFunction: false);
        public static readonly Options PrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options PrettyPrintJSONPNoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false);
        public static readonly Options PrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options PrettyPrintNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashFunction: false, includeInherited: true);
        public static readonly Options ExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options ExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options ExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options JSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options PrettyPrintExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options PrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options PrettyPrintExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options PrettyPrintJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options ExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options PrettyPrintExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);

        public static readonly Options MillisecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrint = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNulls = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options MillisecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochJSONPNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, allowHashFunction: false, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONPNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, allowHashFunction: false, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);

        public static readonly Options ISO8601 = new Options(dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrint = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true);
        public static readonly Options ISO8601ExcludeNulls = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true);
        public static readonly Options ISO8601JSONP = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601NoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, allowHashFunction: false);
        public static readonly Options ISO8601IncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true);
        public static readonly Options ISO8601PrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, jsonp: true);
        public static readonly Options ISO8601PrettyPrintNoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, allowHashFunction: false);
        public static readonly Options ISO8601PrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, includeInherited: true);
        public static readonly Options ISO8601ExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true);
        public static readonly Options ISO8601ExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, allowHashFunction: false);
        public static readonly Options ISO8601ExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, includeInherited: true);
        public static readonly Options ISO8601JSONPNoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, allowHashFunction: false);
        public static readonly Options ISO8601JSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601NoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, allowHashFunction: false, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, allowHashFunction: false);
        public static readonly Options ISO8601PrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintJSONPNoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, allowHashFunction: false);
        public static readonly Options ISO8601PrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, allowHashFunction: false, includeInherited: true);
        public static readonly Options ISO8601ExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options ISO8601ExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601ExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options ISO8601JSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options ISO8601ExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.ISO8601, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);

        public static readonly Options SecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrint = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNulls = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options SecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochJSONPNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, allowHashFunction: false, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONPNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, allowHashFunction: false, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPNoHashing = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONPNoHashingIncludeInherited = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true, allowHashFunction: false, includeInherited: true);
#pragma warning restore 1591

        internal bool ShouldPrettyPrint { get; private set; }
        internal bool ShouldExcludeNulls { get; private set; }
        internal DateTimeFormat UseDateTimeFormat { get; private set; }
        internal bool IsJSONP { get; private set; }
        internal bool ShouldIncludeInherited { get; private set; }
        internal bool AllowHashFunction { get; private set; }

        /// <summary>
        /// Configuration for Jil serialization options.
        /// 
        /// Available options:
        ///   prettyPrint - whether or not to include whitespace and newlines for ease of reading
        ///   excludeNulls - whether or not to write object members whose value is null
        ///   jsonp - whether or not the serialized json should be valid for use with JSONP
        ///   dateFormat - the style in which to serialize DateTimes
        ///   includeInherited - whether or not to serialize members declared by an objects base types
        ///   allowHashFunction - whether or not Jil should try to use hashes instead of strings when deserializing object members, malicious content may be able to force member collisions if this is enabled
        /// </summary>
        public Options(bool prettyPrint = false, bool excludeNulls = false, bool jsonp = false, DateTimeFormat dateFormat = DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, bool includeInherited = false, bool allowHashFunction = true)
        {
            ShouldPrettyPrint = prettyPrint;
            ShouldExcludeNulls = excludeNulls;
            IsJSONP = jsonp;
            UseDateTimeFormat = dateFormat;
            ShouldIncludeInherited = includeInherited;
            AllowHashFunction = allowHashFunction;
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
                    "{{ ShouldPrettyPrint = {0}, ShouldExcludeNulls = {1}, UseDateTimeFormat = {2}, IsJSONP = {3}, ShouldIncludeInherited = {4}, AllowHashFunction = {5} }}",
                    ShouldPrettyPrint,
                    ShouldExcludeNulls,
                    UseDateTimeFormat,
                    IsJSONP,
                    ShouldIncludeInherited,
                    AllowHashFunction
                );
        }

        /// <summary>
        /// Returns a code that uniquely identifies this set of Options.
        /// </summary>
        public override int GetHashCode()
        {
            int dateTimeMask;
            switch(UseDateTimeFormat)
            {
                case DateTimeFormat.ISO8601: dateTimeMask = 0x20; break;
                case DateTimeFormat.MillisecondsSinceUnixEpoch: dateTimeMask = 0x40; break;
                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch: dateTimeMask = 0x80; break;
                case DateTimeFormat.SecondsSinceUnixEpoch: dateTimeMask = 0x100; break;
                default: throw new Exception("Unexpected DateTimeFormat "+UseDateTimeFormat);
            }

            return
                (ShouldPrettyPrint ? 0x1 : 0x0) |
                (ShouldExcludeNulls ? 0x2 : 0x0) |
                (IsJSONP ? 0x4 : 0x0) |
                (ShouldIncludeInherited ? 0x8 : 0x0) |
                (AllowHashFunction ? 0x10 : 0x0) |
                dateTimeMask;
        }

        /// <summary>
        /// Returns true if two Options are equal, and false otherwise
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = obj as Options;
            if(other == null) return false;

            return
                other.UseDateTimeFormat == this.UseDateTimeFormat &&
                other.ShouldPrettyPrint == this.ShouldPrettyPrint &&
                other.ShouldExcludeNulls == this.ShouldExcludeNulls &&
                other.IsJSONP == this.IsJSONP &&
                other.ShouldIncludeInherited == this.ShouldIncludeInherited &&
                other.AllowHashFunction == this.AllowHashFunction;
        }
    }
}
