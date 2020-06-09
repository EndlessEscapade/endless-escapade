using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using EEMod.Extensions;

namespace EEMod.Autoloading
{
    public static class AutoloadingManager
    {
        public const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        public const BindingFlags flags0 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        internal static void LoadManager(EEMod mod)
        {
            Assembly assembly = mod.Code ?? mod.GetType().Assembly;
            Type[] types = assembly.GetTypesSafe();
            IEnumerable<MemberInfo> members = types.SelectMany(i => i.GetMembers(flags));

            Loading(members, mod);
        }
        private static void Loading(IEnumerable<MemberInfo> members, EEMod mod)
        {
            foreach (MemberInfo member in members)
            {
                // nested types also appear from Assembly.GetTypes()
                if (member is MethodInfo m)
                    LoadInvoke(m, mod);
            }
        }
        private static void LoadInvoke(MethodInfo m, EEMod mod)
        {
            if (!ValidMethod(m, out LoadingAttribute attribute)) // if it has the attribute
                return;
            if (!ValidInCurrent(attribute.Loadmode, Main.dedServ))
                return;

            ParameterInfo[] parameters = m.GetParameters();
            Type modtype = mod.GetType();
            if (parameters is null || parameters.Length == 0)
            {
                m.Invoke(null, null);
            }
            else if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(modtype))
            {
                m.Invoke(null, new object[] { mod });
            }
        }
        public static bool ValidMethod<T>(MethodInfo m, out T attribute) where T : Attribute
        {
            attribute = null;
            return m.IsStatic && !(m.IsAbstract || m.IsGenericMethod || m.IsGenericMethodDefinition || m.GetMethodBody()?.GetILAsByteArray() is null) && m.TryGetCustomAttribute(out attribute);
        }
        private static bool ValidInCurrent(LoadingMode mode, bool server) => mode == LoadingMode.Both || mode == LoadingMode.Server == server;



        internal static void UnloadManager(EEMod mod)
        {
            Assembly assembly = mod.Code ?? mod.GetType().Assembly;
            Type[] types = assembly.GetTypesSafe();
            IEnumerable<MemberInfo> members = types.SelectMany(i => i.GetMembers(flags));

            Unloading(members, mod);
        }
        private static void Unloading(IEnumerable<MemberInfo> members, EEMod mod)
        {
            foreach (MemberInfo member in members)
            {
                if (member is MethodInfo m)
                    UnloadInvoke(m, mod);
                else if (member is FieldInfo field)
                    UnloadField(field);
            }
        }
        private static void UnloadInvoke(MethodInfo method, EEMod mod)
        {
            if (!ValidMethod(method, out UnloadingAttribute attribute)) // if it has the attribute
                return;
            if (!ValidInCurrent(attribute.Loadmode, Main.dedServ))
                return;

            ParameterInfo[] parameters = method.GetParameters();
            Type modtype = mod.GetType();
            if (parameters is null || parameters.Length == 0)
            {
                method.Invoke(null, null);
            }
            else if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(modtype))
            {
                method.Invoke(null, new object[] { mod });
            }
        }
        private static void UnloadField(FieldInfo field)
        {
            Type fieldtype = field.FieldType;
            if (!field.TryGetCustomAttribute(out UnloadingAttribute _))
                return;
            if (!field.IsStatic || field.IsInitOnly || field.IsLiteral || fieldtype.IsValueType || Nullable.GetUnderlyingType(fieldtype) != null)
                return;
            field.SetValue(null, null);
        }
    }
}