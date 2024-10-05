using EndlessEscapade.Content.Tiles.Thermal;

namespace EndlessEscapade.Content.Items.Thermal;

public class RustyPlatingItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<RustyPlatingTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
