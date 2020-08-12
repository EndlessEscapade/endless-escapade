using System;
using System.Reflection;

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
        public static bool TryGetCustomAttribute<T>(this MemberInfo info, out T attribute) where T : Attribute => (attribute = info.GetCustomAttribute<T>()) != null;
        public static bool IsStruct(this Type type) => type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        public static bool IsNullable(this Type type) => !(type.IsValueType && Nullable.GetUnderlyingType(type) == null);
        public static bool ImplementsInterface(this Type type, Type interfaceType) => interfaceType.IsAssignableFrom(type);
        public static bool ImplementsInterface<T>(this Type type) where T : class => typeof(T).IsAssignableFrom(type);
    }
}