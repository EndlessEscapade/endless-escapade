using EEMod.Extensions;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class ShieldplateTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(48, 115, 135));

            DustType = DustID.Rain;
            ItemDrop = ModContent.ItemType<LightGemsand>();
            //SoundStyle = 1;
            MineResist = 1f;
            MinPick = 0;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Lighting.GetColor(i, j);

            int TileFrameX = Framing.GetTileSafely(i, j).TileFrameX;
            int TileFrameY = Framing.GetTileSafely(i, j).TileFrameY;

            if (!Framing.GetTileSafely(i - 1, j).HasTile)
            {
                TileFrameX = 18;
            }
            if (Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<ShieldplateTile>())
            {
                TileFrameX = ((i % 3) * 18) + 36;
            }

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            Texture2D texture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Tiles/ShieldplateTile").Value;
            Rectangle rect = new Rectangle(TileFrameX, TileFrameY, 16, 16);

            Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}
