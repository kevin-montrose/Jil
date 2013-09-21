using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    class InlineSerializer
    {
        public static bool ReorderMembers = true;

        static Action<TextWriter, ForType> LookupPrimitiveType<ForType>()
        {
            var forType = typeof(ForType);

            if (forType == typeof(byte))
            {
                Action<TextWriter, byte> ret = (writer, b) => { writer.Write(b); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(sbyte))
            {
                Action<TextWriter, sbyte> ret = (writer, s) => { writer.Write(s); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(short))
            {
                Action<TextWriter, short> ret = (writer, s) => { writer.Write(s); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(ushort))
            {
                Action<TextWriter, ushort> ret = (writer, u) => { writer.Write(u); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(int))
            {
                Action<TextWriter, int> ret = (writer, i) => { writer.Write(i); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(uint))
            {
                Action<TextWriter, uint> ret = (writer, u) => { writer.Write(u); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(long))
            {
                Action<TextWriter, long> ret = (writer, l) => { writer.Write(l); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(ulong))
            {
                Action<TextWriter, ulong> ret = (writer, u) => { writer.Write(u); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(float))
            {
                Action<TextWriter, float> ret = (writer, f) => { writer.Write(f); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(double))
            {
                Action<TextWriter, double> ret = (writer, d) => { writer.Write(d); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(decimal))
            {
                Action<TextWriter, decimal> ret = (writer, d) => { writer.Write(d); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(string))
            {
                Action<TextWriter, string> ret = (writer, s) => { writer.Write("\""); writer.Write(s.JsonEscape()); writer.Write("\""); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            if (forType == typeof(char))
            {
                Action<TextWriter, char> ret = (writer, c) => { writer.Write("\""); writer.Write(c.JsonEscape()); writer.Write("\""); };

                return (Action<TextWriter, ForType>)(object)ret;
            }

            throw new NotImplementedException();
        }

        static MethodInfo TextWriter_WriteString = typeof(TextWriter).GetMethod("Write", new[] { typeof(string) });
        static void WriteString(string str, Emit emit)
        {
            emit.LoadArgument(0);
            emit.LoadConstant(str);
            emit.CallVirtual(TextWriter_WriteString);
        }

        static List<MemberInfo> OrderMembersForAccess(Type forType)
        {
            var fields = Utils.FieldOffsetsInMemory(forType);
            var props = Utils.PropertyFieldUsage(forType);

            var members = forType.GetProperties().Where(p => p.GetMethod != null).Cast<MemberInfo>().Concat(forType.GetFields());

            // We want to access fields and properties such that we go "with the grain" in memory
            //   Broadly speaking, we access members in the order they are laid out in memory preferring
            //   fields over properties
            var ret =
                !ReorderMembers ?
                    members :
                    members.OrderBy(
                         m =>
                         {
                             var asField = m as FieldInfo;
                             if (asField != null)
                             {
                                 return fields[asField];
                             }

                             var asProp = m as PropertyInfo;
                             if (asProp != null)
                             {
                                 List<FieldInfo> usesFields;
                                 if (!props.TryGetValue(asProp, out usesFields))
                                 {
                                     return int.MaxValue;
                                 }

                                 return usesFields.Select(f => fields[f]).Min();
                             }

                             return int.MaxValue;
                         }
                    ).ThenBy(
                        m => m is FieldInfo ? 0 : 1
                    ).ToList();

            return ret.ToList();
        }

        static void WriteRecursiveType(Type type)
        {
            throw new NotImplementedException();
        }

        static HashSet<Type> FindRecursiveTypes(Type forType)
        {
            return new HashSet<Type>();
        }

        static void WriteMember(MemberInfo member, Emit emit, HashSet<Type> recursiveTypes, Sigil.Local inLocal = null)
        {
            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new Exception("Wha?");

            var serializingType = asField != null ? asField.FieldType : asProp.PropertyType;

            var isRecursive = recursiveTypes.Contains(serializingType);

            // Only put this on the stack if we'll need it
            if (serializingType.IsPrimitiveType() || isRecursive)
            {
                emit.LoadArgument(0);   // TextWriter
            }

            if (inLocal == null)
            {
                emit.LoadArgument(1);       // TextWriter obj
            }
            else
            {
                emit.LoadLocal(inLocal);    // TextWriter obj
            }

            if (asField != null)
            {
                emit.LoadField(asField);    // TextWriter field
            }

            if (asProp != null)
            {
                var getMtd = asProp.GetMethod;
                if(getMtd.IsVirtual)
                {
                    emit.CallVirtual(getMtd);   // TextWriter prop
                }
                else
                {
                    emit.Call(getMtd);          // TextWriter prop
                }
            }

            if (recursiveTypes.Contains(serializingType))
            {
                WriteRecursiveType(serializingType);
                return;
            }

            if (serializingType.IsPrimitiveType())
            {
                var needsIntCoersion = serializingType == typeof(byte) || serializingType == typeof(sbyte) || serializingType == typeof(short) || serializingType == typeof(ushort);

                if (needsIntCoersion)
                {
                    emit.Convert<int>();            // TextWriter int
                    serializingType = typeof(int); 
                }

                var builtInMtd = typeof(TextWriter).GetMethod("Write", new[] { serializingType });

                emit.CallVirtual(builtInMtd);       // --empty--
                return;
            }

            using (var loc = emit.DeclareLocal(serializingType))
            {
                emit.StoreLocal(loc);   // TextWriter;

                var mtd = InlineSerializer_BuildObject;

                mtd.Invoke(null, new object[] { serializingType, emit, recursiveTypes, loc });
            }
        }

        static MethodInfo InlineSerializer_BuildObject = typeof(InlineSerializer).GetMethod("BuildObject", BindingFlags.Static | BindingFlags.NonPublic);
        internal static void BuildObject(Type forType, Emit emit, HashSet<Type> recursiveTypes, Sigil.Local inLocal = null)
        {
            var writeOrder = OrderMembersForAccess(forType);

            var stringsNeeded = Utils.ExtractStringConstants(forType);

            var notNull = emit.DefineLabel();

            if (inLocal != null)
            {
                emit.LoadLocal(inLocal);
            }
            else
            {
                emit.LoadArgument(1);
            }

            emit.BranchIfTrue(notNull);
            WriteString("null", emit);
            emit.Return();

            emit.MarkLabel(notNull);
            WriteString("{", emit);

            var firstPass = true;
            var previousMemberWasStringy = false;
            foreach (var member in writeOrder)
            {
                string keyString;
                if (firstPass)
                {
                    keyString = "\"" + member.Name.JsonEscape() + "\":";
                    firstPass = false;
                }
                else
                {
                    keyString = ",\"" + member.Name.JsonEscape() + "\":";
                }

                if (previousMemberWasStringy)
                {
                    keyString = "\"" + keyString;
                }

                var isStringy = member.IsStringyType();

                if (isStringy)
                {
                    keyString = keyString + "\"";
                }

                WriteString(keyString, emit);
                WriteMember(member, emit, recursiveTypes, inLocal);

                previousMemberWasStringy = isStringy;
            }

            if (previousMemberWasStringy)
            {
                WriteString("\"}", emit);
            }
            else
            {
                WriteString("}", emit);
            }
        }

        static Action<TextWriter, ForType> BuildObjectWithNewDelegate<ForType>()
        {
            var recursiveTypes = FindRecursiveTypes(typeof(ForType));

            var emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType) });
            
            BuildObject(typeof(ForType), emit, recursiveTypes);
            emit.Return();

            return emit.CreateDelegate<Action<TextWriter, ForType>>();
        }

        public static Action<TextWriter, ForType> Build<ForType>()
        {
            var forType = typeof(ForType);

            if (forType.IsPrimitiveType())
            {
                return LookupPrimitiveType<ForType>();
            }

            if (forType.IsDictionaryType())
            {
                throw new NotImplementedException();
            }

            if (forType.IsListType())
            {
                throw new NotImplementedException();
            }

            if (forType.IsValueType) throw new NotImplementedException();

            return BuildObjectWithNewDelegate<ForType>();
        }
    }
}
