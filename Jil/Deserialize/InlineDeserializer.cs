﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;
using Sigil.NonGeneric;
using System.Reflection;

namespace Jil.Deserialize
{
    class InlineDeserializer<ForType>
    {
        const string CharBufferName = "char_buffer";
        const string StringBuilderName = "string_builder";
        
        readonly Type RecursionLookupType;
        HashSet<Type> RecursiveTypes;

        Emit Emit;

        public InlineDeserializer(Type recursionLookupType)
        {
            RecursionLookupType = recursionLookupType;
        }

        void AddGlobalVariables()
        {
            Emit.DeclareLocal<char[]>(CharBufferName);
            Emit.LoadConstant(Methods.CharBufferSize);
            Emit.NewArray<char>();
            Emit.StoreLocal(CharBufferName);

            Emit.DeclareLocal<StringBuilder>(StringBuilderName);
            Emit.NewObject<StringBuilder>();
            Emit.StoreLocal(StringBuilderName);
        }

        void LoadCharBuffer()
        {
            Emit.LoadLocal(CharBufferName);
        }

        void LoadStringBuilder()
        {
            Emit.LoadLocal(StringBuilderName);
        }

        static MethodInfo TextReader_Read = typeof(TextReader).GetMethod("Read", Type.EmptyTypes);
        void RawReadChar(Action endOfStream)
        {
            var haveChar = Emit.DefineLabel();

            Emit.LoadArgument(0);                       // TextReader
            Emit.CallVirtual(TextReader_Read);          // int
            Emit.Duplicate();                           // int int
            Emit.LoadConstant(-1);                      // int int -1
            Emit.UnsignedBranchIfNotEqual(haveChar);    // int
            Emit.Pop();                                 // --empty--
            endOfStream();                              // --empty--

            Emit.MarkLabel(haveChar);                   // int
        }

        void ThrowExpected(char c)
        {
            Emit.LoadConstant("Expected character: '" + c + "'");   // string
            Emit.NewObject<DeserializationException, string>();     // DeserializationException
            Emit.Throw();
        }

        void ThrowExpected(params object[] ps)
        {
            Emit.LoadConstant("Expected: " + string.Join(", ", ps));    // string
            Emit.NewObject<DeserializationException, string>();         // DeserializationException
            Emit.Throw();
        }

        void ExpectChar(char c)
        {
            var gotChar = Emit.DefineLabel();

            RawReadChar(() => ThrowExpected(c));    // int
            Emit.LoadConstant((int)c);              // int int
            Emit.BranchIfEqual(gotChar);            // --empty--
            ThrowExpected(c);                       // --empty--

            Emit.MarkLabel(gotChar);                // --empty--
        }

        void ExpectQuote()
        {
            ExpectChar('"');
        }

        void ExpectRawCharOrNull(char c, Action ifChar, Action ifNull)
        {
            var gotChar = Emit.DefineLabel();
            var gotN = Emit.DefineLabel();
            var done = Emit.DefineLabel();

            RawReadChar(() => ThrowExpected("\"", "null")); // int
            Emit.Duplicate();                               // int int
            Emit.LoadConstant((int)c);                      // int int int
            Emit.BranchIfEqual(gotChar);                    // int 
            Emit.LoadConstant((int)'n');                    // int n
            Emit.BranchIfEqual(gotN);                       // --empty--
            ThrowExpected("\"", "null");                    // --empty--

            Emit.MarkLabel(gotChar);                        // int
            Emit.Pop();                                     // --empty--
            ifChar();                                       // ???
            Emit.Branch(done);                              // ???

            Emit.MarkLabel(gotN);                           // --empty--
            ExpectChar('u');                                // --empty--
            ExpectChar('l');                                // --empty--
            ExpectChar('l');                                // --empty--
            ifNull();                                       // ???

            Emit.MarkLabel(done);                           // --empty--
        }

        void ExpectQuoteOrNull(Action ifQuote, Action ifNull)
        {
            ExpectRawCharOrNull('"', ifQuote, ifNull);
        }

        void ReadChar()
        {
            ExpectQuote();          // --empty--
            ReadEncodedChar();      // char
            ExpectQuote();          // char
        }

        void ReadEncodedChar()
        {
            Emit.LoadArgument(0);               // TextReader
            LoadCharBuffer();
            Emit.Call(Methods.ReadEncodedChar); // char
        }

