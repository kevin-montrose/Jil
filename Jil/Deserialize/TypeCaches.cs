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
        static volatile bool BeingBuilt = false;

        public static volatile Func<TextReader, int, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, int, T> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (InitLock)
            {
                if (Thunk != null || BeingBuilt) return;
                BeingBuilt = true;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, exceptionDuringBuild: out ExceptionDuringBuild);
            }
        }
    }

    static class MillisecondStyleTypeCache<T>
    {
        static readonly object InitLock = new object();
        static volatile bool BeingBuilt = false;

        public static volatile Func<TextReader, int, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, int, T> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (InitLock)
            {
                if (Thunk != null || BeingBuilt) return;
                BeingBuilt = true;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(MillisecondStyleTypeCache<>), DateTimeFormat.MillisecondsSinceUnixEpoch, exceptionDuringBuild: out ExceptionDuringBuild);
            }
        }
    }

    static class SecondStyleTypeCache<T>
    {
        static readonly object InitLock = new object();
        static volatile bool BeingBuilt = false;

        public static volatile Func<TextReader, int, T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, int, T> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (InitLock)
            {
                if (Thunk != null || BeingBuilt) return;
                BeingBuilt = true;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(SecondStyleTypeCache<>), DateTimeFormat.SecondsSinceUnixEpoch, exceptionDuringBuild: out ExceptionDuringBuild);
            }
        }
    }

    static class ISO8601StyleTypeCache<T>
    {
        static readonly object InitLock = new object();
        static volatile bool BeingBuilt = false;

        public static volatile Func<TextReader, int,  T> Thunk;
        public static Exception ExceptionDuringBuild;

        public static Func<TextReader, int, T> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (InitLock)
            {
                if (Thunk != null || BeingBuilt) return;
                BeingBuilt = true;

                Thunk = InlineDeserializerHelper.Build<T>(typeof(ISO8601StyleTypeCache<>), DateTimeFormat.ISO8601, exceptionDuringBuild: out ExceptionDuringBuild);
            }
        }
    }
}
