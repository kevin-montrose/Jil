using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jil
{
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

        internal bool? ShouldPrettyPrint { get; set; }
        internal bool? ShouldExcludeNulls { get; set; }
        internal DateTimeFormat? UseDateTimeFormat { get; set; }

        private Options(bool pretty = false, bool excludeNulls = false, DateTimeFormat dateFormat = DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch)
        {
            ShouldPrettyPrint = pretty;
            ShouldExcludeNulls = excludeNulls;
            UseDateTimeFormat = dateFormat;
        }
    }
}
