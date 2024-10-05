namespace EndlessEscapade.Utilities;

// TODO: This class should not exist. World generation utilities should be built from GenAction and GenShape.
public static class WorldGenUtils
{
    public static void ExtendDownwards(int x, int y, int type) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, type, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }
}
