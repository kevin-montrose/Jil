using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;

namespace Jil.Serialize
{
    class CapacityEstimator
    {
        internal const int ListMultiplier = 10;
        internal const int DictionaryMultiplier = 10;

        const int StringEstimate = 20;
        const int CharacterEstimate = 1;
        const int IntEstimate = 3;
        const int BoolEstimate = 5;
        const int GuidEstimate = 36;
        const int DoubleEstimate = 4;
        const int ISO8601Estimate = 20;
        const int SecondsSinceUnixEpochEstimate = 10;
        const int MillisecondsSinceUnixEpochEstimate = 13;
        const int NewtonsoftStyleMillisecondsSinceUnixEpochEstimate = 23;
        const int RecursiveEstimate = 4;

        static int ForPrimitive(Type primType, Options opts)
        {
            if (primType.IsIntegerNumberType())
            {
                return IntEstimate;
            }

            if (primType.IsFloatingPointNumberType())
            {
                return DoubleEstimate;
            }

            if (primType == typeof(string))
            {
                return StringEstimate + 2; // for quotes
            }

            if (primType == typeof(char))
            {
                return CharacterEstimate + 2; // for quotes
            }

            if (primType == typeof(bool))
            {
                return BoolEstimate;
            }

            if (primType == typeof(Guid))
            {
                return GuidEstimate + 2; // for quotes
            }

            if (primType == typeof(DateTime))
            {
                switch (opts.UseDateTimeFormat)
                {
                    case DateTimeFormat.ISO8601: return ISO8601Estimate + 2; // for quotes
                    case DateTimeFormat.SecondsSinceUnixEpoch: return SecondsSinceUnixEpochEstimate;
                    case DateTimeFormat.MillisecondsSinceUnixEpoch: return MillisecondsSinceUnixEpochEstimate;
                    case DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch: return NewtonsoftStyleMillisecondsSinceUnixEpochEstimate + 2; // for quotes
                    default: throw new Exception("Unexpected DateTimeFormat: " + opts.UseDateTimeFormat);
                }
            }

            throw new Exception("Unexpected primitive type: " + primType);
        }

        static int ForEnum(Type enumType, Options opts)
        {
            var longest = Enum.GetNames(enumType).OrderByDescending(_ => _).First();

            var asJson = longest.JsonEscape(opts.IsJSONP);

            return asJson.Length + 2;   // for quotes
        }

        static int LineBreakAndIndent(Options opts, int depth)
        {
            if (opts.ShouldPrettyPrint)
            {
                return 1 + depth;   // line-break & indent
            }

            return 0;
        }

        static int ForObject(Type objType, Options opts, int depth, HashSet<Type> seenTypes)
        {
            if (seenTypes.Contains(objType))
            {
                return RecursiveEstimate;
            }

            seenTypes.Add(objType);

            var flags = BindingFlags.Public | BindingFlags.Instance;

            if (!opts.ShouldIncludeInherited)
            {
                flags |= BindingFlags.DeclaredOnly;
            }

            var props = objType.GetProperties(flags).Where(p => p.GetMethod != null);
            var fields = objType.GetFields(flags);

            var members = props.Cast<MemberInfo>().Concat(fields).ToList();;

            var ret = 0;

            for (var i = 0; i < members.Count; i++)
            {
                if (i == 0)
                {
                    ret += 1;   // {
                    ret += LineBreakAndIndent(opts, depth);
                }
                else
                {
                    ret += 1;   // ,
                }

                var member = members[i];
                var memberType = member.ReturnType();

                var memberLen = member.Name.JsonEscape(opts.IsJSONP).Length;
                memberLen += 2; // quotes

                if (opts.ShouldPrettyPrint)
                {
                    memberLen += 2; // colon-space
                }
                else
                {
                    memberLen += 1; // colon
                }

                memberLen += For(memberType, opts, depth, seenTypes);

                ret += memberLen;

                if (i == members.Count - 1)
                {
                    ret += LineBreakAndIndent(opts, depth - 1);
                    ret += 1;   // }
                }
                else
                {
                    ret += LineBreakAndIndent(opts, depth);
                }
            }

            return ret;
        }

        static int ForList(Type listType, Options opts, int depth, HashSet<Type> seenTypes)
        {
            var ret = 0;
            ret += 1;   // [

            var elemType = listType.GetListInterface().GetGenericArguments()[0];
            var elemLen = For(elemType, opts, depth, seenTypes);

            ret += elemLen * ListMultiplier;

            if (opts.ShouldPrettyPrint)
            {
                ret += (ListMultiplier - 1) * 2;    // comma-space
            }
            else
            {
                ret += (ListMultiplier - 1);        // comma
            }

            ret += 1;   // ]

            return ret;
        }

        static int ForDictionary(Type dictType, Options opts, int depth, HashSet<Type> seenTypes)
        {
            var ret = 0;

            var keyType = dictType.GetDictionaryInterface().GetGenericArguments()[0];
            var valType = dictType.GetDictionaryInterface().GetGenericArguments()[1];

            var keyLen = For(keyType, opts, depth, seenTypes);

            if (keyType != typeof(char) && keyType != typeof(string))
            {
                keyLen += 2;    // quotes
            }

            var valLen = For(valType, opts, depth, seenTypes);

            foreach (var i in Enumerable.Range(0, DictionaryMultiplier))
            {
                if (i == 0)
                {
                    ret += 1;   // {
                    ret += LineBreakAndIndent(opts, depth);
                }
                else
                {
                    ret += 1;   // ,
                }

                var memberLen = keyLen;

                if (opts.ShouldPrettyPrint)
                {
                    memberLen += 2; // colon-space
                }
                else
                {
                    memberLen += 1; // colon
                }

                memberLen += valLen;

                ret += memberLen;

                if (i == DictionaryMultiplier - 1)
                {
                    ret += LineBreakAndIndent(opts, depth - 1);
                    ret += 1;   // }
                }
                else
                {
                    ret += LineBreakAndIndent(opts, depth);
                }
            }

            return ret;
        }

        public static int For(Type forType, Options opts, int depth, HashSet<Type> seenTypes = null)
        {
            seenTypes = seenTypes ?? new HashSet<Type>();

            if (forType.IsNullableType())
            {
                // Assume the nullable typically has a value
                var inner = Nullable.GetUnderlyingType(forType);

                return For(inner, opts, depth, seenTypes);
            }

            if (forType.IsPrimitiveType())
            {
                return ForPrimitive(forType, opts);
            }

            if (forType.IsDictionaryType())
            {
                return ForDictionary(forType, opts, depth + 1, seenTypes);
            }

            if (forType.IsListType())
            {
                return ForList(forType, opts, depth, seenTypes);
            }

            if (forType.IsEnum)
            {
                return ForEnum(forType, opts);
            }

            return ForObject(forType, opts, depth + 1, seenTypes);
        }
    }
}
