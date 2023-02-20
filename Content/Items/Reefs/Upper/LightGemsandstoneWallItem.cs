using EndlessEscapade.Content.Walls.Reefs.Upper;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Upper;

public class LightGemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<LightGemsandstoneWall>());
    }
}