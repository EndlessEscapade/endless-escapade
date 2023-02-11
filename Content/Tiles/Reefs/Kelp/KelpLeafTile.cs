using EndlessEscapade.Content.Items.Reefs.Kelp;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs.Kelp;

public class KelpLeafTile : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        
        AddMapEntry(new Color(139, 131, 23));

        MineResist = 1f;
        
        HitSound = SoundID.Grass;
        DustType = DustID.JungleGrass;

        ItemDrop = ModContent.ItemType<KelpLeafItem>();
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}