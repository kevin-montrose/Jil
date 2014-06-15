using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;
using System.IO;

namespace Jil.Deserialize
{
    enum EnumMatcherMode
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

    class EnumMatcher<EnumType>
        where EnumType : struct
    {
        public static bool IsAvailable;
        public static Dictionary<string, object> EnumLookup;
        public static Dictionary<string, int> BucketLookup;
        public static Dictionary<string, uint> HashLookup;
        public static EnumMatcherMode Mode;

        static EnumMatcher()
        {
            IsAvailable = MakeEnumMatcher(out EnumLookup, out BucketLookup, out HashLookup, out Mode);
        }

        public static MethodInfo GetHashMethod(EnumMatcherMode mode)
        {
            switch (mode)
            {
                case EnumMatcherMode.One: return Methods.MemberHash1;
                case EnumMatcherMode.Two: return Methods.MemberHash2;
                case EnumMatcherMode.Four: return Methods.MemberHash4;
                case EnumMatcherMode.Eight: return Methods.MemberHash8;
                case EnumMatcherMode.Sixteen: return Methods.MemberHash16;
                case EnumMatcherMode.ThirtyTwo: return Methods.MemberHash32;
                case EnumMatcherMode.SixtyFour: return Methods.MemberHash64;
                default: throw new Exception("Unexpected EnumMatcherMode: " + mode);
            }
        }

        static bool MakeEnumMatcher(out Dictionary<string, object> enumLookup, out Dictionary<string, int> memberToBucket, out Dictionary<string, uint> memberToHash, out EnumMatcherMode bestMode)
        {
            var forType = typeof(EnumType);

            var allValues = Enum.GetValues(forType);

            enumLookup = allValues.Cast<EnumType>().ToDictionary(e => forType.GetEnumValueName(e), e => Convert.ChangeType(e, Enum.GetUnderlyingType(forType)));

            var enumNames = enumLookup.Keys;

            var allModes =
                Enum.GetValues(typeof(EnumMatcherMode))
                    .Cast<EnumMatcherMode>()
                    .Where(m => m != EnumMatcherMode.None)
                    .OrderBy(o => (int)o)
                    .ToList();

            foreach (var mode in allModes)
            {
                var mToH = new Dictionary<string, uint>();

                var method = GetHashMethod(mode);

                var buckets =
                    enumNames.GroupBy(
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
            bestMode = EnumMatcherMode.None;
            return false;
        }
    }
}
