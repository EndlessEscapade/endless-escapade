using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.TwilightZone;

public class GemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.TwilightZone.GemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
