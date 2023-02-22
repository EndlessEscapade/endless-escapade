using EndlessEscapade.Content.Tiles.Reefs.Shallows;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Shallows;

public class LightGemsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<LightGemsandTile>());
    }
}