using EndlessEscapade.Content.Items.Reefs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs;

public class DarkGemsand : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        
        TileID.Sets.Conversion.Sand[Type] = true;

        AddMapEntry(new Color(71, 106, 183));
        
        MineResist = 1f;

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
        
        ItemDrop = ModContent.ItemType<Items.Reefs.DarkGemsand>();
    }
    
    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}