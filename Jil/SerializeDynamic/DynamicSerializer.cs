using System;
using Jil.Common;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using System.Runtime.CompilerServices;
using Jil.Serialize;
using System.Collections;
using Sigil.NonGeneric;
using System.Reflection;

namespace Jil.SerializeDynamic
{
    class DynamicSerializer
    {
        static readonly Hashtable GetGetMemberCache = new Hashtable();
        static Func<object, object> GetGetMember(Type type, string memberName)
        {
            var key = Tuple.Create(type, memberName);
            var cached = (Func<object, object>)GetGetMemberCache[key];
            if (cached != null) return cached;

            var binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, memberName, type, new[] { CSharpArgumentInfo.Create(0, null) });
            var callSite = CallSite<Func<CallSite, object, object>>.Create(binder);

            lock (GetGetMemberCache)
            {
                cached = (Func<object, object>)GetGetMemberCache[key];
                if (cached != null) return cached;

                GetGetMemberCache[key] = cached = (obj => callSite.Target.Invoke(callSite, obj));
            }

            return cached;
        }

        static readonly ParameterExpression CachedParameterExp = Expression.Parameter(typeof(object));
        static void SerializeDynamicObject(IDynamicMetaObjectProvider dyn, TextWriter stream, Options opts, int depth)
        {
            var quoteColon = "\":";
            if (opts.ShouldPrettyPrint)
            {
                quoteColon = "\": ";
            }

            stream.Write('{');
            depth++;

            var dynType = dyn.GetType();
            var metaObj = dyn.GetMetaObject(CachedParameterExp);

            var first = true;
            foreach (var memberName in metaObj.GetDynamicMemberNames())
            {
                var getter = GetGetMember(dynType, memberName);
                var val = getter(dyn);

                if (val == null && opts.ShouldExcludeNulls) continue;

                if (!first)
                {
                    stream.Write(',');
                }
                first = false;

                if (opts.ShouldPrettyPrint)
                {
                    LineBreakAndIndent(stream, depth);
                }

                stream.Write('"');
                stream.Write(memberName.JsonEscape(jsonp: opts.IsJSONP));
                stream.Write(quoteColon);

                Serialize(stream, val, opts, depth + 1);
            }

            depth--;
            if (opts.ShouldPrettyPrint)
            {
                LineBreakAndIndent(stream, depth);
            }

            stream.Write('}');
        }

        static void LineBreakAndIndent(TextWriter stream, int depth)
        {
            stream.Write('\n');

            switch(depth){
                case 0: return;
                case 1: stream.Write(' '); return;
                case 2: stream.Write("  "); return;
                case 3: stream.Write("   "); return;
                case 4: stream.Write("    "); return;
                case 5: stream.Write("     "); return;
                case 6: stream.Write("      "); return;
                case 7: stream.Write("       "); return;
                case 8: stream.Write("        "); return;
                case 9: stream.Write("         "); return;
                case 10: stream.Write("          "); return;
            }

            for (var i = 0; i < depth; i++)
            {
                stream.Write(' ');
            }
        }

        static readonly Hashtable GetSemiStaticInlineSerializerForCache = new Hashtable();
        static readonly MethodInfo GetSemiStaticInlineSerializerFor = typeof(DynamicSerializer).GetMethod("_GetSemiStaticInlineSerializerFor", BindingFlags.NonPublic | BindingFlags.Static);
        static Action<TextWriter, ForType, int> _GetSemiStaticInlineSerializerFor<ForType>(Options opts)
        {
            var type = typeof(ForType);

            var key = Tuple.Create(typeof(ForType), opts);

            var ret = (Action<TextWriter, ForType, int>)GetSemiStaticInlineSerializerForCache[key];
            if (ret != null) return ret;

            var cacheType = OptionsLookup.GetTypeCacheFor(opts);
            var builder = InlineSerializerHelper.BuildWithDynamism.MakeGenericMethod(type);

            lock (GetSemiStaticInlineSerializerForCache)
            {
                ret = (Action<TextWriter, ForType, int>)GetSemiStaticInlineSerializerForCache[key];
                if (ret != null) return ret;
                GetSemiStaticInlineSerializerForCache[key] = ret = (Action<TextWriter, ForType, int>)builder.Invoke(null, new object[] { cacheType, opts.ShouldPrettyPrint, opts.ShouldExcludeNulls, opts.IsJSONP, opts.UseDateTimeFormat, opts.ShouldIncludeInherited });
            }

            return ret;
        }

