#if BUFFER_AND_SEQUENCE

using System.IO.Pipelines;
using System;
using System.Buffers;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    public sealed partial class JSON
    {
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


            var seq = new ReadOnlySequence<char>(text.AsMemory());
            return Deserialize<T>(seq, options);
        }

        /// <summary>
        /// Deserializes JSON from the given ReadOnlySequence(char).
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static T Deserialize<T>(ReadOnlySequence<char> sequence, Options options = null)
        {
            if (typeof(T) == typeof(object))
            {
                return DeserializeDynamic(sequence, options);
            }

            try
            {
                options = options ?? DefaultOptions;

                var thunk = new Jil.Deserialize.ThunkReader(sequence);
                var del = GetDeserializeStringThunkDelegate<T>(options);
                return del(ref thunk, 0);
            }
            catch (Exception e)
            {
                if (e is DeserializationException) throw;

                throw new DeserializationException(e, false);
            }
        }

        /// <summary>
        /// Deserializes JSON from the given PipeReader, using the given Encoding to convert bytes to characters.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static ValueTask<T> DeserializeAsync<T>(PipeReader reader, Encoding encoding, Options options = null, CancellationToken cancel = default)
        {
            // todo: test!

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            if(encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using (var wrapper = new Jil.Deserialize.PipeReaderAdapter(reader, encoding))
            {
                var ret = Deserialize<T>(wrapper, options);

                return new ValueTask<T>(ret);
            }
        }

        /// <summary>
        /// Deserializes JSON from the given string, inferring types from the structure of the JSON text.
        /// 
        /// For the best performance, use the strongly typed Deserialize method when possible.
        /// </summary>
        public static dynamic DeserializeDynamic(string text, Options options = null)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }

            var seq = new ReadOnlySequence<char>(text.AsMemory());

            return DeserializeDynamic(seq, options);
        }

        /// <summary>
        /// Deserializes JSON from the given ReadOnlySequence(char), inferring types from the structure of the JSON text.
        /// 
        /// For the best performance, use the strongly typed Deserialize method when possible.
        /// </summary>
        public static dynamic DeserializeDynamic(ReadOnlySequence<char> sequence, Options options = null)
        {
            var thunkReader = new Deserialize.ThunkReader(sequence);
            options = options ?? DefaultOptions;

            var built = Jil.DeserializeDynamic.DynamicDeserializer.DeserializeThunkReader(ref thunkReader, options);

            return built.BeingBuilt;
        }

        /// <summary>
        /// Deserializes JSON from the given PipeReader, using the given Encoding to convert bytes to characters.
        /// 
        /// Pass an Options object to specify the particulars (such as DateTime formats) of
        /// the JSON being deserialized.  If omitted Options.Default is used, unless JSON.SetDefaultOptions(Options) has been
        /// called with a different Options object.
        /// </summary>
        public static ValueTask<dynamic> DeserializeDynamicAsync(PipeReader reader, Encoding encoding, Options options = null, CancellationToken cancel = default)
        {
            // todo: test!
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using (var wrapper = new Jil.Deserialize.PipeReaderAdapter(reader, encoding))
            {
                var ret = DeserializeDynamic(wrapper, options);

                return new ValueTask<dynamic>(ret);
            }
        }
    }
}
#endif