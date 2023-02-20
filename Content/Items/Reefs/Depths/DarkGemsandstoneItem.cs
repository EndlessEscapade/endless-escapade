using EndlessEscapade.Content.Tiles.Reefs.Depths;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Depths;

public class DarkGemsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<DarkGemsandstoneTile>());
    }
}