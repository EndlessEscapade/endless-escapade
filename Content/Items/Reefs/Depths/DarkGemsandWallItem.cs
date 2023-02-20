using EndlessEscapade.Content.Walls.Reefs.Depths;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Depths;

public class DarkGemsandWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<DarkGemsandWall>());
    }
}