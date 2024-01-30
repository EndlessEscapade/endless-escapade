using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles;

public class Aquamarine : ModTile
{
    public static readonly SoundStyle AquamarineHitSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Custom/AquamarineHit", 3) {
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
        Main.tileOreFinderPriority[Type] = 600;

        TileID.Sets.Ore[Type] = true;

        AddMapEntry(new Color(152, 171, 198), CreateMapEntryName());

        MineResist = 2f;
        MinPick = 100;
        DustType = DustID.Platinum;
        HitSound = AquamarineHitSound;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
        r = 0.1f;
        b = 0.4f;
    }

    public override bool CanExplode(int i, int j) {
        return NPC.downedMechBossAny;
    }
}
