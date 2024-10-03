using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class LightGemsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LightGemsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
