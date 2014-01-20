﻿using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;

namespace Jil.Serialize
{
    class InlineSerializer<ForType>
    {
        public static bool ReorderMembers = true;
        public static bool SkipNumberFormatting = true;
        public static bool UseCustomIntegerToString = true;
        public static bool SkipDateTimeMathMethods = true;
        public static bool UseCustomISODateFormatting = true;
        public static bool UseFastLists = true;
        public static bool UseFastArrays = true;
        public static bool UseFastGuids = true;
        public static bool AllocationlessDictionaries = true;

        static string CharBuffer = "char_buffer";
        internal const int CharBufferSize = 36;
        internal const int RecursionLimit = 50;

        static Dictionary<char, string> CharacterEscapes = 
            new Dictionary<char, string>{
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

        private readonly Type RecusionLookupType;
        private readonly bool ExcludeNulls;
        private readonly bool PrettyPrint;
        private readonly bool JSONP;
        private readonly DateTimeFormat DateFormat;
        private readonly bool IncludeInherited;
        
        private Dictionary<Type, Sigil.Local> RecursiveTypes;

        private Emit Emit;

        internal InlineSerializer(Type recusionLookupType, bool pretty, bool excludeNulls, bool jsonp, DateTimeFormat dateFormat, bool includeInherited)
        {
            RecusionLookupType = recusionLookupType;
            PrettyPrint = pretty;
            ExcludeNulls = excludeNulls;
            JSONP = jsonp;
            DateFormat = dateFormat;
            IncludeInherited = includeInherited;
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

        static MethodInfo TextWriter_WriteString = typeof(TextWriter).GetMethod("Write", new[] { typeof(string) });
        void WriteString(string str)
        {
            Emit.LoadArgument(0);
            Emit.LoadConstant(str);
            Emit.CallVirtual(TextWriter_WriteString);
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

            var props = forType.GetProperties(flags).Where(p => p.GetMethod != null);
            var fields = forType.GetFields(flags);

            var members = props.Cast<MemberInfo>().Concat(fields);

            if (forType.IsValueType)
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

        

        void WriteMember(MemberInfo member, Sigil.Local inLocal = null)
        {
            // Stack is empty

            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new Exception("Wha?");

            var serializingType = asField != null ? asField.FieldType : asProp.PropertyType;

            // It's a list or dictionary, go and build that code
            if (serializingType.IsListType() || serializingType.IsDictionaryType())
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

                    if (serializingType.IsListType())
                    {
                        WriteList(serializingType, loc);
                        return;
                    }

                    if (serializingType.IsDictionaryType())
                    {
                        WriteDictionary(serializingType, loc);
                        return;
                    }

                    WriteList(serializingType, loc);
                }

                return;
            }

            var isRecursive = RecursiveTypes.ContainsKey(serializingType);

            if (isRecursive)
            {
                Emit.LoadLocal(RecursiveTypes[serializingType]);    // Action<TextWriter, serializingType>
            }

            // Only put this on the stack if we'll need it
            var preloadTextWriter = serializingType.IsPrimitiveType() || isRecursive || serializingType.IsNullableType();
            if (preloadTextWriter)
            {
                Emit.LoadArgument(0);   // TextWriter
            }

            if (inLocal == null)
            {
                Emit.LoadArgument(1);       // TextWriter obj
            }
            else
            {
                Emit.LoadLocal(inLocal);    // TextWriter obj
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

                var recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), serializingType, typeof(int));
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
                WriteNullable(serializingType, quotesNeedHandling: true);
                return;
            }

            if (serializingType.IsEnum)
            {
                WriteEnum(serializingType, popTextWriter: false);
                return;
            }

            using (var loc = Emit.DeclareLocal(serializingType))
            {
                Emit.StoreLocal(loc);   // TextWriter;

                WriteObject(serializingType, loc);
            }
        }

