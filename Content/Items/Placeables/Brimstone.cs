using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class Brimstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Brimstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
