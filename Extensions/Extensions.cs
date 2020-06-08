using System;
using System.Reflection;

namespace EEMod.Extensions
{
    public static class MemeberInfoExtensions
    {
        public static bool TryGetCustomAttribute<T>(this MemberInfo info, out T attribute) where T : Attribute => (attribute = info.GetCustomAttribute<T>()) != null;
    }
}