using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding.Biomes;

public sealed class BrokenSailboat : MicroBiome
{
    public override bool Place(Point origin, StructureMap structures) {
        return true;
    }
}
