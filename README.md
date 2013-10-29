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

=== TODO: Write up Jil's crazy optimizations ===
