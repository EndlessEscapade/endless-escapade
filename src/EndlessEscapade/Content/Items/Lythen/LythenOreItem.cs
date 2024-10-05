using EndlessEscapade.Content.Tiles.Lythen;
using Terraria.DataStructures;

namespace EndlessEscapade.Content.Items.Lythen;

public class LythenOreItem : ModItem
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 10));
    }

    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LythenOreTile>());

        Item.width = 32;
        Item.height = 28;
    }
}
