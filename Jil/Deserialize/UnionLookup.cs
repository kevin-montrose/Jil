using Jil.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    [Flags]
    internal enum UnionCharsets : byte
    {
        None = 0,
        
        Signed = 1,
        Number = 2,
        Stringy = 4,
        Bool = 8,
        Object = 16,
        Listy = 32,

        Null = 128
    }

    abstract class UnionLookupConfigBase
    {
        public abstract UnionCharsets Charsets { get; }
        public abstract bool AllowsNull { get; }
    }

    static class UnionCharsetArrays
    {
        public static readonly IEnumerable<char> UnionSignedSet = new[] { '-' };
        public static readonly IEnumerable<char> UnionNumberSet = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static readonly IEnumerable<char> UnionStringySet = new[] { '"' };
        public static readonly IEnumerable<char> UnionBoolSet = new[] { 't', 'f' };
        public static readonly IEnumerable<char> UnionObjectSet = new[] { '{' };
        public static readonly IEnumerable<char> UnionListySet = new[] { '[' };

        /// <summary>
        /// Special case, this shouldn't be used in conjuction with types like string or int?; only for the exact null value.
        /// </summary>
        public static readonly IEnumerable<char> UnionNull = new[] { 'n' };
    }

    /// <summary>
    /// Based on the given generic parameter, this classes memebers end up as follows.
    /// 
    /// MinimumChar is the smallest legal char allowed for the lookup, when constructing a switch subtract this from the
    ///    character to be looked up.
    /// 
    /// Lookup is an array of indexes where you lookup by character (after subtracting MinimumCharacter), and get an index into
    ///     CharsetOrder.  Whatever exists at CharsetOrder is the type corresponding to the looked up character.  If the index
    ///     fetched from Lookup is out of range, that means their is no mapped type.
    /// </summary>
    static class UnionLookup<Config> where Config : UnionLookupConfigBase, new()
    {
        public readonly static int MinimumChar;
        public readonly static int[] Lookup;
        public readonly static UnionCharsets[] CharsetOrder;

        static UnionLookup()
        {
            var config = new Config();
            var charsets = config.Charsets;

            var allChars = Enumerable.Empty<char>();
            var allSets = new List<UnionCharsets>();

            if (config.AllowsNull)
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionNull);
                allSets.Add(UnionCharsets.Null);
            }

            if (charsets.HasFlag(UnionCharsets.Signed))
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionSignedSet);
                allSets.Add(UnionCharsets.Signed);
            }
            if (charsets.HasFlag(UnionCharsets.Number))
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionNumberSet);
                allSets.Add(UnionCharsets.Number);
            }
            if (charsets.HasFlag(UnionCharsets.Stringy))
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionStringySet);
                allSets.Add(UnionCharsets.Stringy);
            }
            if (charsets.HasFlag(UnionCharsets.Bool))
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionBoolSet);
                allSets.Add(UnionCharsets.Bool);
            }
            if (charsets.HasFlag(UnionCharsets.Object))
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionObjectSet);
                allSets.Add(UnionCharsets.Object);
            }
            if (charsets.HasFlag(UnionCharsets.Listy))
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionListySet);
                allSets.Add(UnionCharsets.Listy);
            }

            var inOrder = allChars.OrderBy(_ => _).ToList();

            MinimumChar = inOrder.First();
            CharsetOrder = allSets.ToArray();

            var maxChar = inOrder.Last();

            var flightSize = maxChar - MinimumChar + 1;
            
            Lookup = Enumerable.Repeat(-1, flightSize).ToArray();

            foreach (var c in allChars)
            {
                var lIx = c - MinimumChar;
                var set = UnionCharsets.None;
                if (UnionCharsetArrays.UnionSignedSet.Contains(c)) set = UnionCharsets.Signed;
                if (UnionCharsetArrays.UnionNumberSet.Contains(c)) set = UnionCharsets.Number;
                if (UnionCharsetArrays.UnionStringySet.Contains(c)) set = UnionCharsets.Stringy;
                if (UnionCharsetArrays.UnionBoolSet.Contains(c)) set = UnionCharsets.Bool;
                if (UnionCharsetArrays.UnionObjectSet.Contains(c)) set = UnionCharsets.Object;
                if (UnionCharsetArrays.UnionListySet.Contains(c)) set = UnionCharsets.Listy;
                if (UnionCharsetArrays.UnionNull.Contains(c)) set = UnionCharsets.Null;
                
                var ix = allSets.IndexOf(set);
                if (ix == -1) throw new Exception("Unexpected UnionCharsetArrays [" + set + "]");

                Lookup[lIx] = ix;
            }
        }
    }

    internal static class UnionConfigLookup
    {
        static readonly Hashtable NullCache = new Hashtable();
        static readonly Hashtable NonNullCache = new Hashtable();

        static readonly ModuleBuilder ModBuilder;
        static readonly AssemblyBuilder AsmBuilder;

        static UnionConfigLookup()
        {
            AsmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("JilUnionConfigLookupTypes"), AssemblyBuilderAccess.Run);
            ModBuilder = AsmBuilder.DefineDynamicModule("UnionConfigLookupTypes");
        }

        public static Type Get(UnionCharsets ucs, bool allowsNull)
        {
            var cache = allowsNull ? NullCache : NonNullCache;

            var cached = (Type)cache[ucs];
            if (cached != null) return cached;

            lock (cache)
            {
                cached = (Type)cache[ucs];
                if (cached != null) return cached;

                // TODO: This should be done in Sigil, probably a defect that I can't
                var newType = ModBuilder.DefineType(ucs.ToString()+"_"+allowsNull, TypeAttributes.NotPublic | TypeAttributes.Class, typeof(UnionLookupConfigBase));
                var field = newType.DefineField("_Charsets", typeof(UnionCharsets), FieldAttributes.Private | FieldAttributes.InitOnly);
                var prop = newType.DefineProperty("Charsets", PropertyAttributes.None, typeof(UnionCharsets), Type.EmptyTypes);

                var getE = newType.DefineMethod("getCharsets", MethodAttributes.Public | MethodAttributes.Virtual, typeof(UnionCharsets), Type.EmptyTypes);
                var il = getE.GetILGenerator();
                il.Emit(OpCodes.Ldc_I4, (int)ucs);
                il.Emit(OpCodes.Conv_U1);
                il.Emit(OpCodes.Ret);

                var shouldOverride = typeof(UnionLookupConfigBase).GetProperty("Charsets").GetMethod;
                newType.DefineMethodOverride(getE, shouldOverride);

                var allowE = newType.DefineMethod("getAllowsNull", MethodAttributes.Public | MethodAttributes.Virtual, typeof(bool), Type.EmptyTypes);
                il = allowE.GetILGenerator();
                if (allowsNull)
                {
                    il.Emit(OpCodes.Ldc_I4_1);
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4_0);
                }
                il.Emit(OpCodes.Ret);

                shouldOverride = typeof(UnionLookupConfigBase).GetProperty("AllowsNull").GetMethod;
                newType.DefineMethodOverride(allowE, shouldOverride);

                var type = newType.CreateType();

                cache[ucs] = cached = type;

                return cached;
            }
        }
    }
}
