using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.DarkestTrenches;

public class DarkGemsandWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.DarkestTrenches.DarkGemsandWall>());
    }
}