using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Common
{
    static class Typesafe
    {
        // GetConstructor

        public static MethodInfo Method(Expression<Action> example)
        {
            var methodCall = example.Body as MethodCallExpression;
            if (methodCall == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Call but was a {0}", example.Body.NodeType));
            return methodCall.Method;
        }

        public static MethodInfo Method<T>(Expression<Action<T>> example)
        {
            var methodCall = example.Body as MethodCallExpression;
            if (methodCall == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Call but was a {0}", example.Body.NodeType));
            return methodCall.Method;
        }

        public static MethodInfo Method<T1, T2>(Expression<Action<T1, T2>> example)
        {
            var methodCall = example.Body as MethodCallExpression;
            if (methodCall == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Call but was a {0}", example.Body.NodeType));
            return methodCall.Method;
        }

        public static MemberInfo Member<R>(Expression<Func<R>> example)
        {
            var methodCall = example.Body as MemberExpression;
            if (methodCall == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Member but was a {0}", example.Body.NodeType));
            return methodCall.Member;
        }

        public static MemberInfo Member<T, R>(Expression<Func<T, R>> example)
        {
            var MemberCall = example.Body as MemberExpression;
            if (MemberCall == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Member but was a {0}", example.Body.NodeType));
            return MemberCall.Member;
        }

        public static MemberInfo Member<T1, T2, R>(Expression<Func<T1, T2, R>> example)
        {
            var MemberCall = example.Body as MemberExpression;
            if (MemberCall == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Member but was a {0}", example.Body.NodeType));
            return MemberCall.Member;
        }

        private static PropertyInfo ToProperty(MemberInfo member)
        {
            var property = member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("The member was not a property");
            return property;
        }

        public static PropertyInfo Property<R>(Expression<Func<R>> example)
        {
            return ToProperty(Member(example));
        }

        public static PropertyInfo Property<T, R>(Expression<Func<T, R>> example)
        {
            return ToProperty(Member(example));
        }

        public static PropertyInfo Property<T1, T2, R>(Expression<Func<T1, T2, R>> example)
        {
            return ToProperty(Member(example));
        }

        public static MethodInfo PropertyGet<R>(Expression<Func<R>> example)
        {
            return Property(example).GetMethod;
        }

        public static MethodInfo PropertyGet<T, R>(Expression<Func<T, R>> example)
        {
            return Property(example).GetMethod;
        }

        public static MethodInfo PropertyGet<T1, T2, R>(Expression<Func<T1, T2, R>> example)
        {
            return Property(example).GetMethod;
        }

        private static FieldInfo ToField(MemberInfo member)
        {
            var field = member as FieldInfo;
            if (field == null)
                throw new ArgumentException("The member was not a field");
            return field;
        }

        public static FieldInfo Field<R>(Expression<Func<R>> example)
        {
            return ToField(Member(example));
        }

        public static FieldInfo Field<T, R>(Expression<Func<T, R>> example)
        {
            return ToField(Member(example));
        }

        public static FieldInfo Field<T1, T2, R>(Expression<Func<T1, T2, R>> example)
        {
            return ToField(Member(example));
        }

    }
}
