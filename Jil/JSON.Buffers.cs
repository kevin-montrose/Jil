#if BUFFER_AND_SEQUENCE

using Jil.Serialize;
using Jil.SerializeDynamic;
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

            if(typeof(T) == typeof(object))
            {
                SerializeDynamic(data, writer, options);
                return;
            }

            var innerWriter = new ThunkWriter();
            innerWriter.Init(writer);
            GetThunkerDelegate<T>(options)(ref innerWriter, data, 0);
            innerWriter.End();
        }

        /// <summary>
        /// Serializes the given data to the provided IBufferWriter(char).
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
        public static void SerializeDynamic(dynamic data, IBufferWriter<char> output, Options options = null)
        {
            var thunk = new WriterProxy();
            thunk.Init(output);
            DynamicSerializer.Serialize(ref thunk, (object)data, options ?? DefaultOptions, 0);
        }
    }
}
#endif
