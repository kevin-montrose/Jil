#if BUFFER_AND_SEQUENCE
using Jil;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace JilTests
{
    public partial class SerializeTests
    {
        private class CharWriter : IBufferWriter<char>
        {
            private readonly PipeWriter Inner;

            public CharWriter(PipeWriter inner)
            {
                Inner = inner;
            }

            public void Advance(int count)
            => Inner.Advance(count * sizeof(char));

            public Memory<char> GetMemory(int sizeHint = 0)
            => throw new NotImplementedException();

            public Span<char> GetSpan(int sizeHint = 0)
            {
                var bytes = Inner.GetSpan(sizeHint * sizeof(char));
                var chars = MemoryMarshal.Cast<byte, char>(bytes);

                return chars;
            }

            public ValueTask<FlushResult> FlushAsync()
            => Inner.FlushAsync();
        }

        private static async Task<string> ReadToEndAsync(PipeReader reader)
        {
            var bytes = new List<byte>();

            while (true)
            {
                var res = await reader.ReadAsync();

                foreach (var seq in res.Buffer)
                {
                    bytes.AddRange(seq.ToArray());
                }

                reader.AdvanceTo(res.Buffer.End);

                if (res.IsCompleted) break;
            }

            return FromBytes(bytes);
        }

        private static string FromBytes(IEnumerable<byte> bs)
        {
            var arr = bs.ToArray();
            var byteSpan = arr.AsSpan();
            var charSpan = MemoryMarshal.Cast<byte, char>(byteSpan);
            return new string(charSpan);
        }

        [Fact]
        public async Task SerializeToBufferAsync()
        {
            var pipe = new Pipe();
            JSON.Serialize(new { Foo = 123 }, new CharWriter(pipe.Writer));
            await pipe.Writer.FlushAsync();
            pipe.Writer.Complete();
            
            var res = await ReadToEndAsync(pipe.Reader);
            Assert.Equal("{\"Foo\":123}", res);
        }
    }
}
#endif
