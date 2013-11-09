using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    abstract class SerializingAction
    {
        public SerializingAction() { }
    }

    class WriteStringAction : SerializingAction
    {
        public string String { get; private set; }

        private WriteStringAction() { }

        public static WriteStringAction For(string str)
        {
            return new WriteStringAction { String = str };
        }
    }

    class ListStartAction : SerializingAction { }
    class ListEndAction : SerializingAction { }

    class DictionaryStartAction : SerializingAction { }
    class DictionaryEndAction : SerializingAction { }

    class WriteEncodedStringAction : SerializingAction { }
    class WriteCharAction : SerializingAction { }
    class WriteIntAction : SerializingAction { }
    class WriteBoolAction : SerializingAction { }
    class WriteGuidAction : SerializingAction { }
}
