using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Tropical;

public class TropicalWood : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Reefs.Tropical.TropicalWood>());
    }
}
