#if BUFFER_AND_SEQUENCE
using Jil;
using System;
using System.Buffers;
using System.Collections.Generic;
using Xunit;

namespace JilTests
{
    public partial class DeserializeTests
    {
        private sealed class Node : ReadOnlySequenceSegment<char>
        {
            public Node(ReadOnlyMemory<char> mem, int charsUsed)
            {
                this.Memory = mem.Slice(0, charsUsed);
                this.RunningIndex = 0;
                this.Next = null;
            }

            public Node Append(ReadOnlyMemory<char> mem, int charsUsed)
            {
                var ret = new Node(mem, charsUsed);
                ret.RunningIndex = this.RunningIndex + this.Memory.Length;
                this.Next = ret;

                return ret;
            }
        }

        private static ReadOnlySequence<char> AsSequence(string raw, bool forceSingle)
        {
            var r = new Random();
            var len = raw.Length;
            var divs = new List<ReadOnlyMemory<char>>();
            if (!forceSingle)
            {
            tryAgain:
                var ix = 0;
                while (ix < len)
                {
                    var left = len - ix;
                    var take = r.Next(left);
                    if (take == 0) take = 1;

                    var part = raw.Substring(ix, take).AsMemory();
                    divs.Add(part);

                    ix += take;
                }

                if (divs.Count == 1)
                {
                    divs.Clear();
                    goto tryAgain;
                }

                var firstMem = divs[0];
                var firstSizedMem = RandomlyResize(firstMem);

                var first = new Node(firstSizedMem, divs[0].Length);
                Node end = first;

                for (var i = 1; i < divs.Count; i++)
                {
                    var nextMem = divs[i];
                    var nextSizedMem = RandomlyResize(nextMem);
                    end = end.Append(nextSizedMem, nextMem.Length);
                }

                return new ReadOnlySequence<char>(first, 0, end, end.Memory.Length);
            }
            else
            {
                return new ReadOnlySequence<char>(raw.AsMemory());
            }

            ReadOnlyMemory<char> RandomlyResize(ReadOnlyMemory<char> mem)
            {
                if (r.Next() % 2 == 0)
                {
                    var larger = MemoryPool<char>.Shared.Rent(mem.Length * 2);
                    var newMem = larger.Memory;
                    mem.CopyTo(newMem);

                    return newMem;
                }

                return mem;
            }
        }

        [Fact]
        public void ValueTypesSequence()
        {
            foreach (var single in new[] { true, false })
            {
                var seq = AsSequence("{\"A\":\"hello\\u0000world\", \"B\":12345}", forceSingle: single);
                var res = JSON.Deserialize<_ValueTypes>(seq);
                Assert.Equal("hello\0world", res.A);
                Assert.Equal(12345, res.B);
            }
        }

        [Fact]
        public void LargeCharBufferSequence()
        {
            foreach (var single in new[] { true, false })
            {
                var seq = AsSequence("{\"Date\": \"2013-12-30T04:17:21Z\", \"String\": \"hello world\"}", forceSingle: single);
                var res = JSON.Deserialize<_LargeCharBuffer>(seq, Options.ISO8601);
                Assert.Equal(new DateTime(2013, 12, 30, 4, 17, 21, DateTimeKind.Utc), res.Date);
                Assert.Equal("hello world", res.String);
            }
        }

        [Fact]
        public void SmallCharBufferSequence()
        {
            foreach (var single in new[] { true, false })
            {
                var seq = AsSequence("{\"Date\": 1388377041, \"String\": \"hello world\"}", forceSingle: single);
                var res = JSON.Deserialize<_SmallCharBuffer>(seq, Options.SecondsSinceUnixEpoch);
                Assert.Equal(new DateTime(2013, 12, 30, 4, 17, 21, DateTimeKind.Utc), res.Date);
                Assert.Equal("hello world", res.String);
            }
        }

        [Fact]
        public void IDictionaryIntToIntSequence()
        {
            foreach (var single in new[] { true, false })
            {
                var seq = AsSequence("{\"1\":2, \"3\":4, \"5\": 6}", forceSingle: single);
                var res = JSON.Deserialize<IDictionary<int, int>>(seq);
                Assert.Equal(3, res.Count);
                Assert.Equal(2, res[1]);
                Assert.Equal(4, res[3]);
                Assert.Equal(6, res[5]);
            }
        }
    }
}
#endif