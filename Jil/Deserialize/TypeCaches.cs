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
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }

    static class NewtonsoftStyleNoHashingTypeCache<T>
    {
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(NewtonsoftStyleNoHashingTypeCache<>), DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, allowHashing: false, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }

    static class MillisecondStyleTypeCache<T>
    {
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(MillisecondStyleTypeCache<>), DateTimeFormat.MillisecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }

    static class MillisecondStyleNoHashingTypeCache<T>
    {
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(MillisecondStyleNoHashingTypeCache<>), DateTimeFormat.MillisecondsSinceUnixEpoch, allowHashing: false, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }

    static class SecondStyleTypeCache<T>
    {
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(SecondStyleTypeCache<>), DateTimeFormat.SecondsSinceUnixEpoch, allowHashing: true, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }

    static class SecondStyleNoHashingTypeCache<T>
    {
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;
        
        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(SecondStyleNoHashingTypeCache<>), DateTimeFormat.SecondsSinceUnixEpoch, allowHashing: false, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }

    static class ISO8601StyleTypeCache<T>
    {
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;
        
        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(ISO8601StyleTypeCache<>), DateTimeFormat.ISO8601, allowHashing: true, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }

    static class ISO8601StyleNoHashingTypeCache<T>
    {
        static readonly object InitLock = new object();
        static Func<TextReader, T> Thunk;
        public static Exception ExceptionDuringBuild;
        
        public static Func<TextReader, T> Get()
        {
            if (Thunk != null) return Thunk;

            lock (InitLock)
            {
                if (Thunk != null) return Thunk;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(ISO8601StyleNoHashingTypeCache<>), DateTimeFormat.ISO8601, allowHashing: false, exceptionDuringBuild: out ExceptionDuringBuild);

                return Thunk;
            }
        }
    }
}
