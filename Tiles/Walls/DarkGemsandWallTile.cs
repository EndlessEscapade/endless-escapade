using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Walls;
using Terraria.ID;

namespace EEMod.Tiles.Walls
{
    public class DarkGemsandWallTile : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(67, 47, 155));

            Main.wallHouse[Type] = true;
            dustType = DustID.Rain;
            drop = ModContent.ItemType<DarkGemsandWall>();
            soundStyle = 1;
        }
    }
}