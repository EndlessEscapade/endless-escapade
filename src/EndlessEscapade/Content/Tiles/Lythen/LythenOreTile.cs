using Terraria.Audio;

namespace EndlessEscapade.Content.Tiles.Lythen;

public class LythenOreTile : ModTile
{
    public static readonly SoundStyle LythenHitSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Custom/LythenHit", 3) {
        PitchVariance = 0.25f
    };

    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.tileSpelunker[Type] = true;
        Main.tileShine2[Type] = true;
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        Main.tileShine[Type] = 1000;
        Main.tileOreFinderPriority[Type] = 400;

        TileID.Sets.Ore[Type] = true;

        AddMapEntry(new Color(44, 193, 139), CreateMapEntryName());

        MineResist = 1f;
        MinPick = 30;
        HitSound = LythenHitSound;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
       base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
        base.ModifyLight(i, j, ref r, ref g, ref b);

        g = 0.1f;
        b = 0.4f;
    }
}
