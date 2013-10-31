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
        public static readonly Options Default = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        public static readonly Options ExcludeNulls = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true);
        public static readonly Options PrettyPrint = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, pretty: true);
        public static readonly Options PrettyPrintExcludeNulls = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, pretty: true, excludeNulls: true);
        public static readonly Options JSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options ExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, excludeNulls: true, jsonp: true);
        public static readonly Options PrettyPrintJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, pretty: true, jsonp: true);
        public static readonly Options PrettyPrintExcludeNullsJSONP = new Options(dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, pretty: true, excludeNulls: true, jsonp: true);

        public static readonly Options MillisecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrint = new Options(pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNullsJSONP = new Options(excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintJSONP = new Options(pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: true);

        public static readonly Options ISO8601 = new Options(dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601ExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrint = new Options(pretty: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601JSONP = new Options(dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601ExcludeNullsJSONP = new Options(excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601PrettyPrintJSONP = new Options(pretty: true, dateFormat: DateTimeFormat.ISO8601, jsonp: true);
        public static readonly Options ISO8601PrettyPrintExcludeNullsJSONP = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: true);

        public static readonly Options SecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrint = new Options(pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochJSONP = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochExcludeNullsJSONP = new Options(excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintJSONP = new Options(pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNullsJSONP = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: true);

        internal bool ShouldPrettyPrint { get; private set; }
        internal bool ShouldExcludeNulls { get; private set; }
        internal DateTimeFormat UseDateTimeFormat { get; private set; }
        internal bool IsJSONP { get; private set; }

        private Options(bool pretty = false, bool excludeNulls = false, bool jsonp = false, DateTimeFormat dateFormat = DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch)
        {
            ShouldPrettyPrint = pretty;
            ShouldExcludeNulls = excludeNulls;
            IsJSONP = jsonp;
            UseDateTimeFormat = dateFormat;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{{ ShouldPrettyPrint = {0}, ShouldExcludeNulls = {1}, UseDateTimeFormat = {2}, IsJSONP = {3} }}",
                    ShouldPrettyPrint,
                    ShouldExcludeNulls,
                    UseDateTimeFormat,
                    JSONP
                );
        }
    }
}
