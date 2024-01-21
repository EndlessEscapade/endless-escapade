using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class RustyPlating : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.RustyPlating>());

        Item.width = 16;
        Item.height = 16;
    }
}
