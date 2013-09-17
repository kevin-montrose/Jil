using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    class StringConstants
    {
        public string TotalString { get; private set; }
        public FieldInfo StoredInField { get; private set; }

        private Dictionary<string, int> Lookup;

        public StringConstants(List<string> allStrings, FieldInfo storedInField)
        {
            allStrings = allStrings.Distinct().ToList();

            var uniqueStrs = allStrings.Where(s => !allStrings.Any(t => s != t && t.IndexOf(s) != -1)).ToList();

            TotalString = string.Concat(uniqueStrs);

            Lookup = allStrings.ToDictionary(s => s, s => TotalString.IndexOf(s));

            StoredInField = storedInField;
        }

        public int? LookupString(string str)
        {
            if (!Lookup.ContainsKey(str)) return null;

            return Lookup[str];
        }
    }
}
