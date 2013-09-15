using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil
{
    internal static class ExtensionMethods
    {
        public static bool IsDictionaryType(this Type t)
        {
            return false;
        }

        public static bool IsListType(this Type t)
        {
            return false;
        }
    }
}
