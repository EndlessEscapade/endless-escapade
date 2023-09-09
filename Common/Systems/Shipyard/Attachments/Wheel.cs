using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class Wheel : TileAttachment
{
    public Wheel(int type) : base(type) { }

    public override Point16 Offset => new(12, 24);
}
