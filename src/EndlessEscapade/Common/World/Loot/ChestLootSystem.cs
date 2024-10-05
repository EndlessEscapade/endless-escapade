using System.Collections.Generic;
using EndlessEscapade.Utilities.Extensions;

namespace EndlessEscapade.Common.World.Loot;

public sealed class ChestLootSystem : ModSystem
{
    private static readonly Dictionary<int, bool> ItemFlagsByType = new();

    private static readonly List<ChestLoot> Loot = new();

    public override void PostSetupContent() {
        base.PostSetupContent();

        foreach (var loot in Loot) {
            ItemFlagsByType[loot.ItemType] = false;
        }
    }

    public override void PostWorldGen() {
        base.PostWorldGen();

        GenerateGuaranteedLoot();
        GenerateExtraLoot();
    }

    private static void GenerateGuaranteedLoot() {
        foreach (var loot in Loot) {
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
    }

    private static void GenerateExtraLoot() {
        foreach (var loot in Loot) {
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
