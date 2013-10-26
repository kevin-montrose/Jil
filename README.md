### Jil (WIP)

A fast JSON serializer, built on [Sigil](https://github.com/kevin-montrose/Sigil) with a number of somewhat crazy optimization tricks.

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

## Supported Types

Jil will only serialize types that can be reasonably represented as [JSON](http://json.org).

The following types (and any user defined types composed of them) are supported:

  - Strings (including char)
  - Booleans
  - Integer numbers (int, long, byte, etc.)
  - Floating point numbers (float, double, and decimal)
  - DateTime
    * See Configuration for further details
  - Nullable types
  - Enumerations
  - IList&lt;T&gt; implementations
  - IDictionary&lt;TKey, TValue&gt; implementations where TKey is a string or enumeration

Jil serializes all public fields and properties; the order in which they are serialized is not defined (it is unlikely to be in
declaration order).

## Configuration

Jil's `JSON.Serialize` method takes an optional `Options` object which controls:

  - The format of serialized DateTimes, one of
    * NewtonsoftStyleMillisecondsSinceUnixEpoch, a string, ie. "\/Date(##...##)\/"
	* MillisecondsSinceUnixEpoch, a number, which can be passed directly to JavaScript's Date() constructor
	* SecondsSinceUnixEpoch, a number, commonly refered to as [unix time](http://en.wikipedia.org/wiki/Unix_time)
	* ISO8601, a string, ie. "2011-07-14T19:43:37Z"
  - Whether or not to exclude null values when serializing dictionaries, and object members
  - Whether or not to "pretty print" while serializing, which adds extra linebreaks and whitespace for presentation's sake

## Benchmarks

Jil aims to be the fastest general purpose JSON serializer for .NET.  Flexibility and "nice to have" features are explicitly discounted
in the pursuit of speed.

For comparison, here's how Jil stacks up against other popular .NET serializers.

=== TODO: Add the actual benchmarks ===

## Tricks

=== TODO: Write up Jil's crazy optimizations ===