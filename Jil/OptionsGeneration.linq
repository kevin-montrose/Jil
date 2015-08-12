<Query Kind="Program" />

const string startComment = @"// Start OptionsGeneration.linq generated content",
			   endComment = @"// End OptionsGeneration.linq generated content";

void Main()
{
	var optionList = new List<Option> {
		new Option<DateTimeFormat>("DateFormat", "UseDateTimeFormat")
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
		},
		new Option<bool>("PrettyPrint", "ShouldPrettyPrint"),
		new Option<bool>("ExcludeNulls", "ShouldExcludeNulls"),
		new Option<bool>("JSONP", "IsJSONP"),
		new Option<bool>("IncludeInherited", "ShouldIncludeInherited")
		{
			GetTypeCacheName = v => v ? "Inherited" : ""
		},
		new Option<UnspecifiedDateTimeKindBehavior>("UnspecifiedDateTimeKindBehavior", "UseUnspecifiedDateTimeKindBehavior")
		{
			GetName = v => v == UnspecifiedDateTimeKindBehavior.IsUTC ? "Utc" : "",
			TypeCachePropertyName = "DateTimeKindBehavior"
		}
	};

	var permutations = from df in optionList[0].Permutations
					   from pp in optionList[1].Permutations
					   from en in optionList[2].Permutations
					   from jp in optionList[3].Permutations
					   from ii in optionList[4].Permutations
					   from db in optionList[5].Permutations
					   select new List<OptionPermutation> { 
					   		df, pp, en, jp, ii, db
					   };
	permutations = permutations.Reverse()
							   .OrderBy (p => p[0].Name)
							   .ThenBy(p => p.Count(ip => ip.Param != ""));
							   
	StringBuilder options = new StringBuilder(),
				  typeCaches = new StringBuilder(),
				  writeSwitch = new StringBuilder(),
				  thunkerSwitch = new StringBuilder();
							   
	foreach (var permutation in permutations)
	{
		var name = string.Join("", permutation.Select(p => p.Name));
		if (name == "") name = "Default";
		options.AppendLine($@"        public static readonly Options {name} = new Options({string.Join(", ", permutation.Where(p => p.Param != "").Select(p => p.Param))});");

		typeCaches.AppendLine($@"
    class {permutation.GetTypeCacheName()} : ISerializeOptions
    {{
        {string.Join("\n        ", permutation.Select(p => p.TypeCacheProperty))}
    }}");
	}

	WriteSwitchTree(writeSwitch, optionList, "return TypeCache<{0}, T>.Get();"); //(output, data, 0)
	WriteSwitchTree(thunkerSwitch, optionList, "return TypeCache<{0}, T>.GetToString();");

	PipeToFile(@"Options.cs", options.ToString());
	PipeToFile(@"Serialize\TypeCaches.cs", typeCaches.ToString());
	PipeToFile(@"JSON.cs", writeSwitch.ToString(), "GetWriterAction");
	PipeToFile(@"JSON.cs", thunkerSwitch.ToString(), "GetThunkerDelegate");
}

void WriteSwitchTree(StringBuilder sb, IEnumerable<Option> options, string format, int level = 3, Stack<OptionPermutation> stack = null)
{
	int indent = level * 2;
	if (stack == null)
	{
		stack = new Stack<OptionPermutation>();
	}
	if (!options.Any())
	{
		sb.Indent(level).AppendFormat(format, stack.Reverse().ToList().GetTypeCacheName()).AppendLine();
		return;
	}
	var option = options.First();

	sb.Indent(level).AppendFormat("switch (options.{0})", option.OptionProperty).AppendLine()
	  .Indent(level).Append("{").AppendLine();

	foreach(var p in option.Permutations)
	{
	    stack.Push(p);
		sb.Indent(level+1).AppendFormat("case {0}:", p.OptionCase).AppendLine();
		WriteSwitchTree(sb, options.Skip(1), format, level+2, stack);
		stack.Pop();
	}

	sb.Indent(level).Append("}").AppendLine();
	if (stack.Count > 0) sb.Indent(level).Append("break;").AppendLine();
}

void PipeToFile(string filename, string codeBlock, string suffix = "")
{
	string filePath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), filename),
				  t = File.ReadAllText(filePath);
	if (!string.IsNullOrEmpty(suffix)) suffix = ": " + suffix;

	var re = new Regex($@"({Regex.Escape(startComment + suffix)}\s*)(.*)(\n\s*{Regex.Escape(endComment + suffix)})", RegexOptions.Singleline);
	//t = re.Replace(t, m => m.Groups[0].Value + codeBlock.Trim() + m.Groups[2].Value, 1).Dump();
	t = re.Replace(t, $"$1{codeBlock.Trim()}$3").Dump();
	File.WriteAllText(filePath, t);
}

public abstract class Option
{
	public bool ParamEverPresent { get; set; }
	public string NameFormat { get; set; }
	public string PropertyName { get; }
	public string TypeCachePropertyName { get; set; }
	public string OptionProperty { get; }
	protected string ParamName => PropertyName.All(char.IsUpper) ? PropertyName.ToLower() : PropertyName.Substring(0, 1).ToLower() + PropertyName.Substring(1);

	public string[] ValueStrings { get; set; }
	
	public Option(string propertyName, string optionProperty)
	{
		PropertyName = propertyName;
		OptionProperty = optionProperty;
	}
	
	private List<OptionPermutation> _permutations;
	public List<OptionPermutation> Permutations => _permutations ?? (_permutations = GetPermutations());
    protected abstract List<OptionPermutation> GetPermutations();
}

public class Option<T> : Option where T : struct
{
	public Type Type => typeof(T);
	public string TypeShortName => IsBool ? "bool" : Type.Name;
	public bool IsBool => Type == typeof(bool);
	public Func<T, string> GetName { get; set; }
	public Func<T, string> GetTypeCacheName { get; set; }
	private T[] _values;
	public T[] Values
	{
		get
		{
			if (_values != null) return _values;
			if (Type == typeof(bool)) _values = new [] { false, true } as T[];
			if (Type.IsEnum) _values = Enum.GetValues(Type).Cast<T>().ToArray();
			return _values;
		}
	}

	public Option(string propertyName, string optionProperty) : base (propertyName, optionProperty)
	{
		if (IsBool) GetName = b => (bool)(object)b ? PropertyName : "";
		else GetName = e => e.ToString();
	}
	
	protected override List<OptionPermutation> GetPermutations()
    {
		var _permutations = new List<OptionPermutation>();
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
				TypeCacheProperty = $"public {TypeShortName} {TypeCachePropertyName ?? PropertyName} {{ get {{ return {vString}; }} }}",
				Type = Type,
				OptionProperty = OptionProperty,
				OptionCase = vString
			};
			_permutations.Add(opt);
		}
		return _permutations;
	}
}
public struct OptionPermutation
{
	public string Name { get; set; }
	public string TypeCacheName { get; set; }
	public string Param { get; set; }
	public string TypeCacheProperty { get; set; }
	public Type Type { get; set; }
	public string OptionProperty { get; set; }
	public string OptionCase { get; set; }
}
public static class Extensions
{
	public static string GetTypeCacheName(this List<OptionPermutation> permutation)
	{
		return string.Join("", permutation.Select(p => p.TypeCacheName));
	}
	public static StringBuilder Indent(this StringBuilder sb, int level)
	{
		return sb.Append(' ', level * 4);
	}
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