using EndlessEscapade.Content.Tiles.Reefs.Lower;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Lower;

public class GemsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<GemsandTile>());
    }
}