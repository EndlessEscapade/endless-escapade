using EndlessEscapade.Content.Tiles.Trenches;

namespace EndlessEscapade.Content.Items.Trenches;

public class DarkGemsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<DarkGemsandTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
