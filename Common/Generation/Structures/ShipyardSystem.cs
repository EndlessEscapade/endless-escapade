using EndlessEscapade.Utilities;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Generation.Structures;

public class ShipyardSystem : ModSystem
{
    public static Point16 ShipyardOrigin { get; private set; }

    public override void PostWorldGen() {
        Point16 edge = WorldGenUtils.ScanFromEdge(TileID.Sand);
        Point16 dims = Point16.Zero;
        
        if (!Generator.GetDimensions("Assets/Structures/Shipyard", Mod, ref dims)) {
            return;
        }

        int shipyardXOffset = dims.X / 2;
        int shipyardYOffset = dims.Y - dims.Y / 3;

        int x = edge.X - shipyardXOffset;
        int y = edge.Y - shipyardYOffset;

        ShipyardOrigin = new Point16(x, y);
        
        if (!Generator.GenerateStructure("Assets/Structures/Shipyard", ShipyardOrigin, Mod)) {
            return;
        }

        ExtendPillars(); 
    }
    
    private static void ExtendPillars() {
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
            int pillarX = ShipyardOrigin.X + xOffset;
            int pillarY = ShipyardOrigin.Y + yOffset;

            while (!WorldGen.SolidTile(pillarX, pillarY) && WorldGen.InWorld(pillarX, pillarY)) {
                WorldGen.PlaceTile(pillarX, pillarY, TileID.LivingWood, true, true);
                WorldGen.SlopeTile(pillarX, pillarY, (int)SlopeType.Solid);

                pillarY++;
            }
        }
    }
}