using EndlessEscapade.Content.Tiles.Reefs.Upper;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Upper;

public class LightGemsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<LightGemsandstoneTile>());
    }
}