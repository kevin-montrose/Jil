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
    static class TypeCache<T>
    {
        public static Emit<Action<TextWriter, T>> SerializerEmit;
        public static readonly Action<TextWriter, T> Thunk;

        private static readonly TypeBuilder Serializer;
        
        static TypeCache()
        {
            // Setup a bunch of proxies for recursing
            SerializerEmit = SerializerBuilder.Init<T>();

            // Build the *actual* serializer method
            Thunk = SerializerBuilder.Build(SerializerEmit);
        }
    }
}
