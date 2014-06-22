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
            {
                var emit = Emit<Func<TEnum, TEnum, TEnum>>.NewDynamicMethod(doVerify: Utils.DoVerify);
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
        delegate void CombineInPlaceDelegate(object newFlag, object accumFlag);

        static readonly Hashtable CombineCache = new Hashtable();
        static readonly Hashtable CombineInPlaceCache = new Hashtable();

        public static object Combine(Type enumType, object a, object b)
        {
            var cached = (Func<object, object, object>)CombineCache[enumType];
            if (cached != null)
            {
                return cached(a, b);
            }

            var emit = Emit<Func<object, object, object>>.NewDynamicMethod(doVerify: Utils.DoVerify);
            emit.LoadArgument(0);       // object
            emit.UnboxAny(enumType);    // enum
            emit.LoadArgument(1);       // enum object
            emit.UnboxAny(enumType);    // enum enum
            emit.Or();                  // enum
            emit.Box(enumType);         // object
            emit.Return();              // --empty--

            var newDel = emit.CreateDelegate(Utils.DelegateOptimizationOptions);

            lock (CombineCache)
            {
                cached = (Func<object, object, object>)CombineCache[enumType];
                if (cached == null)
                {
                    CombineCache[enumType] = cached = newDel;
                }
            }

            return cached(a, b);
        }

        public static void CombineInPlace(Type enumType, object a, object b)
        {
            var cached = (CombineInPlaceDelegate)CombineInPlaceCache[enumType];
            if (cached != null)
            {
                cached(a, b);
                return;
            }

            var emit = Emit<CombineInPlaceDelegate>.NewDynamicMethod(doVerify: Utils.DoVerify);
            emit.LoadArgument(1);               // object
            emit.Unbox(enumType);               // enum&
            emit.LoadArgument(0);               // enum& object
            emit.UnboxAny(enumType);            // enum& enum
            emit.LoadArgument(1);               // enum& enum object
            emit.UnboxAny(enumType);            // enum& enum enum
            emit.Or();                          // enum& enum
            emit.StoreObject(enumType);         // --empty--
            emit.Return();

            var newDel = emit.CreateDelegate(Utils.DelegateOptimizationOptions);

            lock (CombineCache)
            {
                cached = (CombineInPlaceDelegate)CombineInPlaceCache[enumType];
                if (cached == null)
                {
                    CombineInPlaceCache[enumType] = cached = newDel;
                }
            }

            cached(a, b);
            return;
        }
    }
}
