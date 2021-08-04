using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace EEMod.Tiles.Foliage.Coral.HangingCoral
{

    public class GlowHangCoral2 : EETile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Height = 11;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coral Lamp");
            AddMapEntry(new Color(0, 100, 200), name);
            dustType = DustID.Dirt;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.frameX < 18)
            {
                r = 0.05f;
                g = 0.05f;
                b = 0.05f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (tile != null && tile.active() && tile.type == Type)
            {
                int frameX = tile.frameX;
                int frameY = tile.frameY;
                const int width = 20;
                const int offsetY = 0;
                const int height = 16;
                const int offsetX = 0;
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                tile.frameX = 17;
                Color color = Color.White;
                Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X + offsetX - (width - 16f) / 2f + 2, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero;
                Rectangle rect = new Rectangle(frameX, frameY, width, height);
                color *= (float)Math.Sin(Main.GameUpdateCount / 60f + i + j) * 0.5f + 0.5f;

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("Tiles/Foliage/Coral/HangingCoral/GlowHangCoral2Glow"), position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}