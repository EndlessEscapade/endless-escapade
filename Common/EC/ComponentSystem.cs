using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.EC;

public sealed class ComponentSystem : ModSystem
{
    private static class ComponentData<T> where T : Component
    {
        public static T?[] Components = Array.Empty<T>();
    }

    public static bool Has<T>(int entityId) where T : Component {
        if (entityId < 0 || entityId >= ComponentData<T>.Components.Length) {
            return false;
        }

        return ComponentData<T>.Components[entityId] != null;
    }

    public static T Get<T>(int entityId) where T : Component {
        if (entityId < 0 || entityId >= ComponentData<T>.Components.Length) {
            return null;
        }

        return ComponentData<T>.Components[entityId];
    }

    public static T Set<T>(int entityId, T component) where T : Component {
        if (entityId >= ComponentData<T>.Components.Length) {
            var newSize = Math.Max(1, ComponentData<T>.Components.Length);

            while (newSize <= entityId) {
                newSize *= 2;
            }
            
            Array.Resize(ref ComponentData<T>.Components, newSize);
        }
        
        ComponentData<T>.Components[entityId] = component;

        return ComponentData<T>.Components[entityId];
    }

    public static void Remove<T>(int entityId) where T : Component {
        if (entityId < 0 || entityId >= ComponentData<T>.Components.Length) {
            return;
        }

        ComponentData<T>.Components[entityId] = null;
    }
}
