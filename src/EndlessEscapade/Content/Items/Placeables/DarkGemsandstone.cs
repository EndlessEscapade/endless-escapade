using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class DarkGemsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkGemsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
