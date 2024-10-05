using EndlessEscapade.Content.Tiles.Shoals;

namespace EndlessEscapade.Content.Items.Shoals;

public class CoralsandItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<CoralsandTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
