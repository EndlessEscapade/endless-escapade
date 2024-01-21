using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class LightGemsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LightGemsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
