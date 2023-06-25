using EndlessEscapade.Common.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs.Tropical;

public class TropicalWood : CompositeTileBase
{
    public override int HorizontalSheetCount { get; } = 3;

    public override int VerticalSheetCount { get; } = 1;

    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        AddMapEntry(new Color(158, 106, 74));

        HitSound = SoundID.Dig;
        DustType = DustID.WoodFurniture;

        RegisterItemDrop(ModContent.ItemType<Items.Reefs.Tropical.TropicalWood>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}