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

            var typeBuilder = 
                AssemblyBuilderContainer.ModBuilder.DefineType(
                    iType.Name + "Impl", 
                    TypeAttributes.Class | TypeAttributes.Sealed, 
                    typeof(object), 
                    new[] { iType }
                );

            var allMembers = iType.GetAllInterfaceMembers();

            foreach (var prop in allMembers.OfType<PropertyInfo>())
            {
                var propType = prop.ReturnType();

                var propBuilder = typeBuilder.DefineProperty(prop.Name, prop.Attributes, propType, Type.EmptyTypes);

                var iGetter = prop.GetMethod;
                var iSetter = prop.SetMethod;

                var backingField =
                    iSetter != null ? 
                        typeBuilder.DefineField("_" + prop.Name, propType, FieldAttributes.Private) : 
                        null;

                if (iGetter != null)
                {
                    var accessor = iGetter.Attributes;
                    var name = iGetter.Name;

                    accessor &= ~MethodAttributes.Abstract;

                    var emit = Sigil.NonGeneric.Emit.BuildInstanceMethod(propType, Type.EmptyTypes, typeBuilder, name, accessor);

                    if (iSetter != null)
                    {
                        // property could be populated, so we need a real implementation
                        emit.LoadArgument(0);
                        emit.LoadField(backingField);
                        emit.Return();
                    }
                    else
                    {
                        // property can never be set (no setter), so this is a no-op
                        if (propType.IsValueType)
                        {
                            using (var loc = emit.DeclareLocal(propType))
                            {
                                emit.LoadLocalAddress(loc);
                                emit.InitializeObject(propType);
                                emit.LoadLocal(loc);
                                emit.Return();
                            }
                        }
                        else
                        {
                            emit.LoadNull();
                            emit.Return();
                        }
                    }

                    var getter = emit.CreateMethod();
                    propBuilder.SetGetMethod(getter);
                }


                if(iSetter != null)
                {
                    var accessor = iSetter.Attributes;
                    var name = iSetter.Name;

                    accessor &= ~MethodAttributes.Abstract;

                    var emit = Sigil.NonGeneric.Emit.BuildInstanceMethod(typeof(void), new [] { propType }, typeBuilder, name, accessor);

                    if (iGetter != null)
                    {
                        // property can be populated, so setter needs a real impl
                        emit.LoadArgument(0);
                        emit.LoadArgument(1);
                        emit.StoreField(backingField);
                        emit.Return();
                    }
                    else
                    {
                        // property can never be read (no getter), so this is a no-op
                        emit.Return();
                    }

                    var setter = emit.CreateMethod();
                    propBuilder.SetSetMethod(setter);
                }
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
