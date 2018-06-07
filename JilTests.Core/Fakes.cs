using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodAttribute : Attribute { }

    public static class Assert
    {
        public static void AreEqual<T>(T a, T b, string message = null)
        {
            var eq = false;

            try
            {
                eq = a.Equals(b);
            }
            catch { }

            if (!eq)
            {
                throw new Exception($"Expected {a}, Found {b} {(message != null ? ':' + message : "")}");
            }
        }

        public static void AreNotEqual(object a, object b, string message = null)
        {
            var neq = true;

            try
            {
                neq = !a.Equals(b);
            }
            catch { }

            if (!neq)
            {
                throw new Exception($"Expected {b} to be different from {a} {(message != null ? ':' + message : "")}");
            }
        }

        public static void IsNotNull(object a, string message = null)
        {
            if(a == null)
            {
                throw new Exception($"Expected {a} to be non-null {(message != null ? ':' + message : "")}");
            }
        }

        public static void IsNull(object a, string message = null)
        {
            if (a != null)
            {
                throw new Exception($"Expected {a} to be null {(message != null ? ':' + message : "")}");
            }
        }

        public static void IsTrue(bool b, string message = null)
        {
            if (!b)
            {
                throw new Exception($"Expected true, found false {(message != null ? ':' + message : "")}");
            }
        }

        public static void IsFalse(bool b, string message = null)
        {
            if (b)
            {
                throw new Exception($"Expected false, found true {(message != null ? ':' + message : "")}");
            }
        }

        public static void IsInstanceOfType(object val, Type expected, string message = null)
        {
            var eq = false;

            try
            {
                eq = val.GetType() == expected;
            }
            catch { }

            if (!eq)
            {
                throw new Exception($"Expected {expected}, Found {val} {(message != null ? ':' + message : "")}");
            }
        }

        public static void Fail(string message = null)
        {
            throw new Exception($"Failed {(message != null ? ':' + message : "")}");
        }
    }

    public static class CollectionAssert
    {
        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message = null)
        {
            var expectedList = expected?.ToList();
            var actualList = actual?.ToList();

            if(expectedList == null || actualList == null || expectedList.Count != actualList.Count)
            {
                throw new Exception($"Expected {string.Join(", ", expectedList ?? Enumerable.Empty<T>())}, found {string.Join(", ", actualList ?? Enumerable.Empty<T>())} {(message != null ? ':' + message : "")}");
            }

            for(var i = 0; i < expectedList.Count; i++)
            {
                var e = expectedList[i];
                var a = actualList[i];

                if (!e.Equals(a))
                {
                    throw new Exception($"Expected {string.Join(", ", expectedList ?? Enumerable.Empty<T>())}, found {string.Join(", ", actualList ?? Enumerable.Empty<T>())} {(message != null ? ':' + message : "")}");
                }
            }
        }
    }
}
