using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class RustyPipe : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.RustyPipe>());

        Item.width = 16;
        Item.height = 16;
    }
}
