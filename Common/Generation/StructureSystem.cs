using Microsoft.Xna.Framework.Input;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Generation;

public sealed class StructureSystem : ModSystem
{
    public override void PostUpdateEverything() {
        if (Main.keyState.IsKeyDown(Keys.F) && !Main.oldKeyState.IsKeyDown(Keys.F)) {
            PlacePier((int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f));
        }
    }

    private void PlacePier(int i, int j) {
        Generator.GenerateStructure("Assets/Structures/Pier", new Point16(i, j), Mod);

        ExtendPillar(5, 24);
        ExtendPillar(6, 24);
        
        ExtendPillar(21, 24);
        ExtendPillar(22, 24);
        
        ExtendPillar(37, 24);
        ExtendPillar(38, 24);
        
        void ExtendPillar(int xOffset, int yOffset) {
            int pillarX = i + xOffset;
            int pillarY = j + yOffset;

            while (!WorldGen.SolidTile(pillarX, pillarY)) {
                WorldGen.PlaceTile(pillarX, pillarY, TileID.LivingWood, true, true);
                WorldGen.SlopeTile(pillarX, pillarY, (int)SlopeType.Solid);

                pillarY++;
            }
        }
    }
}