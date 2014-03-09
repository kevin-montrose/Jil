using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;

namespace Jil.DeserializeDynamic
{
    sealed partial class JsonObject
    {
        public override bool TryGetIndex(System.Dynamic.GetIndexBinder binder, object[] indexes, out object result)
        {
            if (Type == JsonObjectType.Array)
            {
                if (indexes.Length != 1)
                {
                    result = null;
                    return false;
                }

                var indexRaw = indexes[0];
                if (!(indexRaw is int))
                {
                    result = null;
                    return false;
                }

                var ix = (int)indexRaw;
                if (ix < 0 || ix >= ArrayValue.Count)
                {
                    result = null;
                    return false;
                }

                var val = ArrayValue[ix];
                return val.InnerTryConvert(binder.ReturnType, out result);
            }

            if (Type == JsonObjectType.Object)
            {
                if (indexes.Length != 1)
                {
                    result = null;
                    return false;
                }

                var key = indexes[0] as string;
                if (key == null)
                {
                    result = null;
                    return false;
                }

                JsonObject rawValue;
                if (!ObjectMembers.TryGetValue(key, out rawValue))
                {
                    result = null;
                    return false;
                }

                return rawValue.InnerTryConvert(binder.ReturnType, out result);
            }

            result = null;
            return false;
        }

        static readonly IEnumerable<string> ArrayMembers = new[] { "Length", "Count" };

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            if (Type == JsonObjectType.Object)
            {
                return ObjectMembers.Keys;
            }

            if (Type == JsonObjectType.Array)
            {
                return ArrayMembers;
            }

            return Enumerable.Empty<string>();
        }

        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            if (Type == JsonObjectType.Array)
            {
                if (binder.Name == "Count" || binder.Name == "Length")
                {
                    result = ArrayValue.Count;
                    return true;
                }

                result = null;
                return false;
            }

            if (Type != JsonObjectType.Object)
            {
                result = null;
                return false;
            }
            
            JsonObject val;
            if (!ObjectMembers.TryGetValue(binder.Name, out val))
            {
                result = null;
                return false;
            }

