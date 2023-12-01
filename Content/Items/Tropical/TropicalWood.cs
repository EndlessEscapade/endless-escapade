using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWood : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Tropical.TropicalWood>());

        Item.width = 20;
        Item.height = 16;
    }
}
