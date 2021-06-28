using System;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace EEMod.Extensions
{
    public static class ReflectionExtensions
    {
        public static Type[] GetTypesSafe(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types;
            }
        }

        /// <summary>
        /// Creates a new delegate with the <paramref name="method"/>
        /// </summary>
        /// <typeparam name="TDelegate">The type of delegate to make</typeparam>
        /// <param name="method">The method</param>
        /// <returns></returns>
        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method) where TDelegate : Delegate => (TDelegate)method.CreateDelegate(typeof(TDelegate));

        /// <summary>
        /// Creates a new delegate with the <paramref name="method"/> with the target <paramref name="target"/>
        /// </summary>
        /// <typeparam name="TDelegate">The type of delegate to make</typeparam>
        /// <param name="method">The method to make the delegate</param>
        /// <param name="target">The target for the delegate<br />Can be null for static methods or methods that take <paramref name="target"/> as first parameter</param>
        /// <returns></returns>
        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method, object target) where TDelegate : Delegate => (TDelegate)method.CreateDelegate(typeof(TDelegate), target);

        public static bool TryGetCustomAttribute<T>(this MemberInfo info, out T attribute) where T : Attribute => (attribute = info.GetCustomAttribute<T>()) != null;

        /// <summary>
        /// If the type is a value type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStruct(this Type type) => type.IsValueType || Nullable.GetUnderlyingType(type) != null;

        /// <summary>
        /// If the type can have null values
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static bool IsNullable(this Type type) => !(type.IsValueType && Nullable.GetUnderlyingType(type) == null);

        public static bool IsCompilerGenerated(this Type type) => type.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() != null;

        public static bool IsSubclassOfGeneric(this Type type, Type generictype)
        {
            while (type != null && type != typeof(object))
            {
                if ((type.IsGenericType ? type.GetGenericTypeDefinition() : type) == generictype)
                {
                    return true;
                }

                type = type.BaseType;
            }
            return false;
        }

        public static bool IsSubclassOfGeneric(this Type type, Type generictype, out Type gType)
        {
            gType = type;
            while (gType != null && gType != typeof(object))
            {
                if ((gType.IsGenericType ? gType.GetGenericTypeDefinition() : gType) == generictype)
                {
                    return true;
                }

                gType = gType.BaseType;
            }
            gType = null;
            return false;
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType) => interfaceType.IsAssignableFrom(type);

        public static bool ImplementsInterface<T>(this Type type) where T : class => typeof(T).IsAssignableFrom(type);

        public static bool TryCreateInstance(this Type type, out object result) => TryCreateInstance(type, false, out result);

        public static bool TryCreateInstance(this Type type, bool privateConstructor, out object result)
        {
            if (CouldBeInstantiated(type) && DefaultConstructor(type, privateConstructor) != null)
            {
                result = System.Activator.CreateInstance(type);
                return true;
            }
            result = null;
            return false;
        }

        public static bool TryCreateInstance<T>(this Type type, out T result) => TryCreateInstance(type, false, out result);

        public static bool TryCreateInstance<T>(this Type type, bool privateConstructor, out T result)
        {
            if (CouldBeInstantiated(type) && DefaultConstructor(type, privateConstructor) != null)
            {
                result = (T)System.Activator.CreateInstance(type);
                return true;
            }
            return (result = default) != null;
        }

        public static ConstructorInfo DefaultConstructor(this Type type, bool nonPublic = false) => nonPublic ? type.GetConstructor(Helpers.FlagsInstance, null, Type.EmptyTypes, null) : type.GetConstructor(Type.EmptyTypes);

        public static bool CouldBeInstantiated(this Type type) => type.IsValueType || !type.IsAbstract && (type.IsGenericType == type.IsConstructedGenericType);
    }
}