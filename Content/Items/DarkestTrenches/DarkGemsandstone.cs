using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.DarkestTrenches;

public class DarkGemsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkestTrenches.DarkGemsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
