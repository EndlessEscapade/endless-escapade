using EndlessEscapade.Content.Tiles.Kelp;

namespace EndlessEscapade.Content.Items.Kelp;

public class KelpRockItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<KelpRockTile>());

        Item.width = 20;
        Item.height = 20;
    }
}
