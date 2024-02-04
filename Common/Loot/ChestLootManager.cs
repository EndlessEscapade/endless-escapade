using System.Collections.Generic;
using System.IO;
using System.Linq;
using EndlessEscapade.Common.Ambience;
using EndlessEscapade.Common.IO;
using EndlessEscapade.Utilities.Extensions;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Loot;

public sealed class ChestLootManager : ModSystem
{
    private static List<ChestLoot> ChestLoot = new();
    private static Dictionary<int, bool> ItemFlagsByType = new();
    
    public override void PostSetupContent() {
        ChestLoot = PrefabManager.EnumeratePrefabs<ChestLoot>("ChestLoot").ToList();

        foreach (var loot in ChestLoot) {
            ItemFlagsByType[loot.ItemType] = false;
        }
    }

    public override void PostWorldGen() {
        // Initial generation, ensures each item generates at least once.
        foreach (var loot in ChestLoot) {
            var filteredChests = new List<Chest>();

            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validType = tile.TileType == loot.TileType;
                var validFrame = loot.Frames.Any(x => tile.TileFrameX == (int)x * 36);

                if (!validType || !validFrame) {
                    continue;
                }

                filteredChests.Add(chest);
            }

            while (!ItemFlagsByType[loot.ItemType]) {
                var chest = WorldGen.genRand.Next(filteredChests);
                var stack = WorldGen.genRand.Next(loot.MinStack, loot.MaxStack);

                if (chest.HasItem(loot.ItemType) || chest.TryAddItem(loot.ItemType, stack, loot.RandomSlot)) {
                    ItemFlagsByType[loot.ItemType] = true;
                    break;
                }
            }
        }

        // Extra generation, generates extra items based on their loot spawn rate.
        foreach (var loot in ChestLoot) {
            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validType = tile.TileType == loot.TileType;
                var validFrame = loot.Frames.Any(x => tile.TileFrameX == (int)x * 36);

                var shouldBeAdded = !chest.HasItem(loot.ItemType) && WorldGen.genRand.NextBool(loot.Chance);

                if (!validType || !validFrame || !shouldBeAdded) {
                    continue;
                }

                var stack = WorldGen.genRand.Next(loot.MinStack, loot.MaxStack);

                if (chest.TryAddItem(loot.ItemType, stack, loot.RandomSlot)) {
                    ItemFlagsByType[loot.ItemType] = true;
                    break;
                }
            }
        }
    }
}
