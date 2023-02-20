using EndlessEscapade.Content.Tiles.Reefs.Upper;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs;

public class LightGemsandItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<LightGemsandTile>());
    }
}