        void WriteNullable(Type nullableType, bool quotesNeedHandling)
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
                if (!ExcludeNulls)
                {
                    WriteString("null");        // --empty--
                }
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
                if (underlyingType.IsEnum)
                {
                    WriteEnum(underlyingType, popTextWriter: true);
                }
                else
                {
                    using (var loc = Emit.DeclareLocal(underlyingType))
                    {
                        Emit.StoreLocal(loc);   // TextWriter

                        if (RecursiveTypes.ContainsKey(underlyingType))
                        {
                            var act = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), underlyingType, typeof(int));
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
                                WriteList(underlyingType, loc);
                            }
                            else
                            {
                                if (underlyingType.IsDictionaryType())
                                {
                                    WriteList(underlyingType, loc);
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

            Emit.MarkLabel(done);
        }

        void WriteNewtonsoftStyleDateTime()
        {
            // top of stack:
            //   - DateTime
            //   - TextWriter

            var toUniversalTime = typeof(DateTime).GetMethod("ToUniversalTime");

            using (var loc = Emit.DeclareLocal<DateTime>())
            {
                Emit.StoreLocal(loc);       // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter DateTime*
                Emit.Call(toUniversalTime); // TextWriter DateTime
                Emit.StoreLocal(loc);       // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter DateTime*
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
            var toUniversalTime = typeof(DateTime).GetMethod("ToUniversalTime");

            using (var loc = Emit.DeclareLocal<DateTime>())
            {
                Emit.StoreLocal(loc);       // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter DateTime*
                Emit.Call(toUniversalTime); // TextWriter DateTime
                Emit.StoreLocal(loc);       // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter DateTime*
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
            var toUniversalTime = typeof(DateTime).GetMethod("ToUniversalTime");

            using (var loc = Emit.DeclareLocal<DateTime>())
            {
                Emit.StoreLocal(loc);       // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter DateTime*
                Emit.Call(toUniversalTime); // TextWriter DateTime
                Emit.StoreLocal(loc);       // TextWriter
                Emit.LoadLocalAddress(loc); // TextWriter DateTime*
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

            var toUniversalTime = typeof(DateTime).GetMethod("ToUniversalTime");

            if (!UseCustomISODateFormatting)
            {
                var toString = typeof(DateTime).GetMethod("ToString", new[] { typeof(string) });

                using (var loc = Emit.DeclareLocal<DateTime>())
                {
                    Emit.StoreLocal(loc);                           // TextWriter
                    Emit.LoadLocalAddress(loc);                     // TextWriter DateTime*
                }

                Emit.Call(toUniversalTime);                         // TextWriter DateTime

                using (var loc = Emit.DeclareLocal<DateTime>())
                {
                    Emit.StoreLocal(loc);       // TextWriter
                    Emit.LoadLocalAddress(loc); // TextWriter DateTime*
                }

                Emit.LoadConstant("\\\"yyyy-MM-ddTHH:mm:ssZ\\\"");      // TextWriter DateTime* string
                Emit.Call(toString);                                // TextWriter string
                Emit.Call(TextWriter_WriteString);                  // --empty--
                return;
            }

            Emit.LoadLocal(CharBuffer);                     // TextWriter DateTime char[]
            Emit.Call(Methods.CustomISO8601ToString);       // --empty--
        }

        void WriteDateTime()
        {
            // top of stack:
            //   - DateTime
            //   - TextWriter

            if (DateFormat == DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch)
            {
                WriteNewtonsoftStyleDateTime();
                return;
            }

            if (DateFormat == DateTimeFormat.MillisecondsSinceUnixEpoch)
            {
                WriteMillisecondsStyleDateTime();
                return;
            }

            if (DateFormat == DateTimeFormat.SecondsSinceUnixEpoch)
            {
                WriteSecondsStyleDateTime();
                return;
            }

            if (DateFormat == DateTimeFormat.ISO8601)
            {
                WriteISO8601StyleDateTime();
                return;
            }

            throw new InvalidOperationException("Unexpected DateFormat: " + DateFormat);
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

            if (primitiveType == typeof(DateTime))
            {
                WriteDateTime();
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

            var needsIntCoersion = primitiveType == typeof(byte) || primitiveType == typeof(sbyte) || primitiveType == typeof(short) || primitiveType == typeof(ushort);

            if (needsIntCoersion)
            {
                Emit.Convert<int>();            // TextWriter int
                primitiveType = typeof(int);
            }

            if (primitiveType == typeof(int) && SkipNumberFormatting)
            {
                var done = Emit.DefineLabel();

                // stack is:
                // TextWriter int

                using (var loc = Emit.DeclareLocal<int>())
                {
                    Emit.StoreLocal(loc);   // TextWriter
                    Emit.LoadLocal(loc);    // TextWriter int

                    Emit.Call(Methods.SwitchWriteInt);      // bool
                    Emit.BranchIfTrue(done);                // --empty--

                    Emit.LoadArgument(0);   // TextWriter
                    Emit.LoadLocal(loc);    // TextWriter int
                }

                if (UseCustomIntegerToString)
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int (ref char[])
                    Emit.Call(Methods.CustomWriteInt);   // --empty--
                }
                else
                {
                    var writeInt = typeof(TextWriter).GetMethod("Write", new[] { typeof(int) });
                    Emit.CallVirtual(writeInt);     // --empty--
                }

                Emit.MarkLabel(done);           // --empty--

                return;
            }

            var builtInMtd = typeof(TextWriter).GetMethod("Write", new[] { primitiveType });

            var isIntegerType = primitiveType == typeof(int) || primitiveType == typeof(uint) || primitiveType == typeof(long) || primitiveType == typeof(ulong);

            if (isIntegerType && UseCustomIntegerToString)
            {
                if (primitiveType == typeof(int))
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int char[]
                    Emit.Call(Methods.CustomWriteInt); // --empty--

                    return;
                }

                if (primitiveType == typeof(uint))
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int char[]
                    Emit.Call(Methods.CustomWriteUInt); // --empty--

                    return;
                }

                if (primitiveType == typeof(long))
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int char[]
                    Emit.Call(Methods.CustomWriteLong); // --empty--

                    return;
                }

                if (primitiveType == typeof(ulong))
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int char[]
                    Emit.Call(Methods.CustomWriteULong); // --empty--

                    return;
                }
            }

            Emit.CallVirtual(builtInMtd);       // --empty--
        }

