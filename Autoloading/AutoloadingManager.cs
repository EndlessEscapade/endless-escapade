using System;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using EEMod.Extensions;

namespace EEMod.Autoloading
{
    public static class AutoloadingManager
    {
        public const BindingFlags FLAGS_ANY = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        public const BindingFlags FLAGS_STATIC = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        public const BindingFlags FLAGS_INSTANCE = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        internal static void LoadManager(EEMod a)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            Assembly assembly = a.Code ?? a.GetType().Assembly;
            Type[] types = assembly.GetTypesSafe();
            foreach (var field in types.SelectMany(i => i.GetFields(FLAGS_STATIC)))
            {
                if (field.IsInitOnly || field.IsLiteral)
                    continue;

                FieldInitAttribute attribute = field.GetCustomAttribute<FieldInitAttribute>();

                if (attribute is null)
                    continue;

                Type fieldtype = field.FieldType;

                if (attribute.hasGivenValue)
                {
                    object val = attribute.value;
                    if (val is null || fieldtype.IsAssignableFrom(val.GetType()))
                        field.SetValue(null, val);
                    return;
                }

                Type underlyingNullType = Nullable.GetUnderlyingType(fieldtype);
                if (underlyingNullType != null) // if it's a nullable struct initialize it with the struct's value
                {
                    field.SetValue(null, System.Activator.CreateInstance(underlyingNullType));
                }

                ConstructorInfo constructor = attribute.lookForPrivateConstructor
                    ? fieldtype.GetConstructor(FLAGS_INSTANCE, null, Type.EmptyTypes, null)
                    : fieldtype.GetConstructor(Type.EmptyTypes);

                if (constructor != null)
                    field.SetValue(null, constructor.Invoke(null));
            }

            foreach (var method in types.SelectMany(i => i.GetMethods(FLAGS_STATIC)))
            {
                LoadingMethodAttribute attribute = method.GetCustomAttribute<LoadingMethodAttribute>();
                if (attribute is null || method.IsAbstract || method.IsGenericMethod || method.IsGenericMethodDefinition || method.GetMethodBody()?.GetILAsByteArray() is null || method.GetParameters().Length > 0)
                    continue;

                if (attribute.mode == LoadMode.Both || attribute.mode == LoadMode.Server == Main.dedServ)
                    method.Invoke(null, null);
            }
        }

        internal static void UnloadManager(EEMod a)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));
            Assembly assembly = a.Code ?? a.GetType().Assembly;
            Type[] types = assembly.GetTypesSafe();

            foreach (var field in types.SelectMany(i => i.GetFields(FLAGS_STATIC)))
            {
                if (field.IsInitOnly || field.IsLiteral)
                    continue;

                Type fieldtype = field.FieldType;
                if (fieldtype.IsValueType && Nullable.GetUnderlyingType(fieldtype) == null) // ignore structs and non nullables
                    continue;

                if (field.GetCustomAttribute<UnloadIgnoreAttribute>() == null)
                    field.SetValue(null, null);
            }

            foreach (var method in types.SelectMany(i => i.GetMethods(FLAGS_STATIC)))
            {
                if (method.GetCustomAttribute<UnloadingMethodAttribute>() is null || method.IsAbstract || method.IsGenericMethod || method.IsGenericMethodDefinition || method.GetMethodBody()?.GetILAsByteArray() is null || method.GetParameters().Length > 0)
                    continue;

                method.Invoke(null, null);
            }
        }
    }
}