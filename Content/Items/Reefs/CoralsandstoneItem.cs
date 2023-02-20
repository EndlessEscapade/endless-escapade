using EndlessEscapade.Content.Tiles.Reefs;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs;

public class CoralsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<CoralsandstoneTile>());
    }
}