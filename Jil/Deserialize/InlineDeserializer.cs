using System;
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

        void ExpectQuote()
        {
            var gotQuote = Emit.DefineLabel();

            RawReadChar(() => ThrowExpected('"'));  // int
            Emit.LoadConstant((int)'"');            // int "
            Emit.BranchIfEqual(gotQuote);           // --empty--
            ThrowExpected('"');                     // --empty--

            Emit.MarkLabel(gotQuote);               // --empty--
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
            ExpectQuote();                          // --empty--
            Emit.LoadArgument(0);                   // TextReader
            LoadCharBuffer();                       // TextReader char[]
            LoadStringBuilder();                    // TextReader char[] StringBuilder
            Emit.Call(Methods.ReadEncodedString);   // string
        }

        void ReadNumber(Type numberType)
        {
            throw new NotImplementedException();
        }

        void ReadPrimitive(Type primitiveType)
        {
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

        Func<TextReader, int, ForType> BuildPrimitiveWithNewDelegate()
        {
            Emit = Emit.NewDynamicMethod(typeof(ForType), new[] { typeof(TextReader), typeof(int) });

            AddGlobalVariables();

            ReadPrimitive(typeof(ForType));

            Emit.Return();

            return Emit.CreateDelegate<Func<TextReader, int, ForType>>();
        }

        Func<TextReader, int, ForType> BuildDictionaryWithNewDelegate()
        {
            throw new NotImplementedException();
        }

        Func<TextReader, int, ForType> BuildObjectWithNewDelegate()
        {
            throw new NotImplementedException();
        }

        Func<TextReader, int, ForType> BuildListWithNewDelegate()
        {
            throw new NotImplementedException();
        }

        Func<TextReader, int, ForType> BuildEnumWithNewDelegate()
        {
            throw new NotImplementedException();
        }

        Func<TextReader, int, ForType> BuildNullableWithNewDelegate()
        {
            throw new NotImplementedException();
        }

        public Func<TextReader, int, ForType> Build()
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

            if (forType.IsEnum)
            {
                return BuildEnumWithNewDelegate();
            }

            return BuildObjectWithNewDelegate();
        }
    }

    static class InlineDeserializerHelper
    {
        public static Func<TextReader, int, ReturnType> Build<ReturnType>(Type typeCacheType)
        {
            var obj = new InlineDeserializer<ReturnType>(typeCacheType);

            var ret = obj.Build();

            return ret;
        }
    }
}
