using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class AtlanteanBrickTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(68, 89, 195));

            dustType = 154;
            drop = ModContent.ItemType<AtlanteanBrick>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}