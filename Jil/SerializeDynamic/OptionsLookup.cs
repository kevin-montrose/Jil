using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.SerializeDynamic
{
    class OptionsLookup
    {
        static readonly Dictionary<Options, FieldInfo> Lookup = new Dictionary<Options, FieldInfo>();
        static OptionsLookup()
        {
            var precalced = typeof(Options).GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in precalced)
            {
                var opts = (Options)field.GetValue(null);
                Lookup[opts] = field;
            }
        }

        public static FieldInfo GetFor(Options opts)
        {
            return Lookup[opts];
        }
    }
}
