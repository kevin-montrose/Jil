using System.IO;

namespace Jil.Common
{
    class PeekSupportingTextReader : TextReader
    {
        TextReader Inner;

        int? Peeked;

        public PeekSupportingTextReader(TextReader inner)
        {
            Inner = inner;
        }

        /// <summary>
        /// Reads the next character without changing the state of the reader or the character source. Returns the next available character without actually reading it from the reader.
        /// </summary>
        /// <returns>
        /// An integer representing the next character to be read, or -1 if no more characters are available or the reader does not support seeking.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader"/> is closed. </exception><exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override int Peek()
        {
            if (Peeked != null) return Peeked.Value;

            Peeked = Inner.Read();

            return Peeked.Value;
        }

        /// <summary>
        /// Reads the next character from the text reader and advances the character position by one character.
        /// </summary>
        /// <returns>
        /// The next character from the text reader, or -1 if no more characters are available. The default implementation returns -1.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader"/> is closed. </exception><exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override int Read()
        {
            if (Peeked != null)
            {
                var ret = Peeked.Value;
                Peeked = null;

                return ret;
            }

            return Inner.Read();
        }
    }
}
