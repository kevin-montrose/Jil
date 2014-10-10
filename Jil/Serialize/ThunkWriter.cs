using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    delegate void StringThunkDelegate<T>(ref ThunkWriter writer, T data, int depth);

    struct ThunkWriter
    {
        const int InitialSize = 128;

        int Index;
        char[] Builder;

        public void Init()
        {
            Index = 0;
            Builder = new char[InitialSize];
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

        void Expand(int adding)
        {
            var mustBeAtLeast = Index + adding;
            if (mustBeAtLeast >= Builder.Length)
            {
                var newBuilder = new char[Builder.Length * 2];
                Builder.CopyTo(newBuilder, 0);
                Builder = newBuilder;
            }
        }

        public void Write(char[] ch, int startIx, int len)
        {
            Expand(len);

            Array.Copy(ch, startIx, Builder, Index, len);

            Index += len;
        }

        public void Write(char ch)
        {
            Expand(1);
            Builder[Index] = ch;
            Index++;
        }

        public unsafe void Write(string strRef)
        {
            var len = strRef.Length;
            if (len == 0) return;

            Expand(len);

            fixed (char* strPtr = strRef)
            {
                var str = strPtr;
                for (var i = 0; i < len; i++)
                {
                    Builder[Index] = *str;
                    str++;
                    Index++;
                }
            }
        }

        public string StaticToString()
        {
            return new string(Builder, 0, Index);
        }

        #region Slow Builds Only
        // these methods are only called to compare faster methods to serializing
        //   as such they need not be optimized

        public void Write(byte b)
        {
            Write(b.ToString());
        }
        public void Write(sbyte b)
        {
            Write(b.ToString());
        }
        public void Write(short b)
        {
            Write(b.ToString());
        }
        public void Write(ushort b)
        {
            Write(b.ToString());
        }
        public void Write(int b)
        {
            Write(b.ToString());
        }
        public void Write(uint b)
        {
            Write(b.ToString());
        }
        public void Write(long b)
        {
            Write(b.ToString());
        }
        public void Write(ulong b)
        {
            Write(b.ToString());
        }
        #endregion
    }
}
