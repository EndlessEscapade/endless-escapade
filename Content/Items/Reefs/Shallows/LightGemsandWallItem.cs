using EndlessEscapade.Content.Walls.Reefs.Shallows;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Shallows;

public class LightGemsandWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<LightGemsandWall>());
    }
}