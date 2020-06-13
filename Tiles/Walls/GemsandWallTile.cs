using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;

namespace EEMod.Tiles.Walls
{
    public class GemsandWallTile : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(67, 47, 155));

            Main.wallHouse[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<GemsandWall>();
            soundStyle = 1;
        }
    }
}
