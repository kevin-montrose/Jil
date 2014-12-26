using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    sealed partial class JsonObject
    {
        public Dictionary<string, JsonObject>.KeyCollection GetMemberNames()
        {
            return this.ObjectMembers.Keys;
        }

        public JsonObject GetMember(string memberName)
        {
            return this.ObjectMembers[memberName];
        }

        public bool TryCastBool(out bool bit)
        {
            if (Type == JsonObjectType.True)
            {
                bit = true;
                return true;
            }

            if(Type == JsonObjectType.False)
            {
                bit = false;
                return true;
            }

            bit = default(bool);
            return false;
        }

        public bool TryCastInteger(out ulong number, out bool negative)
        {
            if (Type == JsonObjectType.FastNumber && FastNumberIsInteger())
            {
                negative = this.FastNumberNegative;
                number = this.FastNumberPart1;

                return true;
            }

            if (Type == JsonObjectType.Number)
            {
                negative = this.NumberValue < 0;
                number = (ulong)Math.Abs(this.NumberValue);

                return true;
            }

            number = default(ulong);
            negative = default(bool);
            return false;
        }

        public bool TryCastFloatingPoint(out double floatingPoint)
        {
            if (Type == JsonObjectType.FastNumber)
            {
                return FastNumberToDouble(out floatingPoint);
            }

            if (Type == JsonObjectType.Number)
            {
                floatingPoint = NumberValue;
                return true;
            }

            floatingPoint = default(double);
            return false;
        }

        public bool TryCastDateTime(out DateTime dt)
        {
            if (Type == JsonObjectType.FastNumber)
            {
                long res;
                var ret = FastNumberToLong(out res);

                if (!ret)
                {
                    dt = default(DateTime);
                    return false;
                }

                switch (Options.UseDateTimeFormat)
                {
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        dt = Methods.UnixEpoch + TimeSpan.FromMilliseconds(res);
                        return true;
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        dt = Methods.UnixEpoch + TimeSpan.FromSeconds(res);
                        return true;
                }
            }

            if (Type == JsonObjectType.Number)
            {
                long res = (long)NumberValue;

                switch (Options.UseDateTimeFormat)
                {
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        dt = Methods.UnixEpoch + TimeSpan.FromMilliseconds(res);
                        return true;
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        dt = Methods.UnixEpoch + TimeSpan.FromSeconds(res);
                        return true;
                }
            }

            dt = default(DateTime);
            return false;
        }

        public bool TryCastTimeSpan(out TimeSpan ts)
        {
            // TODO: Implement!
            ts = TimeSpan.MinValue;
            return false;
        }

        public bool TryCastGuid(out Guid g)
        {
            if (Type != JsonObjectType.String)
            {
                g = default(Guid);
                return false;
            }

            return Guid.TryParseExact(StringValue, "D", out g);
        }

        public bool TryCastString(out string str)
        {
            if (Type != JsonObjectType.String)
            {
                str = null;
                return false;
            }

            str = StringValue;
            return true;
        }

        public bool TryConvertEnumerable(out IEnumerable enumerable)
        {
            if (Type == JsonObjectType.Array)
            {
                enumerable = EnumerableArrayWrapper.MakeAsIEnumerable(ArrayValue);
                return true;
            }

            if (Type == JsonObjectType.Object)
            {
                enumerable = EnumerableObjectWrapper.MakeAsIEnumerable(ObjectMembers);
                return true;
            }

            enumerable = null;
            return false;
        }

        public bool IsDictionary()
        {
            return Type == JsonObjectType.Object;
        }
    }
}
