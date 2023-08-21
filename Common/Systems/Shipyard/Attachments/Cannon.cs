using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class Cannon : TileAttachment
{
    public readonly int Type;

    public Cannon(int type) : base(type) { }
    
    public override Point16 Offset => new Point16(31, 25);
}
