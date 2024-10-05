using EndlessEscapade.Content.Tiles.Twilight;

namespace EndlessEscapade.Content.Items.Twilight;

public class GemsandItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<GemsandTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
