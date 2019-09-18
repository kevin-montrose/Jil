#if BUFFER_AND_SEQUENCE

using Jil.Serialize;
using System.Buffers;

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
            var wrapped = new StringBuilderBufferWriter();
            Serialize<T>(data, wrapped, options);
            return wrapped.GetString();
        }

        /// <summary>
        /// Serializes the given data to the provided IBufferWriter(char).
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static void Serialize<T>(T data, IBufferWriter<char> writer, Options options = null)
        {
            options = options ?? DefaultOptions;

            var innerWriter = new ThunkWriter();
            innerWriter.Init(writer);
            GetThunkerDelegate<T>(options)(ref innerWriter, data, 0);
            innerWriter.End();
        }
    }
}
#endif
