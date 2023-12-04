using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.TwilightZone;

public class Gemsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightZone.Gemsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
