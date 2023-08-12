using EndlessEscapade.Content.NPCs.Shipyard;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.World;

public class ShipyardSystem : ModSystem
{
    public override void PostWorldGen() {
        var foundOcean = false;
        var foundBeach = false;
        
        var x = 0;
        var y = (int)(Main.worldSurface * 0.35f);
        
        while (!foundOcean) {
            var tile = Framing.GetTileSafely(x, y);
            
            if (tile.LiquidAmount >= 255 && tile.LiquidType == LiquidID.Water) {
                foundOcean = true;
                break;
            }

            y++;
        }

        y--;

        while (!foundBeach) {
            var tile = Framing.GetTileSafely(x, y);

            if (WorldGen.SolidTile(tile) && tile.HasTile && tile.TileType == TileID.Sand) {
                foundBeach = true;
                break;
            }

            x++;
        }   

        const int sailboatDistance = 100;
        
        GenerateShipyard(x, y);
        GenerateSailboat(x - sailboatDistance, y);
    }

    private void GenerateShipyard(int x, int y) {
        var dims = Point16.Zero;
    
        if (!Generator.GetDimensions("Assets/Structures/Shipyard", Mod, ref dims)) {
            return;
        }
        
        var offsetX = dims.X / 2;
        var offsetY = dims.Y - dims.Y / 3;

        PlaceShipyard(x - offsetX, y - offsetY);
    }
    
    private void PlaceShipyard(int x, int y) {
        void ExtendPillar(int xOffset, int yOffset) {
            var pillarX = x + xOffset;
            var pillarY = y + yOffset;

            while (!WorldGen.SolidTile(pillarX, pillarY) && WorldGen.InWorld(pillarX, pillarY)) {
                WorldGen.PlaceTile(pillarX, pillarY, TileID.LivingWood, true, true);
                WorldGen.SlopeTile(pillarX, pillarY, (int)SlopeType.Solid);

                pillarY++;
            }
        }
        
        if (!Generator.GenerateStructure("Assets/Structures/Shipyard", new Point16(x, y), Mod)) {
            return;
        }

        const int firstPillarX = 4;
        const int secondPillarX = 20;
        const int thirdPillarX = 36;

        const int pillarBottomY = 39;

        ExtendPillar(firstPillarX, pillarBottomY);
        ExtendPillar(firstPillarX + 1, pillarBottomY);

        ExtendPillar(secondPillarX, pillarBottomY);
        ExtendPillar(secondPillarX + 1, pillarBottomY);

        ExtendPillar(thirdPillarX, pillarBottomY);
        ExtendPillar(thirdPillarX + 1, pillarBottomY);
        
        const int roomOffsetX = 60;
        const int roomOffsetY = 10;

        var sailorX = (int)((x + roomOffsetX) * 16f);
        var sailorY = (int)((y + roomOffsetY) * 16f);
        
        NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<Sailor>());
    }
    
    private void GenerateSailboat(int x, int y) {
        var dims = Point16.Zero;
        
        if (!Generator.GetDimensions("Assets/Structures/ShipyardBrokenSailboat", Mod, ref dims)) {
            return;
        }
        
        var offsetX = dims.X / 2;
        var offsetY = dims.Y - dims.Y / 3;
        
        Generator.GenerateStructure("Assets/Structures/ShipyardBrokenSailboat", new Point16(x - offsetX, y - offsetY), Mod);
    }
}