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
    [Flags]
    enum ExpectedEndMarker : byte
    {
        Undefined = 0,

        EndOfStream = 1,
        Comma = 2,
        CurlyBrace = 4,
        SquareBrace = 8
    }

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

        void ThrowExpected(params object[] ps)
        {
            Emit.LoadConstant("Expected: " + string.Join(", ", ps));    // string
            Emit.NewObject<DeserializationException, string>();         // DeserializationException
            Emit.Throw();
        }

        void ExpectChar(char c)
        {
            var gotChar = Emit.DefineLabel();

            RawReadChar(() => ThrowExpected(c));  // int
            Emit.LoadConstant((int)c);            // int "
            Emit.BranchIfEqual(gotChar);           // --empty--
            ThrowExpected(c);                     // --empty--

            Emit.MarkLabel(gotChar);               // --empty--
        }

        void ExpectQuote()
        {
            ExpectChar('"');
        }

        void ExpectQuoteOrNull(Action ifQuote, Action ifNull)
        {
            var gotQuote = Emit.DefineLabel();
            var gotN = Emit.DefineLabel();
            var done = Emit.DefineLabel();

            RawReadChar(() => ThrowExpected("\"", "null")); // int
            Emit.Duplicate();                               // int int
            Emit.LoadConstant((int)'"');                    // int int "
            Emit.BranchIfEqual(gotQuote);                   // int 
            Emit.LoadConstant((int)'n');                    // int n
            Emit.BranchIfEqual(gotN);                       // --empty--
            ThrowExpected("\"", "null");                    // --empty--

            Emit.MarkLabel(gotQuote);                       // int
            Emit.Pop();                                     // --empty--
            ifQuote();                                      // ???
            Emit.Branch(done);                              // ???

            Emit.MarkLabel(gotN);                           // --empty--
            ExpectChar('u');                                // --empty--
            ExpectChar('l');                                // --empty--
            ExpectChar('l');                                // --empty--
            ifNull();                                       // ???

            Emit.MarkLabel(done);                           // --empty--
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

        static MethodInfo GetReadUInt8(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadUInt8TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadInt8(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadInt8TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadInt16(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadInt16TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadUInt16(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadUInt16TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadInt32(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadInt32TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadUInt32(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadUInt32TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadInt64(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadInt64TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadUInt64(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadUInt64TillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        static MethodInfo GetReadFloatingPoint(ExpectedEndMarker marker)
        {
            switch (marker)
            {
                case ExpectedEndMarker.EndOfStream: return Methods.ReadFloatingPointTillEnd;
                default: throw new Exception("Unexpected ExpectedEndMarker: " + marker);
            }
        }

        void ReadNumber(Type numberType, ExpectedEndMarker end)
        {
            Emit.LoadArgument(0);

            if (numberType == typeof(byte))
            {
                Emit.Call(GetReadUInt8(end));
                return;
            }

            if (numberType == typeof(sbyte))
            {
                Emit.Call(GetReadInt8(end));
                return;
            }

            if (numberType == typeof(short))
            {
                Emit.Call(GetReadInt16(end));
                return;
            }

            if (numberType == typeof(ushort))
            {
                Emit.Call(GetReadUInt16(end));
                return;
            }

            if (numberType == typeof(int))
            {
                Emit.Call(GetReadInt32(end));
                return;
            }

            if (numberType == typeof(uint))
            {
                Emit.Call(GetReadUInt32(end));
                return;
            }

            if (numberType == typeof(long))
            {
                Emit.Call(GetReadInt64(end));
                return;
            }

            if (numberType == typeof(ulong))
            {
                Emit.Call(GetReadUInt64(end));
                return;
            }

            Emit.Call(GetReadFloatingPoint(end));

            if (numberType == typeof(float))
            {
                Emit.Convert<float>();
                return;
            }

            if (numberType == typeof(decimal))
            {
                Emit.NewObject<decimal, double>();
                return;
            }
        }

        void ReadPrimitive(Type primitiveType, ExpectedEndMarker end)
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

            ReadNumber(primitiveType, end);
        }

        Func<TextReader, int, ForType> BuildPrimitiveWithNewDelegate()
        {
            Emit = Emit.NewDynamicMethod(typeof(ForType), new[] { typeof(TextReader), typeof(int) });

            AddGlobalVariables();

            ReadPrimitive(typeof(ForType), ExpectedEndMarker.EndOfStream);

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
