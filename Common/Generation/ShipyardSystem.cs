using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Generation;

public class ShipyardSystem : ModSystem
{
    public override void PostWorldGen() {
        Point16 edge = GetOrigin();
        Point16 dims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/Shipyard", Mod, ref dims)) {
            return;
        }

        int shipyardXOffset = dims.X / 2;
        int shipyardYOffset = dims.Y - dims.Y / 3;

        int x = edge.X - shipyardXOffset;
        int y = edge.Y - shipyardYOffset;

        if (!Generator.GenerateStructure("Assets/Structures/Shipyard", new Point16(x, y), Mod)) {
            return;
        }

        ExtendPillars(x, y);
    }

    private static void ExtendPillars(int x, int y) {
        const int firstPillarX = 5;
        const int secondPillarX = 21;
        const int thirdPillarX = 37;

        const int pillarBottomY = 39;

        ExtendPillar(firstPillarX, pillarBottomY);
        ExtendPillar(firstPillarX + 1, pillarBottomY);

        ExtendPillar(secondPillarX, pillarBottomY);
        ExtendPillar(secondPillarX + 1, pillarBottomY);

        ExtendPillar(thirdPillarX, pillarBottomY);
        ExtendPillar(thirdPillarX + 1, pillarBottomY);

        void ExtendPillar(int xOffset, int yOffset) {
            int pillarX = x + xOffset;
            int pillarY = y + yOffset;

            while (WorldGen.InWorld(pillarX, pillarY) && !WorldGen.SolidTile(pillarX, pillarY)) {
                WorldGen.PlaceTile(pillarX, pillarY, TileID.LivingWood, true, true);
                WorldGen.SlopeTile(pillarX, pillarY);

                pillarY++;
            }
        }
    }

    private static Point16 GetOrigin() {
        var foundScan = false;

        var scanX = 0;
        var scanY = 0;

        while (!foundScan) {
            Tile tile = Framing.GetTileSafely(scanX, scanY);
            Tile tileAbove = Framing.GetTileSafely(scanX, scanY - 1);

            if (tileAbove.LiquidAmount > 0) {
                scanX++;
                scanY = 0;
                continue;
            }

            if (tile.HasTile && tile.TileType == TileID.Sand) {
                foundScan = true;
                break;
            }

            scanY++;
        }

        return new Point16(scanX, scanY);
    }
}