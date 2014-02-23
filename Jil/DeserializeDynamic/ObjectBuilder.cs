using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    sealed class ObjectBuilder
    {
        public StringBuilder CommonStringBuffer;

        char[] _CommonCharBuffer;
        public char[] CommonCharBuffer
        {
            get
            {
                if (_CommonCharBuffer != null) return _CommonCharBuffer;

                _CommonCharBuffer = new char[Methods.CharBufferSize];
                return _CommonCharBuffer;
            }
        }

        public void PutNull()
        {
            throw new NotImplementedException();
        }

        public void PutTrue()
        {
            throw new NotImplementedException();
        }

        public void PutFalse()
        {
            throw new NotImplementedException();
        }

        public void PutString(string str)
        {
            throw new NotImplementedException();
        }

        public void StartArray()
        {
            throw new NotImplementedException();
        }

        public void EndArray()
        {
            throw new NotImplementedException();
        }

        public void StartObject()
        {
            throw new NotImplementedException();
        }

        public void EndObject()
        {
            throw new NotImplementedException();
        }

        public void StartObjectMember()
        {
            throw new NotImplementedException();
        }

        public void EndObjectMember()
        {
            throw new NotImplementedException();
        }

        public void PutNumber(double number)
        {
            throw new NotImplementedException();
        }
    }
}
