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
        public const string ObjectVariable = "obj";
        public const string WriterVariable = "writer";
        public const string StringConstantsField = "_StringsConstants";
        public const string SerializeMethod = "Serialize";

        private static AssemblyBuilder Assembly;
        private static ModuleBuilder Module;

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

                emit = Emit.BuildStaticMethod(typeof(void), new[] { typeof(TextWriter), forType }, ret, SerializeMethod, MethodAttributes.Public);

                emit.DeclareLocal(typeof(TextWriter), WriterVariable);
                emit.DeclareLocal(forType, ObjectVariable);

                emit.LoadArgument(0);
                emit.StoreLocal(WriterVariable);

                emit.LoadArgument(1);
                emit.StoreLocal(ObjectVariable);

                return ret;
            }
        }

        /// There are a ton of string constants when serializing most JSON.
        /// So shove constants into an array we can suck data out of rather than
        ///   having to de-string everywhere.
        private static StringConstants AddStringsToType(TypeBuilder toType, List<string> typeStrings)
        {
            var jsonPropStrings = typeStrings.Select(s => ",\"" + s.JsonEscape() + "\":").ToList();

            var ret = new StringConstants(jsonPropStrings);

            var consts = toType.DefineField(StringConstantsField, typeof(char[]), FieldAttributes.Static | FieldAttributes.Public);

            var staticConst = toType.DefineConstructor(MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes);
            var il = staticConst.GetILGenerator();
            il.Emit(OpCodes.Ldstr, ret.TotalString);
            il.Emit(OpCodes.Call, typeof(string).GetMethod("ToCharArray", Type.EmptyTypes));
            il.Emit(OpCodes.Stsfld, consts);
            il.Emit(OpCodes.Ret);

            return ret;
        }

        private static void WriteString(Type toType, StringConstants strs, string str, Emit emit)
        {
            var locInCache = strs.LookupString(str);

            if (locInCache == null)
            {
                // If we didn't put it in the strings cache, use a string literal
                emit.LoadLocal(WriterVariable);
                emit.LoadConstant(str);
                emit.CallVirtual(typeof(TextWriter).GetMethod("Write", new[] { typeof(string) }));

                return;
            }

            emit.LoadLocal(WriterVariable);
            emit.LoadField(toType.GetField(StringConstantsField));
            emit.LoadConstant(locInCache.Value);
            emit.LoadConstant(str.Length);
            emit.CallVirtual(typeof(TextWriter).GetMethod("Write", new[] { typeof(char[]), typeof(int), typeof(int) }));
        }

        private static void WriteMember(MemberInfo member, Emit emit)
        {
            var asField = member as FieldInfo;
            if (asField != null)
            {
                WriteField(asField, emit);
                return;
            }

            var asProp = member as PropertyInfo;
            if (asProp != null)
            {
                WriteProperty(asProp, emit);
                return;
            }

            throw new Exception("WriteMember got " + member + ", which makes no sense");
        }

        private static void WriteField(FieldInfo field, Emit emit)
        {
            emit.LoadLocal(WriterVariable);
            emit.LoadLocal(ObjectVariable);
            emit.LoadField(field);

            CallSerializer(field.FieldType, emit);
        }

        private static void WriteProperty(PropertyInfo prop, Emit emit)
        {
            emit.LoadLocal(WriterVariable);

            emit.LoadLocal(ObjectVariable);

            if (prop.GetMethod.IsVirtual)
            {
                emit.CallVirtual(prop.GetMethod);
            }
            else
            {
                emit.Call(prop.GetMethod);
            }

            CallSerializer(prop.PropertyType, emit);
        }

        private static void CallSerializer(Type memberType, Emit emit)
        {
            // top of stack:
            // [memberType]
            // [TextWriter]

            var builtInMtd = typeof(TextWriter).GetMethod("Write", new[] { memberType });
            if (builtInMtd != null)
            {
                // For now, pass directly to TextWriter if we can

                emit.CallVirtual(builtInMtd);
                return;
            }

            var typeCacheType = typeof(TypeCache<>).MakeGenericType(memberType);
            var serializerType = (Type)typeCacheType.GetField("Serializer").GetValue(null);

            var serializeMtd = serializerType.GetMethod(SerializeMethod);

            emit.Call(serializeMtd);
        }

        private static void BuildObject(Type forType, TypeBuilder intoType, Emit emit)
        {
            var fields = Utils.FieldOffsetsInMemory(forType);
            var props = Utils.PropertyFieldUsage(forType);

            var members = forType.GetProperties().Where(p => p.GetMethod != null).Cast<MemberInfo>().Concat(forType.GetFields());

            // We want to access fields and properties such that we go "with the grain" in memory
            //   Broadly speaking, we access members in the order they are laid out in memory preferring
            //   fields over properties
            var writeOrder =
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

            var stringsNeeded = Utils.ExtractStringConstants(forType);

            //var strs = new StringConstants(new List<string>());

            var strs = AddStringsToType(intoType, stringsNeeded);

            var notNull = emit.DefineLabel("not_null");

            emit.LoadLocal(ObjectVariable);
            emit.BranchIfTrue(notNull);
            WriteString(intoType, strs, "null", emit);
            emit.Return();

            emit.MarkLabel(notNull);
            WriteString(intoType, strs, "{", emit);
            
            var firstPass = true;
            foreach (var member in writeOrder)
            {
                if (firstPass)
                {
                    WriteString(intoType, strs, "\"" + member.Name.JsonEscape() + "\":", emit);
                }
                else
                {
                    WriteString(intoType, strs, ",\"" + member.Name.JsonEscape() + "\":", emit);
                }
                WriteMember(member, emit);
            }

            WriteString(intoType, strs, "}", emit);

            emit.Return();

            string instrs;
            emit.CreateMethod(out instrs);
        }

        public static void BuildDictionary(Type dictType, TypeBuilder intoType, Emit emit)
        {
            throw new NotImplementedException();
        }

        public static void BuildList(Type listType, TypeBuilder intoType, Emit emit)
        {
            throw new NotImplementedException();
        }

        public static void Build(Type forType, TypeBuilder intoType, Emit emit)
        {
            if (forType.IsDictionaryType())
            {
                BuildDictionary(forType, intoType, emit);
                return;
            }

            if (forType.IsDictionaryType())
            {
                BuildList(forType, intoType, emit);
                return;
            }

            if (forType.IsValueType) throw new NotImplementedException();

            BuildObject(forType, intoType, emit);
        }
    }
}
