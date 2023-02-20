using Terraria;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Utilities;

public static class WorldGenUtils
{
    public static Point16 ScanFromEdge(int type, int startX = 0, int startY = 0, bool ignoreLiquid = false) {
        bool foundScan = false;
        
        int scanX = startX;
        int scanY = startY;
        
        while (!foundScan) {
            Tile tile = Framing.GetTileSafely(scanX, scanY);
            Tile tileAbove = Framing.GetTileSafely(scanX, scanY - 1);

            if (!WorldGen.InWorld(scanX, scanY) || !WorldGen.InWorld(scanX, scanY - 1)) {
                continue;
            }
            
            if (!ignoreLiquid && tileAbove.LiquidAmount > 0) {
                scanX++;
                scanY = 0;
                continue;
            }
            
            if (tile.HasTile && tile.TileType == type) {
                foundScan = true;
                break;
            }

            scanY++;
        }

        return new Point16(scanX, scanY);
    }
}