using EEMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class TropicalWoodTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(102, 26, 0));

            DustType = DustID.Rain;
            ItemDrop = ModContent.ItemType<TropicalWoodItem>();
            //SoundStyle = 1;
            MineResist = 1f;
            MinPick = 0;
        }
    }
}