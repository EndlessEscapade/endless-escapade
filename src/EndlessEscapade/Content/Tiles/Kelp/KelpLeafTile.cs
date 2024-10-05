using EndlessEscapade.Content.Items.Kelp;
using EndlessEscapade.Content.Tiles.Base;

namespace EndlessEscapade.Content.Tiles.Kelp;

public class KelpLeafTile : CompositeTileBase
{
    public override int HorizontalSheetCount { get; } = 3;

    public override int VerticalSheetCount { get; } = 2;

    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        AddMapEntry(new Color(139, 131, 23));

        HitSound = SoundID.Grass;
        DustType = DustID.JungleGrass;

        RegisterItemDrop(ModContent.ItemType<KelpLeafItem>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }
}
