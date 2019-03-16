using System;
using System.Collections;
using System.Collections.Immutable;
using Jil;
using Jil.DeserializeDynamic;
using Xunit;

namespace JilTests
{
    public class ImmutableTests
    {
        private static Options SerOptions = Options.ISO8601PrettyPrintIncludeInheritedCamelCase;

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Serialize(bool derived)
        {
            var input = derived ? CreateDerivedTestData() : CreateTestData();
            var json = JSON.Serialize(input, SerOptions);
            // Serialize will only Serialize "MyClass" members, not the additional MyClass2 ones
            Assert.Equal(CreateTestJson(false), json);
        }

        [Fact]
        public void Serialize_Derived()
        {
            var input = CreateDerivedTestData();
            var json = JSON.Serialize(input, SerOptions);
            Assert.Equal(CreateTestJson(true), json);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void SerializeDynamic(bool derived)
        {
            var input = derived ? CreateDerivedTestData() : CreateTestData();
            var json = JSON.SerializeDynamic(input, SerOptions);
            Assert.Equal(CreateTestJson(derived), json);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize(bool derived)
        {
            var json = CreateTestJson(derived);
            var data = JSON.Deserialize<MyClass>(json, SerOptions);
            TestDeserializedObject(data);
        }

        [Fact]
        public void Deserialize_Derived()
        {
            var json = CreateTestJson(true);
            var data = JSON.Deserialize<MyClass2>(json, SerOptions);
            TestDeserializedObject(data);
            Assert.Equal("Derived", data.Name);
        }

        private void TestDeserializedObject(MyClass data)
        {
            Assert.Equal(12345, data.Id);
            Assert.Empty(data.NoBytes);
            Assert.Null(data.NoDictionary);
            Assert.Null(data.NoStrings);
            Assert.Equal(3, data.SomeBytes.Length);
            Assert.Equal(5, data.SomeStrings.Count);
            Assert.Single(data.SomeDictionary);
            Assert.Equal(2, data.SomeDictionary["two"].Length);
            Assert.Single(data.NestedImmutableArray);
            Assert.Equal(2, data.NestedImmutableArray[0].Length);
            Assert.Empty(data.NestedImmutableArray[0][0]);
            Assert.Single(data.NestedImmutableArray[0][1]);
            Assert.Equal(7, data.NestedImmutableArray[0][1][0]);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_Dynamic(bool derived)
        {
            var json = CreateTestJson(derived);
            var data = JSON.DeserializeDynamic(json, SerOptions);

            // Can't use TestDeserializedObject because of casing, but the casing change is part of the test
            if (derived)
            {
                Assert.Equal("Derived", (string)data.name);
            }

            Assert.Equal(12345, (int)data.id);
            Assert.Equal(0, (int)data.noBytes.Count);
            Assert.Null((object)data.noDictionary);
            Assert.Null((object)data.noStrings);
            Assert.Equal(3, (int)data.someBytes.Length);
            Assert.Equal(5, (int)data.someStrings.Count);
            Assert.Equal(2, (int)data.someDictionary.two.Length);
            Assert.Equal(1, (int)data.nestedImmutableArray.Length);
            Assert.Equal(2, (int)data.nestedImmutableArray[0].Length);
            Assert.Equal(0, (int)data.nestedImmutableArray[0][0].Length);
            Assert.Equal(1, (int)data.nestedImmutableArray[0][1].Length);
            Assert.Equal(7, (int)data.nestedImmutableArray[0][1][0]);
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
