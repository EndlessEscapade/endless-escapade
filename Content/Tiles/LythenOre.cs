using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles;

public class LythenOre : ModTile
{
    public static readonly SoundStyle LythenHitSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Custom/LythenHit", 3) {
        PitchVariance = 0.25f
    };

    public override void SetStaticDefaults() {
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
        num = fail ? 1 : 3;
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
        g = 0.1f;
        b = 0.4f;
    }
}
