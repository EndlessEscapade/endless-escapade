using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Tropical;

public class TropicalWoodHammer : ModItem
{
    public override void SetDefaults() {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 8;
        Item.knockBack = 5.5f;

        Item.useTime = 18;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;
        
        Item.width = 32;
        Item.height = 32;

        Item.hammer = 45;

        Item.UseSound = SoundID.Item1;
    }

    public override void AddRecipes() {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(10);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}