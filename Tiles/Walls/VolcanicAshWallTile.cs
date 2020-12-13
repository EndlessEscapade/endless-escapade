using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Walls;

namespace EEMod.Tiles.Walls
{
    public class VolcanicAshWallTile : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(75, 19, 0));

            Main.wallHouse[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<VolcanicAshWall>();
            soundStyle = 1;
        }
    }
}