using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sigil;
using System.Reflection;

namespace Jil.Serialize
{
    class SerializeDynamicThunk<T>
    {
        public static Action<object, TextWriter, Options> Thunk;

        static SerializeDynamicThunk()
        {
            var t = typeof(T);
            var serialize = typeof(JSON).GetMethod("Serialize", BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(t);

            var emit = Emit<Action<object, TextWriter, Options>>.NewDynamicMethod();
            
            emit.LoadArgument(0);       // obj
            if (t.IsValueType)
            {
                emit.UnboxAny<T>();     // T
            }
            else
            {
                emit.CastClass(t);      // T
            }

            emit.LoadArgument(1);
            emit.LoadArgument(2);

            emit.Call(serialize);
            emit.Return();

            Thunk = emit.CreateDelegate();
        }
    }
}
