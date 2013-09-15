using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    class CompiledSerializer
    {
        public static Action<object, Stream> Compile(StateMachine machine)
        {
            var stringConstants = Utils.ExtractStringConstants(machine);

            throw new NotImplementedException();
        }
    }
}
