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



        internal static Serialize.ISerializeTypeCache CreateSerializeTypeCache(Options options)
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
                                                                    return new TypeCache<MicrosoftStyle>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStyleInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStyleJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStyleJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStyleExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStyleExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrint>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrintInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MicrosoftStylePrettyPrintExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<Milliseconds>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrint>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrintInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<MillisecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<Seconds>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrint>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrintInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrintJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrintJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<SecondsPrettyPrintExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601CamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601Utc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601UtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601Inherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601InheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601InheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601InheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601JSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601JSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601JSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601JSONPUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601JSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601JSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601JSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601JSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601ExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601ExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601ExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601ExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601ExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrint>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrintInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrintJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrintJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<ISO8601PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123CamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123Utc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123UtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123Inherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123InheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123InheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123InheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123JSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123JSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123JSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123JSONPUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123JSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123JSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123JSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123JSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123ExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123ExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123ExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123ExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123ExcludeNullsJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrint>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrintInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrintJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrintJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintJSONPInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNulls>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsInheritedUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONP>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONPCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONPUtcCamelCase>();
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
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInherited>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedCamelCase>();
                                                            }
                                                            break;
                                                        case UnspecifiedDateTimeKindBehavior.IsUTC:
                                                            switch (options.SerializationNameFormat)
                                                            {
                                                                case SerializationNameFormat.Verbatim:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtc>();
                                                                case SerializationNameFormat.CamelCase:
                                                                    return new TypeCache<RFC1123PrettyPrintExcludeNullsJSONPInheritedUtcCamelCase>();
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
        static Action<TextWriter, T, int> GetWriterAction<T>(Options options)
        {
            return options.SerializeTypeCache.Get<T>();
        }

        

        /// <summary>
        /// Generated giant switch of option finding via OptionsGeneration.linq
        /// </summary>
        static StringThunkDelegate<T> GetThunkerDelegate<T>(Options options)
        {
            return options.SerializeTypeCache.GetToString<T>();
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

        internal static Deserialize.IDeserializeTypeCache CreateDeserializeTypeCache(Options options)
        {
            try
            {
                switch (options.UseDateTimeFormat)
                {
                    case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyle>();
                            case SerializationNameFormat.CamelCase:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.MicrosoftStyleCamelCase>();
                        }
                        break;
                    case DateTimeFormat.MillisecondsSinceUnixEpoch:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyle>();
                            case SerializationNameFormat.CamelCase:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.MillisecondStyleCamelCase>();
                        }
                        break;
                    case DateTimeFormat.SecondsSinceUnixEpoch:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyle>();
                            case SerializationNameFormat.CamelCase:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.SecondStyleCamelCase>();
                        }
                        break;
                    case DateTimeFormat.ISO8601:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601Style>();
                            case SerializationNameFormat.CamelCase:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.ISO8601StyleCamelCase>();
                        }
                        break;
                    case DateTimeFormat.RFC1123:
                        switch (options.SerializationNameFormat)
                        {
                            case SerializationNameFormat.Verbatim:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123Style>();
                            case SerializationNameFormat.CamelCase:
                                return new Jil.Deserialize.TypeCache<Jil.Deserialize.RFC1123StyleCamelCase>();
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


                options = options ?? DefaultOptions;


                return options.DeserializeTypeCache.Get<T>()(reader, 0);
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

                var thunk = new Deserialize.ThunkReader(text);

               return options.DeserializeTypeCache.GetFromString<T>()(ref thunk, 0);
              
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
