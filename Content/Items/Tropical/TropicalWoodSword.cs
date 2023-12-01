using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodSword : ModItem
{
    public override void SetDefaults() {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 15;
        Item.knockBack = 4.5f;

        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.width = 34;
        Item.height = 34;

        Item.UseSound = SoundID.Item1;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(8);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
