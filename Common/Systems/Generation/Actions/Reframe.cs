using Microsoft.Xna.Framework;
using Terraria;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.Generation.Actions;

public sealed class Reframe : GenAction
{
    public override bool Apply(Point origin, int x, int y, params object[] args) {
        WorldGen.Reframe(x, y, true);

        return UnitApply(origin, x, y, args);
    }
}
