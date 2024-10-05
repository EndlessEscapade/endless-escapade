using EndlessEscapade.Content.Walls.Tropical;

namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodFenceItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<TropicalWoodFence>());

        Item.width = 32;
        Item.height = 32;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe(4)
            .AddIngredient<TropicalWoodItem>()
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
