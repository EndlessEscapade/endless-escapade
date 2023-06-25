using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.TwilightZone;

public class GemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.TwilightZone.GemsandWall>());
    }
}