using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.TwilightZone;

public class Gemsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightZone.Gemsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
