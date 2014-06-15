using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;

namespace Jil.Deserialize
{
    class EnumValues<TEnum>
        where TEnum : struct
    {
        static readonly Dictionary<string, TEnum> PreCalculated;

        static EnumValues()
        {
            PreCalculated = new Dictionary<string, TEnum>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var val in Enum.GetValues(typeof(TEnum)))
            {
                var name = typeof(TEnum).GetEnumValueName(val);
                PreCalculated[name] = (TEnum)val;
            }
        }

        public static bool TryParse(string str, out TEnum ret)
        {
            return PreCalculated.TryGetValue(str, out ret);
        }
    }
}
