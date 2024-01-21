using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class Coralsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Coralsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
