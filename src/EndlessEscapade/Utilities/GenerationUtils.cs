namespace EndlessEscapade.Utilities;

/// <summary>
///     Provides world generation utilities.
/// </summary>
public static class GenerationUtils
{
    /// <summary>
    ///     Extends a tile downwards until it reaches a solid tile.
    /// </summary>
    /// <param name="x">The horizontal position of the origin in tile coordinates.</param>
    /// <param name="y">The vertical position of the origin in tile coordinates.</param>
    public static void ExtendDownwards(int x, int y) {
        var tile = Framing.GetTileSafely(x, y);

        if (!tile.HasTile) {
            return;
        }

        y++;

        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, tile.TileType, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }
}
