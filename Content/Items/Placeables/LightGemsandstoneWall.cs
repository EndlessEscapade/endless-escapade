using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class LightGemsandstoneWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.LightGemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
