using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    struct ThunkReader
    {
        string Value;
        int Index;

        public ThunkReader(string val) : this()
        {
            Value = val;
            Index = -1;
        }

        public int Peek()
        {
            var ix = Index + 1;
            if(ix >= Value.Length)
            {
                return -1;
            }

            return Value[ix];
        }

        public int Read()
        {
            Index++;
            if(Index >= Value.Length)
            {
                return -1;
            }

            return Value[Index];
        }
    }
}