        void WriteGuidFast(bool quotesNeedHandling)
        {
            if (quotesNeedHandling)
            {
                WriteString("\"");          // TextWriter Guid
            }

            Emit.LoadLocal(CharBuffer);     // TextWriter Guid char[]

            Emit.Call(Methods.WriteGuid);   // --empty--
            
            if (quotesNeedHandling)
            {
                WriteString("\"");          // --empty--
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
            Emit.Call(TextWriter_WriteString);  // --empty--

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

            var writeChar = typeof(TextWriter).GetMethod("Write", new[] { typeof(char) });

            var lowestCharNeedingEncoding = (int)CharacterEscapes.Keys.OrderBy(c => (int)c).First();

            var needLabels = CharacterEscapes.OrderBy(kv => kv.Key).Select(kv => Tuple.Create(kv.Key - lowestCharNeedingEncoding, kv.Value)).ToList();

            var labels = new List<Tuple<Sigil.Label, string>>();

            int? prev = null;
            foreach (var pair in needLabels)
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
            Emit.LoadConstant(lowestCharNeedingEncoding);   // TextWriter char char int
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

            var isValueType = forType.IsValueType;

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
                if (!PrettyPrint)
                {
                    string keyString;
                    if (firstPass)
                    {
                        keyString = "\"" + member.Name.JsonEscape(JSONP) + "\":";
                        firstPass = false;
                    }
                    else
                    {
                        keyString = ",\"" + member.Name.JsonEscape(JSONP) + "\":";
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

                    WriteString("\"" + member.Name.JsonEscape(JSONP) + "\": ");

                    WriteMember(member, inLocal);
                }
            }

            DecreaseIndent();

            WriteString("}");       // --empty--

            Emit.MarkLabel(end);
        }

        void WriteObject(Type forType, Sigil.Local inLocal = null)
        {
            if (!ExcludeNulls)
            {
                WriteObjectWithNulls(forType, inLocal);
            }
            else
            {
                WriteObjectWithoutNulls(forType, inLocal);
            }
        }

        void WriteObjectWithNullsWithConditionalSerialization(Type forType, Sigil.Local inLocal, List<MemberInfo> writeOrder)
        {
            var notNull = Emit.DefineLabel();

            var isValueType = forType.IsValueType;

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

            var isValueType = forType.IsValueType;

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

            if (asField == null && asProp == null) throw new Exception("Wha?");

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

            var canBeNull = serializingType.IsNullableType() || !serializingType.IsValueType;
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

                if (serializingType.IsValueType)
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

            if (PrettyPrint)
            {
                WriteString("\"" + member.Name.JsonEscape(JSONP) + "\": ");     // --empty--
            }
            else
            {
                WriteString("\"" + member.Name.JsonEscape(JSONP) + "\":");      // --empty--
            }

            WriteMember(member, inLocal);           // --empty--

            Emit.MarkLabel(end);
        }

        void WriteMemberConditionally(Type onType, MemberInfo member, Sigil.Local inLocal, Sigil.Local isFirst)
        {
            // top of stack
            //  - obj(*?)

            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new Exception("Wha?");

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

            if (PrettyPrint)
            {
                WriteString("\"" + member.Name.JsonEscape(JSONP) + "\": ");     // --empty--
            }
            else
            {
                WriteString("\"" + member.Name.JsonEscape(JSONP) + "\":");      // --empty--
            }

            WriteMember(member, inLocal);           // --empty--

            Emit.MarkLabel(end);
        }

        void WriteListFast(Type listType, Sigil.Local inLocal = null)
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

            var elementType = listType.GetListInterface().GetGenericArguments()[0];
            var countMtd = listType.GetCollectionInterface().GetProperty("Count").GetMethod;
            var accessorMtd = listType.GetListInterface().GetProperty("Item").GetMethod;

            var iList = typeof(IList<>).MakeGenericType(elementType);

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || isRecursive || elementType.IsNullableType();

            var notNull = Emit.DefineLabel();

            loadList();                         // IList<>

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);         // --empty--
            if (!ExcludeNulls)
            {
                WriteString("null");            // --empty--
            }
            Emit.Branch(end);                   // --empty--

