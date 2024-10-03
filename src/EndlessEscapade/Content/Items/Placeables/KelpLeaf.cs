using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class KelpLeaf : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.KelpLeaf>());

        Item.width = 18;
        Item.height = 22;
    }
}
