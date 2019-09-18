#if !BUFFER_AND_SEQUENCE
using System.IO;

namespace Jil.SerializeDynamic
{
    internal ref partial struct WriterProxy
    {
        private TextWriter Inner;

        public void InitWithWriter(TextWriter inner)
        {
            Inner = inner;
        }

        public void Write(char c)
        {
            Inner.Write(c);
        }

        public void Write(string str)
        {
            Inner.Write(str);
        }

        public TextWriter AsWriter()
        {
            return Inner;
        }

        public void DoneWithWriter(){ }
    }
}
#endif