            Emit.MarkLabel(notNull);            // --empty--
            WriteString("[");                   // --empty--

            var done = Emit.DefineLabel();

            using (var e = Emit.DeclareLocal<int>())
            {
                loadList();                                 // IList<>
                Emit.CastClass(iList);                      // IList<>
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

                    WriteElement(elementType);               // --empty--
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

                    WriteElement(elementType);              // --empty--

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

        void WriteArrayFast(Type listType, Sigil.Local inLocal = null)
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

            var iList = typeof(IList<>).MakeGenericType(elementType);

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || isRecursive || elementType.IsNullableType();

            var notNull = Emit.DefineLabel();

            loadArray();                         // type[]

            var end = Emit.DefineLabel();

            Emit.BranchIfTrue(notNull);         // --empty--
            if (!ExcludeNulls)
            {
                WriteString("null");            // --empty--
            }
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

                    WriteElement(elementType);               // --empty--
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

                    WriteElement(elementType);              // --empty--

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

        void WriteList(Type listType, Sigil.Local inLocal = null)
        {
            if (listType.IsArray && UseFastArrays)
            {
                WriteArrayFast(listType, inLocal);
                return;
            }

            if (UseFastLists)
            {
                WriteListFast(listType, inLocal);
                return;
            }

            var elementType = listType.GetListInterface().GetGenericArguments()[0];

            var iEnumerable = typeof(IEnumerable<>).MakeGenericType(elementType);
            var iEnumerableGetEnumerator = iEnumerable.GetMethod("GetEnumerator");
            var enumeratorMoveNext = typeof(System.Collections.IEnumerator).GetMethod("MoveNext");
            var enumeratorCurrent = iEnumerableGetEnumerator.ReturnType.GetProperty("Current");

            var iList = typeof(IList<>).MakeGenericType(elementType);

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            var preloadTextWriter = elementType.IsPrimitiveType() || isRecursive || elementType.IsNullableType();

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
            if (!ExcludeNulls)
            {
                WriteString("null");
            }
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

                Emit.CastClass(iList);                        // IList<>
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

                    WriteElement(elementType);   // --empty--
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

                WriteElement(elementType);   // --empty--

                Emit.Branch(loop);
            }

            Emit.MarkLabel(done);

            WriteString("]");

            Emit.MarkLabel(end);
        }

