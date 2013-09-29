using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jil
{
    public sealed class Options
    {
        public static readonly Options None = new Options();
        public static readonly Options ExcludeNulls = new Options(excludeNulls: true);
        public static readonly Options PrettyPrint = new Options(pretty: true);
        public static readonly Options PrettyPrintExcludeNulls = new Options(pretty: true, excludeNulls: true);

        internal bool? ShouldPrettyPrint { get; set; }
        internal bool? ShouldExcludeNulls { get; set; }

        private Options(bool? pretty = null, bool? excludeNulls = null)
        {
            ShouldPrettyPrint = pretty;
            ShouldExcludeNulls = excludeNulls;
        }
    }
}
