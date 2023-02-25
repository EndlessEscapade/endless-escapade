using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Shoals;

public class Coralsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Reefs.Shoals.Coralsandstone>());
    }
}