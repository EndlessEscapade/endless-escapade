using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class Gemsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Gemsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
