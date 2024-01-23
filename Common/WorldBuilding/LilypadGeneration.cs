using System;
using EndlessEscapade.Content.Tiles;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class LilypadGeneration : ModSystem
{
    public override void PostUpdateWorld() {
        if (Main.keyState.IsKeyDown(Keys.F) && !Main.oldKeyState.IsKeyDown(Keys.F)) {
            var position = (Main.MouseWorld / 16f).ToPoint();
            
            WorldGen.PlaceTile(position.X, position.Y, ModContent.TileType<Lilypad>(), true, true);
        }
    }

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

        for (var i = jungleStart + 100; i < jungleStart + jungleWidth - 100; i++) {
            for (var j = (int)(Main.worldSurface * 0.35f); j < Main.maxTilesY - 200; j++) {
                var tile = Framing.GetTileSafely(i, j);
                var tileLeft = Framing.GetTileSafely(i - 1, j);
                var tileRight = Framing.GetTileSafely(i + 1, j);
                var tileAbove = Framing.GetTileSafely(i, j - 1);

                if (!tile.HasTile 
                    && tile.LiquidType == LiquidID.Water 
                    && tile.LiquidAmount >= byte.MaxValue 
                    && !tileAbove.HasTile
                    && tileAbove.LiquidAmount <= 0
                    && !tileLeft.HasTile
                    && !tileRight.HasTile
                    && WorldGen.genRand.NextBool(2)) {
                    WorldGen.PlaceTile(i, j + 1, ModContent.TileType<Lilypad>(), true, true);
                }
            }
        }
    }
}
