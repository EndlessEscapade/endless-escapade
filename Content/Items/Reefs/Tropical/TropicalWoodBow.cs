using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Tropical;

public class TropicalWoodBow : ModItem
{
    public override void SetDefaults() {
        Item.DamageType = DamageClass.Ranged;
        Item.DefaultToBow(27, 7f);
    }

    public override Vector2? HoldoutOffset() {
        return new Vector2(2f, 0f);
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(10);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}