        void WriteElement(Type elementType)
        {
            if (elementType.IsPrimitiveType())
            {
                WritePrimitive(elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType.IsNullableType())
            {
                WriteNullable(elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType.IsEnum)
            {
                WriteEnum(elementType, popTextWriter: false);
                return;
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            if (isRecursive)
            {
                // Stack is:
                //  - serializingType
                //  - TextWriter
                //  - Action<TextWriter, serializingType>

                var recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), elementType, typeof(int));
                var invoke = recursiveAct.GetMethod("Invoke");

                Emit.LoadArgument(2);

                Emit.Call(invoke);

                return;
            }

            using(var loc = Emit.DeclareLocal(elementType))
            {
                Emit.StoreLocal(loc);

                if (elementType.IsListType())
                {
                    WriteList(elementType, loc);
                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteDictionary(elementType, loc);
                    return;
                }

                WriteObject(elementType, loc);
            }
        }

        void WriteDictionary(Type dictType, Sigil.Local inLocal = null)
        {
            if (!ExcludeNulls)
            {
                WriteDictionaryWithNulls(dictType, inLocal);
            }
            else
            {
                WriteDictionaryWithoutNulls(dictType, inLocal);
            }
        }

        void WriteDictionaryWithoutNulls(Type dictType, Sigil.Local inLocal)
        {
            var dictI = dictType.GetDictionaryInterface();

            var keyType = dictI.GetGenericArguments()[0];
            var elementType = dictI.GetGenericArguments()[1];

            var keyIsString = keyType == typeof(string);
            var keyIsEnum = keyType.IsEnum;
            var keysAreIntegers = keyType.IsIntegerNumberType();

            if (!(keyIsString || keyIsEnum || keysAreIntegers))
            {
                throw new InvalidOperationException("JSON dictionaries must have strings, enums, or integers as keys, found: " + keyType);
            }

            var kvType = typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);

            MethodInfo getEnumerator, enumeratorMoveNext;
            PropertyInfo enumeratorCurrent;
            Action<Sigil.Local> loadEnumerator;
            Action<Sigil.Local> disposeEnumerator;

            if (AllocationlessDictionaries && dictType.IsExactlyDictionaryType())
            {
                var enumerator = dictType.GetNestedType("Enumerator").MakeGenericType(keyType, elementType);
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
            var preloadTextWriter = elementType.IsPrimitiveType() || isRecursive || elementType.IsNullableType();

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
            if (!ExcludeNulls)
            {
                WriteString("null");
            }
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

                WriteKeyValueIfNotNull(onTheStack, keyType, elementType, isFirst);   // --empty--

                Emit.Branch(loop);                      // --empty--

                Emit.MarkLabel(done);
                disposeEnumerator(e);
            }

            DecreaseIndent();

            WriteString("}");

            Emit.MarkLabel(end);
        }

        void WriteDictionaryWithNulls(Type dictType, Sigil.Local inLocal)
        {
            var dictI = dictType.GetDictionaryInterface();

            var keyType = dictI.GetGenericArguments()[0];
            var elementType = dictI.GetGenericArguments()[1];

            var keysAreStrings = keyType == typeof(string);
            var keysAreEnums = keyType.IsEnum;
            var keysAreIntegers = keyType.IsIntegerNumberType();

            if (!(keysAreStrings || keysAreEnums || keysAreIntegers))
            {
                throw new InvalidOperationException("JSON dictionaries must have strings, enums, or integers as keys, found: " + keyType);
            }

            var kvType = typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);

            MethodInfo getEnumerator, enumeratorMoveNext;
            PropertyInfo enumeratorCurrent;
            Action<Sigil.Local> loadEnumerator;
            Action<Sigil.Local> disposeEnumerator;

            if (AllocationlessDictionaries && dictType.IsExactlyDictionaryType())
            {
                var enumerator = dictType.GetNestedType("Enumerator").MakeGenericType(keyType, elementType);
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
            var preloadTextWriter = elementType.IsPrimitiveType() || isRecursive || elementType.IsNullableType();

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
            if (!ExcludeNulls)
            {
                WriteString("null");
            }
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

                    WriteKeyValue(keyType, elementType);   // --empty--
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

                WriteKeyValue(keyType, elementType);   // --empty--

                Emit.Branch(loop);                          // --empty--

                Emit.MarkLabel(done);
                disposeEnumerator(e);
            }

