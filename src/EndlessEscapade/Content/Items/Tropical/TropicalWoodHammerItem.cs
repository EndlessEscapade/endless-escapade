namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodHammerItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DamageType = DamageClass.Melee;
        Item.damage = 8;
        Item.knockBack = 5.5f;

        Item.useTime = 18;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.width = 40;
        Item.height = 42;

        Item.hammer = 45;

        Item.UseSound = SoundID.Item1;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<TropicalWoodItem>(10)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
