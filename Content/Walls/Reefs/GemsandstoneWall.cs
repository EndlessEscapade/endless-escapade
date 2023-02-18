using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Walls.Reefs;

public class GemsandstoneWall : ModWall
{
    public override void SetStaticDefaults() {
        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(31, 74, 93));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}