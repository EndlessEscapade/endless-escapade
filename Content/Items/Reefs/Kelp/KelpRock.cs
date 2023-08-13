using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Kelp;

public class KelpRock : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Reefs.Kelp.KelpRock>());
    }
}
