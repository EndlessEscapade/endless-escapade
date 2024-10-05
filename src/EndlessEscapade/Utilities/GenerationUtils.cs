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

        var count = 0;

        while (WorldGen.InWorld(x, y + count) && !WorldGen.SolidTile(x, y + count)) {
            WorldGen.PlaceTile(x, y + count, tile.TileType, true, true);
            WorldGen.SlopeTile(x, y + count);

            count++;
        }
    }
}
