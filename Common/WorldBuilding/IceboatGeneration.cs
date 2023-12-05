using System;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class IceboatGeneration : ModSystem
{
    public override void PostWorldGen() {
        var tundraStart = 0;
        var tundraEnd = 0;
        
        var tundraSurface = 0;
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

        for (var i = 0; i < Main.maxTilesY; i++) {
            var tile = Framing.GetTileSafely(tundraCenter, i);
            
            if (tile.HasTile && tile.TileType == TileID.IceBlock) {
                tundraSurface = i;
                break;
            }
        }
    }

    private void PlaceRuins(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/IceboatRuins", Mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var origin = new Point16(x, y) - offset;

        Generator.GenerateStructure("Content/Structures/IceboatRuins", origin, Mod);
    }
    
    private void PlaceIceboat(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/Iceboat", Mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var origin = new Point16(x, y) - offset;

        Generator.GenerateStructure("Content/Structures/Iceboat", origin, Mod);
    }
}
