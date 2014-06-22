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

        static readonly Hashtable MakeDefaultCache = new Hashtable();
        static readonly Hashtable CombineInPlaceCache = new Hashtable();

        static void LoadConstantOfType(Emit<Func<object>> emit, object val, Type type)
        {
            if (type == typeof(byte))
            {
                emit.LoadConstant((byte)val);
                return;
            }

            if (type == typeof(sbyte))
            {
                emit.LoadConstant((sbyte)val);
                return;
            }

            if (type == typeof(short))
            {
                emit.LoadConstant((short)val);
                return;
            }

            if (type == typeof(ushort))
            {
                emit.LoadConstant((ushort)val);
                return;
            }

            if (type == typeof(int))
            {
                emit.LoadConstant((int)val);
                return;
            }

            if (type == typeof(uint))
            {
                emit.LoadConstant((uint)val);
                return;
            }

            if (type == typeof(long))
            {
                emit.LoadConstant((long)val);
                return;
            }

            if (type == typeof(ulong))
            {
                emit.LoadConstant((ulong)val);
                return;
            }

            throw new ConstructionException("Unexpected type: " + type);
        }

        public static object MakeDefault(Type enumType)
        {
            var cached = (Func<object>)MakeDefaultCache[enumType];
            if (cached != null)
            {
                return cached();
            }

            var defaultVal = Activator.CreateInstance(enumType);
            var emit = Emit<Func<object>>.NewDynamicMethod(doVerify: Utils.DoVerify);
            
            LoadConstantOfType(emit, defaultVal, Enum.GetUnderlyingType(enumType));
            emit.Box(enumType);
            emit.Return();

            var newDel = emit.CreateDelegate();

            lock (MakeDefaultCache)
            {
                cached = (Func<object>)MakeDefaultCache[enumType];
                if (cached == null)
                {
                    MakeDefaultCache[enumType] = cached = newDel;
                }
            }

            return cached();
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

            lock (CombineInPlaceCache)
            {
                cached = (CombineInPlaceDelegate)CombineInPlaceCache[enumType];
                if (cached == null)
                {
                    CombineInPlaceCache[enumType] = cached = newDel;
                }
            }

            cached(a, b);
        }
    }
}
