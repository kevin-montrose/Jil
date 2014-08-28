using Jil.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    class EnumLookup<EnumType>
        where EnumType : struct
    {
        private static Func<TextReader, EnumType> _getEnumValue;

        static EnumLookup()
        {
            var enumValues = GetEnumValues();

            _getEnumValue = CreateFindEnum(enumValues);
        }

        private static IReadOnlyList<Tuple<string, object>> GetEnumValues()
        {
            return 
                Enum.GetValues(typeof(EnumType))
                .Cast<object>()
                .Select(enumVal => Tuple.Create(typeof(EnumType).GetEnumValueName(enumVal), enumVal))
                .ToList()
                .AsReadOnly();
        }

        private static Func<TextReader, EnumType> CreateFindEnum(IEnumerable<Tuple<string, object>> names)
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(EnumType));

            var nameToResults =
                names
                .Select(name => NameAutomata<EnumType>.CreateName(name.Item1, emit => LoadConstantOfType(emit, name.Item2, underlyingType)))
                .ToList();

            return NameAutomata<EnumType>.Create(nameToResults, emit => emit.LoadConstant(-1), false);
        }

        static void LoadConstantOfType(Sigil.Emit<Func<TextReader, EnumType>> Emit, object val, Type type)
        {
            if (type == typeof(byte)) {
                Emit.LoadConstant((byte)val);
            } else if (type == typeof(sbyte)) {
                Emit.LoadConstant((sbyte)val);
            } else if (type == typeof(short)) {
                Emit.LoadConstant((short)val);
            } else if (type == typeof(ushort)) {
                Emit.LoadConstant((ushort)val);
            } else if (type == typeof(int)) {
                Emit.LoadConstant((int)val);
            } else if (type == typeof(uint)) {
                Emit.LoadConstant((uint)val);
            } else if (type == typeof(long)) {
                Emit.LoadConstant((long)val);
            } else if (type == typeof(ulong)) {
                Emit.LoadConstant((ulong)val);
            } else {
                throw new ConstructionException("Unexpected type: " + type);
            }
        }
        
        public static EnumType GetEnumValue(TextReader reader)
        {
            return _getEnumValue(reader);
        }
    }
}
