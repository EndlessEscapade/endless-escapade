namespace EndlessEscapade.Content.Tiles.Thermal;

public class BasaltTile : ModTile
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        TileID.Sets.Conversion.Stone[Type] = true;

        AddMapEntry(new Color(37, 45, 45));

        MineResist = 1f;
        HitSound = SoundID.Tink;
        DustType = DustID.Ash;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
       base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }
}
