using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Kelp;

public class KelpRock : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Kelp.KelpRock>());

        Item.width = 20;
        Item.height = 20;
    }
}