        void ReadString()
        {
            ExpectQuoteOrNull(
                delegate
                {
                    // --empty--
                    Emit.LoadArgument(0);                   // TextReader
                    LoadCharBuffer();                       // TextReader char[]
                    LoadStringBuilder();                    // TextReader char[] StringBuilder
                    Emit.Call(Methods.ReadEncodedString);   // string
                },
                delegate
                {
                    Emit.LoadNull();                        // null
                }
            );
        }

        void ReadNumber(Type numberType)
        {
            Emit.LoadArgument(0);               // TextReader

            if (numberType == typeof(byte))
            {
                Emit.Call(Methods.ReadUInt8);   // byte
                return;
            }

            if (numberType == typeof(sbyte))
            {
                Emit.Call(Methods.ReadInt8);    // sbyte
                return;
            }

            if (numberType == typeof(short))
            {
                Emit.Call(Methods.ReadInt16);   // short
                return;
            }

            if (numberType == typeof(ushort))
            {
                Emit.Call(Methods.ReadUInt16);  // ushort
                return;
            }

            if (numberType == typeof(int))
            {
                Emit.Call(Methods.ReadInt32);   // int
                return;
            }

            if (numberType == typeof(uint))
            {
                Emit.Call(Methods.ReadUInt32);  // uint
                return;
            }

            if (numberType == typeof(long))
            {
                Emit.Call(Methods.ReadInt64);   // long
                return;
            }

            if (numberType == typeof(ulong))
            {
                Emit.Call(Methods.ReadUInt64);  // ulong
                return;
            }

            LoadStringBuilder();                    // TextReader StringBuilder

            if (numberType == typeof(double))
            {
                Emit.Call(Methods.ReadDouble);   // double
                return;
            }

            if (numberType == typeof(float))
            {
                Emit.Call(Methods.ReadSingle);  // float
                return;
            }

            if (numberType == typeof(decimal))
            {
                Emit.Call(Methods.ReadDecimal); // decimal
                return;
            }

            throw new Exception("Unexpected number type: " + numberType);
        }

        void ReadBool()
        {
            var endOfStream = Emit.DefineLabel();
            var mayBeTrue = Emit.DefineLabel();
            var mayBeFalse = Emit.DefineLabel();
            var done = Emit.DefineLabel();

            RawReadChar(() => Emit.Branch(endOfStream));    // int
            Emit.Duplicate();                               // int int
            Emit.LoadConstant('t');                         // int int 't'
            Emit.BranchIfEqual(mayBeTrue);                  // int
            Emit.LoadConstant('f');                         // int 'f'
            Emit.BranchIfEqual(mayBeFalse);                 // --empty--

            // end of stream **AND** not true or false case
            Emit.MarkLabel(endOfStream);                    // --empty--
            ThrowExpected("true or false");                 // --empty--

            Emit.MarkLabel(mayBeTrue);      // int
            Emit.Pop();                     // --empty--
            ExpectChar('r');                // --empty--
            ExpectChar('u');                // --empty--
            ExpectChar('e');                // --empty--

            Emit.LoadConstant(true);        // bool
            Emit.Branch(done);              // bool

            Emit.MarkLabel(mayBeFalse);     // --empty--
            ExpectChar('a');                // --empty--
            ExpectChar('l');                // --empty--
            ExpectChar('s');                // --empty--
            ExpectChar('e');                // --empty--

            Emit.LoadConstant(false);       // bool

            Emit.MarkLabel(done);           // bool
        }

        void ReadPrimitive(Type primitiveType)
        {
            if (primitiveType == typeof(bool))
            {
                ReadBool();
                return;
            }

            if (primitiveType == typeof(char))
            {
                ReadChar();
                return;
            }

            if (primitiveType == typeof(string))
            {
                ReadString();
                return;
            }

            ReadNumber(primitiveType);
        }

        void ConsumeWhiteSpace()
        {
            Emit.LoadArgument(0);                   // TextReader
            Emit.Call(Methods.ConsumeWhiteSpace);   // --empty--
        }

        void ExpectEndOfStream()
        {
            var success = Emit.DefineLabel();

            Emit.LoadArgument(0);           // TextReader
            Emit.Call(TextReader_Read);     // int
            Emit.LoadConstant(-1);          // int -1
            Emit.BranchIfEqual(success);    // --empty--

            Emit.LoadConstant("Expected end of stream");        // string
            Emit.NewObject<DeserializationException, string>(); // DeserializationException
            Emit.Throw();                                       // --empty--

            Emit.MarkLabel(success);        // --empty--
        }

