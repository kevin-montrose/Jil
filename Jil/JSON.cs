using Jil.Serialize;
using Jil.SerializeDynamic;
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
        /// <summary>
        /// Serializes the given data to the provided TextWriter.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted, Options.Default is used.
        /// 
        /// Unlike Serialize, this method will inspect the Type of data to determine what serializer to invoke.
        /// This is not as fast as calling Serialize with a known type.
        /// 
        /// Objects with participate in the DLR will be serialized appropriately, all other types
        /// will be serialized via reflection.
        /// </summary>
        public static void SerializeDynamic(dynamic data, TextWriter output, Options options = null)
        {
            DynamicSerializer.Serialize(output, (object)data, options ?? Options.Default, 0);
        }

        /// <summary>
        /// Serializes the given data, returning it as a string.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted, Options.Default is used.
        /// 
        /// Unlike Serialize, this method will inspect the Type of data to determine what serializer to invoke.
        /// This is not as fast as calling Serialize with a known type.
        /// 
        /// Objects with participate in the DLR will be serialized appropriately, all other types
        /// will be serialized via reflection.
        /// </summary>
        public static string SerializeDynamic(object data, Options options = null)
        {
            using (var str = new StringWriter())
            {
                SerializeDynamic(data, str, options);
                return str.ToString();
            }
        }

        /// <summary>
        /// Serializes the given data to the provided TextWriter.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted, Options.Default is used.
        /// </summary>
        public static void Serialize<T>(T data, TextWriter output, Options options = null)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            if (typeof(T) == typeof(object))
            {
                SerializeDynamic(data, output, options);
                return;
            }

            options = options ?? Options.Default;

            switch (options.UseDateTimeFormat)
            {
                case DateTimeFormat.ISO8601:
                    ISO8601(data, output, options);
                    return;

                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    Milliseconds(data, output, options);
                    return;

                case DateTimeFormat.SecondsSinceUnixEpoch:
                    Seconds(data, output, options);
                    return;

                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                    NewtonsoftStyle(data, output, options);
                    return;

                default: throw new InvalidOperationException("Unexpected Options: " + options);
            }
        }

        /// <summary>
        /// Serializes the given data, returning the output as a string.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted, Options.Default is used.
        /// </summary>
        public static string Serialize<T>(T data, Options options = null)
        {
            if (typeof(T) == typeof(object))
            {
                return SerializeDynamic(data, options);
            }

            using (var str = new StringWriter())
            {
                Serialize(data, str, options);
                return str.ToString();
            }

            // TODO: Actually start using this crazy crap

            options = options ?? Options.Default;

            switch (options.UseDateTimeFormat)
            {
                case DateTimeFormat.ISO8601:
                    return ISO8601ToString(data, options);

                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    return ISO8601ToString(data, options);

                case DateTimeFormat.SecondsSinceUnixEpoch:
                    return ISO8601ToString(data, options);

                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                    return ISO8601ToString(data, options);

                default: throw new InvalidOperationException("Unexpected Options: " + options);
            }
        }

        static void NewtonsoftStyle<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                NewtonsoftStyleExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                NewtonsoftStyleExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                NewtonsoftStylePrettyPrintJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                NewtonsoftStylePrettyPrintInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                NewtonsoftStyleJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                NewtonsoftStyleExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                NewtonsoftStylePrettyPrintTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                NewtonsoftStyleJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                NewtonsoftStyleInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            NewtonsoftStyleTypeCache<T>.Get()(output, data, 0);
        }

        static string NewtonsoftStyleToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return NewtonsoftStyleExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return NewtonsoftStyleExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return NewtonsoftStylePrettyPrintJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return NewtonsoftStylePrettyPrintInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return NewtonsoftStyleJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls)
            {
                return NewtonsoftStyleExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint)
            {
                return NewtonsoftStylePrettyPrintTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP)
            {
                return NewtonsoftStyleJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldIncludeInherited)
            {
                return NewtonsoftStyleInheritedTypeCache<T>.GetToString()(data, 0);
            }

            return NewtonsoftStyleTypeCache<T>.GetToString()(data, 0);
        }

        static void Milliseconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                MillisecondsExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                MillisecondsExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                MillisecondsPrettyPrintJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                MillisecondsPrettyPrintExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                MillisecondsExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                MillisecondsPrettyPrintTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                MillisecondsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                MillisecondsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            MillisecondsTypeCache<T>.Get()(output, data, 0);
        }

        static string MillisecondsToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return MillisecondsExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return MillisecondsPrettyPrintJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return MillisecondsExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return MillisecondsExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return MillisecondsPrettyPrintJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return MillisecondsPrettyPrintExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return MillisecondsPrettyPrintInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return MillisecondsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls)
            {
                return MillisecondsExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint)
            {
                return MillisecondsPrettyPrintTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP)
            {
                return MillisecondsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldIncludeInherited)
            {
                return MillisecondsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            return MillisecondsTypeCache<T>.GetToString()(data, 0);
        }

        static void Seconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                SecondsExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                SecondsExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                SecondsPrettyPrintJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                SecondsPrettyPrintExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                SecondsExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                SecondsPrettyPrintTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                SecondsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                SecondsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            SecondsTypeCache<T>.Get()(output, data, 0);
        }

        static string SecondsToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return SecondsExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return SecondsPrettyPrintJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return SecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return SecondsExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return SecondsExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return SecondsPrettyPrintJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return SecondsPrettyPrintExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return SecondsPrettyPrintInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return SecondsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls)
            {
                return SecondsExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint)
            {
                return SecondsPrettyPrintTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP)
            {
                return SecondsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldIncludeInherited)
            {
                return SecondsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            return SecondsTypeCache<T>.GetToString()(data, 0);
        }

        static void ISO8601<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601ExcludeNullsJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintJSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                ISO8601ExcludeNullsInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                ISO8601ExcludeNullsJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                ISO8601PrettyPrintJSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                ISO8601PrettyPrintExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601JSONPInheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                ISO8601ExcludeNullsTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                ISO8601PrettyPrintTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                ISO8601JSONPTypeCache<T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                ISO8601InheritedTypeCache<T>.Get()(output, data, 0);
                return;
            }

            ISO8601TypeCache<T>.Get()(output, data, 0);
        }

        static string ISO8601ToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ISO8601ExcludeNullsJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ISO8601PrettyPrintJSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ISO8601PrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return ISO8601ExcludeNullsInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return ISO8601ExcludeNullsJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ISO8601PrettyPrintJSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return ISO8601PrettyPrintExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ISO8601PrettyPrintInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ISO8601JSONPInheritedTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldExcludeNulls)
            {
                return ISO8601ExcludeNullsTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldPrettyPrint)
            {
                return ISO8601PrettyPrintTypeCache<T>.GetToString()(data, 0);
            }

            if (options.IsJSONP)
            {
                return ISO8601JSONPTypeCache<T>.GetToString()(data, 0);
            }

            if (options.ShouldIncludeInherited)
            {
                return ISO8601InheritedTypeCache<T>.GetToString()(data, 0);
            }

            return ISO8601TypeCache<T>.GetToString()(data, 0);
        }

        /// <summary>
        /// Deserializes JSON from the given TextReader as the passed type.
        /// 
        /// This is equivalent to calling Deserialize&lt;T&gt;(TextReader, Options), except
        /// without requiring a generic parameter.  For true dynamic deserialization, you 
        /// should use DeserializeDynamic instead.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted, Options.Default is used.
        /// </summary>
        public static object Deserialize(TextReader reader, Type type, Options options = null)
        {
            if(reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if(type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type == typeof(object))
            {
                return DeserializeDynamic(reader, options);
            }

            return Jil.Deserialize.DeserializeIndirect.Deserialize(reader, type, options);
        }

        /// <summary>
        /// Deserializes JSON from the given string as the passed type.
        /// 
        /// This is equivalent to calling Deserialize&lt;T&gt;(string, Options), except
        /// without requiring a generic parameter.  For true dynamic deserialization, you 
        /// should use DeserializeDynamic instead.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted, Options.Default is used.
        /// </summary>
        public static object Deserialize(string text, Type type, Options options = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            using (var reader = new StringReader(text))
            {
                return Deserialize(reader, type, options);
            }
        }

        /// <summary>
        /// Deserializes JSON from the given TextReader.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted, Options.Default is used.
        /// </summary>
        public static T Deserialize<T>(TextReader reader, Options options = null)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (typeof(T) == typeof(object))
            {
                return DeserializeDynamic(reader, options);
            }

            try
            {
                options = options ?? Options.Default;

                switch (options.UseDateTimeFormat)
                {
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.NewtonsoftStyleTypeCache<T>.Get()(reader, 0);
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.MillisecondStyleTypeCache<T>.Get()(reader, 0);
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        return Jil.Deserialize.SecondStyleTypeCache<T>.Get()(reader, 0);
                    case DateTimeFormat.ISO8601:
                        return Jil.Deserialize.ISO8601StyleTypeCache<T>.Get()(reader, 0);
                    default: throw new InvalidOperationException("Unexpected Options: " + options);
                }

            }
            catch (Exception e)
            {
                if (e is DeserializationException) throw;

                throw new DeserializationException(e, reader);
            }
        }

        /// <summary>
        /// Deserializes JSON from the given string.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted, Options.Default is used.
        /// </summary>
        public static T Deserialize<T>(string text, Options options = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (typeof(T) == typeof(object))
            {
                return DeserializeDynamic(text, options);
            }

            using (var reader = new StringReader(text))
            {
                return Deserialize<T>(reader, options);
            }
        }

        /// <summary>
        /// Deserializes JSON from the given TextReader, inferring types from the structure of the JSON text.
        /// 
        /// For the best performance, use the strongly typed Deserialize method when possible.
        /// </summary>
        public static dynamic DeserializeDynamic(TextReader reader, Options options = null)
        {
            options = options ?? Options.Default;

            var built = Jil.DeserializeDynamic.DynamicDeserializer.Deserialize(reader, options);

            return built.BeingBuilt;
        }

        /// <summary>
        /// Deserializes JSON from the given string, inferring types from the structure of the JSON text.
        /// 
        /// For the best performance, use the strongly typed Deserialize method when possible.
        /// </summary>
        public static dynamic DeserializeDynamic(string str, Options options = null)
        {
            using (var reader = new StringReader(str))
            {
                return DeserializeDynamic(reader, options);
            }
        }
    }
}
