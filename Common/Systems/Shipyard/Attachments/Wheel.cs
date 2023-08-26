using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class Wheel : TileAttachment
{
    public readonly int Type;

    public Wheel(int type) : base(type) { }

    public override Point16 Offset => new Point16(12, 24);
}
