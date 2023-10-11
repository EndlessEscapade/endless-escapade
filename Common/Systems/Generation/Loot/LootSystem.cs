using System.Collections.Generic;
using System.Linq;
using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Generation.Loot;

public sealed class LootSystem : ModSystem
{
    // TODO: Possible refactors maybe? Like turning the dictionary below into a BitArray.

    private static readonly Dictionary<int, bool> itemFlagsByType = new();
    private static readonly Dictionary<int, IChestLoot> itemDataByType = new();

    public override void OnModLoad() {
        foreach (var item in Mod.GetContent<ModItem>()) {
            var type = item.GetType();

            if (item is not IChestLoot loot) {
                continue;
            }

            itemDataByType[item.Type] = loot;
            itemFlagsByType[item.Type] = false;
        }
    }

    public override void PostWorldGen() {
        foreach (var (type, data) in itemDataByType) {
            var filteredChests = new List<Chest>();

            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validType = tile.TileType == data.TileType;
                var validFrame = data.Frames.Any(x => tile.TileFrameX == (int)x * 36);

                if (!validType || !validFrame) {
                    continue;
                }

                filteredChests.Add(chest);
            }

            while (!itemFlagsByType[type]) {
                var chest = WorldGen.genRand.Next(filteredChests);
                var stack = WorldGen.genRand.Next(data.MinStack, data.MaxStack);

                if (chest.HasItem(type) || chest.TryAddItem(type, stack, data.RandomSlot)) {
                    itemFlagsByType[type] = true;
                    break;
                }
            }
        }

        foreach (var (type, data) in itemDataByType) {
            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validType = tile.TileType == data.TileType;
                var validFrame = data.Frames.Any(x => tile.TileFrameX == (int)x * 36);

                var shouldBeAdded = !chest.HasItem(type) && WorldGen.genRand.NextBool(data.Chance);

                if (!validType || !validFrame || !shouldBeAdded) {
                    continue;
                }

                var stack = WorldGen.genRand.Next(data.MinStack, data.MaxStack);

                if (chest.TryAddItem(type, stack, data.RandomSlot)) {
                    itemFlagsByType[type] = true;
                    break;
                }
            }
        }
    }
}
