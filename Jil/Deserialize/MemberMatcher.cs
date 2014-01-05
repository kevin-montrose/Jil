using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;

namespace Jil.Deserialize
{
    class MemberMatcher<ForType>
    {
        public static bool IsEligible;
        public static bool IsAvailable;
        public static Dictionary<string, int> HashLookup;

        static MemberMatcher()
        {
            IsAvailable = MakeMemberMatcher(out IsEligible, out HashLookup);
        }

        static bool MakeMemberMatcher(out bool eligible, out Dictionary<string, int> hashToMember)
        {
            var forType = typeof(ForType);

            var fields = forType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            var props = forType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Where(p => p.SetMethod != null);

            var members = fields.Cast<MemberInfo>().Concat(props.Cast<MemberInfo>());

            var memberNames = members.Select(m => m.Name).ToList();

            // This can't be uncapped
            if (memberNames.Count > Methods.MaximumMemberHashes)
            {
                eligible = false;
                hashToMember = null;
                return false;
            }

            eligible = true;

            var hashed =
                memberNames.GroupBy(
                     m =>
                     {
                         using (var str = new StringReader("\"" + m.JsonEscape(jsonp: false) + "\""))
                         {
                             str.Read();    // skip "
                             var ret = (int)Methods.MemberHash.Invoke(null, new object[] { str, null });

                             return ret;
                         }
                     }
                );

            var collisions = hashed.Any(g => g.Count() > 1);

            if (collisions)
            {
                hashToMember = null;
                return false;
            }

            hashToMember = hashed.ToDictionary(d => d.Single(), d => d.Key);
            return true;
        }
    }
}
