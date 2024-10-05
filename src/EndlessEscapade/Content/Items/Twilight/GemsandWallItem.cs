using EndlessEscapade.Content.Walls.Twilight;

namespace EndlessEscapade.Content.Items.Twilight;

public class GemsandWallItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<GemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
