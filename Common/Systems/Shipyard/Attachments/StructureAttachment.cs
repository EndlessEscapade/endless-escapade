using StructureHelper;
using Terraria.DataStructures;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public abstract class StructureAttachment : IAttachment
{
    public readonly string Path;

    public StructureAttachment(string path) { Path = path; }

    public abstract Point16 Offset { get; }

    public virtual bool Generate(int x, int y) {
        var mod = EndlessEscapade.Instance;
        var origin = new Point16(x, y) + Offset;

        return Generator.GenerateStructure(Path, origin, mod);
    }
}
