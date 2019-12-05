#if BUFFER_AND_SEQUENCE
using Jil;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.IO.Pipelines;
using System.Text;

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

        // todo: basically need to add everything here

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

        [Fact]
        public async Task EmptyPipeReaderAsync()
        {
            // only really care about UTFs
            var encodings =
                new[]
                {
                    Encoding.UTF8,
                    Encoding.Unicode,
                    Encoding.UTF32
                };

            foreach(var encoding in encodings)
            {
                var pipe = new Pipe();

                var bytes = encoding.GetBytes("");

                using (var adapter = new Jil.Deserialize.PipeReaderAdapter(pipe.Reader, encoding))
                {
                    var read = new List<char>();

                    await pipe.Writer.WriteAsync(bytes);
                    await pipe.Writer.CompleteAsync();

                    Assert.Equal(-1, adapter.Peek());

                    int i;
                    while ((i = adapter.Read()) != -1)
                    {
                        read.Add((char)i);
                    }

                    var actual = new string(read.ToArray());
                    Assert.Equal("", actual);
                }
            }
        }

        [Fact]
        public async Task PipeReaderAdapter()
        {
            var trickyStrings =
                new[]
                {
                    " ",
                    "hello world",
                    @"",
                    @"",
                    @"­؀؁؂؃؄؅؜۝܏᠎​‌‍‎‏‪‫‬‭‮⁠⁡⁢⁣⁤⁦⁧⁨⁩⁪⁫⁬⁭⁮⁯﻿￹￺￻𑂽𛲠𛲡𛲢𛲣𝅳𝅴𝅵𝅶𝅷𝅸𝅹𝅺󠀁󠀠󠀡󠀢󠀣󠀤󠀥󠀦󠀧󠀨󠀩󠀪󠀫󠀬󠀭󠀮󠀯󠀰󠀱󠀲󠀳󠀴󠀵󠀶󠀷󠀸󠀹󠀺󠀻󠀼󠀽󠀾󠀿󠁀󠁁󠁂󠁃󠁄󠁅󠁆󠁇󠁈󠁉󠁊󠁋󠁌󠁍󠁎󠁏󠁐󠁑󠁒󠁓󠁔󠁕󠁖󠁗󠁘󠁙󠁚󠁛󠁜󠁝󠁞󠁟󠁠󠁡󠁢󠁣󠁤󠁥󠁦󠁧󠁨󠁩󠁪󠁫󠁬󠁭󠁮󠁯󠁰󠁱󠁲󠁳󠁴󠁵󠁶󠁷󠁸󠁹󠁺󠁻󠁼󠁽󠁾󠁿",
                    @"ЁЂЃЄЅІЇЈЉЊЋЌЍЎЏАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя",
                    @"ด้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็ ด้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็ ด้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็",
                    @"田中さんにあげて下さい",
                    @"パーティーへ行かないか",
                    @"和製漢語",
                    @"사회과학원 어학연구소",
                    @"울란바토르",
                    @"𠜎𠜱𠝹𠱓𠱸𠲖𠳏",
                    @"表ポあA鷗ŒéＢ逍Üßªąñ丂㐀𠀀",
                    @"Ⱥ",
                    @"Ⱦ",
                    @"ヽ༼ຈل͜ຈ༽ﾉ ヽ༼ຈل͜ຈ༽ﾉ",
                    @"😍",
                    @"✋🏿 💪🏿 👐🏿 🙌🏿 👏🏿 🙏🏿",
                    @"🚾 🆒 🆓 🆕 🆖 🆗 🆙 🏧",
                    @"0️⃣ 1️⃣ 2️⃣ 3️⃣ 4️⃣ 5️⃣ 6️⃣ 7️⃣ 8️⃣ 9️⃣ 🔟",
                    @"🇺🇸🇷🇺🇸 🇦🇫🇦🇲🇸",
                    @"בְּרֵאשִׁית, בָּרָא אֱלֹהִים, אֵת הַשָּׁמַיִם, וְאֵת הָאָרֶץ",
                    @"הָיְתָהtestالصفحات التّحول",
                    @"﷽",
                    @"ﷺ",
                    @"مُنَاقَشَةُ سُبُلِ اِسْتِخْدَامِ اللُّغَةِ فِي النُّظُمِ الْقَائِمَةِ وَفِيم يَخُصَّ التَّطْبِيقَاتُ الْحاسُوبِيَّةُ، ",
                    @"˙ɐnbᴉlɐ ɐuƃɐɯ ǝɹolop ʇǝ ǝɹoqɐl ʇn ʇunpᴉpᴉɔuᴉ ɹodɯǝʇ poɯsnᴉǝ op pǝs 'ʇᴉlǝ ƃuᴉɔsᴉdᴉpɐ ɹnʇǝʇɔǝsuoɔ 'ʇǝɯɐ ʇᴉs ɹolop ɯnsdᴉ ɯǝɹo˥",
                    @"00˙Ɩ$-",
                    @"𝚃𝚑𝚎 𝚚𝚞𝚒𝚌𝚔 𝚋𝚛𝚘𝚠𝚗 𝚏𝚘𝚡 𝚓𝚞𝚖𝚙𝚜 𝚘𝚟𝚎𝚛 𝚝𝚑𝚎 𝚕𝚊𝚣𝚢 𝚍𝚘𝚐",
                    @"⒯⒣⒠ ⒬⒰⒤⒞⒦ ⒝⒭⒪⒲⒩ ⒡⒪⒳ ⒥⒰⒨⒫⒮ ⒪⒱⒠⒭ ⒯⒣⒠ ⒧⒜⒵⒴ ⒟⒪⒢"
                };

            // only really care about UTFs
            var encodings =
                new[]
                {
                    Encoding.UTF8,
                    Encoding.Unicode,
                    Encoding.UTF32
                };

            foreach (var encoding in encodings) 
            {
                var minToReadAfter = encoding.GetMaxByteCount(1);

                foreach (var str in trickyStrings)
                {
                    var bytes = encoding.GetBytes(str).AsMemory();
                    var pipe = new Pipe();

                    var firstHalf = bytes.Slice(0, bytes.Length / 2);
                    var secondHalf = bytes.Slice(bytes.Length / 2);

                    using (var adapter = new Jil.Deserialize.PipeReaderAdapter(pipe.Reader, encoding))
                    {
                        var read = new List<char>();

                        await pipe.Writer.WriteAsync(firstHalf);
                        if(firstHalf.Length >= minToReadAfter)
                        {
                            var r1 = adapter.Peek();
                            Assert.NotEqual(-1, r1);
                            var r2 = adapter.Read();
                            Assert.Equal(r1, r2);

                            read.Add((char)r2);
                        }
                        await pipe.Writer.WriteAsync(secondHalf);
                        await pipe.Writer.CompleteAsync();

                        int i;
                        while ((i = adapter.Read()) != -1)
                        {
                            read.Add((char)i);
                        }

                        var actual = new string(read.ToArray());
                        Assert.Equal(str, actual);
                    }
                }
            }
        }

        class _FromPipeReader
        {
            public string Foo { get; set; }
            public int Bar { get; set; }
        }

        [Fact]
        public async Task FromPipeReader()
        {
            const string JSON_STRING = "{\"Bar\":123,\"Foo\":\"hello\"}";
            var bytes = Encoding.UTF32.GetBytes(JSON_STRING);

            var pipe = new Pipe();
            await pipe.Writer.WriteAsync(bytes);
            await pipe.Writer.CompleteAsync();

            var res = await JSON.DeserializeAsync<_FromPipeReader>(pipe.Reader, Encoding.UTF32);

            Assert.NotNull(res);
            Assert.Equal("hello", res.Foo);
            Assert.Equal(123, res.Bar);
        }

        [Fact]
        public async Task FromPipeReaderDynamic()
        {
            const string JSON_STRING = "{\"Bar\":123,\"Foo\":\"hello\"}";
            var bytes = Encoding.UTF32.GetBytes(JSON_STRING);

            var pipe = new Pipe();
            await pipe.Writer.WriteAsync(bytes);
            await pipe.Writer.CompleteAsync();

            var res = await JSON.DeserializeDynamicAsync(pipe.Reader, Encoding.UTF32);

            Assert.NotNull(res);
            Assert.Equal("hello", (string)res.Foo);
            Assert.Equal(123, (int)res.Bar);
        }
    }
}
#endif