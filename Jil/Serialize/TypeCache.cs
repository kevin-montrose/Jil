using Sigil;
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
        public static readonly TypeBuilder Serializer;
        public static readonly Action<TextWriter, T> Thunk;

        private static Emit SerializerEmit;
        
        static TypeCache()
        {
            Serializer = SerializerBuilder.Init(typeof(T), out SerializerEmit);

            SerializerBuilder.Build(typeof(T), Serializer, SerializerEmit);

            var type = Serializer.CreateType();

            var serializeMtd = type.GetMethod(SerializerBuilder.SerializeMethod);

            var thunkEmit = Emit<Action<TextWriter, T>>.NewDynamicMethod("_Jil_" + typeof(T).FullName + "_Thunk");

            thunkEmit.Jump(serializeMtd);
            thunkEmit.Return();

            Thunk = thunkEmit.CreateDelegate();
        }
    }
}
