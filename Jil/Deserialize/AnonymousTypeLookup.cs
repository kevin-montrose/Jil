using Jil.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    class AnonymousTypeLookup<ForType>
    {
        public static Dictionary<string, Tuple<Type, int>> ParametersToTypeAndIndex;
        private static Func<TextReader, int> _findConstructorParameterIndex;

        // Still used in ReadAnonymousObjectDictionaryLookup (can be removed if NameAutomata method is always used)
        public static Dictionary<string, int> Lookup;
 
        static AnonymousTypeLookup()
        {
            ParametersToTypeAndIndex = Utils.GetAnonymousNameToConstructorMap(typeof(ForType));

            var orderedNames =
                ParametersToTypeAndIndex
                .OrderBy(kv => kv.Value.Item2)
                .Select(kv => kv.Key);

            _findConstructorParameterIndex = CreateFindMember(orderedNames);

            // Still used in ReadAnonymousObjectDictionaryLookup (can be removed if NameAutomata method is always used)
            Lookup = ParametersToTypeAndIndex.ToDictionary(d => d.Key, d => d.Value.Item2);
        }

        private static Func<TextReader, int> CreateFindMember(IEnumerable<string> names)
        {
            var nameToResults =
                names
                .Select((name, index) => NameAutomata<int>.CreateName<TextReader>(name, emit => emit.LoadConstant(index)))
                .ToList();

            return NameAutomata<int>.Create<TextReader>(nameToResults, true, defaultValue: -1);
        }

        // probably not the best place for this; but sufficent I guess...
        public static int FindConstructorParameterIndex(TextReader reader)
        {
            return _findConstructorParameterIndex(reader);
        }

    }
}
