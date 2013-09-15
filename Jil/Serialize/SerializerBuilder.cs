using Sigil.NonGeneric;
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
    static class SerializerBuilder
    {
        private static AssemblyBuilder Assembly;
        private static ModuleBuilder Module;

        private const string SerializeMethod = "Serialize";

        static SerializerBuilder()
        {
            Assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("_Jil_DynamicAssembly"), AssemblyBuilderAccess.Run);
            Module = Assembly.DefineDynamicModule("_Jil_DynamicModule");
        }

        public static TypeBuilder Init(Type forType, out Emit emit)
        {
            lock (Module)
            {
                var ret = Module.DefineType("_Jil_" + forType.FullName, TypeAttributes.Sealed | TypeAttributes.Class);

                emit = Emit.BuildStaticMethod(typeof(void), new[] { forType, typeof(TextWriter) }, ret, SerializeMethod, MethodAttributes.Public);

                return ret;
            }
        }

        public static TypeBuilder Build(Type forType, TypeBuilder intoType, Emit emit)
        {
            throw new NotImplementedException();
        }
    }
}
