using EndlessEscapade.Content.Tiles.Reefs.Surface;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Surface;

public class CoralsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<CoralsandTile>());
    }
}