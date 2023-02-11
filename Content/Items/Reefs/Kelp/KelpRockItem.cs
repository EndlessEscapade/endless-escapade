using EndlessEscapade.Content.Tiles.Reefs.Kelp;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Kelp;

public class KelpRockItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<KelpRockTile>());
    }
}