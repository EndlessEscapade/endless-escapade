using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;
using EEMod.Tiles;

namespace EEMod.Tiles
{
    public class KelpTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileBlendAll[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = false;

            AddMapEntry(new Color(68, 89, 195));

            Main.tileCut[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<Kelp>();
            soundStyle = 1;
            mineResist = 0f;
            minPick = 0;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Framing.GetTileSafely(i, j - 1).type == ModContent.TileType<KelpTile>())
            {
                WorldGen.KillTile(i, j - 1);
            }
        }
    }
}
