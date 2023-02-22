using EndlessEscapade.Content.Walls.Reefs.Shallows;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Shallows;

public class LightGemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<LightGemsandstoneWall>());
    }
}