using System.Linq;
using Terraria;
using Terraria.ID;

namespace EndlessEscapade.Utilities;

public static class WorldGenUtils
{
    public static ushort[] InvalidOriginTiles = {
        TileID.BlueDungeonBrick,
        TileID.GreenDungeonBrick,
        TileID.PinkDungeonBrick,
        TileID.CrackedBlueDungeonBrick,
        TileID.CrackedGreenDungeonBrick,
        TileID.CrackedPinkDungeonBrick,
        TileID.LihzahrdBrick,
        TileID.SandstoneBrick,
        TileID.Hive,
        TileID.Containers,
        TileID.Containers2,
        TileID.Cobweb,
        TileID.Spider,
        TileID.MinecartTrack
    };

    public static ushort[] InvalidOriginWalls = {
        WallID.BlueDungeon,
        WallID.BlueDungeonSlabUnsafe,
        WallID.BlueDungeonTileUnsafe,
        WallID.GreenDungeon,
        WallID.GreenDungeonSlabUnsafe,
        WallID.GreenDungeonTileUnsafe,
        WallID.PinkDungeon,
        WallID.PinkDungeonSlabUnsafe,
        WallID.PinkDungeonTileUnsafe,
        WallID.LihzahrdBrickUnsafe,
        WallID.Hive,
        WallID.HiveUnsafe,
        WallID.Spider,
        WallID.SpiderUnsafe
    };
    
    public static int FindSurfaceLevel(int x) {
        var foundSurface = false;
        var y = 0;
        
        while (y < Main.worldSurface) {
            if (WorldGen.SolidTile(x, y)) {
                foundSurface = true;
                break;
            }
            
            y++;
        }

        return y;
    }

    public static bool ValidTileForPlacement(int x, int y) {
        var tile = Framing.GetTileSafely(x, y);

        if (InvalidOriginTiles.Contains(tile.TileType) || InvalidOriginWalls.Contains(tile.WallType)) {
            return false;
        }
        
        return true;
    }
    
    public static bool ValidAreaForPlacement(int x, int y, int width, int height) {
        var valid = true;
        
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                var tile = Framing.GetTileSafely(x + i, y + j);

                if (!WorldGenUtils.ValidTileForPlacement(x + i, y + j) || tile.LiquidType > 0) {
                    valid = false;
                    break;
                }
            }
        }

        return valid;
    }
    
    public static void ExtendDownwards(int x, int y, int type) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, type, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }
}
