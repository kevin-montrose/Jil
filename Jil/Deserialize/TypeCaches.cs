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
}
