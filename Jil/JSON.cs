using Jil.Serialize;
using Jil.SerializeDynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;

namespace Jil
{
    /// <summary>
    /// Fast JSON serializer.
    /// </summary>
    public sealed class JSON
    {
        static Options DefaultOptions = Options.Default;

        /// <summary>
        /// Sets the Options object that Jil will use to calls of Serialize(Dynamic) and Deserialize(Dynamic)
        /// if no Options object is provided.
        /// 
        /// By default, Jil will use the Options.Default object.
        /// 
        /// The current default Options can be retrieved with GetDefaultOptions().
        /// </summary>
        public static void SetDefaultOptions(Options options)
        {
            if (options == null) throw new ArgumentNullException("options");

            DefaultOptions = options;
        }

        /// <summary>
        /// Gets the Options object that Jil will use to calls of Serialize(Dynamic) and Deserialize(Dynamic)
        /// if no Options object is provided.
        /// 
        /// By default, Jil will use the Options.Default object.
        /// 
        /// The default Options can be set with SetDefaultOptions(Options options).
        /// </summary>
        public static Options GetDefaultOptions()
        {
            return DefaultOptions;
        }

        /// <summary>
        /// Serializes the given data to the provided TextWriter.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// 
        /// Unlike Serialize, this method will inspect the Type of data to determine what serializer to invoke.
        /// This is not as fast as calling Serialize with a known type.
        /// 
        /// Objects with participate in the DLR will be serialized appropriately, all other types
        /// will be serialized via reflection.
        /// </summary>
        public static void SerializeDynamic(dynamic data, TextWriter output, Options options = null)
        {
            DynamicSerializer.Serialize(output, (object)data, options ?? DefaultOptions, 0);
        }

        /// <summary>
        /// Serializes the given data, returning it as a string.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
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
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
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

            options = options ?? DefaultOptions;

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

                case DateTimeFormat.RFC1123:
                    RFC1123(data, output, options);
                    return;

