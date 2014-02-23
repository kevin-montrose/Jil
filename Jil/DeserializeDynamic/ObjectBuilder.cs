using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.DeserializeDynamic
{
    sealed class JsonObject
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

        JsonObjectType Type;
        string StringValue;
        double NumberValue;
        JsonObject Parent;
        List<JsonObject> ArrayValue;
        List<JsonObject> ObjectMembers;

        JsonObject MemberPart1;
        JsonObject MemberPart2;

        public static JsonObject ForString(string str)
        {
            return new JsonObject { Type = JsonObjectType.String, StringValue = str };
        }

        public static JsonObject ForNumber(double num)
        {
            return new JsonObject { Type = JsonObjectType.Number, NumberValue = num };
        }

        public static JsonObject NewArray(JsonObject parent)
        {
            return new JsonObject { Type = JsonObjectType.Array, Parent = parent, ArrayValue = new List<JsonObject>() };
        }

        public static JsonObject NewObject(JsonObject parent)
        {
            return new JsonObject { Type = JsonObjectType.Object, Parent = parent, ObjectMembers = new List<JsonObject>() };
        }

        public static JsonObject NewObjectMember(JsonObject parent)
        {
            return new JsonObject { Type = JsonObjectType.ObjectMember, Parent = parent };
        }

        public JsonObject Pop()
        {
            return Parent;
        }

        public void Put(JsonObject other)
        {
            switch(Type){
                case JsonObjectType.Array: ArrayValue.Add(other); break;
                case JsonObjectType.Object: ObjectMembers.Add(other); break;
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
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    break;
            }

            throw new InvalidOperationException();
        }

        public object ValueOf()
        {
            switch (Type)
            {
                case JsonObjectType.Array: return ArrayValue.Select(a => a.ValueOf()).ToArray();
                case JsonObjectType.Object: return ObjectMembers.ToDictionary(m => (string)m.MemberPart1.ValueOf(), m => m.MemberPart2.ValueOf());
                case JsonObjectType.ObjectMember: throw new NotImplementedException();
                case JsonObjectType.Null: return null;
                case JsonObjectType.True: return true;
                case JsonObjectType.False: return false;
                case JsonObjectType.String: return StringValue;
                case JsonObjectType.Number: return NumberValue;
                default: throw new InvalidOperationException();
            }
        }
    }

    sealed class ObjectBuilder
    {
        static readonly JsonObject Null = new JsonObject();
        static readonly JsonObject True = new JsonObject();
        static readonly JsonObject False = new JsonObject();

        public StringBuilder CommonStringBuffer;

        char[] _CommonCharBuffer;
        public char[] CommonCharBuffer
        {
            get
            {
                if (_CommonCharBuffer != null) return _CommonCharBuffer;

                _CommonCharBuffer = new char[Methods.CharBufferSize];
                return _CommonCharBuffer;
            }
        }

        JsonObject BeingBuilt;

        public void PutNull()
        {
            if (BeingBuilt == null)
            {
                BeingBuilt = Null;
            }
            else
            {
                BeingBuilt.Put(Null);
            }
        }

        public void PutTrue()
        {
            if (BeingBuilt == null)
            {
                BeingBuilt = True;
            }
            else
            {
                BeingBuilt.Put(True);
            }
        }

        public void PutFalse()
        {
            if (BeingBuilt == null)
            {
                BeingBuilt = False;
            }
            else
            {
                BeingBuilt.Put(False);
            }
        }

        public void PutString(string str)
        {
            if (BeingBuilt == null)
            {
                BeingBuilt = JsonObject.ForString(str);
            }
            else
            {
                BeingBuilt.Put(JsonObject.ForString(str));
            }
        }

        public void StartArray()
        {
            var arr = JsonObject.NewArray(BeingBuilt);

            if (BeingBuilt == null)
            {
                BeingBuilt = arr;
            }
            else
            {
                BeingBuilt.Put(arr);
                BeingBuilt = arr;
            }
        }

        public void EndArray()
        {
            BeingBuilt = BeingBuilt.Pop();
        }

        public void StartObject()
        {
            var obj = JsonObject.NewObject(BeingBuilt);

            if (BeingBuilt == null)
            {
                BeingBuilt = obj;
            }
            else
            {
                BeingBuilt.Put(obj);
                BeingBuilt = obj;
            }
        }

        public void EndObject()
        {
            BeingBuilt = BeingBuilt.Pop();
        }

        public void StartObjectMember()
        {
            var member = JsonObject.NewObjectMember(BeingBuilt);

            BeingBuilt.Put(member);
            BeingBuilt = member;
        }

        public void EndObjectMember()
        {
            BeingBuilt = BeingBuilt.Pop();
        }

        public void PutNumber(double number)
        {
            var num = JsonObject.ForNumber(number);
            if (BeingBuilt == null)
            {
                BeingBuilt = num;
            }
            else
            {
                BeingBuilt.Put(num);
            }
        }

        public object ValueOf()
        {
            return BeingBuilt.ValueOf();
        }
    }
}
