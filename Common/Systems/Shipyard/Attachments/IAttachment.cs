using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public interface IAttachment
{
    public Point16 Offset { get; }

    public bool Generate(int x, int y);
}
