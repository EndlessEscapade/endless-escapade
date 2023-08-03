using System.Threading.Tasks;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Generation;

// TODO: Spawn sailor upon generation.
public class ShipyardSystem : ModSystem
{
    public override void PostWorldGen() {
        // TODO: Optimize code, and allow for future reuse (Other structures).
        var foundSurface = false;

        var x = 0;
        var y = 0;
        
        while (!foundSurface) {
            if (Framing.GetTileSafely(x, y - 1).LiquidAmount > 0) {
                x++;
                y = 0;
                continue;
            }

            if (WorldGen.SolidTile(x, y) && Framing.GetTileSafely(x, y).TileType == TileID.Sand) {
                foundSurface = true;
                break;
            }

            y++;
        }

        var shipyardDims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/Shipyard", Mod, ref shipyardDims)) {
            return;
        }

        var shipyardXOffset = shipyardDims.X / 2;
        var shipyardYOffset = shipyardDims.Y - shipyardDims.Y / 3;

        PlaceShipyard(x - shipyardXOffset, y - shipyardYOffset);
    }

    private void PlaceShipyard(int i, int j) {
        if (!Generator.GenerateStructure("Assets/Structures/Shipyard", new Point16(i, j), Mod)) {
            return;
        }

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
            var pillarX = i + xOffset;
            var pillarY = j + yOffset;

            while (!WorldGen.SolidTile(pillarX, pillarY) && WorldGen.InWorld(pillarX, pillarY)) {
                WorldGen.PlaceTile(pillarX, pillarY, TileID.LivingWood, true, true);
                WorldGen.SlopeTile(pillarX, pillarY, (int)SlopeType.Solid);

                pillarY++;
            }
        }
    }
}