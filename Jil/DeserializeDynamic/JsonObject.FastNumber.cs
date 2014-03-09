using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    sealed partial class JsonObject
    {
        bool FastNumberToDouble(out double result)
        {
            double ret = FastNumberPart1;
            if (FastNumberPart2 != 0)
            {
                double frac = (ret == 0 ? 1 : Math.Sign(ret)) * FastNumberPart2;
                var divideBy = Math.Pow(10, FastNumberPart2Length);
                frac /= divideBy;

                ret += frac;
            }

            if (FastNumberPart3 != 0)
            {
                double power = FastNumberPart3;
                if (FastNumberPart4 != 0)
                {
                    double frac = (power == 0 ? 1 : Math.Sign(power)) * FastNumberPart4;
                    var divideBy = Math.Pow(10, FastNumberPart4Length);
                    frac /= divideBy;

                    power += frac;
                }

                ret *= Math.Pow(10, power);
            }

            result = ret;
            return true;
        }

        bool FastNumberToFloat(out float result)
        {
            double res;
            var ret = FastNumberToDouble(out res);
            result = (float)res;
            return ret;
        }

        bool FastNumberToDecimal(out decimal result)
        {
            double res;
            var ret = FastNumberToDouble(out res);
            result = (decimal)res;
            return ret;
        }

        bool FastNumberToByte(out byte result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < byte.MinValue || res > byte.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (byte)res;
            return true;
        }

        bool FastNumberToSByte(out sbyte result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < sbyte.MinValue || res > sbyte.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (sbyte)res;
            return true;
        }

        bool FastNumberToShort(out short result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < short.MinValue || res > short.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (short)res;
            return true;
        }

        bool FastNumberToUShort(out ushort result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < ushort.MinValue || res > ushort.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (ushort)res;
            return true;
        }

        bool FastNumberToInt(out int result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < int.MinValue || res > int.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (int)res;
            return true;
        }

        bool FastNumberToUInt(out uint result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < uint.MinValue || res > uint.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (uint)res;
            return true;
        }

        bool FastNumberToLong(out long result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < long.MinValue || res > long.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (long)res;
            return true;
        }

        bool FastNumberToULong(out ulong result)
        {
            double res;
            if (!FastNumberToDouble(out res))
            {
                result = 0;
                return false;
            }

            if (res < ulong.MinValue || res > ulong.MaxValue)
            {
                result = 0;
                return false;
            }

            result = (ulong)res;
            return true;
        }
    }
}
