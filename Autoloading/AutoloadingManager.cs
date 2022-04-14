using EEMod.Autoloading.AutoloadTypes;
using EEMod.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Autoloading
{
    public static class AutoloadingManager
    {
        public const BindingFlags FLAGS_ANY = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        public const BindingFlags FLAGS_STATIC = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        public const BindingFlags FLAGS_INSTANCE = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        internal static event Action PostAutoload;

        ///// <summary> Event called during autoload with each known type </summary>
        //public static event Action<Type> TypeListeners;

        ///// <summary>
        ///// Ecent called during autoload with each known method
        ///// </summary>
        //public static event Action<MethodInfo> MethodListeners;

        internal static void LoadManager(Mod formod) => LoadManager((formod ?? throw new ArgumentNullException(nameof(formod))).Code ?? formod.GetType().Assembly);

        internal static void LoadManager(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            Type[] types = assembly.GetTypesSafe();
            List<MethodInfo> methods = new List<MethodInfo>();
            //List<MethodInfo> TypeListenersByReflection = new List<MethodInfo>();
            foreach (Type type in types)
            {
                // InitializingFields
                foreach (FieldInfo field in type.GetFields(FLAGS_STATIC))
                {
                    if (!field.IsInitOnly && !field.IsLiteral && field.TryGetCustomAttribute(out FieldInitAttribute attribute))
                        DoInit(field, attribute);
                    
                }
                // Initializing methods
                foreach (MethodInfo method in type.GetMethods(FLAGS_STATIC))
                {
                    methods.Add(method);
                    if (method.TryGetCustomAttribute(out FieldInitAttribute attribute))
                    {
                        if (ValidCurrent(attribute.LoadMode) && CouldBeCalled(method) && method.GetParameters().Length <= 0)
                        {
                            method.Invoke(null, null);
                        }
                    }
                }

                // run static ctor
                if (!type.IsGenericType)
                {
                    RuntimeHelpers.RunClassConstructor(type.TypeHandle);
                }
            }

            // Call loading methods
            foreach (var method in methods)
            {
                if (method.TryGetCustomAttribute(out LoadingMethodAttribute attribute) && !(method.GetParameters().Length > 0))
                {
                    if (ValidCurrent(attribute.LoadMode) && CouldBeCalled(method))
                    {
                        method.Invoke(null, null);
                    }
                }
                //else if (method.TryGetCustomAttribute(out TypeListenerAttribute listenerattributet) && CouldBeCalled(method))
                //{
                //    if (ValidCurrent(listenerattributet.loadMode))
                //    {
                //        var parameters = method.GetParameters();
                //        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Type) && method.ReturnType == typeof(void))
                //        {
                //            TypeListeners += method.CreateDelegate<Action<Type>>();
                //        }
                //    }
                //}
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

            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(AutoloadTypeManager)))
                {
                    AutoloadTypeManagerManager.TryAddManager(type);
                }
            }

            AutoloadTypeManagerManager.InitializeManagers();
            foreach (Type type in types)
            {
                AutoloadTypeManagerManager.ManagersCheck(type);
            }

            //if (TypeListeners != null)
            //{
            //    foreach (Type type in types)
            //    {
            //        TypeListeners(type);
            //    }
            //}

            //if (MethodListeners != null)
            //    foreach (var method in methods)
            //        MethodListeners(method);

            PostAutoload?.Invoke();

            //TypeListeners = null;
            //MethodListeners = null;
            PostAutoload = null;
        }

        private static void DoInit(FieldInfo field, FieldInitAttribute attribute)
        {
            if (ValidCurrent(attribute.LoadMode))
            {
                if (field.FieldType.TryCreateInstance(out object instance))
                    field.SetValue(null, instance);
            }
        }

        internal static void UnloadManager(Mod formod) => UnloadManager((formod ?? throw new ArgumentNullException(nameof(formod))).Code ?? formod.GetType().Assembly);

        internal static void UnloadManager(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            //Assembly assembly = a.Code ?? a.GetType().Assembly;
            Type[] types = assembly.GetTypesSafe();

            foreach (var method in types.SelectMany(i => i.GetMethods(FLAGS_STATIC)))
            {
                if (method.GetCustomAttribute<UnloadingMethodAttribute>() is null || !CouldBeCalled(method) || method.GetParameters().Length > 0)
                    continue;

                method.Invoke(null, null);
            }
            /*
            Type iunload = typeof(IOnUnload);
            foreach (Type type in types.Where(t => t.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() == null))
            {
                foreach (FieldInfo field in type.GetFields(FLAGS_STATIC))
                {
                    if (field.IsLiteral // ignore constants
                        || field.IsInitOnly //ignore readonly 
                        || type.IsGenericType && !type.IsConstructedGenericType) // fields in generic types can't be accessed unless they're constructed (someClass<Type> is valid, while someClass<> is)
                    {
                        continue;
                    }

                    Type fieldtype = field.FieldType;
                    if (iunload.IsAssignableFrom(fieldtype)) // if it's IOnUnload call the method
                        ((IOnUnload)field.GetValue(null)).Unloading(field, type);

                    if (fieldtype.IsValueType && Nullable.GetUnderlyingType(fieldtype) == null) // ignore structs and non nullables
                    {
                        continue;
                    }

                    if (field.GetCustomAttribute<UnloadIgnoreAttribute>() == null)
                    {
                        EEMod.Instance.Logger.Debug($"Unloaded field {type.FullName}::{field.Name} - {field.FieldType.FullName}");
                        field.SetValue(null, null);
                    }
                }
            }
            */
        }

        private static bool ValidCurrent(LoadMode mode) => mode == LoadMode.Both || mode == LoadMode.Server == Main.dedServ;

        private static bool CouldBeCalled(MethodInfo method) => !(method.IsAbstract || method.IsGenericMethod != method.IsGenericMethodDefinition || method.GetMethodBody()?.GetILAsByteArray() is null);
    }
}