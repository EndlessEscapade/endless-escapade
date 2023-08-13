using Microsoft.Xna.Framework;
using Terraria;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.World;

public class SmoothAction : GenAction
{
    private readonly int[,] snapshot;
    private readonly int type;

    public SmoothAction(int[,] snapshot, int type) {
        this.snapshot = snapshot;
        this.type = type;
    }

    public override bool Apply(Point origin, int x, int y, params object[] args) {
        if (!WorldGen.InWorld(x, y) || WorldGen.TileType(x, y) != type) {
            return Fail();
        }
        
        if (snapshot[x - origin.X, y - origin.Y] == -1) {
            WorldGen.KillTile(x, y);
        }
        else {
            Framing.GetTileSafely(x, y).TileType = (ushort)snapshot[x - origin.X, y - origin.Y];
        }

        return UnitApply(origin, x, y, args);
    }
}
