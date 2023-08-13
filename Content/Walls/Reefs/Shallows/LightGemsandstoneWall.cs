using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Walls.Reefs.Shallows;

public class LightGemsandstoneWall : ModWall
{
    public override void SetStaticDefaults() {
        Main.wallHouse[Type] = true;

        WallID.Sets.Conversion.Sandstone[Type] = true;

        AddMapEntry(new Color(48, 74, 78));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
