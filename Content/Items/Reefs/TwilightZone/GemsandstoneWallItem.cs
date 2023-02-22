using EndlessEscapade.Content.Walls.Reefs.TwilightZone;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.TwilightZone;

public class GemsandstoneWallItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlacableWall((ushort)ModContent.WallType<GemsandstoneWall>());
    }
}