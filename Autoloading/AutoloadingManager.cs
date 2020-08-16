using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using EEMod.Extensions;
using EEMod.Autoloading.AutoloadTypes;

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
            //LinkedList<MethodInfo> methods = new LinkedList<MethodInfo>();

            // InitializingFields
            foreach (var field in types.SelectMany(type => type.GetFields(FLAGS_STATIC))) //types.SelectMany(i => i.GetFields(FLAGS_STATIC)))
            {
                if (!(field.IsInitOnly || field.IsLiteral) && field.TryGetCustomAttribute(out FieldInitAttribute attribute))
                {
                    Type fieldtype = field.FieldType;

                    if (attribute.hasGivenValue)
                    {
                        object val = attribute.value;
                        if (val is null || fieldtype.IsAssignableFrom(val.GetType()))
                            field.SetValue(null, val);
                        continue;
                    }

                    else if (fieldtype.IsValueType)
                    {
                        Type underlyingNullType = Nullable.GetUnderlyingType(fieldtype);
                        if (underlyingNullType != null) // if it's a nullable struct initialize it with the struct's value
                        {
                            field.SetValue(null, System.Activator.CreateInstance(underlyingNullType));
                        }
                    }

                    else if (fieldtype.TryCreateInstance(out object obj))
                        field.SetValue(null, obj);
                }
            }

            // Call loading methods
            foreach (var method in types.SelectMany(type => type.GetMethods(FLAGS_STATIC)))
            {
                if (method.TryGetCustomAttribute(out LoadingMethodAttribute attribute) && !(method.GetParameters().Length > 0))
                {
                    if (ValidCurrent(attribute.mode) && CouldBeCalled(method))
                        method.Invoke(null, null);
                }
                else if (method.TryGetCustomAttribute(out TypeListenerAttribute listenerattributet) && CouldBeCalled(method))
                {
                    if (ValidCurrent(listenerattributet.loadMode))
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Type))
                            TypeListeners += method.CreateDelegate<Action<Type>>();
                    }
                }
                //else if (method.TryGetCustomAttribute(out MethodListenerAttribute listenerattributem) && CouldBeCalled(method))
                //{
                //    if (ValidCurrent(listenerattributem.loadMode))
                //    {
                //        var parameters = method.GetParameters();
                //        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(MethodInfo))
                //            MethodListeners += method.CreateDelegate<Action<MethodInfo>>();
                //    }
                //}
            }

            foreach (var type in types)
                if (type.IsSubclassOf(typeof(AutoloadTypeManager)))
                    AutoloadTypeManagerManager.TryAddManager(type);

            AutoloadTypeManagerManager.InitializeManagers();
            foreach (var type in types)
                AutoloadTypeManagerManager.ManagersCheck(type);

            if (TypeListeners != null)
                foreach (var type in types)
                    TypeListeners(type);

            //if (MethodListeners != null)
            //    foreach (var method in methods)
            //        MethodListeners(method);

            PostAutoload?.Invoke();

            TypeListeners = null;
            //MethodListeners = null;
            PostAutoload = null;
        }

        internal static event Action PostAutoload;

        /// <summary>
        /// Event called during autoload with each known type
        /// </summary>
        public static event Action<Type> TypeListeners;
        ///// <summary>
        ///// Ecent called during autoload with each known method
        ///// </summary>
        //public static event Action<MethodInfo> MethodListeners;

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

        private static bool ValidCurrent(LoadMode mode) => mode == LoadMode.Both || mode == LoadMode.Server == Main.dedServ;
        private static bool CouldBeCalled(MethodInfo method) => !(method.IsAbstract || method.IsGenericMethod != method.IsGenericMethodDefinition || method.GetMethodBody()?.GetILAsByteArray() is null);
    }
}