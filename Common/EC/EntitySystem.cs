using System.Collections.Concurrent;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.EC;

public sealed class EntitySystem : ModSystem
{
    public static int NextEntityId { get; private set; }

    public static readonly List<int> AllEntityIds = new();
    public static readonly List<int> ActiveEntityIds = new();
    public static readonly List<int> InactiveEntityIds = new();

    public static readonly ConcurrentBag<int> FreeEntityIds = new();

    public static Entity Create(bool activate) {
        int id;

        if (!FreeEntityIds.TryTake(out id)) {
            id = NextEntityId++;
        }

        AllEntityIds.Add(id);

        if (activate) {
            ActiveEntityIds.Add(id);
        }

        return new Entity(id);
    }

    public static void Remove(int entityId) {
        // TODO: Find a way to also remove components from the entity.
        
        AllEntityIds.Remove(entityId);
        ActiveEntityIds.Remove(entityId);
        InactiveEntityIds.Remove(entityId);

        FreeEntityIds.Add(entityId);
    }

    public static bool GetActive(int entityId) {
        return ActiveEntityIds.Contains(entityId);
    }

    public static void SetActive(int entityId, bool active) {
        if (active) {
            ActiveEntityIds.Add(entityId);
            InactiveEntityIds.Remove(entityId);
            return;
        }

        ActiveEntityIds.Remove(entityId);
        InactiveEntityIds.Add(entityId);
    }
}
