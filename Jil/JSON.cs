﻿using Jil.Serialize;
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
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<NewtonsoftStyleExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<NewtonsoftStylePrettyPrintJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<NewtonsoftStylePrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStylePrettyPrintInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<NewtonsoftStyleExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStylePrettyPrintNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<NewtonsoftStylePrettyPrint, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<NewtonsoftStyleJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<NewtonsoftStyleInherited, T>.Get()(output, data, 0);
                return;
            }

            if (!options.ShouldConvertToUtc)
            {
                TypeCache<NewtonsoftStyleNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<NewtonsoftStyle, T>.Get()(output, data, 0);
        }

        static string NewtonsoftStyleToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintInherited, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<NewtonsoftStyleExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrintNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<NewtonsoftStylePrettyPrint, T>.GetToString(), data);
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<NewtonsoftStyleJSONP, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<NewtonsoftStyleInherited, T>.GetToString(), data);
            }

            if (!options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<NewtonsoftStyleNotConvertToUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<NewtonsoftStyle, T>.GetToString(), data);
        }

        static void Milliseconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<MillisecondsExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<MillisecondsPrettyPrintJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsPrettyPrintInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<MillisecondsExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsPrettyPrintNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<MillisecondsPrettyPrint, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<MillisecondsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<MillisecondsInherited, T>.Get()(output, data, 0);
                return;
            }

            if(!options.ShouldConvertToUtc)
            {
                TypeCache<MillisecondsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<Milliseconds, T>.Get()(output, data, 0);
        }

        static string MillisecondsToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintInherited, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<MillisecondsExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrintNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<MillisecondsPrettyPrint, T>.GetToString(), data);
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<MillisecondsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<MillisecondsInherited, T>.GetToString(), data);
            }

            if (!options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<MillisecondsNotConvertToUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<Milliseconds, T>.GetToString(), data);
        }

        static void Seconds<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<SecondsExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<SecondsPrettyPrintJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<SecondsPrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsPrettyPrintInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<SecondsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<SecondsExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsPrettyPrintNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<SecondsPrettyPrint, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<SecondsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<SecondsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<SecondsInherited, T>.Get()(output, data, 0);
                return;
            }

            if(!options.ShouldConvertToUtc)
            {
                TypeCache<SecondsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<Seconds, T>.Get()(output, data, 0);
        }

        static string SecondsToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintInherited, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<SecondsExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsPrettyPrintNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<SecondsPrettyPrint, T>.GetToString(), data);
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<SecondsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<SecondsInherited, T>.GetToString(), data);
            }

            if (!options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<SecondsNotConvertToUtc, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<Seconds, T>.GetToString(), data);
        }

        static void ISO8601<T>(T data, TextWriter output, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601ExcludeNullsJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintJSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintJSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601ExcludeNullsInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601ExcludeNullsInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601ExcludeNullsJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                TypeCache<ISO8601ExcludeNullsJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintJSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                TypeCache<ISO8601PrettyPrintJSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                TypeCache<ISO8601PrettyPrintExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601PrettyPrintInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601JSONPInheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601JSONPInherited, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601ExcludeNullsNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldExcludeNulls)
            {
                TypeCache<ISO8601ExcludeNulls, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601PrettyPrintNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldPrettyPrint)
            {
                TypeCache<ISO8601PrettyPrint, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601JSONPNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.IsJSONP)
            {
                TypeCache<ISO8601JSONP, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601InheritedNotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            if (options.ShouldIncludeInherited)
            {
                TypeCache<ISO8601Inherited, T>.Get()(output, data, 0);
                return;
            }

            if (!options.ShouldConvertToUtc)
            {
                TypeCache<ISO8601NotConvertToUtc, T>.Get()(output, data, 0);
                return;
            }

            TypeCache<ISO8601, T>.Get()(output, data, 0);
        }

        static string ISO8601ToString<T>(T data, Options options)
        {
            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintJSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintJSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsJSONP, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintJSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintJSONP, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintInherited, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601JSONPInheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP && options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601JSONPInherited, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNullsNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldExcludeNulls)
            {
                return WriteToString(TypeCache<ISO8601ExcludeNulls, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrintNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldPrettyPrint)
            {
                return WriteToString(TypeCache<ISO8601PrettyPrint, T>.GetToString(), data);
            }

            if (options.IsJSONP && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601JSONPNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.IsJSONP)
            {
                return WriteToString(TypeCache<ISO8601JSONP, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited && !options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601InheritedNotConvertToUtc, T>.GetToString(), data);
            }

            if (options.ShouldIncludeInherited)
            {
                return WriteToString(TypeCache<ISO8601Inherited, T>.GetToString(), data);
            }

            if (!options.ShouldConvertToUtc)
            {
                return WriteToString(TypeCache<ISO8601, T>.GetToString(), data);
            }

            return WriteToString(TypeCache<ISO8601, T>.GetToString(), data);
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
