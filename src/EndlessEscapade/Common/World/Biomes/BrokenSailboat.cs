using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.World.Biomes;

public sealed class BrokenSailboat : MicroBiome
{
    public override bool Place(Point origin, StructureMap structures) {
        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/BrokenSailboat", mod, ref dims)) {
            return false;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var adjustedOrigin = new Point16(origin.X, origin.Y) - offset;
        
        if (!structures.CanPlace(new Rectangle(adjustedOrigin.X, adjustedOrigin.Y, dims.X, dims.Y)) ||
            !Generator.GenerateStructure("Content/Structures/BrokenSailboat", adjustedOrigin, mod)) {
            return false;
        }
        
        return Generator.GenerateStructure("Content/Structures/BrokenSailboat", adjustedOrigin, mod);
    }
}
