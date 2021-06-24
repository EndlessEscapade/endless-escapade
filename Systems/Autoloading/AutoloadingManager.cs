using EEMod.Autoloading.AutoloadTypes;
using EEMod.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        /// <summary> Event called during autoload with each known type </summary>
        public static event Action<Type> TypeListeners;

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
            foreach (var type in types)
            {
                // InitializingFields
                foreach (var field in type.GetFields(FLAGS_STATIC))
                {
                    if (!field.IsInitOnly && !field.IsLiteral && field.TryGetCustomAttribute(out FieldInitAttribute attribute))
                    {
                        DoInit(field, attribute);
                    }
                }
                // Initializing methods
                foreach (var method in type.GetMethods(FLAGS_STATIC))
                {
                    methods.Add(method);
                    if (method.TryGetCustomAttribute(out FieldInitAttribute attribute))
                    {
                        if (ValidCurrent(attribute.loadMode) && CouldBeCalled(method) && method.GetParameters().Length <= 0)
                        {
                            method.Invoke(null, null);
                        }
                    }
                }
            }

            // Call loading methods
            foreach (var method in methods)
            {
                if (method.TryGetCustomAttribute(out LoadingMethodAttribute attribute) && !(method.GetParameters().Length > 0))
                {
                    if (ValidCurrent(attribute.mode) && CouldBeCalled(method))
                    {
                        method.Invoke(null, null);
                    }
                }
                else if (method.TryGetCustomAttribute(out TypeListenerAttribute listenerattributet) && CouldBeCalled(method))
                {
                    if (ValidCurrent(listenerattributet.loadMode))
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Type) && method.ReturnType == typeof(void))
                        {
                            TypeListeners += method.CreateDelegate<Action<Type>>();
                        }
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
            {
                if (type.IsSubclassOf(typeof(AutoloadTypeManager)))
                {
                    AutoloadTypeManagerManager.TryAddManager(type);
                }
            }

            AutoloadTypeManagerManager.InitializeManagers();
            foreach (var type in types)
            {
                AutoloadTypeManagerManager.ManagersCheck(type);
            }

            if (TypeListeners != null)
            {
                foreach (var type in types)
                {
                    TypeListeners(type);
                }
            }

            //if (MethodListeners != null)
            //    foreach (var method in methods)
            //        MethodListeners(method);

            PostAutoload?.Invoke();

            TypeListeners = null;
            //MethodListeners = null;
            PostAutoload = null;
        }

        private static void DoInit(FieldInfo field, FieldInitAttribute attribute)
        {
            switch (attribute.InitType)
            {
                case FieldInitType.DefaultConstructor:
                {
                    if (field.FieldType.TryCreateInstance(out object instance))
                    {
                        field.SetValue(null, instance);
                    }

                    break;
                }

                case FieldInitType.DefaultConstructorPrivate:
                {
                    if (field.FieldType.TryCreateInstance(true, out object inst))
                    {
                        field.SetValue(null, inst);
                    }

                    break;
                }

                case FieldInitType.CustomValue:
                {
                    Type fieldtype = field.FieldType;
                    object value = attribute.InitInfo1;
                    if ((value is null && fieldtype.IsNullable()) || fieldtype.IsAssignableFrom(value.GetType()))
                    {
                        field.SetValue(null, value);
                    }

                    break;
                }

                case FieldInitType.ArrayIntialization:
                {
                    Type arraytype = field.FieldType;
                    if (arraytype.IsArray && attribute.InitInfo1 is int elements)
                    {
                        field.SetValue(null, Array.CreateInstance(arraytype.GetElementType(), elements));
                    }
                }
                break;

                case FieldInitType.SubType:
                {
                    Type fieldType = field.FieldType;
                    if (attribute.InitInfo1 is Type type)
                    {
                        FieldInitType subInitType = attribute.InitInfo2 is FieldInitType t ? t : FieldInitType.DefaultConstructor;
                        if (subInitType == FieldInitType.DefaultConstructor)
                        {
                            if (fieldType.IsAssignableFrom(type) && type.TryCreateInstance(out object subtypeinstance))
                            {
                                field.SetValue(null, subtypeinstance);
                            }
                        }
                        else if (subInitType == FieldInitType.DefaultConstructorPrivate)
                        {
                            if (fieldType.IsAssignableFrom(type) && type.TryCreateInstance(true, out object subtypeinst))
                            {
                                field.SetValue(null, subtypeinst);
                            }
                        }
                    }
                    break;
                }

                case FieldInitType.ArrayMultipleLengths:
                {
                    Type fieldtype = field.FieldType;
                    if (fieldtype.IsArray && attribute.InitInfo1 is int[] arrayLengths)
                    {
                        field.SetValue(null, Array.CreateInstance(fieldtype.GetElementType(), arrayLengths));
                    }
                    break;
                }
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
                        ModContent.GetInstance<EEMod>().Logger.Debug($"Unloaded field {type.FullName}::{field.Name} - {field.FieldType.FullName}");
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