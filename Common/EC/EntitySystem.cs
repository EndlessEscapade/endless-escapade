using System.Collections.Generic;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.EC;

public sealed class EntitySystem : ModSystem
{
    public static readonly Stack<int> Indices = new();

    public static Entity Create() {
        // TODO: Find a way to make this safe.
        var id = Indices.Pop();
        var entity = new Entity(id);

        return entity;
    }

    public static void Remove(int entityId) {
        // TODO: Find a way to remove all components from an entity.
        Indices.Push(entityId);
    }
}
