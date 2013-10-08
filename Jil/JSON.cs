using Jil.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    public sealed class JSON
    {
        public static void Serialize<T>(T data, TextWriter output, Options options = null)
        {
            options = options ?? Options.Default;

            if (options.UseDateTimeFormat.HasValue)
            {
                switch (options.UseDateTimeFormat.Value)
                {
                    case DateTimeFormat.ISO8601: ISO8601Serialize(data, output, options); return;
                    case DateTimeFormat.MillisecondsSinceUnixEpoch: MillisecondsSerialize(data, output, options); return;
                    case DateTimeFormat.SecondsSinceUnixEpoch: SecondsSerialize(data, output, options); return;
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch: break;
                    default: throw new InvalidOperationException("Unexpected DateTimeFormat: " + options.UseDateTimeFormat);
                }
            }

            if (options.ShouldPrettyPrint.GetValueOrDefault())
            {
                NewtonsoftStylePrettyPrintSerialize(data, output, options);
                return;
            }

            if (options.ShouldExcludeNulls.GetValueOrDefault())
            {
                NewtonsoftStyleExcludeNullsSerialize(data, output, options);
                return;
            }

            NewtonSoftStyleTypeCache<T>.Thunk(output, data, 0);
        }

        private static void SecondsSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                SecondsSerializePrettyPrintSerialize(data, output, opts);
                return;
            }

            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                SecondsSerializeExcludeNullsSerialize(data, output, opts);
                return;
            }

            SecondsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void SecondsSerializePrettyPrintSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                SecondsPrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            SecondsPrettyPrintTypeCache<T>.Thunk(output, data, 0);
        }

        private static void SecondsSerializeExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                SecondsPrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            SecondsExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void SecondsPrettyPrintExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            SecondsPrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void MillisecondsSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                MillisecondsSerializePrettyPrintSerialize(data, output, opts);
                return;
            }

            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                MillisecondsSerializeExcludeNullsSerialize(data, output, opts);
                return;
            }

            MillisecondsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void MillisecondsSerializePrettyPrintSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                MillisecondsPrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            MillisecondsPrettyPrintTypeCache<T>.Thunk(output, data, 0);
        }

        private static void MillisecondsSerializeExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                MillisecondsPrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            MillisecondsExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void MillisecondsPrettyPrintExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            MillisecondsPrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void ISO8601Serialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                ISO8601SerializePrettyPrintSerialize(data, output, opts);
                return;
            }

            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                ISO8601SerializeExcludeNullsSerialize(data, output, opts);
                return;
            }

            ISO8601TypeCache<T>.Thunk(output, data, 0);
        }

        private static void ISO8601SerializePrettyPrintSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                ISO8601PrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            ISO8601PrettyPrintTypeCache<T>.Thunk(output, data, 0);
        }

        private static void ISO8601SerializeExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                ISO8601PrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            ISO8601ExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void ISO8601PrettyPrintExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            ISO8601PrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void NewtonsoftStyleExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                NewtonsoftStylePrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            NewtonsoftStyleExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void NewtonsoftStylePrettyPrintSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                NewtonsoftStylePrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            NewtonsoftStylePrettyPrintTypeCache<T>.Thunk(output, data, 0);
        }

        private static void NewtonsoftStylePrettyPrintExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }
    }
}
