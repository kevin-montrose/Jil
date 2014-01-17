using Jil.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    class AnonymousTypeLookup<ForType>
    {
        public static Dictionary<string, Tuple<Type, int>> ParametersToTypeAndIndex;
        public static Dictionary<string, int> Lookup;

        static AnonymousTypeLookup()
        {
            ParametersToTypeAndIndex = Utils.GetAnonymousNameToConstructorMap(typeof(ForType));

            Lookup = ParametersToTypeAndIndex.ToDictionary(d => d.Key, d => d.Value.Item2);
        }
    }
}
