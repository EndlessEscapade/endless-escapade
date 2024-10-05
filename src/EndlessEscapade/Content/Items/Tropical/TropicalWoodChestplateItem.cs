namespace EndlessEscapade.Content.Items.Tropical;

[AutoloadEquip(EquipType.Body)]
public class TropicalWoodChestplateItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.defense = 3;

        Item.width = 24;
        Item.height = 22;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<TropicalWoodItem>(30)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
