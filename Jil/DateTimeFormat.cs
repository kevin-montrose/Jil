using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// Specifies the format of DateTimes and TimeSpans produced or expected by Jil.
    /// </summary>
    public enum DateTimeFormat : byte
    {
        /// <summary>
        /// DateTimes will be formatted as "\/Date(##...##)\/" where ##...## is the 
        /// number of milliseconds since the unix epoch (January 1st, 1970 UTC).
        /// 
        /// TimeSpans will be formatted as "days.hours:minutes:seconds.fractionalSeconds"
        /// </summary>
        NewtonsoftStyleMillisecondsSinceUnixEpoch = 0,
        /// <summary>
        /// DateTimes will be formatted as ##...##, where ##...## is the number
        /// of milliseconds since the unix epoch (January 1st, 1970 UTC).
        /// 
        /// This is format can be passed directly to JavaZcript's Date constructor.
        /// 
        /// TimeSpans will be formatted as ##...##, where ##...## is the TotalMilliseconds
        /// property of the TimeSpan.
        /// </summary>
        MillisecondsSinceUnixEpoch = 1,
        /// <summary>
        /// DateTimes will be formatted as ##...##, where ##...## is the number
        /// of seconds since the unix epoch (January 1st, 1970 UTC).
        /// 
        /// TimeSpans will be formatted as ##...##, where ##...## is the TotalSeconds
        /// property of the TimeSpan.
        /// </summary>
        SecondsSinceUnixEpoch = 2,
        /// <summary>
        /// DateTimes will be formatted as "yyyy-MM-ddThh:mm:ssZ" where
        /// yyyy is the year, MM is the month (starting at 01), dd is the day (starting at 01),
        /// hh is the hour (starting at 00, continuing to 24), mm is the minute (start at 00),
        /// and ss is the second (starting at 00).
        /// 
        /// Examples:
        ///     2011-07-14T19:43:37Z
        ///     2012-01-02T03:04:05Z
        ///     
        /// TimeSpans will be formatted as ISO8601 durations.
        /// 
        /// Examples:
        ///     P123DT11H30M2.3S
        /// </summary>
        ISO8601 = 3
    }
}
