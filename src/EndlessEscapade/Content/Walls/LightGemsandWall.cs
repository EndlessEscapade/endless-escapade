using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Walls;

public class LightGemsandWall : ModWall
{
    public override void SetStaticDefaults() {
        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(53, 87, 83));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
