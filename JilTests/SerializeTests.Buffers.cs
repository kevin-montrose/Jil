#if BUFFER_AND_SEQUENCE
using Jil;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        // todo: basically everything needs to be tested like this

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

        [Fact]
        public async Task EmptyPipeWriterAsync()
        {
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
                var pipe = new Pipe();

                var bytes = encoding.GetBytes("");

                var adapter = new Jil.Serialize.PipeWriterAdapter(pipe.Writer, encoding);
                {
                    var read = new List<byte>();

                    adapter.Write("");
                    adapter.Complete();

                    var buffer = new byte[15];

                    await pipe.Writer.FlushAsync();
                    await pipe.Writer.CompleteAsync();

                    using (var stream = pipe.Reader.AsStream())
                    {

                        int r;
                        while ((r = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            read.AddRange(buffer.Take(r));
                        }
                    }

                    var expected = encoding.GetBytes("");
                    Assert.True(expected.SequenceEqual(read));
                }
            }
        }

        [Fact]
        public async Task PipeWriterAdapter()
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
                var readAfter = encoding.GetMaxByteCount(1);

                foreach (var str in trickyStrings)
                {
                    var pipe = new Pipe();

                    var firstHalf = str.Substring(0, str.Length / 2);
                    var secondHalf = str.Substring(str.Length / 2);

                    var adapter = new Jil.Serialize.PipeWriterAdapter(pipe.Writer, encoding);

                    var read = new List<byte>();

                    using (var stream = pipe.Reader.AsStream())
                    {
                        var buffer = new byte[15];

                        adapter.Write(firstHalf);
                        adapter.Write(secondHalf);
                        adapter.Complete();
                        await pipe.Writer.FlushAsync();
                        await pipe.Writer.CompleteAsync();

                        int r;
                        while ((r = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            read.AddRange(buffer.Take(r));
                        }
                    }

                    var expected = encoding.GetBytes(str);
                    Assert.True(expected.SequenceEqual(read));
                }
            }
        }

        class _ToPipeWriter
        {
            public string Foo { get; set; }
            public int Bar { get; set; }
        }

        [Fact]
        public async Task ToPipeWriter()
        {
            var pipe = new Pipe();

            await JSON.SerializeAsync(new _ToPipeWriter { Foo = "hello", Bar = 123 }, pipe.Writer, Encoding.UTF8);
            await pipe.Writer.CompleteAsync();

            var bytes = new List<byte>();

            var completed = false;

            while (!completed)
            {
                var res = await pipe.Reader.ReadAsync();
                foreach(var seq in res.Buffer)
                {
                    bytes.AddRange(seq.ToArray());
                }

                if(res.IsCanceled || res.IsCompleted)
                {
                    completed = true;
                }
            }

            var str = Encoding.UTF8.GetString(bytes.ToArray());

            Assert.Equal("{\"Bar\":123,\"Foo\":\"hello\"}", str);
        }

        [Fact]
        public async Task ToPipeWriterDynamic()
        {
            var pipe = new Pipe();

            object obj = new _ToPipeWriter { Foo = "hello", Bar = 123 };

            await JSON.SerializeDynamicAsync(obj, pipe.Writer, Encoding.UTF8);
            await pipe.Writer.CompleteAsync();

            var bytes = new List<byte>();

            var completed = false;

            while (!completed)
            {
                var res = await pipe.Reader.ReadAsync();
                foreach (var seq in res.Buffer)
                {
                    bytes.AddRange(seq.ToArray());
                }

                if (res.IsCanceled || res.IsCompleted)
                {
                    completed = true;
                }
            }

            var str = Encoding.UTF8.GetString(bytes.ToArray());

            Assert.Equal("{\"Bar\":123,\"Foo\":\"hello\"}", str);
        }
    }
}
#endif
