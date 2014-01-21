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
    enum MemberMatcherMode
    {
        None = 0,

        SixtyFour = 64,
        ThirtyTwo = 32,
        Sixteen = 16,
        Eight = 8,
        Four = 4,
        Two = 2,
        One = 1
    }

    class MemberMatcher<ForType>
    {
        public static bool IsAvailable;
        public static Dictionary<string, MemberInfo> MemberLookup;
        public static Dictionary<string, int> BucketLookup;
        public static Dictionary<string, uint> HashLookup;
        public static MemberMatcherMode Mode;

        static MemberMatcher()
        {
            IsAvailable = MakeMemberMatcher(out MemberLookup, out BucketLookup, out HashLookup, out Mode);
        }

        public static MethodInfo GetHashMethod(MemberMatcherMode mode)
        {
            switch (mode)
            {
                case MemberMatcherMode.One: return Methods.MemberHash1;
                case MemberMatcherMode.Two: return Methods.MemberHash2;
                case MemberMatcherMode.Four: return Methods.MemberHash4;
                case MemberMatcherMode.Eight: return Methods.MemberHash8;
                case MemberMatcherMode.Sixteen: return Methods.MemberHash16;
                case MemberMatcherMode.ThirtyTwo: return Methods.MemberHash32;
                case MemberMatcherMode.SixtyFour: return Methods.MemberHash64;
                default: throw new Exception("Unexpected MemberMatcherMode: " + mode);
            }
        }

        static bool MakeMemberMatcher(out Dictionary<string, MemberInfo> memberLookup, out Dictionary<string, int> memberToBucket, out Dictionary<string, uint> memberToHash, out MemberMatcherMode bestMode)
        {
            var forType = typeof(ForType);

            var fields = forType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            var props = forType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Where(p => p.SetMethod != null);

            var members = fields.Cast<MemberInfo>().Concat(props.Cast<MemberInfo>());

            memberLookup = members.ToDictionary(m => m.GetSerializationName(), m => m);

            var memberNames = memberLookup.Keys;

            var allModes =
                Enum.GetValues(typeof(MemberMatcherMode))
                    .Cast<MemberMatcherMode>()
                    .Where(m => m != MemberMatcherMode.None)
                    .OrderBy(o => (int)o)
                    .ToList();

            foreach (var mode in allModes)
            {
                var mToH = new Dictionary<string, uint>();

                var method = GetHashMethod(mode);

                var buckets =
                    memberNames.GroupBy(
                         m =>
                         {
                             using (var str = new StringReader("\"" + m.JsonEscape(jsonp: false) + "\""))
                             {
                                 str.Read();    // skip "

                                 var ps = new object[] { str, (int)-1, (uint)0 };
                                 method.Invoke(null, ps);

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
                    continue;
                }

                memberToHash = mToH;
                memberToBucket = buckets.ToDictionary(d => d.Single(), d => d.Key);
                bestMode = mode;
                return true;
            }


            memberToHash = null;
            memberToBucket = null;
            bestMode = MemberMatcherMode.None;
            return false;
        }
    }
}
