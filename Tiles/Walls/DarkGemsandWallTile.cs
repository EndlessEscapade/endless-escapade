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
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(67, 47, 155));

            Main.wallHouse[Type] = true;
            DustType = DustID.Rain;
            ItemDrop = ModContent.ItemType<DarkGemsandWall>();
            SoundStyle = 1;
        }
    }
}