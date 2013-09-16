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
            Serializer = SerializerBuilder.Init(out SerializerEmit);

            // Build the *actual* serializer method
            var serializeMethod = SerializerBuilder.Build(typeof(T), Serializer, SerializerEmit);

            // Build the thunk we'll call to actually serialize
            var type = Serializer.CreateType();

            var finalSerailizeMethod = type.GetMethod(serializeMethod.Name);

            var thunkEmit = Emit<Action<TextWriter, T>>.NewDynamicMethod("_Jil_" + typeof(T).FullName + "_Thunk");

            thunkEmit.Jump(finalSerailizeMethod);
            thunkEmit.Return();

            Thunk = thunkEmit.CreateDelegate();
        }
    }
}
