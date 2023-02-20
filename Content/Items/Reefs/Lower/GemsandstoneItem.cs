using EndlessEscapade.Content.Tiles.Reefs.Lower;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Lower;

public class GemsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<GemsandstoneTile>());
    }
}