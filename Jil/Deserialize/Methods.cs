using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    static partial class Methods
    {
        public const int CharBufferSize = 29;

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct GuidStruct
        {
            [FieldOffset(0)]
            public readonly Guid Value;

            [FieldOffset(0)]
            public byte B00;
            [FieldOffset(1)]
            public byte B01;
            [FieldOffset(2)]
            public byte B02;
            [FieldOffset(3)]
            public byte B03;
            [FieldOffset(4)]
            public byte B04;
            [FieldOffset(5)]
            public byte B05;

            [FieldOffset(6)]
            public byte B06;
            [FieldOffset(7)]
            public byte B07;
            [FieldOffset(8)]
            public byte B08;
            [FieldOffset(9)]
            public byte B09;

            [FieldOffset(10)]
            public byte B10;
            [FieldOffset(11)]
            public byte B11;

            [FieldOffset(12)]
            public byte B12;
            [FieldOffset(13)]
            public byte B13;
            [FieldOffset(14)]
            public byte B14;
            [FieldOffset(15)]
            public byte B15;
        }

        public static readonly MethodInfo ReadISO8601Date = typeof(Methods).GetMethod("_ReadISO8601Date", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static DateTime _ReadISO8601Date(TextReader reader, char[] buffer)
        {
            // ISO8601 is a plague
            //   See: http://en.wikipedia.org/wiki/ISO_8601 &
            //        http://tools.ietf.org/html/rfc3339

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

                if (c == 'T')
                {
                    if (tPos.HasValue) throw new DeserializationException("Unexpected second T in ISO8601 date");
                    tPos = ix - 1;
                }

                if (tPos.HasValue)
                {
                    if (c == 'Z' || c == '+' || c == '-')
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
            if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
            year += (c - '0');
            year *= 10;
            start++;
            c = buffer[start];
            if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
            year += (c - '0');
            year *= 10;
            start++;
            c = buffer[start];
            if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
            year += (c - '0');
            year *= 10;
            start++;
            c = buffer[start];
            if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
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
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        month *= 10;
                        start++;
                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        if (month == 0 || month > 12) throw new DeserializationException("Expected month to be between 01 and 12");

                        return new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Local);

                    case 8:
                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        day *= 10;
                        start++;
                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        day *= 10;
                        start++;
                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        if (day > 366) throw new DeserializationException("Expected ordinal day to be between 000 and 366");

                        return (new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Local)).AddDays(day);

                    case 10:
                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        month *= 10;
                        start++;
                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        month += (c - '0');
                        if (month == 0 || month > 12) throw new DeserializationException("Expected month to be between 01 and 12");
                        start++;

                        if (buffer[start] != '-') throw new DeserializationException("Expected -");
                        start++;

                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                        day += (c - '0');
                        day *= 10;
                        start++;
                        c = buffer[start];
                        if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
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
                    if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    day *= 10;
                    start++;
                    c = buffer[start];
                    if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    day *= 10;
                    start++;
                    c = buffer[start];
                    if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    if (day > 366) throw new DeserializationException("Expected ordinal day to be between 000 and 366");
                    start++;

                    return (new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Local)).AddDays(day);
                
                case 8:
                    c = buffer[start];
                    if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                    month += (c - '0');
                    month *= 10;
                    start++;
                    c = buffer[start];
                    if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                    month += (c - '0');
                    if (month == 0 || month > 12) throw new DeserializationException("Expected month to be between 01 and 12");
                    start++;
                    
                    c = buffer[start];
                    if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
                    day += (c - '0');
                    day *= 10;
                    start++;
                    c = buffer[start];
                    if(c <'0' || c >'9') throw new DeserializationException("Expected digit");
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
            const double HoursToMilliseconds = 3600000;
            const double MinutesToMilliseconds = 60000;
            const double SecondsToMilliseconds = 1000;

            // Here are the possible formats for times
            // hh
            // hh,fff*
            // hh.fff*
            //
            // hhmmss
            // hhmm
            // hhmm,fff*
            // hhmm.fff*
            // hhmmss.fff*
            // hhmmss,fff*
            // hh:mm
            // hh:mm:ss
            // hh:mm,fff*
            // hh:mm:ss,fff*
            // hh:mm.fff*
            // hh:mm:ss.fff*
            
            var len = (stop - start) + 1;
            if (len < 2) throw new DeserializationException("ISO8601 must begin with a 2 character hour");

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
                // hh:mm,fff*
                // hh:mm:ss,fff*
                // hh:mm.fff*
                // hh:mm:ss.fff*

                if (len < 4) throw new DeserializationException("Expected hour part of ISO8601 time");

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
                // hhmm,fff*
                // hhmm.fff*
                // hhmmss.fff*
                // hhmmss,fff*

                if (len < 4) throw new DeserializationException("Expected hour part of ISO8601 time");

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
            if (c == 'Z')
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

        public static readonly MethodInfo ReadGuid = typeof(Methods).GetMethod("_ReadGuid", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Guid _ReadGuid(TextReader reader)
        {
            // 1314FAD4-7505-439D-ABD2-DBD89242928C
            // 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
            //
            // Guid is guaranteed to be a 36 character string

            // Bytes are in a different order than you might expect
            // For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0 
            // Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0 
            // Ix:   0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
            //
            // And we have to account for dashes
            //
            // So the map is like so:
            // chars[0]  -> bytes[3]  -> buffer[ 0, 1]
            // chars[1]  -> bytes[2]  -> buffer[ 2, 3]
            // chars[2]  -> bytes[1]  -> buffer[ 4, 5]
            // chars[3]  -> bytes[0]  -> buffer[ 6, 7]
            // chars[4]  -> bytes[5]  -> buffer[ 9,10]
            // chars[5]  -> bytes[4]  -> buffer[11,12]
            // chars[6]  -> bytes[7]  -> buffer[14,15]
            // chars[7]  -> bytes[6]  -> buffer[16,17]
            // chars[8]  -> bytes[8]  -> buffer[19,20]
            // chars[9]  -> bytes[9]  -> buffer[21,22]
            // chars[10] -> bytes[10] -> buffer[24,25]
            // chars[11] -> bytes[11] -> buffer[26,27]
            // chars[12] -> bytes[12] -> buffer[28,29]
            // chars[13] -> bytes[13] -> buffer[30,31]
            // chars[14] -> bytes[14] -> buffer[32,33]
            // chars[15] -> bytes[15] -> buffer[34,35]

            var asStruct = new GuidStruct();
            asStruct.B03 = ReadGuidByte(reader);
            asStruct.B02 = ReadGuidByte(reader);
            asStruct.B01 = ReadGuidByte(reader);
            asStruct.B00 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -");
            
            asStruct.B05 = ReadGuidByte(reader);
            asStruct.B04 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -");

            asStruct.B07 = ReadGuidByte(reader);
            asStruct.B06 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -");

            asStruct.B08 = ReadGuidByte(reader);
            asStruct.B09 = ReadGuidByte(reader);

            if (reader.Read() != '-') throw new DeserializationException("Expected -");

            asStruct.B10 = ReadGuidByte(reader);
            asStruct.B11 = ReadGuidByte(reader);
            asStruct.B12 = ReadGuidByte(reader);
            asStruct.B13 = ReadGuidByte(reader);
            asStruct.B14 = ReadGuidByte(reader);
            asStruct.B15 = ReadGuidByte(reader);

            return asStruct.Value;
        }

        static byte ReadGuidByte(TextReader reader)
        {
            var a = reader.Read();
            if (a == -1) throw new DeserializationException("Expected any character");
            if (!((a >= '0' && a <= '9') || (a >= 'A' && a <= 'F') || (a >= 'a' && a <= 'f'))) throw new DeserializationException("Expected a hex number");
            var b = reader.Read();
            if (b == -1) throw new DeserializationException("Expected any character");
            if (!((b >= '0' && b <= '9') || (b >= 'A' && b <= 'F') || (b >= 'a' && b <= 'f'))) throw new DeserializationException("Expected a hex number");

            if (a <= '9')
            {
                a -= '0';
            }
            else
            {
                if (a <= 'F')
                {
                    a -= ('A' - 10);
                }
                else
                {
                    a -= ('a' - 10);
                }
            }

            if (b <= '9')
            {
                b -= '0';
            }
            else
            {
                if (b <= 'F')
                {
                    b -= ('A' - 10);
                }
                else
                {
                    b -= ('a' - 10);
                }
            }

            return (byte)(a * 16 + b);
        }

        public static readonly MethodInfo Skip = typeof(Methods).GetMethod("_Skip", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _Skip(TextReader reader)
        {
            var leadChar = reader.Peek();

            // skip null
            if (leadChar == 'n')
            {
                reader.Read();
                if (reader.Read() != 'u') throw new DeserializationException("Expected u");
                if (reader.Read() != 'l') throw new DeserializationException("Expected u");
                if (reader.Read() != 'l') throw new DeserializationException("Expected u");
                return;
            }

            // skip a string
            if(leadChar == '"')
            {
                SkipEncodedString(reader);
                return;
            }

            // skip an object
            if (leadChar == '{')
            {
                SkipObject(reader);
                return;
            }

            // skip a list
            if (leadChar == '[')
            {
                SkipList(reader);
                return;
            }

            // skip a number
            if ((leadChar >= '0' && leadChar <= '9') || leadChar == '-')
            {
                SkipNumber(reader);
                return;
            }

            throw new DeserializationException("Expected digit, -, \", {, or [");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipObject(TextReader reader)
        {
            reader.Read();  // skip the {

            _ConsumeWhiteSpace(reader);
            var a = reader.Peek();
            if (a == '}')
            {
                reader.Read();
                return;
            }
            SkipEncodedString(reader);
            _ConsumeWhiteSpace(reader);
            var b = reader.Read();
            if (b != ':') throw new DeserializationException("Expected :");
            _ConsumeWhiteSpace(reader);
            _Skip(reader);

            while (true)
            {
                _ConsumeWhiteSpace(reader);
                var c = reader.Read();
                if (c == '}') return;
                if (c != ',') throw new DeserializationException("Expected ,");

                _ConsumeWhiteSpace(reader);
                SkipEncodedString(reader);
                _ConsumeWhiteSpace(reader);
                var d = reader.Read();
                if (d != ':') throw new DeserializationException("Expected :");
                _ConsumeWhiteSpace(reader);
                _Skip(reader);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipList(TextReader reader)
        {
            reader.Read();  // skip the [

            _ConsumeWhiteSpace(reader);
            var a = reader.Peek();
            if (a == ']')
            {
                reader.Read();
                return;
            }
            _ConsumeWhiteSpace(reader);
            _Skip(reader);

            while (true)
            {
                _ConsumeWhiteSpace(reader);
                var b = reader.Read();
                if (b == ']') return;
                if (b != ',') throw new DeserializationException("Expected ], or ,");
                _ConsumeWhiteSpace(reader);
                _Skip(reader);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipEncodedString(TextReader reader)
        {
            reader.Read();  // skip the "

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character");

                // we didn't have to use anything but the buffer, make a string and return it!
                if (first == '"')
                {
                    return;
                }

                if (first != '\\')
                {
                    continue;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character");

                switch (second)
                {
                    case '"': continue;
                    case '\\': continue;
                    case '/': continue;
                    case 'b': continue;
                    case 'f': continue;
                    case 'n': continue;
                    case 'r': continue;
                    case 't': continue;
                }

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence");

                // now we're in an escape sequence, we expect 4 hex #s; always
                var u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') && (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit");
                u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') && (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit");
                u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') && (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit");
                u = reader.Read();
                if (!((u >= '0' && u <= '9') || (u >= 'A' && u <= 'F') && (u >= 'a' && u <= 'f'))) throw new DeserializationException("Expected hex digit");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SkipNumber(TextReader reader)
        {
            reader.Read();  // ditch the start of the number

            var seenDecimal = false;
            var seenExponent = false;

            while (true)
            {
                var c = reader.Peek();

                if (c >= '0' && c <= '9')
                {
                    reader.Read();  // skip the digit
                    continue;
                }

                if (c == '.' && !seenDecimal)
                {
                    reader.Read();      // skip the decimal
                    seenDecimal = true;
                    continue;
                }

                if ((c == 'e' || c == 'E') && !seenExponent)
                {
                    reader.Read();      // skip the decimal
                    seenExponent = true;
                    seenDecimal = true;

                    var next = reader.Peek();
                    if (next == '-' || next == '+' || (next >= '0' && next <= '9'))
                    {
                        reader.Read();
                        continue;
                    }

                    throw new DeserializationException("Expected -, or a digit");
                }

                return;
            }
        }

        public static readonly MethodInfo ConsumeWhiteSpace = typeof(Methods).GetMethod("_ConsumeWhiteSpace", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void _ConsumeWhiteSpace(TextReader reader)
        {
            int c;
            while ((c = reader.Peek()) != -1)
            {
                if (!IsWhiteSpace(c)) return;

                reader.Read();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsWhiteSpace(int c)
        {
            // per http://www.ietf.org/rfc/rfc4627.txt
            // insignificant whitespace in JSON is defined as 
            //  \u0020  - space
            //  \u0009  - tab
            //  \u000A  - new line
            //  \u000D  - carriage return

            return
                c == 0x20 ||
                c == 0x09 ||
                c == 0x0A ||
                c == 0x0D;
        }

        public static readonly MethodInfo ReadEncodedString = typeof(Methods).GetMethod("_ReadEncodedString", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string _ReadEncodedString(TextReader reader, char[] buffer, StringBuilder commonSb)
        {
            {
                var ix = 0;

                while (ix <= CharBufferSize)
                {
                    if (ix == CharBufferSize)
                    {
                        commonSb.Append(new string(buffer, 0, ix));
                        break;
                    }

                    var first = reader.Read();
                    if (first == -1) throw new DeserializationException("Expected any character");

                    // we didn't have to use anything but the buffer, make a string and return it!
                    if (first == '"')
                    {
                        // avoid an allocation here
                        if (ix == 0) return "";

                        return new string(buffer, 0, ix);
                    }

                    if (first != '\\')
                    {
                        buffer[ix] = (char)first;
                        ix++;
                        continue;
                    }

                    var second = reader.Read();
                    if (second == -1) throw new DeserializationException("Expected any character");

                    switch (second)
                    {
                        case '"': buffer[ix] = '"'; ix++; continue;
                        case '\\': buffer[ix] = '\\'; ix++; continue;
                        case '/': buffer[ix] = '/'; ix++; continue;
                        case 'b': buffer[ix] = '\b'; ix++; continue;
                        case 'f': buffer[ix] = '\f'; ix++; continue;
                        case 'n': buffer[ix] = '\n'; ix++; continue;
                        case 'r': buffer[ix] = '\r'; ix++; continue;
                        case 't': buffer[ix] = '\t'; ix++; continue;
                    }

                    if (second != 'u') throw new DeserializationException("Unrecognized escape sequence");

                    commonSb.Append(buffer, 0, ix);

                    // now we're in an escape sequence, we expect 4 hex #s; always
                    ix = 0;
                    var read = 0;
                    var toRead = 4;
                    do
                    {
                        read = reader.Read(buffer, ix, toRead);
                        if (read == 0) throw new DeserializationException("Expected characters");

                        toRead -= read;
                        ix += read;
                    } while (toRead > 0);

                    var c = FastHexToInt(buffer);
                    commonSb.Append((char)c);
                    break;
                }
            }

            // fall through to using a StringBuilder

            while (true)
            {
                var first = reader.Read();
                if (first == -1) throw new DeserializationException("Expected any character");

                if (first == '"')
                {
                    break;
                }

                if (first != '\\')
                {
                    commonSb.Append((char)first);
                    continue;
                }

                var second = reader.Read();
                if (second == -1) throw new DeserializationException("Expected any character");

                switch (second)
                {
                    case '"': commonSb.Append('"'); continue;
                    case '\\': commonSb.Append('\\'); continue;
                    case '/': commonSb.Append('/'); continue;
                    case 'b': commonSb.Append('\b'); continue;
                    case 'f': commonSb.Append('\f'); continue;
                    case 'n': commonSb.Append('\n'); continue;
                    case 'r': commonSb.Append('\r'); continue;
                    case 't': commonSb.Append('\t'); continue;
                }

                if (second != 'u') throw new DeserializationException("Unrecognized escape sequence");

                // now we're in an escape sequence, we expect 4 hex #s; always
                var ix = 0;
                var read = 0;
                var toRead = 4;
                do
                {
                    read = reader.Read(buffer, ix, toRead);
                    if (read == 0) throw new DeserializationException("Expected characters");

                    toRead -= read;
                    ix += read;
                } while (toRead > 0);

                var asInt = FastHexToInt(buffer);
                commonSb.Append((char)asInt);
            }

            var ret = commonSb.ToString();

            // leave this clean for the next use
            commonSb.Clear();

            return ret;
        }

        public static readonly MethodInfo ReadEncodedChar = typeof(Methods).GetMethod("_ReadEncodedChar", BindingFlags.Static | BindingFlags.NonPublic);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char _ReadEncodedChar(TextReader reader, char[] buffer)
        {
            var first = reader.Read();
            if (first == -1) throw new DeserializationException("Expected any character");

            // TODO: high/low surrogates, do we need to worry about those?
            if (first != '\\') return (char)first;

            var second = reader.Read();
            if (second == -1) throw new DeserializationException("Expected any character");

            switch (second)
            {
                case '"': return '"';
                case '\\': return '\\';
                case '/': return '/';
                case 'b': return '\b';
                case 'f': return '\f';
                case 'n': return '\n';
                case 'r': return '\r';
                case 't': return '\t';
            }

            if (second != 'u') throw new DeserializationException("Unrecognized escape sequence");

            // now we're in an escape sequence, we expect 4 hex #s; always
            var ix = 0;
            var read = 0;
            var toRead = 4;
            do
            {
                read = reader.Read(buffer, ix, toRead);
                if (read == 0) throw new DeserializationException("Expected characters");

                toRead -= read;
                ix += read;
            } while (toRead > 0);


            return (char)FastHexToInt(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int FastHexToInt(char[] buffer)
        {
            var ret = 0;

            //char1:
            {
                const int ix = 0;

                var c = (int)buffer[ix];

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    goto char2;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char2;
                }

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char2;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }

            char2:
            ret *= 16;
            {
                const int ix = 1;

                var c = (int)buffer[ix];

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    goto char3;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char3;
                }

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char3;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }

            char3:
            ret *= 16;
            {
                const int ix = 2;

                var c = (int)buffer[ix];

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    goto char4;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char4;
                }

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    goto char4;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }

            char4:
            ret *= 16;
            {
                const int ix = 3;

                var c = (int)buffer[ix];

                c -= '0';
                if (c >= 0 && c <= 9)
                {
                    ret += c;
                    return ret;
                }

                c -= ('A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    return ret;
                }

                c -= ('f' - 'A' - '0');
                if (c >= 0 && c <= 5)
                {
                    ret += 10 + c;
                    return ret;
                }

                throw new Exception("Expected hex digit, found: " + buffer[ix]);
            }
        }
    }
}
