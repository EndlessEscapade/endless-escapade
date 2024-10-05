using EndlessEscapade.Content.Tiles.Shallows;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsandItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LightGemsandTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
