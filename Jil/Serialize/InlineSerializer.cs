using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;
using Jil.SerializeDynamic;

namespace Jil.Serialize
{
    internal static class InlineSerializer
    {
#if COREFX
        internal static readonly MethodInfo TypeGetProperty = typeof(TypeExtensions)._GetPublicStaticMethod(nameof(TypeExtensions.GetProperty), new[] { typeof(Type), typeof(string), typeof(BindingFlags) }).AssertNotNull(nameof(TypeExtensions.GetProperty));
#else
        internal static readonly MethodInfo TypeGetProperty = typeof(Type).GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(BindingFlags) }).AssertNotNull(nameof(Type.GetProperty));
#endif

        internal static readonly MethodInfo TypeFromHandle = typeof(Type)._GetPublicStaticMethod(nameof(Type.GetTypeFromHandle), new[] { typeof(RuntimeTypeHandle) }).AssertNotNull(nameof(Type.GetTypeFromHandle));
        internal static readonly MethodInfo FieldInfoFromHandle = typeof(FieldInfo)._GetPublicStaticMethod(nameof(FieldInfo.GetFieldFromHandle), new[] { typeof(RuntimeFieldHandle), typeof(RuntimeTypeHandle) }).AssertNotNull(nameof(FieldInfo.GetFieldFromHandle));
        internal static readonly int LowestCharNeedingEncoding;
        internal static readonly Tuple<int, string>[] CharEncodingLabelsNeeded;
        static InlineSerializer()
        {
            Dictionary<char, string> characterEscapes =
            new Dictionary<char, string>
            {
                { '\\',  @"\\" },
                { '"', @"\""" },
                { '\u0000', @"\u0000" },
                { '\u0001', @"\u0001" },
                { '\u0002', @"\u0002" },
                { '\u0003', @"\u0003" },
                { '\u0004', @"\u0004" },
                { '\u0005', @"\u0005" },
                { '\u0006', @"\u0006" },
                { '\u0007', @"\u0007" },
                { '\u0008', @"\b" },
                { '\u0009', @"\t" },
                { '\u000A', @"\n" },
                { '\u000B', @"\u000B" },
                { '\u000C', @"\f" },
                { '\u000D', @"\r" },
                { '\u000E', @"\u000E" },
                { '\u000F', @"\u000F" },
                { '\u0010', @"\u0010" },
                { '\u0011', @"\u0011" },
                { '\u0012', @"\u0012" },
                { '\u0013', @"\u0013" },
                { '\u0014', @"\u0014" },
                { '\u0015', @"\u0015" },
                { '\u0016', @"\u0016" },
                { '\u0017', @"\u0017" },
                { '\u0018', @"\u0018" },
                { '\u0019', @"\u0019" },
                { '\u001A', @"\u001A" },
                { '\u001B', @"\u001B" },
                { '\u001C', @"\u001C" },
                { '\u001D', @"\u001D" },
                { '\u001E', @"\u001E" },
                { '\u001F', @"\u001F" }
            };
            LowestCharNeedingEncoding = characterEscapes.Keys.Min(c => (int)c);
            CharEncodingLabelsNeeded = characterEscapes.Select(kv => Tuple.Create(kv.Key - LowestCharNeedingEncoding, kv.Value)).ToArray();
            Array.Sort(CharEncodingLabelsNeeded, (x, y) => x.Item1.CompareTo(y.Item1));
        }
    }
    class InlineSerializer<ForType>
    {
        public static bool ReorderMembers = true;
        public static bool UseCustomIntegerToString = true;
        public static bool SkipDateTimeMathMethods = true;
        public static bool UseCustomISODateFormatting = true;
        public static bool UseFastLists = true;
        public static bool UseFastArrays = true;
        public static bool UseFastGuids = true;
        public static bool AllocationlessDictionaries = true;
        public static bool PropagateConstants = true;
        public static bool UseCustomWriteIntUnrolled = true;
        public static bool UseCustomRFC1123DateTimeFormatting = true;

        static string CharBuffer = "char_buffer";
        internal const int CharBufferSize = 36;
        internal const int RecursionLimit = 50;

        private readonly Type RecursionLookupOptionsType; // This is a type that implements ISerializeOptions and has an empty, public constructor
        private readonly bool ExcludeNulls;
        private readonly bool PrettyPrint;
        private readonly bool JSONP;
        private readonly DateTimeFormat DateFormat;
        private readonly bool IncludeInherited;
        private readonly UnspecifiedDateTimeKindBehavior UnspecifiedDateTimeBehavior;
        private readonly SerializationNameFormat SerializationNameFormat;

        private Dictionary<Type, Sigil.Local> RecursiveTypes;

        private Emit Emit;

        private readonly bool CallOutOnPossibleDynamic;

        private readonly bool BuildingToString;

        private readonly Stack<Type> WritingDynamicObject;

        internal InlineSerializer(Type recursionLookupOptionsType, bool pretty, bool excludeNulls, bool jsonp, DateTimeFormat dateFormat, bool includeInherited, UnspecifiedDateTimeKindBehavior dateTimeBehavior, SerializationNameFormat serializationNameFormat, bool callOutOnPossibleDynamic, bool buildToString)
        {
            RecursionLookupOptionsType = recursionLookupOptionsType;
            PrettyPrint = pretty;
            ExcludeNulls = excludeNulls;
            JSONP = jsonp;
            DateFormat = dateFormat;
            IncludeInherited = includeInherited;
            UnspecifiedDateTimeBehavior = dateTimeBehavior;
            SerializationNameFormat = serializationNameFormat;

            CallOutOnPossibleDynamic = callOutOnPossibleDynamic;

            BuildingToString = buildToString;

            if (CallOutOnPossibleDynamic)
            {
                WritingDynamicObject = new Stack<Type>();
            }
        }

        void LoadProperty(PropertyInfo prop)
        {
            var getMtd = prop.GetMethod;

            if (getMtd.IsVirtual)
            {
                Emit.CallVirtual(getMtd);
            }
            else
            {
                Emit.Call(getMtd);
            }
        }

        static MethodInfo TextWriter_WriteString = typeof(TextWriter).GetMethod(nameof(TextWriter.Write), new [] { typeof(string) }).AssertNotNull(nameof(TextWriter.Write));
        static MethodInfo ThunkWriter_WriteString = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.Write), new[] { typeof(string) }).AssertNotNull(nameof(ThunkWriter.Write));
        static MethodInfo ThunkWriter_WriteCommonConstant = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.WriteCommonConstant), new[] { typeof(ConstantString_Common) }).AssertNotNull(nameof(ThunkWriter.WriteCommonConstant));
        static MethodInfo ThunkWriter_WriteFormatingContant = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.WriteFormattingConstant), new[] { typeof(ConstantString_Formatting) }).AssertNotNull(nameof(ThunkWriter.WriteFormattingConstant));
        static MethodInfo ThunkWriter_WriteMinConstant = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.WriteMinConstant), new[] { typeof(ConstantString_Min) }).AssertNotNull(nameof(ThunkWriter.WriteMinConstant));
        static MethodInfo ThunkWriter_WriteValueConstant = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.WriteValueConstant), new[] { typeof(ConstantString_Value) }).AssertNotNull(nameof(ThunkWriter.WriteValueConstant));
        static MethodInfo ThunkWriter_Write000EscapeConstant = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.Write000EscapeConstant), new[] { typeof(ConstantString_000Escape) }).AssertNotNull(nameof(ThunkWriter.Write000EscapeConstant));
        static MethodInfo ThunkWriter_Write001EscapeConstant = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.Write001EscapeConstant), new[] { typeof(ConstantString_001Escape) }).AssertNotNull(nameof(ThunkWriter.Write001EscapeConstant));
        static MethodInfo ThunkWriter_WriteDayOfWeek = typeof(ThunkWriter).GetMethod(nameof(ThunkWriter.WriteDayOfWeek), new[] { typeof(ConstantString_DaysOfWeek) }).AssertNotNull(nameof(ThunkWriter.WriteDayOfWeek));
        void WriteString(string str)
        {
            if (BuildingToString)
            {
                Emit.LoadArgument(0);                       // ThunkWriter*

                ConstantString_Common constStr;

                if (ThunkWriter.IsConstantCommonString(str, out constStr))
                {
                    Emit.LoadConstant((int)constStr);           // ThunkWriter* int
                    Emit.Call(ThunkWriter_WriteCommonConstant);       // --empty--
                }
                else
                {
                    ConstantString_Formatting formattingConstString;
                    if (ThunkWriter.IsConstantFormattingString(str, out formattingConstString))
                    {
                        Emit.LoadConstant((ushort)formattingConstString);   // ThinkWriter* ushort
                        Emit.Call(ThunkWriter_WriteFormatingContant);       // --empty--
                    }
                    else
                    {
                        ConstantString_Min minConstString;
                        if (ThunkWriter.IsConstantMinString(str, out minConstString))
                        {
                            Emit.LoadConstant((ushort)minConstString);      // ThunkWriter* ushort
                            Emit.Call(ThunkWriter_WriteMinConstant);        // --empty--
                        }
                        else
                        {
                            ConstantString_Value valConstString;
                            if (ThunkWriter.IsConstantValueString(str, out valConstString))
                            {
                                Emit.LoadConstant((ushort)valConstString);  // ThunkWriter* ushort
                                Emit.Call(ThunkWriter_WriteValueConstant);  // --empty--
                            }
                            else
                            {
                                ConstantString_000Escape e000ConstString;
                                if (ThunkWriter.IsConstant000EscapeString(str, out e000ConstString))
                                {
                                    Emit.LoadConstant((byte)e000ConstString);       // ThunkWriter* byte
                                    Emit.Call(ThunkWriter_Write000EscapeConstant);  // --empty--
                                }
                                else
                                {
                                    ConstantString_001Escape e001ConstString;
                                    if (ThunkWriter.IsConstant001EscapeString(str, out e001ConstString))
                                    {
                                        Emit.LoadConstant((byte)e001ConstString);       // ThunkWriter* byte
                                        Emit.Call(ThunkWriter_Write001EscapeConstant);  // --empty--
                                    }
                                    else
                                    {
                                        ConstantString_DaysOfWeek dayOfWeek;
                                        if (ThunkWriter.IsConstantDaysOfWeek(str, out dayOfWeek))
                                        {
                                            Emit.LoadConstant((byte)dayOfWeek);         // ThunkWriter* byte
                                            Emit.Call(ThunkWriter_WriteDayOfWeek);      // --empty--
                                        }
                                        else
                                        {
                                            Emit.LoadConstant(str);                     // ThunkWriter* string
                                            Emit.Call(ThunkWriter_WriteString);         // --empty--
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Emit.LoadArgument(0);                       // TextWriter
                Emit.LoadConstant(str);                     // TextWriter string
                Emit.CallVirtual(TextWriter_WriteString);   // --empty--
            }
        }

        void LineBreakAndIndent()
        {
            const int precalcLimit = 10;

            var done = Emit.DefineLabel();

            var labels = Enumerable.Range(0, precalcLimit).Select(i => Emit.DefineLabel()).ToArray();

            Emit.LoadArgument(2);   // int
            Emit.Switch(labels);    // --empty--

            // default case
            using (var count = Emit.DeclareLocal<int>())
            {
                WriteString("\n");

                var loop = Emit.DefineLabel();

                Emit.LoadArgument(2);       // int
                Emit.StoreLocal(count);     // --empty--

                Emit.MarkLabel(loop);
                Emit.LoadLocal(count);      // int
                Emit.BranchIfFalse(done);   // --empty--

                WriteString(" ");           // --empty--
                Emit.LoadLocal(count);      // int
                Emit.LoadConstant(-1);      // int -1
                Emit.Add();                 // (int-1)
                Emit.StoreLocal(count);     // --empty-
                Emit.Branch(loop);
            }

            for (var i = 0; i < labels.Length; i++)
            {
                var breakAndIndent = "\n" + new string(' ', i);

                Emit.MarkLabel(labels[i]);      // --empty--
                WriteString(breakAndIndent);    // --empty--
                Emit.Branch(done);              // --empty--
            }

            Emit.MarkLabel(done);               // --empty--
        }

        void IncreaseIndent()
        {
            // We only need to track this if
            //   - we're pretty printing
            //   - or infinite recursion is possible
            if (PrettyPrint || RecursiveTypes.Count != 0)
            {
                Emit.LoadArgument(2);   // indent
                Emit.LoadConstant(1);   // indent 1
                Emit.Add();             // (indent+1)
                Emit.StoreArgument(2);  // --empty--
            }
        }

        void DecreaseIndent()
        {
            // We only need to track this if
            //   - we're pretty printing
            //   - or infinite recursion is possible
            if (PrettyPrint || RecursiveTypes.Count != 0)
            {
                Emit.LoadArgument(2);   // indent
                Emit.LoadConstant(-1);   // indent -1
                Emit.Add();             // (indent-1)
                Emit.StoreArgument(2);  // --empty--
            }

            if (PrettyPrint)
            {
                LineBreakAndIndent();
            }
        }

        List<MemberInfo> OrderMembersForAccess(Type forType, Dictionary<Type, Sigil.Local> recursiveTypes)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;

            if (!IncludeInherited)
            {
                flags |= BindingFlags.DeclaredOnly;
            }

            var props = forType.GetProperties(flags).Where(p => p.GetMethod != null && p.GetIndexParameters().Length == 0);
            var fields = forType.GetFields(flags);

            var members = props.Cast<MemberInfo>().Concat(fields).Where(f => f.ShouldUseMember());

            if (forType._IsValueType())
            {
                return members.ToList();
            }

            // This order appears to be the "best" for access speed purposes
            var ret =
                !ReorderMembers ?
                    members :
                    Utils.IdealMemberOrderForWriting(forType, recursiveTypes.Keys, members);

            return ret.ToList();
        }

        void WriteConstantMember(MemberInfo member, bool prependComma)
        {
            string stringEquiv;

            if (PrettyPrint)
            {
                stringEquiv = "\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\": ";
            }
            else
            {
                stringEquiv = "\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\":";
            }

            if (prependComma)
            {
                stringEquiv = "," + stringEquiv;
            }

            stringEquiv += member.GetConstantJSONStringEquivalent(JSONP);

            WriteString(stringEquiv);
        }

        void WriteMember(MemberInfo member, Sigil.Local inLocal = null)
        {
            // Stack is empty

            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new ConstructionException("Encountered a serializable member that is neither a field nor a property: " + member);

            var serializingType = asField != null ? asField.FieldType : asProp.PropertyType;

            // It's a list or dictionary, go and build that code
            if (serializingType.IsListType() || serializingType.IsDictionaryType() || serializingType.IsReadOnlyListType() || serializingType.IsReadOnlyDictionaryType())
            {
                if (inLocal != null)
                {
                    Emit.LoadLocal(inLocal);
                }
                else
                {
                    Emit.LoadArgument(1);
                }

                if (asField != null)
                {
                    Emit.LoadField(asField);
                }

                if (asProp != null)
                {
                    LoadProperty(asProp);
                }

                using (var loc = Emit.DeclareLocal(serializingType))
                {
                    Emit.StoreLocal(loc);

                    if (serializingType.IsListType() || serializingType.IsReadOnlyListType())
                    {
                        WriteList(member, serializingType, loc);
                        return;
                    }

                    if (serializingType.IsDictionaryType() || serializingType.IsReadOnlyDictionaryType())
                    {
                        WriteDictionary(member, serializingType, loc);
                        return;
                    }

                    throw new Exception("Shouldn't be possible");
                }
            }

            var isRecursive = RecursiveTypes.ContainsKey(serializingType);

            if (isRecursive)
            {
                Emit.LoadLocal(RecursiveTypes[serializingType]);    // Action<TextWriter, serializingType>
            }

            // Only put this on the stack if we'll need it
            var preloadTextWriter = 
                serializingType.IsPrimitiveType() ||
                (serializingType._IsEnum() && member.ShouldConvertEnum(serializingType)) ||
                isRecursive || 
                serializingType.IsNullableType();
            if (preloadTextWriter)
            {
                Emit.LoadArgument(0);   // TextWriter
            }

            if (inLocal == null)
            {
                if (member.DeclaringType._IsValueType())
                {
                    Emit.LoadArgumentAddress(1);    // TextWriter obj*
                }
                else
                {
                    Emit.LoadArgument(1);           // TextWriter obj
                }
            }
            else
            {
                if (inLocal.LocalType._IsValueType())
                {
                    Emit.LoadLocalAddress(inLocal); // TextWriter obj*
                }
                else
                {
                    Emit.LoadLocal(inLocal);        // TextWriter obj
                }
            }

            if (asField != null)
            {
                Emit.LoadField(asField);    // TextWriter field
            }

            if (asProp != null)
            {
                LoadProperty(asProp);       // TextWriter prop
            }

            if (isRecursive)
            {
                // Stack is:
                //  - serializingType
                //  - TextWriter
                //  - Action<TextWriter, serializingType>

                Emit.LoadArgument(2);   // Action<> TextWriter serializingType int

                Type recursiveAct;
                if (BuildingToString)
                {
                    recursiveAct = typeof(StringThunkDelegate<>).MakeGenericType(serializingType);
                }
                else
                {
                    recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), serializingType, typeof(int));
                }

                var invoke = recursiveAct.GetMethod("Invoke");

                Emit.Call(invoke);

                return;
            }

            if (serializingType.IsPrimitiveType())
            {
                WritePrimitive(serializingType, quotesNeedHandling: true);
                return;
            }

            if (serializingType.IsNullableType())
            {
                WriteNullable(member, serializingType, quotesNeedHandling: true);
                return;
            }

            if (serializingType._IsEnum())
            {
                WriteEnumOrPrimitive(member, serializingType, requiresQuotes: false, hasTextWriter: true, popTextWriter: false);
                return;
            }

            using (var loc = Emit.DeclareLocal(serializingType))
            {
                Emit.StoreLocal(loc);   // TextWriter;

                if (serializingType.IsEnumerableType())
                {
                    WriteEnumerable(member, serializingType, loc);
                    return;
                }

                WriteObject(serializingType, loc);
            }
        }

        void WriteNullable(MemberInfo nullableMember, Type nullableType, bool quotesNeedHandling)
        {
            // Top of stack is
            //  - nullable
            //  - TextWriter

            var hasValue = nullableType.GetProperty("HasValue");
            var value = nullableType.GetProperty("Value");
            var underlyingType = nullableType.GetUnderlyingType();
            var done = Emit.DefineLabel();

            using (var loc = Emit.DeclareLocal(nullableType))
            {
                var notNull = Emit.DefineLabel();

                Emit.StoreLocal(loc);           // TextWriter
                Emit.LoadLocalAddress(loc);     // TextWriter nullableType*
                LoadProperty(hasValue);         // TextWriter bool
                Emit.BranchIfTrue(notNull);     // TextWriter

                Emit.Pop();                 // --empty--
                WriteString("null");        // --empty--
                Emit.Branch(done);          // --empty--

                Emit.MarkLabel(notNull);    // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter nullableType*
                LoadProperty(value);        // TextValue value
            }

            if (underlyingType.IsPrimitiveType())
            {
                WritePrimitive(underlyingType, quotesNeedHandling);
            }
            else
            {
                if (underlyingType._IsEnum())
                {
                    WriteEnumOrPrimitive(nullableMember, underlyingType, requiresQuotes: false, hasTextWriter: true, popTextWriter: true);
                }
                else
                {
                    using (var loc = Emit.DeclareLocal(underlyingType))
                    {
                        Emit.StoreLocal(loc);   // TextWriter

                        if (RecursiveTypes.ContainsKey(underlyingType))
                        {
                            Type act;
                            if (BuildingToString)
                            {
                                act = typeof(StringThunkDelegate<>).MakeGenericType(underlyingType);
                            }
                            else
                            {
                                act = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), underlyingType, typeof(int));
                            }
                            
                            var invoke = act.GetMethod("Invoke");

                            Emit.Pop();                                     // --empty--
                            Emit.LoadLocal(RecursiveTypes[underlyingType]); // Action<TextWriter, underlyingType>
                            Emit.LoadArgument(0);                           // Action<,> TextWriter
                            Emit.LoadLocal(loc);                            // Action<,> TextWriter value
                            Emit.LoadArgument(2);                           // Action<,> TextWriter value int
                            Emit.Call(invoke);                              // --empty--
                        }
                        else
                        {
                            if (underlyingType.IsListType())
                            {
                                WriteList(nullableMember, underlyingType, loc);
                            }
                            else
                            {
                                if (underlyingType.IsDictionaryType())
                                {
                                    WriteDictionary(nullableMember, underlyingType, loc);
                                }
                                else
                                {
                                    if (underlyingType.IsEnumerableType())
                                    {
                                        WriteEnumerable(nullableMember, underlyingType, loc);
                                    }
                                    else
                                    {
                                        Emit.Pop();

                                        WriteObject(underlyingType, loc);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Emit.MarkLabel(done);
        }

        void WriteMicrosoftStyleDateTime()
        {
            // top of stack:
            //   - DateTime
            //   - TextWriter

            using (var loc = Emit.DeclareLocal<DateTime>())
            {
                Emit.StoreLocal(loc);                   // TextWriter
                Emit.LoadLocalAddress(loc);             // TextWriter DateTime*
            }

            if (!SkipDateTimeMathMethods)
            {
                var subtractMtd = typeof(DateTime).GetMethod("Subtract", new[] { typeof(DateTime) });
                var totalMs = typeof(TimeSpan).GetProperty("TotalMilliseconds");
                var dtCons = typeof(DateTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(DateTimeKind) });

                Emit.LoadConstant(1970);                    // TextWriter DateTime* 1970
                Emit.LoadConstant(1);                       // TextWriter DateTime* 1970 1
                Emit.LoadConstant(1);                       // TextWriter DateTime* 1970 1 1
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0 0
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0 0 0 
                Emit.LoadConstant((int)DateTimeKind.Utc);   // TextWriter DateTime* 1970 1 1 0 0 0 Utc
                Emit.NewObject(dtCons);                     // TextWriter DateTime* DateTime*
                Emit.Call(subtractMtd);                     // TextWriter TimeSpan

                using (var loc = Emit.DeclareLocal<TimeSpan>())
                {
                    Emit.StoreLocal(loc);                   // TextWriter
                    Emit.LoadLocalAddress(loc);             // TextWriter TimeSpan*
                }

                LoadProperty(totalMs);                      // TextWriter double
                Emit.Convert<long>();                       // TextWriter int

                WriteString("\"\\/Date(");                                  // TextWriter int
                WritePrimitive(typeof(long), quotesNeedHandling: false);    // --empty--
                WriteString(")\\/\"");                                      // --empty--

                return;
            }

            var getTicks = typeof(DateTime).GetProperty("Ticks");

            LoadProperty(getTicks);                         // TextWriter long
            Emit.LoadConstant(621355968000000000L);         // TextWriter long (Unix Epoch Ticks long)
            Emit.Subtract();                                // TextWriter long
            Emit.LoadConstant(10000L);                      // TextWriter long 10000
            Emit.Divide();                                  // TextWriter long

            WriteString("\"\\/Date(");                                  // TextWriter int
            WritePrimitive(typeof(long), quotesNeedHandling: false);    // --empty--
            WriteString(")\\/\"");                                      // --empty--
        }

        void WriteMillisecondsStyleDateTime()
        {
            using (var loc = Emit.DeclareLocal<DateTime>())
            {
                Emit.StoreLocal(loc);                   // TextWriter
                Emit.LoadLocalAddress(loc);             // TextWriter DateTime*
            }

            if (!SkipDateTimeMathMethods)
            {
                var subtractMtd = typeof(DateTime).GetMethod("Subtract", new[] { typeof(DateTime) });
                var totalMs = typeof(TimeSpan).GetProperty("TotalMilliseconds");
                var dtCons = typeof(DateTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(DateTimeKind) });

                Emit.LoadConstant(1970);                    // TextWriter DateTime* 1970
                Emit.LoadConstant(1);                       // TextWriter DateTime* 1970 1
                Emit.LoadConstant(1);                       // TextWriter DateTime* 1970 1 1
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0 0
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0 0 0 
                Emit.LoadConstant((int)DateTimeKind.Utc);   // TextWriter DateTime* 1970 1 1 0 0 0 Utc
                Emit.NewObject(dtCons);                     // TextWriter DateTime* DateTime*
                Emit.Call(subtractMtd);                     // TextWriter TimeSpan

                using (var loc = Emit.DeclareLocal<TimeSpan>())
                {
                    Emit.StoreLocal(loc);                   // TextWriter
                    Emit.LoadLocalAddress(loc);             // TextWriter TimeSpan*
                }

                LoadProperty(totalMs);                      // TextWriter double
                Emit.Convert<long>();                       // TextWriter int

                WritePrimitive(typeof(long), quotesNeedHandling: false);               // --empty--

                return;
            }

            var getTicks = typeof(DateTime).GetProperty("Ticks");

            LoadProperty(getTicks);                         // TextWriter long
            Emit.LoadConstant(621355968000000000L);         // TextWriter long (Unix Epoch Ticks long)
            Emit.Subtract();                                // TextWriter long
            Emit.LoadConstant(10000L);                      // TextWriter long 10000
            Emit.Divide();                                  // TextWriter long

            WritePrimitive(typeof(long), quotesNeedHandling: false);               // --empty--
        }

        void WriteSecondsStyleDateTime()
        {
            using (var loc = Emit.DeclareLocal<DateTime>())
            {
                Emit.StoreLocal(loc);                   // TextWriter
                Emit.LoadLocalAddress(loc);             // TextWriter DateTime*
            }

            if (!SkipDateTimeMathMethods)
            {
                var subtractMtd = typeof(DateTime).GetMethod("Subtract", new[] { typeof(DateTime) });

                var totalS = typeof(TimeSpan).GetProperty("TotalSeconds");
                var dtCons = typeof(DateTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(DateTimeKind) });

                Emit.LoadConstant(1970);                    // TextWriter DateTime* 1970
                Emit.LoadConstant(1);                       // TextWriter DateTime* 1970 1
                Emit.LoadConstant(1);                       // TextWriter DateTime* 1970 1 1
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0 0
                Emit.LoadConstant(0);                       // TextWriter DateTime* 1970 1 1 0 0 0 
                Emit.LoadConstant((int)DateTimeKind.Utc);   // TextWriter DateTime* 1970 1 1 0 0 0 Utc
                Emit.NewObject(dtCons);                     // TextWriter DateTime* DateTime*
                Emit.Call(subtractMtd);                     // TextWriter TimeSpan

                using (var loc = Emit.DeclareLocal<TimeSpan>())
                {
                    Emit.StoreLocal(loc);                   // TextWriter
                    Emit.LoadLocalAddress(loc);             // TextWriter TimeSpan*
                }

                LoadProperty(totalS);                      // TextWriter double
                Emit.Convert<long>();                       // TextWriter int

                WritePrimitive(typeof(long), quotesNeedHandling: false);               // --empty--

                return;
            }

            var getTicks = typeof(DateTime).GetProperty("Ticks");

            LoadProperty(getTicks);                         // TextWriter long
            Emit.LoadConstant(621355968000000000L);         // TextWriter long (Unix Epoch Ticks long)
            Emit.Subtract();                                // TextWriter long
            Emit.LoadConstant(10000000L);                   // TextWriter long 10000000
            Emit.Divide();                                  // TextWriter long

            WritePrimitive(typeof(long), quotesNeedHandling: false);               // --empty--
        }

        void WriteISO8601StyleDateTime()
        {
            // top of stack is
            //  - DateTime
            //  - TextWriter

            if (!UseCustomISODateFormatting)
            {
                var toString = typeof(DateTime).GetMethod("ToString", new[] { typeof(string) });
                
                using (var loc = Emit.DeclareLocal<DateTime>())
                {
                    Emit.StoreLocal(loc);       // TextWriter
                    Emit.LoadLocalAddress(loc); // TextWriter DateTime*
                }

                Emit.LoadConstant("\\\"yyyy-MM-ddTHH:mm:ssZ\\\"");      // TextWriter DateTime* string
                Emit.Call(toString);                                    // TextWriter string

                if (BuildingToString)
                {
                    Emit.Call(ThunkWriter_WriteString);                 // --empty--
                }
                else
                {
                    Emit.CallVirtual(TextWriter_WriteString);           // --empty--
                }
                return;
            }

            Emit.LoadLocal(CharBuffer);                                     // TextWriter DateTime char[]
            Emit.Call(Methods.GetCustomISO8601ToString(BuildingToString));  // --empty--
        }

        static readonly MethodInfo DateTime_ToString = typeof(DateTime).GetMethod("ToString", new[] { typeof(string) });
        void WriteRFC1123StyleDateTime()
        {
            // top of stack is
            //  - DateTime
            //  - TextWriter 

            if (!UseCustomRFC1123DateTimeFormatting)
            {
                using (var loc = Emit.DeclareLocal<DateTime>())
                {
                    Emit.StoreLocal(loc);               // TextWriter
                    Emit.LoadLocalAddress(loc);         // DateTime* TextWriter
                }

                Emit.LoadConstant("R");                                     // string DateTime* TextWriter
                Emit.Call(DateTime_ToString);                               // string TextWriter
                WritePrimitive(typeof(string), quotesNeedHandling: true);   // --empty--

                return;
            }

            Emit.Call(Methods.GetCustomRFC1123(BuildingToString));  // --empty--
        }

        static readonly MethodInfo DateTimeOffset_UtcDateTime = typeof(DateTimeOffset).GetProperty("UtcDateTime").GetMethod;
        static readonly MethodInfo DateTimeOffset_DateTime = typeof(DateTimeOffset).GetProperty("DateTime").GetMethod;
        static readonly MethodInfo DateTimeOffset_UtcTicks = typeof(DateTimeOffset).GetProperty("UtcTicks").GetMethod;
        static readonly MethodInfo DateTimeOffset_Offset = typeof(DateTimeOffset).GetProperty("Offset").GetMethod;
        static readonly MethodInfo TimeSpan_Hours = typeof(TimeSpan).GetProperty("Hours").GetMethod;
        static readonly MethodInfo TimeSpan_Minutes = typeof(TimeSpan).GetProperty("Minutes").GetMethod;
        void WriteDateTimeOffset()
        {
            // top of stack:
            //  - DateTimeOffset
            //  - TextWriter

            if (DateFormat == DateTimeFormat.SecondsSinceUnixEpoch ||
                DateFormat == DateTimeFormat.MillisecondsSinceUnixEpoch ||
                DateFormat == DateTimeFormat.RFC1123)
            {
                // No room for an offset in these forms, so just re-use DateTime logic
                using (var loc = Emit.DeclareLocal<DateTimeOffset>())
                {
                    Emit.StoreLocal(loc);               // TextWriter
                    Emit.LoadLocalAddress(loc);         // TextWriter DateTimeOffset*
                }

                Emit.Call(DateTimeOffset_UtcDateTime);  // TextWriter DateTime
                WriteDateTime();
                return;
            }

            if (DateFormat == DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch)
            {
                // Get the UtcTicks (long) and the offset on the stack
                using (var dtoLoc = Emit.DeclareLocal<DateTimeOffset>())
                using (var tsLoc = Emit.DeclareLocal<TimeSpan>())
                {
                    Emit.StoreLocal(dtoLoc);            // TextWriter
                    Emit.LoadLocalAddress(dtoLoc);      // TextWriter DateTimeOffset*
                    Emit.Call(DateTimeOffset_UtcTicks); // TextWriter long
                    Emit.LoadLocalAddress(dtoLoc);      // TextWriter long DateTimeOffset*
                    Emit.Call(DateTimeOffset_Offset);   // TextWriter long TimeSpan
                    Emit.StoreLocal(tsLoc);             // TextWriter long
                    Emit.LoadLocalAddress(tsLoc);       // TextWriter long TimeSpan*
                    Emit.Call(TimeSpan_Hours);          // TextWriter long int
                    Emit.LoadLocalAddress(tsLoc);       // TextWriter long int TimeSpan*
                    Emit.Call(TimeSpan_Minutes);        // TextWriter long int int
                }

                Emit.LoadLocal(CharBuffer);                                                     // TextWriter long int int char[]
                Emit.Call(Methods.GetCustomWriteMicrosoftStyleWithOffset(BuildingToString));    // --empty--
                return;
            }

            if (DateFormat == DateTimeFormat.ISO8601)
            {
                // Get the DateTime equivalent and the offset on the stack
                using (var dtoLoc = Emit.DeclareLocal<DateTimeOffset>())
                using (var tsLoc = Emit.DeclareLocal<TimeSpan>())
                {
                    Emit.StoreLocal(dtoLoc);            // TextWriter
                    Emit.LoadLocalAddress(dtoLoc);      // TextWriter DateTimeOffset*
                    Emit.Call(DateTimeOffset_DateTime); // TextWriter DateTime
                    Emit.LoadLocalAddress(dtoLoc);      // TextWriter DateTime DateTimeOffset*
                    Emit.Call(DateTimeOffset_Offset);   // TextWriter DateTime TimeSpan
                    Emit.StoreLocal(tsLoc);             // TextWriter DateTime
                    Emit.LoadLocalAddress(tsLoc);       // TextWriter DateTime TimeSpan*
                    Emit.Call(TimeSpan_Hours);          // TextWriter DateTime int
                    Emit.LoadLocalAddress(tsLoc);       // TextWriter DateTime int TimeSpan*
                    Emit.Call(TimeSpan_Minutes);        // TextWriter DateTime int int
                }
                Emit.LoadLocal(CharBuffer);                                                 // TextWriter DateTime int int char[]
                Emit.Call(Methods.GetCustomISO8601WithOffsetToString(BuildingToString));    // --empty--
                return;
            }

            throw new ConstructionException("Unexpected DateTimeFormat: " + DateFormat);
        }

        static readonly MethodInfo TimeSpan_TotalSeconds = typeof(TimeSpan).GetProperty("TotalSeconds").GetMethod;
        static readonly MethodInfo TimeSpan_TotalMilliseconds = typeof(TimeSpan).GetProperty("TotalMilliseconds").GetMethod;
        void WriteTimeSpan()
        {
            // top of stack:
            //  - TimeSpan
            //  - TextWriter

            if (DateFormat == DateTimeFormat.SecondsSinceUnixEpoch)
            {
                using (var loc = Emit.DeclareLocal<TimeSpan>())
                {
                    Emit.StoreLocal(loc);                   // TextWriter
                    Emit.LoadLocalAddress(loc);             // TextWriter TimeSpan*
                }

                Emit.Call(TimeSpan_TotalSeconds);       // TextWriter double
                WritePrimitive(typeof(double), false);  // --empty--
                return;
            }

            if (DateFormat == DateTimeFormat.MillisecondsSinceUnixEpoch)
            {
                using (var loc = Emit.DeclareLocal<TimeSpan>())
                {
                    Emit.StoreLocal(loc);                   // TextWriter
                    Emit.LoadLocalAddress(loc);             // TextWriter TimeSpan*
                }

                Emit.Call(TimeSpan_TotalMilliseconds);  // TextWriter double
                WritePrimitive(typeof(double), false);  // --empty--
                return;
            }

            Emit.LoadLocal(CharBuffer);                                         // TextWriter TimeSpan char[]

            switch(DateFormat)
            {
                case DateTimeFormat.ISO8601: 
                    Emit.Call(Methods.GetWriteTimeSpanISO8601(BuildingToString));       // --empty--
                    return;
                
                case DateTimeFormat.RFC1123:
                case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch:
                    Emit.Call(Methods.GetWriteTimeSpanMicrosoft(BuildingToString));    // --empty--
                    return;
                default: throw new Exception("Unexpected DateTimeFormat [" + DateFormat + "]");
            }
        }

        void WriteDateTime()
        {
            // top of stack:
            //   - DateTime
            //   - TextWriter

            var toUniversalTime = typeof(DateTime).GetMethod("ToUniversalTime");

            var kind = typeof(DateTime).GetProperty("Kind");
            var specifyKind = typeof(DateTime).GetMethod("SpecifyKind", BindingFlags.Public | BindingFlags.Static);

            using (var loc = Emit.DeclareLocal<DateTime>())
            {
                var noChange = Emit.DefineLabel();
                var convertToKind = UnspecifiedDateTimeBehavior == UnspecifiedDateTimeKindBehavior.IsLocal ? DateTimeKind.Local : DateTimeKind.Utc;

                Emit.StoreLocal(loc);                               // TextWriter
                Emit.LoadLocalAddress(loc);                         // TextWriter DateTime*
                LoadProperty(kind);                                 // TextWriter DateTimeKind
                Emit.LoadConstant((int)DateTimeKind.Unspecified);   // TextWriter DateTimeKind DateTimeKind
                
                Emit.UnsignedBranchIfNotEqual(noChange);            // TextWriter
                Emit.LoadLocal(loc);                                // TextWriter DateTime
                Emit.LoadConstant((int)convertToKind);              // TextWriter DateTime DateTimeKind
                Emit.Call(specifyKind);                             // TextWriter DateTime
                Emit.StoreLocal(loc);                               // TextWriter
                
                Emit.MarkLabel(noChange);                           // TextWriter
                Emit.LoadLocalAddress(loc);                         // TextWriter DateTime*
                Emit.Call(toUniversalTime);                         // TextWriter DateTime
            }

            switch (DateFormat)
            {
                case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch:
                    WriteMicrosoftStyleDateTime();
                    return;
                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    WriteMillisecondsStyleDateTime();
                    return;
                case DateTimeFormat.SecondsSinceUnixEpoch:
                    WriteSecondsStyleDateTime();
                    return;
                case DateTimeFormat.ISO8601:
                    WriteISO8601StyleDateTime();
                    return;
                case DateTimeFormat.RFC1123:
                    WriteRFC1123StyleDateTime();
                    return;
                default:
                    throw new ConstructionException("Unexpected DateFormat: " + DateFormat);
            }
        }

        void CallWriteInt()
        {
            if (UseCustomWriteIntUnrolled)
            {
                Emit.Call(Methods.GetCustomWriteIntUnrolledSigned(BuildingToString));
            }
            else
            {
                Emit.Call(Methods.GetCustomWriteInt(BuildingToString));
            }
        }

        void CallWriteUInt()
        {
            if (UseCustomWriteIntUnrolled)
            {
                Emit.Call(Methods.GetCustomWriteUIntUnrolled(BuildingToString));
            }
            else
            {
                Emit.Call(Methods.GetCustomWriteUInt(BuildingToString));
            }
        }

        void CallWriteLong()
        {
            Emit.Call(Methods.GetCustomWriteLong(BuildingToString));
        }

        void CallWriteULong()
        {
            Emit.Call(Methods.GetCustomWriteULong(BuildingToString));
        }

        void WritePrimitive(Type primitiveType, bool quotesNeedHandling)
        {
            if (primitiveType == typeof(char))
            {
                WriteEncodedChar(quotesNeedHandling);

                return;
            }

            if (primitiveType == typeof(string))
            {
                if (quotesNeedHandling)
                {
                    Emit.Call(GetWriteEncodedStringWithQuotesMethod());
                }
                else
                {
                    Emit.Call(GetWriteEncodedStringMethod());
                }
                
                return;
            }

            if(primitiveType == typeof(TimeSpan))
            {
                WriteTimeSpan();
                return;
            }

            if (primitiveType == typeof(DateTime))
            {
                WriteDateTime();
                return;
            }

            if (primitiveType == typeof(DateTimeOffset))
            {
                WriteDateTimeOffset();
                return;
            }

            if (primitiveType == typeof(Guid))
            {
                WriteGuid(quotesNeedHandling);
                return;
            }

            if(primitiveType == typeof(bool))
            {
                var trueLabel = Emit.DefineLabel();
                var done = Emit.DefineLabel();

                Emit.BranchIfTrue(trueLabel);   // TextWriter
                Emit.Pop();                     // --empty--
                WriteString("false");           // --empty--
                Emit.Branch(done);

                Emit.MarkLabel(trueLabel);      // TextWriter
                Emit.Pop();                     // --empty--
                WriteString("true");            // --empty--

                Emit.MarkLabel(done);

                return;
            }

            var needsIntCoercion = primitiveType == typeof(byte) || primitiveType == typeof(sbyte) || primitiveType == typeof(short) || primitiveType == typeof(ushort);

            if (needsIntCoercion)
            {
                Emit.Convert<int>();            // TextWriter int
                primitiveType = typeof(int);
            }

            var isIntegerType = primitiveType == typeof(int) || primitiveType == typeof(uint) || primitiveType == typeof(long) || primitiveType == typeof(ulong);

            if (isIntegerType && UseCustomIntegerToString)
            {
                if (primitiveType == typeof(int))
                {
                    Emit.LoadLocal(CharBuffer);         // TextWriter int char[]
                    CallWriteInt();                     // --empty--

                    return;
                }

                if (primitiveType == typeof(uint))
                {
                    Emit.LoadLocal(CharBuffer);         // TextWriter int char[]
                    CallWriteUInt();                    // --empty--

                    return;
                }

                if (primitiveType == typeof(long))
                {
                    Emit.LoadLocal(CharBuffer);         // TextWriter int char[]
                    CallWriteLong();                    // --empty--

                    return;
                }

                if (primitiveType == typeof(ulong))
                {
                    Emit.LoadLocal(CharBuffer);         // TextWriter int char[]
                    CallWriteULong();                   // --empty--

                    return;
                }
            }

            // Stack is: TextWriter (float|double|decimal)

            MethodInfo proxyMethod;
            if (primitiveType == typeof(float))
            {
                proxyMethod = Methods.GetProxyFloat(BuildingToString);
            }
            else
            {
                if (primitiveType == typeof(double))
                {
                    proxyMethod = Methods.GetProxyDouble(BuildingToString);
                }
                else
                {
                    if (primitiveType == typeof(decimal))
                    {
                        proxyMethod = Methods.GetProxyDecimal(BuildingToString);
                    }
                    else
                    {
                        if (BuildingToString)
                        {
                            proxyMethod = typeof(ThunkWriter).GetMethod("Write", new Type[] { primitiveType });
                        }
                        else
                        {
                            proxyMethod = typeof(TextWriter).GetMethod("Write", new Type[] { primitiveType });
                        }
                    }
                }
            }

            ValidateFloatingPoint(primitiveType);       // TextWriter (float|double|decimal)
            Emit.Call(proxyMethod);                     // --empty--
        }

        void ValidateFloatingPoint(Type type)
        {
            // JSON doesn't allow any sort of NaN value, including +/- Infinity
            //  so we have to bail on them

            MethodInfo validateCall;
            if(type == typeof(double))
            {
                validateCall = Methods.GetValidateDouble();
            }
            else
            {
                if (type == typeof(float))
                {
                    validateCall = Methods.GetValidateFloat();
                }else
                {
                    // decimal has no NaN values, so no checks are needed
                    return;
                }

            }

            Emit.Duplicate();           // TextWriter (float|double|decimal) (float|double|decimal)
            Emit.Call(validateCall);    // TextWriter (float|double|decimal)
        }

        void WriteGuidFast(bool quotesNeedHandling)
        {
            if (quotesNeedHandling)
            {
                WriteString("\"");                              // TextWriter Guid
            }

            Emit.LoadLocal(CharBuffer);                         // TextWriter Guid char[]
            Emit.Call(Methods.GetWriteGuid(BuildingToString));  // --empty--
            
            if (quotesNeedHandling)
            {
                WriteString("\"");                              // --empty--
            }
        }

        void WriteGuid(bool quotesNeedHandling)
        {
            // top of stack is:
            //  - Guid
            //  - TextWriter

            if (UseFastGuids)
            {
                WriteGuidFast(quotesNeedHandling);
                return;
            }

            if (quotesNeedHandling)
            {
                WriteString("\"");      // TextWriter Guid
            }

            using (var loc = Emit.DeclareLocal<Guid>())
            {
                Emit.StoreLocal(loc);       // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter Guid*
            }

            var toString = typeof(Guid).GetMethod("ToString", Type.EmptyTypes);

            // non-virtual, since we're calling the correct method directly
            Emit.Call(toString);                // TextWriter string

            if (BuildingToString)
            {
                Emit.Call(ThunkWriter_WriteString);         // --empty--
            }
            else
            {
                Emit.CallVirtual(TextWriter_WriteString);   // --empty--
            }

            if (quotesNeedHandling)
            {
                WriteString("\"");
            }
        }

        void WriteEncodedChar(bool quotesNeedHandling)
        {
            // top of stack is:
            //  - char
            //  - TextWriter

            MethodInfo writeChar;
            if (BuildingToString)
            {
                writeChar = typeof(ThunkWriter).GetMethod("Write", new[] { typeof(char) });
            }
            else
            {
                writeChar = typeof(TextWriter).GetMethod("Write", new[] { typeof(char) });
            }

            var labels = new List<Tuple<Sigil.Label, string>>();

            int? prev = null;
            foreach (var pair in InlineSerializer.CharEncodingLabelsNeeded)
            {
                if (prev != null && pair.Item1 - prev != 1) break;

                var label = Emit.DefineLabel();

                labels.Add(Tuple.Create(label, pair.Item2));
                
                prev = pair.Item1;
            }

            if (quotesNeedHandling)
            {
                WriteString("\"");
            }

            var done = Emit.DefineLabel();
            var slash = Emit.DefineLabel();
            var quote = Emit.DefineLabel();

            // Only used in JSONP case, don't pre-init
            Sigil.Label lineSeparator = null;
            Sigil.Label paragraphSeparator = null;

            Emit.Duplicate();                               // TextWriter char char
            Emit.Convert<int>();
            Emit.LoadConstant(InlineSerializer.LowestCharNeedingEncoding);   // TextWriter char char int
            Emit.Subtract();                                // TextWriter char int

            Emit.Switch(labels.Select(s => s.Item1).ToArray()); // TextWriter char

            // this is the fall-through (default) case

            Emit.Duplicate();               // TextWriter char char
            Emit.LoadConstant('\\');        // TextWriter char char \
            Emit.BranchIfEqual(slash);      // TextWriter char

            Emit.Duplicate();               // TextWriter char char
            Emit.LoadConstant('"');         // TextWriter char char "
            Emit.BranchIfEqual(quote);      // TextWriter char

            // Curse you line terminators
            if (JSONP)
            {
                lineSeparator = Emit.DefineLabel();
                paragraphSeparator = Emit.DefineLabel();

                // line separator, valid JSON not valid javascript
                Emit.Duplicate();                   // TextWriter char char
                Emit.LoadConstant('\u2028');        // TextWriter char char \u2028
                Emit.BranchIfEqual(lineSeparator);  // TextWriter char

                // paragraph separator, valid JSON not valid javascript
                Emit.Duplicate();                       // TextWriter char char
                Emit.LoadConstant('\u2029');            // TextWriter char char \u2029
                Emit.BranchIfEqual(paragraphSeparator); // TextWriter char
            }

            Emit.CallVirtual(writeChar);    // --empty--
            Emit.Branch(done);              // --empty--

            Emit.MarkLabel(slash);          // TextWriter char
            Emit.Pop();                     // TextWriter
            Emit.Pop();                     // --empty--
            WriteString(@"\\");             // --empty--
            Emit.Branch(done);              // --empty--

            Emit.MarkLabel(quote);          // TextWriter char
            Emit.Pop();                     // TextWriter
            Emit.Pop();                     // --empty--
            WriteString(@"\""");            // --empty--
            Emit.Branch(done);              // --empty--

            if (JSONP)
            {
                Emit.MarkLabel(lineSeparator);  // TextWriter char
                Emit.Pop();                     // TextWriter
                Emit.Pop();                     // --empty--
                WriteString(@"\u2028");         // --empty--
                Emit.Branch(done);              // --empty--

                Emit.MarkLabel(paragraphSeparator); // TextWriter char
                Emit.Pop();                         // TextWriter
                Emit.Pop();                         // --empty--
                WriteString(@"\u2029");             // --empty--
                Emit.Branch(done);                  // --empty--
            }

            foreach (var label in labels)
            {
                Emit.MarkLabel(label.Item1);    // TextWriter char

                Emit.Pop();                     // TextWriter
                Emit.Pop();                     // --empty--
                WriteString(label.Item2);       // --empty-- 
                Emit.Branch(done);              // --empty--
            }

            Emit.MarkLabel(done);

            if (quotesNeedHandling)
            {
                WriteString("\"");
            }
        }

        void WriteObjectWithNulls(Type forType, Sigil.Local inLocal)
        {
            var writeOrder = OrderMembersForAccess(forType, RecursiveTypes);
            var hasConditionalSerialization = writeOrder.OfType<PropertyInfo>().Any(p => p.ShouldSerializeMethod(forType) != null);

            if (hasConditionalSerialization)
            {
                WriteObjectWithNullsWithConditionalSerialization(forType, inLocal, writeOrder);
            }
            else
            {
                WriteObjectWithNullsWithoutConditionalSerialization(forType, inLocal, writeOrder);
            }
        }

        void WriteObjectWithNullsWithoutConditionalSerialization(Type forType, Sigil.Local inLocal, List<MemberInfo> writeOrder)
        {
            var notNull = Emit.DefineLabel();

            var isValueType = forType._IsValueType();

            if (inLocal != null)
            {
                Emit.LoadLocal(inLocal);    // obj
            }
            else
            {
                Emit.LoadArgument(1);       // obj
            }

            if (isValueType)
            {
                using (var temp = Emit.DeclareLocal(forType))
                {
                    Emit.StoreLocal(temp);          // --empty--
                    Emit.LoadLocalAddress(temp);    // obj*
                }
            }

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);             // --empty--

            // No ExcludeNulls checks since this code is never run
            //   if that's set
            WriteString("null");                    // --empty--

            Emit.Branch(end);                       // --empty--

            Emit.MarkLabel(notNull);                    // --empty--
            WriteString("{");                           // --empty--

            IncreaseIndent();

            var firstPass = true;
            foreach (var member in writeOrder)
            {
                if (PropagateConstants && member.IsConstant())
                {
                    WriteConstantMember(member, prependComma: !firstPass);
                    firstPass = false;
                }
                else
                {
                    if (!PrettyPrint)
                    {
                        string keyString;
                        if (firstPass)
                        {
                            keyString = "\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\":";
                            firstPass = false;
                        }
                        else
                        {
                            keyString = ",\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\":";
                        }

                        WriteString(keyString);         // --empty--
                        WriteMember(member, inLocal);   // --empty--
                    }
                    else
                    {
                        if (!firstPass)
                        {
                            WriteString(",");
                        }

                        LineBreakAndIndent();

                        firstPass = false;

                        WriteString("\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\": ");

                        WriteMember(member, inLocal);
                    }
                }
            }

            DecreaseIndent();

            WriteString("}");       // --empty--

            Emit.MarkLabel(end);
        }

        void WriteObject(Type forType, Sigil.Local inLocal = null)
        {
            if (DynamicCallOutCheck(null, forType, inLocal)) return;

            if (CallOutOnPossibleDynamic)
            {
                WritingDynamicObject.Push(forType);
            }

            if (!ExcludeNulls)
            {
                WriteObjectWithNulls(forType, inLocal);
            }
            else
            {
                WriteObjectWithoutNulls(forType, inLocal);
            }

            if (CallOutOnPossibleDynamic)
            {
                var finished = WritingDynamicObject.Pop();
                if (finished != forType)
                {
                    throw new ConstructionException("Internal type tracking in unexpected state, found [" + finished + "] expected [" + forType + "]");
                }
            }
        }

        void WriteObjectWithNullsWithConditionalSerialization(Type forType, Sigil.Local inLocal, List<MemberInfo> writeOrder)
        {
            var notNull = Emit.DefineLabel();

            var isValueType = forType._IsValueType();

            if (inLocal != null)
            {
                Emit.LoadLocal(inLocal);    // obj
            }
            else
            {
                Emit.LoadArgument(1);       // obj
            }

            if (isValueType)
            {
                using (var temp = Emit.DeclareLocal(forType))
                {
                    Emit.StoreLocal(temp);          // --empty--
                    Emit.LoadLocalAddress(temp);    // obj*
                }
            }

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);             // --empty--

            // No ExcludeNulls checks since this code is never run
            //   if that's set
            WriteString("null");                    // --empty--

            Emit.Branch(end);                       // --empty--

            Emit.MarkLabel(notNull);    // --empty--
            WriteString("{");           // --empty--

            IncreaseIndent();

            if (inLocal != null)
            {
                Emit.LoadLocal(inLocal);    // obj
            }
            else
            {
                Emit.LoadArgument(1);       // obj
            }

            if (isValueType)
            {
                using (var temp = Emit.DeclareLocal(forType))
                {
                    Emit.StoreLocal(temp);          // --empty--
                    Emit.LoadLocalAddress(temp);    // obj*
                }
            }

            using (var isFirst = Emit.DeclareLocal<bool>())
            {
                Emit.LoadConstant(true);                        // obj(*?) true
                Emit.StoreLocal(isFirst);                       // obj(*?)
                foreach (var member in writeOrder)
                {
                    Emit.Duplicate();                                               // obj(*?) obj(*?)
                    WriteMemberConditionally(forType, member, inLocal, isFirst);    // obj(*?)
                }
            }

            Emit.Pop();                                     // --empty--

            DecreaseIndent();                               // --empty--

            WriteString("}");                               // --empty--

            Emit.MarkLabel(end);                            // --empty--
        }

        void WriteObjectWithoutNulls(Type forType, Sigil.Local inLocal)
        {
            var writeOrder = OrderMembersForAccess(forType, RecursiveTypes);

            var notNull = Emit.DefineLabel();

            var isValueType = forType._IsValueType();

            if (inLocal != null)
            {
                Emit.LoadLocal(inLocal);                        // obj
            }
            else
            {
                Emit.LoadArgument(1);                           // obj
            }

            if (isValueType)
            {
                using (var temp = Emit.DeclareLocal(forType))
                {
                    Emit.StoreLocal(temp);                      // --empty--
                    Emit.LoadLocalAddress(temp);                // obj*
                }
            }

            var end = Emit.DefineLabel();

            Emit.Duplicate();               // obj(*?) obj(*?)
            Emit.BranchIfTrue(notNull);     // obj(*?)
            Emit.Branch(end);               // obj(*?)

            Emit.MarkLabel(notNull);                    // obj(*?)
            WriteString("{");                           // obj(*?)

            IncreaseIndent();           

            using (var isFirst = Emit.DeclareLocal<bool>())
            {
                Emit.LoadConstant(true);        // obj(*?) true
                Emit.StoreLocal(isFirst);       // obj(*?) true

                foreach (var member in writeOrder)
                {
                    Emit.Duplicate();                                         // obj(*?) obj(*?)
                    WriteMemberIfNonNull(forType, member, inLocal, isFirst);  // obj(*?)
                }
            }

            DecreaseIndent();

            WriteString("}");       // obj(*?)

            Emit.MarkLabel(end);    // obj(*?)
            Emit.Pop();             // --empty--
        }

        void WriteMemberIfNonNull(Type onType, MemberInfo member, Sigil.Local inLocal, Sigil.Local isFirst)
        {
            // Top of stack:
            //  - obj(*?)

            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new ConstructionException("Encountered a serializable member that is neither a field nor a property: " + member);

            var serializingType = asField != null ? asField.FieldType : asProp.PropertyType;

            var end = Emit.DefineLabel();
            var writeValue = Emit.DefineLabel();

            if (asProp != null)
            {
                var shouldSerialize = asProp.ShouldSerializeMethod(onType);
                if (shouldSerialize != null)
                {
                    var canSerialize = Emit.DefineLabel();

                    Emit.Duplicate();                   // obj(*?) obj(*?)

                    if (shouldSerialize.IsVirtual)
                    {
                        Emit.CallVirtual(shouldSerialize);  // obj(*?) bool
                    }
                    else
                    {
                        Emit.Call(shouldSerialize);         // obj(*?) bool
                    }

                    Emit.BranchIfTrue(canSerialize);    // obj(*?)

                    Emit.Pop();                         // --empty--
                    Emit.Branch(end);                   // --empty--

                    Emit.MarkLabel(canSerialize);       // obj(*?)
                }
            }

            var canBeNull = serializingType.IsNullableType() || !serializingType._IsValueType();
            if (canBeNull)
            {
                if (asField != null)
                {
                    Emit.LoadField(asField);    // value
                }
                else
                {
                    LoadProperty(asProp);       // value
                }

                if (serializingType._IsValueType())
                {
                    using (var temp = Emit.DeclareLocal(serializingType))
                    {
                        Emit.StoreLocal(temp);          // --empty--
                        Emit.LoadLocalAddress(temp);    // value*
                    }

                    var hasValue = serializingType.GetProperty("HasValue").GetMethod;
                    Emit.Call(hasValue);        // bool
                }

                Emit.BranchIfFalse(end);        // --empty--
            }
            else
            {
                Emit.Pop();                     // --empty--
            }

            Emit.LoadLocal(isFirst);        // bool
            Emit.BranchIfTrue(writeValue);  // --empty--

            WriteString(",");

            Emit.MarkLabel(writeValue);     // --empty--

            Emit.LoadConstant(false);       // false
            Emit.StoreLocal(isFirst);       // --empty--

            if (PrettyPrint)
            {
                LineBreakAndIndent();
            }

            if (PropagateConstants && member.IsConstant())
            {
                WriteConstantMember(member, prependComma: false);
            }
            else
            {
                if (PrettyPrint)
                {
                    WriteString("\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\": ");     // --empty--
                }
                else
                {
                    WriteString("\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\":");      // --empty--
                }

                WriteMember(member, inLocal);           // --empty--
            }

            Emit.MarkLabel(end);
        }

        void WriteMemberConditionally(Type onType, MemberInfo member, Sigil.Local inLocal, Sigil.Local isFirst)
        {
            // top of stack
            //  - obj(*?)

            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new ConstructionException("Encountered a serializable member that is neither a field nor a property: " + member);

            var end = Emit.DefineLabel();
            var writeValue = Emit.DefineLabel();

            if (asProp != null)
            {
                var shouldSerialize = asProp.ShouldSerializeMethod(onType);
                if (shouldSerialize != null)
                {
                    var canSerialize = Emit.DefineLabel();

                    Emit.Duplicate();                   // obj(*?) obj(*?)

                    if (shouldSerialize.IsVirtual)
                    {
                        Emit.CallVirtual(shouldSerialize);  // obj(*?) bool
                    }
                    else
                    {
                        Emit.Call(shouldSerialize);         // obj(*?) bool
                    }

                    Emit.BranchIfTrue(canSerialize);    // obj(*?)

                    Emit.Pop();                         // --empty--
                    Emit.Branch(end);                   // --empty--

                    Emit.MarkLabel(canSerialize);       // obj(*?)
                }

                Emit.Pop();                         // --empty--
            }

            Emit.LoadLocal(isFirst);        // bool
            Emit.BranchIfTrue(writeValue);  // --empty--

            WriteString(",");

            Emit.MarkLabel(writeValue);     // --empty--

            Emit.LoadConstant(false);       // false
            Emit.StoreLocal(isFirst);       // --empty--

            if (PrettyPrint)
            {
                LineBreakAndIndent();
            }

            if (PropagateConstants && member.IsConstant())
            {
                WriteConstantMember(member, prependComma: false);
            }
            else
            {

                if (PrettyPrint)
                {
                    WriteString("\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\": ");     // --empty--
                }
                else
                {
                    WriteString("\"" + member.GetSerializationName(SerializationNameFormat).JsonEscape(JSONP) + "\":");      // --empty--
                }

                WriteMember(member, inLocal);           // --empty--
            }

            Emit.MarkLabel(end);
        }

        void WriteListFast(MemberInfo listMember, Type listType, Sigil.Local inLocal = null)
        {
            Action loadList =
                delegate
                {
                    if (inLocal != null)
                    {
                        Emit.LoadLocal(inLocal);
                    }
                    else
                    {
                        Emit.LoadArgument(1);
                    }
                };

            var listInterface =
                listType.IsListType()
                    ? listType.GetListInterface()
                    : listType.GetReadOnlyListInterface();

            var collectionInterface =
                listType.IsCollectionType()
                    ? listType.GetCollectionInterface()
                    : listType.GetReadOnlyCollectionInterface();

            var elementType = listInterface.GetGenericArguments()[0];
            var countMtd = collectionInterface.GetProperty("Count").GetMethod;
            var accessorMtd = listInterface.GetProperty("Item").GetMethod;

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || (listMember != null && elementType._IsEnum() && listMember.ShouldConvertEnum(elementType)) || isRecursive || elementType.IsNullableType();

            var notNull = Emit.DefineLabel();

            loadList();                         // IList<>

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);         // --empty--
            WriteString("null");                // --empty--
            Emit.Branch(end);                   // --empty--

            Emit.MarkLabel(notNull);            // --empty--
            WriteString("[");                   // --empty--

            var done = Emit.DefineLabel();

            using (var e = Emit.DeclareLocal<int>())
            {
                loadList();                                 // IList<>
                Emit.CastClass(collectionInterface);        // IList<>
                Emit.CallVirtual(countMtd);                 // int
                Emit.StoreLocal(e);                         // --empty--

                // Do the whole first element before the loop starts, so we don't need a branch to emit a ','
                {
                    Emit.LoadConstant(1);                   // 1
                    loadList();                             // 1 IList<>
                    Emit.CallVirtual(countMtd);             // 1 int
                    Emit.BranchIfGreater(done);             // --empty--

                    if (isRecursive)
                    {
                        var loc = RecursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    loadList();                             // Action<>? TextWriter IList<>
                    Emit.LoadConstant(0);                   // Action<>? TextWriter IList<> 0
                    Emit.CallVirtual(accessorMtd);          // Action<>? TextWriter type

                    WriteElement(listMember, elementType);               // --empty--
                }

                using (var i = Emit.DeclareLocal<int>())
                {
                    Emit.LoadConstant(1);                   // 1
                    Emit.StoreLocal(i);                     // --empty--

                    var loop = Emit.DefineLabel();

                    Emit.MarkLabel(loop);                   // --empty--

                    Emit.LoadLocal(e);                      // length
                    Emit.LoadLocal(i);                      // length i
                    Emit.BranchIfEqual(done);               // --empty--

                    if (isRecursive)
                    {
                        var loc = RecursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    loadList();                             // Action<>? TextWriter? IList<>
                    Emit.LoadLocal(i);                      // Action<>? TextWriter? IList<> i
                    Emit.CallVirtual(accessorMtd);          // Action<>? TextWriter? type

                    if (PrettyPrint)
                    {
                        WriteString(", ");                  // Action<>? TextWriter? type
                    }
                    else
                    {
                        WriteString(",");                   // Action<>? TextWriter? type
                    }

                    WriteElement(listMember, elementType);              // --empty--

                    Emit.LoadLocal(i);                      // i
                    Emit.LoadConstant(1);                   // i 1
                    Emit.Add();                             // i+1
                    Emit.StoreLocal(i);                     // --empty--

                    Emit.Branch(loop);                      // --empty--
                }
            }

            Emit.MarkLabel(done);   // --empty--

            WriteString("]");       // --empty--

            Emit.MarkLabel(end);    // --empty--
        }

        void WriteArrayFast(MemberInfo arrayMember, Type listType, Sigil.Local inLocal = null)
        {
            Action loadArray =
                delegate
                {
                    if (inLocal != null)
                    {
                        Emit.LoadLocal(inLocal);
                    }
                    else
                    {
                        Emit.LoadArgument(1);
                    }
                };

            var elementType = listType.GetListInterface().GetGenericArguments()[0];
            var countMtd = listType.GetProperty("Length").GetMethod;

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || (arrayMember != null && elementType._IsEnum() && arrayMember.ShouldConvertEnum(elementType)) || isRecursive || elementType.IsNullableType();

            var notNull = Emit.DefineLabel();

            loadArray();                         // type[]

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);         // --empty--
            WriteString("null");                // --empty--
            Emit.Branch(end);                   // --empty--

            Emit.MarkLabel(notNull);            // --empty--
            WriteString("[");                   // --empty--

            var done = Emit.DefineLabel();

            using (var e = Emit.DeclareLocal<int>())
            {
                loadArray();                                // type[]
                Emit.CallVirtual(countMtd);                 // int
                Emit.StoreLocal(e);                         // --empty--

                // Do the whole first element before the loop starts, so we don't need a branch to emit a ','
                {
                    Emit.LoadConstant(1);                   // 1
                    loadArray();                            // 1 type[]
                    Emit.CallVirtual(countMtd);             // 1 int
                    Emit.BranchIfGreater(done);             // --empty--

                    if (isRecursive)
                    {
                        var loc = RecursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    loadArray();                            // Action<>? TextWriter type[]
                    Emit.LoadConstant(0);                   // Action<>? TextWriter type[] 0
                    Emit.LoadElement(elementType);          // Action<>? TextWriter type

                    WriteElement(arrayMember, elementType);               // --empty--
                }

                using (var i = Emit.DeclareLocal<int>())
                {
                    Emit.LoadConstant(1);                   // 1
                    Emit.StoreLocal(i);                     // --empty--

                    var loop = Emit.DefineLabel();

                    Emit.MarkLabel(loop);                   // --empty--

                    Emit.LoadLocal(e);                      // length
                    Emit.LoadLocal(i);                      // length i
                    Emit.BranchIfEqual(done);               // --empty--

                    if (isRecursive)
                    {
                        var loc = RecursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    loadArray();                            // Action<>? TextWriter? type[]
                    Emit.LoadLocal(i);                      // Action<>? TextWriter? type[] i
                    Emit.LoadElement(elementType);

                    if (PrettyPrint)
                    {
                        WriteString(", ");                  // Action<>? TextWriter? type
                    }
                    else
                    {
                        WriteString(",");                   // Action<>? TextWriter? type
                    }

                    WriteElement(arrayMember, elementType);              // --empty--

                    Emit.LoadLocal(i);                      // i
                    Emit.LoadConstant(1);                   // i 1
                    Emit.Add();                             // i+1
                    Emit.StoreLocal(i);                     // --empty--

                    Emit.Branch(loop);                      // --empty--
                }
            }

            Emit.MarkLabel(done);   // --empty--

            WriteString("]");       // --empty--

            Emit.MarkLabel(end);    // --empty--
        }

        void WriteList(MemberInfo listMember, Type listType, Sigil.Local inLocal = null)
        {
            if (DynamicCallOutCheck(listMember, listType, inLocal)) return;

            if (listType.IsArray && UseFastArrays)
            {
                WriteArrayFast(listMember, listType, inLocal);
                return;
            }

            if (UseFastLists)
            {
                WriteListFast(listMember, listType, inLocal);
                return;
            }

            WriteEnumerable(listMember, listType, inLocal);
        }

        void WriteEnumerable(MemberInfo enumerableMember, Type enumerableType, Sigil.Local inLocal = null)
        {
            if (DynamicCallOutCheck(enumerableMember, enumerableType, inLocal)) return;

            var elementType = enumerableType.GetEnumerableInterface().GetGenericArguments()[0];

            var iEnumerable = typeof(IEnumerable<>).MakeGenericType(elementType);
            var iEnumerableGetEnumerator = iEnumerable.GetMethod("GetEnumerator");
            var enumeratorMoveNext = typeof(System.Collections.IEnumerator).GetMethod("MoveNext");
            var enumeratorCurrent = iEnumerableGetEnumerator.ReturnType.GetProperty("Current");

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || (enumerableMember != null && elementType._IsEnum() && enumerableMember.ShouldConvertEnum(elementType)) || isRecursive || elementType.IsNullableType();

            var notNull = Emit.DefineLabel();

            if (inLocal != null)
            {
                Emit.LoadLocal(inLocal);
            }
            else
            {
                Emit.LoadArgument(1);
            }

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);
            WriteString("null");
            Emit.Branch(end);

            Emit.MarkLabel(notNull);
            WriteString("[");

            var done = Emit.DefineLabel();

            using (var e = Emit.DeclareLocal(iEnumerableGetEnumerator.ReturnType))
            {
                if (inLocal != null)
                {
                    Emit.LoadLocal(inLocal);
                }
                else
                {
                    Emit.LoadArgument(1);
                }

                Emit.CallVirtual(iEnumerableGetEnumerator);   // IEnumerator<>
                Emit.StoreLocal(e);                           // --empty--

                // Do the whole first element before the loop starts, so we don't need a branch to emit a ','
                {
                    Emit.LoadLocal(e);                      // IEnumerator<>
                    Emit.CallVirtual(enumeratorMoveNext);   // bool
                    Emit.BranchIfFalse(done);               // --empty--

                    if (isRecursive)
                    {
                        var loc = RecursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    Emit.LoadLocal(e);                      // Action<>? TextWriter? IEnumerator<>
                    LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? type

                    WriteElement(enumerableMember, elementType);   // --empty--
                }

                var loop = Emit.DefineLabel();

                Emit.MarkLabel(loop);

                Emit.LoadLocal(e);                      // IEnumerator<>
                Emit.CallVirtual(enumeratorMoveNext);   // bool
                Emit.BranchIfFalse(done);               // --empty--

                if (isRecursive)
                {
                    var loc = RecursiveTypes[elementType];

                    Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                }

                if (preloadTextWriter)
                {
                    Emit.LoadArgument(0);               // Action<>? TextWriter
                }

                Emit.LoadLocal(e);                      // Action<>? TextWriter? IEnumerator<>
                LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? type

                if (PrettyPrint)
                {
                    WriteString(", ");
                }
                else
                {
                    WriteString(",");
                }

                WriteElement(enumerableMember, elementType);   // --empty--

                Emit.Branch(loop);
            }

            Emit.MarkLabel(done);

            WriteString("]");

            Emit.MarkLabel(end);
        }

        void WriteElement(MemberInfo listMember, Type elementType)
        {
            if (elementType.IsPrimitiveType())
            {
                WritePrimitive(elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType.IsNullableType())
            {
                WriteNullable(listMember, elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType._IsEnum())
            {
                WriteEnumOrPrimitive(listMember, elementType, requiresQuotes: false, hasTextWriter: true, popTextWriter: false);
                return;
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            if (isRecursive)
            {
                // Stack is:
                //  - serializingType
                //  - TextWriter
                //  - Action<TextWriter, serializingType, int>

                Type recursiveAct;
                if (BuildingToString)
                {
                    recursiveAct = typeof(StringThunkDelegate<>).MakeGenericType(elementType);
                }
                else
                {
                    recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), elementType, typeof(int));
                }

                var invoke = recursiveAct.GetMethod("Invoke");

                Emit.LoadArgument(2);       // Action<TextWriter, elementType, int> TextWriter int
                Emit.Call(invoke);          // --empty--

                return;
            }

            using(var loc = Emit.DeclareLocal(elementType))
            {
                Emit.StoreLocal(loc);

                if (elementType.IsListType())
                {
                    WriteList(listMember, elementType, loc);
                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteDictionary(listMember, elementType, loc);
                    return;
                }

                if (elementType.IsEnumerableType())
                {
                    WriteEnumerable(listMember, elementType, loc);
                    return;
                }

                WriteObject(elementType, loc);
            }
        }

        bool ShouldDynamicCallOut(Type onType)
        {
            // Exact god-damn match
            var isThisType = onType == typeof(ForType);
            
            if (isThisType)
            {
                var alreadyWorkingOnThisType = CallOutOnPossibleDynamic && WritingDynamicObject.Contains(onType);

                // actually detected recursion, it's time to bail
                if (alreadyWorkingOnThisType) return true;

                return false;
            }

            if (CallOutOnPossibleDynamic && (onType._IsInterface() || !onType._IsSealed()))
            {
                return true;
            }

            return false;
        }

        bool DynamicCallOutCheck(MemberInfo dynamicMember, Type onType, Sigil.Local inLocal)
        {
            if (ShouldDynamicCallOut(onType))
            {
                var serializeMtd = DynamicSerializer.SerializeMtd;
                if (dynamicMember != null)
                {
                    serializeMtd = DynamicSerializer.SerializeInternalMtd;

                    if (dynamicMember is FieldInfo)
                    {
                        // Load the field from a RuntimeFieldHandle constant
                        Emit.LoadConstant((FieldInfo)dynamicMember); // RuntimeFieldHandle
                        Emit.LoadConstant(dynamicMember.DeclaringType); // RuntimeTypeHandle
                        Emit.Call(InlineSerializer.FieldInfoFromHandle); // FieldInfo
                        Emit.CastClass(typeof(MemberInfo)); // MemberInfo
                    }
                    else
                    {
                        // Load the property using reflection, yes it is ugly
                        // but there is no other way
#if COREFXTODO
                        Emit.LoadConstant(dynamicMember.DeclaringType); // RuntimeTypeHandle
#else
                        Emit.LoadConstant(dynamicMember.ReflectedType); // RuntimeTypeHandle
#endif
                        Emit.Call(InlineSerializer.TypeFromHandle); // Type
                        Emit.LoadConstant(dynamicMember.Name); // Type string
                        Emit.LoadConstant((int)(BindingFlags.Public | BindingFlags.Instance)); // Type string int
                        Emit.Call(InlineSerializer.TypeGetProperty); // PropertyInfo
                        Emit.CastClass(typeof(MemberInfo)); // MemberInfo
                    }
                }

                Emit.LoadArgument(0);               // TextWriter

                if (inLocal != null)
                {
                    Emit.LoadLocal(inLocal);        // TextWriter object
                }
                else
                {
                    Emit.LoadArgument(1);           // TextWriter object
                }

                var equivalentOptions = new Options(this.PrettyPrint, this.ExcludeNulls, this.JSONP, this.DateFormat, this.IncludeInherited, this.UnspecifiedDateTimeBehavior, this.SerializationNameFormat);
                var optionsField = OptionsLookup.GetOptionsFieldFor(equivalentOptions);

                Emit.LoadField(optionsField);       // TextWriter object Options

                Emit.LoadArgument(2);               // TextWriter object Options int

                Emit.Call(serializeMtd);            // void

                return true;
            }

            return false;
        }

        void WriteDictionary(MemberInfo dictionaryMember, Type dictType, Sigil.Local inLocal = null)
        {
            if (DynamicCallOutCheck(dictionaryMember, dictType, inLocal)) return;

            if (!ExcludeNulls)
            {
                WriteDictionaryWithNulls(dictionaryMember, dictType, inLocal);
            }
            else
            {
                WriteDictionaryWithoutNulls(dictionaryMember, dictType, inLocal);
            }
        }

        void WriteDictionaryWithoutNulls(MemberInfo dictionaryMember, Type dictType, Sigil.Local inLocal)
        {
            var dictI =
                dictType.IsDictionaryType()
                    ? dictType.GetDictionaryInterface()
                    : dictType.GetReadOnlyDictionaryInterface();

            var keyType = dictI.GetGenericArguments()[0];
            var elementType = dictI.GetGenericArguments()[1];

            var keyIsString = keyType == typeof(string);
            var keyIsEnum = keyType._IsEnum();
            var keysAreIntegers = keyType.IsIntegerNumberType();

            if (!(keyIsString || keyIsEnum || keysAreIntegers))
            {
                throw new ConstructionException("JSON dictionaries must have strings, enums, or integers as keys, found: " + keyType);
            }

            var kvType = typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);

            MethodInfo getEnumerator, enumeratorMoveNext;
            PropertyInfo enumeratorCurrent;
            Action<Sigil.Local> loadEnumerator;
            Action<Sigil.Local> disposeEnumerator;

            if (AllocationlessDictionaries && dictType.IsExactlyDictionaryType())
            {
                var enumerator = dictType.GetNestedType("Enumerator", BindingFlags.Public).MakeGenericType(keyType, elementType);
                getEnumerator = dictType.GetMethod("GetEnumerator");
                enumeratorMoveNext = enumerator.GetMethod("MoveNext");
                enumeratorCurrent = enumerator.GetProperty("Current");
                var dispose = enumerator.GetMethod("Dispose");

                loadEnumerator = e => Emit.LoadLocalAddress(e);
                disposeEnumerator =
                    e =>
                    {
                        Emit.LoadLocalAddress(e);
                        Emit.Call(dispose);
                    };
            }
            else
            {
                var iEnumerable = typeof(IEnumerable<>).MakeGenericType(kvType);
                getEnumerator = iEnumerable.GetMethod("GetEnumerator");
                enumeratorMoveNext = typeof(System.Collections.IEnumerator).GetMethod("MoveNext");
                enumeratorCurrent = getEnumerator.ReturnType.GetProperty("Current");
                loadEnumerator = e => Emit.LoadLocal(e);
                disposeEnumerator = e => { };
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || (dictionaryMember != null && elementType._IsEnum() && dictionaryMember.ShouldConvertEnum(elementType)) || isRecursive || elementType.IsNullableType();

            var notNull = Emit.DefineLabel();

            if (inLocal != null)
            {
                Emit.LoadLocal(inLocal);
            }
            else
            {
                Emit.LoadArgument(1);
            }

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);
            WriteString("null");
            Emit.Branch(end);

            Emit.MarkLabel(notNull);
            WriteString("{");

            IncreaseIndent();

            var done = Emit.DefineLabel();

            int onTheStack = 0;

            using (var e = Emit.DeclareLocal(getEnumerator.ReturnType))
            using (var isFirst = Emit.DeclareLocal<bool>())
            using (var kvpLoc = Emit.DeclareLocal(kvType))
            {
                Emit.LoadConstant(true);                    // true
                Emit.StoreLocal(isFirst);                   // --empty--

                if (inLocal != null)
                {
                    Emit.LoadLocal(inLocal);                // object
                }
                else
                {
                    Emit.LoadArgument(1);                   // object
                }

                Emit.CallVirtual(getEnumerator);        // IEnumerator<KeyValuePair<,>>
                Emit.StoreLocal(e);                     // --empty--

                var loop = Emit.DefineLabel();

                Emit.MarkLabel(loop);                   // --empty--

                loadEnumerator(e);                      // IEnumerator<KeyValuePair<,>>(*?)
                Emit.CallVirtual(enumeratorMoveNext);   // bool
                Emit.BranchIfFalse(done);               // --empty--

                if (isRecursive)
                {
                    onTheStack++;

                    var loc = RecursiveTypes[elementType];

                    Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                }

                if (preloadTextWriter)
                {
                    onTheStack++;

                    Emit.LoadArgument(0);               // Action<>? TextWriter
                }

                loadEnumerator(e);                      // Action<>? TextWriter? IEnumerator<KeyValuePair<,>>(*?)
                LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? KeyValuePair<,>

                Emit.StoreLocal(kvpLoc);                // Action<>? TextWriter?
                Emit.LoadLocalAddress(kvpLoc);          // Action<>? TextWriter? KeyValuePair<,>*

                onTheStack++;

                WriteKeyValueIfNotNull(dictionaryMember, onTheStack, keyType, elementType, isFirst);   // --empty--

                Emit.Branch(loop);                      // --empty--

                Emit.MarkLabel(done);
                disposeEnumerator(e);
            }

            DecreaseIndent();

            WriteString("}");

            Emit.MarkLabel(end);
        }

        void WriteDictionaryWithNulls(MemberInfo dictionaryMember, Type dictType, Sigil.Local inLocal)
        {
            var dictI =
                dictType.IsDictionaryType()
                    ? dictType.GetDictionaryInterface()
                    : dictType.GetReadOnlyDictionaryInterface();

            var keyType = dictI.GetGenericArguments()[0];
            var elementType = dictI.GetGenericArguments()[1];

            var keysAreStrings = keyType == typeof(string);
            var keysAreEnums = keyType._IsEnum();
            var keysAreIntegers = keyType.IsIntegerNumberType();

            if (!(keysAreStrings || keysAreEnums || keysAreIntegers))
            {
                throw new ConstructionException("JSON dictionaries must have strings, enums, or integers as keys, found: " + keyType);
            }

            var kvType = typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);

            MethodInfo getEnumerator, enumeratorMoveNext;
            PropertyInfo enumeratorCurrent;
            Action<Sigil.Local> loadEnumerator;
            Action<Sigil.Local> disposeEnumerator;

            if (AllocationlessDictionaries && dictType.IsExactlyDictionaryType())
            {
                var enumerator = dictType.GetNestedType("Enumerator", BindingFlags.Public).MakeGenericType(keyType, elementType);
                getEnumerator = dictType.GetMethod("GetEnumerator");
                enumeratorMoveNext = enumerator.GetMethod("MoveNext");
                enumeratorCurrent = enumerator.GetProperty("Current");
                var dispose = enumerator.GetMethod("Dispose");

                loadEnumerator = e => Emit.LoadLocalAddress(e);
                disposeEnumerator =
                    e =>
                    {
                        Emit.LoadLocalAddress(e);
                        Emit.Call(dispose);
                    };
            }
            else
            {
                var iEnumerable = typeof(IEnumerable<>).MakeGenericType(kvType);
                getEnumerator = iEnumerable.GetMethod("GetEnumerator");
                enumeratorMoveNext = typeof(System.Collections.IEnumerator).GetMethod("MoveNext");
                enumeratorCurrent = getEnumerator.ReturnType.GetProperty("Current");
                loadEnumerator = e => Emit.LoadLocal(e);
                disposeEnumerator = e => { };
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || (dictionaryMember != null && elementType._IsEnum() && dictionaryMember.ShouldConvertEnum(elementType)) || isRecursive || elementType.IsNullableType();

            var notNull = Emit.DefineLabel();

            if (inLocal != null)
            {
                Emit.LoadLocal(inLocal);
            }
            else
            {
                Emit.LoadArgument(1);
            }

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);
            WriteString("null");
            Emit.Branch(end);

            Emit.MarkLabel(notNull);
            WriteString("{");

            IncreaseIndent();

            var done = Emit.DefineLabel();

            using (var e = Emit.DeclareLocal(getEnumerator.ReturnType))
            using (var kvpLoc = Emit.DeclareLocal(kvType))
            {
                if (inLocal != null)
                {
                    Emit.LoadLocal(inLocal);
                }
                else
                {
                    Emit.LoadArgument(1);
                }

                Emit.CallVirtual(getEnumerator);   // IEnumerator<KeyValuePair<,>>
                Emit.StoreLocal(e);                           // --empty--

                // Do the whole first element before the loop starts, so we don't need a branch to emit a ','
                {
                    loadEnumerator(e);                      // IEnumerator<KeyValuePair<,>>(*?)
                    Emit.CallVirtual(enumeratorMoveNext);   // bool
                    Emit.BranchIfFalse(done);               // --empty--

                    if (isRecursive)
                    {
                        var loc = RecursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    loadEnumerator(e);                      // Action<>? TextWriter? IEnumerator<>(*?)
                    LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? KeyValuePair<,>

                    Emit.StoreLocal(kvpLoc);                // Action<>? TextWriter?
                    Emit.LoadLocalAddress(kvpLoc);          // Action<>? TextWriter? KeyValuePair<,>*

                    WriteKeyValue(dictionaryMember, keyType, elementType);   // --empty--
                }

                var loop = Emit.DefineLabel();

                Emit.MarkLabel(loop);                   // --empty--

                loadEnumerator(e);                      // IEnumerator<KeyValuePair<,>>(*?)
                Emit.CallVirtual(enumeratorMoveNext);   // bool
                Emit.BranchIfFalse(done);               // --empty--

                if (isRecursive)
                {
                    var loc = RecursiveTypes[elementType];

                    Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                }

                if (preloadTextWriter)
                {
                    Emit.LoadArgument(0);               // Action<>? TextWriter
                }

                loadEnumerator(e);                      // Action<>? TextWriter? IEnumerator<>(*?)
                LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? KeyValuePair<,>

                Emit.StoreLocal(kvpLoc);                // Action<>? TextWriter?
                Emit.LoadLocalAddress(kvpLoc);          // Action<>? TextWriter? KeyValuePair<,>*

                WriteString(",");

                WriteKeyValue(dictionaryMember, keyType, elementType);   // --empty--

                Emit.Branch(loop);                          // --empty--

                Emit.MarkLabel(done);
                disposeEnumerator(e);
            }

            DecreaseIndent();

            WriteString("}");

            Emit.MarkLabel(end);
        }

        void WriteKeyValueIfNotNull(MemberInfo dictionaryMember, int ontheStack, Type keyType, Type elementType, Sigil.Local isFirst)
        {
            // top of the stack is a 
            //   - KeyValue<keyType, elementType>
            //   - TextWriter?
            //   - Action<,>?

            var keyValuePair = typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);
            var key = keyValuePair.GetProperty("Key");
            var value = keyValuePair.GetProperty("Value");

            var keyIsString = keyType == typeof(string);
            var keyIsNumber = keyType.IsIntegerNumberType();

            var done = Emit.DefineLabel();
            var doWrite = Emit.DefineLabel();

            var canBeNull = elementType.IsNullableType() || !elementType._IsValueType();

            if (canBeNull)
            {
                Emit.Duplicate();       // kvp kvp
                LoadProperty(value);    // kvp value

                if (elementType.IsNullableType())
                {
                    using (var temp = Emit.DeclareLocal(elementType))
                    {
                        Emit.StoreLocal(temp);          // kvp
                        Emit.LoadLocalAddress(temp);    // kvp value*
                    }

                    var hasValue = elementType.GetProperty("HasValue").GetMethod;

                    Emit.Call(hasValue);                // kvp bool
                }

                Emit.BranchIfTrue(doWrite);             // kvp
                for (var i = 0; i < ontheStack; i++)
                {
                    Emit.Pop();
                }
                Emit.Branch(done);                      // --empty--

                Emit.MarkLabel(doWrite);                // kvp
            }

            var skipComma = Emit.DefineLabel();

            Emit.LoadLocal(isFirst);                // kvp bool
            Emit.BranchIfTrue(skipComma);           // kvp

            WriteString(",");

            Emit.MarkLabel(skipComma);              // kvp

            Emit.LoadConstant(false);               // kvp false
            Emit.StoreLocal(isFirst);               // kvp

            if (PrettyPrint)
            {
                LineBreakAndIndent();
            }

            if (keyIsString)
            {
                WriteString("\"");  // kvp

                Emit.Duplicate();       // kvp kvp
                LoadProperty(key);      // kvp string

                using (var str = Emit.DeclareLocal<string>())
                {
                    Emit.StoreLocal(str);   // kvp
                    Emit.LoadArgument(0);   // kvp TextWriter
                    Emit.LoadLocal(str);    // kvp TextWriter string

                    Emit.Call(GetWriteEncodedStringMethod());   // kvp
                }

                if (PrettyPrint)
                {
                    WriteString("\": ");
                }
                else
                {
                    WriteString("\":");
                }
            }
            else
            {
                if (keyIsNumber)
                {
                    WriteString("\"");          // kvp

                    Emit.Duplicate();           // kvp kvp
                    LoadProperty(key);          // kvp number
                    using (var loc = Emit.DeclareLocal(keyType))
                    {
                        Emit.StoreLocal(loc);   // kvp
                        Emit.LoadArgument(0);   // kvp TextWriter
                        Emit.LoadLocal(loc);    // kvp TextWriter number

                    }

                    WritePrimitive(keyType, quotesNeedHandling: false); // kvp

                    if (PrettyPrint)
                    {
                        WriteString("\": ");        // kvp
                    }
                    else
                    {
                        WriteString("\":");         // kvp
                    }
                }
                else
                {
                    Emit.Duplicate();       // kvp kvp
                    LoadProperty(key);      // kvp enum

                    WriteEnumOrPrimitive(dictionaryMember, keyType, true, hasTextWriter: false, popTextWriter: false);

                    if (PrettyPrint)
                    {
                        WriteString(": ");
                    }
                    else
                    {
                        WriteString(":");         // kvp
                    }
                }
            }

            LoadProperty(value);        // elementType

            if (elementType.IsPrimitiveType())
            {
                WritePrimitive(elementType, quotesNeedHandling: true);

                Emit.MarkLabel(done);

                return;
            }

            if (elementType.IsNullableType())
            {
                WriteNullable(dictionaryMember, elementType, quotesNeedHandling: true);

                Emit.MarkLabel(done);

                return;
            }

            if (elementType._IsEnum())
            {
                WriteEnumOrPrimitive(dictionaryMember, elementType, requiresQuotes: false, hasTextWriter: true, popTextWriter: false);
                return;
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            if (isRecursive)
            {
                // Stack is:
                //  - elementType
                //  - TextWriter
                //  - Action<TextWriter, elementType, int>

                Type recursiveAct;
                if (BuildingToString)
                {
                    recursiveAct = typeof(StringThunkDelegate<>).MakeGenericType(elementType);
                }
                else
                {
                    recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), elementType, typeof(int));
                }
                var invoke = recursiveAct.GetMethod("Invoke");

                Emit.LoadArgument(2);       // Action<TextWriter, elementType, int> TextWriter elementType int
                Emit.Call(invoke);          // --empty--

                Emit.MarkLabel(done);

                return;
            }

            using (var loc = Emit.DeclareLocal(elementType))
            {
                Emit.StoreLocal(loc);

                if (elementType.IsListType())
                {
                    WriteList(dictionaryMember, elementType, loc);

                    Emit.MarkLabel(done);

                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteDictionary(dictionaryMember, elementType, loc);

                    Emit.MarkLabel(done);

                    return;
                }

                if (elementType.IsEnumerableType())
                {
                    WriteEnumerable(dictionaryMember, elementType, loc);

                    Emit.MarkLabel(done);

                    return;
                }

                WriteObject(elementType, loc);
            }

            Emit.MarkLabel(done);
        }

        public MethodInfo GetWriteEncodedStringWithQuotesMethod()
        {
            return 
                JSONP ? 
                    Methods.GetWriteEncodedStringWithQuotesWithNullsInlineJSONPUnsafe(BuildingToString) : 
                    Methods.GetWriteEncodedStringWithQuotesWithNullsInlineUnsafe(BuildingToString);
        }

        MethodInfo GetWriteEncodedStringMethod()
        {
            return
                JSONP ? 
                    Methods.GetWriteEncodedStringWithNullsInlineJSONPUnsafe(BuildingToString) : 
                    Methods.GetWriteEncodedStringWithNullsInlineUnsafe(BuildingToString);
        }

        void WriteKeyValue(MemberInfo dictionaryMember, Type keyType, Type elementType)
        {
            // top of the stack is a KeyValue<keyType, elementType>

            var keyIsString = keyType == typeof(string);
            var keyIsNumber = keyType.IsIntegerNumberType();

            var keyValuePair = typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);
            var key = keyValuePair.GetProperty("Key");
            var value = keyValuePair.GetProperty("Value");

            if (PrettyPrint)
            {
                LineBreakAndIndent();
            }

            if (keyIsString)
            {
                WriteString("\"");

                Emit.Duplicate();           // kvp kvp
                LoadProperty(key);          // kvp string

                using (var str = Emit.DeclareLocal<string>())
                {
                    Emit.StoreLocal(str);   // kvp
                    Emit.LoadArgument(0);   // kvp TextWriter
                    Emit.LoadLocal(str);    // kvp TextWriter string

                    Emit.Call(GetWriteEncodedStringMethod()); // kvp
                }

                if (PrettyPrint)
                {
                    WriteString("\": ");        // kvp
                }
                else
                {
                    WriteString("\":");         // kvp
                }
            }
            else
            {
                if (keyIsNumber)
                {
                    WriteString("\"");

                    Emit.Duplicate();           // kvp kvp
                    LoadProperty(key);          // kvp number
                    using (var loc = Emit.DeclareLocal(keyType))
                    {
                        Emit.StoreLocal(loc);   // kvp
                        Emit.LoadArgument(0);   // kvp TextWriter
                        Emit.LoadLocal(loc);    // kvp TextWriter number

                    }

                    WritePrimitive(keyType, quotesNeedHandling: false); // kvp

                    if (PrettyPrint)
                    {
                        WriteString("\": ");        // kvp
                    }
                    else
                    {
                        WriteString("\":");         // kvp
                    }
                }
                else
                {
                    Emit.Duplicate();           // kvp kvp
                    LoadProperty(key);          // kvp enum

                    WriteEnumOrPrimitive(dictionaryMember, keyType, requiresQuotes: true, hasTextWriter: false, popTextWriter: false);   // kvp

                    if (PrettyPrint)
                    {
                        WriteString(": ");        // kvp
                    }
                    else
                    {
                        WriteString(":");         // kvp
                    }
                }
            }

            LoadProperty(value);        // elementType

            if (elementType.IsPrimitiveType())
            {
                WritePrimitive(elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType.IsNullableType())
            {
                WriteNullable(dictionaryMember, elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType._IsEnum())
            {
                WriteEnumOrPrimitive(dictionaryMember, elementType, requiresQuotes: false, hasTextWriter: true, popTextWriter: false);
                return;
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            if (isRecursive)
            {
                // Stack is:
                //  - elementType
                //  - TextWriter
                //  - Action<TextWriter, elementType, int>

                Type recursiveAct;
                if (BuildingToString)
                {
                    recursiveAct = typeof(StringThunkDelegate<>).MakeGenericType(elementType);
                }
                else
                {
                    recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), elementType, typeof(int));
                }
                var invoke = recursiveAct.GetMethod("Invoke");

                Emit.LoadArgument(2);       // Action<TextWriter, elementType, int> TextWriter elementType int
                Emit.Call(invoke);          // --empty--

                return;
            }

            using (var loc = Emit.DeclareLocal(elementType))
            {
                Emit.StoreLocal(loc);

                if (elementType.IsListType())
                {
                    WriteList(dictionaryMember, elementType, loc);
                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteDictionary(dictionaryMember, elementType, loc);
                    return;
                }

                if (elementType.IsEnumerableType())
                {
                    WriteEnumerable(dictionaryMember, elementType, loc);
                    return;
                }

                WriteObject(elementType, loc);
            }
        }

        bool ValuesAreContiguous(Dictionary<ulong, object> values)
        {
            var min = values.Keys.Min();
            var max = values.Keys.Max();

            ulong i = 0;

            while ((min + i) != max)
            {
                if (!values.ContainsKey(min + i))
                {
                    return false;
                }

                i++;
            }

            return true;
        }

        void WriteContiguousEnumeration(Type enumType, Dictionary<ulong, object> values, bool popTextWriter)
        {
            // top of stack
            //   - enum
            //   - TextWriter?

            var done = Emit.DefineLabel();

            var min = values.Keys.Min();
            var max = values.Keys.Max();

            var labels = Enumerable.Range(0, (int)(max - min + 1)).Select(_ => Emit.DefineLabel()).ToArray();

            Emit.Convert<ulong>();      // TextWriter? ulong
            Emit.LoadConstant(min);     // TextWriter? ulong ulong
            Emit.Subtract();            // TextWriter? ulong
            Emit.Convert<int>();        // TextWriter? int
            Emit.Switch(labels);        // TextWriter?

            // default (ie. no match)
            Emit.LoadConstant("Unexpected value for enumeration " + enumType.FullName);
            Emit.NewObject(typeof(InvalidOperationException), typeof(string));
            Emit.Throw();

            for (ulong i = 0; i < (ulong)labels.Length; i++)
            {
                var val = values[min + i];
                var label = labels[(int)i];
                var asStr = enumType.GetEnumValueName(val);
                var escapedString = "\"" + asStr.JsonEscape(JSONP) + "\"";

                Emit.MarkLabel(label);      // TextWriter?
                WriteString(escapedString); // TextWriter?
                Emit.Branch(done);          // TextWriter?
            }

            Emit.MarkLabel(done);           // TextWriter?
            
            if (popTextWriter)
            {
                Emit.Pop();
            }
        }

        void LoadConstantOfType(object val, Type type)
        {
            if (!Utils.LoadConstantOfType(Emit, val, type))
            {
                throw new ConstructionException("Unexpected type: " + type);
            }
        }
        
        void WriteDiscontiguousEnumeration(Type enumType, bool popTextWriter)
        {
            // top of stack
            //   - enum
            //   - TextWriter?

            var underlyingType = Enum.GetUnderlyingType(enumType);

            var done = Emit.DefineLabel();

            Emit.Convert(underlyingType);   // TextWriter? val

            foreach (var val in Enum.GetValues(enumType).Cast<object>())
            {
                var name = enumType.GetEnumValueName(val);

                var escapeStr = "\"" + name.JsonEscape(JSONP) + "\"";

                var next = Emit.DefineLabel();

                Emit.Duplicate();                       // TextWriter? val val
                LoadConstantOfType(val, underlyingType);// TextWriter? val val val
                Emit.UnsignedBranchIfNotEqual(next);    // TextWriter? val

                WriteString(escapeStr);                 // TextWriter? val
                Emit.Branch(done);                      // TextWriter? val

                Emit.MarkLabel(next);                   // TextWriter? val
            }

            // TextWriter? val
            Emit.Pop();                                                                     // TextWriter?
            Emit.LoadConstant("Unexpected value for enumeration " + enumType.FullName);     // string
            Emit.NewObject(typeof(InvalidOperationException), typeof(string));              // InvalidOperationException
            Emit.Throw();                                                                   // --empty--

            Emit.MarkLabel(done);   // TextWriter? val
            Emit.Pop();             // TextWriter?

            if (popTextWriter)
            {
                Emit.Pop();         // --empty--
            }
        }

        void WriteFlagsEnum(Type enumType, Dictionary<ulong, object> values, bool popTextWriter)
        {
            // top of stack
            //   - enum
            //   - TextWriter?

            var hasZeroValue = values.ContainsKey(0UL);
            var done = Emit.DefineLabel();
            Sigil.Label notZero = null;

            Emit.Convert<ulong>();      // TextWriter? ulong

            // gotta special case this, since 0 & 0 == 0
            if (hasZeroValue)
            {
                notZero = Emit.DefineLabel();

                Emit.Duplicate();                       // TextWriter? ulong ulong
                Emit.LoadConstant(0UL);                 // TextWriter? ulong ulong 0
                Emit.UnsignedBranchIfNotEqual(notZero); // TextWriter? ulong

                var zeroStr = "\"" + enumType.GetEnumValueName(values[0UL]).JsonEscape(JSONP) + "\"";
                WriteString(zeroStr);                   // TextWriter? ulong
                Emit.Pop();                             // TextWriter?
                if (popTextWriter)
                {
                    Emit.Pop();                         // --empty--
                }
                Emit.Branch(done);                      // --empty--
            }

            if (notZero != null)
            {
                Emit.MarkLabel(notZero);        // TextWriter? ulong
            }

            using (var enumLoc = Emit.DeclareLocal<ulong>())
            using (var notFirst = Emit.DeclareLocal<bool>())
            {
                Emit.StoreLocal(enumLoc);       // TextWriter?
                if (popTextWriter)
                {
                    Emit.Pop();                 // --empty--
                }

                WriteString("\"");              // --empty--

                foreach (var valObj in values.Where(_ => _.Key != 0UL).OrderBy(_ => _.Key))
                {
                    var skip = Emit.DefineLabel();
                    var skipCommaSpace = Emit.DefineLabel();

                    var asULong = valObj.Key;
                    var asStr = enumType.GetEnumValueName(valObj.Value).JsonEscape(JSONP);

                    Emit.LoadLocal(enumLoc);        // ulong
                    Emit.LoadConstant(asULong);     // ulong ulong
                    Emit.And();                     // ulong
                    Emit.LoadConstant(0UL);         // ulong 0
                    Emit.BranchIfEqual(skip);       // --empty--

                    Emit.LoadLocal(notFirst);           // bool
                    Emit.BranchIfFalse(skipCommaSpace); // --empty--
                    if (PrettyPrint)
                    {
                        WriteString(", ");              // --empty--
                    }
                    else
                    {
                        WriteString(",");               // --empty--
                    }

                    Emit.MarkLabel(skipCommaSpace);     // --emmpty-
                    WriteString(asStr);                 // --empty--
                    Emit.LoadConstant(true);            // bool
                    Emit.StoreLocal(notFirst);          // --empty--

                    // mask that field out so we can tell if the value in Flags isn't actually a known value
                    Emit.LoadLocal(enumLoc);        // ulong
                    Emit.LoadConstant(~asULong);    // ulong ulong
                    Emit.And();                     // ulong
                    Emit.StoreLocal(enumLoc);       // --empty--

                    Emit.MarkLabel(skip);           // --empty--
                }

                WriteString("\"");

                Emit.LoadLocal(enumLoc);    // ulong
                Emit.LoadConstant(0UL);     // ulong 0
                Emit.BranchIfEqual(done);   // --empty--

                Emit.LoadConstant("Unexpected value for flags enumeration " + enumType.FullName);   // string
                Emit.NewObject(typeof(InvalidOperationException), typeof(string));                  // InvalidOperationException
                Emit.Throw();                                                                       // --empty--
            }

            Emit.MarkLabel(done);
        }

        void WriteEnumOrPrimitive(MemberInfo member, Type enumType, bool requiresQuotes, bool hasTextWriter, bool popTextWriter)
        {
            // top of stack
            //   - enum
            //   - TextWriter?

            Type convertEnumTo;
            if (member != null && member.ShouldConvertEnum(enumType, out convertEnumTo))
            {
                var underlying = Enum.GetUnderlyingType(enumType);
                Emit.Convert(underlying);
                if (underlying != convertEnumTo)
                {
                    Emit.Convert(convertEnumTo);
                }

                if (requiresQuotes)
                {
                    WriteString("\"");
                }

                if (!hasTextWriter)
                {
                    using (var local = Emit.DeclareLocal(enumType))
                    {
                        Emit.StoreLocal(local); // -empty-
                        Emit.LoadArgument(0);   // TextWriter
                        Emit.LoadLocal(local);  // TextWriter enum
                    }
                }

                WritePrimitive(convertEnumTo, quotesNeedHandling: false);

                if (requiresQuotes)
                {
                    WriteString("\"");
                }

                return;
            }
            WriteEnum(enumType, popTextWriter);
        }

        void WriteEnum(Type enumType, bool popTextWriter)
        {
            var allValues = Enum.GetValues(enumType);
            var underlying = Enum.GetUnderlyingType(enumType);

            IEnumerable<Tuple<object, ulong>> asUlongs = null;
            if(underlying == typeof(byte))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)(byte)v));
            }
            if(underlying == typeof(sbyte))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)(sbyte)v));
            }
            if(underlying == typeof(short))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)(short)v));
            }
            if(underlying == typeof(ushort))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)(ushort)v));
            }
            if(underlying == typeof(int))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)(int)v));
            }
            if(underlying == typeof(uint))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)(uint)v));
            }
            if(underlying == typeof(long))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)(long)v));
            }
            if(underlying == typeof(ulong))
            {
                asUlongs = allValues.Cast<object>().Select(v => Tuple.Create(v, (ulong)v));
            }

            var distinctValues = asUlongs.GroupBy(g => g.Item2).ToDictionary(g => g.Key, g => g.First().Item1);

            if (enumType.IsFlagsEnum())
            {
                WriteFlagsEnum(enumType, distinctValues, popTextWriter);
                return;
            }

            if (ValuesAreContiguous(distinctValues))
            {
                WriteContiguousEnumeration(enumType, distinctValues, popTextWriter);
            }
            else
            {
                WriteDiscontiguousEnumeration(enumType, popTextWriter);
            }
        }

        Dictionary<Type, Sigil.Local> PreloadRecursiveTypes(HashSet<Type> recursiveTypes)
        {
            var ret = new Dictionary<Type, Sigil.Local>();

            foreach (var type in recursiveTypes)
            {
                if (ShouldDynamicCallOut(type))
                {
                    var recursiveSerializerCache = typeof(RecursiveSerializerCache<>).MakeGenericType(type);
                    var getMtd = (MethodInfo)recursiveSerializerCache.GetField("GetFor").GetValue(null);

                    var loc = Emit.DeclareLocal(getMtd.ReturnType);
                    Emit.LoadConstant(this.PrettyPrint);        // bool
                    Emit.LoadConstant(this.ExcludeNulls);       // bool bool
                    Emit.LoadConstant(this.JSONP);              // bool bool bool
                    Emit.LoadConstant((byte)this.DateFormat);   // bool bool bool byte
                    Emit.LoadConstant(this.IncludeInherited);   // bool bool bool DateTimeFormat bool
                    Emit.Call(getMtd);                          // Action<TextWriter, type, int>)
                    Emit.StoreLocal(loc);                       // --empty--

                    ret[type] = loc;
                }
                else
                {
                    // static case
                    var cacheType = typeof(TypeCache<,>).MakeGenericType(RecursionLookupOptionsType, type);
                    FieldInfo thunk;

                    if (BuildingToString)
                    {
                        thunk = cacheType.GetField("StringThunk", BindingFlags.Public | BindingFlags.Static);
                    }
                    else
                    {
                        thunk = cacheType.GetField("Thunk", BindingFlags.Public | BindingFlags.Static);
                    }

                    var loc = Emit.DeclareLocal(thunk.FieldType);

                    Emit.LoadField(thunk);  // Action<TextWriter, type, int>
                    Emit.StoreLocal(loc);   // --empty--

                    ret[type] = loc;
                }
            }

            return ret;
        }

        void AddCharBuffer(Type serializingType)
        {
            // Don't tax the naive implementations by allocating a buffer they don't use
            if (!(UseCustomIntegerToString || UseFastGuids || UseCustomISODateFormatting)) return;

            var allTypes = serializingType.InvolvedTypes();

            var hasGuids = allTypes.Any(t => t == typeof(Guid));
            var hasDateTime = allTypes.Any(t => t == typeof(DateTime) || t == typeof(DateTimeOffset));
            var hasInteger = allTypes.Any(t => t.IsIntegerNumberType());

            // Not going to use a buffer?  Don't allocate it
            if (!hasGuids && !hasDateTime && !hasInteger)
            {
                return;
            }

            Emit.DeclareLocal<char[]>(CharBuffer);
            Emit.LoadConstant(CharBufferSize);
            Emit.NewArray<char>();
            Emit.StoreLocal(CharBuffer);
        }

        Emit MakeEmit(Type forType)
        {
            if (BuildingToString)
            {
                return Emit.NewDynamicMethod(typeof(void), new[] { typeof(ThunkWriter).MakeByRefType(), typeof(ForType), typeof(int) }, doVerify: Utils.DoVerify);
            }
            else
            {
                return Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) }, doVerify: Utils.DoVerify);
            }
        }

        void BuildObjectWithNewImpl()
        {
            var recursiveTypes = FindAndPrimeRecursiveOrReusedTypes(typeof(ForType));

            Emit = MakeEmit(typeof(ForType));

            // dirty trick here, we can prove that overflowing is *impossible* if there are no recursive types
            //   If that's the case, don't even bother with the check or the increment
            if (recursiveTypes.Count != 0)
            {
                var goOn = Emit.DefineLabel();

                Emit.LoadArgument(2);               // int
                Emit.LoadConstant(RecursionLimit);  // int int
                Emit.BranchIfLess(goOn);            // --empty--

                Emit.NewObject(typeof(InfiniteRecursionException)); // infiniteRecursionException
                Emit.Throw();                                       // --empty--

                Emit.MarkLabel(goOn);               // --empty--
            }

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            WriteObject(typeof(ForType));
            Emit.Return();
        }

        Action<TextWriter, ForType, int> BuildObjectWithNewDelegate()
        {
            BuildObjectWithNewImpl();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>(Utils.DelegateOptimizationOptions);
        }

        StringThunkDelegate<ForType> BuildObjectWithNewDelegateToString()
        {
            BuildObjectWithNewImpl();

            //return Emit.CreateDelegate<StringThunkDelegate<ForType>>(Utils.DelegateOptimizationOptions);

            string ops;
            var ret = Emit.CreateDelegate<StringThunkDelegate<ForType>>(out ops, Utils.DelegateOptimizationOptions);

            return ret;
        }

        HashSet<Type> FindAndPrimeRecursiveOrReusedTypes(Type forType)
        {
            // if we're serializing dynamically, we can't actually preload
            if (CallOutOnPossibleDynamic) return new HashSet<Type>();

            var ret = forType.FindRecursiveOrReusedTypes();
            foreach (var primeType in ret)
            {
                MethodInfo loadMtd;
                if (BuildingToString)
                {
                    loadMtd = typeof(TypeCache<,>).MakeGenericType(RecursionLookupOptionsType, primeType).GetMethod("LoadToString", BindingFlags.Public | BindingFlags.Static);
                }
                else
                {
                    loadMtd = typeof(TypeCache<,>).MakeGenericType(RecursionLookupOptionsType, primeType).GetMethod("Load", BindingFlags.Public | BindingFlags.Static);
                }

                loadMtd.Invoke(null, new object[0]);
            }

            return ret;
        }

        void BuildListWithNewImpl(MemberInfo dynamicMember)
        {
            var recursiveTypes = FindAndPrimeRecursiveOrReusedTypes(typeof(ForType));

            Emit = MakeEmit(typeof(ForType));

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            WriteList(dynamicMember, typeof(ForType));
            Emit.Return();
        }

        Action<TextWriter, ForType, int> BuildListWithNewDelegate(MemberInfo dynamicMember)
        {
            BuildListWithNewImpl(dynamicMember);

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>(Utils.DelegateOptimizationOptions);
        }

        StringThunkDelegate<ForType> BuildListWithNewDelegateToString()
        {
            BuildListWithNewImpl(null);

            return Emit.CreateDelegate<StringThunkDelegate<ForType>>(Utils.DelegateOptimizationOptions);
        }

        void BuildEnumerableWithNewImpl(MemberInfo dynamicMember)
        {
            var recursiveTypes = FindAndPrimeRecursiveOrReusedTypes(typeof(ForType));

            Emit = MakeEmit(typeof(ForType));

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            WriteEnumerable(dynamicMember, typeof(ForType));
            Emit.Return();
        }

        Action<TextWriter, ForType, int> BuildEnumerableWithNewDelegate(MemberInfo dynamicMember)
        {
            BuildEnumerableWithNewImpl(dynamicMember);

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>(Utils.DelegateOptimizationOptions);
        }

        StringThunkDelegate<ForType> BuildEnumerableWithNewDelegateToString()
        {
            BuildEnumerableWithNewImpl(null);

            return Emit.CreateDelegate<StringThunkDelegate<ForType>>(Utils.DelegateOptimizationOptions);
        }

        void BuildDictionaryWithNewImpl(MemberInfo dynamicMember)
        {
            var recursiveTypes = FindAndPrimeRecursiveOrReusedTypes(typeof(ForType));

            Emit = MakeEmit(typeof(ForType));

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            WriteDictionary(dynamicMember, typeof(ForType));
            Emit.Return();
        }

        Action<TextWriter, ForType, int> BuildDictionaryWithNewDelegate(MemberInfo dynamicMember)
        {
            BuildDictionaryWithNewImpl(dynamicMember);   

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>(Utils.DelegateOptimizationOptions);
        }

        StringThunkDelegate<ForType> BuildDictionaryWithNewDelegateToString()
        {
            BuildDictionaryWithNewImpl(null);

            return Emit.CreateDelegate<StringThunkDelegate<ForType>>(Utils.DelegateOptimizationOptions);
        }

        void BuildPrimitiveWithNewImpl()
        {
            Emit = MakeEmit(typeof(ForType));

            AddCharBuffer(typeof(ForType));

            Emit.LoadArgument(0);
            Emit.LoadArgument(1);

            WritePrimitive(typeof(ForType), quotesNeedHandling: true);

            Emit.Return();
        }

        Action<TextWriter, ForType, int> BuildPrimitiveWithNewDelegate()
        {
            BuildPrimitiveWithNewImpl();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>(Utils.DelegateOptimizationOptions);
        }

        StringThunkDelegate<ForType> BuildPrimitiveWithNewDelegateToString()
        {
            BuildPrimitiveWithNewImpl();

            return Emit.CreateDelegate<StringThunkDelegate<ForType>>(Utils.DelegateOptimizationOptions);
        }

        void BuildNullableWithNewImpl(MemberInfo dynamicMember)
        {
            var recursiveTypes = FindAndPrimeRecursiveOrReusedTypes(typeof(ForType));

            Emit = MakeEmit(typeof(ForType));

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            Emit.LoadArgument(0);
            Emit.LoadArgument(1);

            WriteNullable(dynamicMember, typeof(ForType), quotesNeedHandling: true);

            Emit.Return();
        }

        Action<TextWriter, ForType, int> BuildNullableWithNewDelegate(MemberInfo dynamicMember)
        {
            BuildNullableWithNewImpl(dynamicMember);

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>(Utils.DelegateOptimizationOptions);
        }

        StringThunkDelegate<ForType> BuildNullableWithNewDelegateToString()
        {
            BuildNullableWithNewImpl(null);

            return Emit.CreateDelegate<StringThunkDelegate<ForType>>(Utils.DelegateOptimizationOptions);
        }

        void BuildEnumWithNewImpl(MemberInfo dynamicMember)
        {
            Emit = MakeEmit(typeof(ForType));

            if (dynamicMember != null)
            {
                Emit.LoadArgument(0); // TextWriter
                Emit.LoadArgument(1); // ForType
                WriteEnumOrPrimitive(dynamicMember, typeof(ForType), requiresQuotes: false, hasTextWriter: true, popTextWriter: false);
            }
            else
            {
                Emit.LoadArgument(1); // ForType
                WriteEnum(typeof(ForType), popTextWriter: false);
            }

            Emit.Return();
        }

        Action<TextWriter, ForType, int> BuildEnumWithNewDelegate(MemberInfo dynamicMember)
        {
            BuildEnumWithNewImpl(dynamicMember);

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>(Utils.DelegateOptimizationOptions);
        }

        StringThunkDelegate<ForType> BuildEnumWithNewDelegateToString()
        {
            BuildEnumWithNewImpl(null);

            return Emit.CreateDelegate<StringThunkDelegate<ForType>>(Utils.DelegateOptimizationOptions);
        }

        internal Action<TextWriter, ForType, int> Build(MemberInfo dynamicMember)
        {
            var forType = typeof(ForType);

            if (forType.IsNullableType())
            {
                return BuildNullableWithNewDelegate(dynamicMember);
            }

            if (forType.IsPrimitiveType())
            {
                return BuildPrimitiveWithNewDelegate();
            }

            if (forType.IsDictionaryType() || forType.IsReadOnlyDictionaryType())
            {
                return BuildDictionaryWithNewDelegate(dynamicMember);
            }

            if (forType.IsListType())
            {
                return BuildListWithNewDelegate(dynamicMember);
            }

            if (forType._IsEnum())
            {
                return BuildEnumWithNewDelegate(dynamicMember);
            }

            if (forType.IsEnumerableType())
            {
                return BuildEnumerableWithNewDelegate(dynamicMember);
            }

            return BuildObjectWithNewDelegate();
        }

        internal StringThunkDelegate<ForType> BuildToString()
        {
            var forType = typeof(ForType);

            if (forType.IsNullableType())
            {
                return BuildNullableWithNewDelegateToString();
            }

            if (forType.IsPrimitiveType())
            {
                return BuildPrimitiveWithNewDelegateToString();
            }

            if (forType.IsDictionaryType() || forType.IsReadOnlyDictionaryType())
            {
                return BuildDictionaryWithNewDelegateToString();
            }

            if (forType.IsListType())
            {
                return BuildListWithNewDelegateToString();
            }

            if (forType._IsEnum())
            {
                return BuildEnumWithNewDelegateToString();
            }

            if (forType.IsEnumerableType())
            {
                return BuildEnumerableWithNewDelegateToString();
            }

            return BuildObjectWithNewDelegateToString();
        }
    }

    static class InlineSerializerHelper
    {
        static Action<TextWriter, BuildForType, int> BuildAlwaysFailsWith<BuildForType>(Type optionsType)
        {
            var specificTypeCache = typeof(TypeCache<,>).MakeGenericType(optionsType, typeof(BuildForType));
            var stashField = specificTypeCache.GetField("ThunkExceptionDuringBuild", BindingFlags.Static | BindingFlags.Public);

            var emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(BuildForType), typeof(int) });
            emit.LoadConstant("Error occurred building a serializer for " + typeof(BuildForType));
            emit.LoadField(stashField);
            emit.NewObject<Exception, string, Exception>();
            emit.Throw();

            return emit.CreateDelegate<Action<TextWriter, BuildForType, int>>(Utils.DelegateOptimizationOptions);
        }

        static StringThunkDelegate<BuildForType> BuildAlwaysFailsWithToString<BuildForType>(Type optionsType)
        {
            var specificTypeCache = typeof(TypeCache<,>).MakeGenericType(optionsType, typeof(BuildForType));
            var stashField = specificTypeCache.GetField("StringThunkExceptionDuringBuild", BindingFlags.Static | BindingFlags.Public);

            var emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(ThunkWriter).MakeByRefType(), typeof(BuildForType), typeof(int) });
            emit.LoadConstant("Error occurred building a serializer for " + typeof(BuildForType));
            emit.LoadField(stashField);
            emit.NewObject<Exception, string, Exception>();
            emit.Throw();

            return emit.CreateDelegate<StringThunkDelegate<BuildForType>>(Utils.DelegateOptimizationOptions);
        }

        public static Action<TextWriter, BuildForType, int> Build<BuildForType>(Type optionsType, bool pretty, bool excludeNulls, bool jsonp, DateTimeFormat dateFormat, bool includeInherited, UnspecifiedDateTimeKindBehavior dateTimeBehavior, SerializationNameFormat serializationNameFormat, out Exception exceptionDuringBuild)
        {
            Action<TextWriter, BuildForType, int> ret;
            try
            {
                var obj = new InlineSerializer<BuildForType>(optionsType, pretty, excludeNulls, jsonp, dateFormat, includeInherited, dateTimeBehavior, serializationNameFormat, false, false);

                ret = obj.Build(null);
                exceptionDuringBuild = null;
            }
            catch (ConstructionException e)
            {
                exceptionDuringBuild = e;
                ret = BuildAlwaysFailsWith<BuildForType>(optionsType);
            }

            return ret;
        }

        public static readonly MethodInfo BuildWithDynamism = typeof(InlineSerializerHelper).GetMethod("_BuildWithDynamism", BindingFlags.Static | BindingFlags.NonPublic);
        private static Action<TextWriter, BuildForType, int> _BuildWithDynamism<BuildForType>(MemberInfo dynamicMember, Type optionsType, bool pretty, bool excludeNulls, bool jsonp, DateTimeFormat dateFormat, bool includeInherited, UnspecifiedDateTimeKindBehavior dateTimeBehavior, SerializationNameFormat serializationNameFormat)
        {
            var obj = new InlineSerializer<BuildForType>(optionsType, pretty, excludeNulls, jsonp, dateFormat, includeInherited, dateTimeBehavior, serializationNameFormat, true, false);
            return obj.Build(dynamicMember);
        }

        public static StringThunkDelegate<BuildForType> BuildToString<BuildForType>(Type optionsType, bool pretty, bool excludeNulls, bool jsonp, DateTimeFormat dateFormat, bool includeInherited, UnspecifiedDateTimeKindBehavior dateTimeBehavior, SerializationNameFormat serializationNameFormat, out Exception exceptionDuringBuild)
        {
            StringThunkDelegate<BuildForType> ret;
            try
            {
                var obj = new InlineSerializer<BuildForType>(optionsType, pretty, excludeNulls, jsonp, dateFormat, includeInherited, dateTimeBehavior, serializationNameFormat, false, true);

                ret = obj.BuildToString();

                exceptionDuringBuild = null;
            }
            catch (ConstructionException e)
            {
                exceptionDuringBuild = e;
                ret = BuildAlwaysFailsWithToString<BuildForType>(optionsType);
            }

            return ret;
        }
    }
} 
