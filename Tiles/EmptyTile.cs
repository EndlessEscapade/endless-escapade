using EEMod.Items.Placeables;
using EEMod.Tiles.EmptyTileArrays;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class EmptyTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileBlendAll[Type] = true;
            Main.tileSolidTop[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;

            AddMapEntry(new Color(253, 247, 173));
            SoundStyle = 1;
            MineResist = 4f;
            MinPick = 100;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail)
                EmptyTileEntities.Instance.Invoke(new Vector2(i, j));
            else if (EmptyTileEntities.Instance.EmptyTileEntityPairsCache.ContainsKey(EmptyTileEntities.Instance.Convert(new Vector2(i, j))))
                EmptyTileEntities.Instance.Remove(new Vector2(i, j));
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
        }
    }
}