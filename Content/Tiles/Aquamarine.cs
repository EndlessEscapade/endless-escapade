using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Localization;

namespace EndlessEscapade.Content.Tiles;

public class Aquamarine : ModTile
{
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

        DustType = DustID.Platinum;
        HitSound = SoundID.Tink;
        
        MineResist = 1f;
        MinPick = 100;
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
