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
            if (t.IsValueType)
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

        // Adapted from: https://code.google.com/p/double-conversion/source/browse/
        /*
            Copyright 2006-2011, the V8 project authors. All rights reserved.
            Redistribution and use in source and binary forms, with or without
            modification, are permitted provided that the following conditions are
            met:

                * Redistributions of source code must retain the above copyright
                  notice, this list of conditions and the following disclaimer.
                * Redistributions in binary form must reproduce the above
                  copyright notice, this list of conditions and the following
                  disclaimer in the documentation and/or other materials provided
                  with the distribution.
                * Neither the name of Google Inc. nor the names of its
                  contributors may be used to endorse or promote products derived
                  from this software without specific prior written permission.

            THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
            "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
            LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
            A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
            OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
            SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
            LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
            DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
            THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
            (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
            OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
         */
        public static bool DoubleToAscii(TextWriter writer, double v, char[] buffer)
        {
            if (v == 0)
            {
                writer.Write('0');
                return true;
            }

            var buffer_length = buffer.Length;
            bool sign;
            int length, point;

            if (new Double(v).Sign() < 0)
            {
                sign = true;
                v = -v;
            }
            else
            {
                sign = false;
            }

            bool fast_worked =  FastDtoa(v, 0, buffer, out length, out point);
            if (fast_worked)
            {
                if (sign)
                {
                    writer.Write('-');
                }

                
                if (point < length)
                {
                    if (point > 0)
                    {
                        writer.Write(buffer, 0, point);
                        writer.Write('.');
                    }

                    if (point <= 0)
                    {
                        writer.Write('0');
                        writer.Write('.');

                        while (point < 0)
                        {
                            writer.Write('0');
                            point++;
                        }
                    }

                    
                    writer.Write(buffer, point, length - point);
                }
                else
                {
                    writer.Write(buffer, 0, length);
                    while (point > length)
                    {
                        writer.Write('0');
                        point--;
                    }
                }

                return true;
            }

            // We failed, do it the slow way
            writer.Write(v.ToString());
            return false;
        }

        static bool FastDtoa(double v, int requested_digits, char[] buffer, out int length, out int decimal_point)
        {
            bool result = false;
            int decimal_exponent = 0;
            result = Grisu3(v, buffer, out length, out decimal_exponent);

            decimal_point = -1;

            if (result)
            {
                decimal_point = length + decimal_exponent;
                buffer[length] = '\0';
            }
            return result;
        }

        struct DiyFp
        {
            const ulong kUint64MSB = 0x8000000000000000;
            ulong f_;
            int e_;

            public const int kSignificandSize = 64;

            public DiyFp(ulong f, int e)
            {
                f_ = f;
                e_ = e;
            }


            void Subtract(ref DiyFp other)
            {
                f_ -= other.f_;
            }


            public static DiyFp Minus(ref DiyFp a, ref DiyFp b)
            {
                DiyFp result = a;
                result.Subtract(ref b);
                return result;
            }

            void Multiply(ref DiyFp other)
            {
                const ulong kM32 = 0xFFFFFFFFU;
                ulong a = f_ >> 32;
                ulong b = f_ & kM32;
                ulong c = other.f_ >> 32;
                ulong d = other.f_ & kM32;
                ulong ac = a * c;
                ulong bc = b * c;
                ulong ad = a * d;
                ulong bd = b * d;
                ulong tmp = (bd >> 32) + (ad & kM32) + (bc & kM32);
                tmp += 1U << 31;
                ulong result_f = ac + (ad >> 32) + (bc >> 32) + (tmp >> 32);
                e_ += other.e_ + 64;
                f_ = result_f;
            }

            public static DiyFp Times(ref DiyFp a, ref DiyFp b)
            {
                DiyFp result = a;
                result.Multiply(ref b);
                return result;
            }

            public void Normalize()
            {
                ulong f = f_;
                int e = e_;

                const ulong k10MSBits = 0xFFC0000000000000;
                while ((f & k10MSBits) == 0)
                {
                    f <<= 10;
                    e -= 10;
                }
                while ((f & kUint64MSB) == 0)
                {
                    f <<= 1;
                    e--;
                }
                f_ = f;
                e_ = e;
            }

            public static DiyFp Normalize(ref DiyFp a)
            {
                DiyFp result = a;
                result.Normalize();
                return result;
            }

            public ulong f() { return f_; }
            public int e() { return e_; }

            public void set_f(ulong new_value) { f_ = new_value; }
            public void set_e(int new_value) { e_ = new_value; }


        }

        struct Double
        {
            ulong d64_;

            public const ulong kSignMask = 0x8000000000000000;
            public const ulong kExponentMask = 0x7FF0000000000000;
            public const ulong kSignificandMask = 0x000FFFFFFFFFFFFF;
            public const ulong kHiddenBit = 0x0010000000000000;
            public const int kPhysicalSignificandSize = 52;
            public const int kSignificandSize = 53;

            public Double(double d) : this((ulong)BitConverter.DoubleToInt64Bits(d)) { }
            public Double(ulong d64) { d64_ = d64; }
            public Double(DiyFp diy_fp) : this(DiyFpToUint64(diy_fp)) { }

            public DiyFp AsDiyFp()
            {
                return new DiyFp(Significand(), Exponent());
            }

            public DiyFp AsNormalizedDiyFp()
            {
                ulong f = Significand();
                int e = Exponent();

                while ((f & kHiddenBit) == 0)
                {
                    f <<= 1;
                    e--;
                }

                f <<= DiyFp.kSignificandSize - kSignificandSize;
                e -= DiyFp.kSignificandSize - kSignificandSize;
                return new DiyFp(f, e);
            }

            ulong AsUint64()
            {
                return d64_;
            }

            double NextDouble()
            {
                if (d64_ == kInfinity) return new Double(kInfinity).value();
                if (Sign() < 0 && Significand() == 0)
                {
                    // -0.0
                    return 0.0;
                }
                if (Sign() < 0)
                {
                    return new Double(d64_ - 1).value();
                }
                else
                {
                    return new Double(d64_ + 1).value();
                }
            }

            double PreviousDouble()
            {
                if (d64_ == (kInfinity | kSignMask)) return -Double.Infinity();
                if (Sign() < 0)
                {
                    return new Double(d64_ + 1).value();
                }
                else
                {
                    if (Significand() == 0) return -0.0;
                    return new Double(d64_ - 1).value();
                }
            }

            int Exponent()
            {
                if (IsDenormal()) return kDenormalExponent;

                ulong d64 = AsUint64();
                int biased_e = (int)((d64 & kExponentMask) >> kPhysicalSignificandSize);
                return biased_e - kExponentBias;
            }

            ulong Significand()
            {
                ulong d64 = AsUint64();
                ulong significand = d64 & kSignificandMask;
                if (!IsDenormal())
                {
                    return significand + kHiddenBit;
                }
                else
                {
                    return significand;
                }
            }

            bool IsDenormal()
            {
                ulong d64 = AsUint64();
                return (d64 & kExponentMask) == 0;
            }

            bool IsSpecial()
            {
                ulong d64 = AsUint64();
                return (d64 & kExponentMask) == kExponentMask;
            }

            bool IsNan()
            {
                ulong d64 = AsUint64();
                return ((d64 & kExponentMask) == kExponentMask) &&
                    ((d64 & kSignificandMask) != 0);
            }

            bool IsInfinite()
            {
                ulong d64 = AsUint64();
                return ((d64 & kExponentMask) == kExponentMask) &&
                    ((d64 & kSignificandMask) == 0);
            }

            public int Sign()
            {
                ulong d64 = AsUint64();
                return (d64 & kSignMask) == 0 ? 1 : -1;
            }

            DiyFp UpperBoundary()
            {
                return new DiyFp(Significand() * 2 + 1, Exponent() - 1);
            }

            public void NormalizedBoundaries(out DiyFp out_m_minus, out DiyFp out_m_plus)
            {
                DiyFp v = this.AsDiyFp();
                var temp = new DiyFp((v.f() << 1) + 1, v.e() - 1);
                DiyFp m_plus = DiyFp.Normalize(ref temp);
                DiyFp m_minus;
                if (LowerBoundaryIsCloser())
                {
                    m_minus = new DiyFp((v.f() << 2) - 1, v.e() - 2);
                }
                else
                {
                    m_minus = new DiyFp((v.f() << 1) - 1, v.e() - 1);
                }
                m_minus.set_f(m_minus.f() << (m_minus.e() - m_plus.e()));
                m_minus.set_e(m_plus.e());
                out_m_plus = m_plus;
                out_m_minus = m_minus;
            }

            bool LowerBoundaryIsCloser()
            {
                bool physical_significand_is_zero = ((AsUint64() & kSignificandMask) == 0);
                return physical_significand_is_zero && (Exponent() != kDenormalExponent);
            }

            double value() { return BitConverter.Int64BitsToDouble((long)d64_); }

            static int SignificandSizeForOrderOfMagnitude(int order)
            {
                if (order >= (kDenormalExponent + kSignificandSize))
                {
                    return kSignificandSize;
                }
                if (order <= kDenormalExponent) return 0;
                return order - kDenormalExponent;
            }

            static double Infinity()
            {
                return new Double(kInfinity).value();
            }

            static double NaN()
            {
                return new Double(kNaN).value();
            }

            const int kExponentBias = 0x3FF + kPhysicalSignificandSize;
            const int kDenormalExponent = -kExponentBias + 1;
            const int kMaxExponent = 0x7FF - kExponentBias;
            const ulong kInfinity = (0x7FF0000000000000);
            const ulong kNaN = (0x7FF8000000000000);

            static ulong DiyFpToUint64(DiyFp diy_fp)
            {
                ulong significand = diy_fp.f();
                int exponent = diy_fp.e();
                while (significand > kHiddenBit + kSignificandMask)
                {
                    significand >>= 1;
                    exponent++;
                }
                if (exponent >= kMaxExponent)
                {
                    return kInfinity;
                }
                if (exponent < kDenormalExponent)
                {
                    return 0;
                }
                while (exponent > kDenormalExponent && (significand & kHiddenBit) == 0)
                {
                    significand <<= 1;
                    exponent--;
                }
                ulong biased_exponent;
                if (exponent == kDenormalExponent && (significand & kHiddenBit) == 0)
                {
                    biased_exponent = 0;
                }
                else
                {
                    biased_exponent = (ulong)(exponent + kExponentBias);
                }
                return (significand & kSignificandMask) |
                    (biased_exponent << kPhysicalSignificandSize);
            }
        }

        const int kMinimalTargetExponent = -60;
        const int kMaximalTargetExponent = -32;

        struct CachedPower
        {
            public ulong significand;
            public short binary_exponent;
            public short decimal_exponent;

            public CachedPower(ulong s, short b, short d)
            {
                significand = s;
                binary_exponent = b;
                decimal_exponent = d;
            }
        }

        static readonly CachedPower[] kCachedPowers =
            new[] {
              new CachedPower(0xfa8fd5a0081c0288, -1220, -348),
              new CachedPower(0xbaaee17fa23ebf76, -1193, -340),
              new CachedPower(0x8b16fb203055ac76, -1166, -332),
              new CachedPower(0xcf42894a5dce35ea, -1140, -324),
              new CachedPower(0x9a6bb0aa55653b2d, -1113, -316),
              new CachedPower(0xe61acf033d1a45df, -1087, -308),
              new CachedPower(0xab70fe17c79ac6ca, -1060, -300),
              new CachedPower(0xff77b1fcbebcdc4f, -1034, -292),
              new CachedPower(0xbe5691ef416bd60c, -1007, -284),
              new CachedPower(0x8dd01fad907ffc3c, -980, -276),
              new CachedPower(0xd3515c2831559a83, -954, -268),
              new CachedPower(0x9d71ac8fada6c9b5, -927, -260),
              new CachedPower(0xea9c227723ee8bcb, -901, -252),
              new CachedPower(0xaecc49914078536d, -874, -244),
              new CachedPower(0x823c12795db6ce57, -847, -236),
              new CachedPower(0xc21094364dfb5637, -821, -228),
              new CachedPower(0x9096ea6f3848984f, -794, -220),
              new CachedPower(0xd77485cb25823ac7, -768, -212),
              new CachedPower(0xa086cfcd97bf97f4, -741, -204),
              new CachedPower(0xef340a98172aace5, -715, -196),
              new CachedPower(0xb23867fb2a35b28e, -688, -188),
              new CachedPower(0x84c8d4dfd2c63f3b, -661, -180),
              new CachedPower(0xc5dd44271ad3cdba, -635, -172),
              new CachedPower(0x936b9fcebb25c996, -608, -164),
              new CachedPower(0xdbac6c247d62a584, -582, -156),
              new CachedPower(0xa3ab66580d5fdaf6, -555, -148),
              new CachedPower(0xf3e2f893dec3f126, -529, -140),
              new CachedPower(0xb5b5ada8aaff80b8, -502, -132),
              new CachedPower(0x87625f056c7c4a8b, -475, -124),
              new CachedPower(0xc9bcff6034c13053, -449, -116),
              new CachedPower(0x964e858c91ba2655, -422, -108),
              new CachedPower(0xdff9772470297ebd, -396, -100),
              new CachedPower(0xa6dfbd9fb8e5b88f, -369, -92),
              new CachedPower(0xf8a95fcf88747d94, -343, -84),
              new CachedPower(0xb94470938fa89bcf, -316, -76),
              new CachedPower(0x8a08f0f8bf0f156b, -289, -68),
              new CachedPower(0xcdb02555653131b6, -263, -60),
              new CachedPower(0x993fe2c6d07b7fac, -236, -52),
              new CachedPower(0xe45c10c42a2b3b06, -210, -44),
              new CachedPower(0xaa242499697392d3, -183, -36),
              new CachedPower(0xfd87b5f28300ca0e, -157, -28),
              new CachedPower(0xbce5086492111aeb, -130, -20),
              new CachedPower(0x8cbccc096f5088cc, -103, -12),
              new CachedPower(0xd1b71758e219652c, -77, -4),
              new CachedPower(0x9c40000000000000, -50, 4),
              new CachedPower(0xe8d4a51000000000, -24, 12),
              new CachedPower(0xad78ebc5ac620000, 3, 20),
              new CachedPower(0x813f3978f8940984, 30, 28),
              new CachedPower(0xc097ce7bc90715b3, 56, 36),
              new CachedPower(0x8f7e32ce7bea5c70, 83, 44),
              new CachedPower(0xd5d238a4abe98068, 109, 52),
              new CachedPower(0x9f4f2726179a2245, 136, 60),
              new CachedPower(0xed63a231d4c4fb27, 162, 68),
              new CachedPower(0xb0de65388cc8ada8, 189, 76),
              new CachedPower(0x83c7088e1aab65db, 216, 84),
              new CachedPower(0xc45d1df942711d9a, 242, 92),
              new CachedPower(0x924d692ca61be758, 269, 100),
              new CachedPower(0xda01ee641a708dea, 295, 108),
              new CachedPower(0xa26da3999aef774a, 322, 116),
              new CachedPower(0xf209787bb47d6b85, 348, 124),
              new CachedPower(0xb454e4a179dd1877, 375, 132),
              new CachedPower(0x865b86925b9bc5c2, 402, 140),
              new CachedPower(0xc83553c5c8965d3d, 428, 148),
              new CachedPower(0x952ab45cfa97a0b3, 455, 156),
              new CachedPower(0xde469fbd99a05fe3, 481, 164),
              new CachedPower(0xa59bc234db398c25, 508, 172),
              new CachedPower(0xf6c69a72a3989f5c, 534, 180),
              new CachedPower(0xb7dcbf5354e9bece, 561, 188),
              new CachedPower(0x88fcf317f22241e2, 588, 196),
              new CachedPower(0xcc20ce9bd35c78a5, 614, 204),
              new CachedPower(0x98165af37b2153df, 641, 212),
              new CachedPower(0xe2a0b5dc971f303a, 667, 220),
              new CachedPower(0xa8d9d1535ce3b396, 694, 228),
              new CachedPower(0xfb9b7cd9a4a7443c, 720, 236),
              new CachedPower(0xbb764c4ca7a44410, 747, 244),
              new CachedPower(0x8bab8eefb6409c1a, 774, 252),
              new CachedPower(0xd01fef10a657842c, 800, 260),
              new CachedPower(0x9b10a4e5e9913129, 827, 268),
              new CachedPower(0xe7109bfba19c0c9d, 853, 276),
              new CachedPower(0xac2820d9623bf429, 880, 284),
              new CachedPower(0x80444b5e7aa7cf85, 907, 292),
              new CachedPower(0xbf21e44003acdd2d, 933, 300),
              new CachedPower(0x8e679c2f5e44ff8f, 960, 308),
              new CachedPower(0xd433179d9c8cb841, 986, 316),
              new CachedPower(0x9e19db92b4e31ba9, 1013, 324),
              new CachedPower(0xeb96bf6ebadf77d9, 1039, 332),
              new CachedPower(0xaf87023b9bf0ee6b, 1066, 340),
            };

        const int kCachedPowersLength = 87;
        const int kCachedPowersOffset = 348;
        const double kD_1_LOG2_10 = 0.30102999566398114;
        const int kDecimalExponentDistance = 8;
        const int kMinDecimalExponent = -348;
        const int kMaxDecimalExponent = 340;

        static void GetCachedPowerForBinaryExponentRange(int min_exponent, int max_exponent, out DiyFp power, out int decimal_exponent)
        {
            int kQ = DiyFp.kSignificandSize;
            double k = Math.Ceiling((min_exponent + kQ - 1) * kD_1_LOG2_10);
            int foo = kCachedPowersOffset;
            int index =
                (foo + (int)(k) - 1) / kDecimalExponentDistance + 1;
            CachedPower cached_power = kCachedPowers[index];
            decimal_exponent = cached_power.decimal_exponent;
            power = new DiyFp(cached_power.significand, cached_power.binary_exponent);
        }


        static void GetCachedPowerForDecimalExponent(int requested_exponent, out DiyFp power, out int found_exponent)
        {
            int index =
                (requested_exponent + kCachedPowersOffset) / kDecimalExponentDistance;
            CachedPower cached_power = kCachedPowers[index];
            power = new DiyFp(cached_power.significand, cached_power.binary_exponent);
            found_exponent = cached_power.decimal_exponent;
        }

        static bool Grisu3(double v, char[] buffer, out int length, out int decimal_exponent)
        {
            DiyFp w = new Double(v).AsNormalizedDiyFp();

            DiyFp boundary_minus, boundary_plus;
            new Double(v).NormalizedBoundaries(out boundary_minus, out boundary_plus);

            DiyFp ten_mk;
            int mk;
            int ten_mk_minimal_binary_exponent =
               kMinimalTargetExponent - (w.e() + DiyFp.kSignificandSize);
            int ten_mk_maximal_binary_exponent =
               kMaximalTargetExponent - (w.e() + DiyFp.kSignificandSize);
            GetCachedPowerForBinaryExponentRange(
                ten_mk_minimal_binary_exponent,
                ten_mk_maximal_binary_exponent,
                out ten_mk, out mk);

            DiyFp scaled_w = DiyFp.Times(ref w, ref ten_mk);

            DiyFp scaled_boundary_minus = DiyFp.Times(ref boundary_minus, ref ten_mk);
            DiyFp scaled_boundary_plus = DiyFp.Times(ref boundary_plus, ref ten_mk);

            int kappa;
            bool result = DigitGen(scaled_boundary_minus, scaled_w, scaled_boundary_plus, buffer, out length, out kappa);
            decimal_exponent = -mk + kappa;
            return result;
        }

        static readonly uint[] kSmallPowersOfTen = new uint[] { 0, 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };

        static void BiggestPowerTen(uint number, int number_bits, out uint power, out int exponent_plus_one)
        {
            int exponent_plus_one_guess = ((number_bits + 1) * 1233 >> 12);
            exponent_plus_one_guess++;
            while (number < kSmallPowersOfTen[exponent_plus_one_guess])
            {
                exponent_plus_one_guess--;
            }
            power = kSmallPowersOfTen[exponent_plus_one_guess];
            exponent_plus_one = exponent_plus_one_guess;
        }

        static bool RoundWeed(char[] buffer, int length, ulong distance_too_high_w, ulong unsafe_interval, ulong rest, ulong ten_kappa, ulong unit)
        {
            ulong small_distance = distance_too_high_w - unit;
            ulong big_distance = distance_too_high_w + unit;

            while (rest < small_distance && unsafe_interval - rest >= ten_kappa && (rest + ten_kappa < small_distance || small_distance - rest >= rest + ten_kappa - small_distance))
            {
                buffer[length - 1]--;
                rest += ten_kappa;
            }

            if (rest < big_distance &&
                unsafe_interval - rest >= ten_kappa &&
                (rest + ten_kappa < big_distance ||
                 big_distance - rest > rest + ten_kappa - big_distance))
            {
                return false;
            }

            return (2 * unit <= rest) && (rest <= unsafe_interval - 4 * unit);
        }

        static bool DigitGen(DiyFp low, DiyFp w, DiyFp high, char[] buffer, out int length, out int kappa)
        {
            ulong unit = 1;
            DiyFp too_low = new DiyFp(low.f() - unit, low.e());
            DiyFp too_high = new DiyFp(high.f() + unit, high.e());
            DiyFp unsafe_interval = DiyFp.Minus(ref too_high, ref too_low);
            DiyFp one = new DiyFp(((ulong)1) << -w.e(), w.e());
            uint integrals = (uint)(too_high.f() >> -one.e());
            ulong fractionals = too_high.f() & (one.f() - 1);
            uint divisor;
            int divisor_exponent_plus_one;
            BiggestPowerTen(integrals, DiyFp.kSignificandSize - (-one.e()), out divisor, out divisor_exponent_plus_one);
            kappa = divisor_exponent_plus_one;
            length = 0;

            while (kappa > 0)
            {
                uint digit = integrals / divisor;
                buffer[length] = (char)('0' + digit);
                (length)++;
                integrals %= divisor;
                (kappa)--;
                ulong rest =
                    (((ulong)integrals) << -one.e()) + fractionals;
                if (rest < unsafe_interval.f())
                {
                    return RoundWeed(buffer, length, DiyFp.Minus(ref too_high, ref w).f(),
                                     unsafe_interval.f(), rest,
                                     ((ulong)divisor) << -one.e(), unit);
                }
                divisor /= 10;
            }

            while (true)
            {
                fractionals *= 10;
                unit *= 10;
                unsafe_interval.set_f(unsafe_interval.f() * 10);
                // Integer division by one.
                int digit = (int)(fractionals >> -one.e());
                buffer[length] = (char)('0' + digit);
                (length)++;
                fractionals &= one.f() - 1;  // Modulo by one.
                (kappa)--;
                if (fractionals < unsafe_interval.f())
                {
                    return RoundWeed(buffer, length, DiyFp.Minus(ref too_high, ref w).f() * unit,
                                     unsafe_interval.f(), fractionals, one.f(), unit);
                }
            }
        }
    }
}