using Jil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Globalization;
using Xunit;

namespace JilTests
{
    public class DeserializeDynamicTests
    {
        [Fact]
        public void Bool()
        {
            using (var str = new StringReader("true"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.True((bool)res);
            }

            using (var str = new StringReader("false"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.False((bool)res);
            }
        }

        [Fact]
        public void Null()
        {
            using (var str = new StringReader("null"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Null(res);
            }
        }

        [Fact]
        public void Guids()
        {
            var guid = Guid.NewGuid();

            using (var str = new StringReader("\"" + guid + "\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(guid, (Guid)res);
            }
        }

        enum _Enums
        {
            Hello,
            World
        }

        [Fact]
        public void Enums()
        {
            using (var str = new StringReader("\"Hello\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(_Enums.Hello, (_Enums)res);
            }

            using (var str = new StringReader("\"hello\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(_Enums.Hello, (_Enums)res);
            }

            using (var str = new StringReader("\"World\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(_Enums.World, (_Enums)res);
            }

            using (var str = new StringReader("\"world\""))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(_Enums.World, (_Enums)res);
            }
        }

        [Fact]
        public void Number()
        {
            using (var str = new StringReader("1"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.Equal((1.0).ToString(), val.ToString());
            }

            using (var str = new StringReader("1.234"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.Equal((1.234).ToString(), val.ToString());
            }

            using (var str = new StringReader("-10e4"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.Equal((-100000).ToString(), val.ToString());
            }

            using (var str = new StringReader("-1.3e-4"))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (double)res;
                Assert.Equal((-0.00013).ToString(), val.ToString());
            }
        }

        [Fact]
        public void String()
        {
            using (var str = new StringReader("\"hello world\""))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (string)res;
                Assert.Equal("hello world", val);
            }

            using (var str = new StringReader("\"H\\u0065llo\""))
            {
                var res = JSON.DeserializeDynamic(str);
                var val = (string)res;
                Assert.Equal("Hello", val);
            }
        }

        [Fact]
        public void Array()
        {
            using (var str = new StringReader("[123, \"hello\", true]"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(3, (int)res.Length);
                Assert.Equal(123, (int)res[0]);
                Assert.Equal("hello", (string)res[1]);
                Assert.True((bool)res[2]);
            }

            using (var str = new StringReader("[]"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(0, (int)res.Length);
            }
        }

        [Fact]
        public void Object()
        {
            using (var str = new StringReader("{\"hello\": 123, \"world\":[1,2,3]}"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.Equal(123, (int)res["hello"]);
                var arr = res["world"];
                Assert.Equal(3, (int)arr.Length);
                Assert.Equal(1, (int)arr[0]);
                Assert.Equal(2, (int)arr[1]);
                Assert.Equal(3, (int)arr[2]);
            }

            using (var str = new StringReader("{}"))
            {
                var res = JSON.DeserializeDynamic(str);
                Assert.NotNull(res);
                var c = 0;
                foreach (var x in res)
                {
                    c++;
                }
                Assert.Equal(0, c);
            }
        }

        [Fact]
        public void ObjectEnumeration()
        {
            using (var str = new StringReader("{\"hello\":123, \"world\":456, \"foo\":789}"))
            {
                var c = 0;
                var res = JSON.DeserializeDynamic(str);
                foreach (var kv in res)
                {
                    string key = kv.Key;
                    dynamic val = kv.Value;

                    switch(c){
                        case 0: 
                            Assert.Equal("hello", key);
                            Assert.Equal(123, (int)val);
                            break;
                        case 1:
                            Assert.Equal("world", key);
                            Assert.Equal(456, (int)val);
                            break;
                        case 2:
                            Assert.Equal("foo", key);
                            Assert.Equal(789, (int)val);
                            break;
                        default: throw new Exception();
                    }
                    c++;
                }
                Assert.Equal(3, c);
            }
        }

        [Fact]
        public void ArrayEnumerator()
        {
            using (var str = new StringReader("[\"abcd\", \"efgh\", \"ijkl\"]"))
            {
                var c = 0;
                var res = JSON.DeserializeDynamic(str);
                foreach (var val in res)
                {
                    switch (c)
                    {
                        case 0:
                            Assert.Equal("abcd", (string)val);
                            break;
                        case 1:
                            Assert.Equal("efgh", (string)val);
                            break;
                        case 2:
                            Assert.Equal("ijkl", (string)val);
                            break;
                        default: throw new Exception();
                    }
                    c++;
                }
                Assert.Equal(3, c);
            }
        }

        [Fact]
        public void ArrayCoercion()
        {
            using (var str = new StringReader("[123, 456, 789]"))
            {
                var res = JSON.DeserializeDynamic(str);
                IEnumerable<int> asArr = res;
                Assert.Equal(3, asArr.Count());
                Assert.Equal(123, asArr.ElementAt(0));
                Assert.Equal(456, asArr.ElementAt(1));
                Assert.Equal(789, asArr.ElementAt(2));
            }
        }

        enum _DictionaryCoercion
        {
            a,
            b,
            c
        }

        [Fact]
        public void DictionaryCoercion()
        {
            using (var str = new StringReader("{\"a\": 123, \"b\": 456, \"c\": 789}"))
            {
                var res = JSON.DeserializeDynamic(str);
                IDictionary<string, int> asDict = res;
                Assert.Equal(3, asDict.Count());
                Assert.True(asDict.ContainsKey("a"));
                Assert.Equal(123, asDict["a"]);
                Assert.True(asDict.ContainsKey("b"));
                Assert.Equal(456, asDict["b"]);
                Assert.True(asDict.ContainsKey("c"));
                Assert.Equal(789, asDict["c"]);
            }

            using (var str = new StringReader("{\"a\": 123, \"b\": 456, \"c\": 789}"))
            {
                var res = JSON.DeserializeDynamic(str);
                IDictionary<_DictionaryCoercion, int> asDict = res;
                Assert.Equal(3, asDict.Count());
                Assert.True(asDict.ContainsKey(_DictionaryCoercion.a));
                Assert.Equal(123, asDict[_DictionaryCoercion.a]);
                Assert.True(asDict.ContainsKey(_DictionaryCoercion.b));
                Assert.Equal(456, asDict[_DictionaryCoercion.b]);
                Assert.True(asDict.ContainsKey(_DictionaryCoercion.c));
                Assert.Equal(789, asDict[_DictionaryCoercion.c]);
            }
        }

        [Fact]
        public void TrickyNumbers()
        {
            {
                ulong ul = 10000000002342929928UL;
                var dyn = JSON.DeserializeDynamic(ul.ToString());
                var asULong = (ulong)dyn;
                Assert.Equal(ul, asULong);
            }

            {
                uint i = 1276679976;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("R", CultureInfo.InvariantCulture), Format = "R", I = i });
            }

            {
                uint i = 1343554351;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("R", CultureInfo.InvariantCulture), Format = "R", I = i });
            }

            {
                uint i = 1593835550;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("F", CultureInfo.InvariantCulture), Format = "F", I = i });
            }

            {
                uint i = 1602224307;
                var f = ULongToFloat(i, new byte[4]);
                CheckFloat(new _AllFloatsStruct { Float = f, AsString = f.ToString("F", CultureInfo.InvariantCulture), Format = "F", I = i });
            }
        }

        [Fact]
        public void SignedSmallNumberTests()
        {
            for (long i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                try
                {
                    var asNum = (sbyte)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (sbyte)dyn;
                        Assert.True(asNum == v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (sbyte)i, e);
                }
            }

            for (long i = short.MinValue; i <= short.MaxValue; i++)
            {
                try
                {
                    var asNum = (short)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (short)dyn;
                        Assert.True(asNum == v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (short)i, e);
                }
            }
        }

        [Fact]
        public void UnsignedSmallNumberTests()
        {
            for (long i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                try
                {
                    var asNum = (byte)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (byte)dyn;
                        Assert.True(asNum == v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (byte)i, e);
                }
            }

            for (long i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                try
                {
                    var asNum = (ushort)i;
                    using (var str = new StringReader(asNum.ToString()))
                    {
                        var dyn = JSON.DeserializeDynamic(str);
                        var v = (ushort)dyn;
                        Assert.True(asNum == v, "Failed on i=" + asNum);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed on i = " + (ushort)i, e);
                }
            }
        }

        internal struct _AllFloatsStruct
        {
            public float Float;
            public string AsString;
            public string Format;
            public uint I;
        }

        internal static float ULongToFloat(ulong i, byte[] byteArr)
        {
            var asInt = (uint)i;
            byteArr[0] = (byte)((asInt) & 0xFF);
            byteArr[1] = (byte)((asInt >> 8) & 0xFF);
            byteArr[2] = (byte)((asInt >> 16) & 0xFF);
            byteArr[3] = (byte)((asInt >> 24) & 0xFF);
            var f = BitConverter.ToSingle(byteArr, 0);

            return f;
        }

        internal static void CheckFloat(_AllFloatsStruct part)
        {
            var i = part.I;
            var format = part.Format;
            var asStr = part.AsString;
            var dyn = JSON.DeserializeDynamic(asStr);
            var res = (float)dyn;
            var reStr = res.ToString(format);

            var delta = Math.Abs((float.Parse(asStr) - float.Parse(reStr)));

            var closeEnough = part.Float.ToString() == res.ToString() || asStr == reStr || delta <= float.Epsilon;

            Assert.True(closeEnough, "For i=" + i + " format=" + format + " delta=" + delta + " epsilon=" + float.Epsilon);
        }

        [Fact]
        public void ParseISO8601()
        {
            using (var str = new StringReader("\"1900\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1991-02\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1991, 02, 1, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }
            using (var str = new StringReader("\"19890131\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12,5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T12.5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12,5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T12.5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234,5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T1234.5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456,5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"19890131T123456.5Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1989-01-31T12+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 00, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5+01:23\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12,5+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12.5+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234,5+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234.5+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456,5+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456.5+0123\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12,5-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12.5-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5-11:45\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12,5-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T12.5-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234,5-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T1234.5-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456,5-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"19890131T123456.5-1145\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch.UtcDateTime, dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30Z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01t12:30+00\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"1900-01-01 12:30z\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dt);
            }

            using (var str = new StringReader("\"2004-366\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }

            using (var str = new StringReader("\"2004366\""))
            {
                DateTime dt = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dt);
            }
        }

        [Fact]
        public void ISO8601DateTimeOffsets()
        {
            using (var str = new StringReader("\"1900\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1991-02\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1991, 02, 1, 0, 0, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dto);
            }
            using (var str = new StringReader("\"19890131\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 0, 0, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12,5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12.5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T12\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T12,5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T12.5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T1234\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T1234,5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T1234.5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T123456\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T123456,5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"19890131T123456.5\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12,5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12.5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T12Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 0, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T12,5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T12.5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 30, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T1234Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T1234,5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T1234.5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 30, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T123456Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T123456,5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"19890131T123456.5Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal(new DateTime(1989, 01, 31, 12, 34, 56, 500, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1989-01-31T12+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 00, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12,5+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12.5+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5+01:23\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T12+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T12,5+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T12.5+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T1234+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T1234,5+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T1234.5+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T123456+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T123456,5+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T123456.5+0123\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, new TimeSpan(01, 23, 00));
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12,5-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12.5-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34,5-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34.5-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56,5-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1989-01-31T12:34:56.5-11:45\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T12-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 0, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T12,5-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T12.5-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 30, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T1234-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 0, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T1234,5-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T1234.5-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 30, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T123456-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T123456,5-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"19890131T123456.5-1145\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                var shouldMatch = new DateTimeOffset(1989, 01, 31, 12, 34, 56, 500, (new TimeSpan(11, 45, 0)).Negate());
                Assert.Equal(shouldMatch, dto);
            }

            using (var str = new StringReader("\"1900-01-01 12:30Z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1900-01-01t12:30+00\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"1900-01-01 12:30z\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(1900, 01, 01, 12, 30, 0, DateTimeKind.Utc), dto);
            }

            using (var str = new StringReader("\"2004-366\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dto);
            }

            using (var str = new StringReader("\"2004366\""))
            {
                DateTimeOffset dto = JSON.DeserializeDynamic(str, Options.ISO8601);
                Assert.Equal((DateTimeOffset)new DateTime(2004, 12, 31, 0, 0, 0, DateTimeKind.Local), dto);
            }
        }

        [Fact]
        public void Issue143DateTime() 
        {
            var date = new DateTime(21, DateTimeKind.Utc);
            var str = JSON.Serialize(date, Options.ISO8601);
            var result = (DateTime)JSON.DeserializeDynamic(str, Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);
        }

        [Fact]
        public void Issue143DateTimeFractionOverflow() 
        {
            var date = new DateTime(21, DateTimeKind.Utc);
            var str = "\"0001-01-01T00:00:00.000002100001Z\"";
            var result = (DateTime)JSON.DeserializeDynamic(str, Options.ISO8601);
            Assert.Equal(date.Ticks, result.Ticks);
        }

        [Fact]
        public void Issue143TimeSpan() 
        {
            var span = new TimeSpan(21);
            var str = JSON.Serialize(span, Options.ISO8601);
            var result = (TimeSpan)JSON.DeserializeDynamic(str, Options.ISO8601);
            Assert.Equal(span.Ticks, result.Ticks);
        }

        [Fact]
        public void Issue143TimeSpanFractionOverflow() 
        {
            var span = new TimeSpan(21);
            var str = "\"PT0.000002100001S\"";
            var result = (TimeSpan)JSON.DeserializeDynamic(str, Options.ISO8601);
            Assert.Equal(span.Ticks, result.Ticks);
        }

        [Fact]
        public void Issue143DateTimeOffset() 
        {
            var offset = new DateTimeOffset(new DateTime(21, DateTimeKind.Utc), TimeSpan.Zero);
            var str = JSON.Serialize(offset, Options.ISO8601);
            var result = (DateTimeOffset)JSON.DeserializeDynamic(str, Options.ISO8601);
            Assert.Equal(offset.Ticks, result.Ticks);
        }


        [Fact]
        public void Issue143DateTimeOffsetFractionOverflow() 
        {
            var offset = new DateTimeOffset(new DateTime(21, DateTimeKind.Utc), TimeSpan.Zero);
            var str = "\"0001-01-01T00:00:00.000002100001Z\"";
            var result = (DateTimeOffset)JSON.DeserializeDynamic(str, Options.ISO8601);
            Assert.Equal(offset.Ticks, result.Ticks);
        }

        [Fact]
        public void SecondDateTimes()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.UtcNow;

            var dtStr = JSON.Serialize(dt, Options.SecondsSinceUnixEpoch);
            var nowStr = JSON.Serialize(now, Options.SecondsSinceUnixEpoch);

            using (var str = new StringReader(dtStr))
            {
                var dtRet = JSON.DeserializeDynamic(str, Options.SecondsSinceUnixEpoch);
                var delta = ((DateTime)dtRet - dt).Duration().TotalSeconds;
                Assert.True(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.DeserializeDynamic(str, Options.SecondsSinceUnixEpoch);
                var delta = ((DateTime)nowRet - now).Duration().TotalSeconds;
                Assert.True(delta < 1);
            }
        }

        [Fact]
        public void MillisecondDateTimes()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.UtcNow;

            var dtStr = JSON.Serialize(dt, Options.MillisecondsSinceUnixEpoch);
            var nowStr = JSON.Serialize(now, Options.MillisecondsSinceUnixEpoch);

            using (var str = new StringReader(dtStr))
            {
                var dtRet = JSON.DeserializeDynamic(str, Options.MillisecondsSinceUnixEpoch);
                var delta = ((DateTime)dtRet - dt).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.DeserializeDynamic(str, Options.MillisecondsSinceUnixEpoch);
                var delta = ((DateTime)nowRet - now).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }
        }

        [Fact]
        public void NewtsoftDateTimes()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.UtcNow;

            var dtStr = JSON.Serialize(dt);
            var nowStr = JSON.Serialize(now);

            using (var str = new StringReader(dtStr))
            {
                var dtRet = JSON.DeserializeDynamic(str);
                var delta = ((DateTime)dtRet - dt).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.DeserializeDynamic(str);
                var delta = ((DateTime)nowRet - now).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }
        }

        [Fact]
        public void NewtsoftDateTimeOffsets()
        {
            var dt = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var now = DateTimeOffset.UtcNow;

            var dtStr = JSON.Serialize(dt);
            var nowStr = JSON.Serialize(now);

            using (var str = new StringReader(dtStr))
            {
                var dtRet = JSON.DeserializeDynamic(str);
                var delta = ((DateTimeOffset)dtRet - dt).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }

            using (var str = new StringReader(nowStr))
            {
                var nowRet = JSON.DeserializeDynamic(str);
                var delta = ((DateTimeOffset)nowRet - now).Duration().TotalMilliseconds;
                Assert.True(delta < 1);
            }
        }

        [Fact]
        public void MicrosoftDateTimesWithTimeZones()
        {
            var newtonsoft = Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings
            {
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat
            });

            var now = DateTime.UtcNow;

            for (var i = 0; i < 100000; i++)
            {
                var dtUtc = now + TimeSpan.FromMilliseconds(i);
                var dtLocal = dtUtc.ToLocalTime();

                string asStr;
                using (var str = new StringWriter())
                {
                    newtonsoft.Serialize(str, dtLocal);
                    asStr = str.ToString();
                    Assert.True(asStr.Contains('-') || asStr.Contains('+'));
                }

                DateTime shouldMatch, shouldMatchUtc;
                using (var str = new StringReader(asStr))
                {
                    shouldMatch = (DateTime)newtonsoft.Deserialize(str, typeof(DateTime));
                    shouldMatchUtc = shouldMatch.ToUniversalTime();
                }

                DateTime jilDt, jilDtUtc;
                using (var str = new StringReader(asStr))
                {
                    var dyn = JSON.DeserializeDynamic(str);
                    jilDt = dyn;
                    jilDtUtc = ((DateTime)jilDt).ToUniversalTime();
                }

                Assert.True((dtUtc - shouldMatchUtc).Duration().TotalMilliseconds < 1);
                Assert.True((dtUtc - jilDtUtc).Duration().TotalMilliseconds < 1);
                Assert.True((shouldMatchUtc - jilDtUtc).Duration().TotalMilliseconds == 0);
            }
        }

        [Fact]
        public void IEnumerableDynamic()
        {
            using (var str = new StringReader("[1, 2, 3.3, \"hello\"]"))
            {
                var res = JSON.DeserializeDynamic(str);
                var ie = (IEnumerable<dynamic>)res;
                Assert.Equal(4, ie.Count());
                Assert.Equal(1, (int)ie.ElementAt(0));
                Assert.Equal(2, (int)ie.ElementAt(1));
                Assert.Equal(3.3f, (float)ie.ElementAt(2));
                Assert.Equal("hello", (string)ie.ElementAt(3));
            }

            using (var str = new StringReader("{\"a\": [1, 2, 3.3, \"hello\"]}"))
            {
                var res = JSON.DeserializeDynamic(str);
                var ie = (IEnumerable<dynamic>)res.a;
                Assert.Equal(4, ie.Count());
                Assert.Equal(1, (int)ie.ElementAt(0));
                Assert.Equal(2, (int)ie.ElementAt(1));
                Assert.Equal(3.3f, (float)ie.ElementAt(2));
                Assert.Equal("hello", (string)ie.ElementAt(3));
            }
        }

        [Fact]
        public void Nullables()
        {
            using (var str = new StringReader("[null, 1]"))
            {
                var res = (IEnumerable<dynamic>)JSON.DeserializeDynamic(str);
                Assert.Equal((byte?)null, (byte?)res.ElementAt(0));
                Assert.Equal((byte?)1, (byte?)res.ElementAt(1));
                Assert.Equal((ushort?)null, (ushort?)res.ElementAt(0));
                Assert.Equal((ushort?)1, (ushort?)res.ElementAt(1));
                Assert.Equal((uint?)null, (uint?)res.ElementAt(0));
                Assert.Equal((uint?)1, (uint?)res.ElementAt(1));
                Assert.Equal((ulong?)null, (ulong?)res.ElementAt(0));
                Assert.Equal((ulong?)1, (ulong?)res.ElementAt(1));
            }

            using (var str = new StringReader("[null, -1]"))
            {
                var res = (IEnumerable<dynamic>)JSON.DeserializeDynamic(str);
                Assert.Equal((sbyte?)null, (sbyte?)res.ElementAt(0));
                Assert.Equal((sbyte?)-1, (sbyte?)res.ElementAt(1));
                Assert.Equal((short?)null, (short?)res.ElementAt(0));
                Assert.Equal((short?)-1, (short?)res.ElementAt(1));
                Assert.Equal((int?)null, (int?)res.ElementAt(0));
                Assert.Equal((int?)-1, (int?)res.ElementAt(1));
                Assert.Equal((long?)null, (long?)res.ElementAt(0));
                Assert.Equal((long?)-1, (long?)res.ElementAt(1));
            }
        }

#if NET45
        [Fact]
        public void BenchmarkFailure()
        {
            var json = "{\"tags\":[\"䠌-eX?R?59)Dm㞻'Gx3_t&]5큥g78**!%\\u000B車\\n\\\"j3Y2S?;cWorWgll)LjMGL軍ejq\\fbUfN騏:4\\u000Bj(%Lꌸ䃷3v8PIt9\\n郲x1\\t&eW8lk呴 qd튯m8&Mx(푐m;Gz0hx[\\r]웓䪡#\\nd갂Gwh7레LMPy죕pJp塥/zk釆%Nh\\\\츟VCxX6*\\t\\t\\u000BvA!?枉my/XZJ1KGpelKM%\\u000B딸RaRzDewe7X,[/tbJ\\tM-zh(eXUCRQydPpKWJZ莹3D㩠\\f1v/jmyP웪鹱f\\ta왕ldW!4Rf㑗/Mᆆb\\t1\\\\?_']?s\\fH揖K\\u000B焃So?@Xzdjr\\f䕶([\\\\N\\n耠殧rmf\\rgU9\\nꈽQr2[爜zs*pDJk23\\f7Il\\fVhkwf,dHXbg)8n\\rSg/uo픠\",\"V7:NZ뉄OCshxb[/콀zk給\\\"Szq]\\n\\fᓉ\\\\-f&//H\\f8J*qWZj:UrcB.t菾齏鳧oJX@\\r.qac텡KrDgiᔬ#Hdh戬Px*GK䦈STXyv40?Mm?Z4\\\\eSZ㣻ca(LuLCwn(a\\fpx;Q哽;\\u000BL1E\\rn갏c麜g9uPKVG\\nfx6?:䬎ydt-le;1tGoU10zWApXt:c7\\rd2೪㪁\\\\帓-ᡮNi4m뇌\\\"\\rQ\\\"MB\\t69#쐧RῼWs\\\"Gb*Wz蕼\\n9?uF,4奦_*鏚ES犐U:#枉R2!U%%!XP;BYuV赤qE-\\u000BY!_EF[P/k]\\r.D\\ri賻h2Jv6\",\"6[Ld3GpoO銣*:;&ips\\\"S8;Y;(j薨[gC,yK5WO(my06I S鄐RhFmw[\\\\539HzYUz쮹鍺RYrG\\\\%XM*젋/_]UUAGǘG/\\\"몪F4TJKbwBc앁髗!cU_Ẓ\\t:b06Kh803챀pc澙l)%sEW6WMD룟秇汗HCxNbR]g:&A7XGh19s)0r,c環e뢤MuJ75ɤ怕wਐzw/8\\fq\\rPHYzZh9vE%TqLe!&JnWwGm.9j\\rߞ잏ru2@ቴffsF[튿5TF䵼[s.Y(\\tWN\\rL11au\\n\\u000B\\rfP0,QN僠x(K#j\\\\HYkAe㤕5#7_vPve0l_s7S(9bPh5YpW湍)QF9 f3lqtA%LvOB\\f\\\"vl秏U&'D6!h\\n3\\f\\u000B\\\\.pe7d[M:mtN&JDtꐞs61;6N0ZR2EG顨[y E뚘\\f4uz5\\nHO(fv82LJ\\t0!ksNA\\t, vC_ꁗ?㽨t䭩ꆖ踃dRfU t0\\f4Ih1M榇@汿au\\riU䲣#2)\\r(es,?/;y6ﺤ\\\"&J,h]휩es nZfb棌wb@Z8lBkKf:G_D;;%B9鉜[f0\\txx0k]\\fr#'y]ua\\f\\\"U騍k0to\\fuTw끘bzzHI6RcඬT:h7\",\"l(]d%ub:[䟇@BW.'jZBQf酹e%Fj\\fq\\tF:&Wo'O]Wi9rN鶶阵\\u000BMIQk4Fw\\rp\\\\-cyb\\nA'당.7].s5ui-6q6좀V\\u000Bk\\\"\\\\uB4?#n!WX[㷪*\\nFLWK\\t1;mc5I5W层P맇GpM*JIfF\\\"Dz\\t뻈'f\\\";XR惎냻]EUJjNzX#V9\\f\\tk祔:XI3# :%R[9x\\feJƏ\\n\\\"EoX7.u1XMvR]uoxᐲjkMr*8%ꚁ3Teq䚙C@\\r僛M\\tB bx3vHyPV0?d垪:;鹈\\\\9g@d毜o䞵E2z1LH3ajeac3]e(\\n1cf;gSebV7戜6眨_3,iUq.옐:r];W7Q啣L8\\n@ZVGXGsO'p깁#鋚蹪AW\\r1裑\\\\TdW&BGC釫eMwKuz9偙'p()JX-錳ᑹ\\u000B0H\\n庢Ngv詷/7GI8S2'Sf%y缫?VGwbL/vNj9jD_WsyDW.@XI蠆0JZ!2IP02%(_[u'\\rZ6w\\t%绂E];EVpnY@[%i,U澹삸\"],\"comments\":null,\"answers\":null,\"notice\":{\"owner_user_id\":-2070953081,\"creation_date\":\"1907-01-14T04:41:22Z\",\"body\":null},\"closed_details\":{\"by_users\":null,\"original_questions\":[{\"accepted_answer_id\":null,\"answer_count\":null,\"question_id\":null,\"title\":null},{\"accepted_answer_id\":1776266977,\"answer_count\":1125233352,\"question_id\":971328174,\"title\":null},{\"accepted_answer_id\":null,\"answer_count\":1037626947,\"question_id\":null,\"title\":\"kU癚b쒺(SjIG8\\f랕F;]m/,@'0Vk\\rIZjYDSX\\tKZ剋jKOf(㝢\\f縟\\rm0g\\fmG8zAV\\u000B*'S?]o*#s\\\\\\\\\\f99%:9\\tV\\u000B귘M.\\\\C\\t#8wS_4ꌦ彚酀w*]nV.Kಋ\\fe/kjW?sVሀw1d鸴uv7!nﹸ䍓ns[yXN\\\\J뵉Dj7K]S?辔:em&jzWqu봂\\f[(躂*\\\\yx:wNvDe-\\nD\\\\-OvsV䦨Q1\\rr@情nvF,\\\\UBie\\\\'686⁂G!Pn#ᏠwUMF?蕍r*E\\\"ἒ1m2;ri[T9Ew,G?Ut#/1f煅迶p髛cnyAe[a3@%Y@bQplk'&譩r_JmO\\r0D/lF\\\"7U'PLg?)r\\\\[3fYㇰ'8쑃WkY)sk:/\\u000Bd&ଽ#mYa\"},{\"accepted_answer_id\":null,\"answer_count\":null,\"question_id\":null,\"title\":null},{\"accepted_answer_id\":null,\"answer_count\":null,\"question_id\":null,\"title\":null},{\"accepted_answer_id\":-384763884,\"answer_count\":null,\"question_id\":null,\"title\":null}],\"on_hold\":null,\"description\":null,\"reason\":null},\"migrated_to\":{\"other_site\":null,\"on_date\":null,\"question_id\":null},\"migrated_from\":{\"other_site\":{\"aliases\":null,\"styling\":null,\"related_sites\":null,\"markdown_extensions\":null,\"launch_date\":\"1919-09-21T23:44:54Z\",\"open_beta_date\":null,\"closed_beta_date\":null,\"site_state\":null,\"high_resolution_icon_url\":null,\"twitter_account\":\"Ｉ91JW oqPsDv.,Y7ko6䯔P\\u000B#VJ圹N(\\t(a鈥B@%#u䑻ﭯ?%&BO'o熝s3F(_[L_ZJ:y/epwup쑜1luSc\\u000B*PK25VB*:*oO龦jH橦hnt;M7P'EsE4)Ea;MXJ닓 pqu.\",\"favicon_url\":null,\"icon_url\":\"Gs9yMsH*澉[V-@류vTv,R\\\\n!?]LK_F)n(U\\r衼&q]?UE*N輾*#dGJc.-oQY4MᏛL6ᖇ,5u!e.\\tC2趬4CD-zR䍾avNK)C믦\\\\l䕮Uᙂ;bj\\\\DH.,c!縫@tKsSx9,*Ox-Ut\\u000BNN1][F8&;P1\\rVZrEJBD(\\u000BDkz!]NQb鐼!#0CLx&Cf[Qbv亝:\\\\nho,8ᤁS!l7帒c]z4\\t@O쏀g_)#-tn(\\\"R'&lwbjp&[34;W\\r'_h9Ztd.繍\\r\\u000B*gM#랧UnWW9\\fW@KL:LEPH\\r6Czy\\r/戇㮵A짤dj礑2fb( 8kO\\r\\tT c.QUXs]o!Mgr66,MtHXq\",\"audience\":\"5\\rUUNv㫫bqV\\u000BEU\\rJQi&犁SxyMzO(2I\\ti줨ISn\\fZZ?\\r鴺@7X-eE][\\u000BN/q[q28CiMUm\\\"4D.\\tPPM&!U2Um(d\\t'xm(woB[wP:6tIy嬘\\\"@Ab1)2m[do.\\n[r\\rRAdZu%@m-眆uXi]/见Cub@!T]%DE:\\tujak\\no\\\\6]ｲ\\nxHHE pa1aZt7풶T05TV :P_HH*农nX땗h3A9磳\\fIgB1d@-*q3抹Tx93#/tᬬ\\\\!243Sd혪\\u000Bkqx늱d gh&&w%\\u000BZr?0XLu㘐tD98'5䱞f堯8b0Uɓ\\u000Bau0I\\\\4k\",\"site_url\":null,\"api_site_parameter\":null,\"logo_url\":null,\"name\":null,\"site_type\":null},\"on_date\":null,\"question_id\":null},\"owner\":{\"badge_counts\":{\"bronze\":null,\"silver\":null,\"gold\":null},\"accept_rate\":49700093,\"user_type\":null,\"reputation\":null,\"user_id\":null,\"link\":\"6!dN(\\nYsHM;:Uu#144rMI-A䄗-.qcwIL6!U哿\\\"l\\u000BnSJGX\\\\xq \\fR'rlV(QrP,om㕒-ve\\rG몳f\\nVIYd3JahffdQq䄾1OFp(_ld&IX!*\\\"?3C84'䡵\\\\%C훢FhFଊ\\txBA8.6g3DCCDAd\\n樫\\nﶾ谘B?5@5cy;f-YEzR(wDx\\\"r;Xt\\n6)u5b'ꌚje;E\\t\\\"2hĎdC:K)\\n\\n'\\t\\u000Bc-b\\nMjt[p'rkX/Yd7રPh\\tEJ7yj嘒*'IG4\\tmbawG5vEY(STPE\\t,#F4x'Gm#-㛔K331PX,_),KqOvE\\fgL7qH靄;%*;/u㰯E꼶z0/E\",\"profile_image\":\"Vx5)JẤF5:ipऍu久g\\\"wob#灈n_cat;'/\\rd)邰餀\\rjn24vfs22麜ECEK钌䯲ijitjci檢#U%v췓NCu]/ሧCx!XlFfh明d怳f!8a髾GD]x멈䧑@e橫WaD!#S-*\\f\\nI伲nG-ezfTc\\f(Q偨LL](TUCLwaleebp\\\\he%.76NAv\\u000B#lJ3E唋E/THV](Z\\\\AKA2\\ry3\\u000BG_fZ6쒨U9HW6뾇e%hi1qd䁿N8rco亮(\\u000B_g1nSKPty吺EsJiT[c陉wv;FaTgq\\\"\\trJ2\\r8\\f.S_G*'w(F59)G9]J%6JMuY?!arlho2逾S*Q0#걃f샱\\nXCiH6l늭\\\"(WCeqAg#o4-ꃠx,撤zFUDWsLwnj&T/mrhZneg\\\"JCe*\\\"#Mwpg瓹d/HV'梲C\",\"display_name\":null},\"last_editor\":{\"badge_counts\":null,\"accept_rate\":null,\"user_type\":null,\"reputation\":null,\"user_id\":null,\"link\":null,\"profile_image\":null,\"display_name\":\"mvw칎z0qB齁)G\\\\srhV1鑨%Hmv*N#\\fX:酶(R-q,v'\\r[Gh?\\n.'31&_elcd,PZg\\fRv9\\n\\f md5V;R#0k qU믖@0@\\tC0baVjb,C)6g\\\"f:zA䆭pi.N;;赟@Nk/cjBrmmpzMK!@%T/_W;趦I;fF똛gRlⱊMXoQ);Cp\\fꉁGq:D0칵pHWz20ꈄHFA([jSoBL7qAT/%e\\\"H纟EcCVg.a:.0\\\"餘\\tf0y?\\\"wvo-s%IgE諀施\\tj'bVnXy%QJ,0뢫槬dhl!C5O0लW?,\\\"鋧R쾰\\tOj_Y*叒.\\fD,p!iD\\u000Bng@5쵅\\\\pxN5YIpC]\\t\\\"c鍭o䊆G@QJF8Q蔮rJid\\\\Pp(*Y_것B爞kZjqg]eVG6IUⴢi.FpCl[WgpX&it[69p\\fF\\t&Ud\\tNBSfإK85 oⵁ춫8V37:(\\nClp戺dJBMnL30:(\\flpQqC',cog58g\\r3lQ\\u000Bc\\nP6m@BM#5n;fU㔻\\tpQZeUwg)0N#\\t\\n#9W\\fyJ櫓NCLS\\\"o\\frt \\raB&J3Fkj聴-J峎䬾!mZ阺\\u000B&S4./\"},\"comment_count\":2010823663,\"favorited\":false,\"downvoted\":null,\"upvoted\":null,\"delete_vote_count\":null,\"reopen_vote_count\":null,\"close_vote_count\":null,\"is_answered\":null,\"view_count\":null,\"favorite_count\":null,\"down_vote_count\":null,\"up_vote_count\":null,\"protected_date\":null,\"closed_date\":null,\"bounty_amount\":-1591917649,\"bounty_closes_date\":null,\"accepted_answer_id\":-1220083983,\"answer_count\":1738999618,\"community_owned_date\":\"1960-02-19T23:52:21Z\",\"score\":null,\"locked_date\":\"1923-03-28T16:17:01Z\",\"last_activity_date\":\"1956-06-22T03:20:28Z\",\"creation_date\":null,\"last_edit_date\":null,\"question_id\":null,\"share_link\":\"!N\\u000BPnQtb䁓o4x屳\\\" @]#\\\\wDNﰀq:h춎媬-M%\\\\oV]튽蕡5ZbA.)d9Fፙson\\rUR;a!ꜤlB㟇#계s[索徺\\\\BqWV螮h/;7\",\"body_markdown\":\"]6nVKy\\t&KWB@;zkF7嶖eD&0\\nPqDJ椵?h뤇#:y8(\\\\7On]橠T *.cJ\\\\eEꑗua//&\\\"s;tsgnwhLPtEbR!\\\\E[ABP䣺A/cMMb0牡Dm6d oVx*bu55zSf㛁[QYDC93iYEVe\\u000BE\\\"J祷\\rD%d\\fOHGw@w[3]4; hW[oGy?/ILb,ᗈ\\u000BNp\",\"link\":\"EL꺹nn3z7F%Ng(tUk]lN\\f'[\\tIza5,)c/ED슕?\\t짽\\fFw6\\\"]7XsB\\\"A'kKJiOzŔn機合䇢U\\u000B뗍LbFoQC!NoB[tFH/\\nSg4I;33\\\\V!zk\\u000B_v7rd]崗n.1hC)*疚UDDcgkY!1DEN&2Vso*5谑_Htn;5qP\\\"t棚\\rcgQz\\r9v*answW9o,s#CSdc꩕-Ss0_꿯c\\fjltg\\n8\\ttSp]w(\\\",NgLe:ya3t\\u000Blk'븿bH/g#dxj( ]w%\\\\sZ41L8%m8t:'LjyLK\\fbWR2웟\\n훟bHe좲뜌R-LZcph읧\\nk!L뒩V幻NcF쨺VOs蒙TQ@J驂\\u000BYz栱)\\rF2_鍣鷗%D㙧\\\"a\\\"cONgF\\nku秱36氐8F4r_I屳2fK;@fk낁/\\nDW㑬 g핖DLu\\\"a2# *b\\rTh@aeC#㸇CeJaaMDW膋CczGZw\\r9\\tdY2MߣDkz_W/g\\u000BNT\",\"closed_reason\":\"Eb\\n0AAB9d㴣5aA鑬ykPo'#*%W%DFh -]byd\\fAv*cTd5_hxk:08\\\"*jw\\t&恡/R-35O6Q\\\\KYhPJk䐱a\\\\A\\\\Q睶HCxd( pCX]\\rZad뵩 勉j7pIq5hydN阬@cs%7fup';A\\\"7(7אַEq1\\\"FT#Q岛Yw(wn]fߩ0\\trPf㩅LgQ讯1af*ⱲT-JbfzElYAi(Fe\\f擠:[酌b \\\"4QX'偊.QYYZrY9WW\\u000BORS\\\"QIn\\u000B2DNk'KJ#;j/T58Ccl\\r&W?bzK9[T荗喷w91R\\\\\\nU\\n\\\"cjyd8eC렷]-S颲?\\nn畔mEFT\\r!J[\\\"?#:CXLc죃FfL!唟摣%T䜁\\u000B/㲄O5ᤙ cL*TciYyW6/挻ᝑ'k!l6V4\\\"7\\u000BgCJꐁ\\u000BT,7ca63,@Ig꣑XEcJZውN8&5\\u000BPV뮯n8'7Z 'eQ\\tl1,\\t&gq6_(&9Z1Uvnthhd#_C뇔n鮚d(u滠eiH_qZLoVӶPn먪3u:WrT\\nW\\u000Bjc34*aFwVzW푖ydMDSꂎ(9\\ro*@X7:sElw㳋o\\n봰_#]*lsAI洔䛺%4QP䟡ẇ1Kl8U&톮mBPn\\rN\\u000Bo疭\\n*Io\",\"title\":\"#nj2TfPd#Vh.\\\"@P#GFerzY\\rpz\\n\\f酈VA[R\\\"EhJRa\\te樜TyOaIB뢪N㕎a9%apliuk0p[B䉊(Ino0QjSvl DCsCQ_/1bՀhy7튄PqrZ-j0x]?㵲V %xsgU4YH9cS㭞9Ksi'竾\\n\\rRf5gHbvja'4zPIa\\u000BF[k\\fU##aA?6@F\\u000B1rm!ynsCqX36䌥 o[K!]2-泶BMfR1,毸vo\\\"\\n*vq༡1xp&훪uDe/Cu8#k\\\\*JJqꄩqANW6?麬6\\nJ(l/m(\\\\l9@U3嗂x蛢7B&D/_kU.#粏pb *r?I至%M\\f/ᵎ㻼\\\"w\\n6荼6皞; cd215猊)塉v&a1)8KVfxQ*cdR30]vcNTY4T/[藌a v3! vTAf_儨 \\\"ggO䜠\\rjxKm핏s&8U\\\"j譝,,R殱듲(7G;9&:uj\\nOnU\\rKe䥭6.&搩D# 0kVP]eb96Z86_GIO0/ZZf7\\n\\fubk[\",\"body\":\"kq8GYy)蒁4B荔U&[2\\n 녯GL_)/jft,zry1D2yHTůB_.\\r\\\"HUas푼F5r.s J臏@1n5\\u000B[ctoC%Hp2vZ6po4J\\t1HJs/37a瀺 C\\fOkxgJ3mhꊵ\\r2chI'HBLNlA8鬉.7c?udT]:CCaH\\n뼩 ႐&D\\\\\\\\\\rvOxR?\\\\,Z䵭28ﳔꃼX/)ICDa\\\\ScC3NxH)쎩 n\\\"ihUXvEw献y8M!N7llY;-*9䶮HrS#1XJP_J6Zy8Gc\\\\dD㾓Jo_UUWU6J08TTER%_C!W䋲De䢗.\\t*hyFxW-Srw똴5fF\\\".\\r\\\"\\\"x鍺vu\\fz@ṣs1Pyjie;vnC\\\")afZOnHoK羰撵]ErB/衘yew\\tQYCyL@V?vr齩Fv!#*2-y;DϨarHrS@QAS봝4 ;rxU3N#팮f1rOR@7[Cv\\\\o彂J4)WAJᴣXD.ظe\\\\\\\"?]P[pNjK@dvO宅\\u000B-?প\\u000B郎x,&[3]힟\\nꖋU茶3f?QfIMs\\\"O25\\\"VcH/F[ߪ\\\\皀\\\"\"}";
            var dyn = JSON.DeserializeDynamic(json, Options.ISO8601);
            var stat = JSON.Deserialize<Benchmark.Models.Question>(json, Options.ISO8601);

            var eq = stat.EqualsDynamic(dyn);

            Assert.True(eq);
        }
#endif

        [Fact]
        public void MemberAccess()
        {
            var dyn = JSON.DeserializeDynamic("{\"A\":123.45, \"B\": \"hello world\", \"C\": 678}");
            Assert.Equal(123.45, (double)dyn.A);
            Assert.Equal("hello world", (string)dyn.B);
            Assert.Equal(678, (int)dyn.C);
        }

        [Fact]
        public void Indexer()
        {
            {
                var dyn = JSON.DeserializeDynamic("{\"A\":123.45, \"B\": \"hello world\", \"C\": 678}");
                Assert.Equal(123.45, (double)dyn["A"]);
                Assert.Equal("hello world", (string)dyn["B"]);
                Assert.Equal(678, (int)dyn["C"]);
            }

            {
                var dyn = JSON.DeserializeDynamic("[123.45, \"hello world\", 678]");
                Assert.Equal(123.45, (double)dyn[0]);
                Assert.Equal("hello world", (string)dyn[1]);
                Assert.Equal(678, (int)dyn[2]);
            }
        }

        [Fact]
        public void UnaryPlus()
        {
            {
                var dyn = JSON.DeserializeDynamic("123.45");
                Assert.Equal(123.45, (double)+dyn);
            }

            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(123, (int)+dyn);
            }
        }

        [Fact]
        public void Negate()
        {
            {
                var dyn = JSON.DeserializeDynamic("123.45");
                Assert.Equal(-123.45, (double)-dyn);
            }

            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(-123, (int)-dyn);
            }
        }

        [Fact]
        public void Not()
        {
            {
                var dyn = JSON.DeserializeDynamic("false");
                Assert.True((bool)!dyn);
            }

            {
                var dyn = JSON.DeserializeDynamic("true");
                Assert.False((bool)!dyn);
            }
        }

        [Fact]
        public void Add()
        {
            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(456, (int)(dyn + 333));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("333");
                Assert.Equal(456, (int)(dyn1 + dyn2));
            }
        }

        [Fact]
        public void Subtract()
        {
            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(12, (int)(dyn - 111));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("111");
                Assert.Equal(12, (int)(dyn1 - dyn2));
            }
        }

        [Fact]
        public void Multiply()
        {
            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(246, (int)(dyn * 2));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("2");
                Assert.Equal(246, (int)(dyn1 * dyn2));
            }
        }

        [Fact]
        public void Divide()
        {
            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(61, (int)(dyn / 2));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("2");
                Assert.Equal(61, (int)(dyn1 / dyn2));
            }
        }

        [Fact]
        public void EqualsTest()
        {
            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn == 123);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn1 == dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("\"hello\"");
                Assert.Equal(true, dyn1 == "hello");
            }

            {
                var dyn1 = JSON.DeserializeDynamic("\"hello\"");
                var dyn2 = JSON.DeserializeDynamic("\"hello\"");
                Assert.Equal(true, dyn1 == dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("true");
                Assert.Equal(true, dyn1 == true);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("true");
                var dyn2 = JSON.DeserializeDynamic("true");
                Assert.Equal(true, dyn1 == dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                Assert.Equal(true, dyn1 == false);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                var dyn2 = JSON.DeserializeDynamic("false");
                Assert.Equal(true, dyn1 == dyn2);
            }
        }

        [Fact]
        public void NotEquals()
        {
            {
                var dyn = JSON.DeserializeDynamic("123");
                Assert.Equal(false, dyn != 123);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("123");
                Assert.Equal(false, dyn1 != dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("\"hello\"");
                Assert.Equal(false, dyn1 != "hello");
            }

            {
                var dyn1 = JSON.DeserializeDynamic("\"hello\"");
                var dyn2 = JSON.DeserializeDynamic("\"hello\"");
                Assert.Equal(false, dyn1 != dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("true");
                Assert.Equal(false, dyn1 != true);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("true");
                var dyn2 = JSON.DeserializeDynamic("true");
                Assert.Equal(false, dyn1 != dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                Assert.Equal(false, dyn1 != false);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                var dyn2 = JSON.DeserializeDynamic("false");
                Assert.Equal(false, dyn1 != dyn2);
            }
        }

        [Fact]
        public void GreaterThan()
        {
            {
                var dyn1 = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn1 > 1);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("1");
                Assert.Equal(true, dyn1 > dyn2);
            }
        }

        [Fact]
        public void LessThan()
        {
            {
                var dyn1 = JSON.DeserializeDynamic("123");
                Assert.Equal(false, dyn1 < 1);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("1");
                Assert.Equal(false, dyn1 < dyn2);
            }
        }

        [Fact]
        public void GreaterThanOrEquals()
        {
            {
                var dyn1 = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn1 >= 1);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn1 >= 123);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("1");
                Assert.Equal(true, dyn1 >= dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn1 >= dyn2);
            }
        }

        [Fact]
        public void LessThanOrEquals()
        {
            {
                var dyn1 = JSON.DeserializeDynamic("123");
                Assert.Equal(false, dyn1 <= 1);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn1 <= 123);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("1");
                Assert.Equal(false, dyn1 <= dyn2);
            }

            {
                var dyn1 = JSON.DeserializeDynamic("123");
                var dyn2 = JSON.DeserializeDynamic("123");
                Assert.Equal(true, dyn1 <= dyn2);
            }
        }

        [Fact]
        public void And()
        {
            {
                var dyn1 = JSON.DeserializeDynamic("true");
                Assert.True((bool)(dyn1 && true));
                Assert.False((bool)(dyn1 && false));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("true");
                var dyn2 = JSON.DeserializeDynamic("true");
                Assert.True((bool)(dyn1 && dyn2));
                Assert.True((bool)(dyn1 && dyn2 && true));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                Assert.False((bool)(dyn1 && true));
                Assert.False((bool)(dyn1 && false));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                var dyn2 = JSON.DeserializeDynamic("false");
                Assert.False((bool)(dyn1 && dyn2));
                Assert.False((bool)(dyn1 && dyn2 && false));
            }
        }

        [Fact]
        public void Or()
        {
            {
                var dyn1 = JSON.DeserializeDynamic("true");
                Assert.True((bool)(dyn1 || true));
                Assert.True((bool)(dyn1 || false));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("true");
                var dyn2 = JSON.DeserializeDynamic("true");
                Assert.True((bool)(dyn1 || dyn2));
                Assert.True((bool)(dyn1 || dyn2 || true));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                Assert.True((bool)(dyn1 || true));
                Assert.False((bool)(dyn1 || false));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                var dyn2 = JSON.DeserializeDynamic("false");
                Assert.False((bool)(dyn1 || dyn2));
                Assert.False((bool)(dyn1 || dyn2 || false));
            }
        }

        [Fact]
        public void ShortCircuits()
        {
            {
                var dyn1 = JSON.DeserializeDynamic("true");
                dynamic dyn2 = "what no";
                Assert.True((bool)(dyn1 || dyn2));
            }

            {
                var dyn1 = JSON.DeserializeDynamic("false");
                dynamic dyn2 = "what no";
                Assert.False((bool)(dyn1 && dyn2));
            }
        }

        enum _EnumMemberAttributeOverride
        {
            [EnumMember(Value = "1")]
            A = 1,
            [EnumMember(Value = "2")]
            B = 2,
            [EnumMember(Value = "4")]
            C = 4
        }

        [Fact]
        public void EnumMemberAttributeOverride()
        {
            Assert.Equal(_EnumMemberAttributeOverride.A, (_EnumMemberAttributeOverride)JSON.DeserializeDynamic("\"1\""));
            Assert.Equal(_EnumMemberAttributeOverride.B, (_EnumMemberAttributeOverride)JSON.DeserializeDynamic("\"2\""));
            Assert.Equal(_EnumMemberAttributeOverride.C, (_EnumMemberAttributeOverride)JSON.DeserializeDynamic("\"4\""));
        }

        [Flags]
        enum _FlagsEnum
        {
            A = 1,
            B = 2,
            C = 4
        }

        [Fact]
        public void FlagsEnum()
        {
            Assert.Equal(_FlagsEnum.A, (_FlagsEnum)JSON.DeserializeDynamic("\"A\""));
            Assert.Equal(_FlagsEnum.B, (_FlagsEnum)JSON.DeserializeDynamic("\"B\""));
            Assert.Equal(_FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, (_FlagsEnum)JSON.DeserializeDynamic("\"A, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, (_FlagsEnum)JSON.DeserializeDynamic("\"A,B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, (_FlagsEnum)JSON.DeserializeDynamic("\"B, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B, (_FlagsEnum)JSON.DeserializeDynamic("\"B,A\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A,C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C,A\""));

            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B, C\""));
            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B,C\""));
            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C, B\""));
            Assert.Equal(_FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C,B\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A, B, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A, B,C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A,B, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A,B,C\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A, C, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A, C,B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A,C, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"A,C,B\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B, A, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B, A,C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B,A, C\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B,A,C\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B, C, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B, C,A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B,C, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"B,C,A\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C, A, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C, A,B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C,A, B\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C,A,B\""));

            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C, B, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C, B,A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C,B, A\""));
            Assert.Equal(_FlagsEnum.A | _FlagsEnum.B | _FlagsEnum.C, (_FlagsEnum)JSON.DeserializeDynamic("\"C,B,A\""));
        }

        [Flags]
        enum _EnumMemberAttributeOverrideFlags
        {
            [EnumMember(Value = "1")]
            A = 1,
            [EnumMember(Value = "2")]
            B = 2,
            [EnumMember(Value = "3")]
            C = 4
        }

        [Fact]
        public void EnumMemberAttributeOverrideFlags()
        {
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2,1\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3,1\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3,2\""));

            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1, 2, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1, 2,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1,2, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1,2,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1, 3, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1,3, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"1,3,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2, 1, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2, 1,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2,1, 3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2,1,3\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2, 3, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2, 3,1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2,3, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"2,3,1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3, 1, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3, 1,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3,1, 2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3,1,2\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3, 2, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3, 2,1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3,2, 1\""));
            Assert.Equal(_EnumMemberAttributeOverrideFlags.A | _EnumMemberAttributeOverrideFlags.B | _EnumMemberAttributeOverrideFlags.C, (_EnumMemberAttributeOverrideFlags)JSON.DeserializeDynamic("\"3,2,1\""));
        }

        [Fact]
        public void LongDateTimes()
        {
            var shouldMatch = new DateTime(2014, 08, 08, 14, 04, 01, 426, DateTimeKind.Utc);
            shouldMatch = new DateTime(shouldMatch.Ticks + 5339, DateTimeKind.Utc);
            var dyn = JSON.DeserializeDynamic("\"2014-08-08T14:04:01.4265339+00:00\"", Options.ISO8601);
            DateTime dt = dyn;
            Assert.Equal(shouldMatch, dt);
        }

        [Fact]
        public void Issue61()
        {
            using (var str = new StringReader("{\"hello\":123, \"world\":456, \"foo\":789}"))
            {
                bool hello, world, foo;
                hello = world = foo = false;

                var res = JSON.DeserializeDynamic(str);
                foreach (var kv in res)
                {
                    string key = kv.Key;
                    dynamic val = kv.Value;

                    if(key == "hello") 
                    {
                        if (hello)
                        {
                            Assert.True(false, "hello seen twice");
                        }
                        hello = true;

                        Assert.Equal(123, (int)val);
                        continue;
                    }

                    if (key == "world")
                    {
                        if (world)
                        {
                            Assert.True(false, "world seen twice");
                        }
                        world = true;

                        Assert.Equal(456, (int)val);
                        continue;
                    }

                    if (key == "foo")
                    {
                        if (foo)
                        {
                            Assert.True(false, "foo seen twice");
                        }
                        foo = true;

                        Assert.Equal(789, (int)val);
                        continue;
                    }
                }

                Assert.True(hello);
                Assert.True(world);
                Assert.True(foo);
            }
        }

        [Fact]
        public void DateTimeOffsets()
        {
            // milliseconds
            {
                var now = DateTimeOffset.UtcNow;
                var str1 = JSON.Serialize(now, Options.MillisecondsSinceUnixEpoch);
                var dyn = JSON.DeserializeDynamic(str1, Options.MillisecondsSinceUnixEpoch);
                var dto = (DateTimeOffset)dyn;
                
                var str2 = JSON.Serialize(dto, Options.MillisecondsSinceUnixEpoch);

                Assert.Equal(str1, str2);
            }
            // seconds
            {
                var now = DateTimeOffset.UtcNow;
                var str1 = JSON.Serialize(now, Options.SecondsSinceUnixEpoch);
                var dyn = JSON.DeserializeDynamic(str1, Options.SecondsSinceUnixEpoch);
                var dto = (DateTimeOffset)dyn;

                var str2 = JSON.Serialize(dto, Options.SecondsSinceUnixEpoch);

                Assert.Equal(str1, str2);
            }
            // microsoft-style
            {
                var now = DateTimeOffset.UtcNow;
                var str1 = JSON.Serialize(now, Options.Default);
                var dyn = JSON.DeserializeDynamic(str1, Options.Default);
                var dto = (DateTimeOffset)dyn;

                var str2 = JSON.Serialize(dto, Options.Default);

                Assert.Equal(str1, str2);
            }
            // iso8601
            {
                var now = DateTimeOffset.UtcNow;
                var str1 = JSON.Serialize(now, Options.ISO8601);
                var dyn = JSON.DeserializeDynamic(str1, Options.ISO8601);
                var dto = (DateTimeOffset)dyn;

                var str2 = JSON.Serialize(dto, Options.ISO8601);

                Assert.Equal(str1, str2);
            }
        }

        [Fact]
        public void TimeSpans_Milliseconds()
        {
            var rand = new Random();
            var ts = new List<TimeSpan>();
            for (var i = 0; i < 10000; i++)
            {
                var ms = rand.Next();
                if (rand.Next(2) == 1)
                {
                    ms = -ms;
                }

                ts.Add(TimeSpan.FromMilliseconds(ms));
            }

            ts.Add(TimeSpan.MaxValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            ts.Add(TimeSpan.MinValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            ts.Add(default(TimeSpan));

            foreach (var t1 in ts)
            {
                var str1 = JSON.Serialize(t1, Options.MillisecondsSinceUnixEpoch);
                var dyn = JSON.DeserializeDynamic(str1, Options.MillisecondsSinceUnixEpoch);
                var t2 = (TimeSpan)dyn;
                var str2 = JSON.Serialize(t2, Options.MillisecondsSinceUnixEpoch);

                Assert.Equal(Math.Round(t1.TotalMilliseconds), Math.Round(t2.TotalMilliseconds));
                Assert.Equal(str1, str2);
            }
        }

        [Fact]
        public void TimeSpans_Seconds()
        {
            var rand = new Random();
            var ts = new List<TimeSpan>();
            for (var i = 0; i < 10000; i++)
            {
                var ms = rand.Next();
                if (rand.Next(2) == 1)
                {
                    ms = -ms;
                }

                ts.Add(TimeSpan.FromSeconds(ms));
            }

            ts.Add(TimeSpan.MaxValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            ts.Add(TimeSpan.MinValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            ts.Add(default(TimeSpan));

            foreach (var t1 in ts)
            {
                var str1 = JSON.Serialize(t1, Options.SecondsSinceUnixEpoch);
                var dyn = JSON.DeserializeDynamic(str1, Options.SecondsSinceUnixEpoch);
                var t2 = (TimeSpan)dyn;
                var str2 = JSON.Serialize(t2, Options.SecondsSinceUnixEpoch);

                Assert.Equal(Math.Round(t1.TotalSeconds), Math.Round(t2.TotalSeconds));
                Assert.Equal(str1, str2);
            }
        }

        [Fact]
        public void TimeSpans_Microsoft()
        {
            var rand = new Random();
            var ts = new List<TimeSpan>();
            for (var i = 0; i < 10000; i++)
            {
                var ms = rand.Next();
                if (rand.Next(2) == 1)
                {
                    ms = -ms;
                }

                ts.Add(TimeSpan.FromSeconds(ms));
            }

            ts.Add(TimeSpan.MaxValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            ts.Add(TimeSpan.MinValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            ts.Add(default(TimeSpan));

            foreach (var t1 in ts)
            {
                var str1 = JSON.Serialize(t1, Options.Default);
                var dyn = JSON.DeserializeDynamic(str1, Options.Default);
                var t2 = (TimeSpan)dyn;
                var str2 = JSON.Serialize(t2, Options.Default);

                Assert.Equal(Math.Round(t1.TotalMilliseconds), Math.Round(t2.TotalMilliseconds));
            }
        }

        [Fact]
        public void TimeSpans_ISO8601()
        {
            var rand = new Random();
            var ts = new List<TimeSpan>();
            for (var i = 0; i < 10000; i++)
            {
                var ms = rand.Next();
                if (rand.Next(2) == 1)
                {
                    ms = -ms;
                }

                ts.Add(TimeSpan.FromSeconds(ms));
            }

            ts.Add(TimeSpan.MaxValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MaxValue.Ticks - n)));
            ts.Add(TimeSpan.MinValue);
            ts.AddRange(Enumerable.Range(1, 1000).Select(n => new TimeSpan(TimeSpan.MinValue.Ticks + n)));
            ts.Add(default(TimeSpan));

            foreach (var t1 in ts)
            {
                var str1 = JSON.Serialize(t1, Options.ISO8601);
                var dyn = JSON.DeserializeDynamic(str1, Options.ISO8601);
                var t2 = (TimeSpan)dyn;
                var str2 = JSON.Serialize(t2, Options.ISO8601);

                Assert.Equal(Math.Round(t1.TotalMilliseconds), Math.Round(t2.TotalMilliseconds));
            }
        }

        [Fact]
        public void EmptyArrayWithSpace()
        {
            var res = JSON.DeserializeDynamic("[ ]");
            Assert.NotNull(res);
            Assert.Equal(0, (int)res.Length);
        }

        enum _DynamicTypeConverter
        {
            None = 0,

            Hello = 1,
            World = 2
        }

        [Fact]
        public void DynamicTypeConverter()
        {
            // enums
            {
                var dyn = JSON.DeserializeDynamic("\"Hello\"");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                Assert.True(tc.CanConvertTo(typeof(_DynamicTypeConverter)));
                Assert.Equal(_DynamicTypeConverter.Hello, (_DynamicTypeConverter)tc.ConvertTo(dyn, typeof(_DynamicTypeConverter)));
            }

            // ints
            {
                var dyn = JSON.DeserializeDynamic("123");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                Assert.True(tc.CanConvertTo(typeof(int)));
                Assert.Equal(123, (int)tc.ConvertTo(dyn, typeof(int)));
            }

            // doubles
            {
                var dyn = JSON.DeserializeDynamic("123.45");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                Assert.True(tc.CanConvertTo(typeof(double)));
                Assert.Equal(123.45, (double)tc.ConvertTo(dyn, typeof(double)));
            }

            // floats
            {
                var dyn = JSON.DeserializeDynamic("123.45");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                Assert.True(tc.CanConvertTo(typeof(float)));
                Assert.Equal(123.45f, (float)tc.ConvertTo(dyn, typeof(float)));
            }

            // decimals
            {
                var dyn = JSON.DeserializeDynamic("123.45");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                Assert.True(tc.CanConvertTo(typeof(decimal)));
                Assert.Equal(123.45m, (decimal)tc.ConvertTo(dyn, typeof(decimal)));
            }

            // strings
            {
                var dyn = JSON.DeserializeDynamic("\"hello\"");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                Assert.True(tc.CanConvertTo(typeof(string)));
                Assert.Equal("hello", (string)tc.ConvertTo(dyn, typeof(string)));
            }

            // Guids
            {
                var guid = Guid.NewGuid();
                var dyn = JSON.DeserializeDynamic("\"" + guid.ToString("D") + "\"");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                Assert.True(tc.CanConvertTo(typeof(string)));
                Assert.True(tc.CanConvertTo(typeof(Guid)));
                Assert.Equal(guid.ToString("D"), (string)tc.ConvertTo(dyn, typeof(string)));
                Assert.Equal(guid, (Guid)tc.ConvertTo(dyn, typeof(Guid)));
            }

            // DateTimes
            {
                var dt = DateTime.UtcNow;
                {
                    var json = JSON.Serialize(dt, Options.Default);
                    var dyn = JSON.DeserializeDynamic(json, Options.Default);
                    var asStr = JSON.Deserialize<string>(json);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(string)));
                    Assert.True(tc.CanConvertTo(typeof(DateTime)));
                    Assert.Equal(asStr, (string)tc.ConvertTo(dyn, typeof(string)));
                    var diff = (dt - (DateTime)tc.ConvertTo(dyn, typeof(DateTime))).Duration();
                    Assert.True(diff.TotalMilliseconds < 1);
                }
                {
                    var json = JSON.Serialize(dt, Options.ISO8601);
                    var dyn = JSON.DeserializeDynamic(json, Options.ISO8601);
                    var asStr = JSON.Deserialize<string>(json);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(string)));
                    Assert.True(tc.CanConvertTo(typeof(DateTime)));
                    Assert.Equal(asStr, (string)tc.ConvertTo(dyn, typeof(string)));
                    var diff = (dt - (DateTime)tc.ConvertTo(dyn, typeof(DateTime))).Duration();
                    Assert.True(diff.TotalSeconds < 1);
                }
                {
                    var json = JSON.Serialize(dt, Options.MillisecondsSinceUnixEpoch);
                    var dyn = JSON.DeserializeDynamic(json, Options.MillisecondsSinceUnixEpoch);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(double)));
                    Assert.True(tc.CanConvertTo(typeof(DateTime)));
                    Assert.Equal(double.Parse(json), (double)tc.ConvertTo(dyn, typeof(double)));
                    var diff = (dt - (DateTime)tc.ConvertTo(dyn, typeof(DateTime))).Duration();
                    Assert.True(diff.TotalMilliseconds < 1);
                }
                {
                    var json = JSON.Serialize(dt, Options.SecondsSinceUnixEpoch);
                    var dyn = JSON.DeserializeDynamic(json, Options.SecondsSinceUnixEpoch);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(double)));
                    Assert.True(tc.CanConvertTo(typeof(DateTime)));
                    Assert.Equal(double.Parse(json), (double)tc.ConvertTo(dyn, typeof(double)));
                    var diff = (dt - (DateTime)tc.ConvertTo(dyn, typeof(DateTime))).Duration();
                    Assert.True(diff.TotalSeconds < 1);
                }
            }

            // DateTimeOffsets
            {
                var dto = DateTimeOffset.Now;
                {
                    var json = JSON.Serialize(dto, Options.Default);
                    var dyn = JSON.DeserializeDynamic(json, Options.Default);
                    var asStr = JSON.Deserialize<string>(json);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(string)));
                    Assert.True(tc.CanConvertTo(typeof(DateTimeOffset)));
                    Assert.Equal(asStr, (string)tc.ConvertTo(dyn, typeof(string)));
                    var diff = (dto - (DateTimeOffset)tc.ConvertTo(dyn, typeof(DateTimeOffset))).Duration();
                    Assert.True(diff.TotalMilliseconds < 1);
                }
                {
                    var json = JSON.Serialize(dto, Options.ISO8601);
                    var dyn = JSON.DeserializeDynamic(json, Options.ISO8601);
                    var asStr = JSON.Deserialize<string>(json);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(string)));
                    Assert.True(tc.CanConvertTo(typeof(DateTimeOffset)));
                    Assert.Equal(asStr, (string)tc.ConvertTo(dyn, typeof(string)));
                    var diff = (dto - (DateTimeOffset)tc.ConvertTo(dyn, typeof(DateTimeOffset))).Duration();
                    Assert.True(diff.TotalSeconds < 1);
                }
                {
                    var json = JSON.Serialize(dto, Options.MillisecondsSinceUnixEpoch);
                    var dyn = JSON.DeserializeDynamic(json, Options.MillisecondsSinceUnixEpoch);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(double)));
                    Assert.True(tc.CanConvertTo(typeof(DateTimeOffset)));
                    Assert.Equal(double.Parse(json), (double)tc.ConvertTo(dyn, typeof(double)));
                    var diff = (dto - (DateTimeOffset)tc.ConvertTo(dyn, typeof(DateTimeOffset))).Duration();
                    Assert.True(diff.TotalMilliseconds < 1);
                }
                {
                    var json = JSON.Serialize(dto, Options.SecondsSinceUnixEpoch);
                    var dyn = JSON.DeserializeDynamic(json, Options.SecondsSinceUnixEpoch);
                    System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                    Assert.True(tc.CanConvertTo(typeof(double)));
                    Assert.True(tc.CanConvertTo(typeof(DateTimeOffset)));
                    Assert.Equal(double.Parse(json), (double)tc.ConvertTo(dyn, typeof(double)));
                    var diff = (dto - (DateTimeOffset)tc.ConvertTo(dyn, typeof(DateTimeOffset))).Duration();
                    Assert.True(diff.TotalSeconds < 1);
                }
            }

            // IEnumerable
            {
                var dyn = JSON.DeserializeDynamic("[123, \"hello\", 456.7]");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);
                
                Assert.True(tc.CanConvertTo(typeof(System.Collections.IEnumerable)));
                var e = (System.Collections.IEnumerable)tc.ConvertTo(dyn, typeof(System.Collections.IEnumerable));
                Assert.Equal(3, e.Cast<dynamic>().Count());
                
                tc = System.ComponentModel.TypeDescriptor.GetConverter(e.Cast<dynamic>().ElementAt(0));
                Assert.True(tc.CanConvertTo(typeof(int)));
                Assert.Equal(123, (int)tc.ConvertTo(e.Cast<dynamic>().ElementAt(0), typeof(int)));

                tc = System.ComponentModel.TypeDescriptor.GetConverter(e.Cast<dynamic>().ElementAt(1));
                Assert.True(tc.CanConvertTo(typeof(string)));
                Assert.Equal("hello", (string)tc.ConvertTo(e.Cast<dynamic>().ElementAt(1), typeof(string)));

                tc = System.ComponentModel.TypeDescriptor.GetConverter(e.Cast<dynamic>().ElementAt(2));
                Assert.True(tc.CanConvertTo(typeof(double)));
                Assert.Equal(456.7, (double)tc.ConvertTo(e.Cast<dynamic>().ElementAt(2), typeof(double)));
            }

            // IEnumerable<T>
            {
                var dyn = JSON.DeserializeDynamic("[123, 456, 789]");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);

                Assert.True(tc.CanConvertTo(typeof(IEnumerable<int>)));
                var e = (IEnumerable<int>)tc.ConvertTo(dyn, typeof(IEnumerable<int>));
                Assert.Equal(3, e.Count());
                Assert.Equal(123, e.ElementAt(0));
                Assert.Equal(456, e.ElementAt(1));
                Assert.Equal(789, e.ElementAt(2));
            }

            // Dictionary<string, dynamic>
            {
                var dyn = JSON.DeserializeDynamic("{\"hello\":123, \"world\": 456.7, \"goodbye\": null }");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);

                Assert.True(tc.CanConvertTo(typeof(IDictionary<string, dynamic>)));
                var d = (Dictionary<string, dynamic>)tc.ConvertTo(dyn, typeof(IDictionary<string, dynamic>));
                Assert.Equal(3, d.Count);
                Assert.True(d.ContainsKey("hello"));                
                Assert.True(d.ContainsKey("world"));
                Assert.True(d.ContainsKey("goodbye"));

                tc = System.ComponentModel.TypeDescriptor.GetConverter(d["hello"]);
                Assert.True(tc.CanConvertTo(typeof(int)));
                Assert.Equal(123, (int)tc.ConvertTo(d["hello"], typeof(int)));

                tc = System.ComponentModel.TypeDescriptor.GetConverter(d["world"]);
                Assert.True(tc.CanConvertTo(typeof(double)));
                Assert.Equal(456.7, (double)tc.ConvertTo(d["world"], typeof(double)));

                Assert.Null((d["goodbye"]));
            }

            // Dictionary<enum, dynamic>
            {
                var dyn = JSON.DeserializeDynamic("{\"Hello\":123, \"World\": 456.7 }");
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(dyn);

                Assert.True(tc.CanConvertTo(typeof(IDictionary<_DynamicTypeConverter, dynamic>)));
                var d = (Dictionary<_DynamicTypeConverter, dynamic>)tc.ConvertTo(dyn, typeof(IDictionary<_DynamicTypeConverter, dynamic>));
                Assert.Equal(2, d.Count);
                Assert.True(d.ContainsKey(_DynamicTypeConverter.Hello));
                Assert.True(d.ContainsKey(_DynamicTypeConverter.World));

                tc = System.ComponentModel.TypeDescriptor.GetConverter(d[_DynamicTypeConverter.Hello]);
                Assert.True(tc.CanConvertTo(typeof(int)));
                Assert.Equal(123, (int)tc.ConvertTo(d[_DynamicTypeConverter.Hello], typeof(int)));

                tc = System.ComponentModel.TypeDescriptor.GetConverter(d[_DynamicTypeConverter.World]);
                Assert.True(tc.CanConvertTo(typeof(double)));
                Assert.Equal(456.7, (double)tc.ConvertTo(d[_DynamicTypeConverter.World], typeof(double)));
            }
        }

        [Theory]
        [InlineData("01", "Number cannot have leading zeros")]
        [InlineData("001", "Number cannot have leading zeros")]
        [InlineData("-01", "Number cannot have leading zeros")]
        [InlineData("-001", "Number cannot have leading zeros")]
        [InlineData("1.", "Expected digit")]
        [InlineData("12.", "Expected digit")]
        [InlineData("123.", "Expected digit")]
        [InlineData("-1.", "Expected digit")]
        [InlineData("-12.", "Expected digit")]
        [InlineData("-123.", "Expected digit")]
        public void Issue117(string input, string expectedError)
        {
            var ex = Assert.Throws<DeserializationException>(() => JSON.DeserializeDynamic(input));
            Assert.Equal(expectedError, ex.Message);
        }

        [Fact]
        public void RFC1123()
        {
            var now = DateTime.UtcNow;
            var str = JSON.Serialize(now, Options.RFC1123);
            var dyn = JSON.DeserializeDynamic(str, Options.RFC1123);
            DateTime res = dyn;

            var diff = (now - res).Duration();
            Assert.True(diff.TotalSeconds < 1);
        }

