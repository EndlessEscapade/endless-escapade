using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Structures;

public class SailboatSystem : ModSystem
{
    private static Point16 brokenBoatDims = Point16.Zero;
    private static Point16 brokenBoatOrigin = Point16.Zero;
    
    private static Point16 fixedBoatDims = Point16.Zero;
    private static Point16 fixedBoatOrigin = Point16.Zero;
    
    public static bool HasFixedBoat { get; private set; }

    public override void PostWorldGen() {
        Generator.GetDimensions("Assets/Structures/BrokenSailboat", Mod, ref brokenBoatDims);

        brokenBoatOrigin = new Point16(WorldGen.oceanDistance / 2, GetOceanLevel() - (brokenBoatDims.Y - brokenBoatDims.Y / 3));

        Generator.GenerateStructure("Assets/Structures/BrokenSailboat", brokenBoatOrigin, Mod);
        
        Liquid.QuickWater();
        Liquid.UpdateLiquid();
    }

    public static void FixBrokenBoat() {
        Generator.GetDimensions("Assets/Structures/FixedSailboat", ModContent.GetInstance<EndlessEscapade>(), ref fixedBoatDims);

        fixedBoatOrigin = brokenBoatOrigin;
        fixedBoatOrigin += brokenBoatDims;
        fixedBoatOrigin -= fixedBoatDims;
        
        Generator.GenerateStructure("Assets/Structures/FixedSailboat", fixedBoatOrigin, ModContent.GetInstance<EndlessEscapade>());
        
        Liquid.QuickWater();
        Liquid.UpdateLiquid();
        
        HasFixedBoat = true;
    }

    private static int GetOceanLevel() {
        bool foundOceanLevel = false;

        int worldBorderOffset = 50;
        int oceanLevel = 0;

        while (!foundOceanLevel) {
            var tile = Framing.GetTileSafely(worldBorderOffset, oceanLevel);

            if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount > 0) {
                foundOceanLevel = true;
                break;
            }
            
            oceanLevel++;
        }
        
        return oceanLevel;
    }
}