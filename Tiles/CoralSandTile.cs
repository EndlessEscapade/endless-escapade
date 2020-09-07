using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class CoralSandTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(253, 247, 173));

            dustType = 154;
            drop = ModContent.ItemType<CoralSand>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}