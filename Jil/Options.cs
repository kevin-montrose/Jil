using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jil
{
    public sealed class Options
    {
        public static readonly Options None = new Options();
        public static readonly Options Pretty = new Options(indent: 2);
        public static readonly Options ExcludeNulls = new Options(excludeNulls: true);

        internal int? Indent { get; set; }
        internal bool? ShouldExcludeNulls { get; set; }

        public Options(int? indent = null, bool? excludeNulls = null)
        {
            Indent = indent;
            ShouldExcludeNulls = excludeNulls;
        }
    }
}
