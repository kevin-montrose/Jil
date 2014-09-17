using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;
using Sigil.NonGeneric;
using System.Reflection;
using System.Reflection.Emit;

namespace Jil.Deserialize
{
    class InlineDeserializer<ForType>
    {
        public static bool AlwaysUseCharBufferForStrings = true;
        public static bool UseNameAutomata = true;
        public static bool UseNameAutomataForEnums = true;

        const string CharBufferName = "char_buffer";
        const string StringBuilderName = "string_builder";
        
        readonly Type RecursionLookupType;
        readonly DateTimeFormat DateFormat;

        bool UsingCharBuffer;
        HashSet<Type> RecursiveTypes;

        Emit Emit;

        public InlineDeserializer(Type recursionLookupType, DateTimeFormat dateFormat)
        {
            RecursionLookupType = recursionLookupType;
            DateFormat = dateFormat;
        }

        void SetProperty(PropertyInfo prop)
        {
            // Top of stack
            // value
            // object(*?)

            var setMtd = prop.SetMethod;

            if (setMtd.IsVirtual)
            {
                Emit.CallVirtual(setMtd);
            }
            else
            {
                Emit.Call(setMtd);
            }
        }

        void AddGlobalVariables()
        {
            var involvedTypes = typeof(ForType).InvolvedTypes();

            var hasStringyTypes = 
                involvedTypes.Contains(typeof(string)) ||
                involvedTypes.Any(t => t.IsUserDefinedType());

            var needsCharBuffer = (AlwaysUseCharBufferForStrings && hasStringyTypes) || (involvedTypes.Contains(typeof(DateTime)) && DateFormat == DateTimeFormat.ISO8601);

            if (needsCharBuffer)
            {
                UsingCharBuffer = true;

                Emit.DeclareLocal<char[]>(CharBufferName);

                Emit.LoadConstant(Methods.CharBufferSize);  // int
                Emit.NewArray<char>();                      // char[]
                Emit.StoreLocal(CharBufferName);            // --empty--
            }

            // we can't know, for sure, that a StringBuilder will be needed w/o seeing the data
            //   but we can save the slot on the stack in rare cases
            var mayNeedStringBuilder =
                involvedTypes.Contains(typeof(string)) ||
                involvedTypes.Contains(typeof(float)) ||
                involvedTypes.Contains(typeof(double)) ||
                involvedTypes.Contains(typeof(decimal)) ||
                involvedTypes.Any(t => t.IsEnum) ||
                involvedTypes.Any(t => t.IsUserDefinedType());

            if (mayNeedStringBuilder)
            {
                Emit.DeclareLocal<StringBuilder>(StringBuilderName);
            }
        }

        void LoadCharBuffer()
        {
            Emit.LoadLocal(CharBufferName);
        }

        void LoadStringBuilder()
        {
            Emit.LoadLocalAddress(StringBuilderName);
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
            Emit.LoadConstant("Expected character: '" + c + "'");               // string
            Emit.LoadArgument(0);                                               // string TextReader
            Emit.NewObject<DeserializationException, string, TextReader>();     // DeserializationException
            Emit.Throw();
        }

        void ThrowExpected(params object[] ps)
        {
            Emit.LoadConstant("Expected: " + string.Join(", ", ps));                // string
            Emit.LoadArgument(0);                                                   // string TextReader
            Emit.NewObject<DeserializationException, string, TextReader>();         // DeserializationException
            Emit.Throw();
        }

        void ExpectChar(char c)
        {
            var gotChar = Emit.DefineLabel();

            Emit.LoadArgument(0);                   // TextReader
            Emit.CallVirtual(TextReader_Read);      // int
            Emit.LoadConstant((int)c);              // int int
            Emit.BranchIfEqual(gotChar);            // --empty--
            ThrowExpected(c);                       // --empty--

            Emit.MarkLabel(gotChar);                // --empty--
        }

        void CheckChar(char c)
        {
            var gotChar = Emit.DefineLabel();

            Emit.LoadConstant((int)c);              // int int
            Emit.BranchIfEqual(gotChar);            // --empty--
            ThrowExpected(c);                       // --empty--

            Emit.MarkLabel(gotChar);                // --empty--
        }

        void ExpectQuote()
        {
            ExpectChar('"');
        }

        void CheckQuote()
        {
            CheckChar('"');
        }

        void ExpectRawCharOrNull(char c, Action ifChar, Action ifNull)
        {
            var gotChar = Emit.DefineLabel();
            var gotN = Emit.DefineLabel();
            var done = Emit.DefineLabel();

            RawReadChar(() => ThrowExpected(c, "null")); // int
            Emit.Duplicate();                               // int int
            Emit.LoadConstant((int)c);                      // int int int
            Emit.BranchIfEqual(gotChar);                    // int 
            Emit.LoadConstant((int)'n');                    // int n
            Emit.BranchIfEqual(gotN);                       // --empty--
            ThrowExpected(c, "null");                    // --empty--

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
            Emit.Call(Methods.ReadEncodedChar); // char
        }

        void CallReadEncodedString()
        {
            // Stack starts
            // TextReader

            if (UsingCharBuffer)
            {
                LoadCharBuffer();                           // TextReader char[]
                LoadStringBuilder();                        // TextReader char[] StringBuilder
                Emit.Call(Methods.ReadEncodedStringWithBuffer);   // string
            }
            else
            {
                LoadStringBuilder();                        // TextReader StringBuilder
                Emit.Call(Methods.ReadEncodedString);  // string
            }
        }

