using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;
using Sigil.NonGeneric;

namespace Jil.Deserialize
{
    static class InterfaceImplementation<Interface>
    {
        public static readonly Type Proxy;

        static InterfaceImplementation()
        {
            var iType = typeof(Interface);

            var typeBuilder = AssemblyBuilderContainer.ModBuilder.DefineType(iType.Name + "Impl", TypeAttributes.Class, typeof(object), new[] { iType });

            foreach (var prop in iType.GetProperties())
            {
                var propType = prop.ReturnType();

                var propBuilder = typeBuilder.DefineProperty(prop.Name, prop.Attributes, propType, Type.EmptyTypes);

                var backingField = typeBuilder.DefineField("_" + prop.Name, propType, FieldAttributes.Private);

                var iGetter = prop.GetMethod;
                MethodBuilder getter;
                {
                    var accessor = iGetter != null ? iGetter.Attributes : MethodAttributes.Private;
                    var name = iGetter != null ? iGetter.Name : "get_" + prop.Name;

                    accessor &= ~MethodAttributes.Abstract;

                    var emit = Sigil.NonGeneric.Emit.BuildInstanceMethod(propType, Type.EmptyTypes, typeBuilder, name, accessor);
                    emit.LoadArgument(0);
                    emit.LoadField(backingField);
                    emit.Return();

                    getter = emit.CreateMethod();
                }
                propBuilder.SetGetMethod(getter);

                var iSetter = prop.SetMethod;
                MethodBuilder setter;
                {
                    var accessor = iSetter != null ? iSetter.Attributes : MethodAttributes.Private;
                    var name = iSetter != null ? iSetter.Name : "get_" + prop.Name;

                    accessor &= ~MethodAttributes.Abstract;

                    var emit = Sigil.NonGeneric.Emit.BuildInstanceMethod(typeof(void), new [] { propType }, typeBuilder, name, accessor);
                    emit.LoadArgument(0);
                    emit.LoadArgument(1);
                    emit.StoreField(backingField);
                    emit.Return();

                    setter = emit.CreateMethod();
                }
                propBuilder.SetSetMethod(setter);
            }

            Proxy = typeBuilder.CreateType();
        }
    }

    static class AssemblyBuilderContainer
    {
        public static readonly ModuleBuilder ModBuilder;

        static readonly AssemblyBuilder AsmBuilder;

        static AssemblyBuilderContainer()
        {
            AsmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("JilInterfaceTypeProxies"), AssemblyBuilderAccess.Run);
            ModBuilder = AsmBuilder.DefineDynamicModule("InterfaceProxies");
        }
    }
}
