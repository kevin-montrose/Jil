using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    sealed partial class JsonObject : DynamicObject
    {
        enum JsonObjectType : byte
        {
            Array = 0,
            Object = 1,
            ObjectMember = 2,
            True = 3,
            False = 4,
            String = 5,
            Number = 6,
            FastNumber = 7
        }

        internal static readonly JsonObject True = new JsonObject { Type = JsonObjectType.True };
        internal static readonly JsonObject False = new JsonObject { Type = JsonObjectType.False };

        JsonObjectType Type;
        string StringValue;
        double NumberValue;
        JsonObject Parent;
        List<JsonObject> ArrayValue;
        Dictionary<string, JsonObject> ObjectMembers;

        bool FastNumberNegative;
        ulong FastNumberPart1;
        uint FastNumberPart2;
        byte FastNumberPart2Length;
        long FastNumberPart3;

        JsonObject MemberPart1;
        JsonObject MemberPart2;

        internal static JsonObject ForString(string str)
        {
            return new JsonObject { Type = JsonObjectType.String, StringValue = str };
        }

        internal static JsonObject ForNumber(double num)
        {
            return new JsonObject { Type = JsonObjectType.Number, NumberValue = num };
        }

        internal static JsonObject ForFastNumber(bool neg, ulong a, uint b, byte bLen, long c)
        {
            return new JsonObject { Type = JsonObjectType.FastNumber, FastNumberNegative = neg, FastNumberPart1 = a, FastNumberPart2 = b, FastNumberPart2Length = bLen, FastNumberPart3 = c };
        }

        internal static JsonObject NewArray(JsonObject parent)
        {
            return new JsonObject { Type = JsonObjectType.Array, Parent = parent, ArrayValue = new List<JsonObject>() };
        }

        internal static JsonObject NewObject(JsonObject parent)
        {
            return new JsonObject { Type = JsonObjectType.Object, Parent = parent, ObjectMembers = new Dictionary<string, JsonObject>() };
        }

        internal static JsonObject NewObjectMember(JsonObject parent)
        {
            return new JsonObject { Type = JsonObjectType.ObjectMember, Parent = parent };
        }

        internal JsonObject Pop()
        {
            if (Parent == null) return this;

            return Parent;
        }

        internal void Put(JsonObject other)
        {
            switch (Type)
            {
                case JsonObjectType.Array: ArrayValue.Add(other); return;
                case JsonObjectType.ObjectMember:
                    if (MemberPart1 == null)
                    {
                        MemberPart1 = other;
                    }
                    else
                    {
                        if (MemberPart2 == null)
                        {
                            MemberPart2 = other;
                            this.Parent.ObjectMembers.Add(MemberPart1.StringValue, MemberPart2);
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    return;
            }

            throw new InvalidOperationException();
        }
    }
}
