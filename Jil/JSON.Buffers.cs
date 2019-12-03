#if BUFFER_AND_SEQUENCE

using System.IO.Pipelines;
using Jil.Serialize;
using Jil.SerializeDynamic;
using System.Buffers;
using System.IO;
using Jil.Common;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Jil
{
    public sealed partial class JSON
    {
        /// <summary>
        /// Serializes the given data to the provided IBufferWriter(char).
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static void Serialize<T>(T data, IBufferWriter<char> writer, Options options = null)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

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
        /// Serializes the given data to the provided PipeWriter, using the given Encoding to convert characters
        ///   into bytes.
        ///   
        /// The resulting task returns the number of _bytes_ written to the given PipeWriter.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static async ValueTask<int> SerializeAsync<T>(T data, PipeWriter writer, Encoding encoding, Options options = null, CancellationToken cancellationToken = default)
        {
            // todo: test!

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var wrapper = new PipeWriterAdapter(writer, encoding);
            Serialize(data, wrapper, options);
            wrapper.Complete();

            await writer.FlushAsync(cancellationToken);

            return wrapper.BytesWritten;
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
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var thunk = new ThunkWriter();
            thunk.Init(output);
            DynamicSerializer.Serialize(ref thunk, (object)data, options ?? DefaultOptions, 0);
            thunk.End();
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
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var wrapped = output.MakeBufferWriter(out var disposable);
            using (disposable)
            {
                var thunk = new ThunkWriter();
                thunk.Init(wrapped);
                DynamicSerializer.Serialize(ref thunk, (object)data, options ?? DefaultOptions, 0);
                thunk.End();
            }
        }

        /// <summary>
        /// Serializes the given data to the provided PipeWriter, using the given Encoding to convert characters
        ///   into bytes.
        ///   
        /// The resulting task returns the number of _bytes_ written to the given PipeWriter.
        /// 
        /// Pass an Options object to configure the particulars (such as whitespace, and DateTime formats) of
        /// the produced JSON.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// 
        /// Unlike SerializeAsync, this method will inspect the Type of data to determine what serializer to invoke.
        /// This is not as fast as calling Serialize with a known type.
        /// 
        /// Objects with participate in the DLR will be serialized appropriately, all other types
        /// will be serialized via reflection.
        /// </summary>
        public static async ValueTask<int> SerializeDynamicAsync(dynamic data, PipeWriter writer, Encoding encoding, Options options = null, CancellationToken cancellationToken = default)
        {
            // todo: test!

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var wrapper = new PipeWriterAdapter(writer, encoding);
            SerializeDynamic(data, wrapper, options);
            wrapper.Complete();

            await writer.FlushAsync(cancellationToken);

            return wrapper.BytesWritten;
        }
    }
}
#endif
