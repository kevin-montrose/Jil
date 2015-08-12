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

            GetWriterAction<T>(options)(output, data, 0);
        }

        /// <summary>
        /// Generated giant switch of option finding via OptionsGeneration.linq
        /// </summary>
        static Action<TextWriter, T, int> GetWriterAction<T>(Options options)
        {
            // Start OptionsGeneration.linq generated content: GetWriterAction
            switch (options.UseDateTimeFormat)
            {
                case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyle, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrint, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<Milliseconds, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrint, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.SecondsSinceUnixEpoch:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<Seconds, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrint, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.ISO8601:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601Utc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601Inherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601InheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601JSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601JSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601JSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601JSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrint, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.RFC1123:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123Utc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123Inherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123InheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123JSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123JSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123JSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123JSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrint, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNulls, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONP, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
            // End OptionsGeneration.linq generated content: GetWriterAction

            throw new InvalidOperationException("Unexpected Options: " + options);
        }


        /// <summary>
        /// Generated giant switch of option finding via OptionsGeneration.linq
        /// </summary>
        static StringThunkDelegate<T> GetThunkerDelegate<T>(Options options)
        {
            // Start OptionsGeneration.linq generated content: GetThunkerDelegate 
switch (options.UseDateTimeFormat)
            {
                case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyle, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrint, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.MillisecondsSinceUnixEpoch:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<Milliseconds, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrint, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.SecondsSinceUnixEpoch:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<Seconds, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrint, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.ISO8601:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601Utc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601Inherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601InheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601JSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601JSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601JSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601JSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrint, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case DateTimeFormat.RFC1123:
                    switch (options.ShouldPrettyPrint)
                    {
                        case false:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123Utc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123Inherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123InheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123JSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123JSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123JSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123JSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123ExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (options.ShouldExcludeNulls)
                            {
                                case false:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrint, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case true:
                                    switch (options.IsJSONP)
                                    {
                                        case false:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNulls, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case true:
                                            switch (options.ShouldIncludeInherited)
                                            {
                                                case false:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
            // End OptionsGeneration.linq generated content: GetThunkerDelegate

            throw new InvalidOperationException("Unexpected Options: " + options);
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


            var writer = new ThunkWriter();
            writer.Init();
            GetThunkerDelegate<T>(options)(ref writer, data, 0);
            return writer.StaticToString();
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

            if (typeof(T) == typeof(object))
            {
                return DeserializeDynamic(reader, options);
            }

            try
            {
                options = options ?? DefaultOptions;
                reader = reader.MakeSupportPeek();

                switch (options.UseDateTimeFormat)
                {
                    case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyle, T>.Get()(reader, 0);
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

            try
            {
                options = options ?? DefaultOptions;

                var thunk = new Jil.Deserialize.ThunkReader(text);

                switch (options.UseDateTimeFormat)
                {
                    case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch:
                        return Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyle, T>.GetFromString()(ref thunk, 0);
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

                throw new DeserializationException(e, false);
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
