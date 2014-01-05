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
            Thunk = InlineDeserializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true);
        }
    }

    static class NewtonsoftStyleNoHashingTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static NewtonsoftStyleNoHashingTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(NewtonsoftStyleNoHashingTypeCache<>), DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: false);
        }
    }

    static class MillisecondStyleTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static MillisecondStyleTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(MillisecondStyleTypeCache<>), DateTimeFormat.MillisecondsSinceUnixEpoch, allowHashing: true);
        }
    }

    static class MillisecondStyleNoHashingTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static MillisecondStyleNoHashingTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(MillisecondStyleNoHashingTypeCache<>), DateTimeFormat.MillisecondsSinceUnixEpoch, allowHashing: false);
        }
    }

    static class SecondStyleTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static SecondStyleTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(SecondStyleTypeCache<>), DateTimeFormat.SecondsSinceUnixEpoch, allowHashing: true);
        }
    }

    static class SecondStyleNoHashingTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static SecondStyleNoHashingTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(SecondStyleNoHashingTypeCache<>), DateTimeFormat.SecondsSinceUnixEpoch, allowHashing: false);
        }
    }

    static class ISO8601StyleTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static ISO8601StyleTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(ISO8601StyleTypeCache<>), DateTimeFormat.ISO8601, allowHashing: true);
        }
    }

    static class ISO8601StyleNoHashingTypeCache<T>
    {
        public static readonly Func<TextReader, int, T> Thunk;

        static ISO8601StyleNoHashingTypeCache()
        {
            Thunk = InlineDeserializerHelper.Build<T>(typeof(ISO8601StyleNoHashingTypeCache<>), DateTimeFormat.ISO8601, allowHashing: false);
        }
    }
}
