using Jil.Deserialize;
using System;
using System.IO;
using System.Text;

namespace Jil
{
    /// <summary>
    /// An exception thrown when Jil encounters an error when deserializing a stream.
    /// </summary>
    public class DeserializationException : Exception
    {
        private long? _position;
        private string _snippetAfterError;
        private bool _endedUnexpectedly;

        /// <summary>
        /// The position in the TextReader where an error occurred, if it is available.
        /// 
        /// This is meant for debugging purposes only, as exactly when Jil decides to abandon deserialization
        /// and throw an exception is an implementation detail.
        /// </summary>
        public long? Position
        {
            get { return _position; }
            private set { Data["Jil-Position"] = _position = value; }
        }

        /// <summary>
        /// A snippet of text immediately after an error occurred.
        /// 
        /// This is meant for debugging purposes only, as exactly when Jil decides to abandon deserialization
        /// and throw an exception is an implementation detail.
        /// </summary>
        public string SnippetAfterError
        {
            get { return _snippetAfterError; }
            private set { Data["Jil-SnippetAfterError"] = _snippetAfterError = value; }
        }

        /// <summary>
        /// Whether or not the TextReader ended sooner than Jil expected.
        /// 
        /// This is meant for debugging purposes only, as exactly when Jil decides to abandon deserialization
        /// and throw an exception is an implementation detail.
        /// </summary>
        public bool EndedUnexpectedly
        {
            get { return _endedUnexpectedly; }
            private set { Data["Jil-EndedUnexpectedly"] = _endedUnexpectedly = value; }
        }

        internal DeserializationException(Exception inner, TextReader reader, bool endedEarly)
            : base(inner.Message, inner)
        {
            InspectReader(reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(Exception inner, ref ThunkReader reader, bool endedEarly)
            : base(inner.Message, inner)
        {
            InspectReader(ref reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(string msg, TextReader reader, bool endedEarly) 
            : base(msg) 
        {
            InspectReader(reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(string msg, ref ThunkReader reader, bool endedEarly)
            : base(msg)
        {
            InspectReader(ref reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(string msg, TextReader reader, Exception inner, bool endedEarly)
            : base(msg, inner)
        {
            InspectReader(reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(string msg, ref ThunkReader reader, Exception inner, bool endedEarly)
            : base(msg, inner)
        {
            InspectReader(ref reader);
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(string msg, Exception inner, bool endedEarly) 
            : base(msg + ": " + inner.Message, inner) 
        {
            EndedUnexpectedly = endedEarly;
        }

        internal DeserializationException(Exception inner, bool endedEarly)
            : base(inner.Message, inner)
        {
            EndedUnexpectedly = endedEarly;
        }

        void InspectReader(ref ThunkReader reader)
        {
            try
            {
                Position = reader.Position;

                var sb = new StringBuilder();

                int c;
                while ((c = reader.Read()) != -1 && sb.Length < 50)
                {
                    sb.Append((char)c);
                }

                SnippetAfterError = sb.ToString();
            }
            catch (Exception) { /* best effort here, things are already jacked */ }
        }

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
