using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Materials;

public class Aquamarine : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Aquamarine>());

        Item.width = 22;
        Item.height = 30;
    }
}
