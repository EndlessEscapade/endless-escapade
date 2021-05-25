using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class BasaltTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(204, 51, 0));

            dustType = 154;
            drop = ModContent.ItemType<ScorchedGemsand>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}