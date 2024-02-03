using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class KelpRock : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.KelpRock>());

        Item.width = 20;
        Item.height = 20;
    }
}
