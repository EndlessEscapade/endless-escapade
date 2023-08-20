using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public abstract class Attachment
{
    public abstract Point16 Offset { get; }

    public abstract bool Valid(int x, int y);
    
    public abstract bool Generate(int x, int y);
}
