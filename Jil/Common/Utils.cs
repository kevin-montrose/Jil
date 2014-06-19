using Sigil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    partial class Utils
    {
        public const OptimizationOptions DelegateOptimizationOptions = OptimizationOptions.All & ~OptimizationOptions.EnableBranchPatching;

        private static readonly Dictionary<int, OpCode> OneByteOps;
        private static readonly Dictionary<int, OpCode> TwoByteOps;

        static Utils()
        {
            var oneByte = new List<OpCode>();
            var twoByte = new List<OpCode>();

            foreach (var field in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var op = (OpCode)field.GetValue(null);

                if (op.Size == 1)
                {
                    oneByte.Add(op);
                    continue;
                }

                if (op.Size == 2)
                {
                    twoByte.Add(op);
                    continue;
                }

                throw new Exception("Unexpected op size for " + op);
            }

            OneByteOps = oneByte.ToDictionary(d => (int)d.Value, d => d);
            TwoByteOps = twoByte.ToDictionary(d => (int)(d.Value & 0xFF), d => d);
        }

        // The Experiments project was used to find these #s
        // Basically, based on some limited testing it appears that this order of
        //    member access is the "fastest" most often given the buckets I've chosen to use.
        // I make no claims that this is absolutely ideal, but it appears to be better than
        //   declaration order.
        internal static int[] MemberOrdering = new int[] { -1, 3, 4, -2 };

        public static List<MemberInfo> IdealMemberOrderForWriting(Type forType, IEnumerable<Type> recursiveTypes, IEnumerable<MemberInfo> members)
        {
            var fields = Utils.FieldOffsetsInMemory(forType);
            var props = Utils.PropertyFieldUsage(forType);

            var simpleTypes = members.Where(m => m.ReturnType().IsValueType && !m.ReturnType().IsNullableType() && m.ReturnType().IsPrimitiveType()).ToList();
            var otherPrimitive = members.Where(m => (m.ReturnType().IsPrimitiveType() || m.ReturnType().IsNullableType()) && !simpleTypes.Contains(m)).ToList();
            var recursive = members.Where(m => recursiveTypes.Contains(m.ReturnType()) && !simpleTypes.Contains(m) && !otherPrimitive.Contains(m)).ToList();
            var everythingElse = members.Where(m => !simpleTypes.Contains(m) && !otherPrimitive.Contains(m) && !recursive.Contains(m)).ToList();

            Func<MemberInfo, int> byAccessOrder =
                m =>
                {
                    var asField = m as FieldInfo;
                    if (asField != null)
                    {
                        int fieldAddr;
                        if (fields.TryGetValue(asField, out fieldAddr))
                        {
                            return fieldAddr;
                        }

                        return int.MaxValue;
                    }

                    var asProp = m as PropertyInfo;
                    if (asProp != null)
                    {
                        List<FieldInfo> usesFields;
                        if (!props.TryGetValue(asProp, out usesFields))
                        {
                            return int.MaxValue;
                        }

                        if (usesFields.Count == 0) return int.MaxValue;

                        return
                            usesFields.Select(
                                f =>
                                {
                                    int fieldAdd;
                                    if (fields.TryGetValue(f, out fieldAdd))
                                    {
                                        return fieldAdd;
                                    }

                                    return int.MaxValue;
                                }
                           ).Min();
                    }

                    return int.MaxValue;
                };

            Func<MemberInfo, int> fieldsFirst = m => m is FieldInfo ? 0 : 1;

            var ret = new List<MemberInfo>();

            foreach (var ix in MemberOrdering)
            {
                var asc = ix > 0;
                var i = Math.Abs(ix);

                switch (i)
                {
                    case 1:
                        ret.AddRange(
                            asc ?
                                simpleTypes.OrderBy(byAccessOrder).ThenBy(fieldsFirst) :
                                simpleTypes.OrderByDescending(byAccessOrder).ThenByDescending(fieldsFirst)
                        );
                        break;

                    case 2:
                        ret.AddRange(
                            asc ?
                                otherPrimitive.OrderBy(byAccessOrder).ThenBy(fieldsFirst) :
                                otherPrimitive.OrderByDescending(byAccessOrder).ThenByDescending(fieldsFirst)
                        );
                        break;

                    case 3:
                        ret.AddRange(
                            asc ?
                                everythingElse.OrderBy(byAccessOrder).ThenBy(fieldsFirst) :
                                everythingElse.OrderByDescending(byAccessOrder).ThenByDescending(fieldsFirst)
                        );
                        break;

                    case 4:
                        ret.AddRange(
                            asc ?
                                recursive.OrderBy(byAccessOrder).ThenBy(fieldsFirst) :
                                recursive.OrderByDescending(byAccessOrder).ThenByDescending(fieldsFirst)
                        );
                        break;

                    default: throw new Exception();
                }
            }

            return ret;
        }

        public static Dictionary<PropertyInfo, List<FieldInfo>> PropertyFieldUsage(Type t)
        {
            var ret = new Dictionary<PropertyInfo, List<FieldInfo>>();

            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(p => p.GetMethod != null && p.GetMethod.GetParameters().Count() == 0);

            var module = t.Module;

            foreach (var prop in props)
            {
                try
                {
                    var getMtd = prop.GetMethod;
                    var mtdBody = getMtd.GetMethodBody();
                    var il = mtdBody.GetILAsByteArray();

                    var fieldHandles = _GetFieldHandles(il);

                    var fieldInfos =
                        fieldHandles
                            .Select(
                                f =>
                                {
                                    var genArgs = t.GetGenericArguments();

                                    return module.ResolveField(f, genArgs, null);
                                }
                            ).ToList();

                    ret[prop] = fieldInfos;
                }
                catch { /* Anything that goes wrong in here, we can't really do anything about; just continue with less knowledge */ }
            }

            return ret;
        }

        public static List<Tuple<OpCode, int?, long?, double?, FieldInfo>> Decompile(MethodBase mtd)
        {
            var mtdBody = mtd.GetMethodBody();
            if (mtdBody == null) return null;
            var cil = mtdBody.GetILAsByteArray();
            if (cil == null) return null;

            var ret = new List<Tuple<OpCode, int?, long?, double?, FieldInfo>>();

            int i = 0;
            while (i < cil.Length)
            {
                int? fieldHandle;
                OpCode opcode;
                int? intOperand;
                long? longOperand;
                double? doubleOperand;
                var startsAt = i;
                i += _ReadOp(cil, i, out fieldHandle, out opcode, out intOperand, out longOperand, out doubleOperand);

                FieldInfo field = null;
                if (fieldHandle.HasValue)
                {
                    var genArguments = mtd.DeclaringType.GetGenericArguments();

                    field = mtd.Module.ResolveField(fieldHandle.Value, genArguments, null);
                }

                ret.Add(Tuple.Create(opcode, intOperand, longOperand, doubleOperand, field));
            }

            return ret;
        }

        private static List<int> _GetFieldHandles(byte[] cil)
        {
            var ret = new List<int>();

            int i = 0;
            while (i < cil.Length)
            {
                int? fieldHandle;
                OpCode ignoredOp;
                int? ignored1;
                long? ignored2;
                double? ignored3;
                var startsAt = i;
                i += _ReadOp(cil, i, out fieldHandle, out ignoredOp, out ignored1, out ignored2, out ignored3);

                if (fieldHandle.HasValue)
                {
                    ret.Add(fieldHandle.Value);
                }
            }

            return ret;
        }

        private static int _ReadOp(byte[] cil, int ix, out int? fieldHandle, out OpCode opcode, out int? intOperand, out long? longOperand, out double? doubleOperand)
        {
            const byte ContinueOpcode = 0xFE;

            int advance = 0;

            byte first = cil[ix];

            if (first == ContinueOpcode)
            {
                var next = cil[ix + 1];

                opcode = TwoByteOps[next];
                advance += 2;
            }
            else
            {
                opcode = OneByteOps[first];
                advance++;
            }

            fieldHandle = _ReadFieldOperands(opcode, cil, ix, ix + advance, ref advance, out intOperand, out longOperand, out doubleOperand);

            return advance;
        }

        private static int? _ReadFieldOperands(OpCode op, byte[] cil, int instrStart, int operandStart, ref int advance, out int? constantInt, out long? constantLong, out double? constantDouble)
        {
            Func<int, int> readInt = (at) => cil[at] | (cil[at + 1] << 8) | (cil[at + 2] << 16) | (cil[at + 3] << 24);
            Func<int, long> readLong = 
                (at) =>
                {
                    var a = (uint)(cil[at] | (cil[at + 1] << 8) | (cil[at + 2] << 16) | (cil[at + 3] << 24));
                    var b = (uint)(cil[at+4] | (cil[at + 5] << 8) | (cil[at + 6] << 16) | (cil[at + 7] << 24));

                    return (((long)b) << 32) | a;
                };
            Func<int, double> readDouble =
                (at) =>
                {
                    var arr = new byte[8];
                    arr[0] = cil[at];
                    arr[1] = cil[at + 1];
                    arr[2] = cil[at + 2];
                    arr[3] = cil[at + 3];
                    arr[4] = cil[at + 4];
                    arr[5] = cil[at + 5];
                    arr[6] = cil[at + 6];
                    arr[7] = cil[at + 7];

                    if (!BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(arr);
                    }

                    return BitConverter.ToDouble(arr, 0);
                };
            Func<int, float> readFloat =
                (at) =>
                {
                    var arr = new byte[4];
                    arr[0] = cil[at];
                    arr[1] = cil[at + 1];
                    arr[2] = cil[at + 2];
                    arr[3] = cil[at + 3];

                    if (!BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(arr);
                    }

                    return BitConverter.ToSingle(arr, 0);
                };

            constantInt = null;
            constantLong = null;
            constantDouble = null;

            switch (op.OperandType)
            {
                case OperandType.InlineBrTarget:
                    advance += 4;
                    return null;

                case OperandType.InlineSwitch:
                    advance += 4;
                    var len = readInt(operandStart);
                    var offset1 = instrStart + len * 4;
                    for (var i = 0; i < len; i++)
                    {
                        advance += 4;
                    }
                    return null;

                case OperandType.ShortInlineBrTarget:
                    advance += 1;
                    return null;

                case OperandType.InlineField:
                    advance += 4;
                    var field = readInt(operandStart);
                    return field;

                case OperandType.InlineTok:
                case OperandType.InlineType:
                case OperandType.InlineMethod:
                    advance += 4;
                    return null;

                case OperandType.InlineI:
                    advance += 4;
                    constantInt = readInt(operandStart);
                    return null;

                case OperandType.InlineI8:
                    advance += 8;
                    constantLong = readLong(operandStart);
                    return null;

                case OperandType.InlineNone:
                    return null;

                case OperandType.InlineR:
                    advance += 8;
                    constantDouble = readDouble(operandStart);
                    return null;

                case OperandType.InlineSig:
                    advance += 4;
                    return null;

                case OperandType.InlineString:
                    advance += 4;
                    constantInt = readInt(operandStart);
                    return null;

                case OperandType.InlineVar:
                    advance += 2;
                    constantInt = cil[operandStart];
                    return null;

                case OperandType.ShortInlineI:
                    advance += 1;
                    constantInt = cil[operandStart];
                    return null;

                case OperandType.ShortInlineR:
                    advance += 4;
                    constantDouble = readFloat(operandStart);
                    return null;

                case OperandType.ShortInlineVar:
                    advance += 1;
                    constantInt = cil[operandStart];
                    return null;

                default: throw new Exception("Unexpected operand type [" + op.OperandType + "]");
            }
        }

        public static Dictionary<FieldInfo, int> FieldOffsetsInMemory(Type t)
        {
            try
            {
                var fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                var emit = Emit<Func<object, ulong[]>>.NewDynamicMethod("_GetOffsetsInMemory" + t.FullName);
                var retLoc = emit.DeclareLocal<ulong[]>("ret");

                emit.LoadConstant(fields.Length);	// ulong
                emit.NewArray(typeof(ulong));		// ulong[]
                emit.StoreLocal(retLoc);			// --empty--

                for (var i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];

                    emit.LoadLocal(retLoc);			// ulong[]
                    emit.LoadConstant(i);			// ulong[] ulong

                    emit.LoadArgument(0);			// ulong[] ulong param#0
                    emit.CastClass(t);	            // ulong[] ulong param#0

                    emit.LoadFieldAddress(field);	// ulong[] ulong field&
                    emit.Convert<ulong>();			// ulong[] ulong ulong

                    emit.StoreElement<ulong>();		// --empty--
                }

                emit.LoadLocal(retLoc);			// ulong[]
                emit.Return();					// --empty--

                var getAddrs = emit.CreateDelegate(Utils.DelegateOptimizationOptions);

                var cons = t.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(p => p.GetParameters().Count()).FirstOrDefault();
                var consParameters = cons != null ? cons.GetParameters().Select(p => p.ParameterType.DefaultValue()).ToArray() : null;

                object obj;
                if (cons != null)
                {
                    obj = cons.Invoke(consParameters);
                }
                else
                {
                    obj = Activator.CreateInstance(t);
                }

                var addrs = getAddrs(obj);

                if (addrs.Length == 0)
                {
                    return new Dictionary<FieldInfo, int>();
                }

                var min = addrs.Min();

                var ret = new Dictionary<FieldInfo, int>();

                for (var i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];

                    var addr = addrs[i];
                    var offset = addr - min;

                    ret[field] = (int)offset;
                }

                return ret;
            }
            catch
            {
                // A lot can go wrong during this, and the common response is just to bail
                // This catch is much simpler than trying (and probably failing) to enumerate
                //    all the exceptional cases
                return new Dictionary<FieldInfo, int>();
            }
        }

        /// <summary>
        /// This returns a map of "name of member" => [Type of member, index of argument to constructor].
        /// We need this for anonymous types because we can't set properties (they're read-only).
        /// 
        /// How this works is kind of fun.
        /// 
        /// By spec, the arguments to the constructor are in declaration order for an anonymous type.
        /// So: new { A, B, C } => new SomeType(A a, B b, C c)
        /// 
        /// However there is no way to get declaration order via reflection, it's just not data that's
        /// preserved.  Furthermore, the names of backing fields for those read-only properties are not
        /// defined by the spec.
        /// 
        /// So I got clever.
        /// 
        /// This method decompiles the constructor to find out which fields are set by which arguments (by index).
        /// It then decompiles all properties to find out which fields back which properties.
        /// Then it finally works backwards from each property, taking the property's name type and using it's backing
        /// field to lookup which index to pass it in as when constructing the anonymous object.
        /// </summary>
        public static Dictionary<string, Tuple<Type, int>> GetAnonymousNameToConstructorMap(Type objType)
        {
            var cons = objType.GetConstructors().Single();

            var consInstrs = Utils.Decompile(cons);

            var fieldToArgumentIndex = new Dictionary<FieldInfo, int>();

            var fields = objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var storeInstr = consInstrs.FindIndex(d => d.Item1 == OpCodes.Stfld && d.Item5 == field);
                var preceedingLdArg = consInstrs.Take(storeInstr).Reverse().First(f => f.Item1.IsLoadArgumentOpCode());

                int paramIx;
                switch (preceedingLdArg.Item1.Value)
                {
                    // Ldarg_0 will be `this`, so we'll never see it

                    // Ldarg_1
                    case 0x03: paramIx = 1; break;
                    // Ldarg_2
                    case 0x04: paramIx = 2; break;
                    // Ldarg_3
                    case 0x05: paramIx = 3; break;

                    // Ldarg, Ldarg_S
                    default: paramIx = preceedingLdArg.Item2.Value; break;
                }

                fieldToArgumentIndex[field] = paramIx;
            }

            var propertyToBackingField = new Dictionary<PropertyInfo, FieldInfo>();
            foreach (var prop in objType.GetProperties())
            {
                var propInstrs = Utils.Decompile(prop.GetMethod);
                var backingField = propInstrs.Single(p => p.Item1 == OpCodes.Ldfld);

                propertyToBackingField[prop] = backingField.Item5;
            }

            var nameToTypeAndConsIndex =
                propertyToBackingField
                    .ToDictionary(
                        d => d.Key.Name,
                        d => Tuple.Create(d.Key.PropertyType, fieldToArgumentIndex[d.Value] - 1)    // -1 here because `this` adds 1 in the IL
                    );

            return nameToTypeAndConsIndex;
        }

        static MethodInfo DynamicProjectTo = typeof(Utils).GetMethod("_DynamicProjectTo", BindingFlags.NonPublic | BindingFlags.Static);
        static IEnumerable<T> _DynamicProjectTo<T>(IEnumerable<object> e)
        {
            return e.Cast<T>();
        }

        static System.Collections.Hashtable DynamicCastCache = new System.Collections.Hashtable();
        public static object DynamicProject(IEnumerable<object> e, Type castElementsTo)
        {
            var cached = (Func<IEnumerable<object>, object>)DynamicCastCache[castElementsTo];
            if (cached == null)
            {
                var mtd = DynamicProjectTo.MakeGenericMethod(castElementsTo);
                var emit = Sigil.Emit<Func<IEnumerable<object>, object>>.NewDynamicMethod();
                emit.LoadArgument(0);
                emit.Call(mtd);
                emit.Return();
                cached = emit.CreateDelegate(Utils.DelegateOptimizationOptions);

                lock (DynamicCastCache)
                {
                    DynamicCastCache[castElementsTo] = cached;
                }
            }

            return cached(e);
        }

        static MethodInfo ProjectToStringDictionary = typeof(Utils).GetMethod("_ProjectToStringDictionary", BindingFlags.Static | BindingFlags.NonPublic);
        static object _ProjectToStringDictionary<T>(IDictionary<object, object> real)
        {
            var ret = new Dictionary<string, T>();
            foreach (var kv in real)
            {
                ret[(string)kv.Key] = (T)kv.Value;
            }

            return ret;
        }

        static System.Collections.Hashtable StringDictionaryCache = new System.Collections.Hashtable();
        public static object ProjectStringDictionary(IDictionary<object, object> inner, Type valType)
        {
            var cached = (Func<IDictionary<object, object>, object>)StringDictionaryCache[valType];
            if (cached == null)
            {
                var mtd = ProjectToStringDictionary.MakeGenericMethod(valType);
                var emit = Sigil.Emit<Func<IDictionary<object, object>, object>>.NewDynamicMethod();
                emit.LoadArgument(0);
                emit.Call(mtd);
                emit.Return();

                cached = emit.CreateDelegate(Utils.DelegateOptimizationOptions);
                lock (StringDictionaryCache)
                {
                    StringDictionaryCache[valType] = cached;
                }
            }

            return cached(inner);
        }

        static MethodInfo ProjectToEnumDictionary = typeof(Utils).GetMethod("_ProjectToEnumDictionary", BindingFlags.Static | BindingFlags.NonPublic);
        static object _ProjectToEnumDictionary<TEnum, TValue>(IDictionary<object, object> real)
            where TEnum : struct
        {
            var ret = new Dictionary<TEnum, TValue>();
            foreach (var kv in real)
            {
                ret[(TEnum)kv.Key] = (TValue)kv.Value;
            }

            return ret;
        }

        static System.Collections.Hashtable EnumDictionaryCache = new System.Collections.Hashtable();
        public static object ProjectEnumDictionary(IDictionary<object, object> inner, Type keyType, Type valType)
        {
            var cached = (Func<IDictionary<object, object>, object>)EnumDictionaryCache[valType];
            if (cached == null)
            {
                var mtd = ProjectToEnumDictionary.MakeGenericMethod(keyType, valType);
                var emit = Sigil.Emit<Func<IDictionary<object, object>, object>>.NewDynamicMethod();
                emit.LoadArgument(0);
                emit.Call(mtd);
                emit.Return();

                cached = emit.CreateDelegate(Utils.DelegateOptimizationOptions);
                lock (EnumDictionaryCache)
                {
                    EnumDictionaryCache[valType] = cached;
                }
            }

            return cached(inner);
        }
    }
}