        static Hashtable GetSemiStaticSerializerForCache = new Hashtable();
        static Action<TextWriter, object, int> GetSemiStaticSerializerFor(Type type, Options opts)
        {
            var key = Tuple.Create(type, opts);
            var ret = (Action<TextWriter, object, int>)GetSemiStaticSerializerForCache[key];
            if (ret != null) return ret;

            var getSemiStaticSerializer = GetSemiStaticInlineSerializerFor.MakeGenericMethod(type);
            var invoke = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), type, typeof(int)).GetMethod("Invoke");

            var emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(object), typeof(int) }, doVerify: Utils.DoVerify);

            var optsFiled = OptionsLookup.GetOptionsFieldFor(opts);
            emit.LoadField(optsFiled);                              // Options
            emit.Call(getSemiStaticSerializer);                     // Action<TextWriter, Type, int>
            emit.LoadArgument(0);                                   // Action<TextWriter, Type, int> TextWriter
            emit.LoadArgument(1);                                   // Action<TextWriter, Type, int> TextWriter object

            if (type.IsValueType)
            {
                emit.UnboxAny(type);                                // Action<TextWriter, Type, int> TextWriter type
            }
            else
            {
                emit.CastClass(type);                               // Action<TextWriter, Type, int> TextWriter type
            }

            emit.LoadArgument(2);                                   // Action<TextWriter, Type, int> TextWriter type int
            emit.Call(invoke);                                      // --empty--
            emit.Return();                                          // --empty--

            lock (GetSemiStaticSerializerForCache)
            {
                ret = (Action<TextWriter, object, int>)GetSemiStaticSerializerForCache[key];
                if (ret != null) return ret;

                GetSemiStaticSerializerForCache[key] = ret = emit.CreateDelegate<Action<TextWriter, object, int>>(optimizationOptions: Utils.DelegateOptimizationOptions);
            }

            return ret;
        }

        static void SerializeSemiStatically(TextWriter stream, object val, Options opts, int depth)
        {
            var serializer = GetSemiStaticSerializerFor(val.GetType(), opts);

            serializer(stream, val, depth);
        }

        static void SerializeList(TextWriter stream, IEnumerable e, Options opts, int depth)
        {
            var comma = ",";
            if (opts.ShouldPrettyPrint)
            {
                comma = ", ";
            }

            bool isFirst = true;

            stream.Write("[");
            foreach (var i in e)
            {
                if (!isFirst)
                {
                    stream.Write(comma);
                }
                isFirst = false;

                Serialize(stream, i, opts, depth);
            }
            stream.Write("]");
        }

        static bool CanBeBool(dynamic dyn, out bool bit)
        {
            try
            {
                bit = (bool)dyn;
                return true;
            }
            catch { }

            bit = false;
            return false;
        }

        static bool CanBeInteger(dynamic dyn, out ulong integer, out bool negative)
        {
            try
            {
                var asLong = (long)dyn;

                if (asLong < 0)
                {
                    asLong = -asLong;
                    negative = true;
                }
                else
                {
                    negative = false;
                }

                integer = (ulong)asLong;
                return true;
            }
            catch { }

            try
            {
                integer = (ulong)dyn;
                negative = false;

                return true;
            }
            catch { }

            integer = 0;
            negative = false;
            return false;
        }

        static bool CanBeFloatingPoint(dynamic dyn, out double floatingPoint)
        {
            try
            {
                floatingPoint = (double)dyn;
                return true;
            }
            catch { }

            try
            {
                floatingPoint = (float)dyn;
                return true;
            }
            catch { }

            try
            {
                floatingPoint = (double)(decimal)dyn;
                return true;
            }
            catch { }

            floatingPoint = 0;
            return false;
        }

        static bool CanBeDateTime(dynamic dyn, out DateTime dt)
        {
            try
            {
                dt = (DateTime)dyn;
                return true;
            }
            catch { }

            try
            {
                var dto = (DateTimeOffset)dyn;
                dt = dto.UtcDateTime;
                return true;
            }
            catch { }

            dt = DateTime.MinValue;
            return false;
        }

        static bool CanGuid(dynamic dyn, out Guid guid)
        {
            try
            {
                guid = (Guid)dyn;
                return true;
            }
            catch { }

            guid = Guid.Empty;
            return false;
        }

        static bool CanBeString(dynamic dyn, out string str)
        {
            try
            {
                str = (string)dyn;
                return true;
            }
            catch { }

            try
            {
                var c = (char)dyn;
                str = c.ToString();
                return true;
            }
            catch { }

            str = null;
            return false;
        }

        static bool CanBeListAndNotDictionary(dynamic dyn, out IEnumerable enumerable)
        {
            try
            {
                enumerable = (IEnumerable)dyn;

                // if either of these succeed, we're not really a "list"
                try
                {
                    var asDict1 = (IDictionary<string, object>)dyn;
                    return false;
                }
                catch { }
                try
                {
                    var asDict2 = (IDictionary)dyn;
                    return false;
                }
                catch { }

                return true;
            }
            catch { }

            enumerable = null;
            return false;
        }

        public static readonly MethodInfo SerializeMtd = typeof(DynamicSerializer).GetMethod("Serialize");
        public static void Serialize(TextWriter stream, object obj, Options opts, int depth)
        {
            if (obj == null)
            {
                stream.Write("null");
                return;
            }

            var dynObject = obj as IDynamicMetaObjectProvider;
            if (dynObject != null)
            {
                bool bit;
                if(CanBeBool(dynObject, out bit))
                {
                    Serialize(stream, bit, opts, depth);
                    return;
                }

                ulong integer;
                bool negative;
                if(CanBeInteger(dynObject, out integer, out negative))
                {
                    if (negative)
                    {
                        stream.Write('-');
                    }

                    Serialize(stream, integer, opts, depth);
                    return;
                }

                double floatingPoint;
                if (CanBeFloatingPoint(dynObject, out floatingPoint))
                {
                    Serialize(stream, floatingPoint, opts, depth);
                    return;
                }

                DateTime dt;
                if(CanBeDateTime(dynObject, out dt))
                {
                    Serialize(stream, dt, opts, depth);
                    return;
                }

                Guid guid;
                if (CanGuid(dynObject, out guid))
                {
                    Serialize(stream, guid, opts, depth);
                    return;
                }

                string str;
                if (CanBeString(dynObject, out str))
                {
                    Serialize(stream, str, opts, depth);
                    return;
                }

                IEnumerable list;
                if(CanBeListAndNotDictionary(dynObject, out list))
                {
                    SerializeList(stream, list, opts, depth);
                    return;
                }

                SerializeDynamicObject(dynObject, stream, opts, depth);
                return;
            }

            SerializeSemiStatically(stream, obj, opts, depth);
        }
    }
}
