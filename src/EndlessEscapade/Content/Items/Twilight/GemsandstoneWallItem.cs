using EndlessEscapade.Content.Walls.Twilight;

namespace EndlessEscapade.Content.Items.Twilight;

public class GemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<GemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
