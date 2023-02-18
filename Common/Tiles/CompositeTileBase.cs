using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Tiles;

public abstract class CompositeTileBase : ModTile
{
    private const int ChunkWidth = 72;
    private const int ChunkHeight = 90;

    private const int TileSize = 16;

    public abstract int AtlasWidth { get; }
    public abstract int AtlasHeight { get; }

    public virtual int FramePadding { get; } = 2;

    public sealed override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
        Tile tile = Framing.GetTileSafely(i, j);

        int tileScale = TileSize + FramePadding;

        int xOffset = i % AtlasWidth * ChunkWidth;
        int yOffset = j % AtlasHeight * ChunkHeight;

        int newFrameX = 0;
        int newFrameY = 0;

        Tile tileAbove = Framing.GetTileSafely(i, j - 1);
        Tile tileBelow = Framing.GetTileSafely(i, j + 1);

        Tile tileLeft = Framing.GetTileSafely(i - 1, j);
        Tile tileRight = Framing.GetTileSafely(i + 1, j);

        Tile tileTopLeft = Main.tile[i - 1, j - 1];
        Tile tileTopRight = Main.tile[i + 1, j - 1];

        Tile tileBottomLeft = Main.tile[i - 1, j + 1];
        Tile tileBottomRight = Main.tile[i + 1, j + 1];

        if (tileAbove.HasTile && tileBelow.HasTile && tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 1;
            newFrameY = 1;

            if (tileTopRight.HasTile && tileBottomRight.HasTile) {
                newFrameX = 1;
                newFrameY = 4;
            }

            if (tileTopLeft.HasTile && tileBottomLeft.HasTile) {
                newFrameX = 2;
                newFrameY = 4;
            }

            if (tileTopLeft.HasTile && tileTopRight.HasTile) {
                newFrameX = 3;
                newFrameY = 2;
            }

            if (tileBottomLeft.HasTile && tileBottomRight.HasTile) {
                newFrameX = 3;
                newFrameY = 1;
            }
        }

        if (tileAbove.HasTile && tileBelow.HasTile && !tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 1;
            newFrameY = 3;
        }

        if (!tileAbove.HasTile && !tileBelow.HasTile && tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 0;
            newFrameY = 3;
        }

        if (!tileAbove.HasTile && !tileBelow.HasTile && !tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 2;
            newFrameY = 3;
        }

        if (!tileAbove.HasTile && tileBelow.HasTile && tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 1;
            newFrameY = 0;
        }

        if (tileAbove.HasTile && tileBelow.HasTile && !tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 0;
            newFrameY = 1;
        }

        if (tileAbove.HasTile && !tileBelow.HasTile && tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 1;
            newFrameY = 2;
        }

        if (tileAbove.HasTile && tileBelow.HasTile && tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 2;
            newFrameY = 1;
        }

        if (!tileAbove.HasTile && tileBelow.HasTile && !tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 3;
            newFrameY = 0;
        }

        if (tileAbove.HasTile && !tileBelow.HasTile && !tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 3;
            newFrameY = 3;
        }

        if (!tileAbove.HasTile && !tileBelow.HasTile && !tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 0;
            newFrameY = 4;
        }

        if (!tileAbove.HasTile && !tileBelow.HasTile && tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 3;
            newFrameY = 4;
        }

        if (!tileAbove.HasTile && tileBelow.HasTile && !tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 0;
            newFrameY = 0;
        }


        if (!tileAbove.HasTile && tileBelow.HasTile && tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 2;
            newFrameY = 0;
        }

        if (tileAbove.HasTile && !tileBelow.HasTile && !tileLeft.HasTile && tileRight.HasTile) {
            newFrameX = 0;
            newFrameY = 2;
        }

        if (tileAbove.HasTile && !tileBelow.HasTile && tileLeft.HasTile && !tileRight.HasTile) {
            newFrameX = 2;
            newFrameY = 2;
        }

        tile.TileFrameX = (short)(newFrameX * tileScale + xOffset);
        tile.TileFrameY = (short)(newFrameY * tileScale + yOffset);

        return false;
    }
}