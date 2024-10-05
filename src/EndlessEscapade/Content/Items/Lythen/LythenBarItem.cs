using EndlessEscapade.Content.Tiles.Lythen;
using Terraria.DataStructures;

namespace EndlessEscapade.Content.Items.Lythen;

public class LythenBarItem : ModItem
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
    }

    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LythenBarTile>());

        Item.width = 30;
        Item.height = 24;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<LythenOreItem>(3)
            .AddTile(TileID.Furnaces)
            .Register();
    }
}
