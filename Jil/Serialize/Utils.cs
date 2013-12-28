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
using Jil.Common;

namespace Jil.Serialize
{
    partial class Utils
    {
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
        internal static int[] MemberOrdering = new int[] { 3, 1, -4, 2 };

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

        public static List<Tuple<OpCode, int?, long?, double?>> Decompile(MethodInfo mtd)
        {
            if (mtd == null) return null;
            var mtdBody = mtd.GetMethodBody();
            if (mtdBody == null) return null;
            var cil = mtdBody.GetILAsByteArray();
            if (cil == null) return null;

            var ret = new List<Tuple<OpCode, int?, long?, double?>>();

            int i = 0;
            while (i < cil.Length)
            {
                int? ignored;
                OpCode opcode;
                int? intOperand;
                long? longOperand;
                double? doubleOperand;
                var startsAt = i;
                i += _ReadOp(cil, i, out ignored, out opcode, out intOperand, out longOperand, out doubleOperand);

                ret.Add(Tuple.Create(opcode, intOperand, longOperand, doubleOperand));
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
                    return null;

                case OperandType.ShortInlineI:
                    advance += 1;
                    constantInt = cil[operandStart];
                    return null;

                case OperandType.ShortInlineR:
                    advance += 4;
                    // TODO: what generates this?
                    return null;

                case OperandType.ShortInlineVar:
                    advance += 1;
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

                var getAddrs = emit.CreateDelegate();

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
    }
}