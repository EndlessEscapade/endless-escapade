using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Lythen;

public class LythenBar : ModItem
{
    public override void SetStaticDefaults() {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
    }
    
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Lythen.LythenBar>());
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<LythenOre>(3);
        recipe.AddTile(TileID.Furnaces);
        recipe.Register();
    }
}
