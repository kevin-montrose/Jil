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

            using (var str = new StringWriter(System.Globalization.CultureInfo.InvariantCulture))
            {
                Serialize(data, str, options);
                return str.ToString();
            }
        }

        static void NewtonsoftStyle<T>(T data, TextWriter output, Options options)
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
        }

        static void Milliseconds<T>(T data, TextWriter output, Options options)
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
        }

        static void Seconds<T>(T data, TextWriter output, Options options)
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
        }

        static void ISO8601<T>(T data, TextWriter output, Options options)
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

            return Jil.Deserialize.DeserializeIndirect.Deserialize(reader.MakeSupportPeek(), type, options);
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
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
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
                options = options ?? DefaultOptions;

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
