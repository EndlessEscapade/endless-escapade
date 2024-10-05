using EndlessEscapade.Content.Tiles.Thermal;

namespace EndlessEscapade.Content.Items.Thermal;

public class RustyPipeItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<RustyPipeTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
