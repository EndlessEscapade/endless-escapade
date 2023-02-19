using EndlessEscapade.Common.Tiles;
using EndlessEscapade.Content.Items.Reefs.Thermal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs.Thermal;

public class BasaltTile : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        
        TileID.Sets.Conversion.Stone[Type] = true;

        AddMapEntry(new Color(37, 45, 45));

        MineResist = 1f;
        
        HitSound = SoundID.Tink;
        DustType = DustID.Ash;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}