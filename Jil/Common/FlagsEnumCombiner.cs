using Sigil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    class FlagsEnumCombiner<TEnum>
        where TEnum : struct
    {
        public static readonly Func<TEnum, TEnum, TEnum> Combine;

        static FlagsEnumCombiner()
        {
#if DEBUG
            var doVerify = true;
#else
            var doVerify = false;
#endif

            {
                var emit = Emit<Func<TEnum, TEnum, TEnum>>.NewDynamicMethod(doVerify: doVerify);
                emit.LoadArgument(0);       // enum
                emit.LoadArgument(1);       // enum enum
                emit.Or();                  // enum
                emit.Return();              // --empty--

                Combine = emit.CreateDelegate(Utils.DelegateOptimizationOptions);
            }
        }
    }

    class FlagsEnumCombiner
    {
        static readonly Hashtable Cache = new Hashtable();

        public static object Combine(Type enumType, object a, object b)
        {
            var cached = (Func<object, object, object>)Cache[enumType];
            if (cached != null)
            {
                return cached(a, b);
            }

#if DEBUG
            var doVerify = true;
#else
            var doVerify = false;
#endif

            var emit = Emit<Func<object, object, object>>.NewDynamicMethod(doVerify: doVerify);
            emit.LoadArgument(0);       // object
            emit.UnboxAny(enumType);    // enum
            emit.LoadArgument(1);       // enum object
            emit.UnboxAny(enumType);    // enum enum
            emit.Or();                  // enum
            emit.Box(enumType);         // object
            emit.Return();              // --empty--

            var newDel = emit.CreateDelegate(Utils.DelegateOptimizationOptions);

            lock (Cache)
            {
                cached = (Func<object, object, object>)Cache[enumType];
                if (cached == null)
                {
                    Cache[enumType] = cached = newDel;
                }
            }

            return cached(a, b);
        }
    }
}
