using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static class SetterLookup<ForType>
    {
        public static Dictionary<string, int> Lookup;

        static SetterLookup()
        {
            var setters = GetSetters();
            var asList = setters.Select(s => s.Key).OrderBy(_ => _).ToList();

            Lookup = asList.ToDictionary(d => d, d => asList.IndexOf(d));
        }

        public static Dictionary<string, MemberInfo> GetSetters()
        {
            var forType = typeof(ForType);
            var fields = forType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            var props = forType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Where(p => p.SetMethod != null);

            var members = fields.Cast<MemberInfo>().Concat(props.Cast<MemberInfo>());

            return members.ToDictionary(m => m.Name, m => m);
        }
    }
}
