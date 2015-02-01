using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil.Common;

namespace Jil.DeserializeDynamic
{
    //[TypeConverter(typeof(JsonObject.DynamicTypeConverter))]
    sealed partial class JsonObject : ICustomTypeDescriptor
    {
        class DynamicTypeConverter : TypeConverter
        {
            JsonObject Context;

            public DynamicTypeConverter(JsonObject context) : base() 
            {
                Context = context;
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                // we can *never* convert to JsonObject
                return false;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                // we can *never* convert to JsonObject
                return false;
            }

            public override bool IsValid(ITypeDescriptorContext context, object value)
            {
                return value is JsonObject;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                var nonNullable = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

                switch (Context.Type)
                {
                    case JsonObjectType.Array:
                        if (nonNullable == typeof(System.Collections.IEnumerable)) return true;

                        if (!nonNullable.IsGenericEnumerable()) return false;

                        var destinationElemType = nonNullable.GetGenericArguments()[0];

                        // TODO: Less allocations here please
                        return Context.ArrayValue.All(o => { object ignored; return o.InnerTryConvert(destinationElemType, out ignored); });
                    
                    case JsonObjectType.False:
                    case JsonObjectType.True:
                        return nonNullable == typeof(bool);

                    case JsonObjectType.FastNumber:
                    case JsonObjectType.Number:
                        return nonNullable.IsNumberType();

                    case JsonObjectType.Object:
                        // TODO: Implement! (aside, this is a nasty one)
                        throw new NotImplementedException();

                    case JsonObjectType.String:
                        // TODO: Guids and dates and oh my
                        return nonNullable == typeof(string) || nonNullable == typeof(char);

                    default: throw new Exception("Unexpected JsonObjectType: " + Context.Type);
                }
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                object ret;
                if (!Context.InnerTryConvert(destinationType, out ret))
                {
                    throw new NotSupportedException("Could not convert [" + Context + "] to " + destinationType);
                }

                return ret;
            }
        }

        public AttributeCollection GetAttributes()
        {
            return new AttributeCollection();
        }

        public string GetClassName()
        {
            return this.GetType().FullName;
        }

        public string GetComponentName()
        {
            return GetClassName();
        }

        public TypeConverter GetConverter()
        {
            return new DynamicTypeConverter(this);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(new EventDescriptor[0]);
        }

        public EventDescriptorCollection GetEvents()
        {
            return new EventDescriptorCollection(new EventDescriptor[0]);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return null;
        }
    }
}
