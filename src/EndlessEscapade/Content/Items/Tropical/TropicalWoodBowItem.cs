namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodBowItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DefaultToBow(27, 7f);

        Item.width = 24;
        Item.height = 32;
    }

    public override Vector2? HoldoutOffset() {
        return new Vector2(2f, 0f);
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<TropicalWoodItem>(10)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
