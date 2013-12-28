using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    static class ExtensionMethods
    {
        public static bool IsConstant(this MemberInfo member)
        {
            var asField = member as FieldInfo;
            if (asField != null) return asField.IsConstant();

            var asProp = member as PropertyInfo;
            if (asProp != null) return asProp.IsConstant();

            throw new Exception("Expected member to be a FieldInfo or PropetyInfo, found: " + member);
        }

        public static bool IsConstant(this FieldInfo field)
        {
            return field.IsLiteral && field.ReturnType().IsPropagatableType();
        }

        static readonly IEnumerable<OpCode> ReadOnlyOpCodes =
            new[]
            {   
                OpCodes.Ldc_I4,     // Constant integer numbers
                OpCodes.Ldc_I4_0,
                OpCodes.Ldc_I4_1,
                OpCodes.Ldc_I4_2,
                OpCodes.Ldc_I4_3,
                OpCodes.Ldc_I4_4,
                OpCodes.Ldc_I4_5,
                OpCodes.Ldc_I4_6,
                OpCodes.Ldc_I4_7,
                OpCodes.Ldc_I4_8,
                OpCodes.Ldc_I4_M1,
                OpCodes.Ldc_I4_S,
                OpCodes.Ldc_I8,

                OpCodes.Ldc_R4,     // Constant floating point numbers
                OpCodes.Ldc_R8,

                OpCodes.Ldstr,      // Constant strings

                OpCodes.Conv_I,         // Conversion operators
                OpCodes.Conv_I1,
                OpCodes.Conv_I2,
                OpCodes.Conv_I4,
                OpCodes.Conv_I8,
                OpCodes.Conv_Ovf_I,
                OpCodes.Conv_Ovf_I_Un,
                OpCodes.Conv_Ovf_I1,
                OpCodes.Conv_Ovf_I1_Un,
                OpCodes.Conv_Ovf_I2,
                OpCodes.Conv_Ovf_I2_Un,
                OpCodes.Conv_Ovf_I4,
                OpCodes.Conv_Ovf_I4_Un,
                OpCodes.Conv_Ovf_I8,
                OpCodes.Conv_Ovf_I8_Un,
                OpCodes.Conv_R4,
                OpCodes.Conv_R8,

                OpCodes.Ldnull      // always null
            };
        public static bool IsConstant(this PropertyInfo prop)
        {
            var instrs = Jil.Serialize.Utils.Decompile(prop.GetMethod);
            if (instrs == null) return false;

            // anything control flow-y, call-y, load-y, etc. means the property isn't constant
            var hasNonConstantInstructions = instrs.Any(a => !ReadOnlyOpCodes.Contains(a.Item1) && a.Item1 != OpCodes.Ret);
            if (hasNonConstantInstructions) return false;

            // if *two* constants are in play (somehow) we can't tell which one to propagate so break it
            var numberOfConstants = instrs.Count(a => a.Item2.HasValue || a.Item3.HasValue || a.Item4.HasValue);
            if (numberOfConstants > 1) return false;

            return true;
        }

        public static bool IsPropagatableType(this Type t)
        {
            return
                t == typeof(string) ||
                t == typeof(char) ||
                t == typeof(float) ||
                t == typeof(double) ||
                t == typeof(byte) ||
                t == typeof(sbyte) ||
                t == typeof(short) ||
                t == typeof(ushort) ||
                t == typeof(int) ||
                t == typeof(uint) ||
                t == typeof(long) ||
                t == typeof(ulong) ||
                t == typeof(bool) ||
                t.IsEnum;
        }

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

        public static HashSet<Type> FindRecursiveTypes(this Type forType)
        {
            var alreadySeen = new HashSet<Type>();
            var ret = new HashSet<Type>();

            var pending = new List<Type>();
            pending.Add(forType);

            while (pending.Count > 0)
            {
                var curType = pending[0];
                pending.RemoveAt(0);

                if (curType.IsPrimitiveType()) continue;

                if (curType.IsListType())
                {
                    var listI = curType.GetListInterface();
                    var valType = listI.GetGenericArguments()[0];
                    pending.Add(valType);
                    continue;
                }

                if (curType.IsDictionaryType())
                {
                    var dictI = curType.GetDictionaryInterface();
                    var valType = dictI.GetGenericArguments()[1];
                    pending.Add(valType);
                    continue;
                }

                if (alreadySeen.Contains(curType))
                {
                    ret.Add(curType);
                    continue;
                }

                alreadySeen.Add(curType);

                foreach (var field in curType.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    pending.Add(field.FieldType);
                }

                foreach (var prop in curType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetMethod != null))
                {
                    pending.Add(prop.PropertyType);
                }
            }

            return ret;
        }
    }
}
