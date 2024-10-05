using EndlessEscapade.Content.Tiles.Shoals;

namespace EndlessEscapade.Content.Items.Shoals;

public class CoralsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<CoralsandstoneTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
