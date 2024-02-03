using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class TropicalWood : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TropicalWood>());

        Item.width = 20;
        Item.height = 16;
    }
}
