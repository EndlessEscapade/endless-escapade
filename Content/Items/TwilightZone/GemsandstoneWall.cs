using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.TwilightZone;

public class GemsandstoneWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.TwilightZone.GemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
