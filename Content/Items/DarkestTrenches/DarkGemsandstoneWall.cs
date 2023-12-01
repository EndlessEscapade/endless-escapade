using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.DarkestTrenches;

public class DarkGemsandstoneWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.Reefs.DarkestTrenches.DarkGemsandstoneWall>());

        Item.width = 32;
        Item.height = 32;
    }
}
