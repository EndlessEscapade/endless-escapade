using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Shallows;

public class LightGemsand : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Shallows.LightGemsand>());

        Item.width = 16;
        Item.height = 16;
    }
}
