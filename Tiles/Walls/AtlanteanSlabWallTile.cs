using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles.Walls
{
    public class AtlanteanSlabWallTile : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(66, 46, 156));

            Main.wallHouse[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<AtlanteanSlabWall>();
            soundStyle = 1;
        }
    }
}