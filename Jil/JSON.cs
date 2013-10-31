using Jil.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// Fast JSON serializer.
    /// </summary>
    public sealed class JSON
    {
        /// <summary>
        /// Serializes the given data to the provider TextWriter.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted, Options.Default is used.
        /// </summary>
        public static void Serialize<T>(T data, TextWriter output, Options options = null)
        {
            options = options ?? Options.Default;

            switch (options.UseDateTimeFormat)
            {
                case DateTimeFormat.ISO8601:
                    if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                    {
                        ISO8601PrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldExcludeNulls)
                    {
                        ISO8601ExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldPrettyPrint)
                    {
                        ISO8601PrettyPrintTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    ISO8601TypeCache<T>.Thunk(output, data, 0);
                    return;

                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                    {
                        MillisecondsPrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldExcludeNulls)
                    {
                        MillisecondsExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldPrettyPrint)
                    {
                        MillisecondsPrettyPrintTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    MillisecondsTypeCache<T>.Thunk(output, data, 0);
                    return;

                case DateTimeFormat.SecondsSinceUnixEpoch:
                    if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                    {
                        SecondsPrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldExcludeNulls)
                    {
                        SecondsExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldPrettyPrint)
                    {
                        SecondsPrettyPrintTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    SecondsTypeCache<T>.Thunk(output, data, 0);
                    return;

                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                    if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                    {
                        NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldExcludeNulls)
                    {
                        NewtonsoftStyleExcludeNullsTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    if (options.ShouldPrettyPrint)
                    {
                        NewtonsoftStylePrettyPrintTypeCache<T>.Thunk(output, data, 0);
                        return;
                    }

                    NewtonSoftStyleTypeCache<T>.Thunk(output, data, 0);
                    return;

                default: throw new InvalidOperationException("Unexpected Options: " + options);
            }
        }
    }
}
