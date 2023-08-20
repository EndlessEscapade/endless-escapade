using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWood : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<EndlessEscapade.Content.Tiles.Tropical.TropicalWood>());
    }
}
