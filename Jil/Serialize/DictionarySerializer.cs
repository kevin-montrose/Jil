using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    public class DictionarySerializer
    {
        private static readonly string[] Spacer = new[] { "", "," };

        public static void Serialize<T>(TextWriter writer, Dictionary<string, T> dict)
        {
            if (dict == null)
            {
                writer.Write("null");
                return;
            }

            var e = dict.GetEnumerator();

            if (e.MoveNext())
            {
                writer.Write("{\"");
            }
            else
            {
                writer.Write("{}");
                return;
            }

            int i = 0;
            
            do
            {
                writer.Write(Spacer[i]);

                var kv = e.Current;

                writer.Write(kv.Key);
                writer.Write("\":");

                TypeCache<T>.Thunk(writer, kv.Value);

                i = i | 1;
            } while (e.MoveNext());

            writer.Write("}");
        }
    }
}
