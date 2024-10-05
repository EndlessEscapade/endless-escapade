using EndlessEscapade.Content.Tiles.Thermal;

namespace EndlessEscapade.Content.Items.Thermal;

public class BrimstoneItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<BrimstoneTile>());

        Item.width = 16;
        Item.height = 16;
    }
}
