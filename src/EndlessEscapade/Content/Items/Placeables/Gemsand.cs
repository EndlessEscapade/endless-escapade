using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class Gemsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Gemsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
