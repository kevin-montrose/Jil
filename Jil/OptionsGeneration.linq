<Query Kind="Program" />

void Main()
{
	const string t = "true", f = "false";
	
	var dateFormat = new Option {
			ParamEverPresent = true,
			NameFormat = "{0}",
			ParamFormat = "dateFormat: DateTimeFormat.{0}",
			Values = new[] { "MicrosoftStyleMillisecondsSinceUnixEpoch", "MillisecondsSinceUnixEpoch", "ISO8601", "SecondsSinceUnixEpoch", "RFC1123"} };
	var prettyPrint = new Option {
			NameFormat = "PrettyPrint",
			ParamFormat = "prettyPrint: {0}",
			Values = new[] { f, t } };
	var excludeNulls =  new Option {
			NameFormat = "ExcludeNulls",
			ParamFormat = "excludeNulls: {0}",
			Values = new[] { f, t } };
	var jsonp =  new Option {
			NameFormat = "JSONP",
			ParamFormat = "jsonp: {0}",
			Values = new[] { f, t } };
	var includeInherited = new Option {
			NameFormat = "IncludeInherited",
			ParamFormat = "includeInherited: {0}",
			Values = new[] { f, t } };
	var dateBehavior = new Option {
			NameFormat = "Utc",
			ParamFormat = "unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.{0}",
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
	foreach (var permutation in permutations)
	{
		var name = string.Join("", permutation.Select(p => p.Name));
		var param = string.Join(", ", permutation.Where(p => p.Param != "").Select(p => p.Param));
		if (name == "") name = "Default";
		$@"        public static readonly Options {name} = new Options({param});".Dump();
	}
}

public class Option
{
	public bool ParamEverPresent { get; set; }
	public string NameFormat { get; set; }
	public string ParamFormat { get; set; }
	public string[] Values { get; set; }
	public string Default => Values[0];

	public List<OptionPermutation> Permutations()
	{
		return Values.Select(v =>
			new OptionPermutation
			{
				Name = v != Default ? string.Format(NameFormat, v) : "",
				Param = v != Default || ParamEverPresent ? string.Format(ParamFormat, v) : ""
			}).ToList();
	}
}
public struct OptionPermutation
{
	public string Name { get; set; }
	public string Param { get; set; }
}