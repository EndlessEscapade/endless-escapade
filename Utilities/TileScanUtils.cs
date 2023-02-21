using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace EndlessEscapade.Utilities;

public static class TileScanUtils
{
    /*
     * TODO: Make this less primitive overall.
     *
     * - Allow for more modularity (Ignore specific liquids, multiple tile types...)
     * - Allow for different scan types (Downwards, Sideways, etc...)
     */
    public static Point16 ScanFromEdge(int type, int startX = 0, int startY = 0, bool ignoreLiquid = false) {
        bool foundScan = false;

        int scanX = startX;
        int scanY = startY;

        while (!foundScan) {
            Tile tile = Framing.GetTileSafely(scanX, scanY);
            Tile tileAbove = Framing.GetTileSafely(scanX, scanY - 1);

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