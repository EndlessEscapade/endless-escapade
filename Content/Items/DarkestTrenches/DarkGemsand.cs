using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.DarkestTrenches;

public class DarkGemsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkestTrenches.DarkGemsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
