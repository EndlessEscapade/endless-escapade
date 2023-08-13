using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Tropical;

[AutoloadEquip(EquipType.Legs)]
public class TropicalWoodBoots : ModItem
{
    public override void SetDefaults() {
        Item.defense = 2;

        Item.width = 16;
        Item.height = 16;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(25);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
