using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;
using System.IO;
using Sigil;

namespace Jil.Deserialize
{
    delegate int SetterLookupThunkReaderDelegate(ref ThunkReader reader);

    static class SetterLookup<ForType, SerializationNameFormatType>
    {
        private static readonly IReadOnlyList<Tuple<string, MemberInfo>> _nameOrderedSetters;
        private static Func<TextReader, int> _findMember;
        private static SetterLookupThunkReaderDelegate _findMemberThunkReader;

        public static Dictionary<string, int> Lookup;

        static SetterLookup()
        {
            _nameOrderedSetters = GetOrderedSetters();

            Lookup =
                _nameOrderedSetters
                .Select((setter, index) => Tuple.Create(setter.Item1, index))
                .ToDictionary(kv => kv.Item1, kv => kv.Item2);

            _findMember = CreateFindMember(_nameOrderedSetters.Select(setter => setter.Item1));
            _findMemberThunkReader = CreateFindMemberThunkReader(_nameOrderedSetters.Select(setter => setter.Item1));
        }

        private static Tuple<string, MemberInfo>[] GetOrderedSetters()
        {
            var forType = typeof(ForType);
            var flags = BindingFlags.Instance | BindingFlags.Public;
            var serializatioNameFormat = SerializationNameFormat.Verbatim;
            if (typeof (SerializationNameFormatType) == typeof (SerializationNameFormatCamelCase))
            {
                serializatioNameFormat = SerializationNameFormat.CamelCase;
            }

            var fields = forType.GetFields(flags).Where(field => field.ShouldUseMember());
            var props = forType.GetProperties(flags).Where(p => p.SetMethod != null && p.ShouldUseMember());

            return
                fields.Cast<MemberInfo>()
                .Concat(props.Cast<MemberInfo>())
                .Select(member => Tuple.Create(member.GetSerializationName(serializatioNameFormat), member))
                .OrderBy(info => info.Item1)
                .ToArray();
        }

        private static Func<TextReader, int> CreateFindMember(IEnumerable<string> names)
        {
            var nameToResults =
                names
                .Select((name, index) => NameAutomata<int>.CreateName(typeof(TextReader), name, emit => emit.LoadConstant(index)))
                .ToList();

            var ret = NameAutomata<int>.Create<Func<TextReader, int>>(typeof(TextReader), nameToResults, true, defaultValue: -1);

            return (Func<TextReader, int>)ret;
        }

        private static SetterLookupThunkReaderDelegate CreateFindMemberThunkReader(IEnumerable<string> names)
        {
            var nameToResults =
                names
                .Select((name, index) => NameAutomata<int>.CreateName(typeof(ThunkReader).MakeByRefType(), name, emit => emit.LoadConstant(index)))
                .ToList();

            var ret = NameAutomata<int>.Create<SetterLookupThunkReaderDelegate>(typeof(ThunkReader).MakeByRefType(), nameToResults, true, defaultValue: -1);

            return (SetterLookupThunkReaderDelegate)ret;
        }

        // probably not the best place for this; but sufficent I guess...
        public static int FindSetterIndex(TextReader reader)
        {
            return _findMember(reader);
        }

        public static int FindSetterIndexThunkReader(ref ThunkReader reader)
        {
            return _findMemberThunkReader(ref reader);
        }

        public static Dictionary<string, MemberInfo> GetSetters()
        {
            return _nameOrderedSetters.ToDictionary(m => m.Item1, m => m.Item2);
        }
    }
}
