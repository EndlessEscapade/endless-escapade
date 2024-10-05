using EndlessEscapade.Content.Tiles.Base;

namespace EndlessEscapade.Content.Tiles.Kelp;

public class KelpRockTile : CompositeTileBase
{
    public override int HorizontalSheetCount { get; } = 3;

    public override int VerticalSheetCount { get; } = 3;

    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

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
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
       base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }
}
