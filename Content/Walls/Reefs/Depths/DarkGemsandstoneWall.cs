using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Walls.Reefs.Depths;

public class DarkGemsandstoneWall : ModWall
{
    public override void SetStaticDefaults() {
        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(50, 56, 102));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}