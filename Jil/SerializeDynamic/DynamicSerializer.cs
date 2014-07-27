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

                var binder = (GetMemberBinder)Binder.GetMember(0, memberName, dynType, new[] { CSharpArgumentInfo.Create(0, null) });
                var callSite = CallSite<Func<CallSite, object, object>>.Create(binder);

                var val = callSite.Target.Invoke(callSite, dyn);

                stream.Write("\"" + memberName.JsonEscape(jsonp: true) + "\":");
                Serialize(stream, val, opts, depth++);
            }

            stream.Write("}");
        }

        static void SerializePrimitive(Type type, object val, TextWriter stream, Options opts)
        {
            // TODO: some that doesn't suck
            var staticMtd = typeof(JSON).GetMethods().Single(m => m.Name == "Serialize" && m.GetParameters().Length == 3);
            var genericMtd = staticMtd.MakeGenericMethod(type);
            genericMtd.Invoke(null, new[] { val, stream, opts });
        }

        static void SerializeSemiStatically(object val, TextWriter stream, Options opts)
        {
            var valType = val.GetType();
            var mtd = InlineSerializerHelper.BuildWithDynamism.MakeGenericMethod(valType);
            
            // TODO: actually properly grab a cache type, jeez
            var cacheType = typeof(NewtonsoftStyleTypeCache<>);
            var func = (Delegate)mtd.Invoke(null, new object[] { cacheType, opts.ShouldPrettyPrint, opts.ShouldExcludeNulls, opts.IsJSONP, opts.UseDateTimeFormat, opts.ShouldIncludeInherited });
            
            // TODO: recursion and padding check yo!
            func.DynamicInvoke(stream, val, 0);
        }

        public static readonly System.Reflection.MethodInfo SerializeMtd = typeof(DynamicSerializer).GetMethod("Serialize");
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
                SerializePrimitive(objType, obj, stream, opts);
                return;
            }

            var dynObject = obj as IDynamicMetaObjectProvider;
            if (dynObject != null)
            {
                SerializeDynamicObject(dynObject, stream, opts, depth);
                return;
            }

            SerializeSemiStatically(obj, stream, opts);
        }
    }
}
