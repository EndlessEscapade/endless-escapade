using EndlessEscapade.Content.Tiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Kelp;

public class KelpRock : CompositeTile
{
    public override int HorizontalSheetCount { get; } = 3;

    public override int VerticalSheetCount { get; } = 3;

    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileID.Sets.Conversion.Stone[Type] = true;

        AddMapEntry(new Color(99, 136, 132));

        MineResist = 1f;

        HitSound = SoundID.Tink;
        DustType = DustID.JunglePlants;

        RegisterItemDrop(ModContent.ItemType<Items.Kelp.KelpRock>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}
