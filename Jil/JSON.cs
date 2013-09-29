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
            options = options ?? Options.None;

            if (options.ShouldPrettyPrint.GetValueOrDefault())
            {
                PrettyPrintSerialize(data, output, options);
                return;
            }

            if (options.ShouldExcludeNulls.GetValueOrDefault())
            {
                ExcludeNullsSerialize(data, output, options);
                return;
            }

            NoneTypeCache<T>.Thunk(output, data, 0);
        }

        private static void ExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldPrettyPrint.GetValueOrDefault())
            {
                PrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            ExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }

        private static void PrettyPrintSerialize<T>(T data, TextWriter output, Options opts)
        {
            if (opts.ShouldExcludeNulls.GetValueOrDefault())
            {
                PrettyPrintExcludeNullsSerialize(data, output, opts);
                return;
            }

            PrettyPrintTypeCache<T>.Thunk(output, data, 0);
        }

        private static void PrettyPrintExcludeNullsSerialize<T>(T data, TextWriter output, Options opts)
        {
            PrettyPrintExcludeNullsTypeCache<T>.Thunk(output, data, 0);
        }
    }
}
