#if !BUFFER_AND_SEQUENCE
using Jil.Serialize;

namespace Jil
{
    public sealed partial class JSON
    {
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
    }
}
#endif