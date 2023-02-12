using EndlessEscapade.Content.Items.Reefs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs;

public class GemsandTile : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;

        TileID.Sets.Conversion.Sand[Type] = true;
        
        AddMapEntry(new Color(104, 197, 185));
        
        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;
        
        ItemDrop = ModContent.ItemType<GemsandItem>();
    }
    
    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}