using System;
using System.Reflection;
using System.Reflection.Emit;

namespace JilTests
{
    internal static class TypeHelpers
    {
        public static ConstructorInfo AssertNotNull(this ConstructorInfo ctor, string name)
        {
            if (ctor == null) throw new InvalidOperationException("Failed to resolve constructor: " + name);
            return ctor;
        }
        public static MethodInfo AssertNotNull(this MethodInfo method, string name)
        {
            if (method == null) throw new InvalidOperationException("Failed to resolve method: " + name);
            return method;
        }
#if COREFX

        public static object _GetRawConstantValue(this FieldInfo field)
        {
            return field.GetValue(null);
        }
        public static bool _IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
        public static bool _IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
        public static bool _IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
        public static bool _IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }
        public static bool _IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }
        public static bool _IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }
        public static bool _IsSealed(this Type type)
        {
            return type.GetTypeInfo().IsSealed;
        }
        public static Type _CreateType(this TypeBuilder type)
        {
            return type.CreateTypeInfo().AsType();
        }

        public static bool _IsPublic(this Type type)
        {
            return type.GetTypeInfo().IsPublic;
        }
        public static Type _BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }
        public static MethodInfo _GetPublicStaticMethod(this Type type, string name, Type[] args)
        {
            var method = type.GetRuntimeMethod(name, args);
            return (method == null || !method.IsPublic || !method.IsStatic) ? null : method;
        }
        public static ConstructorInfo _GetPublicOrPrivateConstructor(this Type onType, params Type[] parameterTypes)
        {
            if (parameterTypes == null) parameterTypes = Type.EmptyTypes;
            foreach(var ctor in onType.GetTypeInfo().DeclaredConstructors)
            {
                var args = ctor.GetParameters();
                if (args.Length != parameterTypes.Length) continue;
                bool match = true;
                for(int i = 0; i < args.Length; i++)
                {
                    if(args[i].ParameterType != parameterTypes[i])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return ctor;
            }
            return null;
        }
        public static bool _IsDefined(this Type type, Type attributeType)
        {
            return type.GetTypeInfo().IsDefined(attributeType);
        }
#else
        public static object _GetRawConstantValue(this FieldInfo field)
        {
            return field.GetRawConstantValue();
        }
        public static bool _IsValueType(this Type type)
        {
            return type.IsValueType;
        }
        public static bool _IsEnum(this Type type)
        {
            return type.IsEnum;
        }
        public static bool _IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }
        public static bool _IsInterface(this Type type)
        {
            return type.IsInterface;
        }
        public static bool _IsAbstract(this Type type)
        {
            return type.IsAbstract;
        }
        public static bool _IsPrimitive(this Type type)
        {
            return type.IsPrimitive;
        }
        public static bool _IsSealed(this Type type)
        {
            return type.IsSealed;
        }
        public static Type _CreateType(this TypeBuilder type)
        {
            return type.CreateType();
        }

        public static bool _IsPublic(this Type type)
        {
            return type.IsPublic;
        }
        public static Type _BaseType(this Type type)
        {
            return type.BaseType;
        }
        public static MethodInfo _GetPublicStaticMethod(this Type type, string name, Type[] args)
        {
            return type.GetMethod(name, BindingFlags.Public | BindingFlags.Static, null, args, null);
        }
        public static ConstructorInfo _GetPublicOrPrivateConstructor(this Type onType, params Type[] parameterTypes)
        {
            return onType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes ?? Type.EmptyTypes, null);
        }
        public static bool _IsDefined(this Type type, Type attributeType)
        {
            return Attribute.IsDefined(type, attributeType);
        }
#endif
    }
}
