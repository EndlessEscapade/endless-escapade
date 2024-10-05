namespace EndlessEscapade.Content.Items.Tropical;

[AutoloadEquip(EquipType.Legs)]
public class TropicalWoodBootsItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.defense = 2;

        Item.width = 18;
        Item.height = 12;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<TropicalWoodItem>(25)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
