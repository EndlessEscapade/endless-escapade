using System.Linq;
using Terraria;
using Terraria.ID;

namespace EndlessEscapade.Utilities;

public static class WorldGenUtils
{
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

    public static void ExtendDownwards(int x, int y, int type) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, type, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }
}
