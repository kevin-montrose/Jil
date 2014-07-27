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
                Serialize(stream, val, opts, depth++);
            }

            stream.Write("}");
        }

        static readonly Hashtable InlineSerializerCache = new Hashtable();
        static readonly MethodInfo GetInlineSerializerFor = typeof(DynamicSerializer).GetMethod("_GetInlineSerializerFor", BindingFlags.NonPublic | BindingFlags.Static);
        static Action<TextWriter, ForType, int> _GetInlineSerializerFor<ForType>(Options opts)
        {
            var type = typeof(ForType);

            var key = Tuple.Create(type, opts);
            var ret = (Action<TextWriter, ForType, int>)InlineSerializerCache[key];

            if (ret != null) 
            {
                return ret;
            }

            var buildStaticSerializer = typeof(InlineSerializerHelper).GetMethod("Build");
            buildStaticSerializer = buildStaticSerializer.MakeGenericMethod(type);

            var parameters = new object[] { typeof(NullTypeCache<>), opts.ShouldPrettyPrint, opts.ShouldExcludeNulls, opts.IsJSONP, opts.UseDateTimeFormat, opts.ShouldIncludeInherited, null };

            lock(InlineSerializerCache)
            {
                ret = (Action<TextWriter, ForType, int>)InlineSerializerCache[key];
                if (ret != null) return ret;

                InlineSerializerCache[key] = ret = (Action<TextWriter, ForType, int>)buildStaticSerializer.Invoke(null, parameters);
            }

            return ret;
        }

        static readonly Hashtable GetStaticSerializerForCache = new Hashtable();
        static Action<TextWriter, object, int> GetStaticSerializerFor(Type type, Options opts)
        {
            var key = Tuple.Create(type, opts);
            var ret = (Action<TextWriter, object, int>)GetStaticSerializerForCache[key];
            if (ret != null) return ret;

            var getStaticSerializer = GetInlineSerializerFor.MakeGenericMethod(type);
            var invoke = typeof(Action<,,>).MakeGenericType(typeof(TextWriter), type, typeof(int)).GetMethod("Invoke");

            var emit = Emit.NewDynamicMethod(typeof(void), new[] { typeof(TextWriter), typeof(object), typeof(int) }, doVerify: Utils.DoVerify);

            var optsFiled = OptionsLookup.GetOptionsFieldFor(opts);
            emit.LoadField(optsFiled);                              // Options
            emit.Call(getStaticSerializer);                         // Action<TextWriter, Type, int>
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

            lock (GetStaticSerializerForCache)
            {
                ret = (Action<TextWriter, object, int>)GetStaticSerializerForCache[key];
                if (ret != null) return ret;

                GetStaticSerializerForCache[key] = ret = emit.CreateDelegate<Action<TextWriter, object, int>>(optimizationOptions: Utils.DelegateOptimizationOptions);
            }

            return ret;
        }

        static void SerializePrimitive(Type type,TextWriter stream, object val, Options opts, int depth)
        {
            var del = GetStaticSerializerFor(type, opts);
            del(stream, val, depth);
        }

        static void SerializeSemiStatically(TextWriter stream, object val, Options opts, int depth)
        {
            var valType = val.GetType();
            var builder = InlineSerializerHelper.BuildWithDynamism.MakeGenericMethod(valType);
            
            var cacheType = OptionsLookup.GetTypeCacheFor(opts);
            var func = (Delegate)builder.Invoke(null, new object[] { cacheType, opts.ShouldPrettyPrint, opts.ShouldExcludeNulls, opts.IsJSONP, opts.UseDateTimeFormat, opts.ShouldIncludeInherited });
            
            // TODO: recursion and padding check yo!
            func.DynamicInvoke(stream, val, depth);
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

            if(objType.IsPrimitiveType())
            {
                SerializePrimitive(objType, stream, obj, opts, depth);
                return;
            }

            var dynObject = obj as IDynamicMetaObjectProvider;
            if (dynObject != null)
            {
                SerializeDynamicObject(dynObject, stream, opts, depth);
                return;
            }

            SerializeSemiStatically(stream, obj, opts, depth);
        }
    }
}
