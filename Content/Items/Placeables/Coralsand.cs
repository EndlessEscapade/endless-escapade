using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class Coralsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Coralsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
