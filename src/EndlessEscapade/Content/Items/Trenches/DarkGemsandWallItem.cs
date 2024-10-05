using EndlessEscapade.Content.Walls.Trenches;

namespace EndlessEscapade.Content.Items.Trenches;

public class DarkGemsandWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<DarkGemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
