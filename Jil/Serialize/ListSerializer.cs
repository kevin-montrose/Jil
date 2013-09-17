using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    public class ListSerializer
    {
        private static readonly string[] Spacer = new[] { "", "," };

        public static void Serialize<T>(TextWriter writer, IList<T> list)
        {
            if (list == null)
            {
                writer.Write("null");
                return;
            }

            var e = list.GetEnumerator();

            if (e.MoveNext())
            {
                writer.Write("[");
            }
            else
            {
                writer.Write("[]");
                return;
            }

            int i = 0;

            do
            {
                writer.Write(Spacer[i]);

                var val = e.Current;

                TypeCache<T>.Thunk(writer, val);

                i = i | 1;
            } while (e.MoveNext());

            writer.Write("]");
        }
    }
}
