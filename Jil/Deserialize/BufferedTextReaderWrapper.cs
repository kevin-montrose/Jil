using System.IO;
using System.Text;

namespace Jil.Deserialize
{
    /// <summary>
    /// Wraps TextReader instance 
    /// </summary>
    public class BufferedTextReaderWrapper : TextReader
    {
        private readonly TextReader _inner;
        private readonly StringBuilder _commonStringBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="commonStringBuilder"></param>
        public BufferedTextReaderWrapper(TextReader inner, StringBuilder commonStringBuilder)
        {
            _inner = inner;
            _commonStringBuilder = commonStringBuilder??new StringBuilder();
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
            return _inner.Peek();            
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
            var res = _inner.Read();
            if (res != -1)
            {
                _commonStringBuilder.Append((char) res);
            }
            return res;
        }

        /// <summary>
        /// Gets buffered string
        /// </summary>
        /// <returns></returns>
        public string GetBufferedString()
        {
            return _commonStringBuilder.ToString();
        }

        /// <summary>
        /// Get inner reader
        /// </summary>
        /// <returns></returns>
        public TextReader Unwrap => _inner;

        /// <summary>
        /// Wraps TextReader instance
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static BufferedTextReaderWrapper Wrap(TextReader reader, StringBuilder sb)
        {
            return new BufferedTextReaderWrapper(reader, sb);
        }
    }
}