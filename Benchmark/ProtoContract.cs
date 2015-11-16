#if COREFXTODO
using System;

namespace ProtoBuf
{

    public class ProtoContractAttribute : Attribute
    {
    }
    public class ProtoMemberAttribute : Attribute
    {
        public ProtoMemberAttribute(int tag) { }
    }
}
#endif