        void ReadString()
        {
            ExpectQuoteOrNull(
                delegate
                {
                    // --empty--
                    Emit.LoadArgument(0);                   // TextReader
                    CallReadEncodedString();                // string
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

            throw new ConstructionException("Unexpected number type: " + numberType);
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

        void ReadGuid()
        {
            ExpectQuote();
            Emit.LoadArgument(0);           // TextReader
            Emit.Call(Methods.ReadGuid);    // Guid
            ExpectQuote();                  // Guid
        }

        static readonly ConstructorInfo DateTimeOffsetConst = typeof(DateTimeOffset).GetConstructor(new[] { typeof(DateTime) });
        void ReadDateTimeOffset()
        {
            ReadDate();                             // DateTime
            Emit.NewObject(DateTimeOffsetConst);    // DateTimeOffset
        }

        void ReadDate()
        {
            switch (DateFormat)
            {
                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch: ReadNewtosoftDateTime(); break;
                case DateTimeFormat.MillisecondsSinceUnixEpoch: ReadMillisecondsDateTime(); break;
                case DateTimeFormat.SecondsSinceUnixEpoch: ReadSecondsDateTime(); break;
                case DateTimeFormat.ISO8601: ReadISO8601DateTime(); break;
                default: throw new ConstructionException("Unexpected DateTimeFormat: " + DateFormat);
            }
            
        }

        void ReadNewtosoftDateTime()
        {
            var isPlus = Emit.DefineLabel();
            var isMinus = Emit.DefineLabel();
            var expectEnd = Emit.DefineLabel();
            var withTimeZone = Emit.DefineLabel();

            ExpectQuote();                                      // --empty--
            ExpectChar('\\');                                   // --empty--
            ExpectChar('/');                                    // --empty--
            ExpectChar('D');                                    // --empty--
            ExpectChar('a');                                    // --empty--
            ExpectChar('t');                                    // --empty--
            ExpectChar('e');                                    // --empty--
            ExpectChar('(');                                    // --empty--
            ReadPrimitive(typeof(long));                        // long
            Emit.LoadArgument(0);                               // long TextReader
            Emit.Call(Methods.DiscardNewtonsoftTimeZoneOffset); // long
            ExpectChar(')');                                    // long
            ExpectChar('\\');                                   // long
            ExpectChar('/');                                    // long

            // convert MS into ticks
            Emit.LoadConstant(10000L);              // long long
            Emit.Multiply();                        // long
            Emit.LoadConstant(621355968000000000L); // long long
            Emit.Add();                             // long

            Emit.LoadConstant((int)DateTimeKind.Utc);       // long DateTimeKind
            Emit.NewObject<DateTime, long, DateTimeKind>(); // DateTime

            ExpectQuote();                  // DateTime
        }

        void ReadMillisecondsDateTime()
        {
            ReadPrimitive(typeof(long));            // long
            Emit.LoadConstant(10000L);              // long long
            Emit.Multiply();                        // long
            Emit.LoadConstant(621355968000000000L); // long long
            Emit.Add();                             // long

            Emit.LoadConstant((int)DateTimeKind.Utc);       // long DateTimeKind
            Emit.NewObject<DateTime, long, DateTimeKind>(); // DateTime
        }

        void ReadSecondsDateTime()
        {
            ReadPrimitive(typeof(long));            // long
            Emit.LoadConstant(10000000L);           // long long
            Emit.Multiply();                        // long
            Emit.LoadConstant(621355968000000000L); // long long
            Emit.Add();                             // long

            Emit.LoadConstant((int)DateTimeKind.Utc);       // long DateTimeKind
            Emit.NewObject<DateTime, long, DateTimeKind>(); // DateTime
        }

        void ReadISO8601DateTime()
        {
            ExpectQuote();                      // --empty--
            Emit.LoadArgument(0);               // TextReader
            LoadCharBuffer();
            Emit.Call(Methods.ReadISO8601Date); // DateTime
            ExpectQuote();                      // DateTime
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

            if (primitiveType == typeof(Guid))
            {
                ReadGuid();
                return;
            }

            if (primitiveType == typeof(DateTime))
            {
                ReadDate();
                return;
            }

            if (primitiveType == typeof(DateTimeOffset))
            {
                ReadDateTimeOffset();
                return;
            }

            ReadNumber(primitiveType);
        }

        void ConsumeWhiteSpace()
        {
            Emit.LoadArgument(0);                   // TextReader
            Emit.Call(Methods.ConsumeWhiteSpace);       // --empty--
        }

        void ReadSkipWhitespace()
        {
            Emit.LoadArgument(0);                   // TextReader
            Emit.Call(Methods.ReadSkipWhitespace);  // int
        }

        void ExpectEndOfStream()
        {
            var success = Emit.DefineLabel();

            Emit.LoadArgument(0);           // TextReader
            Emit.Call(TextReader_Read);     // int
            Emit.LoadConstant(-1);          // int -1
            Emit.BranchIfEqual(success);    // --empty--

            Emit.LoadConstant("Expected end of stream");                    // string
            Emit.LoadArgument(0);                                           // string TextReader
            Emit.NewObject<DeserializationException, string, TextReader>(); // DeserializationException
            Emit.Throw();                                                   // --empty--

            Emit.MarkLabel(success);        // --empty--
        }

        static readonly MethodInfo TextReader_Peek = typeof(TextReader).GetMethod("Peek", BindingFlags.Public | BindingFlags.Instance);
        void RawPeekChar()
        {
            Emit.LoadArgument(0);                   // TextReader
            Emit.CallVirtual(TextReader_Peek);      // int
        }

        void ReadFlagsEnum(Type enumType)
        {
            ExpectQuote();                  // --empty--

            var specific = Methods.ReadFlagsEnum.MakeGenericMethod(enumType);

            Emit.LoadArgument(0);           // TextReader
            LoadStringBuilder();            // TextReader StringBuilder&
            Emit.Call(specific);            // enum
        }

        void ReadEnum(Type enumType)
        {
            if (UseNameAutomataForEnums)
            {
                var setterLookup = typeof(EnumLookup<>).MakeGenericType(enumType);
                var getEnumValue = setterLookup.GetMethod("GetEnumValue", new[] { typeof(TextReader) });

                ExpectQuote();
                Emit.LoadArgument(0);     // TextReader
                Emit.Call(getEnumValue);  // emum

                return;
            }

            if (enumType.IsFlagsEnum())
            {
                ReadFlagsEnum(enumType);
                return;
            }

            var specific = Methods.ParseEnum.MakeGenericMethod(enumType);

            ExpectQuote();                  // --empty--
            Emit.LoadArgument(0);           // TextReader
            CallReadEncodedString();        // string
            Emit.LoadArgument(0);           // TextReader
            Emit.Call(specific);            // enum
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

            throw new ConstructionException("Unexpected type: " + type);
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

            if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                listType = typeof(List<>).MakeGenericType(elementType);
            }

            var addMtd = listType.GetMethod("Add");

            var doRead = Emit.DefineLabel();
            var done = Emit.DefineLabel();
            var doneSkipChar = Emit.DefineLabel();

            var isArray = listType.IsArray;

            if (isArray)
            {
                listType = typeof(List<>).MakeGenericType(elementType);
                addMtd = listType.GetMethod("Add");
            }

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
                loadList();                                     // listType(*?)
                ReadSkipWhitespace();                           // listType(*?) int
                Emit.Duplicate();                               // listType(*?) int int
                Emit.LoadConstant(',');                         // listType(*?) int int ','
                Emit.BranchIfEqual(nextItem);                   // listType(*?) int
                Emit.LoadConstant(']');                         // listType(*?) int ']'
                Emit.BranchIfEqual(doneSkipChar);               // listType(*?)

                // didn't get what we expected
                ThrowExpected(",", "]");

                Emit.MarkLabel(nextItem);           // listType(*?) int
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

                if (isArray)
                {
                    var toArray = listType.GetMethod("ToArray");
                    Emit.Call(toArray);             // elementType[]
                }
            }
        }

        void ReadDictionary(Type dictType)
        {
            var keyType = dictType.GetDictionaryInterface().GetGenericArguments()[0];

            var keyIsString = keyType == typeof(string);
            var keyIsInteger = keyType.IsIntegerNumberType();
            var keyIsEnum = keyType.IsEnum;

            if (!(keyIsString || keyIsInteger || keyIsEnum)) throw new ConstructionException("Only dictionaries with strings, integers, or enums for keys can be deserialized");
            var valType = dictType.GetDictionaryInterface().GetGenericArguments()[1];

            if (dictType.IsGenericType && dictType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valType);
            }

            var addMtd = dictType.GetDictionaryInterface().GetMethod("Add", new [] { keyType, valType });

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
                if (keyType == typeof(string))
                {
                    Build(typeof(string));  // dictType(*?) string
                }
                else
                {
                    if (keyIsInteger)
                    {
                        ExpectQuote();          // dictType(*?)
                        Build(keyType);         // dictType(*?) integer
                        ExpectQuote();          // dictType(*?) integer
                    }
                    else
                    {
                        Build(keyType);         // dictType(*?) enum
                    }
                }
                ReadSkipWhitespace();        // dictType(*?) (integer|string|enum)
                CheckChar(':');            // dictType(*?) (integer|string|enum)
                ConsumeWhiteSpace();        // dictType(*?) (integer|string|enum)
                Build(valType);             // dictType(*?) (integer|string|enum) valType
                Emit.CallVirtual(addMtd);   // --empty--

                var nextItem = Emit.DefineLabel();

                Emit.MarkLabel(loopStart);      // --empty--
                loadDict();                     // dictType(*?)
                ReadSkipWhitespace();           // dictType(*?) int 
                Emit.Duplicate();               // dictType(*?) int int
                Emit.LoadConstant(',');         // dictType(*?) int int ','
                Emit.BranchIfEqual(nextItem);   // dictType(*?) int
                Emit.LoadConstant('}');         // dictType(*?) int '}'
                Emit.BranchIfEqual(doneSkipChar); // dictType(*?)

                // didn't get what we expected
                ThrowExpected(",", "}");

                Emit.MarkLabel(nextItem);           // dictType(*?) int
                Emit.Pop();                         // dictType(*?)
                ConsumeWhiteSpace();                // dictType(*?)
                if (keyType == typeof(string))
                {
                    Build(typeof(string));  // dictType(*?) string
                }
                else
                {
                    if (keyIsInteger)
                    {
                        ExpectQuote();          // dictType(*?)
                        Build(keyType);         // dictType(*?) integer
                        ExpectQuote();          // dictType(*?) integer
                    }
                    else
                    {
                        Build(keyType);         // dictType(*?) enum
                    }
                }
                ReadSkipWhitespace();               // dictType(*?) (integer|string|enum)
                CheckChar(':');                     // dictType(*?) (integer|string|enum)
                ConsumeWhiteSpace();                // dictType(*?) (integer|string|enum)
                Build(valType);                     // dictType(*?) (integer|string|enum) valType
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
            var thunk = typeCache.GetField("Thunk", BindingFlags.Public | BindingFlags.Static);
            Emit.LoadField(thunk);
        }

