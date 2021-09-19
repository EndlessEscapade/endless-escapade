using EEMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class SaltTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(204, 51, 0));

            DustType = DustID.Rain;
            ItemDrop = ModContent.ItemType<Salt>();
            SoundStyle = 1;
            MineResist = 1f;
            MinPick = 0;
        }
    }
}