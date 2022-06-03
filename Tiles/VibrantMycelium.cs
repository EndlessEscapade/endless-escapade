using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace EEMod.Tiles
{
    public class VibrantMycelium : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(48, 115, 135));

            DustType = DustID.Rain;
            //drop = ModContent.ItemType<MagmastoneBrick>();
            //HitSound = 1;
            MineResist = 1f;
            MinPick = 0;
        }
    }
}