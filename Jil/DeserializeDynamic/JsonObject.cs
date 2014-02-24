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
            // These first 3 need to be in this order with these values for better code gen; don't jack with them
            Array = 0,
            Object = 1,
            ObjectMember = 2,
            Null = 3,
            True = 4,
            False = 5,
            String = 6,
            Number = 7
        }

        internal static readonly JsonObject True = new JsonObject { Type = JsonObjectType.True };
        internal static readonly JsonObject False = new JsonObject { Type = JsonObjectType.False };

        JsonObjectType Type;
        string StringValue;
        double NumberValue;
        JsonObject Parent;
        List<JsonObject> ArrayValue;
        Dictionary<string, JsonObject> ObjectMembers;

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
