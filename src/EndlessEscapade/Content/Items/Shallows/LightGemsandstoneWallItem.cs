using EndlessEscapade.Content.Walls.Shallows;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<LightGemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
