using Microsoft.Xna.Framework.Input;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Generation;

// TODO: Separate each structure into its own system to avoid godclassing.
public class StructureSystem : ModSystem
{
    public override void PostWorldGen() {
        // TODO: Turn into a utility method.
        bool foundSurface = false;
        
        int x = 0;
        int y = 0;

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
        
        Point16 shipyardDims = Point16.Zero;
        
        if (!Generator.GetDimensions("Assets/Structures/Shipyard", Mod, ref shipyardDims)) {
            return;
        }

        int shipyardXOffset = shipyardDims.X / 2;
        int shipyardYOffset = shipyardDims.Y - shipyardDims.Y / 3;
        
        PlaceShipyard(x - shipyardXOffset, y - shipyardYOffset);
        
        Point16 sailboatDims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/BrokenSailboat", Mod, ref sailboatDims)) {
            return;
        }

        int sailboatXOffset = shipyardXOffset + sailboatDims.X * 2;
        int sailboatYOffset = sailboatDims.Y - sailboatDims.Y / 3;

        Generator.GenerateStructure("Assets/Structures/BrokenSailboat", new Point16(x - sailboatXOffset, y - sailboatYOffset), Mod);
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
            int pillarX = i + xOffset;
            int pillarY = j + yOffset;

            while (!WorldGen.SolidTile(pillarX, pillarY) && WorldGen.InWorld(pillarX, pillarY)) {
                WorldGen.PlaceTile(pillarX, pillarY, TileID.LivingWood, true, true);
                WorldGen.SlopeTile(pillarX, pillarY, (int)SlopeType.Solid);

                pillarY++;
            }
        }
    }
}