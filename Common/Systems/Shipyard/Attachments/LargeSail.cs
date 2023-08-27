using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class LargeSail : StructureAttachment
{
    public LargeSail(string path) : base(path) { }

    public override Point16 Offset => new Point16(21, 0);
}
