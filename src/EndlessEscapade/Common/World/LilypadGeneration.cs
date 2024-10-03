using System;
using EndlessEscapade.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.World;

public sealed class LilypadGeneration : ModSystem
{
    public override void PostWorldGen() {
        var jungleStart = 0;
        var jungleEnd = 0;

        for (var i = Main.maxTilesX; i > 0; i--) {
            for (var j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.JungleGrass) {
                    jungleStart = i;
                    break;
                }
            }
        }

        for (var i = 0; i < Main.maxTilesX; i++) {
            for (var j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.JungleGrass) {
                    jungleEnd = i;
                    break;
                }
            }
        }

        var jungleWidth = Math.Abs(jungleStart - jungleEnd);

        for (var i = jungleStart; i < jungleStart + jungleWidth; i++) {
            for (var j = 0; j < Main.maxTilesY - 200; j++) {
                var tile = Framing.GetTileSafely(i, j);
                var tileAbove = Framing.GetTileSafely(i, j - 1);

                if (!tile.HasTile &&
                    tile.LiquidType == LiquidID.Water &&
                    tile.LiquidAmount > 0 &&
                    !tileAbove.HasTile &&
                    tileAbove.LiquidAmount <= 0 &&
                    WorldGen.genRand.NextBool(5)) {
                    var types = new[] {
                        ModContent.TileType<Lilypad>(),
                        ModContent.TileType<SmallLilypad>()
                    };

                    WorldGen.PlaceTile(i, j, WorldGen.genRand.Next(types), true, true);
                }
            }
        }
    }
}
