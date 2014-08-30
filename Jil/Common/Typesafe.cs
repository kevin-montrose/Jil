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
        // GetField
        // GetProperty

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
    }
}
