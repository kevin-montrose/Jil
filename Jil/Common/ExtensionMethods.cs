using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    static class ExtensionMethods
    {
        public static MethodInfo ShouldSerializeMethod(this PropertyInfo prop, Type serializingType)
        {
            var mtdName = "ShouldSerialize" + prop.Name;

            var ret =
                serializingType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(m => m.Name == mtdName && m.ReturnType == typeof(bool) && m.GetParameters().Length == 0)
                    .SingleOrDefault();

            return ret;
        }

        public static Type ReturnType(this MemberInfo m)
        {
            var asField = m as FieldInfo;
            var asProp = m as PropertyInfo;

            return
                asField != null ? asField.FieldType : asProp.PropertyType;
        }

        public static bool IsNullableType(this Type t)
        {
            var underlying = GetUnderlyingType(t);

            return underlying != null;
        }

        public static Type GetUnderlyingType(this Type t)
        {
            return Nullable.GetUnderlyingType(t);
        }

        public static bool IsListType(this Type t)
        {
            try
            {
                return
                    (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>)) ||
                    t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
            }
            catch (Exception) { return false; }
        }

        public static Type GetListInterface(this Type t)
        {
            return
                (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>)) ?
                t :
                t.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
        }

        public static Type GetCollectionInterface(this Type t)
        {
            return
                (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)) ?
                t :
                t.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        public static bool IsDictionaryType(this Type t)
        {
            return
                (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        public static Type GetDictionaryInterface(this Type t)
        {
            return
                (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ?
                t :
                t.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        public static bool IsExactlyDictionaryType(this Type t)
        {
            if (!t.IsGenericType) return false;

            var generic = t.GetGenericTypeDefinition();

            return generic == typeof(Dictionary<,>);
        }

        public static void ForEach<T>(this IEnumerable<T> e, Action<T> func)
        {
            foreach (var x in e)
            {
                func(x);
            }
        }

        public static bool IsPrimitiveType(this Type t)
        {
            return
                t == typeof(string) ||
                t == typeof(char) ||
                t == typeof(float) ||
                t == typeof(double) ||
                t == typeof(decimal) ||
                t == typeof(byte) ||
                t == typeof(sbyte) ||
                t == typeof(short) ||
                t == typeof(ushort) ||
                t == typeof(int) ||
                t == typeof(uint) ||
                t == typeof(long) ||
                t == typeof(ulong) ||
                t == typeof(bool) ||
                t == typeof(DateTime) ||
                t == typeof(Guid);
        }

        public static bool IsStringyType(this MemberInfo member)
        {
            var asField = member as FieldInfo;
            if (asField != null)
            {
                return asField.FieldType.IsStringyType();
            }

            var asProperty = member as PropertyInfo;
            if (asProperty != null)
            {
                return asProperty.PropertyType.IsStringyType();
            }

            throw new InvalidOperationException();
        }

        public static bool IsStringyType(this Type t)
        {
            return
                t == typeof(string) ||
                t == typeof(char);
        }

        // From: http://www.ietf.org/rfc/rfc4627.txt?number=4627
        public static string JsonEscape(this string str, bool jsonp)
        {
            var ret = "";
            foreach (var c in str)
            {
                switch (c)
                {
                    case '\\': ret += @"\\"; break;
                    case '"': ret += @"\"""; break;
                    case '\u0000': ret += @"\u0000"; break;
                    case '\u0001': ret += @"\u0001"; break;
                    case '\u0002': ret += @"\u0002"; break;
                    case '\u0003': ret += @"\u0003"; break;
                    case '\u0004': ret += @"\u0004"; break;
                    case '\u0005': ret += @"\u0005"; break;
                    case '\u0006': ret += @"\u0006"; break;
                    case '\u0007': ret += @"\u0007"; break;
                    case '\u0008': ret += @"\u0008"; break;
                    case '\u0009': ret += @"\u0009"; break;
                    case '\u000A': ret += @"\u000A"; break;
                    case '\u000B': ret += @"\u000B"; break;
                    case '\u000C': ret += @"\u000C"; break;
                    case '\u000D': ret += @"\u000D"; break;
                    case '\u000E': ret += @"\u000E"; break;
                    case '\u000F': ret += @"\u000F"; break;
                    case '\u0010': ret += @"\u0010"; break;
                    case '\u0011': ret += @"\u0011"; break;
                    case '\u0012': ret += @"\u0012"; break;
                    case '\u0013': ret += @"\u0013"; break;
                    case '\u0014': ret += @"\u0014"; break;
                    case '\u0015': ret += @"\u0015"; break;
                    case '\u0016': ret += @"\u0016"; break;
                    case '\u0017': ret += @"\u0017"; break;
                    case '\u0018': ret += @"\u0018"; break;
                    case '\u0019': ret += @"\u0019"; break;
                    case '\u001A': ret += @"\u001A"; break;
                    case '\u001B': ret += @"\u001B"; break;
                    case '\u001C': ret += @"\u001C"; break;
                    case '\u001D': ret += @"\u001D"; break;
                    case '\u001E': ret += @"\u001E"; break;
                    case '\u001F': ret += @"\u001F"; break;

                    case '\u2028':
                        if(jsonp)
                        {
                            ret += @"\u2028";
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    case '\u2029':
                        if(jsonp)
                        {
                            ret += @"\u2029";
                        }
                        else
                        {
                            goto default;
                        }
                        break;

                    default: ret += c; break;
                }
            }

            return ret;
        }

        public static object DefaultValue(this Type t)
        {
            if (!t.IsValueType) return null;

            return Activator.CreateInstance(t);
        }

        public static bool IsIntegerNumberType(this Type t)
        {
            return
                t == typeof(byte) ||
                t == typeof(sbyte) ||
                t == typeof(short) ||
                t == typeof(ushort) ||
                t == typeof(int) ||
                t == typeof(uint) ||
                t == typeof(long) ||
                t == typeof(ulong);
        }

        public static bool IsFloatingPointNumberType(this Type t)
        {
            return
                t == typeof(float) ||
                t == typeof(double) ||
                t == typeof(decimal);
        }

        public static List<Type> InvolvedTypes(this Type t)
        {
            var ret = new List<Type>();

            var pending = new Stack<Type>();
            pending.Push(t);

            while (pending.Count > 0)
            {
                var cur = pending.Pop();
                if (!ret.Contains(cur))
                {
                    ret.Add(cur);
                }
                else
                {
                    continue;
                }

                if (cur.IsNullableType())
                {
                    var inner = Nullable.GetUnderlyingType(cur);
                    pending.Push(inner);
                    continue;
                }

                if (cur.IsListType())
                {
                    var elem = cur.GetListInterface().GetGenericArguments()[0];
                    pending.Push(elem);
                    continue;
                }

                if (cur.IsDictionaryType())
                {
                    var key = cur.GetDictionaryInterface().GetGenericArguments()[0];
                    var val = cur.GetDictionaryInterface().GetGenericArguments()[1];

                    pending.Push(key);
                    pending.Push(val);

                    continue;
                }

                cur.GetFields().ForEach(f => pending.Push(f.FieldType));
                cur.GetProperties().Where(p => p.GetMethod != null && p.GetMethod.GetParameters().Length == 0).ForEach(p => pending.Push(p.PropertyType));
            }

            return ret;
        }
    }
}
