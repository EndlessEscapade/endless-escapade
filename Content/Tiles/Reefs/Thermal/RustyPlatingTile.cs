using EndlessEscapade.Content.Items.Reefs.Thermal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs.Thermal;

public class RustyPlatingTile : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        AddMapEntry(new Color(89, 94, 86));

        HitSound = SoundID.Tink;
        DustType = DustID.Iron;

        ItemDrop = ModContent.ItemType<RustyPlatingItem>();
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}