        void ReadObject(Type objType)
        {
            var isAnonymous = objType.IsAnonymouseClass();

            if (isAnonymous)
            {
                if (UseNameAutomata)
                {
                    ReadAnonymousObjectAutomata(objType);
                    return;
                }

                ReadAnonymousObjectDictionaryLookup(objType);
                return;
            }

            if (UseNameAutomata)
            {
                ReadObjectAutomata(objType);
                return;
            }
            
            ReadObjectDictionaryLookup(objType);
        }

        void SkipAllMembers(Sigil.Label done, Sigil.Label doneSkipChar)
        {
            var continueSkipping = Emit.DefineLabel();

            // first pass, doesn't check for ,
            ConsumeWhiteSpace();                // objType
            RawPeekChar();                      // objType char
            Emit.LoadConstant('}');             // objType char '}'
            Emit.BranchIfEqual(done);           // objType

            Emit.LoadArgument(0);                   // objType TextReader
            Emit.Call(Methods.SkipEncodedString);   // objType
            ReadSkipWhitespace();                   // objType
            CheckChar(':');                         // objType
            ConsumeWhiteSpace();                    // objType
            SkipObjectMember();                     // objType
            Emit.Branch(continueSkipping);          // objType

            // second (and third, and fourth, and ...) does check for ,
            Emit.MarkLabel(continueSkipping);   // objType
            ConsumeWhiteSpace();                // objType
            RawPeekChar();                      // objType char
            Emit.LoadConstant('}');             // objType char '}'
            Emit.BranchIfEqual(done);           // objType

            // we Peek'd the }, we can just read for ','
            ExpectChar(',');                    // objType

            ConsumeWhiteSpace();                    // objType
            Emit.LoadArgument(0);                   // objType TextReader
            Emit.Call(Methods.SkipEncodedString);   // objType
            ReadSkipWhitespace();                   // objType
            CheckChar(':');                         // objType
            ConsumeWhiteSpace();                    // objType
            SkipObjectMember();                     // objType
            Emit.Branch(continueSkipping);          // objType

            Emit.MarkLabel(done);               // objType
            Emit.LoadArgument(0);               // objType TextReader
            Emit.CallVirtual(TextReader_Read);  // objType int
            Emit.Pop();                         // objType

            Emit.MarkLabel(doneSkipChar);   // objType(*?)
        }

