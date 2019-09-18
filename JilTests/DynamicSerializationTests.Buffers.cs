#if BUFFER_AND_SEQUENCE
using Jil;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Dynamic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace JilTests
{
    public partial class DynamicSerializationTests
    {
        private class CharWriter : IBufferWriter<char>
        {
            private readonly PipeWriter Inner;
            private IMemoryOwner<char> CurrentMemory;
            private Memory<char> LastMem;

            public CharWriter(PipeWriter inner)
            {
                Inner = inner;
            }

            public void Advance(int count)
            {
                var toStore = CurrentMemory.Memory.Slice(0, count);
                var asSpan = toStore.Span;
                var asByteSpan = MemoryMarshal.Cast<char, byte>(asSpan);

                var byteCount = count * sizeof(char) / sizeof(byte);
                var toCopyTo = Inner.GetMemory(byteCount);
                asByteSpan.CopyTo(toCopyTo.Span);
                Inner.Advance(byteCount);

                CurrentMemory.Dispose();
                CurrentMemory = null;
            }

            public Memory<char> GetMemory(int sizeHint = 0)
            {
                int rentSize = sizeHint == 0 ? -1 : sizeHint;
                CurrentMemory = MemoryPool<char>.Shared.Rent(rentSize);

                return CurrentMemory.Memory;
            }

            public Span<char> GetSpan(int sizeHint = 0)
            => GetMemory(sizeHint).Span;

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
        public async Task HeterogenousCollectionAsync()
        {
            {
                var dict = (dynamic)new ExpandoObject();
                dict.Fizz = "Buzz";
                var arr = new object[] { 123, "hello", new { Foo = "bar" }, dict };

                var pipe = new Pipe();
                JSON.SerializeDynamic(arr, new CharWriter(pipe.Writer));
                await pipe.Writer.FlushAsync();
                pipe.Writer.Complete();


                var res = await ReadToEndAsync(pipe.Reader);
                Assert.Equal("[123,\"hello\",{\"Foo\":\"bar\"},{\"Fizz\":\"Buzz\"}]", res);
            }
        }
    }
}
#endif