            DecreaseIndent();

            WriteString("}");

            Emit.MarkLabel(end);
        }

        void WriteKeyValueIfNotNull(int ontheStack, Type keyType, Type elementType, Sigil.Local isFirst)
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

            var canBeNull = elementType.IsNullableType() || !elementType.IsValueType;

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

                    WriteEnum(keyType, popTextWriter: false);

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
                WriteNullable(elementType, quotesNeedHandling: true);

                Emit.MarkLabel(done);

                return;
            }

            if (elementType.IsEnum)
            {
                WriteEnum(elementType, popTextWriter: false);
                return;
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            if (isRecursive)
            {
                // Stack is:
                //  - serializingType
                //  - TextWriter
                //  - Action<TextWriter, serializingType>

                var recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), elementType, typeof(int));
                var invoke = recursiveAct.GetMethod("Invoke");

                Emit.LoadArgument(2);

                Emit.Call(invoke);

                Emit.MarkLabel(done);

                return;
            }

            using (var loc = Emit.DeclareLocal(elementType))
            {
                Emit.StoreLocal(loc);

                if (elementType.IsListType())
                {
                    WriteList(elementType, loc);

                    Emit.MarkLabel(done);

                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteList(elementType, loc);

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
                ExcludeNulls ?
                    JSONP ? Methods.WriteEncodedStringWithQuotesWithoutNullsInlineJSONP : Methods.WriteEncodedStringWithQuotesWithoutNullsInline :
                    JSONP ? Methods.WriteEncodedStringWithQuotesWithNullsInlineJSONP : Methods.WriteEncodedStringWithQuotesWithNullsInline;
        }

        

        MethodInfo GetWriteEncodedStringMethod()
        {
            return
                ExcludeNulls ?
                    JSONP ? Methods.WriteEncodedStringWithoutNullsInlineJSONP : Methods.WriteEncodedStringWithoutNullsInline :
                    JSONP ? Methods.WriteEncodedStringWithNullsInlineJSONP : Methods.WriteEncodedStringWithNullsInline;
        }

        void WriteKeyValue(Type keyType, Type elementType)
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

                    WriteEnum(keyType, popTextWriter: false);   // kvp

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
                WriteNullable(elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType.IsEnum)
            {
                WriteEnum(elementType, popTextWriter: false);
                return;
            }

            var isRecursive = RecursiveTypes.ContainsKey(elementType);
            if (isRecursive)
            {
                // Stack is:
                //  - serializingType
                //  - TextWriter
                //  - Action<TextWriter, serializingType>

                var recursiveAct = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), elementType, typeof(int));
                var invoke = recursiveAct.GetMethod("Invoke");

                Emit.LoadArgument(2);

                Emit.Call(invoke);

                return;
            }

            using (var loc = Emit.DeclareLocal(elementType))
            {
                Emit.StoreLocal(loc);

                if (elementType.IsListType())
                {
                    WriteList(elementType, loc);
                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteList(elementType, loc);
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
                var asStr = Enum.GetName(enumType, val);
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
            if (type == typeof(byte))
            {
                Emit.LoadConstant((byte)val);
                return;
            }
            if (type == typeof(sbyte))
            {
                Emit.LoadConstant((sbyte)val);
                return;
            }
            if (type == typeof(short))
            {
                Emit.LoadConstant((short)val);
                return;
            }
            if (type == typeof(ushort))
            {
                Emit.LoadConstant((ushort)val);
                return;
            }
            if (type == typeof(int))
            {
                Emit.LoadConstant((int)val);
                return;
            }
            if (type == typeof(uint))
            {
                Emit.LoadConstant((uint)val);
                return;
            }
            if (type == typeof(long))
            {
                Emit.LoadConstant((long)val);
                return;
            }
            if (type == typeof(ulong))
            {
                Emit.LoadConstant((ulong)val);
                return;
            }

            throw new Exception("Unexpected type: " + type);
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
                var name = Enum.GetName(enumType, val);

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
                var cacheType = RecusionLookupType.MakeGenericType(type);
                var thunk = cacheType.GetField("Thunk", BindingFlags.Public | BindingFlags.Static);

                var loc = Emit.DeclareLocal(thunk.FieldType);

                Emit.LoadField(thunk);  // Action<TextWriter, type>
                Emit.StoreLocal(loc);   // --empty--

                ret[type] = loc;
            }

            return ret;
        }

        void AddCharBuffer(Type serializingType)
        {
            // Don't tax the naive implementations by allocating a buffer they don't use
            if (!(UseCustomIntegerToString || UseFastGuids || UseCustomISODateFormatting)) return;

            var allTypes = serializingType.InvolvedTypes();

            var hasGuids = allTypes.Any(t => t == typeof(Guid));
            var hasDateTime = allTypes.Any(t => t == typeof(DateTime));
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

        Action<TextWriter, ForType, int> BuildObjectWithNewDelegate(bool doVerify)
        {
            var recursiveTypes = typeof(ForType).FindRecursiveTypes();

             Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) }, doVerify: doVerify);

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

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildListWithNewDelegate(bool doVerify)
        {
            var recursiveTypes = typeof(ForType).FindRecursiveTypes();

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) }, doVerify: doVerify);

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            WriteList(typeof(ForType));
            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildDictionaryWithNewDelegate(bool doVerify)
        {
            var recursiveTypes = typeof(ForType).FindRecursiveTypes();

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) }, doVerify: doVerify);

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            WriteDictionary(typeof(ForType));
            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildPrimitiveWithNewDelegate(bool doVerify)
        {
            var primitiveType = typeof(ForType);

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) }, doVerify: doVerify);

            AddCharBuffer(typeof(ForType));

            Emit.LoadArgument(0);
            Emit.LoadArgument(1);

            WritePrimitive(typeof(ForType), quotesNeedHandling: true);

            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildNullableWithNewDelegate(bool doVerify)
        {
            var recursiveTypes = typeof(ForType).FindRecursiveTypes();

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) }, doVerify: doVerify);

            AddCharBuffer(typeof(ForType));

            RecursiveTypes = PreloadRecursiveTypes(recursiveTypes);

            Emit.LoadArgument(0);
            Emit.LoadArgument(1);

            WriteNullable(typeof(ForType), quotesNeedHandling: true);
            
            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildEnumWithNewDelegate(bool doVerify)
        {
            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) }, doVerify: doVerify);

            Emit.LoadArgument(1);

            WriteEnum(typeof(ForType), popTextWriter: false);

            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        internal Action<TextWriter, ForType, int> Build()
        {
            var forType = typeof(ForType);

            bool doVerify;
#if DEBUG
            doVerify = true;
#else
            doVerify = false;
#endif

            if (forType.IsNullableType())
            {
                return BuildNullableWithNewDelegate(doVerify);
            }

            if (forType.IsPrimitiveType())
            {
                return BuildPrimitiveWithNewDelegate(doVerify);
            }

            if (forType.IsDictionaryType())
            {
                return BuildDictionaryWithNewDelegate(doVerify);
            }

            if (forType.IsListType())
            {
                return BuildListWithNewDelegate(doVerify);
            }

            if (forType.IsEnum)
            {
                return BuildEnumWithNewDelegate(doVerify);
            }

            return BuildObjectWithNewDelegate(doVerify);
        }
    }

    static class InlineSerializerHelper
    {
        public static Action<TextWriter, BuildForType, int> Build<BuildForType>(Type typeCacheType = null, bool pretty = false, bool excludeNulls = false, bool jsonp = false, DateTimeFormat dateFormat = DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, bool includeInherited = false)
        {
            typeCacheType = typeCacheType ?? typeof(NewtonsoftStyleTypeCache<>);

            var obj = new InlineSerializer<BuildForType>(typeCacheType, pretty, excludeNulls, jsonp, dateFormat, includeInherited);

            var ret = obj.Build();

            var opts = new Options(pretty, excludeNulls, jsonp, dateFormat, includeInherited);

            var estimatedInitialCapacity = CapacityEstimator.For(typeof(BuildForType), opts, 0);

            CapacityCache.Set<BuildForType>(opts, estimatedInitialCapacity);

            return ret;
        }
    }
}
