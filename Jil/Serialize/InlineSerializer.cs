using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    class InlineSerializer<ForType>
    {
        public static bool ReorderMembers = true;
        public static bool SkipNumberFormatting = true;
        public static bool UseCustomIntegerToString = true;
        public static bool SkipDateTimeMathMethods = true;

        static string CharBuffer = "char_buffer";
        const int CharBufferSize = 20;

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

        private Emit Emit;

        internal InlineSerializer(Type recusionLookupType, bool pretty, bool excludeNulls)
        {
            RecusionLookupType = recusionLookupType;
            PrettyPrint = pretty;
            ExcludeNulls = excludeNulls;
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
                var breakAndIndent = "\n" + string.Concat(Enumerable.Range(0, i).Select(_ => " "));

                Emit.MarkLabel(labels[i]);      // --empty--
                WriteString(breakAndIndent);    // --empty--
                Emit.Branch(done);              // --empty--
            }

            Emit.MarkLabel(done);               // --empty--
        }

        void IncreaseIndent()
        {
            Emit.LoadArgument(2);   // indent
            Emit.LoadConstant(1);   // indent 1
            Emit.Add();             // (indent+1)
            Emit.StoreArgument(2);  // --empty--
        }

        void DecreaseIndent()
        {
            Emit.LoadArgument(2);   // indent
            Emit.LoadConstant(-1);   // indent -1
            Emit.Add();             // (indent-1)
            Emit.StoreArgument(2);  // --empty--
        }

        static List<MemberInfo> OrderMembersForAccess(Type forType)
        {
            var members = forType.GetProperties().Where(p => p.GetMethod != null).Cast<MemberInfo>().Concat(forType.GetFields());

            if (forType.IsValueType)
            {
                return members.ToList();
            }

            // This order appears to be the "best" for access speed purposes
            var ret =
                !ReorderMembers ?
                    members :
                    Utils.IdealMemberOrderForWriting(forType, members);

            return ret.ToList();
        }

        static HashSet<Type> FindRecursiveTypes(Type forType)
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

        void WriteMember(MemberInfo member, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal = null)
        {
            // Stack is empty

            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new Exception("Wha?");

            var serializingType = asField != null ? asField.FieldType : asProp.PropertyType;

            // It's a list, go and build that code
            if (serializingType.IsListType())
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
                        WriteList(serializingType, recursiveTypes, loc);
                        return;
                    }

                    if (serializingType.IsDictionaryType())
                    {
                        WriteDictionary(serializingType, recursiveTypes, loc);
                        return;
                    }

                    WriteList(serializingType, recursiveTypes, loc);
                }

                return;
            }

            var isRecursive = recursiveTypes.ContainsKey(serializingType);

            if (isRecursive)
            {
                Emit.LoadLocal(recursiveTypes[serializingType]);    // Action<TextWriter, serializingType>
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
                WriteNullable(serializingType, recursiveTypes, quotesNeedHandling: true);
                return;
            }

            using (var loc = Emit.DeclareLocal(serializingType))
            {
                Emit.StoreLocal(loc);   // TextWriter;

                WriteObject(serializingType, recursiveTypes, loc);
            }
        }

        void WriteNullable(Type nullableType, Dictionary<Type, Sigil.Local> recursiveTypes, bool quotesNeedHandling)
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
                using (var loc = Emit.DeclareLocal(underlyingType))
                {
                    Emit.StoreLocal(loc);   // TextWriter

                    if (recursiveTypes.ContainsKey(underlyingType))
                    {
                        var act = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), underlyingType, typeof(int));
                        var invoke = act.GetMethod("Invoke");

                        Emit.Pop();                                     // --empty--
                        Emit.LoadLocal(recursiveTypes[underlyingType]); // Action<TextWriter, underlyingType>
                        Emit.LoadArgument(0);                           // Action<,> TextWriter
                        Emit.LoadLocal(loc);                            // Action<,> TextWriter value
                        Emit.LoadArgument(2);                           // Action<,> TextWriter value int
                        Emit.Call(invoke);                              // --empty--
                    }
                    else
                    {
                        if (underlyingType.IsListType())
                        {
                            WriteList(underlyingType, recursiveTypes, loc);
                        }
                        else
                        {
                            if (underlyingType.IsDictionaryType())
                            {
                                WriteList(underlyingType, recursiveTypes, loc);
                            }
                            else
                            {
                                Emit.Pop();

                                WriteObject(underlyingType, recursiveTypes, loc);
                            }
                        }
                    }
                }
            }

            Emit.MarkLabel(done);
        }

        void WriteDateTime()
        {
            // top of stack:
            //   - DateTime
            //   - TextWriter

            using (var loc = Emit.DeclareLocal<DateTime>())
            {
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

                WriteString("\"\\/Date(");                  // TextWriter int
                WritePrimitive(typeof(long), quotesNeedHandling: false);               // --empty--
                WriteString(")\\/\"");                      // --empty--

                return;
            }

            var getTicks = typeof(DateTime).GetProperty("Ticks");

            LoadProperty(getTicks);                         // TextWriter long
            Emit.LoadConstant(621355968000000000L);         // TextWriter long (Unix Epoch Ticks long)
            Emit.Subtract();                                // TextWriter long
            Emit.LoadConstant(10000L);                      // TextWriter long 10000
            Emit.Divide();                                  // TextWriter long

            WriteString("\"\\/Date(");                  // TextWriter int
            WritePrimitive(typeof(long), quotesNeedHandling: true);               // --empty--
            WriteString(")\\/\"");                      // --empty--
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
                WriteEncodedString(quotesNeedHandling);
                return;
            }

            if (primitiveType == typeof(DateTime))
            {
                WriteDateTime();
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
                var writeInt = typeof(TextWriter).GetMethod("Write", new[] { typeof(int) });
                var done = Emit.DefineLabel();

                Emit.Duplicate();               // TextWriter int int

                var labels = Enumerable.Range(0, 100).Select(l => Emit.DefineLabel()).ToArray();

                Emit.Switch(labels);            // TextWriter int

                // default case

                if (UseCustomIntegerToString)
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int (ref char[])
                    Emit.Call(InlineSerializer_CustomWriteInt); // --empty--
                }
                else
                {
                    Emit.CallVirtual(writeInt);     // --empty--
                }

                Emit.Branch(done);              // --empty--

                for (var i = 0; i < labels.Length; i++)
                {
                    var label = labels[i];

                    Emit.MarkLabel(label);      // TextWriter int
                    Emit.Pop();                 // TextWriter
                    Emit.Pop();                 // --empty--
                    WriteString("" + i);        // --empty--
                    Emit.Branch(done);          // --empty--
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
                    Emit.Call(InlineSerializer_CustomWriteInt); // --empty--

                    return;
                }

                if (primitiveType == typeof(uint))
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int char[]
                    Emit.Call(InlineSerializer_CustomWriteUInt); // --empty--

                    return;
                }

                if (primitiveType == typeof(long))
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int char[]
                    Emit.Call(InlineSerializer_CustomWriteLong); // --empty--

                    return;
                }

                if (primitiveType == typeof(ulong))
                {
                    Emit.LoadLocal(CharBuffer);          // TextWriter int char[]
                    Emit.Call(InlineSerializer_CustomWriteULong); // --empty--

                    return;
                }
            }

            Emit.CallVirtual(builtInMtd);       // --empty--
        }

        static MethodInfo InlineSerializer_CustomWriteInt = typeof(InlineSerializer<ForType>).GetMethod("CustomWriteInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteInt(TextWriter writer, int number, char[] buffer)
        {
            // Gotta special case this, we can't negate it
            if (number == int.MinValue)
            {
                writer.Write("-2147483648");
                return;
            }

            var ptr = CharBufferSize - 1;

            var copy = number;
            if (copy < 0)
            {
                copy = -copy;
            }

            do
            {
                var ix = copy % 10;
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            if (number < 0)
            {
                buffer[ptr] = '-';
                ptr--;
            }

            writer.Write(buffer, ptr + 1, CharBufferSize - 1 - ptr);
        }

        static MethodInfo InlineSerializer_CustomWriteUInt = typeof(InlineSerializer<ForType>).GetMethod("CustomWriteUInt", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteUInt(TextWriter writer, uint number, char[] buffer)
        {
            var ptr = CharBufferSize - 1;

            var copy = number;
            
            do
            {
                var ix = copy % 10;
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            writer.Write(buffer, ptr + 1, CharBufferSize - 1 - ptr);
        }

        static MethodInfo InlineSerializer_CustomWriteLong = typeof(InlineSerializer<ForType>).GetMethod("CustomWriteLong", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteLong(TextWriter writer, long number, char[] buffer)
        {
            // Gotta special case this, we can't negate it
            if (number == long.MinValue)
            {
                writer.Write("-9223372036854775808");
                return;
            }

            var ptr = CharBufferSize - 1;

            var copy = number;
            if (copy < 0)
            {
                copy = -copy;
            }

            do
            {
                var ix = (int)(copy % 10);
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            if (number < 0)
            {
                buffer[ptr] = '-';
                ptr--;
            }

            writer.Write(buffer, ptr + 1, CharBufferSize - 1 - ptr);
        }

        static MethodInfo InlineSerializer_CustomWriteULong = typeof(InlineSerializer<ForType>).GetMethod("CustomWriteULong", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CustomWriteULong(TextWriter writer, ulong number, char[] buffer)
        {
            var ptr = CharBufferSize - 1;

            var copy = number;

            do
            {
                var ix = (int)(copy % 10);
                copy /= 10;

                buffer[ptr] = (char)('0' + ix);
                ptr--;
            } while (copy != 0);

            writer.Write(buffer, ptr + 1, CharBufferSize - 1 - ptr);
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
            Emit.BranchIfEqual(quote);      // TextWriter clear

            Emit.CallVirtual(writeChar);    // --empty--
            Emit.Branch(done);              // --empty--

            Emit.MarkLabel(slash);          // TextWriter char

            Emit.Pop();                     // TextWriter
            Emit.Pop();                     // --empty--
            WriteString(@"\\");             // --empty--
            Emit.Branch(done);              // --empty--

            Emit.MarkLabel(quote);
            Emit.Pop();                     // TextWriter
            Emit.Pop();                     // --empty--
            WriteString(@"\""");            // --empty--
            Emit.Branch(done);              // --empty--

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

        void WriteEncodedString(bool quotesNeedHandling)
        {
            // top of stack is:
            //  - string
            //  - TextWriter

            var writeChar = typeof(TextWriter).GetMethod("Write", new[] { typeof(char) });
            var strLength = typeof(string).GetProperty("Length");
            var strCharsIx = typeof(string).GetProperty("Chars");

            var notNull = Emit.DefineLabel();
            var done = Emit.DefineLabel();
            var doneNull = Emit.DefineLabel();

            Emit.Duplicate();           // TextWriter string string
            Emit.BranchIfTrue(notNull); // TextWriter string

            if (!ExcludeNulls)
            {
                WriteString("null");        // TextWriter string
            }

            Emit.Pop();                 // TextWriter
            Emit.Pop();                 // --empty--
            Emit.Branch(doneNull);      // --empty--

            Emit.MarkLabel(notNull);

            if (quotesNeedHandling)
            {
                WriteString("\"");
            }

            using (var str = Emit.DeclareLocal<string>())
            using (var i = Emit.DeclareLocal<int>())
            {
                var loop = Emit.DefineLabel();

                Emit.LoadConstant(0);   // TextWriter string 0
                Emit.StoreLocal(i);     // TextWriter string
                Emit.StoreLocal(str);   // TextWriter
                Emit.Duplicate();       // TextWriter TextWriter

                Emit.MarkLabel(loop);               // TextWriter TextWriter
                Emit.LoadLocal(i);                  // TextWriter TextWriter i
                Emit.LoadLocal(str);                // TextWriter TextWriter i string
                LoadProperty(strLength);            // TextWriter TextWriter i str.Length
                Emit.BranchIfGreaterOrEqual(done);  // TextWriter TextWriter

                Emit.LoadLocal(str);            // TextWriter TextWriter string
                Emit.LoadLocal(i);              // TextWriter TextWriter string i
                LoadProperty(strCharsIx);       // TextWriter TextWriter char

                WriteEncodedChar(false);        // TextWriter

                Emit.LoadLocal(i);      // TextWriter i
                Emit.LoadConstant(1);   // TextWriter i 1
                Emit.Add();             // TextWriter (i+1)
                Emit.StoreLocal(i);     // TextWriter
                Emit.Duplicate();       // TextWriter TextWriter
                Emit.Branch(loop);      // TextWriter TextWriter
            }

            Emit.MarkLabel(done);       // TextWriter TextWriter
            Emit.Pop();                 // TextWriter
            Emit.Pop();                 // --empty--

            if (quotesNeedHandling)
            {
                WriteString("\"");
            }

            Emit.MarkLabel(doneNull);   // --empty--
        }

        void WriteObjectWithNulls(Type forType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal = null)
        {
            var writeOrder = OrderMembersForAccess(forType);

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

            Emit.MarkLabel(notNull);                // --empty--
            WriteString("{");                       // --empty--

            if (PrettyPrint)
            {
                IncreaseIndent();
            }

            var firstPass = true;
            foreach (var member in writeOrder)
            {
                if (!PrettyPrint)
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

                    WriteString(keyString);                         // --empty--
                    WriteMember(member, recursiveTypes, inLocal);   // --empty--
                }
                else
                {
                    if (!firstPass)
                    {
                        WriteString(",");
                    }

                    LineBreakAndIndent();

                    firstPass = false;

                    WriteString("\"" + member.Name.JsonEscape() + "\": ");

                    WriteMember(member, recursiveTypes, inLocal);
                }
            }

            if (PrettyPrint)
            {
                DecreaseIndent();
                LineBreakAndIndent();
            }

            WriteString("}");                               // --empty--

            Emit.MarkLabel(end);
        }

        void WriteObject(Type forType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal = null)
        {
            if (!ExcludeNulls)
            {
                WriteObjectWithNulls(forType, recursiveTypes, inLocal);
            }
            else
            {
                WriteObjectWithoutNulls(forType, recursiveTypes, inLocal);
            }
        }

        void WriteObjectWithoutNulls(Type forType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal = null)
        {
            var writeOrder = OrderMembersForAccess(forType);

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

            Emit.MarkLabel(notNull);        // obj(*?)
            WriteString("{");               // obj(*?)

            if (PrettyPrint)
            {
                IncreaseIndent();
            }

            using (var isFirst = Emit.DeclareLocal<bool>())
            {
                Emit.LoadConstant(true);        // obj(*?) true
                Emit.StoreLocal(isFirst);       // obj(*?) true

                foreach (var member in writeOrder)
                {
                    Emit.Duplicate();                                                   // obj(*?) obj(*?)
                    WriteMemberIfNonNull(member, recursiveTypes, inLocal, isFirst);  // obj(*?)
                }
            }

            if (PrettyPrint)
            {
                DecreaseIndent();
                LineBreakAndIndent();
            }

            WriteString("}");       // obj(*?)

            Emit.MarkLabel(end);    // obj(*?)
            Emit.Pop();             // --empty--
        }

        void WriteMemberIfNonNull(MemberInfo member, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal, Sigil.Local isFirst)
        {
            // Top of stack:
            //  - obj(*?)

            var asField = member as FieldInfo;
            var asProp = member as PropertyInfo;

            if (asField == null && asProp == null) throw new Exception("Wha?");

            var serializingType = asField != null ? asField.FieldType : asProp.PropertyType;

            var end = Emit.DefineLabel();
            var writeValue = Emit.DefineLabel();

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

            WriteString("\"" + member.Name.JsonEscape() + "\":");   // --empty--

            WriteMember(member, recursiveTypes, inLocal);           // --empty--

            Emit.MarkLabel(end);
        }

        void WriteList(Type listType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal = null)
        {
            var elementType = listType.GetListInterface().GetGenericArguments()[0];

            var iEnumerable = typeof(IEnumerable<>).MakeGenericType(elementType);
            var iEnumerableGetEnumerator = iEnumerable.GetMethod("GetEnumerator");
            var enumeratorMoveNext = typeof(System.Collections.IEnumerator).GetMethod("MoveNext");
            var enumeratorCurrent = iEnumerableGetEnumerator.ReturnType.GetProperty("Current");

            var iList = typeof(IList<>).MakeGenericType(elementType);

            var isRecursive = recursiveTypes.ContainsKey(elementType);
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
                        var loc = recursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    Emit.LoadLocal(e);                      // Action<>? TextWriter? IEnumerator<>
                    LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? type

                    WriteElement(elementType, recursiveTypes);   // --empty--
                }

                var loop = Emit.DefineLabel();

                Emit.MarkLabel(loop);

                Emit.LoadLocal(e);                      // IEnumerator<>
                Emit.CallVirtual(enumeratorMoveNext);   // bool
                Emit.BranchIfFalse(done);               // --empty--

                if (isRecursive)
                {
                    var loc = recursiveTypes[elementType];

                    Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                }

                if (preloadTextWriter)
                {
                    Emit.LoadArgument(0);               // Action<>? TextWriter
                }

                Emit.LoadLocal(e);                      // Action<>? TextWriter? IEnumerator<>
                LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? type

                WriteString(",");

                WriteElement(elementType, recursiveTypes);   // --empty--

                Emit.Branch(loop);
            }

            Emit.MarkLabel(done);

            WriteString("]");

            Emit.MarkLabel(end);
        }

        void WriteElement(Type elementType, Dictionary<Type, Sigil.Local> recursiveTypes)
        {
            if (elementType.IsPrimitiveType())
            {
                WritePrimitive(elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType.IsNullableType())
            {
                WriteNullable(elementType, recursiveTypes, quotesNeedHandling: true);
                return;
            }

            var isRecursive = recursiveTypes.ContainsKey(elementType);
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
                    WriteList(elementType, recursiveTypes, loc);
                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteDictionary(elementType, recursiveTypes, loc);
                    return;
                }

                WriteObject(elementType, recursiveTypes, loc);
            }
        }

        void WriteDictionary(Type dictType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal = null)
        {
            if (!ExcludeNulls)
            {
                WriteDictionaryWithNulls(dictType, recursiveTypes, inLocal);
            }
            else
            {
                WriteDictionaryWithoutNulls(dictType, recursiveTypes, inLocal);
            }
        }

        void WriteDictionaryWithoutNulls(Type dictType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal)
        {
            var dictI = dictType.GetDictionaryInterface();

            var keyType = dictI.GetGenericArguments()[0];
            var elementType = dictI.GetGenericArguments()[1];

            if (keyType != typeof(string))
            {
                throw new InvalidOperationException("JSON dictionaries must have strings as keys, found: " + keyType);
            }

            var kvType = typeof(KeyValuePair<,>).MakeGenericType(typeof(string), elementType);

            var iEnumerable = typeof(IEnumerable<>).MakeGenericType(kvType);
            var iEnumerableGetEnumerator = iEnumerable.GetMethod("GetEnumerator");
            var enumeratorMoveNext = typeof(System.Collections.IEnumerator).GetMethod("MoveNext");
            var enumeratorCurrent = iEnumerableGetEnumerator.ReturnType.GetProperty("Current");

            var iDictionary = typeof(IDictionary<,>).MakeGenericType(typeof(string), elementType);

            var isRecursive = recursiveTypes.ContainsKey(elementType);
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

            if (PrettyPrint)
            {
                IncreaseIndent();
            }

            var done = Emit.DefineLabel();

            int onTheStack = 0;

            using (var e = Emit.DeclareLocal(iEnumerableGetEnumerator.ReturnType))
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

                Emit.CastClass(iDictionary);                  // IDictionary<,>
                Emit.CallVirtual(iEnumerableGetEnumerator);   // IEnumerator<KeyValuePair<,>>
                Emit.StoreLocal(e);                           // --empty--

                var loop = Emit.DefineLabel();

                Emit.MarkLabel(loop);                   // --empty--

                Emit.LoadLocal(e);                      // IEnumerator<KeyValuePair<,>>
                Emit.CallVirtual(enumeratorMoveNext);   // bool
                Emit.BranchIfFalse(done);               // --empty--

                if (isRecursive)
                {
                    onTheStack++;

                    var loc = recursiveTypes[elementType];

                    Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                }

                if (preloadTextWriter)
                {
                    onTheStack++;

                    Emit.LoadArgument(0);               // Action<>? TextWriter
                }

                Emit.LoadLocal(e);                      // Action<>? TextWriter? IEnumerator<>
                LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? KeyValuePair<,>

                Emit.StoreLocal(kvpLoc);                // Action<>? TextWriter?
                Emit.LoadLocalAddress(kvpLoc);          // Action<>? TextWriter? KeyValuePair<,>*

                onTheStack++;

                WriteKeyValueIfNotNull(onTheStack, elementType, recursiveTypes, isFirst);   // --empty--

                Emit.Branch(loop);                      // --empty--
            }

            Emit.MarkLabel(done);

            if (PrettyPrint)
            {
                DecreaseIndent();
                LineBreakAndIndent();
            }

            WriteString("}");

            Emit.MarkLabel(end);
        }

        void WriteDictionaryWithNulls(Type dictType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local inLocal)
        {
            var dictI = dictType.GetDictionaryInterface();

            var keyType = dictI.GetGenericArguments()[0];
            var elementType = dictI.GetGenericArguments()[1];

            if (keyType != typeof(string))
            {
                throw new InvalidOperationException("JSON dictionaries must have strings as keys, found: " + keyType);
            }

            var kvType = typeof(KeyValuePair<,>).MakeGenericType(typeof(string), elementType);

            var iEnumerable = typeof(IEnumerable<>).MakeGenericType(kvType);
            var iEnumerableGetEnumerator = iEnumerable.GetMethod("GetEnumerator");
            var enumeratorMoveNext = typeof(System.Collections.IEnumerator).GetMethod("MoveNext");
            var enumeratorCurrent = iEnumerableGetEnumerator.ReturnType.GetProperty("Current");

            var iDictionary = typeof(IDictionary<,>).MakeGenericType(typeof(string), elementType);

            var isRecursive = recursiveTypes.ContainsKey(elementType);
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

            if (PrettyPrint)
            {
                IncreaseIndent();
            }

            var done = Emit.DefineLabel();

            using (var e = Emit.DeclareLocal(iEnumerableGetEnumerator.ReturnType))
            using(var kvpLoc = Emit.DeclareLocal(kvType))
            {
                if (inLocal != null)
                {
                    Emit.LoadLocal(inLocal);
                }
                else
                {
                    Emit.LoadArgument(1);
                }

                Emit.CastClass(iDictionary);                  // IDictionary<,>
                Emit.CallVirtual(iEnumerableGetEnumerator);   // IEnumerator<KeyValuePair<,>>
                Emit.StoreLocal(e);                           // --empty--

                // Do the whole first element before the loop starts, so we don't need a branch to emit a ','
                {
                    Emit.LoadLocal(e);                      // IEnumerator<KeyValuePair<,>>
                    Emit.CallVirtual(enumeratorMoveNext);   // bool
                    Emit.BranchIfFalse(done);               // --empty--

                    if (isRecursive)
                    {
                        var loc = recursiveTypes[elementType];

                        Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                    }

                    if (preloadTextWriter)
                    {
                        Emit.LoadArgument(0);               // Action<>? TextWriter
                    }

                    Emit.LoadLocal(e);                      // Action<>? TextWriter? IEnumerator<>
                    LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? KeyValuePair<,>
                    
                    Emit.StoreLocal(kvpLoc);                // Action<>? TextWriter?
                    Emit.LoadLocalAddress(kvpLoc);          // Action<>? TextWriter? KeyValuePair<,>*

                    WriteKeyValue(elementType, recursiveTypes);   // --empty--
                }

                var loop = Emit.DefineLabel();

                Emit.MarkLabel(loop);                   // --empty--

                Emit.LoadLocal(e);                      // IEnumerator<KeyValuePair<,>>
                Emit.CallVirtual(enumeratorMoveNext);   // bool
                Emit.BranchIfFalse(done);               // --empty--

                if (isRecursive)
                {
                    var loc = recursiveTypes[elementType];

                    Emit.LoadLocal(loc);                // Action<TextWriter, elementType>
                }

                if (preloadTextWriter)
                {
                    Emit.LoadArgument(0);               // Action<>? TextWriter
                }

                Emit.LoadLocal(e);                      // Action<>? TextWriter? IEnumerator<>
                LoadProperty(enumeratorCurrent);        // Action<>? TextWriter? KeyValuePair<,>

                Emit.StoreLocal(kvpLoc);                // Action<>? TextWriter?
                Emit.LoadLocalAddress(kvpLoc);          // Action<>? TextWriter? KeyValuePair<,>*

                WriteString(",");

                WriteKeyValue(elementType, recursiveTypes);   // --empty--

                Emit.Branch(loop);                          // --empty--
            }

            Emit.MarkLabel(done);

            if (PrettyPrint)
            {
                DecreaseIndent();
                LineBreakAndIndent();
            }

            WriteString("}");

            Emit.MarkLabel(end);
        }

        void WriteKeyValueIfNotNull(int ontheStack, Type elementType, Dictionary<Type, Sigil.Local> recursiveTypes, Sigil.Local isFirst)
        {
            // top of the stack is a 
            //   - KeyValue<string, elementType>
            //   - TextWriter?
            //   - Action<,>?

            var keyValuePair = typeof(KeyValuePair<,>).MakeGenericType(typeof(string), elementType);
            var key = keyValuePair.GetProperty("Key");
            var value = keyValuePair.GetProperty("Value");

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

            WriteString(",");                       // kvp

            Emit.MarkLabel(skipComma);              // kvp

            Emit.LoadConstant(false);               // kvp false
            Emit.StoreLocal(isFirst);               // kvp

            if (PrettyPrint)
            {
                LineBreakAndIndent();
            }

            WriteString("\"");                      // kvp

            Emit.Duplicate();       // kvp kvp
            LoadProperty(key);      // kvp string

            using (var str = Emit.DeclareLocal<string>())
            {
                Emit.StoreLocal(str);   // kvp
                Emit.LoadArgument(0);   // kvp TextWriter
                Emit.LoadLocal(str);    // kvp TextWriter string

                WriteEncodedString(quotesNeedHandling: false);   // kvp
            }

            if (PrettyPrint)
            {
                WriteString("\": ");
            }
            else
            {
                WriteString("\":");         // kvp
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
                WriteNullable(elementType, recursiveTypes, quotesNeedHandling: true);

                Emit.MarkLabel(done);

                return;
            }

            var isRecursive = recursiveTypes.ContainsKey(elementType);
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
                    WriteList(elementType, recursiveTypes, loc);

                    Emit.MarkLabel(done);

                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteList(elementType, recursiveTypes, loc);

                    Emit.MarkLabel(done);

                    return;
                }

                WriteObject(elementType, recursiveTypes, loc);
            }

            Emit.MarkLabel(done);
        }

        void WriteKeyValue(Type elementType, Dictionary<Type, Sigil.Local> recursiveTypes)
        {
            // top of the stack is a KeyValue<string, elementType>

            var keyValuePair = typeof(KeyValuePair<,>).MakeGenericType(typeof(string), elementType);
            var key = keyValuePair.GetProperty("Key");
            var value = keyValuePair.GetProperty("Value");

            if (PrettyPrint)
            {
                LineBreakAndIndent();
            }

            WriteString("\"");

            Emit.Duplicate();           // kvp kvp
            LoadProperty(key);    // kvp string

            using (var str = Emit.DeclareLocal<string>())
            {
                Emit.StoreLocal(str);   // kvp
                Emit.LoadArgument(0);   // kvp TextWriter
                Emit.LoadLocal(str);    // kvp TextWriter string

                //WriteEncodedString(false); // kvp
                Emit.CallVirtual(TextWriter_WriteString);   // kvp
            }

            if (PrettyPrint)
            {
                WriteString("\": ");        // kvp
            }
            else
            {
                WriteString("\":");         // kvp
            }

            LoadProperty(value);        // elementType

            if (elementType.IsPrimitiveType())
            {
                WritePrimitive(elementType, quotesNeedHandling: true);
                return;
            }

            if (elementType.IsNullableType())
            {
                WriteNullable(elementType, recursiveTypes, quotesNeedHandling: true);
                return;
            }

            var isRecursive = recursiveTypes.ContainsKey(elementType);
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
                    WriteList(elementType, recursiveTypes, loc);
                    return;
                }

                if (elementType.IsDictionaryType())
                {
                    WriteList(elementType, recursiveTypes, loc);
                    return;
                }

                WriteObject(elementType, recursiveTypes, loc);
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

        void AddCharBuffer()
        {
            if (UseCustomIntegerToString)
            {
                Emit.DeclareLocal<char[]>(CharBuffer);
                Emit.LoadConstant(CharBufferSize);
                Emit.NewArray<char>();
                Emit.StoreLocal(CharBuffer);
            }
        }

        Action<TextWriter, ForType, int> BuildObjectWithNewDelegate()
        {
            var recursiveTypes = FindRecursiveTypes(typeof(ForType));

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) });

            AddCharBuffer();

            var preloaded = PreloadRecursiveTypes(recursiveTypes);

            WriteObject(typeof(ForType), preloaded);
            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildListWithNewDelegate()
        {
            var recursiveTypes = FindRecursiveTypes(typeof(ForType));

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) });

            AddCharBuffer();

            var preloaded = PreloadRecursiveTypes(recursiveTypes);

            WriteList(typeof(ForType), preloaded);
            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildDictionaryWithNewDelegate()
        {
            var recursiveTypes = FindRecursiveTypes(typeof(ForType));

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) });

            AddCharBuffer();

            var preloaded = PreloadRecursiveTypes(recursiveTypes);

            WriteDictionary(typeof(ForType), preloaded);
            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildPrimitiveWithNewDelegate()
        {
            var primitiveType = typeof(ForType);

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) });

            AddCharBuffer();

            Emit.LoadArgument(0);
            Emit.LoadArgument(1);

            WritePrimitive(typeof(ForType), quotesNeedHandling: true);

            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        Action<TextWriter, ForType, int> BuildNullableWithNewDelegate()
        {
            var recursiveTypes = FindRecursiveTypes(typeof(ForType));

            Emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(ForType), typeof(int) });

            AddCharBuffer();

            var preloaded = PreloadRecursiveTypes(recursiveTypes);

            Emit.LoadArgument(0);
            Emit.LoadArgument(1);

            WriteNullable(typeof(ForType), preloaded, quotesNeedHandling: true);
            
            Emit.Return();

            return Emit.CreateDelegate<Action<TextWriter, ForType, int>>();
        }

        internal Action<TextWriter, ForType, int> Build()
        {
            var forType = typeof(ForType);

            if (forType.IsNullableType())
            {
                return BuildNullableWithNewDelegate();
            }

            if (forType.IsPrimitiveType())
            {
                return BuildPrimitiveWithNewDelegate();
            }

            if (forType.IsDictionaryType())
            {
                return BuildDictionaryWithNewDelegate();
            }

            if (forType.IsListType())
            {
                return BuildListWithNewDelegate();
            }

            return BuildObjectWithNewDelegate();
        }
    }

    static class InlineSerializerHelper
    {
        public static Action<TextWriter, BuildForType, int> Build<BuildForType>(Type typeCacheType = null, bool pretty = false, bool excludeNulls = false)
        {
            typeCacheType = typeCacheType ?? typeof(NoneTypeCache<>);

            var obj = new InlineSerializer<BuildForType>(typeCacheType, pretty, excludeNulls);

            return obj.Build();
        }
    }
}