#if !DEBUG

        [Fact]
        public void RFC1123DateTimes()
        {
            var toTest = new List<DateTime>();

            var now = DateTime.UtcNow;
            toTest.Add(now);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            for (var i = 0; i < 357; i++)
            {
                toTest.Add(epoch + TimeSpan.FromDays(i));
                for (var j = 0; j < 24; j++)
                {
                    toTest.Add(epoch + TimeSpan.FromDays(i) + TimeSpan.FromHours(j));
                    for (var k = 0; k < 60; k++)
                    {
                        toTest.Add(epoch + TimeSpan.FromDays(i) + TimeSpan.FromHours(j) + TimeSpan.FromMinutes(k));
                        for (var l = 0; l < 60; l++)
                        {
                            toTest.Add(epoch + TimeSpan.FromDays(i) + TimeSpan.FromHours(j) + TimeSpan.FromMinutes(k) + TimeSpan.FromSeconds(l));
                        }
                    }
                }
            }

            foreach (var dt in toTest)
            {
                var json = "\"" + dt.ToString("R") + "\"";
                var dyn = JSON.DeserializeDynamic(json, Options.RFC1123);
                DateTime res = dyn;

                var diff = dt - res;
                Assert.True(diff.TotalSeconds < 1);
            }
        }
#endif

        [Fact]
        public void Issue134() {
            var json = "{\"data\":{\"id\":\"1\",\"name\":null,\"url\":\"http://www.google.com\"}}";
            var result = JSON.DeserializeDynamic(json); // or JSON.DeserializeDynamic(json);
            var name = result.data["name"];
            Assert.Null(name);
        }
    }
}
