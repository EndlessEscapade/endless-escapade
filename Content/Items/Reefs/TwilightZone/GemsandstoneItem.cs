using EndlessEscapade.Content.Tiles.Reefs.TwilightZone;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.TwilightZone;

public class GemsandstoneItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<GemsandstoneTile>());
    }
}