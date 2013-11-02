using Jil.Serialize;
using System;
using System.Collections;
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
        private static Hashtable SerializeDynamicLookup = new Hashtable();

        /// <summary>
        /// Serializes the given data to the provided TextWriter.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted, Options.Default is used.
        /// 
        /// Unlike Serialize, this method will inspect the Type of data to determine what serializer to invoke.
        /// This is not as fast as calling Serialize with a known type.
        /// </summary>
        public static void SerializeDynamic(object data, TextWriter output, Options options = null)
        {
            // Can't infer the type if we don't even have an object
            if (data == null)
            {
                if (!(options ?? Options.Default).ShouldExcludeNulls)
                {
                    output.Write("null");
                }

                return;
            }

            var type = data.GetType();
            var invoke = (Action<object, TextWriter, Options>)SerializeDynamicLookup[type];
            if (invoke == null)
            {
                invoke = (Action<object, TextWriter, Options>)typeof(SerializeDynamicThunk<>).MakeGenericType(type).GetField("Thunk", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);

                lock (SerializeDynamicLookup)
                {
                    SerializeDynamicLookup[type] = invoke;
                }
            }

            invoke(data, output, options);
        }

        /// <summary>
        /// Serializes the given data to the provided TextWriter.
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
                    ISO8601(data, output, options);
                    return;

                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    Milliseconds<T>(data, output, options);
                    return;

                case DateTimeFormat.SecondsSinceUnixEpoch:
                    Seconds<T>(data, output, options);
                    return;

                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                    NewtonsoftStyle<T>(data, output, options);
                    return;

                default: throw new InvalidOperationException("Unexpected Options: " + options);
            }
        }

        static void NewtonsoftStyle<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                NewtonsoftStyleExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                NewtonsoftStylePrettyPrintJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

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


            if (options.IsJSONP)
            {
                NewtonsoftStyleJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            NewtonsoftStyleTypeCache<T>.Thunk(output, data, 0);
        }

        static void Milliseconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                MillisecondsExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                MillisecondsPrettyPrintJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

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


            if (options.IsJSONP)
            {
                MillisecondsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            MillisecondsTypeCache<T>.Thunk(output, data, 0);
        }

        static void Seconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                SecondsExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                SecondsPrettyPrintJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

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


            if (options.IsJSONP)
            {
                SecondsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            SecondsTypeCache<T>.Thunk(output, data, 0);
        }

        static void ISO8601<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                ISO8601ExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                ISO8601PrettyPrintJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

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

            if (options.IsJSONP)
            {
                ISO8601JSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            ISO8601TypeCache<T>.Thunk(output, data, 0);
        }
    }
}
