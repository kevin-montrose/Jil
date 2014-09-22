using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    delegate string StringThunkDelegate<T>(ref ThunkWriter writer, T data, int depth);

    struct ThunkWriter
    {
        public void Write(float f)
        {
            Write(f.ToString(CultureInfo.InvariantCulture));
        }

        public void Write(double d)
        {
            Write(d.ToString(CultureInfo.InvariantCulture));
        }

        public void Write(decimal m)
        {
            Write(m.ToString(CultureInfo.InvariantCulture));
        }

        public void Write(char[] ch, int startIx, int len)
        {
            throw new NotImplementedException();
        }

        public void Write(char ch)
        {
            throw new NotImplementedException();
        }

        public void Write(string str)
        {
            throw new NotImplementedException();
        }

        public string StaticToString()
        {
            throw new NotImplementedException();
        }
    }
}