        static readonly MethodInfo TextReader_Peek = typeof(TextReader).GetMethod("Peek", BindingFlags.Public | BindingFlags.Instance);
        void RawPeekChar()
        {
            Emit.LoadArgument(0);                   // TextReader
            Emit.CallVirtual(TextReader_Peek);      // int
        }

        static readonly MethodInfo Type_GetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public);
        static readonly MethodInfo Enum_Parse = typeof(Enum).GetMethod("Parse", new[] { typeof(Type), typeof(string), typeof(bool) });
        void ReadEnum(Type enumType)
        {
            ExpectQuote();                          // --empty--

            Emit.LoadConstant(enumType);            // RuntimeTypeHandle
            Emit.Call(Type_GetTypeFromHandle);      // Type

            Emit.LoadArgument(0);                   // Type TextReader
            LoadCharBuffer();                       // Type TextReader char[]
            LoadStringBuilder();                    // Type TextReader char[] StringBuilder
            Emit.Call(Methods.ReadEncodedString);   // Type string

            Emit.LoadConstant(true);                // Type string bool

            Emit.Call(Enum_Parse);                  // object
            Emit.UnboxAny(enumType);                // enum
        }

        void ReadNullable(Type nullableType)
        {
            var underlying = Nullable.GetUnderlyingType(nullableType);

            var nullableConst = nullableType.GetConstructor(new[] { underlying });

            var maybeNull = Emit.DefineLabel();
            var done = Emit.DefineLabel();

            using (var loc = Emit.DeclareLocal(nullableType))
            {
                RawPeekChar();                      // int
                Emit.LoadConstant('n');             // int n
                Emit.BranchIfEqual(maybeNull);      // --empty--

                Build(underlying);                  // underlying
                Emit.NewObject(nullableConst);      // nullableType
                Emit.Branch(done);                  // nullableType

                Emit.MarkLabel(maybeNull);          // --empty--
                ExpectChar('n');                    // --empty--
                ExpectChar('u');                    // --empty--
                ExpectChar('l');                    // --empty--
                ExpectChar('l');                    // --empty--

                Emit.LoadLocalAddress(loc);             // nullableType*
                Emit.InitializeObject(nullableType);    // --empty--
                Emit.LoadLocal(loc);                    // nullableType

                Emit.MarkLabel(done);               // nullableType
            }
        }

        void ReadList(Type listType)
        {
            var elementType = listType.GetListInterface().GetGenericArguments()[0];
            var addMtd = listType.GetMethod("Add");

            var doRead = Emit.DefineLabel();
            var done = Emit.DefineLabel();
            var doneSkipChar = Emit.DefineLabel();

            using (var loc = Emit.DeclareLocal(listType))
            {
                Action loadList;

                if (!listType.IsValueType)
                {
                    loadList = () => Emit.LoadLocal(loc);

                    ExpectRawCharOrNull(
                        '[',
                        () => { },
                        () =>
                        {
                            Emit.LoadNull();
                            Emit.Branch(doneSkipChar);
                        }
                    );
                    Emit.Branch(doRead);
                }
                else
                {
                    loadList = () => Emit.LoadLocalAddress(loc);

                    ExpectChar('[');
                }

                Emit.MarkLabel(doRead);                 // --empty--
                if (listType.IsValueType)
                {
                    Emit.LoadLocalAddress(loc);         // listType*
                    Emit.InitializeObject(listType);    // --empty--
                }
                else
                {
                    Emit.NewObject(listType.GetConstructor(Type.EmptyTypes));   // listType
                    Emit.StoreLocal(loc);                                       // --empty--
                }

                // first step unrolled, cause ',' isn't legal
                ConsumeWhiteSpace();                            // --empty--
                loadList();                                     // listType
                RawPeekChar();                                  // listType int 
                Emit.LoadConstant(']');                         // listType int ']'
                Emit.BranchIfEqual(done);                       // listType(*?)
                Build(elementType);                             // listType(*?) elementType
                Emit.CallVirtual(addMtd);                       // --empty--

                var startLoop = Emit.DefineLabel();
                var nextItem = Emit.DefineLabel();

                Emit.MarkLabel(startLoop);                      // --empty--
                ConsumeWhiteSpace();                            // --empty--
                loadList();                                     // listType(*?)
                RawPeekChar();                                  // listType(*?) int
                Emit.Duplicate();                               // listType(*?) int int
                Emit.LoadConstant(',');                         // listType(*?) int int ','
                Emit.BranchIfEqual(nextItem);                   // listType(*?) int
                Emit.LoadConstant(']');                         // listType(*?) int ']'
                Emit.BranchIfEqual(done);                       // listType(*?)

                // didn't get what we expected
                ThrowExpected(",", "]");

                Emit.MarkLabel(nextItem);           // listType(*?) int
                Emit.Pop();                         // listType(*?)
                Emit.LoadArgument(0);               // listType(*?) TextReader
                Emit.CallVirtual(TextReader_Read);  // listType(*?) int
                Emit.Pop();                         // listType(*?)
                ConsumeWhiteSpace();                // listType(*?)
                Build(elementType);                 // listType(*?) elementType
                Emit.CallVirtual(addMtd);           // --empty--
                Emit.Branch(startLoop);             // --empty--

                Emit.MarkLabel(done);               // listType(*?)
                Emit.LoadArgument(0);               // listType(*?) TextReader
                Emit.CallVirtual(TextReader_Read);  // listType(*?) int
                Emit.Pop();                         // listType(*?)

                Emit.MarkLabel(doneSkipChar);       // listType(*?)
            }
        }

