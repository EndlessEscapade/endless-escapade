using Terraria;
using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class Cannon : Attachment
{
    public override Point16 Offset => new Point16(31, 25);
    
    public readonly int Type;

    public Cannon(int type) {
        Type = type;
    }

    public override bool Valid(int x, int y) {
        if (!WorldGen.InWorld(x + Offset.X, y + Offset.Y)) {
            return false;
        }
        
        return WorldGen.TileType(x + Offset.X, y + Offset.Y) == Type;
    }

    public override bool Generate(int x, int y) {
        var mod = EndlessEscapade.Instance;
        var origin = new Point16(x, y) + Offset;

        return WorldGen.PlaceTile(origin.X, origin.Y, Type, true, true);
    }
}
