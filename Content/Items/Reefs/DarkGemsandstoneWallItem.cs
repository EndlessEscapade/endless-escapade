using EndlessEscapade.Content.Walls.Reefs.Depths;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs;

public class DarkGemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<DarkGemsandstoneWall>());
    }
}