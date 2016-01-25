using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jil
{
    /// <summary>
    /// Custom Attribute Extensions
    /// </summary>
    public static class CustomAttributeExtensions
    {
        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified assembly.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <paramref name="attributeType"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The assembly to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception>
        
        public static Attribute GetCustomAttribute(this Assembly element, Type attributeType)
        {
            return Attribute.GetCustomAttribute(element, attributeType);
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified module.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <paramref name="attributeType"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The module to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception>
        
        public static Attribute GetCustomAttribute(this Module element, Type attributeType)
        {
            return Attribute.GetCustomAttribute(element, attributeType);
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified member.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <paramref name="attributeType"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType)
        {
            return Attribute.GetCustomAttribute(element, attributeType);
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <paramref name="attributeType"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType)
        {
            return Attribute.GetCustomAttribute(element, attributeType);
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified assembly.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <typeparamref name="T"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The assembly to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception>
        
        public static T GetCustomAttribute<T>(this Assembly element) where T : Attribute
        {
            return (T)CustomAttributeExtensions.GetCustomAttribute(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified module.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <typeparamref name="T"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The module to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception>

        public static T GetCustomAttribute<T>(this Module element) where T : Attribute
        {
            return (T)CustomAttributeExtensions.GetCustomAttribute(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified member.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <typeparamref name="T"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The member to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>

        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute
        {
            return (T)CustomAttributeExtensions.GetCustomAttribute(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <typeparamref name="T"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static T GetCustomAttribute<T>(this ParameterInfo element) where T : Attribute
        {
            return (T)CustomAttributeExtensions.GetCustomAttribute(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified member, and optionally inspects the ancestors of that member.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <paramref name="attributeType"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="attributeType">The type of attribute to search for.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType, bool inherit)
        {
            return Attribute.GetCustomAttribute(element, attributeType, inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified parameter, and optionally inspects the ancestors of that parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute matching <paramref name="attributeType"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="attributeType">The type of attribute to search for.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType, bool inherit)
        {
            return Attribute.GetCustomAttribute(element, attributeType, inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified member, and optionally inspects the ancestors of that member.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <typeparamref name="T"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit) where T : Attribute
        {
            return (T)CustomAttributeExtensions.GetCustomAttribute(element, typeof(T), inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified parameter, and optionally inspects the ancestors of that parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A custom attribute that matches <typeparamref name="T"/>, or null if no such attribute is found.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static T GetCustomAttribute<T>(this ParameterInfo element, bool inherit) where T : Attribute
        {
            return (T)CustomAttributeExtensions.GetCustomAttribute(element, typeof(T), inherit);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified assembly.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The assembly to inspect.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this Assembly element)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified module.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The module to inspect.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this Module element)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified member.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The member to inspect.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified member, and optionally inspects the ancestors of that member.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> that match the specified criteria, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, bool inherit)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, inherit);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified parameter, and optionally inspects the ancestors of that parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, bool inherit)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, inherit);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified assembly.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <paramref name="attributeType"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The assembly to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this Assembly element, Type attributeType)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, attributeType);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified module.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <paramref name="attributeType"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The module to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this Module element, Type attributeType)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, attributeType);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified member.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <paramref name="attributeType"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, attributeType);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <paramref name="attributeType"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, Type attributeType)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, attributeType);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified assembly.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <typeparamref name="T"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The assembly to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception>
        
        public static IEnumerable<T> GetCustomAttributes<T>(this Assembly element) where T : Attribute
        {
            return (IEnumerable<T>)CustomAttributeExtensions.GetCustomAttributes(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified module.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <typeparamref name="T"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The module to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception>
        
        public static IEnumerable<T> GetCustomAttributes<T>(this Module element) where T : Attribute
        {
            return (IEnumerable<T>)CustomAttributeExtensions.GetCustomAttributes(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified member.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <typeparamref name="T"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The member to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element) where T : Attribute
        {
            return (IEnumerable<T>)CustomAttributeExtensions.GetCustomAttributes(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <typeparamref name="T"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element) where T : Attribute
        {
            return (IEnumerable<T>)CustomAttributeExtensions.GetCustomAttributes(element, typeof(T));
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified member, and optionally inspects the ancestors of that member.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <paramref name="attributeType"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="attributeType">The type of attribute to search for.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType, bool inherit)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, attributeType, inherit);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified parameter, and optionally inspects the ancestors of that parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <paramref name="attributeType"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="attributeType">The type of attribute to search for.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, Type attributeType, bool inherit)
        {
            return (IEnumerable<Attribute>)Attribute.GetCustomAttributes(element, attributeType, inherit);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified member, and optionally inspects the ancestors of that member.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <typeparamref name="T"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element, bool inherit) where T : Attribute
        {
            return (IEnumerable<T>)CustomAttributeExtensions.GetCustomAttributes(element, typeof(T), inherit);
        }

        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to a specified parameter, and optionally inspects the ancestors of that parameter.
        /// </summary>
        /// 
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="element"/> and that match <typeparamref name="T"/>, or an empty collection if no such attributes exist.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><typeparam name="T">The type of attribute to search for.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception><exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
        
        public static IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element, bool inherit) where T : Attribute
        {
            return (IEnumerable<T>)CustomAttributeExtensions.GetCustomAttributes(element, typeof(T), inherit);
        }

        /// <summary>
        /// Indicates whether custom attributes of a specified type are applied to a specified assembly.
        /// </summary>
        /// 
        /// <returns>
        /// true if an attribute of the specified type is applied to <paramref name="element"/>; otherwise, false.
        /// </returns>
        /// <param name="element">The assembly to inspect.</param><param name="attributeType">The type of the attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception>
        
        public static bool IsDefined(this Assembly element, Type attributeType)
        {
            return Attribute.IsDefined(element, attributeType);
        }

        /// <summary>
        /// Indicates whether custom attributes of a specified type are applied to a specified module.
        /// </summary>
        /// 
        /// <returns>
        /// true if an attribute of the specified type is applied to <paramref name="element"/>; otherwise, false.
        /// </returns>
        /// <param name="element">The module to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception>
        
        public static bool IsDefined(this Module element, Type attributeType)
        {
            return Attribute.IsDefined(element, attributeType);
        }

        /// <summary>
        /// Indicates whether custom attributes of a specified type are applied to a specified member.
        /// </summary>
        /// 
        /// <returns>
        /// true if an attribute of the specified type is applied to <paramref name="element"/>; otherwise, false.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception>
        
        public static bool IsDefined(this MemberInfo element, Type attributeType)
        {
            return Attribute.IsDefined(element, attributeType);
        }

        /// <summary>
        /// Indicates whether custom attributes of a specified type are applied to a specified parameter.
        /// </summary>
        /// 
        /// <returns>
        /// true if an attribute of the specified type is applied to <paramref name="element"/>; otherwise, false.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="attributeType">The type of attribute to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception>
        
        public static bool IsDefined(this ParameterInfo element, Type attributeType)
        {
            return Attribute.IsDefined(element, attributeType);
        }

        /// <summary>
        /// Indicates whether custom attributes of a specified type are applied to a specified member, and, optionally, applied to its ancestors.
        /// </summary>
        /// 
        /// <returns>
        /// true if an attribute of the specified type is applied to <paramref name="element"/>; otherwise, false.
        /// </returns>
        /// <param name="element">The member to inspect.</param><param name="attributeType">The type of the attribute to search for.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception><exception cref="T:System.NotSupportedException"><paramref name="element"/> is not a constructor, method, property, event, type, or field. </exception>
        
        public static bool IsDefined(this MemberInfo element, Type attributeType, bool inherit)
        {
            return Attribute.IsDefined(element, attributeType, inherit);
        }

        /// <summary>
        /// Indicates whether custom attributes of a specified type are applied to a specified parameter, and, optionally, applied to its ancestors.
        /// </summary>
        /// 
        /// <returns>
        /// true if an attribute of the specified type is applied to <paramref name="element"/>; otherwise, false.
        /// </returns>
        /// <param name="element">The parameter to inspect.</param><param name="attributeType">The type of attribute to search for.</param><param name="inherit">true to inspect the ancestors of <paramref name="element"/>; otherwise, false. </param><exception cref="T:System.ArgumentNullException"><paramref name="element"/> or <paramref name="attributeType"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="attributeType"/> is not derived from <see cref="T:System.Attribute"/>. </exception>
        
        public static bool IsDefined(this ParameterInfo element, Type attributeType, bool inherit)
        {
            return Attribute.IsDefined(element, attributeType, inherit);
        }
    }
}