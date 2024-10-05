namespace EndlessEscapade.Content.Tiles.Shoals;

public class CoralsandTile : ModTile
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        TileID.Sets.Conversion.Sand[Type] = true;

        AddMapEntry(new Color(179, 116, 65));

        HitSound = SoundID.Dig;
        DustType = DustID.Sand;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
       base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }
}
