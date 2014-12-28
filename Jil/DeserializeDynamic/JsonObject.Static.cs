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

            if (Type == JsonObjectType.String)
            {
                DateTime res;
                bool ret;

                switch (Options.UseDateTimeFormat)
                {
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                        ret = Methods.ReadNewtonsoftStyleDateTime(StringValue, out res);
                        dt = res;
                        return ret;
                    case DateTimeFormat.ISO8601:
                        ret = Methods.ReadISO8601DateTime(StringValue, out res);
                        dt = res;
                        return ret;
                }
            }

            dt = default(DateTime);
            return false;
        }

        public bool TryCastTimeSpan(out TimeSpan ts)
        {
            if (Type == JsonObjectType.FastNumber)
            {
                const double TicksPerMillisecond = 10000;
                const double TicksPerSecond = 10000000;

                double res;
                var ret = FastNumberToDouble(out res);
                if (!ret)
                {
                    ts = default(TimeSpan);
                    return false;
                }
                switch (Options.UseDateTimeFormat)
                {
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        var msTicksDouble = res * TicksPerMillisecond;
                        var msTicks = (long)msTicksDouble;

                        if (msTicksDouble >= TimeSpan.MaxValue.Ticks)
                        {
                            msTicks = TimeSpan.MaxValue.Ticks;
                        }

                        if (msTicksDouble <= TimeSpan.MinValue.Ticks)
                        {
                            msTicks = TimeSpan.MinValue.Ticks;
                        }

                        ts = new TimeSpan(msTicks);
                        return true;
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        var sTicksDouble = res * TicksPerSecond;
                        var sTicks = (long)sTicksDouble;

                        if (sTicksDouble >= TimeSpan.MaxValue.Ticks)
                        {
                            sTicks = TimeSpan.MaxValue.Ticks;
                        }

                        if (sTicksDouble <= TimeSpan.MinValue.Ticks)
                        {
                            sTicks = TimeSpan.MinValue.Ticks;
                        }

                        ts = new TimeSpan(sTicks);
                        return true;
                }
            }

            if (Type == JsonObjectType.Number)
            {
                const double TicksPerMillisecond = 10000;
                const double TicksPerSecond = 10000000;

                var res = NumberValue;
                switch (Options.UseDateTimeFormat)
                {
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        var msTicksDouble = res * TicksPerMillisecond;
                        var msTicks = (long)msTicksDouble;

                        if (msTicksDouble >= TimeSpan.MaxValue.Ticks)
                        {
                            msTicks = TimeSpan.MaxValue.Ticks;
                        }

                        if (msTicksDouble <= TimeSpan.MinValue.Ticks)
                        {
                            msTicks = TimeSpan.MinValue.Ticks;
                        }

                        ts = new TimeSpan(msTicks);
                        return true;
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        var sTicksDouble = res * TicksPerSecond;
                        var sTicks = (long)sTicksDouble;

                        if (sTicksDouble >= TimeSpan.MaxValue.Ticks)
                        {
                            sTicks = TimeSpan.MaxValue.Ticks;
                        }

                        if (sTicksDouble <= TimeSpan.MinValue.Ticks)
                        {
                            sTicks = TimeSpan.MinValue.Ticks;
                        }

                        ts = new TimeSpan(sTicks);
                        return true;
                }
            }

            if(Type == JsonObjectType.String)
            {
                bool ret;

                switch (Options.UseDateTimeFormat)
                {
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                        ret = Methods.ReadNewtonsoftStyleTimeSpan(StringValue, out ts);
                        return ret;
                    case DateTimeFormat.ISO8601:
                        ret = Methods.ReadISO8601TimeSpan(StringValue, out ts);
                        return ret;
                }
            }

            ts = default(TimeSpan);
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
