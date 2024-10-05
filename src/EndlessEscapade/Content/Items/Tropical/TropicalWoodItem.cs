using EndlessEscapade.Content.Tiles.Tropical;

namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<TropicalWoodTile>());

        Item.width = 20;
        Item.height = 16;
    }
}
