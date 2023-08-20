using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.Shipyard.Attachments;

public class SmallSail : Attachment
{
    public override Point16 Offset => new Point16(7, 9);
    
    public readonly string Path;
    public readonly ushort[] Types;

    public SmallSail(string path, params ushort[] types) {
        Path = path;
        Types = types;
    }

    public override bool Valid(int x, int y) {
        var mod = EndlessEscapade.Instance;

        var lookup = new Dictionary<ushort, int>();
        var count = 0;

        var dims = Point16.Zero;

        if (!Generator.GetDimensions(Path, mod, ref dims)) {
            return false;
        }

        WorldUtils.Gen(new Point(x + Offset.X, y + Offset.Y), new Shapes.Rectangle(dims.X, dims.Y), new Actions.TileScanner(Types).Output(lookup));

        for (int i = 0; i < Types.Length; i++) {
            count += lookup[Types[i]];
        }

        return count >= 10;
    }

    public override bool Generate(int x, int y) {
        var mod = EndlessEscapade.Instance;
        var origin = new Point16(x, y) + Offset;

        return Generator.GenerateStructure(Path, origin, mod);
    }
}