                default: throw new InvalidOperationException("Unexpected Options: " + options);
            }
        }

        /// <summary>
        /// Serializes the given data, returning the output as a string.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static string Serialize<T>(T data, Options options = null)
        {
            if (typeof(T) == typeof(object))
            {
                return SerializeDynamic(data, options);
            }

            options = options ?? DefaultOptions;

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

                case DateTimeFormat.RFC1123:
                    return RFC1123ToString(data, options);

                default: throw new InvalidOperationException("Unexpected Options: " + options);
            }
        }

        static string WriteToString<T>(StringThunkDelegate<T> del, T data)
        {
            var writer = new ThunkWriter();
            writer.Init();
            del(ref writer, data, 0);

            return writer.StaticToString();
        }

        static void NewtonsoftStyle<T>(T data, TextWriter output, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStyleExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStylePrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStyleExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    TypeCache<NewtonsoftStyleExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<NewtonsoftStylePrettyPrintJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    TypeCache<NewtonsoftStylePrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStylePrettyPrintInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStyleJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls)
                {
                    TypeCache<NewtonsoftStyleExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint)
                {
                    TypeCache<NewtonsoftStylePrettyPrint, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP)
                {
                    TypeCache<NewtonsoftStyleJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldIncludeInherited)
                {
                    TypeCache<NewtonsoftStyleInherited, T>.Get()(output, data, 0);
                    return;
                }

                TypeCache<NewtonsoftStyle, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<NewtonsoftStyleExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<NewtonsoftStylePrettyPrintJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<NewtonsoftStyleExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<NewtonsoftStylePrettyPrintUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<NewtonsoftStyleJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<NewtonsoftStyleUtc, T>.Get()(output, data, 0);
            return;
        }

        static string NewtonsoftStyleToString<T>(T data, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrintInherited, T>.GetToString(), data);
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStyleJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls)
                {
                    return WriteToString(TypeCache<NewtonsoftStyleExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<NewtonsoftStylePrettyPrint, T>.GetToString(), data);
                }

                if (options.IsJSONP)
                {
                    return WriteToString(TypeCache<NewtonsoftStyleJSONP, T>.GetToString(), data);
                }

                if (options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<NewtonsoftStyleInherited, T>.GetToString(), data);
                }

                return WriteToString(TypeCache<NewtonsoftStyle, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintInheritedUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStyleJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleInheritedUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<NewtonsoftStyleUtc, T>.GetToString(), data);
        }

        static void Milliseconds<T>(T data, TextWriter output, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    TypeCache<MillisecondsExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<MillisecondsPrettyPrintJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsPrettyPrintInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls)
                {
                    TypeCache<MillisecondsExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint)
                {
                    TypeCache<MillisecondsPrettyPrint, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP)
                {
                    TypeCache<MillisecondsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldIncludeInherited)
                {
                    TypeCache<MillisecondsInherited, T>.Get()(output, data, 0);
                    return;
                }

                TypeCache<Milliseconds, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<MillisecondsExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<MillisecondsPrettyPrintJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<MillisecondsExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<MillisecondsPrettyPrintUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<MillisecondsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<MillisecondsUtc, T>.Get()(output, data, 0);
            return;
        }

        static string MillisecondsToString<T>(T data, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    return WriteToString(TypeCache<MillisecondsExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrintJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrintInherited, T>.GetToString(), data);
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls)
                {
                    return WriteToString(TypeCache<MillisecondsExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<MillisecondsPrettyPrint, T>.GetToString(), data);
                }

                if (options.IsJSONP)
                {
                    return WriteToString(TypeCache<MillisecondsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<MillisecondsInherited, T>.GetToString(), data);
                }

                return WriteToString(TypeCache<Milliseconds, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintInheritedUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsInheritedUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<MillisecondsUtc, T>.GetToString(), data);
        }

        static void Seconds<T>(T data, TextWriter output, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsPrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    TypeCache<SecondsExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<SecondsPrettyPrintJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    TypeCache<SecondsPrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsPrettyPrintInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls)
                {
                    TypeCache<SecondsExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint)
                {
                    TypeCache<SecondsPrettyPrint, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP)
                {
                    TypeCache<SecondsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldIncludeInherited)
                {
                    TypeCache<SecondsInherited, T>.Get()(output, data, 0);
                    return;
                }

                TypeCache<Seconds, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<SecondsExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<SecondsPrettyPrintJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<SecondsExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<SecondsPrettyPrintUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<SecondsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<SecondsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<SecondsUtc, T>.Get()(output, data, 0);
            return;
        }

        static string SecondsToString<T>(T data, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrintJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    return WriteToString(TypeCache<SecondsExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrintJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrintExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrintInherited, T>.GetToString(), data);
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls)
                {
                    return WriteToString(TypeCache<SecondsExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<SecondsPrettyPrint, T>.GetToString(), data);
                }

                if (options.IsJSONP)
                {
                    return WriteToString(TypeCache<SecondsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<SecondsInherited, T>.GetToString(), data);
                }

                return WriteToString(TypeCache<Seconds, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintInheritedUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsInheritedUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<SecondsUtc, T>.GetToString(), data);
        }

        static void RFC1123<T>(T data, TextWriter output, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<RFC1123PrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123ExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123PrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123PrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123ExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    TypeCache<RFC1123ExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<RFC1123PrettyPrintJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    TypeCache<RFC1123PrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123PrettyPrintInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123JSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls)
                {
                    TypeCache<RFC1123ExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint)
                {
                    TypeCache<RFC1123PrettyPrint, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP)
                {
                    TypeCache<RFC1123JSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldIncludeInherited)
                {
                    TypeCache<RFC1123Inherited, T>.Get()(output, data, 0);
                    return;
                }

                TypeCache<RFC1123, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123ExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123PrettyPrintJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123ExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<RFC1123ExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<RFC1123PrettyPrintJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<RFC1123PrettyPrintExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123PrettyPrintInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123JSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<RFC1123ExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<RFC1123PrettyPrintUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<RFC1123JSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<RFC1123InheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<RFC1123Utc, T>.Get()(output, data, 0);
            return;
        }

        static string RFC1123ToString<T>(T data, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123ExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrintJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123ExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    return WriteToString(TypeCache<RFC1123ExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrintJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrintInherited, T>.GetToString(), data);
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123JSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls)
                {
                    return WriteToString(TypeCache<RFC1123ExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<RFC1123PrettyPrint, T>.GetToString(), data);
                }

                if (options.IsJSONP)
                {
                    return WriteToString(TypeCache<RFC1123JSONP, T>.GetToString(), data);
                }

                if (options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<RFC1123Inherited, T>.GetToString(), data);
                }

                return WriteToString(TypeCache<RFC1123, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123ExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123ExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<RFC1123ExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintInheritedUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123JSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<RFC1123ExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<RFC1123PrettyPrintUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<RFC1123JSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<RFC1123InheritedUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<RFC1123Utc, T>.GetToString(), data);
        }

        static void ISO8601<T>(T data, TextWriter output, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601PrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601ExcludeNullsInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    TypeCache<ISO8601ExcludeNullsJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    TypeCache<ISO8601PrettyPrintJSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    TypeCache<ISO8601PrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601PrettyPrintInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601JSONPInherited, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldExcludeNulls)
                {
                    TypeCache<ISO8601ExcludeNulls, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldPrettyPrint)
                {
                    TypeCache<ISO8601PrettyPrint, T>.Get()(output, data, 0);
                    return;
                }

                if (options.IsJSONP)
                {
                    TypeCache<ISO8601JSONP, T>.Get()(output, data, 0);
                    return;
                }

                if (options.ShouldIncludeInherited)
                {
                    TypeCache<ISO8601Inherited, T>.Get()(output, data, 0);
                    return;
                }

                TypeCache<ISO8601, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601ExcludeNullsJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintJSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601ExcludeNullsInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<ISO8601ExcludeNullsJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<ISO8601PrettyPrintJSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601JSONPInheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<ISO8601ExcludeNullsUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<ISO8601PrettyPrintUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<ISO8601JSONPUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601InheritedUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<ISO8601Utc, T>.Get()(output, data, 0);
            return;
        }

        static string ISO8601ToString<T>(T data, Options options)
        {
            if (options.UseUnspecifiedDateTimeKindBehavior == UnspecifiedDateTimeKindBehavior.IsLocal)
            {
                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrintJSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601ExcludeNullsInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.IsJSONP)
                {
                    return WriteToString(TypeCache<ISO8601ExcludeNullsJSONP, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.IsJSONP)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrintJSONP, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrintInherited, T>.GetToString(), data);
                }

                if (options.IsJSONP && options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601JSONPInherited, T>.GetToString(), data);
                }

                if (options.ShouldExcludeNulls)
                {
                    return WriteToString(TypeCache<ISO8601ExcludeNulls, T>.GetToString(), data);
                }

                if (options.ShouldPrettyPrint)
                {
                    return WriteToString(TypeCache<ISO8601PrettyPrint, T>.GetToString(), data);
                }

                if (options.IsJSONP)
                {
                    return WriteToString(TypeCache<ISO8601JSONP, T>.GetToString(), data);
                }

                if (options.ShouldIncludeInherited)
                {
                    return WriteToString(TypeCache<ISO8601Inherited, T>.GetToString(), data);
                }

                return WriteToString(TypeCache<ISO8601, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintJSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintJSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintInheritedUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601JSONPInheritedUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601JSONPUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601InheritedUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<ISO8601Utc, T>.GetToString(), data);
        }

        /// <summary>
        /// Deserializes JSON from the given TextReader as the passed type.
        /// 
        /// This is equivalent to calling Deserialize&lt;T&gt;(TextReader, Options), except
        /// without requiring a generic parameter.  For true dynamic deserialization, you 
        /// should use DeserializeDynamic instead.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
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

            return Jil.Deserialize.DeserializeIndirect.DeserializeFromStream(reader.MakeSupportPeek(), type, options);
        }

        /// <summary>
        /// Deserializes JSON from the given string as the passed type.
        /// 
        /// This is equivalent to calling Deserialize&lt;T&gt;(string, Options), except
        /// without requiring a generic parameter.  For true dynamic deserialization, you 
        /// should use DeserializeDynamic instead.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static object Deserialize(string text, Type type, Options options = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type == typeof(object))
            {
                return DeserializeDynamic(text, options);
            }

            return Jil.Deserialize.DeserializeIndirect.DeserializeFromString(text, type, options);
        }

        /// <summary>
        /// Deserializes JSON from the given TextReader.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static T Deserialize<T>(TextReader reader, Options options = null)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            var txt = reader.ReadToEnd();
            return Deserialize<T>(txt, options);

            // TODO: just testing deserializing from string
            //       once that's done, restore the below

            /*if (typeof(T) == typeof(object))
            {
                return DeserializeDynamic(reader, options);
            }

            try
            {
                options = options ?? DefaultOptions;
                reader = reader.MakeSupportPeek();

                switch (options.UseDateTimeFormat)
                {
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.NewtonsoftStyle, T>.Get()(reader, 0);
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyle, T>.Get()(reader, 0);
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyle, T>.Get()(reader, 0);
                    case DateTimeFormat.ISO8601:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601Style, T>.Get()(reader, 0);
                    case DateTimeFormat.RFC1123:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123Style, T>.Get()(reader, 0);
                    default: throw new InvalidOperationException("Unexpected Options: " + options);
                }

            }
            catch (Exception e)
            {
                if (e is DeserializationException) throw;

                throw new DeserializationException(e, reader, false);
            }*/
        }

        /// <summary>
        /// Deserializes JSON from the given string.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
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

            try
            {
                options = options ?? DefaultOptions;

                var thunk = new Jil.Deserialize.ThunkReader(text);

                switch (options.UseDateTimeFormat)
                {
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.NewtonsoftStyle, T>.GetFromString()(ref thunk, 0);
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyle, T>.GetFromString()(ref thunk, 0);
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyle, T>.GetFromString()(ref thunk, 0);
                    case DateTimeFormat.ISO8601:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601Style, T>.GetFromString()(ref thunk, 0);
                    case DateTimeFormat.RFC1123:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123Style, T>.GetFromString()(ref thunk, 0);
                    default: throw new InvalidOperationException("Unexpected Options: " + options);
                }

            }
            catch (Exception e)
            {
                if (e is DeserializationException) throw;

                throw new DeserializationException("", e, false);
            }
        }

        /// <summary>
        /// Deserializes JSON from the given TextReader, inferring types from the structure of the JSON text.
        /// 
        /// For the best performance, use the strongly typed Deserialize method when possible.
        /// </summary>
        public static dynamic DeserializeDynamic(TextReader reader, Options options = null)
        {
            options = options ?? DefaultOptions;

            var built = Jil.DeserializeDynamic.DynamicDeserializer.Deserialize(reader.MakeSupportPeek(), options);

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
