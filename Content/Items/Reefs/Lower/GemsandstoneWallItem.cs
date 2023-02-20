using EndlessEscapade.Content.Walls.Reefs.Lower;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Lower;

public class GemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<GemsandstoneWall>());
    }
}