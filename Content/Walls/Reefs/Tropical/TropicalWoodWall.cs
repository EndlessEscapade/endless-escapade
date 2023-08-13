using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Walls.Reefs.Tropical;

public class TropicalWoodWall : ModWall
{
    public override void SetStaticDefaults() {
        Main.wallHouse[Type] = true;

        AddMapEntry(new Color(158, 106, 74));

        HitSound = SoundID.Dig;
        DustType = DustID.WoodFurniture;
    }
}
