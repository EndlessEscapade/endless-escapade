using EndlessEscapade.Content.Walls.Shallows;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsandWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<LightGemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
