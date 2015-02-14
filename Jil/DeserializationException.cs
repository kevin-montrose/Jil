using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    /// <summary>
    /// An exception thrown when Jil encounters an error when deserializing a stream.
    /// </summary>
    public class DeserializationException : Exception
    {
        /// <summary>
        /// The position in the TextReader where an error occurred, if it is available.
        /// 
        /// This is meant for debugging purposes only, as exactly when Jil decides to abandon deserialization
        /// and throw an exception is an implementation detail.
        /// </summary>
        public long? Position { get; private set; }

        /// <summary>
        /// A snippet of text immediately after an error occurred.
        /// 
        /// This is meant for debugging purposes only, as exactly when Jil decides to abandon deserialization
        /// and throw an exception is an implementation detail.
        /// </summary>
        public string SnippetAfterError { get; private set; }

        /// <summary>
        /// Whether or not the TextReader ended sooner than Jil expected.
        /// 
        /// This is meant for debugging purposes only, as exactly when Jil decides to abandon deserialization
        /// and throw an exception is an implementation detail.
        /// </summary>
        public bool? EndedUnexpectedly { get; private set; }

        // TODO: Always indicate endedEarly, and then delete these constructors
        internal DeserializationException(Exception e, TextReader reader) : this(e, reader, default(bool?)) { }
        internal DeserializationException(string msg, TextReader reader) : this(msg, reader, default(bool?)) { }
        internal DeserializationException(string msg, TextReader reader, Exception inner) : this(msg, reader, inner, default(bool?)) { }

        internal DeserializationException(Exception inner, TextReader reader, bool? endedEarly)
            : base(inner.Message, inner)
        {
            InspectReader(reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(string msg, TextReader reader, bool? endedEarly) 
            : base(msg) 
        {
            InspectReader(reader);
            EndedUnexpectedly = endedEarly;
        }



        internal DeserializationException(string msg, TextReader reader, Exception inner, bool? endedEarly)
            : base(msg, inner)
        {
            InspectReader(reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(string msg, Exception inner) : base(msg + ": " + inner.Message, inner) { }

        void InspectReader(TextReader reader)
        {
            try
            {
                var asStreamReader = reader as StreamReader;
                if (asStreamReader != null)
                {
                    if (asStreamReader.BaseStream.CanSeek)
                    {
                        Position = asStreamReader.BaseStream.Position;
                    }
                }

                var sb = new StringBuilder();

                int c;
                while((c = reader.Read()) != -1 && sb.Length < 50)
                {
                    sb.Append((char)c);
                }

                SnippetAfterError = sb.ToString();
            }
            catch (Exception) { /* best effort here, things are already jacked */ }
        }
    }
}
