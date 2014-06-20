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
        /// 
        /// Note that this lookup only happens on the *root object*, members of type System.Object will not
        /// be serialized via a dynamic lookup.
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
        /// Serializes the given data, returning the output as a string.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted, Options.Default is used.
        /// 
        /// Unlike Serialize, this method will inspect the Type of data to determine what serializer to invoke.
        /// This is not as fast as calling Serialize with a known type.
        /// 
        /// Note that this lookup only happens on the *root object*, members of type System.Object will not
        /// be serialized via a dynamic lookup.
        /// </summary>
        public static string SerializeDynamic(object data, Options options = null)
        {
            options = options ?? Options.Default;

            if (data == null)
            {
                if (options.ShouldExcludeNulls)
                {
                    return "";
                }

                return "null";
            }

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
        /// the produced JSON.  If omitted, Options.Default is used.
        /// </summary>
        public static string Serialize<T>(T data, Options options = null)
        {
            if (typeof(T) == typeof(object))
                return SerializeDynamic(data, options);

            options = options ?? Options.Default;

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

        /// <summary>
        /// Deserializes JSON from the given TextReader.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted, Options.Default is used.
        /// </summary>
        public static T Deserialize<T>(TextReader reader, Options options = null)
        {
            if (typeof(T) == typeof(object))
                return DeserializeDynamic(reader, options);
            try
            {

                options = options ?? Options.Default;

                if (options.AllowHashFunction)
                {
                    switch (options.UseDateTimeFormat)
                    {
                        case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.NewtonsoftStyleTypeCache<T>.Get()(reader);
                        case DateTimeFormat.MillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.MillisecondStyleTypeCache<T>.Get()(reader);
                        case DateTimeFormat.SecondsSinceUnixEpoch:
                            return Jil.Deserialize.SecondStyleTypeCache<T>.Get()(reader);
                        case DateTimeFormat.ISO8601:
                            return Jil.Deserialize.ISO8601StyleTypeCache<T>.Get()(reader);
                        default: throw new InvalidOperationException("Unexpected Options: " + options);
                    }
                }
                else
                {
                    switch (options.UseDateTimeFormat)
                    {
                        case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.NewtonsoftStyleNoHashingTypeCache<T>.Get()(reader);
                        case DateTimeFormat.MillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.MillisecondStyleNoHashingTypeCache<T>.Get()(reader);
                        case DateTimeFormat.SecondsSinceUnixEpoch:
                            return Jil.Deserialize.SecondStyleNoHashingTypeCache<T>.Get()(reader);
                        case DateTimeFormat.ISO8601:
                            return Jil.Deserialize.ISO8601StyleNoHashingTypeCache<T>.Get()(reader);
                        default: throw new InvalidOperationException("Unexpected Options: " + options);
                    }
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
            if (typeof(T) == typeof(object))
                return DeserializeDynamic(text, options);
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
