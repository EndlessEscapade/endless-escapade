using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shoals;

public class Coralsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Shoals.Coralsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
