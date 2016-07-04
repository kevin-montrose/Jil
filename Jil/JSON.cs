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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyle, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrint, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<Milliseconds, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrint, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<Seconds, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrint, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601DontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601CamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601CamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601Utc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601UtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601UtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601UtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601Inherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601InheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601InheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601InheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrint, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123DontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123CamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123CamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123Utc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123UtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123UtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123UtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123Inherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123InheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123InheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123InheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrint, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNulls, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONP, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInherited, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.Get();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.Get();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.Get();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyle, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrint, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<Milliseconds, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrint, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<Seconds, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrint, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601DontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601CamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601CamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601Utc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601UtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601UtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601UtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601Inherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601InheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601InheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601InheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601InheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601JSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601JSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601ExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrint, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123DontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123CamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123CamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123Utc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123UtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123UtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123UtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123Inherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123InheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123InheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123InheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123InheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123JSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123JSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123ExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrint, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNulls, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
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
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONP, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case true:
                                                    switch (options.UseUnspecifiedDateTimeKindBehavior)
                                                    {
                                                        case UnspecifiedDateTimeKindBehavior.IsLocal:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInherited, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcDontCoerceToDateOnSerialize, T>.GetToString();
                                                                    }
                                                                    break;
                                                                case SerializationNameFormat.CamelCase:
                                                                    switch (options.ShouldNotCoerceToDateOnSerialize)
                                                                    {
                                                                        case false:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase, T>.GetToString();
                                                                        case true:
                                                                            return TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcCamelCaseDontCoerceToDateOnSerialize, T>.GetToString();
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
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyle, T>.Get()(reader, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyleCamelCase, T>.Get()(reader, 0);
                        }
                        break;
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyle, T>.Get()(reader, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyleCamelCase, T>.Get()(reader, 0);
                        }
                        break;
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyle, T>.Get()(reader, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyleCamelCase, T>.Get()(reader, 0);
                        }
                        break;
                    case DateTimeFormat.ISO8601:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601Style, T>.Get()(reader, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601StyleCamelCase, T>.Get()(reader, 0);
                        }
                        break;
                    case DateTimeFormat.RFC1123:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123Style, T>.Get()(reader, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123StyleCamelCase, T>.Get()(reader, 0);
                        }
                        break;
                }
                throw new InvalidOperationException("Unexpected Options: " + options);
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
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyle, T>.GetFromString()(ref thunk, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyleCamelCase, T>.GetFromString()(ref thunk, 0);
                        }
                        break;
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyle, T>.GetFromString()(ref thunk, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyleCamelCase, T>.GetFromString()(ref thunk, 0);
                        }
                        break;
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyle, T>.GetFromString()(ref thunk, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyleCamelCase, T>.GetFromString()(ref thunk, 0);
                        }
                        break;
                    case DateTimeFormat.ISO8601:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601Style, T>.GetFromString()(ref thunk, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601StyleCamelCase, T>.GetFromString()(ref thunk, 0);
                        }
                        break;
                    case DateTimeFormat.RFC1123:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123Style, T>.GetFromString()(ref thunk, 0);
                            case SerializationNameFormat.CamelCase:
                                return Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123StyleCamelCase, T>.GetFromString()(ref thunk, 0);
                        }
                        break;
                }
                throw new InvalidOperationException("Unexpected Options: " + options);
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
