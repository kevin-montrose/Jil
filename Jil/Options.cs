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
        public static readonly Options ExcludeNulls = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options PrettyPrint = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true);
        public static readonly Options PrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true);
        public static readonly Options JSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options ExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options PrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, jsonp: true);
        public static readonly Options PrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, prettyPrint: true, excludeNulls: true, jsonp: true);

        public static readonly Options MillisecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrint = new Options(prettyPrint: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(prettyPrint: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONP = new Options(excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONP = new Options(prettyPrint: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(prettyPrint: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);

        public static readonly Options ISO8601 = new Options(dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601ExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrint = new Options(prettyPrint: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrintExcludeNulls = new Options(prettyPrint: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601JSONP = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601ExcludeNullsJSONP = new Options(excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601PrettyPrintJSONP = new Options(prettyPrint: true, dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONP = new Options(prettyPrint: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: true);

        public static readonly Options SecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrint = new Options(prettyPrint: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(prettyPrint: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONP = new Options(excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONP = new Options(prettyPrint: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(prettyPrint: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
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
                    JSONP,
                    AllowHashFunction
                );
        }
    }
}
