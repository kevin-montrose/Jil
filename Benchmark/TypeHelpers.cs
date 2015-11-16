
using System;
using System.Reflection;
namespace Benchmark
{
    public static class TypeHelpers
    {
#if COREFX
        public static Delegate CreateDelegate(Type type, MethodInfo method)
        {
            return method.CreateDelegate(type);
        }
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }
        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }
        public static bool IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }
#else
        public static Delegate CreateDelegate(Type type, MethodInfo method)
        {
            return Delegate.CreateDelegate(type, method);
        }
        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }
        public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive;
        }
        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }
        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }
        public static bool IsAbstract(this Type type)
        {
            return type.IsAbstract;
        }
#endif
    }
}