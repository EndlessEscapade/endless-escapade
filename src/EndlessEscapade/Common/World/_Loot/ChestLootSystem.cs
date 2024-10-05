using System.Collections.Generic;
using EndlessEscapade.Utilities.Extensions;

namespace EndlessEscapade.Common.World;

public sealed class ChestLootSystem : ModSystem
{
    private static int flags;

    public override void PostSetupContent() {
        base.PostSetupContent();

        foreach (var loot in ModContent.GetContent<IChestLoot>()) {
            SetFlag(loot.GetItemType(), false);
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
            var chests = new List<Chest>();

            var itemType = loot.GetItemType();
            var tileType = loot.GetTileType();

            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validTile = tile.TileType == tileType;
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

                chests.Add(chest);
            }

            while (!HasFlag(itemType)) {
                var chest = WorldGen.genRand.Next(chests);
                var stack = WorldGen.genRand.Next(loot.MinStack, loot.MaxStack);

                if (chest.HasItem(itemType) || chest.TryAddItem(itemType, stack, loot.RandomSlot)) {
                    SetFlag(itemType, true);
                    break;
                }
            }
        }
    }

    private static void GenerateExtraLoot() {
        foreach (var loot in ModContent.GetContent<IChestLoot>()) {
            var itemType = loot.GetItemType();
            var tileType = loot.GetTileType();

            for (var i = 0; i < Main.maxChests; i++) {
                var chest = Main.chest[i];

                if (chest == null) {
                    continue;
                }

                var tile = Framing.GetTileSafely(chest.x, chest.y);

                var validTile = tile.TileType == tileType;
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

                var shouldBeAdded = !chest.HasItem(itemType) && WorldGen.genRand.NextBool(loot.Chance);

                if (!shouldBeAdded) {
                    continue;
                }

                var stack = WorldGen.genRand.Next(loot.MinStack, loot.MaxStack);

                if (!chest.TryAddItem(itemType, stack, loot.RandomSlot) || HasFlag(itemType)) {
                    continue;
                }

                SetFlag(itemType, true);
            }
        }
    }
}
