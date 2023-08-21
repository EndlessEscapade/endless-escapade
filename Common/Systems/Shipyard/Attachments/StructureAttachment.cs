using System.Collections.Generic;
using EndlessEscapade.Common.Systems.World.Actions;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public abstract class StructureAttachment : IAttachment, TagSerializable
{
    public readonly string Path;
    public readonly ushort[] TileTypes;
    public readonly ushort[] WallTypes;

    public StructureAttachment(string path, ushort[] tileTypes, ushort[] wallTypes) {
        Path = path;
        TileTypes = tileTypes;
        WallTypes = wallTypes;
    }

    public abstract Point16 Offset { get; }

    public virtual bool Valid(int x, int y) {
        var mod = EndlessEscapade.Instance;

        var tileLookup = new Dictionary<ushort, int>();
        var wallLookup = new Dictionary<ushort, int>();

        var totalCount = 0;

        var dims = Point16.Zero;

        if (!Generator.GetDimensions(Path, mod, ref dims)) {
            return false;
        }

        var actions = new GenAction[] { new Actions.TileScanner().Output(tileLookup), new WallScanner().Output(wallLookup) };

        WorldUtils.Gen(new Point(x + Offset.X, y + Offset.Y), new Shapes.Rectangle(dims.X, dims.Y), Actions.Chain(actions));

        for (var i = 0; i < TileTypes.Length; i++) {
            totalCount += tileLookup[TileTypes[i]];
        }

        for (var i = 0; i < WallTypes.Length; i++) {
            totalCount += wallLookup[WallTypes[i]];
        }

        return totalCount >= 10;
    }

    public virtual bool Generate(int x, int y) {
        var mod = EndlessEscapade.Instance;
        var origin = new Point16(x, y) + Offset;

        return Generator.GenerateStructure(Path, origin, mod);
    }
    
    public virtual TagCompound SerializeData() {
        return new TagCompound {
            ["Path"] = Path,
            ["TileTypes"] = TileTypes,
            ["WallTypes"] = WallTypes
        };
    }
}
