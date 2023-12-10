using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Base;

public abstract class CompositeTile : ModTile
{
    public const int ChunkWidth = 72;
    public const int ChunkHeight = 90;

    public const int TileSize = 16;
    public const int TilePadding = 2;

    public abstract int HorizontalSheetCount { get; }
    public abstract int VerticalSheetCount { get; }

    public sealed override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
        var tileScale = TileSize + TilePadding;

        var xOffset = i % HorizontalSheetCount * ChunkWidth;
        var yOffset = j % VerticalSheetCount * ChunkHeight;

        var newFrameX = 0;
        var newFrameY = 0;

        var tile = Framing.GetTileSafely(i, j);

        var tileAbove = Framing.GetTileSafely(i, j - 1);
        var tileBelow = Framing.GetTileSafely(i, j + 1);

        var tileLeft = Framing.GetTileSafely(i - 1, j);
        var tileRight = Framing.GetTileSafely(i + 1, j);

        var tileTopLeft = Framing.GetTileSafely(i - 1, j - 1);
        var tileTopRight = Framing.GetTileSafely(i + 1, j - 1);

        var tileBottomLeft = Framing.GetTileSafely(i - 1, j + 1);
        var tileBottomRight = Framing.GetTileSafely(i + 1, j + 1);

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
