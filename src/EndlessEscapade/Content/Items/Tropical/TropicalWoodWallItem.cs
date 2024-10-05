using EndlessEscapade.Content.Walls.Tropical;

namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodWallItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<TropicalWoodWall>());

        Item.width = 24;
        Item.height = 24;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe(4)
            .AddIngredient<TropicalWoodItem>()
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
