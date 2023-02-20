using EndlessEscapade.Content.Walls.Reefs.Upper;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs;

public class LightGemsandWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<LightGemsandWall>());
    }
}