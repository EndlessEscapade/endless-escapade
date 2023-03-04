using Terraria.DataStructures;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Generation;

public class SailboatSystem : ModSystem
{
    private static Point16 lastBoatDims;
    
    public override void PostWorldGen() {
        
    }

    private static Point16 GetOrigin() {
        int scanX = 0;
        int scanY = 0;

        return new Point16(scanX, scanY);
    }
}