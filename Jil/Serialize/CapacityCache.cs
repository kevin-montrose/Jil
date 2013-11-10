using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static class CapacityCache
    {
        [Flags]
        enum OptionFlags : int
        {
            None = 0,

            ISO8601 = 1,
            MillisecondsSinceUnixEpoch = 2,
            SecondsSinceUnixEpoch = 4,
            NewtonsoftStyleMillisecondsSinceUnixEpoch = 8,
            IsJSONP = 16,
            ShouldExcludeNulls = 32,
            ShouldIncludeInherited = 64,
            ShouldPrettyPrint = 128
        }

        struct CapacityKey
        {
            public Type ForType { get; private set; }
            public OptionFlags Options { get; private set; }
            private int Hash;

            public static CapacityKey For(Type t, Options opts)
            {
                var hash = t.MetadataToken ^ t.Module.MetadataToken;
                var options = OptionFlags.None;
                switch (opts.UseDateTimeFormat)
                {
                    case DateTimeFormat.ISO8601: options |= OptionFlags.ISO8601; break;
                    case DateTimeFormat.MillisecondsSinceUnixEpoch: options |= OptionFlags.MillisecondsSinceUnixEpoch; break;
                    case DateTimeFormat.SecondsSinceUnixEpoch: options |= OptionFlags.SecondsSinceUnixEpoch; break;
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch: options |= OptionFlags.NewtonsoftStyleMillisecondsSinceUnixEpoch; break;
                    default: throw new Exception("Unexpected DateTimeFormat: " + opts.UseDateTimeFormat);
                }

                if (opts.IsJSONP) options |= OptionFlags.IsJSONP;
                if (opts.ShouldExcludeNulls) options |= OptionFlags.ShouldExcludeNulls;
                if (opts.ShouldIncludeInherited) options |= OptionFlags.ShouldIncludeInherited;
                if (opts.ShouldPrettyPrint) options |= OptionFlags.ShouldPrettyPrint;

                hash ^= (int)options;

                return
                    new CapacityKey
                    {
                        ForType = t,
                        Options = options,
                        Hash = hash
                    };
            }

            public override int GetHashCode()
            {
                return Hash;
            }

            public override bool Equals(object obj)
            {
                var other = (CapacityKey)obj;

                return
                    other.Options == this.Options &&
                    other.ForType == this.ForType;
            }
        }

        const int DefaultCapacity = 16;

        static Hashtable Cache = new Hashtable();

        public static int Get(Type type, Options opts)
        {
            var key = CapacityKey.For(type, opts);

            var ret = Cache[key] ?? DefaultCapacity;

            return (int)ret;
        }

        public static int Get<T>(Options opts)
        {
            return Get(typeof(T), opts);
        }

        public static void Set<T>(Options opts, int rawEstimate)
        {
            var finalEstimate = CalculateFinalEstimate(rawEstimate);

            var key = CapacityKey.For(typeof(T), opts);
            Cache[key] = finalEstimate;
        }

        static int CalculateFinalEstimate(int rawEstimate)
        {
            // convert our to-the-byte estimate into something rather like the 
            //  "double every time up to 8,000" behavior StringBuilder would do
            if (rawEstimate < 8000)
            {
                if (rawEstimate < 16) return 16;
                if (rawEstimate < 32) return 32;
                if (rawEstimate < 64) return 64;
                if (rawEstimate < 128) return 128;
                if (rawEstimate < 256) return 256;
                if (rawEstimate < 512) return 512;
                if (rawEstimate < 1024) return 1024;
                if (rawEstimate < 2048) return 2048;
                if (rawEstimate < 4098) return 4098;

                return 8000;
            }

            var missing = 8000 - (rawEstimate % 8000);

            return rawEstimate + missing;
        }
    }
}
