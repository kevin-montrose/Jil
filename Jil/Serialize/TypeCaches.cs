using Sigil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    delegate string StringThunkDelegate<T>(T data, int depth);

    static class NewtonsoftStyleTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStyleJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStyleExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStyleJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStyleInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStyleExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStyleExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStyleExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStyleExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(NewtonsoftStylePrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class MillisecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(MillisecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(MillisecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsTypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsJSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsJSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsInheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class SecondsPrettyPrintExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(SecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(SecondsPrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.SecondsSinceUnixEpoch, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601TypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601TypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601TypeCache<>), pretty: false, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601JSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601JSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601JSONPTypeCache<>), pretty: false, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<>), pretty: true, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601ExcludeNullsJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601ExcludeNullsJSONPInheritedTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintJSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintJSONPInheritedTypeCache<>), pretty: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintExcludeNullsInheritedTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601ExcludeNullsInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601ExcludeNullsInheritedTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintInheritedTypeCache<>), pretty: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601JSONPInheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601JSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601JSONPInheritedTypeCache<>), pretty: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601InheritedTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601InheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601InheritedTypeCache<>), pretty: false, dateFormat: DateTimeFormat.ISO8601, includeInherited: true, excludeNulls: false, jsonp: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601ExcludeNullsJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601ExcludeNullsJSONPTypeCache<>), pretty: false, excludeNulls: true, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintJSONPTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintJSONPTypeCache<>), pretty: true, excludeNulls: false, jsonp: true, dateFormat: DateTimeFormat.ISO8601, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintTypeCache<>), pretty: true, excludeNulls: false, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601ExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601ExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601ExcludeNullsTypeCache<>), pretty: false, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }

    static class ISO8601PrettyPrintExcludeNullsTypeCache<T>
    {
        static readonly object ThunkInitLock = new object();
        static volatile bool ThunkBeingBuilt = false;
        public static volatile Action<TextWriter, T, int> Thunk;
        public static Exception ThunkExceptionDuringBuild;

        static readonly object StringThunkInitLock = new object();
        static volatile bool StringThunkBeingBuilt = false;
        public static volatile StringThunkDelegate<T> StringThunk;
        public static Exception StringThunkExceptionDuringBuild;

        public static Action<TextWriter, T, int> Get()
        {
            Load();
            return Thunk;
        }

        public static void Load()
        {
            if (Thunk != null) return;

            lock (ThunkInitLock)
            {
                if (Thunk != null || ThunkBeingBuilt) return;
                ThunkBeingBuilt = true;

                Thunk = InlineSerializerHelper.Build<T>(typeof(ISO8601PrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out ThunkExceptionDuringBuild);
            }
        }

        public static StringThunkDelegate<T> GetToString()
        {
            LoadToString();
            return StringThunk;
        }

        public static void LoadToString()
        {
            if (StringThunk != null) return;

            lock (StringThunkInitLock)
            {
                if (StringThunk != null || StringThunkBeingBuilt) return;
                StringThunkBeingBuilt = true;

                StringThunk = InlineSerializerHelper.BuildToString<T>(typeof(ISO8601PrettyPrintExcludeNullsTypeCache<>), pretty: true, excludeNulls: true, dateFormat: DateTimeFormat.ISO8601, jsonp: false, includeInherited: false, exceptionDuringBuild: out StringThunkExceptionDuringBuild);
            }
        }
    }
}