using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InteritosMod.Items.Placeables;
using Microsoft.Xna.Framework;

namespace InteritosMod.Tiles
{
    public class HardenedGemsandTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(67, 47, 155));

            dustType = 154;
            drop = ModContent.ItemType<HardenedGemsand>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}