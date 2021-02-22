using EEMod.Extensions;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EEMod.Autoloading.AutoloadTypes
{
#pragma warning disable IDE0051 // Private members
    /// <summary>
    /// A
    /// </summary>
    internal static class AutoloadTypeManagerManager // A
    {
        [FieldInit]
        private static List<AutoloadTypeManager> managers;

        internal static void InitializeManagers()
        {
            foreach (var manager in managers)
            {
                manager.Initialize();
            }
        }

        internal static void ManagersCheck(Type type)
        {
            if (typeof(IAutoloadType).IsAssignableFrom(type))
            {
                foreach (var manager in managers)
                {
                    AutoloadTypeManager.Evaluate(manager, type);
                }
            }
        }

        internal static bool TryAddManager(Type managertype)
        {
            if (managertype.IsSubclassOfGeneric(typeof(AutoloadTypeManager<>)))
            {
                if (managertype.TryCreateInstance(out AutoloadTypeManager manager))
                {
                    managers.Add(manager);
                    ContentInstance.Register(manager);
                    return true;
                }
            }
            return false;
        }

        [UnloadingMethod]
        private static void UnloadManagers()
        {
            foreach (var manager in managers)
                manager.Unload();
            managers?.Clear();
            managers = null;
        }
    }
}