        void ReadDictionary(Type dictType)
        {
            var keyType = dictType.GetDictionaryInterface().GetGenericArguments()[0];
            if(keyType != typeof(string)) throw new Exception("Only dictionaries with strings for keys can be deserialized");
            var valType = dictType.GetDictionaryInterface().GetGenericArguments()[1];

            var addMtd = dictType.GetDictionaryInterface().GetMethod("Add", new [] { typeof(string), valType });

            var done = Emit.DefineLabel();
            var doneSkipChar = Emit.DefineLabel();

            if (!dictType.IsValueType)
            {
                ExpectRawCharOrNull(
                    '{',
                    () => { },
                    () =>
                    {
                        Emit.LoadNull();
                        Emit.Branch(doneSkipChar);
                    }
                );
            }
            else
            {
                ExpectChar('{');
            }

            using (var loc = Emit.DeclareLocal(dictType))
            {
                Action loadDict;
                if (dictType.IsValueType)
                {
                    Emit.LoadLocalAddress(loc);         // dictType*
                    Emit.InitializeObject(dictType);    // --empty--

                    loadDict = () => Emit.LoadLocalAddress(loc);
                }
                else
                {
                    Emit.NewObject(dictType.GetConstructor(Type.EmptyTypes));   // dictType
                    Emit.StoreLocal(loc);                                       // --empty--

                    loadDict = () => Emit.LoadLocal(loc);
                }

                var loopStart = Emit.DefineLabel();

                ConsumeWhiteSpace();        // --empty--
                loadDict();                 // dictType(*?)
                RawPeekChar();              // dictType(*?) int 
                Emit.LoadConstant('}');     // dictType(*?) int '}'
                Emit.BranchIfEqual(done);   // dictType(*?)
                Build(typeof(string));      // dictType(*?) string
                ConsumeWhiteSpace();        // dictType(*?) string
                ExpectChar(':');            // dictType(*?) string
                ConsumeWhiteSpace();        // dictType(*?) string
                Build(valType);             // dictType(*?) string valType
                Emit.CallVirtual(addMtd);   // --empty--

                var nextItem = Emit.DefineLabel();

                Emit.MarkLabel(loopStart);      // --empty--
                ConsumeWhiteSpace();            // --empty--
                loadDict();                     // dictType(*?)
                RawPeekChar();                  // dictType(*?) int 
                Emit.Duplicate();               // dictType(*?) int int
                Emit.LoadConstant(',');         // dictType(*?) int int ','
                Emit.BranchIfEqual(nextItem);   // dictType(*?) int
                Emit.LoadConstant('}');         // dictType(*?) int '}'
                Emit.BranchIfEqual(done);       // dictType(*?)

                // didn't get what we expected
                ThrowExpected(",", "}");

                Emit.MarkLabel(nextItem);           // dictType(*?) int
                Emit.Pop();                         // dictType(*?)
                Emit.LoadArgument(0);               // dictType(*?) TextReader
                Emit.CallVirtual(TextReader_Read);  // dictType(*?) int
                Emit.Pop();                         // dictType(*?)
                ConsumeWhiteSpace();
                Build(typeof(string));              // dictType(*?) string
                ConsumeWhiteSpace();                // dictType(*?) string
                ExpectChar(':');                    // dictType(*?) string
                ConsumeWhiteSpace();                // dictType(*?) string
                Build(valType);                     // dictType(*?) string valType
                Emit.CallVirtual(addMtd);           // --empty--
                Emit.Branch(loopStart);             // --empty--
            }

            Emit.MarkLabel(done);               // dictType(*?)
            Emit.LoadArgument(0);               // dictType(*?) TextReader
            Emit.CallVirtual(TextReader_Read);  // dictType(*?) int
            Emit.Pop();                         // dictType(*?)

            Emit.MarkLabel(doneSkipChar);       // dictType(*?)
        }

