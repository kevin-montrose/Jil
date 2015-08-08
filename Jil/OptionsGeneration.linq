<Query Kind="Program" />

void Main()
{
	const string t = "true", f = "false";
	
	var dateFormat = new Option {
			ParamEverPresent = true,
			NameFormat = "{0}",
			ParamFormat = "dateFormat: DateTimeFormat.{0}",
			PropertyFormat = "public DateTimeFormat DateFormat {{ get {{ return DateTimeFormat.{0}; }} }}",
			Values = new[] { "MicrosoftStyleMillisecondsSinceUnixEpoch", "MillisecondsSinceUnixEpoch", "ISO8601", "SecondsSinceUnixEpoch", "RFC1123" },
			TypeCacheNames = new[] { "MicrosoftStyle", "Milliseconds", null, "Seconds", null } };
	var prettyPrint = new Option {
			NameFormat = "PrettyPrint",
			ParamFormat = "prettyPrint: {0}",
			PropertyFormat = "public bool PrettyPrint {{ get {{ return {0}; }} }}",
			Values = new[] { f, t } };
	var excludeNulls =  new Option {
			NameFormat = "ExcludeNulls",
			ParamFormat = "excludeNulls: {0}",
			PropertyFormat = "public bool ExcludeNulls {{ get {{ return {0}; }} }}",
			Values = new[] { f, t } };
	var jsonp =  new Option {
			NameFormat = "JSONP",
			ParamFormat = "jsonp: {0}",
			PropertyFormat = "public bool JSONP {{ get {{ return {0}; }} }}",
			Values = new[] { f, t } };
	var includeInherited = new Option {
			NameFormat = "IncludeInherited",
			ParamFormat = "includeInherited: {0}",
			PropertyFormat = "public bool IncludeInherited {{ get {{ return {0}; }} }}",
			TypeCacheNames = new[] { "", "Inherited" },
			Values = new[] { f, t } };
	var dateBehavior = new Option {
			NameFormat = "Utc",
			ParamFormat = "unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.{0}",
			PropertyFormat = "public UnspecifiedDateTimeKindBehavior DateTimeKindBehavior {{ get {{ return UnspecifiedDateTimeKindBehavior.{0}; }} }}",
		    Values = new[] { "IsLocal", "IsUTC" } };


	var permutations = from df in dateFormat.Permutations()
					   from pp in prettyPrint.Permutations()
					   from en in excludeNulls.Permutations()
					   from jp in jsonp.Permutations()
					   from ii in includeInherited.Permutations()
					   from db in dateBehavior.Permutations()
					   select new List<OptionPermutation> { 
					   		df, pp, en, jp, ii, db
					   };
	permutations = permutations.Reverse()
							   .OrderBy (p => p[0].Name)
							   .ThenBy(p => p.Count(ip => ip.Param != ""));
							   
	StringBuilder options = new StringBuilder(),
				  typeCaches = new StringBuilder();
							   
	foreach (var permutation in permutations)
	{
		var name = string.Join("", permutation.Select(p => p.Name));
		var typeCacheName = string.Join("", permutation.Select(p => p.TypeCacheName));
		var param = string.Join(", ", permutation.Where(p => p.Param != "").Select(p => p.Param));
		var typeCacheProperties = string.Join("\n        ", permutation.Select(p => p.TypeCacheProperty));
		if (name == "") name = "Default";
		options.AppendLine($@"        public static readonly Options {name} = new Options({param});");
		typeCaches.AppendLine($@"
    class {typeCacheName} : ISerializeOptions
    {{
        {typeCacheProperties}
    }}");
	}

	options.ToString().Dump("Options.cs");
	typeCaches.ToString().Dump("TypeCaches.cs");
}

public class Option
{
	public bool ParamEverPresent { get; set; }
	public string NameFormat { get; set; }
	public string ParamFormat { get; set; }
	public string PropertyFormat { get; set; }
	public string[] Values { get; set; }
	public string[] TypeCacheNames { get; set; }
	public string Default => Values[0];

	public List<OptionPermutation> Permutations()
	{
		var perms = new List<OptionPermutation>();
		for (var i = 0; i < Values.Length; i++)
		{
			var v = Values[i];
			var name = v != Default ? string.Format(NameFormat, v) : "";
			var typeCacheName = TypeCacheNames?[i] ?? name;
			var opt = new OptionPermutation
			{
				Name = name,
				Param = v != Default || ParamEverPresent ? string.Format(ParamFormat, v) : "",
				TypeCacheName = typeCacheName,
				TypeCacheProperty = string.Format(PropertyFormat, v)
			};
			perms.Add(opt);
		}
		return perms;
	}
}
public struct OptionPermutation
{
	public string Name { get; set; }
	public string TypeCacheName { get; set; }
	public string Param { get; set; }
	public string TypeCacheProperty { get; set; }
}