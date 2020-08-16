using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using EEMod.Extensions;

namespace EEMod.Autoloading.AutoloadTypes
{
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
                manager.Initialize();
        }

        internal static void ManagersCheck(Type type)
        {
            if(type.IsSubclassOf(typeof(AutoloadType)))
            foreach (var manager in managers)
                AutoloadTypeManager.Evaluate(manager, type);
        }

        internal static bool TryAddManager(Type managertype)
        {
            if(managertype.IsSubclassOfGeneric(typeof(AutoloadTypeManager<>)))
            {
                if (managertype.TryCreateInstance(out AutoloadTypeManager manager))
                {
                    managers.Add(manager);
                    ContentInstance.Register(manager);
                }
            }
            return false;
        }

        [LoadingMethod]
        private static void postautoload() => AutoloadingManager.PostAutoload += () => { managers.Clear(); managers = null; };
    }
}
