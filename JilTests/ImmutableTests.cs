using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Jil;
using Xunit;

namespace JilTests
{
    public class ImmutableTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Serialize(bool derived)
        {
            var input = derived ? CreateDerivedTestData() : CreateTestData();
            var json = JSON.Serialize(input, Options.ISO8601PrettyPrintIncludeInheritedCamelCase);
            // Serialize will only Serialize "MyClass" members, not the additional MyClass2 ones
            Assert.Equal(CreateTestJson(false), json);
        }

        [Fact]
        public void Serialize_Derived()
        {
            var input = CreateDerivedTestData();
            var json = JSON.Serialize(input, Options.ISO8601PrettyPrintIncludeInheritedCamelCase);
            Assert.Equal(CreateTestJson(true), json);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void SerializeDynamic(bool derived)
        {
            var input = derived ? CreateDerivedTestData() : CreateTestData();
            var json = JSON.SerializeDynamic(input, Options.ISO8601PrettyPrintIncludeInheritedCamelCase);
            Assert.Equal(CreateTestJson(derived), json);
        }

        private string CreateTestJson(bool derived)
        {
            if (derived)
            {
                return @"{
 ""id"": 12345,
 ""noBytes"": [],
 ""someBytes"": [0, 128, 255],
 ""noStrings"": null,
 ""someStrings"": [""one"", ""two"", null, """", ""three""],
 ""noDictionary"": null,
 ""someDictionary"": {
  ""two"": [1, 2]
 },
 ""nestedImmutableArray"": [[[], [7]]],
 ""name"": ""Derived""
}".Replace("\r\n", "\n");
            }
            else
            {
                return @"{
 ""id"": 12345,
 ""noStrings"": null,
 ""someStrings"": [""one"", ""two"", null, """", ""three""],
 ""noDictionary"": null,
 ""someDictionary"": {
  ""two"": [1, 2]
 },
 ""noBytes"": [],
 ""someBytes"": [0, 128, 255],
 ""nestedImmutableArray"": [[[], [7]]]
}".Replace("\r\n", "\n");
            }
        }

        private MyClass2 CreateDerivedTestData()
        {
            var result = CreateTestData(true) as MyClass2;
            result.Name = "Derived";
            return result;
        }
           

        private MyClass CreateTestData(bool derived = false)
        {
            var obj = derived ? new MyClass2() : new MyClass();

            obj.Id = 12345;
            obj.SomeBytes = new byte[] { 0x00, 0x80, 0xff }.ToImmutableArray();
            obj.SomeStrings = new[] { "one", "two", null, "", "three" }.ToImmutableList();

            var dictBuilder = ImmutableDictionary.CreateBuilder<string, ImmutableArray<int>>();
            dictBuilder.Add("two", new[] { 1, 2 }.ToImmutableArray());
            obj.SomeDictionary = dictBuilder.ToImmutable();

            obj.NestedImmutableArray = new ImmutableArray<ImmutableArray<int>>[]
            {
                new ImmutableArray<int>[] {
                    new int[] {}.ToImmutableArray(),
                    new int[] {7}.ToImmutableArray()
                }.ToImmutableArray()
            }.ToImmutableArray();

            return obj;
        }


        public class MyClass
        {
            public int Id { get; set; }
            public ImmutableArray<byte> NoBytes { get; set; }
            public ImmutableArray<byte> SomeBytes { get; set; }
            public ImmutableList<string> NoStrings { get; set; }
            public ImmutableList<string> SomeStrings { get; set; }
            public ImmutableDictionary<string, ImmutableHashSet<int>> NoDictionary { get; set; }
            public ImmutableDictionary<string, ImmutableArray<int>> SomeDictionary { get; set; }
            public ImmutableArray<ImmutableArray<ImmutableArray<int>>> NestedImmutableArray { get; set; }
        }

        public class MyClass2 : MyClass
        {
            public string Name { get; set; }
        }
    }
}
