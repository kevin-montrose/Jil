using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static class TypeCache<T>
    {
        public static TypeBuilder Serializer;

        private static Emit Emit;
        
        static TypeCache()
        {
            Serializer = SerializerBuilder.Init(typeof(T), out Emit);

            SerializerBuilder.Build(typeof(T), Serializer, Emit);
        }
    }
}
