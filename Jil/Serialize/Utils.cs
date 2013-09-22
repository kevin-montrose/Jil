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

            foreach(var field in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
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

        private static Dictionary<Type, Dictionary<PropertyInfo, List<FieldInfo>>> PropertyFieldUsageCached = new Dictionary<Type, Dictionary<PropertyInfo, List<FieldInfo>>>();
        public static Dictionary<PropertyInfo, List<FieldInfo>> PropertyFieldUsage(Type t)
        {
            lock (PropertyFieldUsageCached)
            {
                Dictionary<PropertyInfo, List<FieldInfo>> cached;
                if (PropertyFieldUsageCached.TryGetValue(t, out cached))
                {
                    return cached;
                }
            }

            var ret = _GetPropertyFieldUsage(t);

            lock (PropertyFieldUsageCached)
            {
                PropertyFieldUsageCached[t] = ret;
            }

            return ret;
        }

        private static Dictionary<PropertyInfo, List<FieldInfo>> _GetPropertyFieldUsage(Type t)
        {
            if (t.IsValueType)
            {
                // We'll deal with value types in a bit...
                throw new NotImplementedException();
            }

            var ret = new Dictionary<PropertyInfo, List<FieldInfo>>();

            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(p => p.GetMethod != null);

			var module = t.Module;

            foreach (var prop in props)
            {
                var getMtd = prop.GetMethod;
                var mtdBody = getMtd.GetMethodBody();
                var il = mtdBody.GetILAsByteArray();

                var fieldHandles = _GetFieldHandles(il);

                var fieldInfos = fieldHandles.Select(f => module.ResolveField(f)).ToList();

                ret[prop] = fieldInfos;
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
                OpCode ignored;
                var startsAt = i;
                i += _ReadOp(cil, i, out fieldHandle, out ignored);

                if (fieldHandle.HasValue)
                {
                    ret.Add(fieldHandle.Value);
                }
            }

            return ret;
        }

        private static int _ReadOp(byte[] cil, int ix, out int? fieldHandle, out OpCode opcode)
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

            fieldHandle = _ReadFieldOperands(opcode, cil, ix, ix + advance, ref advance);

            return advance;
        }

        private static int? _ReadFieldOperands(OpCode op, byte[] cil, int instrStart, int operandStart, ref int advance)
        {
			Func<int, int> readInt = (at) => cil[at] | (cil[at + 1] << 8) | (cil[at + 2] << 16) | (cil[at + 3] << 24);

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
                    return null;

                case OperandType.InlineI8:
                    advance += 8;
                    return null;

                case OperandType.InlineNone:
                    return null;

                case OperandType.InlineR:
                    advance += 8;
                    return null;

                case OperandType.InlineSig:
                    advance += 4;
                    return null;

                case OperandType.InlineString:
                    advance += 4;
                    return null;

                case OperandType.InlineVar:
                    advance += 2;
                    return null;

                case OperandType.ShortInlineI:
                    advance += 1;
                    return null;

                case OperandType.ShortInlineR:
                    advance += 4;
                    return null;

                case OperandType.ShortInlineVar:
                    advance += 1;
                    return null;

                default: throw new Exception("Unexpected operand type [" + op.OperandType + "]");
            }
        }

        private static Dictionary<Type, Dictionary<FieldInfo, int>> FieldOffsetsInMemoryFieldCache = new Dictionary<Type, Dictionary<FieldInfo, int>>();
        public static Dictionary<FieldInfo, int> FieldOffsetsInMemory(Type t)
        {
            lock (FieldOffsetsInMemoryFieldCache)
            {
				Dictionary<FieldInfo, int> cached;
                if (FieldOffsetsInMemoryFieldCache.TryGetValue(t, out cached))
                {
                    return cached;
                }
            }

            var ret = _GetFieldOffsetsInMemory(t);

            lock (FieldOffsetsInMemoryFieldCache)
            {
                FieldOffsetsInMemoryFieldCache[t] = ret;
            }

            return ret;
        }

        private static Dictionary<FieldInfo, int> _GetFieldOffsetsInMemory(Type t)
        {
			if(t.IsValueType)
            {
				// We'll deal with value types in a bit...
				throw new NotImplementedException();
            }

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

        private static readonly IEnumerable<OpCode> LegalSimpleProperyOps = new[] { OpCodes.Nop, OpCodes.Ldarg_0, OpCodes.Ldfld, OpCodes.Ret };
        public static FieldInfo GetSimplePropertyBackingField(MethodInfo getMtd)
        {
            var mtdBody = getMtd.GetMethodBody();
            if (mtdBody == null) return null;

            var cil = mtdBody.GetILAsByteArray();
            if (cil == null) return null;

            var fieldHandles = new List<int>();
            var ops = new List<OpCode>();

            int i = 0;
            while (i < cil.Length)
            {
                int? fieldHandle;
                OpCode op;
                var startsAt = i;
                i += _ReadOp(cil, i, out fieldHandle, out op);

                ops.Add(op);

                if (fieldHandle.HasValue)
                {
                    fieldHandles.Add(fieldHandle.Value);
                }
            }

            if (fieldHandles.Count != 1) return null;
            if (ops.Any(a => !LegalSimpleProperyOps.Contains(a))) return null;

            Type[] genericTypeParameters = null;
            Type[] genericMethodParameters = null;

            if (getMtd.DeclaringType.IsGenericType)
            {
                genericTypeParameters = getMtd.DeclaringType.GenericTypeArguments;
            }

            if (getMtd.IsGenericMethod)
            {
                genericMethodParameters = getMtd.GetGenericArguments();
            }

            return getMtd.Module.ResolveField(fieldHandles[0], genericTypeParameters, genericMethodParameters);
        }
    }
}
