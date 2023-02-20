using EndlessEscapade.Content.Tiles.Reefs.Depths;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs;

public class DarkGemsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<DarkGemsandTile>());
    }
}