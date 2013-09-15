using Jil.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    public sealed class JSON
    {
        public static void Serialize<T>(T data, TextWriter output, Options options = null)
        {
            options = options ?? Options.None;

            if (options != Options.None) throw new NotImplementedException();

            TypeCache<T>.Thunk(output, data);
        }
    }
}
