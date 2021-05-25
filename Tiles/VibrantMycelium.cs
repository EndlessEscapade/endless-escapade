using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class VibrantMycelium : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(48, 115, 135));

            dustType = 154;
            //drop = ModContent.ItemType<MagmastoneBrick>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}