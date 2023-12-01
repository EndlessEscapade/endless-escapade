using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Kelp;

public class KelpLeaf : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Kelp.KelpLeaf>());

        Item.width = 18;
        Item.height = 22;
    }
}
