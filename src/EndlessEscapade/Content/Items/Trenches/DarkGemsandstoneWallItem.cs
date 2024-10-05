using EndlessEscapade.Content.Walls.Trenches;

namespace EndlessEscapade.Content.Items.Trenches;

public class DarkGemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<DarkGemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