        void SkipObjectMember()
        {
            Emit.LoadArgument(0);       // TextReader
            Emit.Call(Methods.Skip);    // --empty--
        }

        void LoadRecursiveTypeDelegate(Type recursiveType)
        {
            var typeCache = RecursionLookupType.MakeGenericType(recursiveType);
            var field = typeCache.GetField("Thunk", BindingFlags.Public | BindingFlags.Static);

            Emit.LoadField(field);
        }

        void ReadObject(Type objType)
        {
            var done = Emit.DefineLabel();
            var doneSkipChar = Emit.DefineLabel();

            if (!objType.IsValueType)
            {
                ExpectRawCharOrNull(
                    '{',
                    () => { },
                    () =>
                    {
                        Emit.LoadNull();
                        Emit.Branch(doneSkipChar);
                    }
                );
            }
            else
            {
                ExpectChar('{');
            }

            using (var loc = Emit.DeclareLocal(objType))
            {
                Action loadObj;
                if (objType.IsValueType)
                {
                    Emit.LoadLocalAddress(loc);     // objType*
                    Emit.InitializeObject(objType); // --empty--

                    loadObj = () => Emit.LoadLocalAddress(loc);
                }
                else
                {
                    Emit.NewObject(objType.GetConstructor(Type.EmptyTypes));    // objType
                    Emit.StoreLocal(loc);                                       // --empty--

                    loadObj = () => Emit.LoadLocal(loc);
                }

                var loopStart = Emit.DefineLabel();

                var setterLookup = typeof(SetterLookup<>).MakeGenericType(objType);

                var setters = (Dictionary<string, MemberInfo>)setterLookup.GetMethod("GetSetters", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]);
                
                var tryGetValue = typeof(Dictionary<string, int>).GetMethod("TryGetValue");

                var order = setterLookup.GetField("Lookup", BindingFlags.Public | BindingFlags.Static);
                var orderInst = (Dictionary<string, int>)order.GetValue(null);
                var labels = setters.ToDictionary(d => d.Key, d => Emit.DefineLabel());

                var inOrderLabels = labels.OrderBy(l => orderInst[l.Key]).Select(l => l.Value).ToArray();

                ConsumeWhiteSpace();        // --empty--
                loadObj();                  // objType(*?)
                RawPeekChar();              // objType(*?) int 
                Emit.LoadConstant('}');     // objType(*?) int '}'
                Emit.BranchIfEqual(done);   // objType(*?)
                Emit.LoadField(order);      // objType(*?) Dictionary<string, int> string
                Build(typeof(string));      // obType(*?) Dictionary<string, int> string
                ConsumeWhiteSpace();        // objType(*?) Dictionary<string, int> string
                ExpectChar(':');            // objType(*?) Dictionary<string, int> string
                ConsumeWhiteSpace();        // objType(*?) Dictionary<string, int> string

                var readingMember = Emit.DefineLabel();
                Emit.MarkLabel(readingMember);  // objType(*?) Dictionary<string, int> string

                using(var oLoc = Emit.DeclareLocal<int>())
                {
                    var isMember = Emit.DefineLabel();

                    Emit.LoadLocalAddress(oLoc);    // objType(*?) Dictionary<string, int> string int*
                    Emit.Call(tryGetValue);         // objType(*?) bool
                    Emit.BranchIfTrue(isMember);    // objType(*?)

                    Emit.Pop();                     // --empty--
                    SkipObjectMember();             // --empty--
                    Emit.Branch(loopStart);         // --empty--

                    Emit.MarkLabel(isMember);       // objType(*?)
                    Emit.LoadLocal(oLoc);           // objType(*?) int
                    Emit.Switch(inOrderLabels);     // objType(*?)

                    // fallthrough case
                    ThrowExpected("a member name"); // --empty--
                }

                foreach (var kv in labels)
                {
                    var label = kv.Value;
                    var member = setters[kv.Key];
                    var memberType = member.ReturnType();

                    Emit.MarkLabel(label);      // objType(*?)
                    Build(member.ReturnType()); // objType(*?) memberType

                    if (member is FieldInfo)
                    {
                        Emit.StoreField((FieldInfo)member);             // --empty--
                    }
                    else
                    {
                        Emit.Call(((PropertyInfo)member).SetMethod);    // --empty--
                    }

                    Emit.Branch(loopStart);     // --empty--
                }

                var nextItem = Emit.DefineLabel();

                Emit.MarkLabel(loopStart);      // --empty--
                ConsumeWhiteSpace();            // --empty--
                loadObj();                      // objType(*?)
                RawPeekChar();                  // objType(*?) int 
                Emit.Duplicate();               // objType(*?) int int
                Emit.LoadConstant(',');         // objType(*?) int int ','
                Emit.BranchIfEqual(nextItem);   // objType(*?) int
                Emit.LoadConstant('}');         // objType(*?) int '}'
                Emit.BranchIfEqual(done);       // objType(*?)

                // didn't get what we expected
                ThrowExpected(",", "}");

                Emit.MarkLabel(nextItem);           // objType(*?) int
                Emit.Pop();                         // objType(*?)
                Emit.LoadArgument(0);               // objType(*?) TextReader
                Emit.CallVirtual(TextReader_Read);  // objType(*?) int
                Emit.Pop();                         // objType(*?)
                ConsumeWhiteSpace();
                Emit.LoadField(order);              // objType(*?) Dictionary<string, int> string
                Build(typeof(string));              // objType(*?) Dictionary<string, int> string
                ConsumeWhiteSpace();                // objType(*?) Dictionary<string, int> string
                ExpectChar(':');                    // objType(*?) Dictionary<string, int> string
                ConsumeWhiteSpace();                // objType(*?) Dictionary<string, int> string
                Emit.Branch(readingMember);         // objType(*?) Dictionary<string, int> string
            }

