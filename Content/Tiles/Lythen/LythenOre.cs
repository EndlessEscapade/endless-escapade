using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Lythen;

public class LythenOre : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileSolid[Type] = true;
        Main.tileShine2[Type] = true; 
        Main.tileSpelunker[Type] = true; 
        Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;    
        
        Main.tileShine[Type] = 975; 
        Main.tileOreFinderPriority[Type] = 400; 
        
        TileID.Sets.Ore[Type] = true;
        
        AddMapEntry(new Color(44, 193, 139), CreateMapEntryName());

        MineResist = 1f;
        MinPick = 30;
    }
    
    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}
