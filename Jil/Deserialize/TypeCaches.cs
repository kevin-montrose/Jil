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
            try
            {
                Thunk = InlineDeserializerHelper.Build<T>(typeof(NewtonsoftStyleTypeCache<>));
            }
            catch (Sigil.SigilVerificationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
