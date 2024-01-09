using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.EC;

public sealed class EntitySystem : ModSystem
{
    private struct EntityData
    {
        public EntityData() { }
    }

    private static EntityData[] Data = Array.Empty<EntityData>();

    private static readonly List<int> ActiveEntityIds = new();
    private static readonly List<int> InactiveEntityIds = new();

    private static readonly ConcurrentBag<int> FreeEntityIds = new();
    
    private static int NextEntityId;


    public static Entity Create(bool activate) {
        int entityId;

        if (!FreeEntityIds.TryTake(out entityId)) {
            entityId = NextEntityId++;
        }

        if (entityId >= Data.Length) {
            var newSize = Math.Max(1, Data.Length);

            while (newSize <= entityId) {
                newSize *= 2;
            }
            
            Array.Resize(ref Data, newSize);
        }
        
        Data[entityId] = new EntityData();

        if (activate) {
            ActiveEntityIds.Add(entityId);
        }
        
        return new Entity(entityId);
    }

    public static void Remove(int entityId) {
        if (entityId < 0 || entityId >= Data.Length) {
            return;
        }
        
        // TODO: Find a way to remove components from the entity.

        ActiveEntityIds.Remove(entityId);
        InactiveEntityIds.Remove(entityId);

        FreeEntityIds.Add(entityId);
    }

    public static bool GetActive(int entityId) {
        if (entityId < 0 || entityId >= Data.Length) {
            return false;
        }
        
        return ActiveEntityIds.Contains(entityId);
    }

    public static void SetActive(int entityId, bool active) {
        if (entityId < 0 || entityId >= Data.Length) {
            return;
        }
        
        if (active) {
            ActiveEntityIds.Add(entityId);
            InactiveEntityIds.Remove(entityId);
            return;
        }

        ActiveEntityIds.Remove(entityId);
        InactiveEntityIds.Add(entityId);
    }
}
