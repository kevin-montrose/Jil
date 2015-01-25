### Jil

A fast JSON (de)serializer, built on [Sigil](https://github.com/kevin-montrose/Sigil) with a number of somewhat crazy optimization tricks.

[Releases are available on Nuget](https://www.nuget.org/packages/Jil/) in addition to this repository.

## Usage

### Serializing

```
    using(var output = new StringWriter())
    {
        JSON.Serialize(
            new
            {
                MyInt = 1,
                MyString = "hello world",
                // etc.
            },
            output
        );
    }
```

There is also a `Serialize` method that returns a string.

The first time Jil is used to serialize a given configuration and type pair, it will spend extra time building the serializer.
Subsequent invocations will be much faster, so if a consistently fast runtime is necessary in your code you may want to "prime the pump"
with an earlier "throw away" serialization.

### Dynamic Serialization

If you need to serialize compile-time unknown types (including subclasses, and virtual properties) you should use `JSON.SerializeDynamic` instead.
`JSON.SerializeDynamic` does not require a generic type parameter, and can cope with subclasses, `object`/`dynamic` members, and [DLR](http://msdn.microsoft.com/en-us/library/dd233052(v=vs.110).aspx) participating 
types such as [ExpandoObject](http://msdn.microsoft.com/en-us/library/system.dynamic.expandoobject(v=vs.110).aspx) and [DynamicObject](http://msdn.microsoft.com/en-us/library/system.dynamic.dynamicobject(v=vs.110).aspx).

### Deserializing

```
    using(var input = new StringReader(myString))
    {
        var result = JSON.Deserialize<MyType>(input);
    }
```

There is also a `Deserialize` method that takes a string as input.

The first time Jil is used to deserialize a given configuration and type pair, it will spend extra time building the deserializer.
Subsequent invocations will be much faster, so if a consistently fast runtime is necessary in your code you may want to "prime the pump"
with an earlier "throw away" deserialization.

### Dynamic Deserialization

```
    using(var input = new StringReader(myString))
    {
    	var result = JSON.DeserializeDynamic(input);
    }
```

There is also a `DeserializeDynamic` method that works directly on strings.

These methods return `dynamic`, and support the following operations:

  - Casts
    * ie. `(int)JSON.DeserializeDynamic("123")`
  - Member access
    * ie. `JSON.DeserializeDynamic(@"{""A"":123}").A`
  - Indexers
    * ie. `JSON.DeserializeDynamic(@"{""A"":123}")["A"]`
    * or `JSON.DeserializeDynamic("[0, 1, 2]")[0]`
  - Foreach loops
    * ie. `foreach(var keyValue in JSON.DeserializeDynamic(@"{""A"":123}")) { ... }`
      - in this example, `keyValue` is a dynamic with `Key` and `Value` properties
    * or `foreach(var item in JSON.DeserializeDynamic("[0, 1, 2]")) { ... }`
      - in this example, `item` is a `dynamic` and will have values 0, 1, and 2
  - Common unary operators (+, -, and !)
  - Common binary operators (&&, ||, +, -, *, /, ==, !=, <, <=, >, and >=)
  - `.Length` & `.Count` on arrays
  - `.ContainsKey(string)` on objects

## Supported Types

Jil will only (de)serialize types that can be reasonably represented as [JSON](http://json.org).

The following types (and any user defined types composed of them) are supported:

  - Strings (including char)
  - Booleans
  - Integer numbers (int, long, byte, etc.)
  - Floating point numbers (float, double, and decimal)
  - DateTimes & DateTimeOffsets
    * See Configuration for further details
  - TimeSpans
    * See Configuration for further details
  - Nullable types
  - Enumerations
    * Including \[Flags\]
  - Guids
    * Only the ["D" format](http://msdn.microsoft.com/en-us/library/97af8hh4.aspx)
  - IList&lt;T&gt; implementations
  - IDictionary&lt;TKey, TValue&gt; implementations where TKey is a string or enumeration

Jil deserializes public fields and properties; the order in which they are serialized is not defined (it is unlikely to be in
declaration order).  The [`DataMemberAttribute.Name` property](http://msdn.microsoft.com/en-us/library/system.runtime.serialization.datamemberattribute.name(v=vs.110).aspx) and [`IgnoreDataMemberAttribute`](http://msdn.microsoft.com/en-us/library/system.runtime.serialization.ignoredatamemberattribute.aspx) are respected by Jil, as is the [ShouldSerializeXXX() pattern](http://msdn.microsoft.com/en-us/library/53b8022e(v=vs.110).aspx).  For situations where `DataMemberAttribute` and `IgnoreDataMemberAttribute` cannot be used, Jil provides the [`JilDirectiveAttribute`](https://github.com/kevin-montrose/Jil/blob/master/Jil/JilDirectiveAttribute.cs) which provides equivalent functionality.

## Configuration

Jil's `JSON.Serialize` and `JSON.Deserialize` methods take an optional `Options` parameter which controls:

  - The format of DateTimes, DateTimeOffsets, and TimeSpans; one of
    * NewtonsoftStyleMillisecondsSinceUnixEpoch, a string
	  - "\/Date(##...##)\/" for DateTimes & DateTimeOffsets
	  - "1.23:45:56.78" for TimeSpans
	* MillisecondsSinceUnixEpoch, a number
	  - for DateTimes & DateTimeOffsets it can be passed directly to [JavaScript's Date() constructor](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date)
	  - for TimeSpans it's simply [TimeSpan.TotalMilliseconds](http://msdn.microsoft.com/en-us/library/system.timespan.totalmilliseconds%28v=vs.110%29.aspx)
	* SecondsSinceUnixEpoch, a number
	  - for DateTimes & DateTimeOffsets this is commonly refered to as [unix time](http://en.wikipedia.org/wiki/Unix_time)
	  - for TimeSpans it's simply [TimeSpan.TotalSeconds](http://msdn.microsoft.com/en-us/library/system.timespan.totalseconds%28v=vs.110%29.aspx)
	* ISO8601, a string
	  - for DateTimes & DateTimeOffsets, ie. "2011-07-14T19:43:37Z"
	    * DateTimes are always serialized in UTC (timezone offset = 00:00), because Local DateTimes cannot reliably roundtrip
		* DateTimeOffsets include their timezone offset when serialized
	  - for TimeSpans, ie. "P40DT11H10M9.4S" 
  - Whether or not to exclude null values when serializing dictionaries, and object members
  - Whether or not to "pretty print" while serializing, which adds extra linebreaks and whitespace for presentation's sake
  - Whether or not the serialized JSON will be used as JSONP (which requires slightly more work be done w.r.t. escaping)
  - Whether or not to include inherited members when serializing

## Benchmarks

Jil aims to be the fastest general purpose JSON (de)serializer for .NET.  Flexibility and "nice to have" features are explicitly discounted
in the pursuit of speed.

These benchmarks were run on a machine with the following specs:

<ul>
 <li>Operating System: Windows 8.1 Enterprise 64-bit (6.3, Build 9600) (9600.winblue_r3.140827-1500)</li>
 <li>System Manufacturer: Apple Inc.</li>
 <li>System Model: MacBookPro11,3</li>
 <li>Processor: Intel(R) Core(TM) i7-4960HQ CPU @ 2.60GHz (8 CPUs), ~2.6GHz</li>
 <li>Memory: 16384MB RAM</li>
 <ul>
  <li>DDR3</li>
  <li>Dual Channel</li>
  <li>798.1 MHZ</li>
 </ul>
</ul>

As with all benchmarks, take these with a grain of salt.

### Serialization

For comparison, here's how Jil stacks up against other popular .NET serializers in a [synthetic benchmark](https://github.com/kevin-montrose/Jil/tree/7915b2e8897024e82628c514d63af596fcfd5013/Benchmark):

 - [Json.NET](http://james.newtonking.com/json) - JSON library included with ASP.NET MVC, version 6.0.7
 - [ServiceStack.Text](https://github.com/ServiceStack/ServiceStack.Text) - JSON, CSV, and JSV library; a part of the [ServiceStack framework](https://github.com/ServiceStack/ServiceStack), version 3.9.71
 - [protobuf-net](https://code.google.com/p/protobuf-net/) - binary serializer for Google's [Protocol Buffers](https://code.google.com/p/protobuf/), version 2.0.0.688
   * __does not__ serialize JSON, included as a baseline

All three libraries are in use at [Stack Exchange](https://stackexchange.com/) in various production roles.

<img src="https://i.imgur.com/kagJdod.png" />

<img src="https://i.imgur.com/ggFu8Kw.png" />

<img src="https://i.imgur.com/moSS1Mv.png" />

Note that the bars in each group of each graph are scaled so that the fastest library is 100.

Numbers, include millisecond timings, can found in [this Google Document](https://docs.google.com/spreadsheets/d/1Jx7DAGopJo3BC0St_L5qHJJrWpZe9x9BCHgdeY9-b-w/edit).

The Question, Answer, and User types are taken from the [Stack Exchange API](http://api.stackexchange.com/).

Data for each type is randomly generated from a fixed seed.  Random text is biased towards ASCII<sup>*</sup>, but includes all unicode.

<sub>*This is meant to simulate typical content from the Stack Exchange API.</sub>

### Deserialization

The same libraries and same types were used to test deserialization.

<img src="https://i.imgur.com/Qvdvo44.png" />

<img src="https://i.imgur.com/QPtJzZV.png" />

<img src="https://i.imgur.com/fESbbgm.png" />

Note that the bars in each group of each graph are scaled so that the fastest library is 100.

Numbers, include millisecond timings, can be found in [the same Google Document](https://docs.google.com/spreadsheets/d/1Jx7DAGopJo3BC0St_L5qHJJrWpZe9x9BCHgdeY9-b-w/edit).

## Tricks

Jil has a lot of tricks to make it fast.  These may be interesting, even if Jil itself is too limited for your use.

### Sigil

Jil does a lot of IL generation to produce tight, focused code.  While possible with [ILGenerator](http://msdn.microsoft.com/en-us/library/system.reflection.emit.ilgenerator.aspx), Jil instead uses the [Sigil library](https://github.com/kevin-montrose/Sigil).
Sigil automatically does a lot of the busy work you'd normally have to do manually to produce ideal IL.
Using Sigil also makes hacking on Jil much more productive, as debuging IL generation without it is pretty slow going.

### Trade Memory For Speed

Jil's internal serializers and deserializers are (in the absense of recursive types) monolithic, and per-type; avoiding extra runtime lookups, and giving
.NET's JIT more context when generating machine code.

The methods Jil create also do no Options checking at serialization time; Options are baked in at first use.  This means
that Jil may create up to 32 different serializers and 8 different deserializers for a single type (though in practice, many fewer).

### Optimizing Member Access Order

Perhaps the [most arcane code in Jil](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/Utils.cs#L52) determines the preferred order to access members, so the CPU doesn't stall waiting for values from memory.

Members are divided up into 4 groups:
<ul>
  <li>Simple
    <ul>
      <li>primitive ValueTypes such as int, double, etc.</li>
    </ul>
  </li>
  <li>Nullable Types</li>
  <li>Recursive Types</li>
  <li>Everything Else</li>
</ul>

Members within each group are ordered by the offset of the fields backing them (properties are decompiled to determine fields they use).

This is a fairly naive implementation of this idea, there's almost certainly more that could be squeezed out especially with regards to consistency of gains.

### Don't Allocate If You Can Avoid It

.NET's GC is excellent, but no-GC is still faster than any-GC.

Jil tries to avoid allocating any reference types, with some exceptions:

 - [a 36-length char\[\]](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/InlineSerializer.cs#L2785) if any integer numbers, DateTimes, or GUIDs are being serialized
 - [a 32-length char\[\]](https://github.com/kevin-montrose/Jil/blob/44aef95ecb762b34827ec22967ea263056b96434/Jil/Deserialize/InlineDeserializer.cs#L64) if any strings, user defined objects, or ISO8601 DateTimes are being deserialized

Depending on the data being deserialized a `StringBuilder` may also be allocated.  If a `TextWriter` does not have an invariant culture, strings may also be allocated when serializing floating point numbers.

### Escaping Tricks

JSON has escaping rules for `\`, `"`, and control characters.  These can be kind of time consuming to deal with. Jil avoids it as much as possible in two ways.

First, all known key names are determined once and baked into the generated delegates [like so](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/InlineSerializer.cs#L980).
Known keys are member names and enumeration values.

Second, rather than lookup encoded characters in a dictionary or a long series of branches Jil does explicit checks for `"` and `\` and turns the rest into
a subtraction and jump table lookup.  This comes out to ~three branches (with mostly consistently taken paths, good for branch prediction in theory) per character.

This works because control characters in .NET strings (bascally UTF-16, but might as well be ASCII for this trick) are sequential, being [0,31].

JSONP also requires escaping of line separator (\u2028) and paragraph separator (\u2029) characters.  When configured to serialize JSONP,
Jil escapes them in the same manner as `\` and `"`.

### Custom Number Formatting

While number formatting in .NET is pretty fast, it has a lot of baggage to handle custom number formatting.

Since JSON has a strict definition of a number, a Write() implementation without configuration is noticeably faster.
To go the extra mile, Jil contains [separate implementations for `int`, `uint`, `ulong`, and `long`](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/Methods.cs#L803).

Jil __does not__ include custom `decimal`, `double`, or `single` Write() implementations, as despite my best efforts I haven't been able to beat the ones built into .NET.
If you think you're up to the challenge, I'd be really interested in seeing code that *is* faster than the included implementations.

### Custom Date Formatting

Similarly to numbers, each of Jil's date formats has a custom Write() implementation.

 - [ISO8601](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/Methods.cs#L142) can be unrolled into a smaller number of `/` and `%` instructions
 - [Newtonsoft-style](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/InlineSerializer.cs#L471) is a subtraction and division, then fed into the custom `long` writing code
 - [Milliseconds since the unix epoch](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/InlineSerializer.cs#L528) is essentially the same
 - [Seconds since the unix epoch](https://github.com/kevin-montrose/Jil/blob/519a0c552e9fb93a4df94eed0b2f9804271f2fef/Jil/Serialize/InlineSerializer.cs#L577) just has a different divisor
 
### Custom Guid Formatting

Noticing a pattern?

Jil has a [custom Guid writer](https://github.com/kevin-montrose/Jil/blob/201a611f1589c07b6a65d4fb07bf1d95616bf1e4/Jil/Serialize/Methods.cs#L100) (which is one of the reasons Jil only supports the D format).

Fun fact about this method, I tested a more branch heavy version (which removed the byte lookup) which turned out to be considerably slower than the built-in method due to [branch prediction failures](http://stackoverflow.com/a/11227902/80572).
Type 4 Guids being random makes for something quite close to the worst case for branch prediciton.

### Different Code For Arrays

Although arrays implement `IList<T>` the JIT generates much better code if you give it array-ish IL to chew on, so Jil does so.

### Special Casing Enumerations With Sequential Values

Many enums end up having sequential values, Jil will exploit this if possible and generate a subtraction and jump table lookup.
Non-sequential enumerations are handled with a long series of branches.

### Custom Number Readers

Just like Jil maintains many different methods for writing integer types, it also maintains [different methods for reading them](https://github.com/kevin-montrose/Jil/blob/44aef95ecb762b34827ec22967ea263056b96434/Jil/Deserialize/Methods.ReadNumbers.cs).  These methods omit unnecessary sign checks, overflow checks, and culture-specific formatting support.

### Automata Based Member Name Lookups

Rather than read a member name into a string or buffer when deserializing, Jil will try to match it one character at a time using an [automata](http://en.wikipedia.org/wiki/Automata_theory).

### Avoid Abstractions If Able

If you're serializing to `string` (as indicated by using a particular [`Serialize<T>`](https://github.com/kevin-montrose/Jil/blob/7915b2e8897024e82628c514d63af596fcfd5013/Jil/JSON.cs#L140) method) Jil will avoid the overhead of virtually dispatching calls against `TextWriter`, and instead statically call against it's own specialized [`StringBuilder`-eqsue class](https://github.com/kevin-montrose/Jil/blob/7915b2e8897024e82628c514d63af596fcfd5013/Jil/Serialize/ThunkWriter.cs#L133).  In the general case Jil prefers to write against a `TextWriter` so as to keep memory pressure low (a real concern in many real world deployments), but when Jil is going to allocate a `string` anyway avoiding virtual dispatch results in a noticeable speed up. 
