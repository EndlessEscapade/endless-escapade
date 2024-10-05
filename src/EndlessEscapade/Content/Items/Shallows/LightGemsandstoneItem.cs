using EndlessEscapade.Content.Tiles.Shallows;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<LightGemsandstoneTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
