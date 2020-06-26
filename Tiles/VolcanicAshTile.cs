using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Placeables;

namespace EEMod.Tiles
{
    public class VolcanicAshTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(67, 47, 155));

            dustType = 154;
            drop = ModContent.ItemType<VolcanicAsh>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}