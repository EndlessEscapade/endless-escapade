using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles.Walls
{
    public class CoralSandWallTile : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(67, 47, 155));

            Main.wallHouse[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<CoralSandWall>();
            soundStyle = 1;
        }
    }
}