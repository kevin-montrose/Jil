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
    partial class Methods
    {
        public static readonly MethodInfo ReadISO8601Date = typeof(Methods).GetMethod("_ReadISO8601Date", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static DateTime _ReadISO8601Date(TextReader reader, char[] buffer)
        {
            // ISO8601 / RFC3339 (the internet "profile"* of ISO8601) is a plague
            //   See: http://en.wikipedia.org/wiki/ISO_8601 &
            //        http://tools.ietf.org/html/rfc3339
            //        *is bullshit

            // Here are the possible formats for dates
            // YYYY-MM-DD
            // YYYY-MM
            // YYYY-DDD (ordinal date)
            // YYYY-Www (week date, the W is a literal)
            // YYYY-Www-D
            // YYYYMMDD
            // YYYYWww
            // YYYYWwwD
            // YYYYDDD

            // Here are the possible formats for times
            // hh
            // hh:mm
            // hhmm
            // hh:mm:ss
            // hhmmss
            // hh,fff*
            // hh:mm,fff*
            // hhmm,fff*
            // hh:mm:ss,fff*
            // hhmmss,fff*
            // hh.fff*
            // hh:mm.fff*
            // hhmm.fff*
            // hh:mm:ss.fff*
            // hhmmss.fff*
            // * arbitrarily many (technically an "agreed upon" number, I'm agreeing on 3)

            // Here are the possible formats for timezones
            // Z
            // +hh
            // +hh:mm
            // +hhmm
            // -hh
            // -hh:mm
            // -hhmm

            // they are concatenated to form a full instant, with T as a separator between date & time
            // i.e. <date>T<time><timezone>
            // the longest possible string:
            // 9999-12-31T01:23:45.678+01:23
            // 0123456789ABCDEFGHIJKLMNOPQRS
            //
            // Maximum date size is 29 characters

            var ix = -1;
            int? tPos = null;
            int? zPlusOrMinus = null;
            while (true)
            {
                var c = reader.Peek();
                if (c == -1) throw new DeserializationException("Unexpected end of stream while parsing ISO8601 date");

                if (c == '"') break;

                // actually consume that character
                reader.Read();

                ix++;
                if (ix == CharBufferSize) throw new DeserializationException("ISO8601 date is too long, expected " + CharBufferSize + " characters or less");
                buffer[ix] = (char)c;

                // RFC3339 allows lowercase t and spaces as alternatives to ISO8601's T
                if (c == 'T' || c == 't' || c == ' ')
                {
                    if (tPos.HasValue) throw new DeserializationException("Unexpected second T in ISO8601 date");
                    tPos = ix - 1;
                }

                if (tPos.HasValue)
                {
                    // RFC3339 allows lowercase z as alternatives to ISO8601's Z
                    if (c == 'Z' || c == 'z' || c == '+' || c == '-')
                    {
                        if (zPlusOrMinus.HasValue) throw new DeserializationException("Unexpected second Z, +, or - in ISO8601 date");
                        zPlusOrMinus = ix - 1;
                    }
                }
            }

            bool? hasSeparators;

            var date = ParseISO8601Date(buffer, 0, tPos ?? ix, out hasSeparators); // this is in *LOCAL TIME* because that's what the spec says
            if (!tPos.HasValue) return date;

            var time = ParseISO8601Time(buffer, tPos.Value + 2, zPlusOrMinus ?? ix, ref hasSeparators);
            if (!zPlusOrMinus.HasValue) return date + time;

            bool unknownLocalOffset;
            // only +1 here because the separator is significant (oy vey)
            var timezoneOffset = ParseISO8601TimeZoneOffset(buffer, zPlusOrMinus.Value + 1, ix, ref hasSeparators, out unknownLocalOffset);

            if (unknownLocalOffset)
            {
                return DateTime.SpecifyKind(date, DateTimeKind.Unspecified) + time;
            }

            return DateTime.SpecifyKind(date, DateTimeKind.Utc) + time + timezoneOffset;
        }

        static DateTime ParseISO8601Date(char[] buffer, int start, int stop, out bool? hasSeparators)
        {
            // Here are the possible formats for dates
            // YYYY-MM-DD
            // YYYY-MM
            // YYYY-DDD (ordinal date)
            // YYYY-Www (week date, the W is a literal)
            // YYYY-Www-D
            // YYYYMMDD
            // YYYYWww
            // YYYYWwwD
            // YYYYDDD

            // Explicitly not implementing week dates at this time
            // ^ TODO fix that!

            var len = (stop - start) + 1;
            if (len < 4) throw new DeserializationException("ISO8601 date must begin with a 4 character year");

            var year = 0;
            var month = 0;
            var day = 0;
            int c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            year += (c - '0');
            year *= 10;
            start++;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            year += (c - '0');
            year *= 10;
            start++;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            year += (c - '0');
            year *= 10;
            start++;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            year += (c - '0');

            if (year == 0) throw new DeserializationException("ISO8601 year 0000 cannot be converted to a DateTime");

            // we've reached the end
            if (start == stop)
            {
                hasSeparators = null;
                return new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Local);
            }

            start++;
            hasSeparators = buffer[start] == '-';

            if (hasSeparators.Value)
            {
                start++;

                // Could still be:
                // YYYY-MM-DD           length: 10
                // YYYY-MM              length:  7
                // YYYY-DDD             length:  8

                switch (len)
                {
                    case 7:
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        month *= 10;
                        start++;
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        if (month == 0 || month > 12) throw new DeserializationException("Expected month to be between 01 and 12");

                        return new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Local);

                    case 8:
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        day *= 10;
                        start++;
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        day *= 10;
                        start++;
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        if (day > 366) throw new DeserializationException("Expected ordinal day to be between 000 and 366");

                        return (new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Local)).AddDays(day);

                    case 10:
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        month *= 10;
                        start++;
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        if (month == 0 || month > 12) throw new DeserializationException("Expected month to be between 01 and 12");
                        start++;

                        if (buffer[start] != '-') throw new DeserializationException("Expected -");
                        start++;

                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        day *= 10;
                        start++;
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        if (day == 0 || day > 31) throw new DeserializationException("Expected day to be between 01 and 31");
                        start++;

                        return (new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local));

                    default:
                        throw new DeserializationException("Unexpected date string length");
                }
            }

            // Could still be
            // YYYYMMDD         length: 8
            // YYYYDDD          length: 7

            switch (len)
            {
                case 7:
                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    day *= 10;
                    start++;
                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    day *= 10;
                    start++;
                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    if (day > 366) throw new DeserializationException("Expected ordinal day to be between 000 and 366");
                    start++;

                    return (new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Local)).AddDays(day);

                case 8:
                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    month += (c - '0');
                    month *= 10;
                    start++;
                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    month += (c - '0');
                    if (month == 0 || month > 12) throw new DeserializationException("Expected month to be between 01 and 12");
                    start++;

                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    day *= 10;
                    start++;
                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    if (day == 0 || day > 31) throw new DeserializationException("Expected day to be between 01 and 31");
                    start++;

                    return (new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local));

                default:
                    throw new DeserializationException("Unexpected date string length");
            }
        }

        static TimeSpan ParseISO8601Time(char[] buffer, int start, int stop, ref bool? hasSeparators)
        {
            const double HoursToMilliseconds   = 3600000;
            const double MinutesToMilliseconds =   60000;
            const double SecondsToMilliseconds =    1000;

            // Here are the possible formats for times
            // hh
            // hh,fff
            // hh.fff
            //
            // hhmmss
            // hhmm
            // hhmm,fff
            // hhmm.fff
            // hhmmss.fff
            // hhmmss,fff
            // hh:mm
            // hh:mm:ss
            // hh:mm,fff
            // hh:mm:ss,fff
            // hh:mm.fff
            // hh:mm:ss.fff

            var len = (stop - start) + 1;
            if (len < 2) throw new DeserializationException("ISO8601 time must begin with a 2 character hour");

            var hour = 0;
            int c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            hour += (c - '0');
            hour *= 10;
            start++;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            hour += (c - '0');
            if (hour > 24) throw new DeserializationException("Expected hour to be between 00 and 24");

            // just an hour part
            if (start == stop)
            {
                return TimeSpan.FromHours(hour);
            }

            start++;
            c = buffer[start];

            // hour with a fractional part
            if (c == ',' || c == '.')
            {
                start++;
                var frac = 0;
                var fracLength = 0;
                while (start <= stop)
                {
                    c = buffer[start];
                    if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                    frac *= 10;
                    frac += (c - '0');

                    fracLength++;
                    start++;
                }

                if (fracLength == 0) throw new DeserializationException("Expected fractional part of ISO8601 time");

                double hoursAsMilliseconds = hour * HoursToMilliseconds;
                hoursAsMilliseconds += ((double)frac) / Math.Pow(10, fracLength) * HoursToMilliseconds;

                return TimeSpan.FromMilliseconds(hoursAsMilliseconds);
            }

            if (c == ':')
            {
                if (hasSeparators.HasValue && !hasSeparators.Value) throw new DeserializationException("Unexpected separator");

                hasSeparators = true;
                start++;
            }
            else
            {
                if (hasSeparators.HasValue && hasSeparators.Value) throw new DeserializationException("Expected :");

                hasSeparators = false;
            }

            if (hasSeparators.Value)
            {
                // Could still be
                // hh:mm
                // hh:mm:ss
                // hh:mm,fff
                // hh:mm:ss,fff
                // hh:mm.fff
                // hh:mm:ss.fff

                if (len < 4) throw new DeserializationException("Expected minute part of ISO8601 time");

                var min = 0;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                min += (c - '0');
                min *= 10;
                start++;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                min += (c - '0');
                if (min > 59) throw new DeserializationException("Expected minute to be between 00 and 59");

                // just HOUR and MINUTE part
                if (start == stop)
                {
                    return new TimeSpan(hour, min, 0);
                }

                start++;
                c = buffer[start];

                // HOUR, MINUTE, and FRACTION
                if (c == ',' || c == '.')
                {
                    start++;
                    var frac = 0;
                    var fracLength = 0;
                    while (start <= stop)
                    {
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        frac *= 10;
                        frac += (c - '0');

                        fracLength++;
                        start++;
                    }

                    if (fracLength == 0) throw new DeserializationException("Expected fractional part of ISO8601 time");

                    double hoursAsMilliseconds = hour * HoursToMilliseconds;
                    double minsAsMilliseconds = min * MinutesToMilliseconds;
                    minsAsMilliseconds += ((double)frac) / Math.Pow(10, fracLength) * MinutesToMilliseconds;

                    return TimeSpan.FromMilliseconds(hoursAsMilliseconds + minsAsMilliseconds);
                }

                if (c != ':') throw new DeserializationException("Expected :");
                start++;

                var secs = 0;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                secs += (c - '0');
                secs *= 10;
                start++;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                secs += (c - '0');

                // HOUR, MINUTE, and SECONDS
                if (start == stop)
                {
                    return new TimeSpan(hour, min, secs);
                }

                start++;
                c = buffer[start];
                if (c == ',' || c == '.')
                {
                    start++;
                    var frac = 0;
                    var fracLength = 0;
                    while (start <= stop)
                    {
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        frac *= 10;
                        frac += (c - '0');

                        fracLength++;
                        start++;
                    }

                    if (fracLength == 0) throw new DeserializationException("Expected fractional part of ISO8601 time");

                    double hoursAsMilliseconds = hour * HoursToMilliseconds;
                    double minsAsMilliseconds = min * MinutesToMilliseconds;
                    double secsAsMilliseconds = secs * SecondsToMilliseconds;
                    secsAsMilliseconds += ((double)frac) / Math.Pow(10, fracLength) * SecondsToMilliseconds;

                    return TimeSpan.FromMilliseconds(hoursAsMilliseconds + minsAsMilliseconds + secsAsMilliseconds);
                }

                throw new DeserializationException("Expected ,, or .");
            }
            else
            {
                // Could still be
                // hhmmss
                // hhmm
                // hhmm,fff
                // hhmm.fff
                // hhmmss.fff
                // hhmmss,fff

                if (len < 4) throw new DeserializationException("Expected minute part of ISO8601 time");

                var min = 0;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                min += (c - '0');
                min *= 10;
                start++;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                min += (c - '0');
                if (min > 59) throw new DeserializationException("Expected minute to be between 00 and 59");

                // just HOUR and MINUTE part
                if (start == stop)
                {
                    return new TimeSpan(hour, min, 0);
                }

                start++;
                c = buffer[start];

                // HOUR, MINUTE, and FRACTION
                if (c == ',' || c == '.')
                {
                    start++;
                    var frac = 0;
                    var fracLength = 0;
                    while (start <= stop)
                    {
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        frac *= 10;
                        frac += (c - '0');

                        fracLength++;
                        start++;
                    }

                    if (fracLength == 0) throw new DeserializationException("Expected fractional part of ISO8601 time");

                    double hoursAsMilliseconds = hour * HoursToMilliseconds;
                    double minsAsMilliseconds = min * MinutesToMilliseconds;
                    minsAsMilliseconds += ((double)frac) / Math.Pow(10, fracLength) * MinutesToMilliseconds;

                    return TimeSpan.FromMilliseconds(hoursAsMilliseconds + minsAsMilliseconds);
                }

                if (c == ':') throw new DeserializationException("Unexpected separator in ISO8601 time");

                var secs = 0;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                secs += (c - '0');
                secs *= 10;
                start++;
                c = buffer[start];
                if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                secs += (c - '0');

                // HOUR, MINUTE, and SECONDS
                if (start == stop)
                {
                    return new TimeSpan(hour, min, secs);
                }

                start++;
                c = buffer[start];
                if (c == ',' || c == '.')
                {
                    start++;
                    var frac = 0;
                    var fracLength = 0;
                    while (start <= stop)
                    {
                        c = buffer[start];
                        if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
                        frac *= 10;
                        frac += (c - '0');

                        fracLength++;
                        start++;
                    }

                    if (fracLength == 0) throw new DeserializationException("Expected fractional part of ISO8601 time");

                    double hoursAsMilliseconds = hour * HoursToMilliseconds;
                    double minsAsMilliseconds = min * MinutesToMilliseconds;
                    double secsAsMilliseconds = secs * SecondsToMilliseconds;
                    secsAsMilliseconds += ((double)frac) / Math.Pow(10, fracLength) * SecondsToMilliseconds;

                    return TimeSpan.FromMilliseconds(hoursAsMilliseconds + minsAsMilliseconds + secsAsMilliseconds);
                }

                throw new DeserializationException("Expected ,, or .");
            }
        }

        static TimeSpan ParseISO8601TimeZoneOffset(char[] buffer, int start, int stop, ref bool? hasSeparators, out bool unknownLocalOffset)
        {
            // Here are the possible formats for timezones
            // Z
            // +hh
            // +hh:mm
            // +hhmm
            // -hh
            // -hh:mm
            // -hhmm

            int c = buffer[start];
            // no need to validate, the caller has done that
            if (c == 'Z' || c == 'z')
            {
                unknownLocalOffset = false;
                return TimeSpan.Zero;
            }
            var isNegative = c == '-';
            start++;

            var len = (stop - start) + 1;

            if (len < 2) throw new DeserializationException("Expected hour part of ISO8601 timezone offset");
            var hour = 0;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            hour += (c - '0');
            hour *= 10;
            start++;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            hour += (c - '0');
            if (hour > 24) throw new DeserializationException("Expected hour offset to be between 00 and 24");

            // just an HOUR offset
            if (start == stop)
            {
                unknownLocalOffset = false;

                if (isNegative)
                {
                    return new TimeSpan(-hour, 0, 0);
                }
                else
                {
                    return new TimeSpan(hour, 0, 0);
                }
            }

            start++;
            c = buffer[start];
            if (c == ':')
            {
                if (hasSeparators.HasValue && !hasSeparators.Value) throw new DeserializationException("Unexpected separator in ISO8601 timezone offset");

                hasSeparators = true;
                start++;
            }
            else
            {
                if (hasSeparators.HasValue && hasSeparators.Value) throw new DeserializationException("Expected :");

                hasSeparators = false;
            }

            var mins = 0;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            mins += (c - '0');
            mins *= 10;
            start++;
            c = buffer[start];
            if (c < '0' || c > '9') throw new DeserializationException("Expected digit");
            mins += (c - '0');
            if (mins > 59) throw new DeserializationException("Expected minute offset to be between 00 and 59");

            if (isNegative)
            {
                // per Section 4.3 of of RFC3339 (http://tools.ietf.org/html/rfc3339)
                // a timezone of "-00:00" is used to indicate an "Unknown Local Offset"
                unknownLocalOffset = hour == 0 && mins == 0;

                return new TimeSpan(-hour, -mins, 0);
            }
            else
            {
                unknownLocalOffset = false;
                return new TimeSpan(hour, mins, 0);
            }
        }
    }
}
