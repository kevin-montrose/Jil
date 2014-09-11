using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    sealed partial class JsonObject
    {
        sealed class ArrayEnumeratorWrapper : DynamicObject, IEnumerator
        {
            List<JsonObject>.Enumerator Wrapped;

            public ArrayEnumeratorWrapper(List<JsonObject>.Enumerator wrapped)
            {
                Wrapped = wrapped;
            }

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                result = null;
                
                switch(binder.Name)
                {
                    case "Dispose":
                        if (args.Length == 0)
                        {
                            Wrapped.Dispose();
                            return true;
                        }
                        return false;
                    case "Equals":
                        if (args.Length == 1)
                        {
                            result = Wrapped.Equals(args[0]);
                            return true;
                        }
                        return false;
                    case "GetHashCode":
                        if (args.Length == 0)
                        {
                            result = Wrapped.GetHashCode();
                            return true;
                        }
                        return false;
                    case "GetType":
                        if (args.Length == 0)
                        {
                            // Do *not* proxy this call, but *do* respond to it
                            result = this.GetType();
                            return true;
                        }
                        return false;
                    case "MoveNext":
                        if (args.Length == 0)
                        {
                            result = Wrapped.MoveNext();
                            return true;
                        }
                        return false;
                    case "ToString":
                        if (args.Length == 0)
                        {
                            result = Wrapped.ToString();
                            return true;
                        }
                        return false;
                    case "Reset":
                        if (args.Length == 0)
                        {
                            ((IEnumerator)Wrapped).Reset();
                            return true;
                        }
                        return false;
                    default:
                        return false;
                }
            }

            static readonly string[] DynamicMemberNames = new[] { "Current" };
            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return DynamicMemberNames;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = null;

                if (binder.Name == "Current")
                {
                    result = Wrapped.Current;
                    return true;
                }

                return false;
            }

            public object Current
            {
                get { return Wrapped.Current; }
            }

            public bool MoveNext()
            {
                return Wrapped.MoveNext();
            }

            public void Reset()
            {
                ((IEnumerator)Wrapped).Reset();
            }
        }

        sealed class ObjectEnumeratorWrapper : DynamicObject, IEnumerator
        {
            sealed class KeyValuePairWrapper : DynamicObject
            {
                KeyValuePair<string, JsonObject> Wrapped;

                public KeyValuePairWrapper(KeyValuePair<string, JsonObject> wrapped)
                {
                    Wrapped = wrapped;
                }

                static readonly string[] DynamicMemberNames = new[] { "Key", "Value" };
                public override IEnumerable<string> GetDynamicMemberNames()
                {
                    return DynamicMemberNames;
                }

                public override bool TryGetMember(GetMemberBinder binder, out object result)
                {
                    result = null;

                    switch (binder.Name)
                    {
                        case "Key":
                            result = Wrapped.Key;
                            return true;
                        case "Value":
                            result = Wrapped.Value;
                            return true;
                        default:
                            return false;
                    }
                }

                public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
                {
                    result = null;

                    switch (binder.Name)
                    {
                        case "Equals":
                            if (args.Length == 1)
                            {
                                result = Wrapped.Equals(args[0]);
                                return true;
                            }
                            return false;
                        case "GetHashCode":
                            if (args.Length == 0)
                            {
                                result = Wrapped.GetHashCode();
                                return true;
                            }
                            return false;
                        case "GetType":
                            if (args.Length == 0)
                            {
                                // Do *not* proxy this call, but *do* respond to it
                                result = this.GetType();
                                return true;
                            }
                            return false;
                        case "ToString":
                            if (args.Length == 0)
                            {
                                result = Wrapped.ToString();
                                return true;
                            }

                            return false;
                        default:
                            return false;
                    }
                }
            }

            Dictionary<string, JsonObject>.Enumerator Wrapped;

            public ObjectEnumeratorWrapper(Dictionary<string, JsonObject>.Enumerator wrapped)
            {
                Wrapped = wrapped;
            }

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                result = null;

                switch (binder.Name)
                {
                    case "Dispose":
                        if (args.Length == 0)
                        {
                            Wrapped.Dispose();
                            return true;
                        }
                        result = null;
                        return false;
                    case "Equals":
                        if (args.Length == 1)
                        {
                            result = Wrapped.Equals(args[0]);
                            return true;
                        }
                        return false;
                    case "GetHashCode":
                        if (args.Length == 0)
                        {
                            result = Wrapped.GetHashCode();
                            return true;
                        }
                        return false;
                    case "GetType":
                        if (args.Length == 0)
                        {
                            // Do *not* proxy this call, but *do* respond to it
                            result = this.GetType();
                            return true;
                        }

                        return false;
                    case "MoveNext":
                        if (args.Length == 0)
                        {
                            result = Wrapped.MoveNext();
                            return true;
                        }
                        return false;
                    case "ToString":
                        if (args.Length == 0)
                        {
                            result = Wrapped.ToString();
                            return true;
                        }
                        return false;
                    case "Reset":
                        if (args.Length == 0)
                        {
                            ((IEnumerator)Wrapped).Reset();
                            return true;
                        }
                        return false;
                    default:
                        return false;
                }
            }

            static readonly string[] DynamicMemberNames = new[] { "Current", "Entry", "Key", "Value" };
            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return DynamicMemberNames;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = null;
                switch (binder.Name)
                {
                    case "Current":
                        result = new KeyValuePairWrapper(Wrapped.Current);
                        return true;
                    case "Entry":
                        result = ((IDictionaryEnumerator)Wrapped).Entry;
                        return true;
                    case "Key":
                        result = ((IDictionaryEnumerator)Wrapped).Key;
                        return true;
                    case "Value":
                        result = ((IDictionaryEnumerator)Wrapped).Value;
                        return true;
                    default:
                        return false;
                }
            }

            public object Current
            {
                get { return new KeyValuePairWrapper(Wrapped.Current); }
            }

            public bool MoveNext()
            {
                return Wrapped.MoveNext();
            }

            public void Reset()
            {
                ((IEnumerator)Wrapped).Reset();
            }
        }

        sealed class EnumerableObjectWrapper : IEnumerable
        {
            Dictionary<string, JsonObject> Wrapped;

            public EnumerableObjectWrapper(Dictionary<string, JsonObject> wrapped)
            {
                Wrapped = wrapped;
            }

            public IEnumerator GetEnumerator()
            {
                return new ObjectEnumeratorWrapper(Wrapped.GetEnumerator());
            }
        }

        sealed class EnumerableArrayWrapper : IEnumerable
        {
            List<JsonObject> Wrapped;

            public EnumerableArrayWrapper(List<JsonObject> wrapped)
            {
                Wrapped = wrapped;
            }

            public IEnumerator GetEnumerator()
            {
                return new ArrayEnumeratorWrapper(Wrapped.GetEnumerator());
            }
        }
    }
}
