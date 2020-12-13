using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Walls;

namespace EEMod.Tiles.Walls
{
    public class MagmastoneWallTile : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(153, 40, 0));

            Main.wallHouse[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<MagmastoneWall>();
            soundStyle = 1;
        }
    }
}