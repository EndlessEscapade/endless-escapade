using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class DirtySandTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(253, 247, 173));

            DustType = DustID.Rain;
            ItemDrop = ModContent.ItemType<CoralSand>();
            SoundStyle = 1;
            MineResist = 1f;
            MinPick = 0;
        }
    }
}