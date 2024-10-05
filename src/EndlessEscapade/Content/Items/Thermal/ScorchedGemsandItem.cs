using EndlessEscapade.Content.Tiles.Thermal;

namespace EndlessEscapade.Content.Items.Thermal;

public class ScorchedGemsandItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<ScorchedGemsandTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
