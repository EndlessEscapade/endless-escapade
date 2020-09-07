using EEMod.Extensions;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EEMod.Autoloading.AutoloadTypes
{
    /// <summary>
    /// A
    /// </summary>
    internal static class AutoloadTypeManagerManager // A
    {
        [FieldInit]
        private static readonly List<AutoloadTypeManager> managers;

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
                }
            }
            return false;
        }
    }
}