namespace EndlessEscapade.Content.Tiles.Thermal;

public class RustyPipeTile : ModTile
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        AddMapEntry(new Color(133, 49, 21));

        HitSound = SoundID.Tink;
        DustType = DustID.Copper;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }
}
