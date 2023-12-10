using System;
using System.Collections.Generic;
using EndlessEscapade.Utilities;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class IceboatGeneration : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
        var index = tasks.FindIndex(pass => pass.Name == "Ice");

        tasks.Insert(index + 1, new PassLegacy($"{nameof(EndlessEscapade)}:Iceboat", GenerateIceboat));
    }

    // TODO: Turn into MicroBiome.
    private void GenerateIceboat(GenerationProgress progress, GameConfiguration configuration) {
        progress.Message = "Sinking the iceboat...";
        
        var tundraStart = 0;
        var tundraEnd = 0;

        var tundraBottom = 0;

        for (var i = Main.maxTilesX; i > 0; i--) {
            for (var j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.IceBlock) {
                    tundraStart = i;
                    break;
                }
            }
        }

        for (var i = 0; i < Main.maxTilesX; i++) {
            for (var j = 0; j < Main.maxTilesY; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile && tile.TileType == TileID.IceBlock) {
                    tundraEnd = i;
                    break;
                }
            }
        }

        var tundraWidth = Math.Abs(tundraStart - tundraEnd);
        var tundraCenter = tundraStart + tundraWidth / 2;

        for (var i = Main.maxTilesY; i > 0; i--) {
            var tile = Framing.GetTileSafely(tundraCenter, i);

            if (tile.HasTile && tile.TileType == TileID.IceBlock) {
                tundraBottom = i;
                break;
            }
        }

        var generatedIceboat = false;

        while (!generatedIceboat) {
            var x = WorldGen.genRand.Next(tundraStart + 100, tundraEnd - 100);
            var y = WorldGenUtils.FindSurfaceLevel(x);
            var offset = WorldGen.genRand.Next(150, 200);

            var tile = Framing.GetTileSafely(x, y);
            
            if (WorldGen.genRand.NextBool(1000) && tile.HasTile && tile.TileType == TileID.SnowBlock && CanPlaceRuins(x, y) && CanPlaceIceboat(x, tundraBottom - offset)) {
                PlaceRuins(x, y);
                PlaceIceboat(x, tundraBottom - offset);
                generatedIceboat = true;
                break;
            }
        }
    }

    private bool CanPlaceRuins(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/IceboatRuins", Mod, ref dims)) {
            return false;
        }

        var offset = new Point16(dims.X / 2, 0);
        var origin = new Point16(x, y) - offset;

        if (!WorldGenUtils.ValidAreaForPlacement(origin.X, origin.Y, dims.X, dims.Y)) {
            return false;
        }

        var leftAdjacentTile = Framing.GetTileSafely(origin.X - 1, origin.Y - 1);
        var rightAdjacentTile = Framing.GetTileSafely(origin.X + dims.X, origin.Y - 1);

        if (leftAdjacentTile.HasTile || rightAdjacentTile.HasTile) {
            return false;
        }

        var solidTileCount = 0;

        for (var i = origin.X; i < origin.X + dims.X; i++) {
            for (var j = origin.Y; j < origin.Y + dims.Y; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile) {
                    solidTileCount++;
                }
            }
        }

        return solidTileCount >= dims.X * dims.Y / 2;
    }

    private bool CanPlaceIceboat(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/IceboatWithIce", Mod, ref dims)) {
            return false;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var origin = new Point16(x, y) - offset;

        if (!WorldGenUtils.ValidAreaForPlacement(origin.X, origin.Y, dims.X, dims.Y)) {
            return false;
        }

        var solidTileCount = 0;

        for (var i = origin.X; i < origin.X + dims.X; i++) {
            for (var j = origin.Y; j < origin.Y + dims.Y; j++) {
                var tile = Framing.GetTileSafely(i, j);

                if (tile.HasTile) {
                    solidTileCount++;
                }
            }
        }

        return solidTileCount >= dims.X * dims.Y / 2;
    }

    private void PlaceRuins(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/IceboatRuins", Mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, 0);
        var origin = new Point16(x, y) - offset;

        if (!WorldGenUtils.ValidAreaForPlacement(origin.X, origin.Y, dims.X, dims.Y)) {
            return;
        }

        Generator.GenerateStructure("Content/Structures/IceboatRuins", origin, Mod);
    }

    private void PlaceIceboat(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/Iceboat", Mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var origin = new Point16(x, y) - offset;

        if (!WorldGenUtils.ValidAreaForPlacement(origin.X, origin.Y, dims.X, dims.Y)) {
            return;
        }
        
        Generator.GenerateStructure("Content/Structures/Iceboat", origin, Mod);
    }
}