        void ReadObjectAutomata(Type objType)
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
                    var cons = objType.GetConstructor(Type.EmptyTypes);

                    if (cons == null) throw new ConstructionException("Expected a parameterless constructor for " + objType);

                    Emit.NewObject(cons);   // objType
                    Emit.StoreLocal(loc);   // --empty--

                    loadObj = () => Emit.LoadLocal(loc);
                }

                var loopStart = Emit.DefineLabel();

                var setterLookup = typeof(SetterLookup<>).MakeGenericType(objType);

                var setters = (Dictionary<string, MemberInfo>)setterLookup.GetMethod("GetSetters", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]);

                // special case object w/ no deserializable properties
                if (setters.Count == 0)
                {
                    loadObj();                      // objType(*?)

                    if (objType.IsValueType)
                    {
                        Emit.LoadObject(objType);   // objType
                    }

                    SkipAllMembers(done, doneSkipChar); // objType

                    return;
                }

                var orderedSetters =
                    setters
                    .OrderBy(kv => kv.Key)
                    .Select((kv, i) => new { Index = i, Name = kv.Key, Setter = kv.Value, Label = Emit.DefineLabel() })
                    .ToList();

                var findSetterIdx = setterLookup.GetMethod("FindSetterIndex", new[] { typeof(TextReader) });

                var inOrderLabels =
                    orderedSetters
                    .Select(o => o.Label)
                    .ToArray();

                ConsumeWhiteSpace();        // --empty--
                loadObj();                  // objType(*?)
                RawPeekChar();              // objType(*?) int 
                Emit.LoadConstant('}');     // objType(*?) int '}'
                Emit.BranchIfEqual(done);   // objType(*?)
                ReadSkipWhitespace();       // objType(*?)

                CheckQuote();               // objType(*?)
                Emit.LoadArgument(0);       // objType(*?) TextReader
                Emit.Call(findSetterIdx);  // objType(*?) int

                ReadSkipWhitespace();        // objType(*?) int
                CheckChar(':');            // objType(*?) int
                ConsumeWhiteSpace();        // objType(*?) int

                var readingMember = Emit.DefineLabel();
                Emit.MarkLabel(readingMember);  // objType(*?) int

                using (var oLoc = Emit.DeclareLocal<int>())
                {
                    var isMember = Emit.DefineLabel();

                    Emit.Duplicate();// objType(*?) int int
                    Emit.StoreLocal(oLoc);// objType(*?) int
                    Emit.LoadConstant(0); // objType(*?) int int
                    Emit.BranchIfGreaterOrEqual(isMember); // objType(*?)

                    Emit.Pop();                     // --empty--
                    SkipObjectMember();             // --empty--
                    Emit.Branch(loopStart);         // --empty--

                    Emit.MarkLabel(isMember);       // objType(*?)
                    Emit.LoadLocal(oLoc);           // objType(*?) int
                    Emit.Switch(inOrderLabels);     // objType(*?)

                    // fallthrough case
                    ThrowExpected("a member name"); // --empty--
                }

                foreach (var kv in orderedSetters)
                {
                    var label = kv.Label;
                    var member = kv.Setter;
                    var memberType = member.ReturnType();

                    Emit.MarkLabel(label);      // objType(*?)
                    Build(member.ReturnType()); // objType(*?) memberType

                    if (member is FieldInfo)
                    {
                        Emit.StoreField((FieldInfo)member);             // --empty--
                    }
                    else
                    {
                        SetProperty((PropertyInfo)member);              // --empty--
                    }

                    Emit.Branch(loopStart);     // --empty--
                }

                var nextItem = Emit.DefineLabel();

                Emit.MarkLabel(loopStart);      // --empty--
                loadObj();                      // objType(*?)
                ReadSkipWhitespace();           // objType(*?) int 
                Emit.Duplicate();               // objType(*?) int int
                Emit.LoadConstant(',');         // objType(*?) int int ','
                Emit.BranchIfEqual(nextItem);   // objType(*?) int
                Emit.LoadConstant('}');         // objType(*?) int '}'
                Emit.BranchIfEqual(doneSkipChar);// objType(*?)

                // didn't get what we expected
                ThrowExpected(",", "}");

                Emit.MarkLabel(nextItem);           // objType(*?) int
                Emit.Pop();                         // objType(*?)
                ReadSkipWhitespace();

                CheckQuote();
                Emit.LoadArgument(0);           // TextReader
                Emit.Call(findSetterIdx);      // int

                ReadSkipWhitespace();               // objType(*?) int
                CheckChar(':');                     // objType(*?) int
                ConsumeWhiteSpace();                // objType(*?) int
                Emit.Branch(readingMember);         // objType(*?) int
            }

            Emit.MarkLabel(done);               // objType(*?)
            Emit.LoadArgument(0);               // objType(*?) TextReader
            Emit.CallVirtual(TextReader_Read);  // objType(*?) int
            Emit.Pop();                         // objType(*?)

            Emit.MarkLabel(doneSkipChar);       // objType(*?)

            if (objType.IsValueType)
            {
                Emit.LoadObject(objType);     // objType
            }
        }

        void ReadObjectDictionaryLookup(Type objType)
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
                    var cons = objType.GetConstructor(Type.EmptyTypes);

                    if (cons == null) throw new ConstructionException("Expected a parameterless constructor for " + objType);

                    Emit.NewObject(cons);   // objType
                    Emit.StoreLocal(loc);   // --empty--

                    loadObj = () => Emit.LoadLocal(loc);
                }

                var loopStart = Emit.DefineLabel();

                var setterLookup = typeof(SetterLookup<>).MakeGenericType(objType);

                var setters = (Dictionary<string, MemberInfo>)setterLookup.GetMethod("GetSetters", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]);

                // special case object w/ no deserializable properties
                if (setters.Count == 0)
                {
                    loadObj();                      // objType(*?)

                    if (objType.IsValueType)
                    {
                        Emit.LoadObject(objType);   // objType
                    }

                    SkipAllMembers(done, doneSkipChar); // objType

                    return;
                }

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
                ReadSkipWhitespace();       // objType(*?) Dictionary<string, int> string
                CheckChar(':');             // objType(*?) Dictionary<string, int> string
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
                        SetProperty((PropertyInfo)member);              // --empty--
                    }

                    Emit.Branch(loopStart);     // --empty--
                }

                var nextItem = Emit.DefineLabel();

                Emit.MarkLabel(loopStart);      // --empty--
                loadObj();                      // objType(*?)
                ReadSkipWhitespace();                  // objType(*?) int 
                Emit.Duplicate();               // objType(*?) int int
                Emit.LoadConstant(',');         // objType(*?) int int ','
                Emit.BranchIfEqual(nextItem);   // objType(*?) int
                Emit.LoadConstant('}');         // objType(*?) int '}'
                Emit.BranchIfEqual(doneSkipChar);       // objType(*?)

                // didn't get what we expected
                ThrowExpected(",", "}");

                Emit.MarkLabel(nextItem);           // objType(*?) int
                Emit.Pop();                         // objType(*?)
                ConsumeWhiteSpace();
                Emit.LoadField(order);              // objType(*?) Dictionary<string, int> string
                Build(typeof(string));              // objType(*?) Dictionary<string, int> string
                ReadSkipWhitespace();               // objType(*?) Dictionary<string, int> string
                CheckChar(':');                     // objType(*?) Dictionary<string, int> string
                ConsumeWhiteSpace();                // objType(*?) Dictionary<string, int> string
                Emit.Branch(readingMember);         // objType(*?) Dictionary<string, int> string
            }

            Emit.MarkLabel(done);               // objType(*?)
            Emit.LoadArgument(0);               // objType(*?) TextReader
            Emit.CallVirtual(TextReader_Read);  // objType(*?) int
            Emit.Pop();                         // objType(*?)

            Emit.MarkLabel(doneSkipChar);       // objType(*?)

            if(objType.IsValueType)
            {
                Emit.LoadObject(objType);     // objType
            }
        }

        void ReadAnonymousObjectDictionaryLookup(Type objType)
        {
            var doneNotNull = Emit.DefineLabel();
            var doneNull = Emit.DefineLabel();

            ExpectRawCharOrNull(
                '{',
                () => { },
                () =>
                {
                    Emit.Branch(doneNull);
                }
            );

            var cons = objType.GetConstructors().Single();

            var setterLookup = typeof(AnonymousTypeLookup<>).MakeGenericType(objType);
            var propertyMap = (Dictionary<string, Tuple<Type, int>>)setterLookup.GetField("ParametersToTypeAndIndex").GetValue(null);

            if (propertyMap.Count == 0)
            {
                var doneSkipChar = Emit.DefineLabel();

                Emit.NewObject(cons);           // objType
                Emit.Branch(doneNotNull);       // objType

                Emit.MarkLabel(doneNull);       // null
                Emit.LoadNull();                // null
                Emit.Branch(doneSkipChar);

                Emit.MarkLabel(doneNotNull);    // objType

                var doneSkipping = Emit.DefineLabel();
                
                SkipAllMembers(doneSkipping, doneSkipChar); // objType

                return;
            }

            var order = setterLookup.GetField("Lookup", BindingFlags.Public | BindingFlags.Static);
            var tryGetValue = typeof(Dictionary<string, int>).GetMethod("TryGetValue");
            var orderInst = (Dictionary<string, int>)order.GetValue(null);

            var localMap = new Dictionary<string, Sigil.Local>();
            foreach (var kv in propertyMap)
            {
                localMap[kv.Key] = Emit.DeclareLocal(kv.Value.Item1);
            }

            var labels = orderInst.ToDictionary(d => d.Key, d => Emit.DefineLabel());
            var inOrderLabels = labels.OrderBy(l => orderInst[l.Key]).Select(l => l.Value).ToArray();

            var loopStart = Emit.DefineLabel();

            ConsumeWhiteSpace();        // --empty--
            RawPeekChar();              // int 
            Emit.LoadConstant('}');     // int '}'
            Emit.BranchIfEqual(doneNotNull);   // --empty--
            Emit.LoadField(order);      // Dictionary<string, int> string
            Build(typeof(string));      // Dictionary<string, int> string
            ReadSkipWhitespace();       // Dictionary<string, int> string
            CheckChar(':');             // Dictionary<string, int> string
            ConsumeWhiteSpace();        // Dictionary<string, int> string

            var readingMember = Emit.DefineLabel();
            Emit.MarkLabel(readingMember);  // Dictionary<string, int> string

            using (var oLoc = Emit.DeclareLocal<int>())
            {
                var isMember = Emit.DefineLabel();

                Emit.LoadLocalAddress(oLoc);    // Dictionary<string, int> string int*
                Emit.Call(tryGetValue);         // bool
                Emit.BranchIfTrue(isMember);    // --empty--

                SkipObjectMember();             // --empty--
                Emit.Branch(loopStart);         // --empty--

                Emit.MarkLabel(isMember);       // --empty--
                Emit.LoadLocal(oLoc);           // int
                Emit.Switch(inOrderLabels);     // --empty--

                // fallthrough case
                ThrowExpected("a member name"); // --empty--
            }

            foreach (var kv in labels)
            {
                var label = kv.Value;
                var local = localMap[kv.Key];

                Emit.MarkLabel(label);  // --empty--
                Build(local.LocalType); // localType

                Emit.StoreLocal(local); // --empty--

                Emit.Branch(loopStart); // --empty--
            }

            var nextItem = Emit.DefineLabel();

            Emit.MarkLabel(loopStart);      // --empty--
            ConsumeWhiteSpace();            // --empty--
            RawPeekChar();                  // int 
            Emit.Duplicate();               // int int
            Emit.LoadConstant(',');         // int int ','
            Emit.BranchIfEqual(nextItem);   // int
            Emit.LoadConstant('}');         // int '}'
            Emit.BranchIfEqual(doneNotNull);       // --empty--

            // didn't get what we expected
            ThrowExpected(",", "}");

            Emit.MarkLabel(nextItem);           // int
            Emit.Pop();                         // --empty--
            Emit.LoadArgument(0);               // TextReader
            Emit.CallVirtual(TextReader_Read);  // int
            Emit.Pop();                         // --empty--
            ConsumeWhiteSpace();                // --empty--
            Emit.LoadField(order);              // Dictionary<string, int> string
            Build(typeof(string));              // Dictionary<string, int> string
            ReadSkipWhitespace();               // Dictionary<string, int> string
            CheckChar(':');                     // Dictionary<string, int> string
            ConsumeWhiteSpace();                // Dictionary<string, int> string
            Emit.Branch(readingMember);         // Dictionary<string, int> string

            Emit.MarkLabel(doneNotNull);               // --empty--
            Emit.LoadArgument(0);               // TextReader
            Emit.CallVirtual(TextReader_Read);  // int
            Emit.Pop();                         // --empty--

            var done = Emit.DefineLabel();

            foreach(var kv in propertyMap.OrderBy(o => o.Value.Item2))
            {
                var local = localMap[kv.Key];
                Emit.LoadLocal(local);
            }

            Emit.NewObject(cons);               // objType
            Emit.Branch(done);                  // objType

            Emit.MarkLabel(doneNull);           // --empty--
            Emit.LoadNull();                    // null

            Emit.MarkLabel(done);               // objType

            // free up all those locals for use again
            foreach (var kv in localMap)
            {
                kv.Value.Dispose();
            }
        }

        void ReadAnonymousObjectAutomata(Type objType)
        {
            var doneNotNull = Emit.DefineLabel();
            var doneNotNullPopSkipChar = Emit.DefineLabel();
            var doneNotNullSkipChar = Emit.DefineLabel();
            var doneNull = Emit.DefineLabel();

            ExpectRawCharOrNull(
                '{',
                () => { },
                () =>
                {
                    Emit.Branch(doneNull);
                }
            );

            var cons = objType.GetConstructors().Single();

            var setterLookup = typeof(AnonymousTypeLookup<>).MakeGenericType(objType);
            var findConstructorParameterIndex = setterLookup.GetMethod("FindConstructorParameterIndex", new[] { typeof(TextReader) });
            var propertyMap = (Dictionary<string, Tuple<Type, int>>)setterLookup.GetField("ParametersToTypeAndIndex").GetValue(null);

            if (propertyMap.Count == 0)
            {
                var doneSkipChar = Emit.DefineLabel();

                Emit.NewObject(cons);           // objType
                Emit.Branch(doneNotNull);       // objType

                Emit.MarkLabel(doneNull);       // null
                Emit.LoadNull();                // null
                Emit.Branch(doneSkipChar);

                Emit.MarkLabel(doneNotNull);    // objType

                var doneSkipping = Emit.DefineLabel();

                SkipAllMembers(doneSkipping, doneSkipChar); // objType

                return;
            }

            var localMap = new Dictionary<string, Sigil.Local>();
            foreach (var kv in propertyMap)
            {
                localMap[kv.Key] = Emit.DeclareLocal(kv.Value.Item1);
            }

            var labels =
                propertyMap
                .ToDictionary(d => d.Key, d => Emit.DefineLabel());

            var inOrderLabels =
                propertyMap
                .OrderBy(l => l.Value.Item2)
                .Select(l => labels[l.Key])
                .ToArray();

            var loopStart = Emit.DefineLabel();

            ReadSkipWhitespace();       // int 
            Emit.Duplicate();
            Emit.LoadConstant('}');     // int '}'
            Emit.BranchIfEqual(doneNotNullPopSkipChar);   // --empty--

            CheckQuote();                               // --empty--
            Emit.LoadArgument(0);                       // TextReader
            Emit.Call(findConstructorParameterIndex);   // int

            ReadSkipWhitespace();       // int
            CheckChar(':');             // int
            ConsumeWhiteSpace();        // int

            var readingMember = Emit.DefineLabel();
            Emit.MarkLabel(readingMember);  // int

            Emit.Switch(inOrderLabels);     // --empty--
            SkipObjectMember();             // --empty--

            foreach (var kv in labels)
            {
                var label = kv.Value;
                var local = localMap[kv.Key];

                Emit.MarkLabel(label);  // --empty--
                Build(local.LocalType); // localType

                Emit.StoreLocal(local); // --empty--

                Emit.Branch(loopStart); // --empty--
            }

            var nextItem = Emit.DefineLabel();

            Emit.MarkLabel(loopStart);      // --empty--
            ReadSkipWhitespace();           // --empty--
            Emit.Duplicate();               // int int
            Emit.LoadConstant(',');         // int int ','
            Emit.BranchIfEqual(nextItem);   // int
            Emit.LoadConstant('}');         // int '}'
            Emit.BranchIfEqual(doneNotNullSkipChar);// --empty--

            // didn't get what we expected
            ThrowExpected(",", "}");

            Emit.MarkLabel(nextItem);           // int
            Emit.Pop();                         // --empty--
            ReadSkipWhitespace();               // --empty--

            CheckQuote();                               // --empty--
            Emit.LoadArgument(0);                       // TextReader
            Emit.Call(findConstructorParameterIndex);   // int

            ReadSkipWhitespace();               // int
            CheckChar(':');                     // int
            ConsumeWhiteSpace();                // int
            Emit.Branch(readingMember);         // int

            Emit.MarkLabel(doneNotNull);        // --empty--
            Emit.LoadArgument(0);               // TextReader
            Emit.CallVirtual(TextReader_Read);  // int

            Emit.MarkLabel(doneNotNullPopSkipChar);
            Emit.Pop();                         // --empty--

            Emit.MarkLabel(doneNotNullSkipChar);// --empty--

            var done = Emit.DefineLabel();

            foreach (var kv in propertyMap.OrderBy(o => o.Value.Item2))
            {
                var local = localMap[kv.Key];
                Emit.LoadLocal(local);
            }

            Emit.NewObject(cons);               // objType
            Emit.Branch(done);                  // objType

            Emit.MarkLabel(doneNull);           // --empty--
            Emit.LoadNull();                    // null

            Emit.MarkLabel(done);               // objType

            // free up all those locals for use again
            foreach (var kv in localMap)
            {
                kv.Value.Dispose();
            }
        }

        static ConstructorInfo OptionsCons = typeof(Options).GetConstructor(new[] { typeof(bool), typeof(bool), typeof(bool), typeof(DateTimeFormat), typeof(bool) });
        static ConstructorInfo ObjectBuilderCons = typeof(Jil.DeserializeDynamic.ObjectBuilder).GetConstructor(new[] { typeof(Options) });
        void ReadDynamic()
        {
            using (var dyn = Emit.DeclareLocal<Jil.DeserializeDynamic.ObjectBuilder>())
            {
                Emit.LoadArgument(0);                                                       // TextReader
                Emit.LoadConstant(false);                                                   // TextReader bool
                Emit.LoadConstant(false);                                                   // TextReader bool bool
                Emit.LoadConstant(false);                                                   // TextReader bool bool bool
                Emit.LoadConstant((byte)DateFormat);                                        // TextReader bool bool bool byte
                Emit.LoadConstant(false);                                                   // TextReader bool bool bool byte bool
                Emit.NewObject(OptionsCons);                                                // TextReader Options
                Emit.NewObject(ObjectBuilderCons);                                          // TextReader ObjectBuilder
                Emit.StoreLocal(dyn);                                                       // TextReader
                Emit.LoadLocal(dyn);                                                        // TextReader ObjectBuilder
                Emit.Call(Jil.DeserializeDynamic.DynamicDeserializer.DeserializeMember);    // --empty--
                Emit.LoadLocal(dyn);                                                        // ObjectBuilder
                Emit.LoadField(Jil.DeserializeDynamic.ObjectBuilder._BeingBuilt);           // JsonObject
            }
        }

        void Build(Type forType, bool allowRecursion = true)
        {
            // EXACT MATCH, this is the best way to detect `dynamic`
            if (forType == typeof(object))
            {
                ReadDynamic();
                return;
            }

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

            if (forType.IsGenericReadOnlyDictionary())
            {
                var keyType = forType.GetGenericArguments()[0];
                var valueType = forType.GetGenericArguments()[1];
                var fakeDictionary = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                ReadDictionary(fakeDictionary);
                return;
            }

            if (forType.IsListType())
            {
                ReadList(forType);
                return;
            }

            // Final, special, case for IEnumerable<X> if *not* a List
            // We can make this work by just acting like it *is* a List<X>
            if (forType.IsGenericEnumerable() || forType.IsGenericReadOnlyList())
            {
                var elementType = forType.GetGenericArguments()[0];
                var fakeList = typeof(List<>).MakeGenericType(elementType);
                ReadList(fakeList);
                return;
            }

            if (forType.IsEnum)
            {
                ReadEnum(forType);
                return;
            }

            if (allowRecursion && RecursiveTypes.Contains(forType))
            {
                var funcType = typeof(Func<,>).MakeGenericType(typeof(TextReader), forType);
                var funcInvoke = funcType.GetMethod("Invoke");

                LoadRecursiveTypeDelegate(forType); // Func<TextReader, memberType>
                Emit.LoadArgument(0);               // Func<TextReader, memberType> TextReader
                Emit.Call(funcInvoke);              // memberType
                return;
            }

            if (forType.IsSimpleInterface())
            {
                var interfaceImpl = typeof(InterfaceImplementation<>).MakeGenericType(forType);
                var interfaceImplType = (Type)interfaceImpl.GetField("Proxy").GetValue(null);

                ReadObject(interfaceImplType);
                return;
            }

            ReadObject(forType);
        }

        HashSet<Type> FindAndPrimeRecursiveOrReusedTypes(Type forType)
        {
            var ret = forType.FindRecursiveOrReusedTypes();
            foreach (var primeType in ret)
            {
                var loadMtd = this.RecursionLookupType.MakeGenericType(primeType).GetMethod("Load", BindingFlags.Public | BindingFlags.Static);
                loadMtd.Invoke(null, new object[0]);
            }

            return ret;
        }

        public Func<TextReader, ForType> BuildWithNewDelegate()
        {
            var forType = typeof(ForType);

            RecursiveTypes = FindAndPrimeRecursiveOrReusedTypes(forType);

            Emit = Emit.NewDynamicMethod(forType, new[] { typeof(TextReader) }, doVerify: Utils.DoVerify);

            AddGlobalVariables();

            ConsumeWhiteSpace();

            Build(forType, allowRecursion: false);

            // we have to consume this, otherwise we might succeed with invalid JSON
            ConsumeWhiteSpace();

            // We also must confirm that we read everything, again otherwise we might accept garbage as valid
            ExpectEndOfStream();

            Emit.Return();

            return Emit.CreateDelegate<Func<TextReader, ForType>>(Utils.DelegateOptimizationOptions);
        }
    }

    static class InlineDeserializerHelper
    {
        static Func<TextReader, ReturnType> BuildAlwaysFailsWith<ReturnType>(Type typeCacheType)
        {
            var specificTypeCache = typeCacheType.MakeGenericType(typeof(ReturnType));
            var stashField = specificTypeCache.GetField("ExceptionDuringBuild", BindingFlags.Static | BindingFlags.Public);

            var emit = Emit.NewDynamicMethod(typeof(ReturnType), new[] { typeof(TextReader) });
            emit.LoadConstant("Error occurred building a deserializer for " + typeof(ReturnType));
            emit.LoadField(stashField);
            emit.NewObject<DeserializationException, string, Exception>();
            emit.Throw();

            return emit.CreateDelegate<Func<TextReader, ReturnType>>(Utils.DelegateOptimizationOptions);
        }

        public static Func<TextReader, ReturnType> Build<ReturnType>(Type typeCacheType, DateTimeFormat dateFormat, out Exception exceptionDuringBuild)
        {
            var obj = new InlineDeserializer<ReturnType>(typeCacheType, dateFormat);

            Func<TextReader, ReturnType> ret;
            try
            {
                ret = obj.BuildWithNewDelegate();
                exceptionDuringBuild = null;
            }
            catch (ConstructionException e)
            {
                exceptionDuringBuild = e;
                ret = BuildAlwaysFailsWith<ReturnType>(typeCacheType);
            }

            return ret;
        }
    }
}
