using System.Collections.Generic;
using EndlessEscapade.Utilities.Extensions;

namespace EndlessEscapade.Common.World;

public sealed class ChestLootSystem : ModSystem
{
    private static int flags;

    public override void PostSetupContent() {
        base.PostSetupContent();

        foreach (var loot in ModContent.GetContent<IChestLoot>()) {
            SetFlag(loot.ItemType, false);
        }
    }

    public override void PostWorldGen() {
        base.PostWorldGen();

        GenerateGuaranteedLoot();
        GenerateExtraLoot();
    }

    private static void SetFlag(int type, bool value) {
        var mask = 1 << type;

        if (value) {
            flags |= mask;
        }
        else {
            flags &= ~mask;
        }
    }

    private static bool HasFlag(int type) {
        var mask = 1 << type;

        return (flags & mask) != 0;
    }

    private static void GenerateGuaranteedLoot() {
        foreach (var loot in ModContent.GetContent<IChestLoot>()) {
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

            while (!HasFlag(loot.ItemType)) {
                var chest = WorldGen.genRand.Next(filteredChests);
                var stack = WorldGen.genRand.Next(loot.MinStack, loot.MaxStack);

                if (chest.HasItem(loot.ItemType) || chest.TryAddItem(loot.ItemType, stack, loot.RandomSlot)) {
                    SetFlag(loot.ItemType, true);
                    break;
                }
            }
        }
    }

    private static void GenerateExtraLoot() {
        foreach (var loot in ModContent.GetContent<IChestLoot>()) {
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

                if (!chest.TryAddItem(loot.ItemType, stack, loot.RandomSlot) || HasFlag(loot.ItemType)) {
                    continue;
                }

                SetFlag(loot.ItemType, true);
            }
        }
    }
}
