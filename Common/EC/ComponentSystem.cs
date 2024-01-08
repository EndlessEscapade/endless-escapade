using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.EC;

public sealed class ComponentSystem : ModSystem
{
    private static class ComponentData<T> where T : Component
    {
        public static readonly int Id;

        public static readonly Dictionary<int, T> Components = new();

        static ComponentData() {
            Id = ComponentTypeCount++;
        }
    }

    public static int ComponentTypeCount;
    
    public static bool Has<T>(int entityId) where T : Component {
        if (entityId < 0 || entityId >= ComponentData<T>.Components.Count) {
            return false;
        }
        
        return ComponentData<T>.Components[entityId] != null;
    }

    public static T Get<T>(int entityId) where T : Component {
        if (entityId < 0 || entityId >= ComponentData<T>.Components.Count) {
            return null;
        }
        
        return ComponentData<T>.Components[entityId];
    }

    public static T Set<T>(int entityId, T component) where T : Component {
        ComponentData<T>.Components[entityId] = component;
        
        return ComponentData<T>.Components[entityId];
    }
    
    public static void Remove<T>(int entityId) where T : Component {
        if (entityId < 0 || entityId >= ComponentData<T>.Components.Count) {
            return;
        }
        
        ComponentData<T>.Components[entityId] = null;
    }
}
