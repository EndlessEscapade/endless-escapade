using EndlessEscapade.Content.Tiles.Trenches;

namespace EndlessEscapade.Content.Items.Trenches;

public class DarkGemsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<DarkGemsandstoneTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
