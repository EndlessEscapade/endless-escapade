using EndlessEscapade.Content.Tiles.Reefs.DarkestTrenches;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.DarkestTrenches;

public class DarkGemsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<DarkGemsandTile>());
    }
}