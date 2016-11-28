using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace JilTests
{
    public static class ExtensionMethods
    {
        public static MethodInfo GetMethod(this Type type, string name, BindingFlags flags, Type[] parameterTypes)
        {
            var info = type.GetTypeInfo();
            var mtds = info.GetMethods(flags).Where(m => m.Name == name);

            foreach (var mtd in mtds)
            {
                var ps = mtd.GetParameters();
                if (ps.Length != parameterTypes.Length) continue;

                var allMatch = true;
                for (var i = 0; i < ps.Length; i++)
                {
                    var p = ps[i].ParameterType;
                    var pt = parameterTypes[i];
                    if (p != pt)
                    {
                        allMatch = false;
                    }
                }

                if (allMatch)
                {
                    return mtd;
                }
            }

            return null;
        }

        public static ConstructorInfo GetConstructor(this Type type, BindingFlags flags, Type[] parameterTypes)
        {
            var info = type.GetTypeInfo();
            var cons = info.GetConstructors(flags);
            foreach (var con in cons)
            {
                var ps = con.GetParameters();
                if (ps.Length != parameterTypes.Length) continue;

                var allMatch = true;

                for (var i = 0; i < ps.Length; i++)
                {
                    var p = ps[i].ParameterType;
                    var pt = parameterTypes[i];
                    if (pt != p)
                    {
                        allMatch = false;
                    }
                }

                if (allMatch) return con;
            }

            return null;
        }
        public static bool IsValueType(this Type type)
        {
            var info = type.GetTypeInfo();

            return info.IsValueType;
        }

        public static bool IsGenericType(this Type type)
        {
            var info = type.GetTypeInfo();

            return info.IsGenericType;
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            var info = type.GetTypeInfo();

            return info.GetProperties();
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            var info = type.GetTypeInfo();

            return info.GetGenericArguments();
        }

        public static bool IsAssignableFrom(this Type a, Type b)
        {
            var aInfo = a.GetTypeInfo();
            var bInfo = b.GetTypeInfo();

            return aInfo.IsAssignableFrom(bInfo);
        }
    }
}
