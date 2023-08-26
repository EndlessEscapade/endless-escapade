using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class Hull : StructureAttachment
{
    public readonly string Path;
    public readonly ushort[] TileTypes;
    public readonly ushort[] WallTypes;

    public Hull(string path, ushort[] tileTypes, ushort[] wallTypes) : base(path, tileTypes, wallTypes) { }
    
    public override Point16 Offset => new Point16(0, 17);
}
