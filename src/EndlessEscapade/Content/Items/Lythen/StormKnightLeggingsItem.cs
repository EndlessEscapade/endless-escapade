namespace EndlessEscapade.Content.Items.Lythen;

[AutoloadEquip(EquipType.Legs)]
public class StormKnightLeggingsItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.width = 22;
        Item.height = 14;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<LythenBarItem>(4)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
