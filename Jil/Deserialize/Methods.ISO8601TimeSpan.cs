using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static partial class Methods
    {
        static readonly ulong MinTicks = (ulong)(-TimeSpan.MinValue.Ticks);
        static readonly ulong MaxTicks = (ulong)TimeSpan.MaxValue.Ticks;
        public static readonly MethodInfo ReadISO8601TimeSpan = typeof(Methods).GetMethod("_ReadISO8601TimeSpan", BindingFlags.NonPublic | BindingFlags.Static);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TimeSpan _ReadISO8601TimeSpan(TextReader reader, char[] str)
        {
            const ulong TicksPerDay = 864000000000;
            const ulong TicksPerHour = 36000000000;
            const ulong TicksPerMinute = 600000000;
            const ulong TicksPerSecond = 10000000;

            // Format goes like so:
            // - (-)P(([n]Y)([n]M)([n]D))(T([n]H)([n]M)([n]S))
            // - P[n]W

            var len = ReadTimeSpanInto(reader, str);

            if (len == 0)
            {
                throw new DeserializationException("Unexpected empty string", reader);
            }

            var ix = 0;
            var isNegative = false;

            var c = str[ix];
            if (c == '-')
            {
                isNegative = true;
                ix++;
            }

            if (ix >= len)
            {
                throw new DeserializationException("Expected P, instead TimeSpan string ended", reader);
            }

            c = str[ix];
            if (c != 'P')
            {
                throw new DeserializationException("Expected P, found " + c, reader);
            }

            ix++;   // skip 'P'

            double year, month, week, day;
            var hasTimePart = ISO8601TimeSpan_ReadDatePart(reader, str, len, ref ix, out year, out month, out week, out day);

            if (week != -1 && (year != -1 || month != -1 || day != -1))
            {
                throw new DeserializationException("Week part of TimeSpan defined along with one or more of year, month, or day", reader);
            }

            if (week != -1 && hasTimePart)
            {
                throw new DeserializationException("TimeSpans with a week defined cannot also have a time defined", reader);
            }

            if (year == -1) year = 0;
            if (month == -1) month = 0;
            if (week == -1) week = 0;
            if (day == -1) day = 0;

            double hour, minute, second;

            if (hasTimePart)
            {
                ix++;   // skip 'T'
                ISO8601TimeSpan_ReadTimePart(reader, str, len, ref ix, out hour, out minute, out second);
            }
            else
            {
                hour = minute = second = 0;
            }

            if (year != 0) throw new NotImplementedException();
            if (month != 0) throw new NotImplementedException();

            var ticks = (ulong)(day * TicksPerDay + hour * TicksPerHour + minute * TicksPerMinute + second * TicksPerSecond);

            TimeSpan ret;

            if (ticks >= MaxTicks && !isNegative)
            {
                return TimeSpan.MaxValue;
            }

            if (ticks >= MinTicks && isNegative)
            {
                return TimeSpan.MinValue;
            }

            ret = new TimeSpan((long)ticks);
            if (isNegative)
            {
                ret = ret.Negate();
            }

            return ret;
        }

        static int ReadTimeSpanInto(TextReader reader, char[] buffer)
        {
            var i = reader.Peek();
            if (i != '"') throw new DeserializationException("Expected \", found " + (char)i, reader);
            
            reader.Read();  // skip the opening '"'

            var ix = 0;
            while (true)
            {
                if (ix >= CharBufferSize) throw new DeserializationException("ISO8601 duration too long", reader);

                i = reader.Read();
                if (i == -1) throw new DeserializationException("Unexpected end of stream", reader);
                if (i == '"')
                {
                    break;
                }

                buffer[ix] = (char)i;
                ix++;
            }

            return ix;
        }

        static bool ISO8601TimeSpan_ReadDatePart(TextReader reader, char[] str, int strLen, ref int ix, out double year, out double month, out double week, out double day)
        {
            year = month = week = day = -1;

            bool yearSeen, monthSeen, weekSeen, daySeen;
            yearSeen = monthSeen = weekSeen = daySeen = false;

            var fracSeen = false;

            while (ix != strLen)
            {
                if (str[ix] == 'T')
                {
                    return true;
                }

                if (fracSeen)
                {
                    throw new DeserializationException("Expected Time part of TimeSpan to start", reader);
                }

                int whole, fraction, fracLen;
                var part = ISO8601TimeSpan_ReadPart(reader, str, strLen, ref ix, out whole, out fraction, out fracLen);

                if (fracLen != 0)
                {
                    fracSeen = true;
                }

                if (part == 'Y')
                {
                    if (yearSeen)
                    {
                        throw new DeserializationException("Year part of TimeSpan seen twice", reader);
                    }

                    if (monthSeen)
                    {
                        throw new DeserializationException("Year part of TimeSpan seen after month already parsed", reader);
                    }

                    if (daySeen)
                    {
                        throw new DeserializationException("Year part of TimeSpan seen after day already parsed", reader);
                    }

                    year = ISO8601TimeSpan_ToDouble(reader, whole, fraction, fracLen);
                    yearSeen = true;
                    continue;
                }

                if (part == 'M')
                {
                    if (monthSeen)
                    {
                        throw new DeserializationException("Month part of TimeSpan seen twice", reader);
                    }

                    if (daySeen)
                    {
                        throw new DeserializationException("Month part of TimeSpan seen after day already parsed", reader);
                    }

                    month = ISO8601TimeSpan_ToDouble(reader, whole, fraction, fracLen);
                    monthSeen = true;
                    continue;
                }

                if (part == 'W')
                {
                    if (weekSeen)
                    {
                        throw new DeserializationException("Week part of TimeSpan seen twice", reader);
                    }

                    week = ISO8601TimeSpan_ToDouble(reader, whole, fraction, fracLen);
                    weekSeen = true;
                    continue;
                }

                if (part == 'D')
                {
                    if (daySeen)
                    {
                        throw new DeserializationException("Day part of TimeSpan seen twice", reader);
                    }

                    day = ISO8601TimeSpan_ToDouble(reader, whole, fraction, fracLen);
                    daySeen = true;
                    continue;
                }

                throw new DeserializationException("Expected Y, M, W, or D but found: " + part, reader);
            }

            return false;
        }

        private static void ISO8601TimeSpan_ReadTimePart(TextReader reader, char[] str, int strLen, ref int ix, out double hour, out double minutes, out double seconds)
        {
            hour = minutes = seconds = 0;

            bool hourSeen, minutesSeen, secondsSeen;
            hourSeen = minutesSeen = secondsSeen = false;

            var fracSeen = false;

            while (ix != strLen)
            {
                if (fracSeen)
                {
                    throw new DeserializationException("Expected Time part of TimeSpan to end", reader);
                }

                int whole, fraction, fracLen;
                var part = ISO8601TimeSpan_ReadPart(reader, str, strLen, ref ix, out whole, out fraction, out fracLen);

                if (fracLen != 0)
                {
                    fracSeen = true;
                }

                if (part == 'H')
                {
                    if (hourSeen)
                    {
                        throw new DeserializationException("Hour part of TimeSpan seen twice", reader);
                    }

                    if (minutesSeen)
                    {
                        throw new DeserializationException("Hour part of TimeSpan seen after minutes already parsed", reader);
                    }

                    if (secondsSeen)
                    {
                        throw new DeserializationException("Hour part of TimeSpan seen after seconds already parsed", reader);
                    }

                    hour = ISO8601TimeSpan_ToDouble(reader, whole, fraction, fracLen);
                    hourSeen = true;
                    continue;
                }

                if (part == 'M')
                {
                    if (minutesSeen)
                    {
                        throw new DeserializationException("Minute part of TimeSpan seen twice", reader);
                    }

                    if (secondsSeen)
                    {
                        throw new DeserializationException("Minute part of TimeSpan seen after seconds already parsed", reader);
                    }

                    minutes = ISO8601TimeSpan_ToDouble(reader, whole, fraction, fracLen);
                    minutesSeen = true;
                    continue;
                }

                if (part == 'S')
                {
                    if (secondsSeen)
                    {
                        throw new DeserializationException("Seconds part of TimeSpan seen twice", reader);
                    }

                    seconds = ISO8601TimeSpan_ToDouble(reader, whole, fraction, fracLen);
                    secondsSeen = true;
                    continue;
                }

                throw new DeserializationException("Expected H, M, or S but found: " + part, reader);
            }
        }

        static double ISO8601TimeSpan_ToDouble(TextReader reader, int whole, int fraction, int fracLen)
        {
            double ret = whole;
            double frac = fraction;

            if (fracLen > 0)
            {
                frac /= DivideFractionBy[fracLen - 1];
            }

            ret += frac;

            return ret;
        }

        static char ISO8601TimeSpan_ReadPart(TextReader reader, char[] str, int strLen, ref int ix, out int whole, out int fraction, out int fracLen)
        {
            var part = 0;
            while (true)
            {
                var c = str[ix];

                if (c == '.' || c == ',')
                {
                    whole = part;
                    break;
                }

                ix++;
                if (c < '0' || c > '9' || ix == strLen)
                {
                    whole = part;
                    fraction = 0;
                    fracLen = 0;
                    return c;
                }

                part *= 10;
                part += (c - '0');
            }

            var ixOfPeriod = ix;

            ix++;   // skip the '.' or ','
            part = 0;
            while (true)
            {
                var c = str[ix];

                ix++;
                if (c < '0' || c > '9' || ix == strLen)
                {
                    fraction = part;
                    fracLen = (ix - 1) - (ixOfPeriod + 1);
                    return c;
                }

                part *= 10;
                part += (c - '0');
            }
        }
    }
}
