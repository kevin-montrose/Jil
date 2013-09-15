using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static class TypeCache<T>
    {
        public static Action<object, Stream> Serializer;

        static TypeCache()
        {
            var stateMachine = StateMachineCache<T>.StateMachine;

            Serializer = CompiledSerializer.Compile(stateMachine);
        }
    }
}
