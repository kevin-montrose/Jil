#if COREFX
using System.Collections.Generic;

namespace System.Collections
{
    public class Hashtable
    {
        private Dictionary<object,object> inner  = new Dictionary<object, object>();
        public bool ContainsKey(object key)
        {
            lock(inner) return inner.ContainsKey(key);
        }
        public object this[object key]
        {
            get {
                lock(inner) {
                    object found;
                    return inner.TryGetValue(key, out found) ? found : null;
                }
            }
            set {
                lock(inner) {
                    inner[key] = value;
                }
            } 
        }
    }
}
#endif