#if BUFFER_AND_SEQUENCE
using System;
using System.Buffers;

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
    }
}
#endif