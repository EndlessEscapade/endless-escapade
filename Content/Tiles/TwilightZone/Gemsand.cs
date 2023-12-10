using EndlessEscapade.Content.Tiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.TwilightZone;

public class Gemsand : CompositeTile
{
    public override int HorizontalSheetCount { get; } = 3;

    public override int VerticalSheetCount { get; } = 3;

    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileID.Sets.Conversion.Sand[Type] = true;

        AddMapEntry(new Color(104, 197, 185));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;

        RegisterItemDrop(ModContent.ItemType<Items.TwilightZone.Gemsand>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}
