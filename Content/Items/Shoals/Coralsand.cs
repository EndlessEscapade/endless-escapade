using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shoals;

public class Coralsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Shoals.Coralsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
