using Terraria.ModLoader;

namespace EEMod.Extensions
{
    public static class RecipeExtensions
    {
        public static void AddIngredient<T>(this ModRecipe recipe, int stack = 1) where T : ModItem => recipe.AddIngredient(ModContent.ItemType<T>(), stack);

        public static void AddTile<T>(this ModRecipe recipe) where T : ModTile => recipe.AddTile(ModContent.TileType<T>());
    }
}