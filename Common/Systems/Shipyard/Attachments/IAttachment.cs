using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public interface IAttachment
{
    public Point16 Offset { get; }

    public bool Valid(int x, int y);

    public bool Generate(int x, int y);
}
