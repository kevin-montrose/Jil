<Query Kind="Program" />

void Main()
{
	var dateFormat = new Option<DateTimeFormat>("DateFormat", "UseDateTimeFormat")
	{
		ParamEverPresent = true,
		GetTypeCacheName = v =>
		{
			switch (v)
			{
				case DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch: return "MicrosoftStyle";
				case DateTimeFormat.MillisecondsSinceUnixEpoch: return "Milliseconds";
				case DateTimeFormat.SecondsSinceUnixEpoch: return "Seconds";
				default: return v.ToString();
			}
		}
	};
	var prettyPrint = new Option<bool>("PrettyPrint", "ShouldPrettyPrint");
	var excludeNulls = new Option<bool>("ExcludeNulls", "ShouldExcludeNulls");
	var jsonp = new Option<bool>("JSONP", "IsJSONP");
	var includeInherited = new Option<bool>("IncludeInherited", "ShouldIncludeInherited")
	{
		GetTypeCacheName = v => v ? "Inherited" : ""
	};
	var dateBehavior = new Option<UnspecifiedDateTimeKindBehavior>("UnspecifiedDateTimeKindBehavior", "UseUnspecifiedDateTimeKindBehavior") 
	{
		GetName = v => v == UnspecifiedDateTimeKindBehavior.IsUTC ? "Utc" : "",
		TypeCachePropertyName = "DateTimeKindBehavior"
	};

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

	PipeToFile(@"Options.cs", options.ToString());
	PipeToFile(@"Serialize\TypeCaches.cs", typeCaches.ToString());

	//options.ToString().Dump("Options.cs");
	//typeCaches.ToString().Dump(".cs");
}

public void PipeToFile(string filename, string codeBlock)
{
	const string startComment = @"// Start OptionsGeneration.linq generated content",
				 endComment = @"// End OptionsGeneration\.linq generated content";
	var currentPath = Path.GetDirectoryName(Util.CurrentQueryPath);
	var filePath = Path.Combine(currentPath, filename);
	var re = new Regex($@"({Regex.Escape(startComment)}\n)(.*)({Regex.Escape(endComment)}\n)", RegexOptions.Singleline);

	string t = File.ReadAllText(filePath);

	t = re.Replace(t, $"$1{codeBlock.Trim()}$2");
	t.Dump(filename);
	File.WriteAllText(filePath, t);
}

public class Option<T> where T : struct
{
	static bool[] boolOptions = new bool[] { false, true };

	public Type Type => typeof(T);
	public string TypeShortName => IsBool ? "bool" : Type.Name;
	public bool IsBool => Type == typeof(bool);
	public bool ParamEverPresent { get; set; }
	public string NameFormat { get; set; }
	public string PropertyName { get; }
	public string TypeCachePropertyName { get; set; }
	public string OptionProperty { get; }
	private string ParamName => PropertyName.All(char.IsUpper) ? PropertyName.ToLower() : PropertyName.Substring(0, 1).ToLower() + PropertyName.Substring(1);
	public Func<T, string> GetName { get; set; }
	public Func<T, string> GetTypeCacheName { get; set; }
	
	private T[] _values;
	public T[] Values
	{
		get
		{
			if (_values != null) return _values;
			if (Type == typeof(bool)) _values = boolOptions as T[];
			if (Type.IsEnum) _values = Enum.GetValues(Type).Cast<T>().ToArray();
			return _values;
		}
	}

	public Option(string propertyName, string optionProperty)
	{
		PropertyName = propertyName;
		OptionProperty = OptionProperty;
		if (IsBool) GetName = b => (bool)(object)b ? PropertyName : "";
		else GetName = e => e.ToString();
	}

	public List<OptionPermutation> Permutations()
	{
		var perms = new List<OptionPermutation>();
		for (var i = 0; i < Values.Length; i++)
		{
			var isDefault = i == 0;
			var v = Values[i];
			var vString = IsBool ? v.ToString().ToLower() : $"{Type.Name}.{v}";
			var name = isDefault ? "" : GetName(v);
			var typeCacheName = !isDefault || ParamEverPresent ? (GetTypeCacheName ?? GetName)(v) : "";
			var opt = new OptionPermutation
			{
				Name = name,
				Param = !isDefault || ParamEverPresent ? $"{ParamName}: {vString}" : "",
				TypeCacheName = typeCacheName,
				TypeCacheProperty = $"public {TypeShortName} {TypeCachePropertyName ?? PropertyName} {{ get {{ return {vString}; }} }}"
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

public enum UnspecifiedDateTimeKindBehavior : byte
{
	IsLocal = 0,
	IsUTC
}
public enum DateTimeFormat : byte
{
	MicrosoftStyleMillisecondsSinceUnixEpoch = 0,
    MillisecondsSinceUnixEpoch = 1,
	SecondsSinceUnixEpoch = 2,
	ISO8601 = 3,
	RFC1123 = 4,
}