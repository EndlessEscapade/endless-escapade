using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture.Coral
{
    public class FloorGlow1x2Coral1 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.9f;
            g = 0.9f;
            b = 0.9f;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            int frameX = Main.tile[i, j].frameX;
            int frameY = Main.tile[i, j].frameY;
            const int width = 20;
            const int offsetY = 2;
            const int height = 20;
            const int offsetX = 2;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X + offsetX - (width - 16f) / 2f, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero;
            Rectangle rect = new Rectangle(frameX, frameY, width, height);
            for (int k = 0; k < 7; k++)
            {
                Main.spriteBatch.Draw(EEMod.instance.GetTexture("Tiles/Furniture/Coral/FloorGlow1x2Coral1Glow"), position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}