using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Placeables;

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
