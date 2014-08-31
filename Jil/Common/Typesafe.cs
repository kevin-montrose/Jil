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
        private static MethodInfo GetMethod(LambdaExpression example)
        {
            var methodCall = example.Body as MethodCallExpression;
            if (methodCall == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Call but was a {0}", example.Body.NodeType));
            return methodCall.Method;
        }

        public static MethodInfo Method(Expression<Action> example)
        {
            return GetMethod(example);
        }

        public static MethodInfo Method<T>(Expression<Action<T>> example)
        {
            return GetMethod(example);
        }

        public static MethodInfo Method<T1, T2>(Expression<Action<T1, T2>> example)
        {
            return GetMethod(example);
        }

        private static MemberInfo GetMember(LambdaExpression example)
        {
            var member = example.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Member but was a {0}", example.Body.NodeType));
            return member.Member;
        }

        public static MemberInfo Member<R>(Expression<Func<R>> example)
        {
            return GetMember(example);
        }

        public static MemberInfo Member<T, R>(Expression<Func<T, R>> example)
        {
            return GetMember(example);
        }

        public static MemberInfo Member<T1, T2, R>(Expression<Func<T1, T2, R>> example)
        {
            return GetMember(example);
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

        private static ConstructorInfo GetConstructor(LambdaExpression example)
        {
            var constructor = example.Body as NewExpression;
            if (constructor == null)
                throw new ArgumentException(string.Format("Expected the expression to be a Constructor but was a {0}", example.Body.NodeType));
            return constructor.Constructor;
        }

        public static ConstructorInfo Constructor(Expression<Action> example)
        {
            return GetConstructor(example);
        }
    }
}
