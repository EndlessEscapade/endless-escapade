using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class ScorchedGemsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.ScorchedGemsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
