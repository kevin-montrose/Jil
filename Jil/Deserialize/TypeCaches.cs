using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static class NewtonsoftStyleTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static NewtonsoftStyleTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch);
        }
    }

    static class MillisecondStyleTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static MillisecondStyleTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(MillisecondStyleTypeCache<>), DateTimeFormat.MillisecondsSinceUnixEpoch);
        }
    }

    static class SecondStyleTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static SecondStyleTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(SecondStyleTypeCache<>), DateTimeFormat.SecondsSinceUnixEpoch);
        }
    }

    static class ISO8601StyleTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static ISO8601StyleTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(ISO8601StyleTypeCache<>), DateTimeFormat.ISO8601);
        }
    }
}
