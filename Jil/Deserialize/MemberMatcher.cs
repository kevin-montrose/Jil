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
        public static Dictionary<string, int> BucketLookup;
        public static Dictionary<string, uint> HashLookup;

        static MemberMatcher()
        {
            IsAvailable = MakeMemberMatcher(out IsEligible, out BucketLookup, out HashLookup);
        }

        static bool MakeMemberMatcher(out bool eligible, out Dictionary<string, int> memberToBucket, out Dictionary<string, uint> memberToHash)
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
                memberToBucket = null;
                memberToHash = null;
                return false;
            }

            eligible = true;

            var mToH = new Dictionary<string,uint>();

            var buckets =
                memberNames.GroupBy(
                     m =>
                     {
                         using (var str = new StringReader("\"" + m.JsonEscape(jsonp: false) + "\""))
                         {
                             str.Read();    // skip "

                             var ps = new object[] { str, (int)-1, (uint)0 };
                             Methods.MemberHash.Invoke(null, ps);

                             var bucket = (int)ps[1];
                             var hash = (uint)ps[2];

                             mToH[m] = hash;

                             return bucket;
                         }
                     }
                );

            var collisions = buckets.Any(g => g.Count() > 1);

            if (collisions)
            {
                memberToHash = null;
                memberToBucket = null;
                return false;
            }

            memberToHash = mToH;
            memberToBucket = buckets.ToDictionary(d => d.Single(), d => d.Key);
            return true;
        }
    }
}
