using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    class StringConstants
    {
        public string TotalString { get; private set; }

        private Dictionary<string, int> Lookup;

        public StringConstants(string totalString, Dictionary<string, int> lookup)
        {
            TotalString = totalString;
            Lookup = lookup;
        }

        public int LookupString(string str)
        {
            return Lookup[str];
        }
    }
}
