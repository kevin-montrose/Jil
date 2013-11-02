### Jil (WIP)

A fast JSON serializer, built on [Sigil](https://github.com/kevin-montrose/Sigil) with a number of somewhat crazy optimization tricks.

While *usable* in it's current state, Jil is far from finished.  It should be treated as a Work In Progress, don't use it for anything
serious just yet...

## Usage

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

The first time Jil is used to serialize a given configuration and type pair, it will spend extra time building the serializer.
Subsequent invocations will be much faster, so if a consistently fast runtime is necessary in your code you may want to "prime the pump"
with an earlier "throw away" serialization.

Note, at this time Jil **does not** include a JSON _deserializer_.

## Supported Types

Jil will only serialize types that can be reasonably represented as [JSON](http://json.org).

The following types (and any user defined types composed of them) are supported:

  - Strings (including char)
  - Booleans
  - Integer numbers (int, long, byte, etc.)
  - Floating point numbers (float, double, and decimal)
  - DateTimes
    * See Configuration for further details
  - Nullable types
  - Enumerations
  - Guids
    * Only the ["D" format](http://msdn.microsoft.com/en-us/library/97af8hh4.aspx)
  - IList&lt;T&gt; implementations
  - IDictionary&lt;TKey, TValue&gt; implementations where TKey is a string or enumeration

Jil serializes all public fields and properties; the order in which they are serialized is not defined (it is unlikely to be in
declaration order).

## Configuration

Jil's `JSON.Serialize` method takes an optional `Options` parameter which controls:

  - The format of serialized DateTimes, one of
    * NewtonsoftStyleMillisecondsSinceUnixEpoch, a string, ie. "\/Date(##...##)\/"
	* MillisecondsSinceUnixEpoch, a number, which can be passed directly to [JavaScript's Date() constructor](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date)
	* SecondsSinceUnixEpoch, a number, commonly refered to as [unix time](http://en.wikipedia.org/wiki/Unix_time)
	* ISO8601, a string, ie. "2011-07-14T19:43:37Z"
  - Whether or not to exclude null values when serializing dictionaries, and object members
  - Whether or not to "pretty print" while serializing, which adds extra linebreaks and whitespace for presentation's sake
  - Whether or not the JSON will be used as JSONP (which requires slightly more work be done w.r.t. escaping)

## Benchmarks

Jil aims to be the fastest general purpose JSON serializer for .NET.  Flexibility and "nice to have" features are explicitly discounted
in the pursuit of speed.

For comparison, here's how Jil stacks up against other popular .NET serializers in a [synthetic benchmark](https://github.com/kevin-montrose/Jil/tree/3ccb091e1f2659e5d6832518657ae9e3a42e3634/Benchmark):

 - [Json.NET](http://james.newtonking.com/json) - JSON library included with ASP.NET MVC
 - [ServiceStack.Text](https://github.com/ServiceStack/ServiceStack.Text) - JSON, CSV, and JSV library; a part of the [ServiceStack framework](https://github.com/ServiceStack/ServiceStack)
 - [protobuf-net](https://code.google.com/p/protobuf-net/) - binary serializer for Google's [Protocol Buffers](https://code.google.com/p/protobuf/)
   * __does not__ serialize JSON, included as a baseline

All three libraries are in use at [Stack Exchange](https://stackexchange.com/) in various production roles.

<img src="https://i.imgur.com/DBpzOyt.png" />

<img src="https://i.imgur.com/nUb74Mv.png" />

<img src="https://i.imgur.com/3zGueX0.png" />

Numbers can found in [this Google Document](https://docs.google.com/spreadsheet/ccc?key=0AjfqnvvE279FdENqWE5QTVhsSjZUMV9MQVg1SV9TNnc&usp=sharing).

The Question, Answer, and User types are taken from the [Stack Exchange API](http://api.stackexchange.com/).

Data for each type is randomly generated from a fixed seed.  Random text is biased towards ASCII<sup>*</sup>, but includes all unicode.

This benchmark was run on a machine with the following specs:

<ul>
 <li>Operating System: Windows 8 Enterprise 64-bit (6.2, Build 9200) (9200.win8_gdr.130531-1504)</li>
 <li>System Manufacturer: Apple Inc.</li>
 <li>System Model: MacBookPro8,2</li>
 <li>Processor: Intel(R) Core(TM) i7-2860QM CPU @ 2.50GHz (8 CPUs), ~2.5GHz</li>
 <li>Memory: 8192MB RAM</li>
 <ul>
  <li>DDR3</li>
  <li>Dual Channel</li>
  <li>665.2 MHZ</li>
 </ul>
</ul>

As with all benchmarks, take these with a grain of salt.

<sub>*This is meant to simulate typical content from the Stack Exchange API.</sub>

## Tricks

Jil has a lot of tricks to make it fast.  These may be interesting, even if Jil itself is too limitted for your use.

### Sigil

Jil does a lot of IL generation to produce tight, focus code.  While possible with [ILGenerator](http://msdn.microsoft.com/en-us/library/system.reflection.emit.ilgenerator.aspx), Jil instead uses the [Sigil library](https://github.com/kevin-montrose/Sigil).
Sigil automatically does a lot of the busy work you'd normally have to do manually to produce ideal IL.
Using Sigil also makes hacking on Jil much more productive, as debuging IL generation without it is pretty slow going.

### Trade Memory For Speed

Jil's internal serializers are (in the absense of recursive types) monolithic, and per-type; avoiding extra runtime lookups, and giving
.NET's JIT more context when generating machine code.

The serializers Jil create also do no Options checking at serialization time, Options are baked in at first use.  This means
that Jil may create up to 32 different serializers for a single type (though in practice, many fewer).

### Optimizing Member Access Order

Perhaps the [most arcane code in Jil](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/Utils.cs#L52) determines the preferred order to access members, so the CPU doesn't stall waiting for values from memory.

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

This is a fairly naïve implementation of this idea, there's almost more that could be squeezed out especially with regards to consistency of gains.

### Don't Allocate If You Can Avoid It

.NET's GC is excellent, but no-GC is still faster than any-GC.

Jil tries to avoid allocating any reference types, with following exceptions:

 - [a 36-length char\[\]](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/InlineSerializer.cs#L2720) if any integer numbers, DateTimes, or GUIDs are being serialized
 - one byte[] per GUID being serialized, as a consequence of using [Guid.ToByteArray](http://msdn.microsoft.com/en-us/library/system.guid.tobytearray.aspx)

### Escaping Tricks

JSON has escaping rules for `\`, `"`, and control characters.  These can be kind be time consuming to deal with, Jil avoids as much as possible in two ways.

First, all known key names are once and baked into the generated delegates [like so](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/InlineSerializer.cs#L1063).
Known keys are member names and enumeration values.

Second, rather than lookup encoded characters in a dictionary or a long series of branches Jil does explicit checks for `"` and `\` and turns the rest into
a subtraction and jump table lookup.  This comes out to ~three branches (with mostly consistently taken paths, good for branch prediction in theory) per character.

This works because control characters in .NET strings (bascally UTF-16, but might as well be ASCII for this trick) are sequential, being [0,31].

JSONP also requires escaping of line separator (\u2028) and paragraph separator (\u2029) characters.  When configured to serialize JSONP,
Jil escapes them in the same manner as `\` and `"`.

### Custom Number Formatting

While number formatting in .NET is pretty fast, it has a lot of baggage to handle custom number formatting.

Since JSON has a strict definition of a number, a Write() implementation without configuration is noticeably faster.
To go the extra mile, Jil contains [separate implementations for `int`, `uint`, `ulong`, and `long`](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/Methods.cs#L495).

Jil __does not__ include custom `decimal`, `double`, or `single` Write() implementations, as despite my best efforts I haven't been able to beat the one's built into .NET.
If you think you're up to the challenge, I'd be really interested in seeing code that *is* faster than the included implementations.

### Custom Date Formatting

Similarly to numbers, each of Jil's date formats has a custom Write() implementation.

 - [ISO8601](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/Methods.cs#L142) can be unrolled into a smaller number of `/` and `%` instructions
 - [Newtonsoft-style](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/InlineSerializer.cs#L471) is a subtraction and division, then fed into the custom `long` writing code
 - [Milliseconds since the unix epoch](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/InlineSerializer.cs#L528) is essentially the same
 - [Seconds since the unix epoch](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/InlineSerializer.cs#L577) just has a different divisor
 
### Custom Guid Formatting

Noticing a pattern?

Jil has a [custom Guid writer](https://github.com/kevin-montrose/Jil/blob/master/Jil/Serialize/Methods.cs#L18) (which is one of the reason's Jil only supports the D format).

Fun fact about this method, I tested a more branch heavy version (which removed the byte lookup) which turned out to be considerably slower than the built-in method due to [branch prediction failures](http://stackoverflow.com/a/11227902/80572).
Type 4 Guids being random makes for something quite close to the worst case for branch prediciton.

### Different Code For Arrays

Although arrays implement `IList<T>` the JIT generates much better code if you give it array-ish IL to chew on, so Jil does so.

### Special Casing Enumerations With Sequential Values

Many enums end up having sequential values, Jil will exploit this if possible and generate a subtraction and jump table lookup.
Non-sequential enumerations are handled with a long series of branches.

