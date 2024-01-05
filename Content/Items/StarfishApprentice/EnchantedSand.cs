using EndlessEscapade.Content.Items.Shoals;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.StarfishApprentice;

public class EnchantedSand : ModItem
{
    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 18;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.FallenStar);
        recipe.AddIngredient<Coralsand>(4);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
