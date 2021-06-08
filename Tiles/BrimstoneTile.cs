using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class BrimstoneTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            Main.tileFrameImportant[Type] = true;

            AddMapEntry(new Color(204, 51, 0));

            dustType = DustID.Rain;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tile = Framing.GetTileSafely(i, j);


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

            if (tileAbove.active() && tileBelow.active() && tileLeft.active() && tileRight.active())
            {
                newFrameX = 18;
                newFrameY = 18;

                if (tileTopRight.active() && tileBottomRight.active())
                {
                    newFrameX = 18;
                    newFrameY = 72;
                }
                if (tileTopLeft.active() && tileBottomLeft.active())
                {
                    newFrameX = 36;
                    newFrameY = 72;
                }

                if (tileTopLeft.active() && tileTopRight.active())
                {
                    newFrameX = 54;
                    newFrameY = 36;
                }
                if (tileBottomLeft.active() && tileBottomRight.active())
                {
                    newFrameX = 54;
                    newFrameY = 18;
                }
            }

            if (tileAbove.active() && tileBelow.active() && !tileLeft.active() && !tileRight.active())
            {
                newFrameX = 18;
                newFrameY = 54;
            }

            if (!tileAbove.active() && !tileBelow.active() && tileLeft.active() && tileRight.active())
            {
                newFrameX = 0;
                newFrameY = 54;
            }

            if (!tileAbove.active() && !tileBelow.active() && !tileLeft.active() && !tileRight.active())
            {
                newFrameX = 36;
                newFrameY = 54;
            }

            if (!tileAbove.active() && tileBelow.active() && tileLeft.active() && tileRight.active())
            {
                newFrameX = 18;
                newFrameY = 0;
            }

            if (tileAbove.active() && tileBelow.active() && !tileLeft.active() && tileRight.active())
            {
                newFrameX = 0;
                newFrameY = 18;
            }

            if (tileAbove.active() && !tileBelow.active() && tileLeft.active() && tileRight.active())
            {
                newFrameX = 18;
                newFrameY = 36;
            }

            if (tileAbove.active() && tileBelow.active() && tileLeft.active() && !tileRight.active())
            {
                newFrameX = 36;
                newFrameY = 18;
            }

            if (!tileAbove.active() && tileBelow.active() && !tileLeft.active() && !tileRight.active())
            {
                newFrameX = 54;
                newFrameY = 0;
            }

            if (tileAbove.active() && !tileBelow.active() && !tileLeft.active() && !tileRight.active())
            {
                newFrameX = 54;
                newFrameY = 54;
            }

            if (!tileAbove.active() && !tileBelow.active() && !tileLeft.active() && tileRight.active())
            {
                newFrameX = 0;
                newFrameY = 72;
            }

            if (!tileAbove.active() && !tileBelow.active() && tileLeft.active() && !tileRight.active())
            {
                newFrameX = 54;
                newFrameY = 72;
            }

            if (!tileAbove.active() && tileBelow.active() && !tileLeft.active() && tileRight.active())
            {
                newFrameX = 0;
                newFrameY = 0;
            }


            if (!tileAbove.active() && tileBelow.active() && tileLeft.active() && !tileRight.active())
            {
                newFrameX = 36;
                newFrameY = 0;
            }

            if (tileAbove.active() && !tileBelow.active() && !tileLeft.active() && tileRight.active())
            {
                newFrameX = 0;
                newFrameY = 36;
            }

            if (tileAbove.active() && !tileBelow.active() && tileLeft.active() && !tileRight.active())
            {
                newFrameX = 36;
                newFrameY = 36;
            }

            Texture2D tex = ModContent.GetTexture("EEMod/Tiles/BrimstoneTile");

            tile.frameX = (short)(newFrameX + frameXOffset);
            tile.frameY = (short)(newFrameY + frameYOffset);

            return true;
        }
    }
}