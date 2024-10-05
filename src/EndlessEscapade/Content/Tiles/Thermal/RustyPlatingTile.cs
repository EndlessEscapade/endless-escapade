namespace EndlessEscapade.Content.Tiles.Thermal;

public class RustyPlatingTile : ModTile
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        AddMapEntry(new Color(89, 94, 86));

        HitSound = SoundID.Tink;
        DustType = DustID.Iron;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }
}
