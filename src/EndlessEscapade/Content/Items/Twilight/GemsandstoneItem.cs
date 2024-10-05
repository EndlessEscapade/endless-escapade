using EndlessEscapade.Content.Tiles.Twilight;

namespace EndlessEscapade.Content.Items.Twilight;

public class GemsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<GemsandstoneTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
