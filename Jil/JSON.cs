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
                NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                NewtonsoftStylePrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                NewtonsoftStyleExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                NewtonsoftStylePrettyPrintJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                NewtonsoftStylePrettyPrintExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                NewtonsoftStyleExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                NewtonsoftStylePrettyPrintInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                NewtonsoftStyleJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldIncludeInherited)
            {
                NewtonsoftStyleInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            NewtonsoftStyleTypeCache<T>.Thunk(output, data, 0);
        }

        static void Milliseconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                MillisecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                MillisecondsExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                MillisecondsPrettyPrintInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                MillisecondsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldIncludeInherited)
            {
                MillisecondsInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            MillisecondsTypeCache<T>.Thunk(output, data, 0);
        }

        static void Seconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                SecondsPrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                SecondsExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                SecondsPrettyPrintInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                SecondsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldIncludeInherited)
            {
                SecondsInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            SecondsTypeCache<T>.Thunk(output, data, 0);
        }

        static void ISO8601<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                ISO8601PrettyPrintExcludeNullsJSONPTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601ExcludeNullsJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintJSONPInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                ISO8601ExcludeNullsInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                ISO8601PrettyPrintInheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                ISO8601JSONPInheritedTypeCache<T>.Thunk(output, data, 0);
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

            if (options.ShouldIncludeInherited)
            {
                ISO8601InheritedTypeCache<T>.Thunk(output, data, 0);
                return;
            }

            ISO8601TypeCache<T>.Thunk(output, data, 0);
        }

        /// <summary>
        /// Deserializes JSON from the given TextReader.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted, Options.Default is used.
        /// </summary>
        public static T Deserialize<T>(TextReader reader, Options options = null)
        {
            try
            {

                options = options ?? Options.Default;

                if (options.AllowHashFunction)
                {
                    switch (options.UseDateTimeFormat)
                    {
                        case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.NewtonsoftStyleTypeCache<T>.Thunk(reader);
                        case DateTimeFormat.MillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.MillisecondStyleTypeCache<T>.Thunk(reader);
                        case DateTimeFormat.SecondsSinceUnixEpoch:
                            return Jil.Deserialize.SecondStyleTypeCache<T>.Thunk(reader);
                        case DateTimeFormat.ISO8601:
                            return Jil.Deserialize.ISO8601StyleTypeCache<T>.Thunk(reader);
                        default: throw new InvalidOperationException("Unexpected Options: " + options);
                    }
                }
                else
                {
                    switch (options.UseDateTimeFormat)
                    {
                        case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.NewtonsoftStyleNoHashingTypeCache<T>.Thunk(reader);
                        case DateTimeFormat.MillisecondsSinceUnixEpoch:
                            return Jil.Deserialize.MillisecondStyleNoHashingTypeCache<T>.Thunk(reader);
                        case DateTimeFormat.SecondsSinceUnixEpoch:
                            return Jil.Deserialize.SecondStyleNoHashingTypeCache<T>.Thunk(reader);
                        case DateTimeFormat.ISO8601:
                            return Jil.Deserialize.ISO8601StyleNoHashingTypeCache<T>.Thunk(reader);
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
            using (var reader = new StringReader(text))
            {
                return Deserialize<T>(reader, options);
            }
        }
    }
}