            Emit.MarkLabel(done);               // objType(*?)
            Emit.LoadArgument(0);               // objType(*?) TextReader
            Emit.CallVirtual(TextReader_Read);  // objType(*?) int
            Emit.Pop();                         // objType(*?)

            Emit.MarkLabel(doneSkipChar);       // objType(*?)
        }

        void Build(Type forType, bool allowRecursion = true)
        {
            if (forType.IsNullableType())
            {
                ReadNullable(forType);
                return;
            }

            if (forType.IsPrimitiveType())
            {
                ReadPrimitive(forType);
                return;
            }

            if (forType.IsDictionaryType())
            {
                ReadDictionary(forType);
                return;
            }

            if (forType.IsListType())
            {
                ReadList(forType);
                return;
            }

            if (forType.IsEnum)
            {
                ReadEnum(forType);
                return;
            }

            if (allowRecursion && RecursiveTypes.Contains(forType))
            {
                var funcType = typeof(Func<,,>).MakeGenericType(typeof(TextReader), typeof(int), forType);
                var funcInvoke = funcType.GetMethod("Invoke");

                LoadRecursiveTypeDelegate(forType); // Func<TextReader, int, memberType>
                Emit.LoadArgument(0);               // Func<TextReader, int, memberType> TextReader
                Emit.LoadArgument(1);               // Func<TextReader, int, memberType> TextReader int
                Emit.LoadConstant(1);               // Func<TextReader, int, memberType> TextReader int 1
                Emit.Add();                         // Func<TextReader, int, memberType> TextReader int
                Emit.Call(funcInvoke);              // memberType
                return;
            }

            ReadObject(forType);
        }

        public Func<TextReader, int, ForType> BuildWithNewDelegate()
        {
            var forType = typeof(ForType);

            RecursiveTypes = forType.FindRecursiveTypes();

            bool doVerify;
#if DEBUG
            doVerify = true;
#else
            doVerify = false;
#endif

            Emit = Emit.NewDynamicMethod(forType, new[] { typeof(TextReader), typeof(int) }, doVerify: doVerify);

            AddGlobalVariables();

            ConsumeWhiteSpace();

            Build(forType, allowRecursion: false);

            // we have to consume this, otherwise we might succeed with invalid JSON
            ConsumeWhiteSpace();

            // We also must confirm that we read everything, again otherwise we might accept garbage as valid
            ExpectEndOfStream();

            Emit.Return();

            return Emit.CreateDelegate<Func<TextReader, int, ForType>>();
        }
    }

    static class InlineDeserializerHelper
    {
        public static Func<TextReader, int, ReturnType> Build<ReturnType>(Type typeCacheType)
        {
            var obj = new InlineDeserializer<ReturnType>(typeCacheType);

            var ret = obj.BuildWithNewDelegate();

            return ret;
        }
    }
}
