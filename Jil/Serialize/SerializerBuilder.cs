using Sigil;
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

        public static TypeBuilder Init<T>(out Emit<Action<TextWriter, T>> emit)
        {
            var forType = typeof(T);

            lock (Module)
            {
                var ret = Module.DefineType("_Jil_" + forType.FullName, TypeAttributes.Sealed | TypeAttributes.Class);

                emit = Emit<Action<TextWriter, T>>.BuildStaticMethod(ret, SerializeMethod, MethodAttributes.Public, allowUnverifiableCode: true);

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

            var consts = toType.DefineField(StringConstantsField, typeof(char[]), FieldAttributes.Static | FieldAttributes.Public);

            var ret = new StringConstants(jsonPropStrings, consts);

            var staticConst = toType.DefineConstructor(MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes);
            var il = staticConst.GetILGenerator();
            il.Emit(OpCodes.Ldstr, ret.TotalString);
            il.Emit(OpCodes.Call, typeof(string).GetMethod("ToCharArray", Type.EmptyTypes));
            il.Emit(OpCodes.Stsfld, consts);
            il.Emit(OpCodes.Ret);

            return ret;
        }

        private static void WriteString<T>(Type toType, StringConstants strs, string str, Emit<Action<TextWriter, T>> emit)
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
            emit.LoadField(strs.StoredInField);
            emit.LoadConstant(locInCache.Value);
            emit.LoadConstant(str.Length);
            emit.CallVirtual(typeof(TextWriter).GetMethod("Write", new[] { typeof(char[]), typeof(int), typeof(int) }));
        }

        private static void WriteMember<T>(MemberInfo member, Emit<Action<TextWriter, T>> emit)
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

        private static void WriteField<T>(FieldInfo field, Emit<Action<TextWriter, T>> emit)
        {
            emit.LoadLocal(WriterVariable);
            emit.LoadLocal(ObjectVariable);
            emit.LoadField(field);

            CallSerializer(field.FieldType, emit);
        }

        private static void WriteProperty<T>(PropertyInfo prop, Emit<Action<TextWriter, T>> emit)
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

        private static void CallSerializer<T>(Type memberType, Emit<Action<TextWriter, T>> emit)
        {
            // top of stack:
            // [memberType]
            // [TextWriter]

            if (memberType.IsPrimitiveType())
            {
                // For now, pass directly to TextWriter if we can

                var needsIntCoersion = memberType == typeof(byte) || memberType == typeof(sbyte) || memberType == typeof(short) || memberType == typeof(ushort);

                if (needsIntCoersion)
                {
                    emit.Convert<int>();
                    memberType = typeof(int);
                }

                var builtInMtd = typeof(TextWriter).GetMethod("Write", new[] { memberType });

                emit.CallVirtual(builtInMtd);
                return;
            }

            var invokeEmit = typeof(SerializerBuilder).GetMethod("InvokeCallWithEmit", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(memberType, typeof(T));

            invokeEmit.Invoke(null, new[] { emit });
        }

        private static void InvokeCallWithEmit<MemberType, T>(Emit<Action<TextWriter, T>> emit)
        {
            emit.Call(TypeCache<MemberType>.SerializerEmit);
        }

        private static MethodInfo BuildObject<T>(TypeBuilder intoType, Emit<Action<TextWriter, T>> emit)
        {
            var forType = typeof(T);

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

            var strs = AddStringsToType(intoType, stringsNeeded);

            var notNull = emit.DefineLabel("not_null");

            emit.LoadLocal(ObjectVariable);
            emit.BranchIfTrue(notNull);
            WriteString(intoType, strs, "null", emit);
            emit.Return();

            emit.MarkLabel(notNull);
            WriteString(intoType, strs, "{", emit);
            
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

                WriteString(intoType, strs, keyString, emit);
                WriteMember(member, emit);

                previousMemberWasStringy = isStringy;
            }

            if (previousMemberWasStringy)
            {
                WriteString(intoType, strs, "\"}", emit);
            }
            else
            {
                WriteString(intoType, strs, "}", emit);
            }

            emit.Return();

            return emit.CreateMethod();
        }

        private static MethodInfo BuildDictionary<T>(TypeBuilder intoType, Emit<Action<TextWriter, T>> emit)
        {
            var dictI = typeof(T).GetDictionaryInterface();

            var keyType = dictI.GetGenericArguments()[0];
            var valType = dictI.GetGenericArguments()[1];

            if (keyType != typeof(string))
            {
                throw new InvalidOperationException("JSON dictionaries must have strings as keys, found " + keyType);
            }

            var serializer = typeof(DictionarySerializer).GetMethod("Serialize").MakeGenericMethod(valType);

            emit.LoadArgument(0);
            emit.LoadArgument(1);
            emit.Call(serializer);
            emit.Return();
            
            return emit.CreateMethod();
        }

        private static MethodInfo BuildList<T>(TypeBuilder intoType, Emit<Action<TextWriter, T>> emit)
        {
            throw new NotImplementedException();
        }

        private static MethodInfo WritePrimitiveType<T>(Emit<Action<TextWriter, T>> emit)
        {
            var isStringy = typeof(T).IsStringyType();

            if (isStringy)
            {
                emit.LoadArgument(0);
                emit.LoadConstant("\"");
                emit.CallVirtual(typeof(TextWriter).GetMethod("Write", new [] { typeof(string) }));
            }

            emit.LoadArgument(0);
            emit.LoadArgument(1);

            CallSerializer(typeof(T), emit);

            if (isStringy)
            {
                emit.LoadArgument(0);
                emit.LoadConstant("\"");
                emit.CallVirtual(typeof(TextWriter).GetMethod("Write", new [] { typeof(string) }));
            }

            emit.Return();

            return emit.CreateMethod();
        }

        public static MethodInfo Build<T>(TypeBuilder intoType, Emit<Action<TextWriter, T>> emit)
        {
            var forType = typeof(T);

            if (forType.IsPrimitiveType())
            {
                return WritePrimitiveType(emit);
            }

            if (forType.IsDictionaryType())
            {
                return BuildDictionary(intoType, emit);
            }

            if (forType.IsDictionaryType())
            {
                return BuildList(intoType, emit);
            }

            if (forType.IsValueType) throw new NotImplementedException();

            return BuildObject(intoType, emit);
        }
    }
}
