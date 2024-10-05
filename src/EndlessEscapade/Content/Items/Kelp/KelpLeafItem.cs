using EndlessEscapade.Content.Tiles.Kelp;

namespace EndlessEscapade.Content.Items.Kelp;

public class KelpLeafItem : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<KelpLeafTile>());

        Item.width = 18;
        Item.height = 22;
    }
}
