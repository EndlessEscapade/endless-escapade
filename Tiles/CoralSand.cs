using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;

namespace EEMod.Tiles
{
    public class CoralSand : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(253,247,173));

            dustType = 154;
            //drop = ModContent.ItemType<Gemsand>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
    }
}
