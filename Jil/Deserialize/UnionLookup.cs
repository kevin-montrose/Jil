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
        
        Nullable = 1,
        Signed = 2,
        Number = 4,
        Stringy = 8,
        Bool = 16,
        Object = 32,
        Listy = 64
    }

    abstract class UnionLookupConfigBase
    {
        public abstract UnionCharsets Charsets { get; }
    }

    static class UnionCharsetArrays
    {
        public static readonly IEnumerable<char> UnionNullableSet = new[] { 'n' };
        public static readonly IEnumerable<char> UnionSignedSet = new[] { '-' };
        public static readonly IEnumerable<char> UnionNumberSet = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static readonly IEnumerable<char> UnionStringySet = new[] { '"' };
        public static readonly IEnumerable<char> UnionBoolSet = new[] { 't', 'f' };
        public static readonly IEnumerable<char> UnionObjectSet = new[] { '{' };
        public static readonly IEnumerable<char> UnionListySet = new[] { '[' };
    }

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
            if (charsets.HasFlag(UnionCharsets.Nullable))
            {
                allChars = allChars.Concat(UnionCharsetArrays.UnionNullableSet);
                allSets.Add(UnionCharsets.Nullable);
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
            Lookup = Enumerable.Repeat(-1, maxChar - MinimumChar + 1).ToArray();

            foreach (var c in allChars)
            {
                var set = UnionCharsets.None;
                if (UnionCharsetArrays.UnionNullableSet.Contains(c)) set = UnionCharsets.Nullable;
                if (UnionCharsetArrays.UnionSignedSet.Contains(c)) set = UnionCharsets.Signed;
                if (UnionCharsetArrays.UnionNumberSet.Contains(c)) set = UnionCharsets.Number;
                if (UnionCharsetArrays.UnionStringySet.Contains(c)) set = UnionCharsets.Stringy;
                if (UnionCharsetArrays.UnionBoolSet.Contains(c)) set = UnionCharsets.Bool;
                if (UnionCharsetArrays.UnionObjectSet.Contains(c)) set = UnionCharsets.Object;
                if (UnionCharsetArrays.UnionListySet.Contains(c)) set = UnionCharsets.Listy;

                var ix = allSets.IndexOf(set);
                if (ix == -1) throw new Exception("Unexpected UnionCharsetArrays [" + set + "]");

                var lIx = c - MinimumChar;
                Lookup[lIx] = ix;
            }
        }
    }

    internal static class UnionConfigLookup
    {
        static readonly Hashtable Cache = new Hashtable();

        static readonly ModuleBuilder ModBuilder;
        static readonly AssemblyBuilder AsmBuilder;

        static UnionConfigLookup()
        {
            AsmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("JilUnionConfigLookupTypes"), AssemblyBuilderAccess.Run);
            ModBuilder = AsmBuilder.DefineDynamicModule("UnionConfigLookupTypes");
        }

        public static Type Get(UnionCharsets ucs)
        {
            var cached = (Type)Cache[ucs];
            if (cached != null) return cached;

            lock (Cache)
            {
                cached = (Type)Cache[ucs];
                if (cached != null) return cached;

                var newType = ModBuilder.DefineType(ucs.ToString(), TypeAttributes.NotPublic | TypeAttributes.Class, typeof(UnionLookupConfigBase));
                var field = newType.DefineField("_Charsets", typeof(UnionCharsets), FieldAttributes.Private | FieldAttributes.InitOnly);
                var prop = newType.DefineProperty("Charsets", PropertyAttributes.None, typeof(UnionCharsets), Type.EmptyTypes);

                var getE = newType.DefineMethod("getCharsets", MethodAttributes.Public | MethodAttributes.Virtual, typeof(UnionCharsets), Type.EmptyTypes);
                var il = getE.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
                il.Emit(OpCodes.Ret);

                var shouldOverride = typeof(UnionLookupConfigBase).GetProperty("Charsets").GetMethod;
                newType.DefineMethodOverride(getE, shouldOverride);

                var consE = newType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
                il = consE.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, (int)ucs);
                il.Emit(OpCodes.Conv_I1);
                il.Emit(OpCodes.Stfld, field);
                il.Emit(OpCodes.Ret);

                var type = newType.CreateType();

                Cache[ucs] = cached = type;

                return cached;
            }
        }
    }
}
