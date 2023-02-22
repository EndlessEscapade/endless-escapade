using EndlessEscapade.Content.Tiles.Reefs.Shoals;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Shoals;

public class CoralsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<CoralsandstoneTile>());
    }
}