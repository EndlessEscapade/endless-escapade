using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Walls;

public class DarkGemsandWall : ModWall
{
    public override void SetStaticDefaults() {
        Main.wallHouse[Type] = true;

        AddMapEntry(new Color(64, 70, 116));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
    }
}
