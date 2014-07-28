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
        static void SerializeDynamicObject(IDynamicMetaObjectProvider dyn, TextWriter stream, Options opts, int depth)
        {
            stream.Write("{");

            var dynType = dyn.GetType();
            var param = Expression.Parameter(typeof(object));
            var metaObj = dyn.GetMetaObject(param);

            var first = true;

            // TODO: Pretty print and exclude null support
            foreach (var memberName in metaObj.GetDynamicMemberNames())
            {
                if (!first)
                {
                    stream.Write(",");
                }

                first = false;

                var binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, memberName, dynType, new[] { CSharpArgumentInfo.Create(0, null) });
                var callSite = CallSite<Func<CallSite, object, object>>.Create(binder);

                var val = callSite.Target.Invoke(callSite, dyn);

                stream.Write("\"" + memberName.JsonEscape(jsonp: true) + "\":");
                Serialize(stream, val, opts, depth + 1);
            }

            stream.Write("}");
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

        public static readonly MethodInfo SerializeMtd = typeof(DynamicSerializer).GetMethod("Serialize");
        public static void Serialize(TextWriter stream, object obj, Options opts, int depth)
        {
            if (obj == null)
            {
                stream.Write("null");
                return;
            }

            var objType = obj.GetType();

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

                SerializeDynamicObject(dynObject, stream, opts, depth);
                return;
            }

            SerializeSemiStatically(stream, obj, opts, depth);
        }
    }
}
