using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Placeables;

namespace EEMod.Tiles
{
    public class MagmastoneSlabTile : ModTile
    {
        public override void ChangeWaterfallStyle(ref int style)
        {
            style = mod.GetWaterfallStyleSlot("Surfacebg");
        }
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(48, 115, 135));

            dustType = 154;
            drop = ModContent.ItemType<MagmastoneSlab>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}
