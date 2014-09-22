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
        StringBuilder Builder;

        public void Init()
        {
            Builder = new StringBuilder();
        }

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
            Builder.Append(ch, startIx, len);
        }

        public void Write(char ch)
        {
            Builder.Append(ch);
        }

        public void Write(string str)
        {
            Builder.Append(str);
        }

        public string StaticToString()
        {
            return Builder.ToString();
        }
    }
}
