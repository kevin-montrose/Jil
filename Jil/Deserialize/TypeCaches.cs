using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    interface IDeserializeOptions
    {
        DateTimeFormat DateFormat { get; }
    }

    static class TypeCache<TOptions, T>
        where TOptions : IDeserializeOptions, new()
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

                var options = new TOptions();

                Thunk = InlineDeserializerHelper.Build<T>(typeof(TOptions), options.DateFormat, exceptionDuringBuild: out ExceptionDuringBuild);
            }
        }
    }

    class NewtonsoftStyle : IDeserializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch; } }
    }

    class MillisecondStyle : IDeserializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.MillisecondsSinceUnixEpoch; } }
    }

    class SecondStyle : IDeserializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.SecondsSinceUnixEpoch; } }
    }

    class ISO8601Style : IDeserializeOptions
    {
        public DateTimeFormat DateFormat { get { return DateTimeFormat.ISO8601; } }
    }
}