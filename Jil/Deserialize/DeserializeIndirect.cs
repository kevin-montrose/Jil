using Jil.Common;
using Sigil.NonGeneric;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static class DeserializeIndirect
    {
        static Hashtable DeserializeIndirectCache = new Hashtable();

        static MethodInfo JSONDeserialize = typeof(JSON).GetMethod("Deserialize", new[] { typeof(TextReader), typeof(Options) });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Deserialize(TextReader reader, Type type, Options options)
        {
            var cached = (Func<TextReader, Options, object>)DeserializeIndirectCache[type];
            if (cached == null)
            {
                lock(DeserializeIndirectCache)
                {
                    cached = (Func<TextReader, Options, object>)DeserializeIndirectCache[type];
                    if (cached == null)
                    {
                        var emit = Emit.NewDynamicMethod(typeof(object), new[] { typeof(TextReader), typeof(Options) }, doVerify: Utils.DoVerify);
                        var mtd = JSONDeserialize.MakeGenericMethod(type);

                        emit.LoadArgument(0);   // TextReader
                        emit.LoadArgument(1);   // TextReader Options
                        emit.Call(mtd);         // type
                        if (type.IsValueType)
                        {
                            emit.Box(type);     // object
                        }
                        emit.Return();

                        DeserializeIndirectCache[type] = cached = emit.CreateDelegate<Func<TextReader, Options, object>>(Utils.DelegateOptimizationOptions);
                    }
                }
            }

            return cached(reader, options);
        }
    }
}