            return val.InnerTryConvert(binder.ReturnType, out result);
        }

        bool InnerTryConvert(Type returnType, out object result)
        {
            if (returnType == typeof(object))
            {
                result = this;
                return true;
            }

            switch (Type)
            {
                case JsonObjectType.False:
                    result = false;
                    return returnType == typeof(bool);
                case JsonObjectType.True:
                    result = true;
                    return returnType == typeof(bool);
                case JsonObjectType.FastNumber:
                    if (returnType == typeof(double))
                    {
                        double res;
                        var ret = FastNumberToDouble(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(float))
                    {
                        float res;
                        var ret = FastNumberToFloat(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(decimal))
                    {
                        decimal res;
                        var ret = FastNumberToDecimal(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(byte))
                    {
                        byte res;
                        var ret = FastNumberToByte(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(sbyte))
                    {
                        sbyte res;
                        var ret = FastNumberToSByte(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(short))
                    {
                        short res;
                        var ret = FastNumberToShort(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(ushort))
                    {
                        ushort res;
                        var ret = FastNumberToUShort(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(int))
                    {
                        int res;
                        var ret = FastNumberToInt(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(uint))
                    {
                        uint res;
                        var ret = FastNumberToUInt(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(long))
                    {
                        long res;
                        var ret = FastNumberToLong(out res);
                        result = res;
                        return ret;
                    }
                    if (returnType == typeof(ulong))
                    {
                        ulong res;
                        var ret = FastNumberToULong(out res);
                        result = res;
                        return ret;
                    }
                    break;
                case JsonObjectType.Number:
                    if (returnType == typeof(double))
                    {
                        result = NumberValue;
                        return true;
                    }
                    if (returnType == typeof(float))
                    {
                        result = (float)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(decimal))
                    {
                        result = (decimal)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(byte))
                    {
                        result = (byte)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(sbyte))
                    {
                        result = (sbyte)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(short))
                    {
                        result = (short)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(ushort))
                    {
                        result = (ushort)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(int))
                    {
                        result = (int)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(uint))
                    {
                        result = (uint)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(long))
                    {
                        result = (long)NumberValue;
                        return true;
                    }
                    if (returnType == typeof(ulong))
                    {
                        result = (ulong)NumberValue;
                        return true;
                    }
                    break;
                case JsonObjectType.String:
                    if (returnType == typeof(string))
                    {
                        result = StringValue;
                        return true;
                    }
                    if (returnType.IsEnum)
                    {
                        result = Enum.Parse(returnType, StringValue, ignoreCase: true);
                        return true;
                    }
                    if (returnType == typeof(Guid))
                    {
                        Guid guid;
                        if (!Guid.TryParseExact(StringValue, "D", out guid))
                        {
                            result = null;
                            return false;
                        }

                        result = guid;
                        return true;
                    }
                    break;
                case JsonObjectType.Object:
                    if (returnType == typeof(System.Collections.IEnumerable))
                    {
                        result = ObjectMembers;
                        return true;
                    }

                    if (returnType.IsGenericDictionary())
                    {
                        var args = returnType.GetGenericArguments();
                        var keyType = args[0];
                        var valType = args[1];

                        var stringKeys = keyType == typeof(string);
                        var enumKeys = keyType.IsEnum;

                        // only strings and enums can be keys
                        if (!(stringKeys || enumKeys))
                        {
                            result = null;
                            return false;
                        }

                        var coerced = new Dictionary<object, object>(ObjectMembers.Count);
                        foreach (var kv in ObjectMembers)
                        {
                            object innerResult;
                            if (!kv.Value.InnerTryConvert(valType, out innerResult))
                            {
                                result = null;
                                return false;
                            }

                            if (stringKeys)
                            {
                                coerced[kv.Key] = innerResult;
                            }
                            else
                            {
                                object @enum = Enum.Parse(keyType, kv.Key, ignoreCase: true);
                                coerced[@enum] = innerResult;
                            }
                        }

                        if (stringKeys)
                        {
                            result = Utils.ProjectStringDictionary(coerced, valType);
                        }
                        else
                        {
                            // enum keys
                            result = Utils.ProjectEnumDictionary(coerced, keyType, valType);
                        }

                        return true;
                    }
                    break;
                case JsonObjectType.Array:
                    if (returnType == typeof(System.Collections.IEnumerable))
                    {
                        result = ArrayValue;
                        return true;
                    }

                    if (returnType.IsGenericEnumerable())
                    {
                        var castTo = returnType.GetGenericArguments()[0];

                        var dynamicProjection =
                            ArrayValue.Select(
                                val =>
                                {
                                    object innerResult;
                                    if (!val.InnerTryConvert(castTo, out innerResult))
                                    {
                                        throw new Microsoft.CSharp.RuntimeBinder.RuntimeBinderException("Cannot convert " + val.GetType().FullName + " to " + castTo.FullName);
                                    }

                                    return innerResult;
                                }
                            );

                        result = Utils.DynamicProject(dynamicProjection, castTo);
                        return true;
                    }
                    break;
            }

            result = null;
            return false;
        }

        public override bool TryConvert(System.Dynamic.ConvertBinder binder, out object result)
        {
            return this.InnerTryConvert(binder.ReturnType, out result);
        }

        public override bool TryInvokeMember(System.Dynamic.InvokeMemberBinder binder, object[] args, out object result)
        {
            if (Type == JsonObjectType.Object)
            {
                if (binder.Name == "GetEnumerator" && args.Length == 0)
                {
                    result = ObjectMembers.GetEnumerator();
                    return true;
                }

                if(binder.Name == "ContainsKey" && args.Length == 1)
                {
                    var key = args[0] as string;
                    if(key == null)
                    {
                        result = null;
                        return false;
                    }

                    result = ObjectMembers.ContainsKey(key);
                    return true;
                }

                result = null;
                return false;
            }

            if (Type == JsonObjectType.Array)
            {
                if (binder.Name == "GetEnumerator" && args.Length == 0)
                {
                    result = ArrayMembers.GetEnumerator();
                    return true;
                }

                result = null;
                return false;
            }

            result = null;
            return false;
        }
    }
}
