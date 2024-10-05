namespace EndlessEscapade.Content.Items.Lythen;

[AutoloadEquip(EquipType.Head)]
public class StormKnightCrestItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.width = 26;
        Item.height = 28;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<LythenBarItem>(3)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
