using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsandstone : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Shallows.LightGemsandstone>());

        Item.width = 16;
        Item.height = 16;
    }
}
