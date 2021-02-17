using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Foliage.Coral
{
    public class GroundGlowshroomStem1 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 20 };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            if (tile.frameX == 0 && tile.frameY == 0)
            {
                Texture2D tex = mod.GetTexture("Tiles/Foliage/Coral/GroundGlowshroom1");
                Main.spriteBatch.Draw(tex, new Rectangle((i * 16) - (int)Main.screenPosition.X + (int)zero.X, (j * 16) - (int)Main.screenPosition.Y + (int)zero.Y, tex.Bounds.X, tex.Bounds.Y), tex.Bounds, Color.White, 0f, tex.Bounds.Size() / 2f, SpriteEffects.None, 0f);
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {

        }
    }
}