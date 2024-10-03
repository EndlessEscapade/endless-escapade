using System.Collections.Generic;
using EndlessEscapade.Common.IO;
using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Loot;

public sealed class ChestLootManager : ModSystem
{
    private static readonly Dictionary<int, bool> ItemFlagsByType = new();
    
    private static List<ChestLoot> chestLoot = new();

    public override void PostSetupContent() {
        chestLoot = new List<ChestLoot>(PrefabManager.EnumeratePrefabs<ChestLoot>("ChestLoot"));

        foreach (var loot in chestLoot) {
            ItemFlagsByType[loot.ItemType] = false;
        }
    }

    public override void PostWorldGen() {
        // Initial generation, ensures each item generates at least once.
        foreach (var loot in chestLoot) {
            var filteredChests = new List<Chest>();

            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validTile = tile.TileType == loot.TileType;
                var validTileFrame = false;

                foreach (var frame in loot.Frames) {
                    if (tile.TileFrameX == frame * 36) {
                        validTileFrame = true;
                        break;
                    }
                }

                if (!validTile || !validTileFrame) {
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
        foreach (var loot in chestLoot) {
            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validTile = tile.TileType == loot.TileType;
                var validTileFrame = false;

                foreach (var frame in loot.Frames) {
                    if (tile.TileFrameX == frame * 36) {
                        validTileFrame = true;
                        break;
                    }
                }

                var shouldBeAdded = !chest.HasItem(loot.ItemType) && WorldGen.genRand.NextBool(loot.Chance);

                if (!validTile || !validTileFrame || !shouldBeAdded) {
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
