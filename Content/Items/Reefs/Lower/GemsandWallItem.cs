using EndlessEscapade.Content.Walls.Reefs.Lower;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Lower;

public class GemsandWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<GemsandWall>());
    }
}