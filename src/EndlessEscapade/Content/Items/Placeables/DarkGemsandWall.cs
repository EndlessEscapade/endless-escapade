using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class DarkGemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.DarkGemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
