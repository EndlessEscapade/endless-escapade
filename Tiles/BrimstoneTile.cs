using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Tiles
{
    public class BrimstoneTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            Main.tileFrameImportant[Type] = true;

            AddMapEntry(new Color(204, 51, 0));

            DustType = DustID.Rain;
            SoundStyle = 1;
            MineResist = 1f;
            MinPick = 0;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            int tilescale = 18;

            int frameXOffset = (i % 3) * 72;
            int frameYOffset = (j % 3) * 90;

            int newFrameX = 0;
            int newFrameY = 0;

            Tile tileAbove = Main.tile[i, j - 1];
            Tile tileBelow = Main.tile[i, j + 1];

            Tile tileLeft = Main.tile[i - 1, j];
            Tile tileRight = Main.tile[i + 1, j];

            Tile tileTopLeft = Main.tile[i - 1, j - 1];
            Tile tileTopRight = Main.tile[i + 1, j - 1];

            Tile tileBottomLeft = Main.tile[i - 1, j + 1];
            Tile tileBottomRight = Main.tile[i + 1, j + 1];

            if (tileAbove.IsActive && tileBelow.IsActive && tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 1;
                newFrameY = 1;

                if (tileTopRight.IsActive && tileBottomRight.IsActive)
                {
                    newFrameX = 1;
                    newFrameY = 4;
                }
                if (tileTopLeft.IsActive && tileBottomLeft.IsActive)
                {
                    newFrameX = 2;
                    newFrameY = 4;
                }

                if (tileTopLeft.IsActive && tileTopRight.IsActive)
                {
                    newFrameX = 3;
                    newFrameY = 2;
                }
                if (tileBottomLeft.IsActive && tileBottomRight.IsActive)
                {
                    newFrameX = 3;
                    newFrameY = 1;
                }
            }

            if (tileAbove.IsActive && tileBelow.IsActive && !tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 1;
                newFrameY = 3;
            }

            if (!tileAbove.IsActive && !tileBelow.IsActive && tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 0;
                newFrameY = 3;
            }

            if (!tileAbove.IsActive && !tileBelow.IsActive && !tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 2;
                newFrameY = 3;
            }

            if (!tileAbove.IsActive && tileBelow.IsActive && tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 1;
                newFrameY = 0;
            }

            if (tileAbove.IsActive && tileBelow.IsActive && !tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 0;
                newFrameY = 1;
            }

            if (tileAbove.IsActive && !tileBelow.IsActive && tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 1;
                newFrameY = 2;
            }

            if (tileAbove.IsActive && tileBelow.IsActive && tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 2;
                newFrameY = 1;
            }

            if (!tileAbove.IsActive && tileBelow.IsActive && !tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 3;
                newFrameY = 0;
            }

            if (tileAbove.IsActive && !tileBelow.IsActive && !tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 3;
                newFrameY = 3;
            }

            if (!tileAbove.IsActive && !tileBelow.IsActive && !tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 0;
                newFrameY = 4;
            }

            if (!tileAbove.IsActive && !tileBelow.IsActive && tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 3;
                newFrameY = 4;
            }

            if (!tileAbove.IsActive && tileBelow.IsActive && !tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 0;
                newFrameY = 0;
            }


            if (!tileAbove.IsActive && tileBelow.IsActive && tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 2;
                newFrameY = 0;
            }

            if (tileAbove.IsActive && !tileBelow.IsActive && !tileLeft.IsActive && tileRight.IsActive)
            {
                newFrameX = 0;
                newFrameY = 2;
            }

            if (tileAbove.IsActive && !tileBelow.IsActive && tileLeft.IsActive && !tileRight.IsActive)
            {
                newFrameX = 2;
                newFrameY = 2;
            }

            Texture2D tex = ModContent.Request<Texture2D>("EEMod/Tiles/BrimstoneTile").Value;

            tile.frameX = (short)((newFrameX * tilescale) + frameXOffset);
            tile.frameY = (short)((newFrameY * tilescale) + frameYOffset);

            return true;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/Tiles/BrimstoneTileGlow").Value;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(Main.tile[i, j].frameX, Main.tile[i, j].frameY, 16, 16), Lighting.GetColor(i, j) * (1.5f + ((float)Math.Sin((i * j) + Main.GameUpdateCount / 20f) * 0.3f)), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}