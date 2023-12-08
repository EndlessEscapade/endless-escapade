using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.DarkestTrenches;

public class DarkGemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.DarkestTrenches.DarkGemsandWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
