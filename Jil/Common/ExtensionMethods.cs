using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    static class ExtensionMethods
    {
        public static string GetSerializationName(this MemberInfo member)
        {
            var attrs = member.GetCustomAttribute<System.Runtime.Serialization.DataMemberAttribute>();

            if (attrs == null || string.IsNullOrEmpty(attrs.Name)) return member.Name;

            return attrs.Name;
        }

        public static bool IsLoadArgumentOpCode(this OpCode op)
        {
            return
                op == OpCodes.Ldarg ||
                op == OpCodes.Ldarg_0 ||
                op == OpCodes.Ldarg_1 ||
                op == OpCodes.Ldarg_2 ||
                op == OpCodes.Ldarg_3 ||
                op == OpCodes.Ldarg_S;
        }

        /// <summary>
        /// HACK: This is a best effort attempt to divine if a type is anonymous based on the language spec.
        /// 
        /// Reference section 7.6.10.6 of the C# language spec as of 2012/11/19
        /// 
        /// It checks:
        ///     - is a class
        ///     - descends directly from object
        ///     - has [CompilerGenerated]
        ///     - has a single constructor
        ///     - that constructor takes exactly the same parameters as its public properties
        ///     - all public properties are not writable
        ///     - has a private field for every public property
        ///     - overrides Equals(object)
        ///     - overrides GetHashCode()
        /// </summary>
        public static bool IsAnonymouseClass(this Type type) // don't fix the typo, it's fitting.
        {
            if (type.IsValueType) return false;
            if (type.BaseType != typeof(object)) return false;
            if (!Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute))) return false;

            var allCons = type.GetConstructors();
            if (allCons.Length != 1) return false;

            var cons = allCons[0];
            if (!cons.IsPublic) return false;

            var props = type.GetProperties();
            if (props.Any(p => p.CanWrite)) return false;

            var propTypes = props.Select(t => t.PropertyType).ToList();

            foreach (var param in cons.GetParameters())
            {
                if (!propTypes.Contains(param.ParameterType)) return false;

                propTypes.Remove(param.ParameterType);
            }

            if (propTypes.Count != 0) return false;

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            if (fields.Any(f => !f.IsPrivate)) return false;

            propTypes = props.Select(t => t.PropertyType).ToList();
            foreach (var field in fields)
            {
                if (!propTypes.Contains(field.FieldType)) return false;

                propTypes.Remove(field.FieldType);
            }

            if (propTypes.Count != 0) return false;

            var equals = type.GetMethod("Equals", new Type[] { typeof(object) });
            var hashCode = type.GetMethod("GetHashCode", new Type[0]);

            if (!equals.IsOverride() || !hashCode.IsOverride()) return false;

            return true;
        }

        public static bool IsOverride(this MethodInfo method)
        {
            return method.GetBaseDefinition() != method;
        }

        public static bool IsUserDefinedType(this Type type)
        {
            return !type.IsListType() && !type.IsDictionaryType() && !type.IsEnum && !type.IsPrimitiveType();
        }

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
            try
            {
                return field.IsLiteral && field.ReturnType().IsPropagatableType() && field.GetRawConstantValue() != null;
            }
            catch (Exception)
            {
                // Something went sideways, bail
                return false;
            }
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

        static readonly IEnumerable<OpCode> ConstantLoadOpCodes = 
            new[]
            {
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

                OpCodes.Ldnull
            };

        public static bool IsConstant(this PropertyInfo prop)
        {
            var getMtd = prop.GetMethod;

            // virtual methods can't be proven constant, who knows what overrides are out there
            if (getMtd == null || getMtd.IsVirtual) return false;

            var instrs = Utils.Decompile(getMtd);
            if (instrs == null) return false;

            // anything control flow-y, call-y, load-y, etc. means the property isn't constant
            var hasNonConstantInstructions = instrs.Any(a => !ReadOnlyOpCodes.Contains(a.Item1) && a.Item1 != OpCodes.Ret);
            if (hasNonConstantInstructions) return false;

            // if *two* constants are in play (somehow) we can't tell which one to propagate so break it
            var numberOfConstants = instrs.Count(a => ConstantLoadOpCodes.Contains(a.Item1) || a.Item2.HasValue || a.Item3.HasValue || a.Item4.HasValue);
            if (numberOfConstants != 1) return false;

            return true;
        }

        public static string GetConstantJSONStringEquivalent(this MemberInfo member, bool jsonp)
        {
            var asField = member as FieldInfo;
            if (asField != null) return asField.GetConstantJSONStringEquivalent(jsonp);

            var asProp = member as PropertyInfo;
            if (asProp != null) return asProp.GetConstantJSONStringEquivalent(jsonp);

            throw new Exception("Expected member to be a FieldInfo or PropetyInfo, found: " + member);
        }

        public static string GetConstantJSONStringEquivalent(this PropertyInfo prop, bool jsonp)
        {
            var instrs = Utils.Decompile(prop.GetMethod);

            var constInstr = instrs.Single(o => ConstantLoadOpCodes.Contains(o.Item1) || o.Item2.HasValue || o.Item3.HasValue || o.Item4.HasValue);

            object equivObj = null;

            if (ConstantLoadOpCodes.Contains(constInstr.Item1))
            {
                if (constInstr.Item1 == OpCodes.Ldnull)
                {
                    equivObj = null;
                }
                else
                {
                    if (constInstr.Item1 == OpCodes.Ldc_I4_0) equivObj = 0;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_1) equivObj = 1;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_2) equivObj = 2;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_3) equivObj = 3;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_4) equivObj = 4;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_5) equivObj = 5;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_6) equivObj = 6;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_7) equivObj = 7;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_8) equivObj = 8;
                    if (constInstr.Item1 == OpCodes.Ldc_I4_M1) equivObj = -1;

                    if (equivObj == null) throw new Exception("Couldn't determine constant being loaded");
                }
            }
            else
            {
                if (constInstr.Item1 == OpCodes.Ldstr)
                {
                    var handle = constInstr.Item2.Value;
                    equivObj = prop.Module.ResolveString(handle);
                }

                if (constInstr.Item1 == OpCodes.Ldc_R8)
                {
                    equivObj = constInstr.Item4.Value;
                }

                if (constInstr.Item1 == OpCodes.Ldc_R4)
                {
                    equivObj = constInstr.Item4.Value;
                }

                if (constInstr.Item1 == OpCodes.Ldc_I4)
                {
                    equivObj = constInstr.Item2.Value;
                }

                if (constInstr.Item1 == OpCodes.Ldc_I4_S)
                {
                    equivObj = (sbyte)constInstr.Item2.Value;
                }

                if (constInstr.Item1 == OpCodes.Ldc_I8)
                {
                    equivObj = constInstr.Item3.Value;
                }
            }

            if (equivObj != null && equivObj.GetType() != prop.ReturnType())
            {
                equivObj = ConvertType(equivObj, equivObj.GetType(), prop.ReturnType());
            }

            return GetConstantJSONStringEquivalent(prop.ReturnType(), equivObj, jsonp);
        }

        private static object ConvertType(object val, Type fromType, Type toType)
        {
            if (toType.IsEnum)
            {
                toType = Enum.GetUnderlyingType(toType);
            }

            if (toType == typeof(sbyte))
            {
                if (fromType.IsSigned())
                {
                    var l = (long)Convert.ChangeType(val, typeof(long));

                    return (sbyte)l;
                }

                var ul = (ulong)Convert.ChangeType(val, typeof(ulong));

                return (sbyte)ul;
            }

            if (toType == typeof(ushort))
            {
                if (fromType.IsSigned())
                {
                    var l = (long)Convert.ChangeType(val, typeof(long));

                    return (ushort)l;
                }

                var ul = (ulong)Convert.ChangeType(val, typeof(ulong));

                return (ushort)ul;
            }

            if (toType == typeof(uint))
            {
                if (fromType.IsSigned())
                {
                    var l = (long)Convert.ChangeType(val, typeof(long));

                    return (uint)l;
                }

                var ul = (ulong)Convert.ChangeType(val, typeof(ulong));

                return (uint)ul;
            }

            if (toType == typeof(ulong))
            {
                if (fromType.IsSigned())
                {
                    long l = (long)Convert.ChangeType(val, typeof(long));

                    return (ulong)l;
                }

                return Convert.ChangeType(val, typeof(ulong));
            }

            return Convert.ChangeType(val, toType);
        }

        public static bool IsUnsigned(this Type t)
        {
            return !t.IsSigned();
        }

        public static bool IsSigned(this Type t)
        {
            return
                t == typeof(sbyte) ||
                t == typeof(short) ||
                t == typeof(int) ||
                t == typeof(long);
        }

        public static string GetConstantJSONStringEquivalent(this FieldInfo field, bool jsonp)
        {
            var obj = field.GetRawConstantValue();

            return GetConstantJSONStringEquivalent(field.ReturnType(), obj, jsonp);
        }

        private static string GetConstantJSONStringEquivalent(Type type, object obj, bool jsonp)
        {
            if (obj == null) return "null";

            var asStr = obj as string;
            if (asStr != null)
            {
                return "\"" + asStr.JsonEscape(jsonp) + "\"";
            }

            var asChar = obj is char ? (char?)(char)obj : (char?)null;
            if (asChar != null)
            {
                return "\"" + asChar.Value.JsonEscape(jsonp) + "\"";
            }

            var asBool = obj is bool ? (bool?)(bool)obj : (bool?)null;
            if (asBool != null)
            {
                return asBool.Value ? "true" : "false";
            }

            if (type.IsEnum)
            {
                return "\"" + Enum.GetName(type, obj).JsonEscape(jsonp) + "\"";
            }
            
            var formattable = obj as IFormattable;
            if (formattable != null)
                return formattable.ToString(null, CultureInfo.InvariantCulture);
            else
                return obj.ToString();
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
                ret += c.JsonEscape(jsonp);
            }

            return ret;
        }

        public static string JsonEscape(this char c, bool jsonp)
        {
            switch (c)
            {
                case '\\': return @"\\";
                case '"': return @"\""";
                case '\u0000': return @"\u0000";
                case '\u0001': return @"\u0001";
                case '\u0002': return @"\u0002";
                case '\u0003': return @"\u0003";
                case '\u0004': return @"\u0004";
                case '\u0005': return @"\u0005";
                case '\u0006': return @"\u0006";
                case '\u0007': return @"\u0007";
                case '\u0008': return @"\u0008";
                case '\u0009': return @"\t";
                case '\u000A': return @"\n";
                case '\u000B': return @"\v";
                case '\u000C': return @"\f";
                case '\u000D': return @"\r";
                case '\u000E': return @"\u000E";
                case '\u000F': return @"\u000F";
                case '\u0010': return @"\u0010";
                case '\u0011': return @"\u0011";
                case '\u0012': return @"\u0012";
                case '\u0013': return @"\u0013";
                case '\u0014': return @"\u0014";
                case '\u0015': return @"\u0015";
                case '\u0016': return @"\u0016";
                case '\u0017': return @"\u0017";
                case '\u0018': return @"\u0018";
                case '\u0019': return @"\u0019";
                case '\u001A': return @"\u001A";
                case '\u001B': return @"\u001B";
                case '\u001C': return @"\u001C";
                case '\u001D': return @"\u001D";
                case '\u001E': return @"\u001E";
                case '\u001F': return @"\u001F";

                case '\u2028':
                    if (jsonp)
                    {
                        return @"\u2028";
                    }
                    else
                    {
                        goto default;
                    }

                case '\u2029':
                    if (jsonp)
                    {
                        return @"\u2029";
                    }
                    else
                    {
                        goto default;
                    }

                default: return c.ToString();
            }
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
