using Jil.Common;
using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    delegate EnumType EnumThunkReaderDelegate<EnumType>(ref ThunkReader reader) 
        where EnumType : struct;

    class EnumLookup<EnumType>
        where EnumType : struct
    {
        private static Func<TextReader, EnumType> _getEnumValue;
        private static EnumThunkReaderDelegate<EnumType> _getEnumValueThunkReader;

        static EnumLookup()
        {
            var enumValues = GetEnumValues();

            _getEnumValue =
                typeof(EnumType).IsFlagsEnum()
                    ? CreateFindFlagsEnum(enumValues)
                    : CreateFindEnum(enumValues);

            _getEnumValueThunkReader =
                typeof(EnumType).IsFlagsEnum()
                    ? CreateFindFlagsEnumThunkReader(enumValues)
                    : CreateFindEnumThunkReader(enumValues);
        }

        private static IReadOnlyList<Tuple<string, object>> GetEnumValues()
        {
            var names = Enum.GetNames(typeof(EnumType));
            var ret = new List<Tuple<string, object>>();

            foreach(var name in names)
            {
                object val = Enum.Parse(typeof(EnumType), name);

                var field = typeof(EnumType).GetField(name);
                var enumMember = field.GetCustomAttribute<System.Runtime.Serialization.EnumMemberAttribute>();
                if(enumMember != null)
                {
                    ret.Add(Tuple.Create(enumMember.Value, val));
                }
                else
                {
                    ret.Add(Tuple.Create(name, val));
                }
            }

            return ret.AsReadOnly();
        }

        private static Func<TextReader, EnumType> CreateFindEnum(IEnumerable<Tuple<string, object>> names)
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(EnumType));

            var nameToResults =
                names
                .Select(name => NameAutomata<EnumType>.CreateName(typeof(TextReader), name.Item1, emit => LoadConstantOfType(emit, name.Item2, underlyingType)))
                .ToList();
            
            var ret =
                NameAutomata<EnumType>.Create<Func<TextReader, EnumType>>(
                    typeof(TextReader),
                    nameToResults,
                    false,
                    defaultValue: null
                );

            return (Func<TextReader, EnumType>)ret;
        }

        private static EnumThunkReaderDelegate<EnumType> CreateFindEnumThunkReader(IEnumerable<Tuple<string, object>> names)
        {
            var thunkReaderRef = typeof(ThunkReader).MakeByRefType();

            var underlyingType = Enum.GetUnderlyingType(typeof(EnumType));

            var nameToResults =
                names
                .Select(name => NameAutomata<EnumType>.CreateName(thunkReaderRef, name.Item1, emit => LoadConstantOfType(emit, name.Item2, underlyingType)))
                .ToList();

            var ret =
                NameAutomata<EnumType>.Create<EnumThunkReaderDelegate<EnumType>>(
                    thunkReaderRef,
                    nameToResults,
                    false,
                    defaultValue: null
                );

            return (EnumThunkReaderDelegate<EnumType>)ret;
        }

        private static Func<TextReader, EnumType> CreateFindFlagsEnum(IReadOnlyList<Tuple<string, object>> names)
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(EnumType));

            var resultValue = "result_value";

            var nameToResults =
                names
                .Select(name =>
                    NameAutomata<EnumType>.CreateName(
                        typeof(TextReader),
                        name.Item1,
                        emit =>
                        {
                            LoadConstantOfType(emit, name.Item2, underlyingType);
                            emit.LoadLocal(resultValue);
                            emit.Or();
                            emit.StoreLocal(resultValue);
                        }))
                .ToList();


            var ret =
                NameAutomata<EnumType>.CreateFold<Func<TextReader, EnumType>>(
                    typeof(TextReader),
                    nameToResults,
                    emit =>
                    {
                        emit.DeclareLocal(underlyingType, resultValue);
                        LoadConstantOfType(emit, 0, underlyingType);
                        emit.StoreLocal(resultValue);
                    },
                    emit =>
                    {
                        emit.LoadLocal(resultValue);
                        emit.Return();
                    },
                    true,
                    true,
                    false,
                    defaultValue: null
                );

            return (Func<TextReader, EnumType>)ret;
        }

        private static EnumThunkReaderDelegate<EnumType> CreateFindFlagsEnumThunkReader(IReadOnlyList<Tuple<string, object>> names)
        {
            var thunkReaderRef = typeof(ThunkReader).MakeByRefType();

            var underlyingType = Enum.GetUnderlyingType(typeof(EnumType));

            var resultValue = "result_value";

            var nameToResults =
                names
                .Select(name =>
                    NameAutomata<EnumType>.CreateName(
                        thunkReaderRef,
                        name.Item1,
                        emit =>
                        {
                            LoadConstantOfType(emit, name.Item2, underlyingType);
                            emit.LoadLocal(resultValue);
                            emit.Or();
                            emit.StoreLocal(resultValue);
                        }))
                .ToList();


            var ret =
                NameAutomata<EnumType>.CreateFold<EnumThunkReaderDelegate<EnumType>>(
                    thunkReaderRef,
                    nameToResults,
                    emit =>
                    {
                        emit.DeclareLocal(underlyingType, resultValue);
                        LoadConstantOfType(emit, 0, underlyingType);
                        emit.StoreLocal(resultValue);
                    },
                    emit =>
                    {
                        emit.LoadLocal(resultValue);
                        emit.Return();
                    },
                    true,
                    true,
                    false,
                    defaultValue: null
                );

            return (EnumThunkReaderDelegate<EnumType>)ret;
        }

        static void LoadConstantOfType(Emit Emit, object val, Type type)
        {
            dynamic dynamicVal = (dynamic) val;

            if (type == typeof(byte))
            {
                Emit.LoadConstant((byte)dynamicVal);
            }
            else if (type == typeof(sbyte))
            {
                Emit.LoadConstant((sbyte)dynamicVal);
            }
            else if (type == typeof(short))
            {
                Emit.LoadConstant((short)dynamicVal);
            }
            else if (type == typeof(ushort))
            {
                Emit.LoadConstant((ushort)dynamicVal);
            }
            else if (type == typeof(int))
            {
                Emit.LoadConstant((int)dynamicVal);
            }
            else if (type == typeof(uint))
            {
                Emit.LoadConstant((uint)val);
            }
            else if (type == typeof(long))
            {
                Emit.LoadConstant((long)dynamicVal);
            }
            else if (type == typeof(ulong))
            {
                Emit.LoadConstant((ulong)dynamicVal);
            }
            else
            {
                throw new ConstructionException("Unexpected type: " + type);
            }
        }

        public static EnumType GetEnumValue(TextReader reader)
        {
            return _getEnumValue(reader);
        }

        public static EnumType GetEnumValueThunkReader(ref ThunkReader reader)
        {
            return _getEnumValueThunkReader(ref reader);
        }
    }
}
