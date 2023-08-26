using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class SmallSail : StructureAttachment
{
    public SmallSail(string path, ushort[] tileTypes, ushort[] wallTypes) : base(path, tileTypes, wallTypes) { }

    public override Point16 Offset => new Point16(7, 9);
}
