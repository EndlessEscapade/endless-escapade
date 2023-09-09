using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class SmallSail : StructureAttachment
{
    public SmallSail(string path) : base(path) { }

    public override Point16 Offset => new(7, 9);
}
