using System;
using System.Reflection;

namespace InteritosMod.Extensions
{
    public static class MemeberInfoExtensions
    {
        public static bool TryGetCustomAttribute<T>(this MemberInfo info, out T attribute) where T : Attribute => (attribute = info.GetCustomAttribute<T>()) != null;
    }
}