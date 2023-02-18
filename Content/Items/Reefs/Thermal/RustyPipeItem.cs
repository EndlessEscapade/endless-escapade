using EndlessEscapade.Content.Tiles.Reefs.Thermal;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Thermal;

public class RustyPipeItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<RustyPipeTile>());
    }
}