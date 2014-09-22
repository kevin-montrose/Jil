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

            var asStr = Serialize(data, options);
            output.Write(asStr);

            // TODO: Uncomment this stuff when the time comes; 
            //       Memory pressure is a lot worse if we actually
            //       spin everything out into a string, TextWriter
            //       is an all around better interface

            /*options = options ?? Options.Default;

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
            }*/
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

            options = options ?? Options.Default;

            switch (options.UseDateTimeFormat)
            {
                case DateTimeFormat.ISO8601:
                    return ISO8601ToString(data, options);

                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    return MillisecondsToString(data, options);

                case DateTimeFormat.SecondsSinceUnixEpoch:
                    return SecondsToString(data, options);

                case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                    return NewtonsoftStyleToString(data, options);

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

        static string ReadToString<T>(StringThunkDelegate<T> del, T data)
        {
            var writer = new ThunkWriter();
            writer.Init();
            del(ref writer, data, 0);

            return writer.StaticToString();
        }

        static string NewtonsoftStyleToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStyleExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return ReadToString(NewtonsoftStyleExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(NewtonsoftStylePrettyPrintJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return ReadToString(NewtonsoftStylePrettyPrintExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStylePrettyPrintInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStyleJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return ReadToString(NewtonsoftStyleExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return ReadToString(NewtonsoftStylePrettyPrintTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return ReadToString(NewtonsoftStyleJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return ReadToString(NewtonsoftStyleInheritedTypeCache<T>.GetToString(), data);
            }

            return ReadToString(NewtonsoftStyleTypeCache<T>.GetToString(), data);
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
                return ReadToString(MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(MillisecondsExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(MillisecondsPrettyPrintJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return ReadToString(MillisecondsExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return ReadToString(MillisecondsExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(MillisecondsPrettyPrintJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return ReadToString(MillisecondsPrettyPrintExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(MillisecondsPrettyPrintInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(MillisecondsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return ReadToString(MillisecondsExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return ReadToString(MillisecondsPrettyPrintTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return ReadToString(MillisecondsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return ReadToString(MillisecondsInheritedTypeCache<T>.GetToString(), data);
            }

            return ReadToString(MillisecondsTypeCache<T>.GetToString(), data);
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
                return ReadToString(SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(SecondsExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(SecondsPrettyPrintJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(SecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return ReadToString(SecondsExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return ReadToString(SecondsExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(SecondsPrettyPrintJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return ReadToString(SecondsPrettyPrintExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(SecondsPrettyPrintInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(SecondsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return ReadToString(SecondsExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return ReadToString(SecondsPrettyPrintTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return ReadToString(SecondsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return ReadToString(SecondsInheritedTypeCache<T>.GetToString(), data);
            }

            return ReadToString(SecondsTypeCache<T>.GetToString(), data);
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
                return ReadToString(ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(ISO8601ExcludeNullsJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(ISO8601PrettyPrintJSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(ISO8601PrettyPrintExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return ReadToString(ISO8601ExcludeNullsInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return ReadToString(ISO8601ExcludeNullsJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return ReadToString(ISO8601PrettyPrintJSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return ReadToString(ISO8601PrettyPrintExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return ReadToString(ISO8601PrettyPrintInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return ReadToString(ISO8601JSONPInheritedTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return ReadToString(ISO8601ExcludeNullsTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return ReadToString(ISO8601PrettyPrintTypeCache<T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return ReadToString(ISO8601JSONPTypeCache<T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return ReadToString(ISO8601InheritedTypeCache<T>.GetToString(), data);
            }

            return ReadToString(ISO8601TypeCache<T>.GetToString(), data);
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
