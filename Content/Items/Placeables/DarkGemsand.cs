using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class DarkGemsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkGemsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
