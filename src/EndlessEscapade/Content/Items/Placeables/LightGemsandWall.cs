using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class LightGemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.LightGemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
