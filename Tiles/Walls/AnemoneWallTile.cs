using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace EEMod.Tiles.Walls
{
    public class AnemoneWallTile : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(66, 46, 156));

            Main.wallHouse[Type] = false;
            dustType = DustID.Dirt;
            soundStyle = 1;
        }
    }
}