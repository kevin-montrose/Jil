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
        public static readonly Options Default = new Options();
        public static readonly Options ExcludeNulls = new Options(excludeNulls: true);
        public static readonly Options PrettyPrint = new Options(pretty: true);
        public static readonly Options PrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true);

        public static readonly Options MillisecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrint = new Options(pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);
        public static readonly Options MillisecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch);

        public static readonly Options ISO8601 = new Options(dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601ExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrint = new Options(pretty: true, dateFormat: DateTimeFormat.ISO8601);
        public static readonly Options ISO8601PrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601);

        public static readonly Options SecondsSinceUnixEpoch = new Options(dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochExcludeNulls = new Options(excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrint = new Options(pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);
        public static readonly Options SecondsSinceUnixEpochPrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch);

        internal bool ShouldPrettyPrint { get; private set; }
        internal bool ShouldExcludeNulls { get; private set; }
        internal DateTimeFormat UseDateTimeFormat { get; private set; }
        internal bool JSONP { get; private set; }

        private Options(bool pretty = false, bool excludeNulls = false, bool jsonp = false, DateTimeFormat dateFormat = DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch)
        {
            ShouldPrettyPrint = pretty;
            ShouldExcludeNulls = excludeNulls;
            JSONP = jsonp;
            UseDateTimeFormat = dateFormat;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{{ ShouldPrettyPrint = {0}, ShouldExcludeNulls = {1}, UseDateTimeFormat = {2}, JSONP = {3} }}",
                    ShouldPrettyPrint,
                    ShouldExcludeNulls,
                    UseDateTimeFormat,
                    JSONP
                );
        }
    }
}
